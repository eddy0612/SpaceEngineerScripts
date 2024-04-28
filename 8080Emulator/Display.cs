//-----------------------------------------------------------------------------
// Using code initially from 
//    https://github.com/BluestormDNA/i8080-Space-Invaders
// Then modified/optimized in order to meet the requirements for Space Engineers
//-----------------------------------------------------------------------------
using System;

namespace IngameScript
{
    partial class Program
    {
        public class Display
        {
            public enum paletteType { MONO, RGB, RBG, CUSTOM };

            // Game specific variables
            public static bool isRed;
            public static bool backgroundDisable;
            public static bool backgroundSelect;
            public int rotate = 270;
            public static byte backGroundCol = 0;  // gbr - 3 bits, prob palette index
            public static bool m_color_map = false;
            private bool useOptimizations = false;
            public paletteType palType;
            public static bool oldIsRed = false;

            // General variables
            public const int WIDTH = 224;
            public const int HEIGHT = 256;
            private const ushort videoRamStart = 0x2400;
            private const ushort videoRamEnd = 0x4000;
            private const ushort MW8080BW_VCOUNTER_START_NO_VBLANK = 0x20;

            private Memory memory;
            private Specifics specifics;
            private Arcade8080Machine am = null;

            public Display(Memory memory, Specifics specs, Arcade8080Machine mach)
            {
                this.memory = memory;
                this.specifics = specs;
                this.am = mach;

                // Clear statics:
                isRed = false;
                backgroundDisable = false;
                backgroundSelect = false;
                backGroundCol = 0;
                m_color_map = false;
                oldIsRed = false;

                // Should we use optimizations - can only do so if the fore colour is based from a
                // read only memory (B&W automatically uses optimizations)
                if (Memory.game == GetRomData.Games.schaser) {
                    useOptimizations = false;
                } else {
                    useOptimizations = true;
                }
            }

            // Repeat state information (when coming in mid-display generation)
            public DirectBitmap Screen = null;
            int screenPosn;
            int[] prevMem = null;
            int[] curMem = null;
            int[] curCol = null;
            int[] prevCol = null;
            int[] curCol2 = null;
            int[] prevCol2 = null;

