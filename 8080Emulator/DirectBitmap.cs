using Sandbox.Game.Screens.Helpers;
using System;
using VRageMath;

namespace IngameScript
{
    partial class Program
    {
        public class DirectBitmap
        {
            // Cache screen so only need to redraw changes
            public static char[] Pixels;
            // Optimization to do 8 bits at a time
            public static char[][] QuickPix;
            // Only 0-8 used in supported games so far as brightness used in
            // rollingc 
            public static char[] Palette;
            private static bool lastRollingCGame;

            private Specifics specifics;


            public int Height { get; private set; }
            public int Width { get; private set; }
            public int Memory_Width { get; private set; }   // Pixels + CR + LF


            public DirectBitmap(int width, int height, Specifics spec, Display.paletteType pal)
            {
                specifics = spec;
                Width = width;
                Memory_Width = width + 2;
                Height = height;  //TODO: We had +4 at one time, why?

                /* We only want to do this ONCE for the class - Hack via Static */
                if (Pixels == null) {
                    Pixels = new char[Memory_Width * Height];

                    // Optimization - Add CR/LFs once for always
                    for (int i = 0; i < Height; i++) {
                        Pixels[(i * Memory_Width) + Width] = '\x0d';
                        Pixels[(i * Memory_Width) + (Width + 1)] = '\x0a';
                    }
                }

                if (Palette == null || Bus.rollingc_saved != lastRollingCGame) {
                    Palette = new char[16];
                    lastRollingCGame = Bus.rollingc_saved;
                    specifics.mypgm.Echo("Building pallette again");

                    // Initialize Palette array 
                    if (Memory.game == GetRomData.Games.rollingc) {

                        for (int i = 0; i < 16; i++) {
                            int intensity = 128;
                            if (Bus.rollingc_saved) intensity = 255;
                            if (i > 7) intensity = 255;

                            // From MAME:
                            // but according to photos, pen 6 is clearly orange instead of dark-yellow, and pen 5 is less dark as well
                            // pens 1, 2 and 4 are good though. Maybe we're missing a color prom?
                            if (!Bus.rollingc_saved && i == 5) {
                                Palette[i] = specifics.mypgm.jlcd.ColorToChar(0xff, 0x00, 0x80);
                            } else if (!Bus.rollingc_saved && i == 6) {
                                Palette[i] = specifics.mypgm.jlcd.ColorToChar(0xff, 0x80, 0x00);
                            } else {
                                Palette[i] = specifics.mypgm.jlcd.ColorToChar(((i & 4) > 0) ? (byte)intensity : (byte)0,
                                                                     ((i & 2) > 0) ? (byte)intensity : (byte)0,
                                                                     ((i & 1) > 0) ? (byte)intensity : (byte)0);
                            }
                        }
                    } else {
                        for (int i = 0; i < 8; i++) {

                            if (pal == Display.paletteType.RBG) {
                                Palette[i] = specifics.mypgm.jlcd.ColorToChar(((i & 1) > 0) ? (byte)255 : (byte)0,
                                                                     ((i & 4) > 0) ? (byte)255 : (byte)0,
                                                                     ((i & 2) > 0) ? (byte)255 : (byte)0);
                            } else if (pal == Display.paletteType.RGB) {
                                // rgb
                                Palette[i] = specifics.mypgm.jlcd.ColorToChar(((i & 1) > 0) ? (byte)255 : (byte)0,
                                                                     ((i & 2) > 0) ? (byte)255 : (byte)0,
                                                                     ((i & 4) > 0) ? (byte)255 : (byte)0);
                            } else if (pal == Display.paletteType.MONO) {
                                if (i == 0) {
                                    Palette[i] = specifics.mypgm.jlcd.ColorToChar(0x00, 0x00, 0x00);
                                } else {
                                    Palette[i] = specifics.mypgm.jlcd.ColorToChar(0xff, 0xff, 0xff);
                                }
                            }
                        }
                    }

                    // Build an array of byte -> 8 chars to speed up
                    // when drawing screen in black and white mode
                    // When in colour, we need to go slower
                    QuickPix = new char[265][];
                    for (int i = 0; i < 256; i++) {
                        QuickPix[i] = new char[8];
                        for (byte b = 0; b < 8; b++) {
                            if ((i & (0x1 << b)) != 0) {
                                QuickPix[i][b] = JLCD.COLOUR_WHITE;
                            } else {
                                QuickPix[i][b] = JLCD.COLOUR_BLACK;
                            }
                        }
                    }
                
                }
            }

            public void Set8PixelsBW(int x, int y, byte b)
            {
                Array.Copy(QuickPix[b], 0, Pixels, (y * Memory_Width) + (x), 8);
            }

            public void Set8PixelsCol(int x, int y, byte fore, byte back, byte data, bool isRed)
            {
                //try {
                int index = x + (y * Memory_Width);
                char whichCol;
                for (int b = 0; b < 8; b++, index++) {
                    if ((data & 0x1) != 0) {
                        if (isRed) {
                            whichCol = JLCD.COLOUR_RED;
                        } else {
                            if (Memory.game == GetRomData.Games.rollingc) {
                                whichCol = Palette[fore & 0x0f];
                            } else {
                                whichCol = Palette[fore & 0x07];
                            }
                        }

                    } else {
                        whichCol = Palette[back];
                    }
                    Pixels[index] = whichCol;
                    data = (byte)(data >> 1);
                }
                //} catch (Exception e) {
                //    specifics.mypgm.Echo("Exception: x:" + x + ", y:" + y + ", fore:" + fore + ", back: " + back + ", data:" + data + ", isRed:" + isRed);
                //    throw e;
                //}
            }

            public void SetPixel(int x, int y, byte fore, byte back, byte data, int bit, bool isRed)
             {
                int index = x + (y * Memory_Width);
                char whichCol;

                if ((data & (0x1 << bit)) != 0) {
                    if (isRed) {
                        whichCol = JLCD.COLOUR_RED;
                    } else {
                        whichCol = Palette[fore];
                    }
                } else {
                    whichCol = Palette[back];
                }
                Pixels[index] = whichCol;
            }

            static int oldRotate = -1;
            public void FinishScreen(int rotate)
            {
                if (rotate !=  oldRotate) {
                    specifics.setRotation(rotate);
                    oldRotate = rotate;
                }
            }


        }
    }
}