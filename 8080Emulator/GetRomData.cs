using System;

// KNOWN BUGS
//   - Balloon Bomber - balloons have stripe
//   - Balloon Bomber - 2nd logo redraw in demo is dashed
//   - Balloon Bomber - crashes/screen corruption/no ship if left running too long in demo mode
//   - rollingc - vertical white should be green

namespace IngameScript
{
    partial class Program
    {
        public class GetRomData
        {
            public enum Games
            {
                None,
                ballbomb,
                invaders,
                lrescue,
                spacerng,
                vortex,
                invrvnge,
                galxwars,
                rollingc,
                schaser,
                lupin3,
                polaris,
                indianbt,
                astropal,
                galactic,
                attackfc,
            };

            // unzip -p invaders.zip | base64 --wrap=0 | clip
            //         => Go to block, change block custom data to that string, block name to invaders.zip

            public static String getRomData(Games game)
            {
                return "";
            }

            private static String start_ballbomb = "AAAAwxgAAA";
            private static String start_invaders = "IMPJFiGEIH";
            private static String start_lrescue  = "Dw8PDw8PDw";
            private static String start_spacerng = "CAgICAgICA";
            private static String start_vortex   = "wicZKqd9Ks";
            private static String start_invrvnge = "//////////";
            private static String start_galxwars = "CAgICAgICA";
            private static String start_rollingc = "88MAQNQYAA";
            private static String start_schaser  = "Dw8PDw8PDw";
            private static String start_lupin3   = "AAAAw0AAAA";
            private static String start_polaris  = "AAAAw8YFAA";
            private static String start_indianbt = "AAAAw+VfKw";
            private static String start_astropal = "MaAglwDDtg";
            private static String start_galactic = "AAAAw5sA//";
            private static String start_attackfc = "PgAyKyLDAA";

            public static bool checkRomData(Games game, String gameData)
            {
                String expected = "";
                switch (game) {
                    case Games.ballbomb: expected = start_ballbomb; break;
                    case Games.invaders: expected = start_invaders; break;
                    case Games.lrescue: expected = start_lrescue; break;
                    case Games.spacerng: expected = start_spacerng; break;
                    case Games.vortex: expected = start_vortex; break;
                    case Games.invrvnge: expected = start_invrvnge; break;
                    case Games.galxwars: expected = start_galxwars; break;
                    case Games.rollingc: expected = start_rollingc; break;
                    case Games.schaser: expected = start_schaser; break;
                    case Games.lupin3: expected = start_lupin3; break;
                    case Games.polaris: expected = start_polaris; break;
                    case Games.indianbt: expected = start_indianbt; break;
                    case Games.astropal: expected = start_astropal; break;
                    case Games.galactic: expected = start_galactic; break;
                    case Games.attackfc: expected = start_attackfc; break;
                    default:
                        throw new Exception("Unexpected game: " + game);
                }
                return (expected.Equals(gameData.Substring(0, 10)));
            }