            public bool generateFrameToDisplay(ref int State)
            {
                // State 2 - Being called to generate the frame from scratch
                if (State == 2) {
                    Screen = new DirectBitmap(256, 224, specifics, palType);
                    screenPosn = 0;

                    // Clone the memory for optimzations later
                    curMem = new int[(videoRamEnd - videoRamStart) / sizeof(int)];
                    Buffer.BlockCopy(memory.allProms[0], videoRamStart, curMem, 0, (videoRamEnd - videoRamStart));

                    if (Memory.game == GetRomData.Games.schaser) {
                        int blockSize = 0x1C00;
                        curCol = new int[blockSize / sizeof(int)];
                        Buffer.BlockCopy(memory.allProms[0], 0xc400, curCol, 0, blockSize);
                    }
                    if (Memory.game == GetRomData.Games.rollingc) {
                        int blockSize = 0x1C00;
                        curCol = new int[blockSize / sizeof(int)];
                        Buffer.BlockCopy(memory.allProms[0], 0xa400, curCol, 0, blockSize);
                        curCol2 = new int[blockSize / sizeof(int)];
                        Buffer.BlockCopy(memory.allProms[0], 0xe400, curCol2, 0, blockSize);
                    }

                    State = 3;
                    if (am.showStates) specifics.Echo("Moved to state " + State);
                    screenPosn = 0;
                    if (memory.allProms[1] == null) useOptimizations = true;
                }

                byte[] mem = memory.allProms[0];
                byte[] col = memory.allProms[1];
                byte[] extramem = memory.allProms[2];

                // Stage 3 - loop through the screen. Pick up where left off means all variables
                // relied on between iterations needs to be globals
                for (; screenPosn < ((videoRamEnd - videoRamStart) / sizeof(int)); screenPosn++) {
                    if (specifics.GetInstructionCount() >= Specifics.maxFrames) return false;

                    int y = (screenPosn * sizeof(int)) / 32;

                    bool redrawthisbyte = false;

                    // If we are using optimizations, we only need to do anything if 4 bytes havent changed
                    // Note if the colourmap is in memory, this isnt potentially true but I havent found anything
                    // that does it yet!
                    if (prevMem == null) {
                        redrawthisbyte = true;
                    } else if (oldIsRed != isRed) {
                        redrawthisbyte = true;
                    } else if (Memory.game == GetRomData.Games.rollingc) {
                        int coloffs = ((((y >> 2) << 7) | ((screenPosn * sizeof(int)) & 0x1f))) / sizeof(int);
                        if ((curCol[coloffs] != prevCol[coloffs]) ||
                            (curCol2[coloffs] != prevCol2[coloffs]) ||
                            (curMem[screenPosn] != prevMem[screenPosn])) {
                            redrawthisbyte = true;
                        }

                    } else if (Memory.game == GetRomData.Games.schaser) {
                        int foreOffs = ((((y >> 2) << 7) | ((screenPosn * sizeof(int)) & 0x1f))) / sizeof(int);
                        if ((curCol[foreOffs] != prevCol[foreOffs]) ||
                            (curMem[screenPosn] != prevMem[screenPosn])) {
                            redrawthisbyte = true;
                        }
                    } else if (useOptimizations && (curMem[screenPosn] != prevMem[screenPosn])) {
                        redrawthisbyte = true;
                    }

                    if (redrawthisbyte) { 

                        // We now have 4 bytes to process
                        for (int j = 0; j < sizeof(int); j++) {

                            // We have a clever optimization for black and white
                            if (palType == paletteType.MONO) {
                                Screen.Set8PixelsBW((((sizeof(int) * screenPosn) + j) % 32) * 8, y, mem[((sizeof(int) * screenPosn) + j) + videoRamStart]);
                                continue;
                            } else {
                                // But for colour we really need to work this out bit by bit
                                int screenByte = (screenPosn * sizeof(int)) + j;
                                byte data = mem[screenByte + videoRamStart];

                                // One colour is set for 8 bits
                                int colIdx = (screenByte >> 8 << 5) | (screenByte & 0x1f);

                                byte foreColour;
                                byte backColour;

                                byte bgr;  // bgr == colour eprom memory address (might be fore or back)
                                if (col == null) {
                                    bgr = 0xff;
                                } else {
                                    // No idea why - couldnt work it out but this hack works, otherwise white stripe
                                    //bgr = col[0x80 + colIdx];
                                    int color_map_base = m_color_map ? 0x0480 : 0x0080;
                                    bgr = col[color_map_base + colIdx];
                                }

                                if (Memory.game == GetRomData.Games.rollingc) {
                                    // Colour is in RAM not compressed, but only per 8 roww
                                    // Drawing ignores m_color_map
                                    foreColour = (byte)(mem[0xa400 + (screenByte & 0x1f00) | (screenByte & 0x1f)] & 0x0f);
                                    backColour = (byte)(mem[0xe400 + (screenByte & 0x1f00) | (screenByte & 0x1f)] & 0x0f);
                                } else if (Memory.game == GetRomData.Games.vortex) {
                                    bool coldata = (mem[((screenByte+1) & 0x1fff) + videoRamStart] & 0x01) > 0;
                                    //foreColour = (byte)((coldata ? 0x00 : 0x04) | (coldata ? 0x02 : 0x00) | (((8 * (screenByte % 32)) & 0x20) > 0 ? 0x01 : 0x00));
                                    foreColour = (byte)((coldata ? 0x00 : 0x01) | (coldata ? 0x02 : 0x00) | (((8 * (screenByte % 32)) & 0x20) > 0 ? 0x04 : 0x00));
                                    backColour = 0;  // Assume 0 == Black
                                } else if (Memory.game == GetRomData.Games.schaser) {
                                    foreColour = (byte)(mem[0xc400 + (((y >> 2) << 7) | (screenByte & 0x1f))] & 0x07);
                                    if (Display.backgroundDisable) {
                                        backColour = 0;  // Assume 0 == Black
                                    } else {
                                        backColour = (byte)(((((bgr & 0x0c) == 0x0c) && Display.backgroundSelect)) ? 4 : 2);
                                    }
                                } else if (Memory.game == GetRomData.Games.polaris) {
                                    foreColour = (byte)(~((mem[0xc400 + (((y >> 2) << 7) | (screenByte & 0x1f))]) & 0x07));
                                    backColour = (byte)((((bgr & 0x01) == 0x01)) ? 6 : 2);
                                } else {
                                    backColour = backGroundCol;
                                    foreColour = (byte)(bgr & 0x07);
                                }
                                Screen.Set8PixelsCol(((screenByte % 32) * 8), y, foreColour, backColour, data, isRed);
                            } // Colour
                        } // Byte per int
                    } // If changed
                } // Per byte
                prevMem = curMem;
                prevCol = curCol;
                prevCol2 = curCol2;
                oldIsRed = isRed;
                Screen.FinishScreen(rotate);
                return true;
            }
        }
    }
}
