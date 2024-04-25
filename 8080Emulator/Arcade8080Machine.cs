//-----------------------------------------------------------------------------
// Using code initially from 
//    https://github.com/BluestormDNA/i8080-Space-Invaders
// Then modified/stripped/optimized in order to meet the requirements for Space
// Engineers and support for various other games added
//-----------------------------------------------------------------------------
using System;

namespace IngameScript
{
    partial class Program
    {
        public class Arcade8080Machine
        {
            Cpu cpu;
            Memory memory;
            Display display;
            Bus iobus;
            public byte[] keyBits;
            public Specifics specifics;
            bool needsProcessing = false;
            int processIndex = 0;
            public int cost = 0;

            public Arcade8080Machine(Specifics specs, GetRomData.Games gameType, String gameData, int cost)
            {
                specifics = specs;
                specs.am = this;

                int rotate270 = 270;
                memory = new Memory();
                byte backCol = 0;
                Display.paletteType palType = Display.paletteType.RBG;

                byte port_shift_result = 0xFF; 
                byte port_shift_data = 0xFF;
                byte port_shift_offset = 0xFF; 
                byte port_input = 0xFF;
                this.cost = cost;

                memory.LoadRom(ref keyBits, ref rotate270, gameType, gameData, ref backCol, ref needsProcessing, ref palType, ref port_shift_result, ref port_shift_data,
                                ref port_shift_offset, ref port_input);

                iobus = new Bus(keyBits, port_shift_result, port_shift_data, port_shift_offset, port_input);
                cpu = new Cpu(memory, iobus);

                display = new Display(memory, specifics);
                display.palType = palType;
                display.rotate = rotate270;
                Display.backGroundCol = backCol;
                specifics.display = display;
                DirectBitmap.Palette = null;
                State = -3;
            }

            int State = -3; // ( -3 == loaded, -2 == midfixup, -1 == begin, 0 == cpupart1, 1 == cpupart2, 2 == drawscreen )

            public void part_exe(ref int curFrames, ref int actFrames)
            {
                specifics.Echo("Called with state " + State);

                // Do we need to process the rom?
                if (State == -3) {
                    if (needsProcessing) {
                        processIndex = 0;
                        State = -2;
                    } else {
                        State = -1;
                    }
                }

                if (State == -2) {
                    if (GetRomData.processRom(ref processIndex, ref memory.allProms, specifics, Memory.game)) {
                        cpu.memory = memory.allProms[0];
                        State = -1;
                    } else {
                        return; // More processing to do - out of CPU time
                    }
                }

                // Always process the keys - this is really quick anyway
                specifics.CheckKeys();

                bool seenBegin = false;

                // Now continue the loop until we run out of cycles OR cycle to finish the screen twice
                while (true) {
                    if (State >= 2) {
                        // Drawing the screen can take up to 11000 cycles
                        if (specifics.GetInstructionCount() < Specifics.maxFrames) {
                            bool finished = specifics.drawAndRenderFrame(ref State, ref actFrames);
                            if (finished) {
                                curFrames++;
                                State = -1;
                                specifics.Echo("Moved to state " + State + " - " + specifics.GetInstructionCount());
                            } else {
                                return;
                            }
                        } else {
                            return;
                        }
                    }

                    /* Throttle at 60fps support */
                    if (State == -1) {
                        if (seenBegin) return; // Throttle at 1 frame per tick max
                        seenBegin = true;
                        State = 0;
                        specifics.Echo("Moved to state " + State + " - " + specifics.GetInstructionCount());
                    }

                    // Get here, its cpu time
                    while ((cpu.cycles < 16666) && (specifics.GetInstructionCount() < Specifics.maxFrames)) {
                        cpu.exe();
                    }

                    // Now either interrupt or return
                    if (specifics.GetInstructionCount() >= Specifics.maxFrames) return;

                    // Its interrupt time
                    if (cpu.cycles >= 16666) {
                        cpu.cycles = 0;
                        if (State == 0) {
                            cpu.handleInterrupt(1);
                            State = 1;
                            specifics.Echo("Moved to state " + State + " - " + specifics.GetInstructionCount());
                        } else {
                            cpu.handleInterrupt(2);
                            State = 2;
                            specifics.Echo("Moved to state " + State + " - " + specifics.GetInstructionCount());
                        }
                    }
                }


            }

            public void insertCoin(bool pushed)
            {
                byte whichBit = keyBits[(int)Program.GetRomData.KeyIndex.q];
                specifics.DebugAndEcho("Coin inserted: " + whichBit + "/" + pushed + " at " + DateTime.Now);

                if ((keyBits[(int)Program.GetRomData.KeyIndex.initmask] & whichBit) > 0) {

                    if (!pushed) {
                        iobus.input |= whichBit;
                    } else {
                        iobus.input &= (byte)~whichBit;
                    }

                    // ACTIVE_HIGH
                } else {
                    if (pushed) {
                        iobus.input |= whichBit;
                    } else {
                        iobus.input &= (byte)~whichBit;
                    }
                }

            }

            public void handleInput(byte b, Boolean pushed)
            {
                // ACTIVE_LOW:
                if ((keyBits[(int)Program.GetRomData.KeyIndex.initmask] & b) > 0)
                {

                    if (!pushed) {
                        iobus.input |= b;
                    } else {
                        iobus.input &= (byte)~b;
                    }

                // ACTIVE_HIGH
                } else {
                    if (pushed) {
                        iobus.input |= b;
                    } else {
                        iobus.input &= (byte)~b;
                    }
                }
            }

        }
    }
}