            public static byte[][] getRomData(Games gameType, String gameBytes, ref byte[] keyports, ref int rotate, 
                                              ref byte backgroundCol, ref bool needsProcessing,
                                              ref Display.paletteType palType,
                                              ref byte port_shift_result, ref byte port_shift_data, 
                                              ref byte port_shift_offset, ref byte port_input)
            {
                byte[][] roms = new byte[3][];
                byte[] rom = Convert.FromBase64String(gameBytes);
                int curPos = 0;

                byte[] main = new byte[0x10000];
                byte[] colour = new byte[2048];
                byte[] extra = new byte[2048];
                rotate = 270;
                backgroundCol = 0;

                // Each rom is dumped differently :-(
                // See MAME source 8080bw.cpp for maps
                //    but the calls are based on order in zip
                if (gameType == Games.invaders) {
                    loadrom(rom, ref curPos, ref main, 0x1800, 0x0800);
                    loadrom(rom, ref curPos, ref main, 0x1000, 0x0800);
                    loadrom(rom, ref curPos, ref main, 0x0800, 0x0800);
                    loadrom(rom, ref curPos, ref main, 0x0000, 0x0800);
                    roms[0] = main;
                    palType = Display.paletteType.MONO;
                    port_shift_data = 0x04;
                    port_shift_offset = 0x02;
                    port_input = 0x01; 
                    port_shift_result = 0x03;
                    // OK

                } else if (gameType == Games.ballbomb) {
                    loadrom(rom, ref curPos, ref main, 0x0000, 0x0800 * 4);
                    loadrom(rom, ref curPos, ref main, 0x4000, 0x0800);
                    loadrom(rom, ref curPos, ref colour, 0x0000, 0x0800);
                    roms[0] = main;
                    roms[1] = colour;
                    backgroundCol = 2;
                    palType = Display.paletteType.RBG;
                    port_shift_data = 0x04;
                    port_shift_offset = 0x02;
                    port_input = 0x01; 
                    port_shift_result = 0x03;
                    // OK (Crashes if too long in demo?)

                } else if (gameType == Games.lrescue) {
                    loadrom(rom, ref curPos, ref colour, 0x0000, 0x0400);
                    curPos -= 0x400;
                    loadrom(rom, ref curPos, ref colour, 0x0400, 0x0400);
                    loadrom(rom, ref curPos, ref main, 0x0000, 0x0800 * 4);
                    loadrom(rom, ref curPos, ref main, 0x4000, 0x0800 * 2);
                    roms[0] = main;
                    roms[1] = colour;
                    palType = Display.paletteType.RBG;
                    port_shift_data = 0x04;
                    port_shift_offset = 0x02;
                    port_input = 0x01; 
                    port_shift_result = 0x03;
                    // OK

                } else if (gameType == Games.schaser) {
                    loadrom(rom, ref curPos, ref colour, 0x0000, 0x0400);
                    loadrom(rom, ref curPos, ref main, 0x0000, 0x0400 * 8);
                    loadrom(rom, ref curPos, ref main, 0x4000, 0x0400 * 2);
                    roms[0] = main;
                    roms[1] = colour;
                    palType = Display.paletteType.RBG;
                    port_shift_data = 0x04;
                    port_shift_offset = 0x02;
                    port_input = 0x01;
                    port_shift_result = 0x03;
                    // OK

                } else if (gameType == Games.invrvnge) {
                    loadrom(rom, ref curPos, ref colour, 0x0000, 0x0800);
                    loadrom(rom, ref curPos, ref main, 0x1800, 0x0800);
                    loadrom(rom, ref curPos, ref main, 0x1000, 0x0800);
                    loadrom(rom, ref curPos, ref main, 0x0800, 0x0800);
                    loadrom(rom, ref curPos, ref main, 0x0000, 0x0800);
                    roms[0] = main;
                    roms[1] = colour;
                    palType = Display.paletteType.RBG;
                    port_shift_data = 0x04;
                    port_shift_offset = 0x02;
                    port_input = 0x01;
                    port_shift_result = 0x03;
                    // OK 

                } else if (gameType == Games.attackfc) {
                    loadrom(rom, ref curPos, ref main, 0x0000, 0x0400);
                    loadrom(rom, ref curPos, ref main, 0x0800, 0x0400);
                    loadrom(rom, ref curPos, ref main, 0x1000, 0x0400);
                    loadrom(rom, ref curPos, ref main, 0x1800, 0x0400);
                    loadrom(rom, ref curPos, ref main, 0x0400, 0x0400);
                    loadrom(rom, ref curPos, ref main, 0x0c00, 0x0400);
                    loadrom(rom, ref curPos, ref main, 0x1c00, 0x0400);
                    rotate = 0;
                    roms[0] = main;
                    needsProcessing = true;
                    palType = Display.paletteType.MONO;
                    port_shift_data = 0x03;
                    port_shift_offset = 0x07;
                    port_input = 0x00;
                    port_shift_result = 0x03;
                    // OK

                } else if (gameType == Games.spacerng) {
                    loadrom(rom, ref curPos, ref colour, 0x0000, 0x0400 * 2);
                    loadrom(rom, ref curPos, ref main, 0x0000, 0x0800 * 4);
                    roms[0] = main;
                    roms[1] = colour;
                    rotate = 90;
                    palType = Display.paletteType.RBG;
                    port_shift_data = 0x04;
                    port_shift_offset = 0x02;
                    port_input = 0x01;
                    port_shift_result = 0x03;
                    // OK

                } else if (gameType == Games.indianbt) {
                    loadrom(rom, ref curPos, ref main, 0x0000, 0x0800 * 4);
                    loadrom(rom, ref curPos, ref main, 0x4000, 0x0800 * 4);
                    loadrom(rom, ref curPos, ref colour, 0x0000, 0x0400 * 2);
                    roms[0] = main;
                    roms[1] = colour;
                    palType = Display.paletteType.RGB;
                    port_shift_data = 0x04;
                    port_shift_offset = 0x02;
                    port_input = 0x01;
                    port_shift_result = 0x03;
                    // OK

                } else if (gameType == Games.astropal) {
                    loadrom(rom, ref curPos, ref main, 0x0000, 0x0400 * 8);
                    roms[0] = main;
                    rotate = 0;
                    palType = Display.paletteType.MONO;
                    port_shift_data = 0x04;
                    port_shift_offset = 0x02;
                    port_input = 0x01;
                    port_shift_result = 0x03;
                    // OK

                } else if (gameType == Games.galxwars) {
                    loadrom(rom, ref curPos, ref colour, 0x0000, 0x0400 * 2);
                    loadrom(rom, ref curPos, ref main, 0x4000, 0x0400 * 2);
                    loadrom(rom, ref curPos, ref main, 0x0000, 0x0400 * 4);
                    roms[0] = main;
                    roms[1] = colour;
                    palType = Display.paletteType.RBG;
                    port_shift_data = 0x04;
                    port_shift_offset = 0x02;
                    port_input = 0x01;
                    port_shift_result = 0x03;
                    // OK

                } else if (gameType == Games.lupin3) {
                    loadrom(rom, ref curPos, ref main, 0x0000, 0x0800 * 4);
                    loadrom(rom, ref curPos, ref main, 0x4000, 0x0800 * 2);
                    loadrom(rom, ref curPos, ref colour, 0x0000, 0x0400 * 2);
                    roms[0] = main;
                    roms[1] = colour;
                    palType = Display.paletteType.RGB;
                    port_shift_data = 0x04;
                    port_shift_offset = 0x02;
                    port_input = 0x01;
                    port_shift_result = 0x03;
                    // OK

                } else if (gameType == Games.galactic) {
                    loadrom(rom, ref curPos, ref main, 0x0000, 0x0800 * 4);
                    loadrom(rom, ref curPos, ref main, 0x4000, 0x0800 * 3);
                    roms[0] = main;
                    palType = Display.paletteType.MONO;
                    port_shift_data = 0x04;
                    port_shift_offset = 0x02;
                    port_input = 0x01;
                    port_shift_result = 0x03;
                    // OK

                } else if (gameType == Games.rollingc) {
                    loadrom(rom, ref curPos, ref main, 0x0000, 0x0400 * 8);
                    loadrom(rom, ref curPos, ref main, 0x4000, 0x0800 * 4);
                    roms[0] = main;
                    palType = Display.paletteType.CUSTOM;
                    port_shift_data = 0x04;
                    port_shift_offset = 0x02;
                    port_input = 0x01;
                    port_shift_result = 0x03;
                    // OK 

                } else if (gameType == Games.polaris) {
                    // mame not happy
                    loadrom(rom, ref curPos, ref main, 0x0000, 0x0800);
                    loadrom(rom, ref curPos, ref main, 0x1000, 0x0800);
                    loadrom(rom, ref curPos, ref main, 0x1800, 0x0800);
                    loadrom(rom, ref curPos, ref main, 0x4000, 0x0800);

                    loadrom(rom, ref curPos, ref extra, 0x0000, 0x0100);
                    loadrom(rom, ref curPos, ref colour, 0x0000, 0x0400);

                    loadrom(rom, ref curPos, ref main, 0x0800, 0x0800);
                    loadrom(rom, ref curPos, ref main, 0x4800, 0x0800);
                    loadrom(rom, ref curPos, ref main, 0x5000, 0x0800);

                    roms[0] = main;
                    roms[1] = colour;
                    roms[2] = extra;
                    palType = Display.paletteType.RBG;
                    port_shift_data = 0x03;
                    port_shift_offset = 0x00;
                    port_input = 0x01;
                    port_shift_result = 0x03;
                    // OK

                } else if (gameType == Games.vortex) {
                    loadrom(rom, ref curPos, ref main, 0x0000, 0x0800 * 4);
                    loadrom(rom, ref curPos, ref main, 0x4000, 0x0800 * 2);
                    needsProcessing = true;
                    roms[0] = main;
                    palType = Display.paletteType.RGB;

                    port_shift_data = 0x06;
                    port_shift_offset = 0x00;
                    port_input = 0x03;
                    port_shift_result = 0x01;
                    // OK

                } else {
                    return null;
                }
                keyports = getKeyBits(gameType);
                return roms;
            }

