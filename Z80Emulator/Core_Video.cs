using EmptyKeys.UserInterface.Generated.DataTemplatesContracts_Bindings;
using System;
using System.Collections.Generic;
using System.Drawing;
//using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
//using System.Windows.Forms;
using VRageMath;

namespace IngameScript
{
    partial class Program
    {
        class Core_Video
        {
        }

        public partial class Core
        {
            private static readonly byte BRIGHT = 0xd7, NORM = 0xff;

            private byte videoBorderColor;
            private bool videoFlashInvert;
            private int videoFlashTimer;

            private char[] pal = new char[16];

            public readonly char[] videoScreenDataLCD;
            private int lastBorderColour = -1;
            public readonly char[][] borderlines;


            private Core(Core_Video coreVideo) : this(new Core_AudioIn())
            {
                videoBorderColor = 8;
                videoFlashInvert = false;
                //videoScreenData = new char[312 * 416];

                videoScreenDataLCD = new char[312 * (416 + 2)];
                Array.Clear(videoScreenDataLCD, 0, videoScreenDataLCD.Length);
                for (int i = 1; i < 312; i++) {
                    videoScreenDataLCD[(i * 418) + 416] = '\x0d';
                    videoScreenDataLCD[(i * 418) + 417] = '\x0a';
                }

                videoFlashTimer = 30;
                pal[0] = ColorToChar(0, 0, 0);
                pal[1] = ColorToChar(0, 0, NORM);
                pal[2] = ColorToChar(NORM, 0, 0);
                pal[3] = ColorToChar(NORM, 0, NORM);
                pal[4] = ColorToChar(0, NORM, 0);
                pal[5] = ColorToChar(0, NORM, NORM);
                pal[6] = ColorToChar(NORM, NORM, 0);
                pal[7] = ColorToChar(NORM, NORM, NORM);
                pal[8] = ColorToChar(0, 0, 0);
                pal[9] = ColorToChar(0, 0, BRIGHT);
                pal[10] = ColorToChar(BRIGHT, 0, 0);
                pal[11] = ColorToChar(BRIGHT, 0, BRIGHT);
                pal[12] = ColorToChar(0, BRIGHT, 0);
                pal[13] = ColorToChar(0, BRIGHT, BRIGHT);
                pal[14] = ColorToChar(BRIGHT, BRIGHT, 0);
                pal[15] = ColorToChar(BRIGHT, BRIGHT, BRIGHT);

                borderlines = new char[16][];
                for (int i = 0; i < 16; i++) {
                    borderlines[i] = new char[416];
                    for (int j=0; j<416; j++) {
                        borderlines[i][j] = pal[i];
                    }
                }
            }

            public void BuildVideoScreen()
            {
                /* Flash every 30 frames */
                if (videoFlashTimer-- == 0)
                {
                    videoFlashInvert = !videoFlashInvert;
                    videoFlashTimer = 30;
                }

                /* Now work out screen string */
                // TODO: Needs to be 312
                for (int i = 0; i < 120; i++)
                {
                    //mypgm.Echo("Drawing line " + i);
                    Video_DrawLine(i);
                }
                lastBorderColour = videoBorderColor;
                mypgm.Echo("Drawn");
            }

            private void Video_DrawLine(int lineNo)
            {
                if (lineNo < 8) return;  /* initialized to 0, ignored after */

                int lineStart = lineNo * 418;
                if (lineNo < 64 || lineNo >= 256) // top and bottom border
                {
                    // Only need to do something if border has changed colour
                    if (videoBorderColor != lastBorderColour) {
                        Array.Copy(borderlines[videoBorderColor], 0, videoScreenDataLCD, lineStart, 416);
                        //Fill(videoScreenDataLCD, pal[videoBorderColor], lineStart, 416);
                    }
                    return;
                }

                if (videoBorderColor != lastBorderColour) {
                    Array.Copy(borderlines[videoBorderColor], 0, videoScreenDataLCD, lineStart, 80);
                    Array.Copy(borderlines[videoBorderColor], 0, videoScreenDataLCD, lineStart+336, 80);
                    //Fill(videoScreenDataLCD, pal[videoBorderColor], lineStart, 80); // left border
                    //Fill(videoScreenDataLCD, pal[videoBorderColor], lineStart + 336, 80); // right border
                }
                lineStart += 80;
                lineNo -= 64;
                int charY = 0x5800 + ((lineNo >> 3) << 5);
                int lineAddr = ((lineNo & 0x07) << 8) | ((lineNo & 0x38) << 2) | ((lineNo & 0xC0) << 5) | 0x4000;
                for (int charX = 0; charX < 32; charX++)
                {
                    byte att = cpuRam[charY + charX];
                    int ink = att & 0x07;
                    int paper = (att & 0x38) >> 3;
                    if ((att & 0x40) != 0)
                    {
                        ink += 8;
                        paper += 8;
                    }

                    bool flash = (att & 0x80) != 0;
                    bool doFlash = flash && videoFlashInvert;
                    byte byt = cpuRam[lineAddr++];
                    for (int bit = 128; bit > 0; bit >>= 1)
                    {
                        if (doFlash)
                            videoScreenDataLCD[lineStart++] = pal[3];//pal[((byt & bit) != 0 ? paper : ink)];
                        else
                            videoScreenDataLCD[lineStart++] = pal[3];//pal[((byt & bit) != 0 ? ink : paper)];
                    }
                }
            }


            private void Fill(char[] array, char with, int start, int len)
            {
                int end = start + len;
                while (start < end) {
                    array[start++] = with;
                }
            }

            public char ColorToChar(byte r, byte g, byte b)
            {
                const double bitSpacing = 255.0 / 7.0;
                return (char)(0xe100 + ((int)Math.Round(r / bitSpacing) << 6) + ((int)Math.Round(g / bitSpacing) << 3) +
                              (int)Math.Round(b / bitSpacing));
            }
        }
    }
}