            private static byte[] newmain;
            public static bool processRom(ref int index, ref byte[][] roms, Specifics specifics, Games gameType)
            {
                if (gameType == Games.attackfc) {
                    /* from mame source: swap a8/a9 */
                    if (index == 0) {
                        newmain = new byte[roms[0].Length];
                    }

                    int[] reorder = new int[] { 15, 14, 13, 12, 11, 10, 8, 9, 7, 6, 5, 4, 3, 2, 1, 0 };
                    for (; index < (Math.Min(roms[0].Length, 0xffff)); index++) {
                        if (specifics.GetInstructionCount() >= Specifics.maxFrames) return false;
                        int newval = ReorderAsShort(index, reorder);
                        newmain[newval] = roms[0][index];
                    }
                    roms[0] = newmain;
                    // ok
                    return true;
                } else if (gameType == Games.vortex) {
                    /* Copied from MAME source */
                    if (index == 0) {
                        newmain = new byte[roms[0].Length];
                    }
                    for (; index < roms[0].Length; index++) {
                        if (specifics.GetInstructionCount() >= Specifics.maxFrames) return false;
                        int addr = index;
                        switch (index & 0xE000) {
                            case 0x0000:
                            case 0x2000:
                                addr ^= 0x0209;
                                break;
                            case 0x4000:
                                addr ^= 0x0209;
                                break;
                            case 0x6000:
                            case 0x8000:
                            case 0xa000:
                            case 0xc000:
                            case 0xe000:
                                addr ^= 0x0208;
                                break;
                        }
                        newmain[addr] = roms[0][index];
                    }
                    roms[0] = newmain;
                    return true;
                }
                throw new Exception("Unexpected game: " + gameType);
            }

            // Reorder array bit15,bit14,... 15 is left most bit
            public static int ReorderAsShort(int a,int []bits)
            {
                int b = 0;
                for (int i = 0; i < 16; i++) {
                    if ((a & (0x01 << i)) > 0) {
                        b |= (0x01 << bits[15-i]);
                    }
                }
                return b;
            }
            private static void loadrom(byte[] rom, ref int curPos, ref byte[] mem, int Start, int Len)
            {
                Array.Copy(rom, curPos, mem, Start, Len);
                curPos += Len;
            }
            public enum KeyIndex
            {
                keyup = 1,
                keydown = 2,
                keyleft = 3,
                keyright = 4,
                space = 5,
                crouch = 6,
                q = 7,
                e = 8,
                initmask = 9
            };

            private static byte[] getKeyBits(Games gameType)
            {
                byte[] indexes = new byte[16];  // Leave space for arrows, insert etc

                switch (gameType) {
                    case Games.astropal:
                    case Games.indianbt:
                        indexes[(int)KeyIndex.q] = 0x01;  // coin in - ACTIVE LOW
                        indexes[(int)KeyIndex.e] = 0x04;  // 1p start
                        indexes[(int)KeyIndex.crouch] = 0x08;
                        indexes[(int)KeyIndex.space] = 0x10;
                        indexes[(int)KeyIndex.keyleft] = 0x20;
                        indexes[(int)KeyIndex.keyright] = 0x40;
                        indexes[(int)KeyIndex.initmask] = 0x81;
                        break;
                    case Games.schaser:
                        indexes[(int)KeyIndex.keyup] = 0x01;
                        indexes[(int)KeyIndex.keyleft] = 0x02;
                        indexes[(int)KeyIndex.keydown] = 0x04;
                        indexes[(int)KeyIndex.keyright] = 0x08;
                        indexes[(int)KeyIndex.space] = 0x10;
                        indexes[(int)KeyIndex.e] = 0x40;  // 1p start
                        indexes[(int)KeyIndex.q] = 0x80;  // coin in 
                        break;
                    case Games.lupin3:
                        indexes[(int)KeyIndex.q] = 0x01;  // coin in 
                        indexes[(int)KeyIndex.e] = 0x04;  // 1p start
                        indexes[(int)KeyIndex.space] = 0x08;
                        indexes[(int)KeyIndex.keyright] = 0x10;
                        indexes[(int)KeyIndex.keydown] = 0x20;
                        indexes[(int)KeyIndex.keyleft] = 0x40;
                        indexes[(int)KeyIndex.keyup] = 0x80;
                        break;
                    case Games.attackfc:
                        indexes[(int)KeyIndex.keyright] = 0x01;
                        indexes[(int)KeyIndex.keyleft] = 0x02;
                        indexes[(int)KeyIndex.space] = 0x04; // not used
                        indexes[(int)KeyIndex.q] = 0x80;  // coin in 
                        indexes[(int)KeyIndex.initmask] = 0xFF;
                        break;
                    case Games.galxwars:
                        indexes[(int)KeyIndex.q] = 0x01;  // coin in 
                        indexes[(int)KeyIndex.e] = 0x04;  // 1p start
                        indexes[(int)KeyIndex.space] = 0x10;
                        indexes[(int)KeyIndex.keyleft] = 0x20;
                        indexes[(int)KeyIndex.keyright] = 0x40;
                        indexes[(int)KeyIndex.initmask] = 0x89;
                        break;
                    case Games.polaris:
                        indexes[(int)KeyIndex.q] = 0x01;  // coin in 
                        indexes[(int)KeyIndex.e] = 0x04;  // 1p start
                        indexes[(int)KeyIndex.space] = 0x08;
                        indexes[(int)KeyIndex.keyright] = 0x10;
                        indexes[(int)KeyIndex.keydown] = 0x20;
                        indexes[(int)KeyIndex.keyleft] = 0x40;
                        indexes[(int)KeyIndex.keyup] = 0x80;
                        indexes[(int)KeyIndex.initmask] = 0x01;
                        break;
                    case Games.rollingc:
                        indexes[(int)KeyIndex.q] = 0x01;  // coin in 
                        indexes[(int)KeyIndex.e] = 0x04;  // 1p start
                        indexes[(int)KeyIndex.space] = 0x10;
                        indexes[(int)KeyIndex.crouch] = 0x80;
                        indexes[(int)KeyIndex.keyleft] = 0x20;
                        indexes[(int)KeyIndex.keyright] = 0x40;
                        break;
                    case Games.galactic:
                        indexes[(int)KeyIndex.q] = 0x01;  // coin in 
                        indexes[(int)KeyIndex.e] = 0x04;  // 1p start
                        indexes[(int)KeyIndex.space] = 0x10;
                        indexes[(int)KeyIndex.keyleft] = 0x20;
                        indexes[(int)KeyIndex.keyright] = 0x40;
                        indexes[(int)KeyIndex.initmask] = 0x01;
                        break;
                    case Games.vortex:
                        indexes[(int)KeyIndex.q] = 0x01;  // coin in 
                        indexes[(int)KeyIndex.e] = 0x04;  // 1p start
                        indexes[(int)KeyIndex.space] = 0x10;
                        indexes[(int)KeyIndex.keyleft] = 0x20;
                        indexes[(int)KeyIndex.keyright] = 0x40;
                        indexes[(int)KeyIndex.initmask] = 0x81;
                        break;
                    default:  /* Invader keys: */
                        indexes[(int)KeyIndex.q] = 0x01;  // coin in 
                        indexes[(int)KeyIndex.e] = 0x04;  // 1p start
                        indexes[(int)KeyIndex.space] = 0x10;
                        indexes[(int)KeyIndex.keyleft] = 0x20;
                        indexes[(int)KeyIndex.keyright] = 0x40;
                        break;
                }
                return indexes;
            }
        }
    }


}
