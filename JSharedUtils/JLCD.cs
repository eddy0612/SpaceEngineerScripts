using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.Game.GUI.TextPanel;
using VRageMath;

namespace IngameScript
{
    partial class Program
    {
        public class JLCD
        {
            public MyGridProgram mypgm = null;
            public JDBG jdbg = null;
            bool suppressDebug = false;

            // Useful for lookup via colour
            public static Dictionary<String, char> solidcolor = new Dictionary<String, char>
            {
                { "YELLOW", '' },
                { "RED", '' },
                { "ORANGE", '' },
                { "GREEN", '' },
                { "CYAN", '' },
                { "PURPLE", ''},
                { "BLUE", '' },
                { "WHITE", ''},
                { "BLACK", ''},
                { "GREY", '' }
            };

            // Useful for direct code
            public static char COLOUR_YELLOW = '';
            public static char COLOUR_RED = '';
            public static char COLOUR_ORANGE = '';
            public static char COLOUR_GREEN = '';
            public static char COLOUR_CYAN = '';
            public static char COLOUR_PURPLE = '';
            public static char COLOUR_BLUE = '';
            public static char COLOUR_WHITE = '';
            public static char COLOUR_BLACK = '';
            public static char COLOUR_GREY = '';

            public JLCD(MyGridProgram pgm, JDBG dbg, bool suppressDebug)
            {
                this.mypgm = pgm;
                this.jdbg = dbg;
                this.suppressDebug = suppressDebug; /* Suppress the 'Writing to...' msg */
            }

            // ---------------------------------------------------------------------------
            // Get a list of the LCDs with a specific tag
            // ---------------------------------------------------------------------------
            public List<IMyTerminalBlock> GetLCDsWithTag(String tag)
            {
                List<IMyTerminalBlock> allLCDs = new List<IMyTerminalBlock>();
                mypgm.GridTerminalSystem.GetBlocksOfType(allLCDs, (IMyTerminalBlock x) => (
                                                                                       (x.CustomName != null) &&
                                                                                       (x.CustomName.ToUpper().IndexOf("[" + tag.ToUpper() + "]") >= 0) &&
                                                                                       (x is IMyTextSurfaceProvider)
                                                                                      ));
                jdbg.Debug("Found " + allLCDs.Count + " lcds to update with tag " + tag);
                return allLCDs;
            }

            // ---------------------------------------------------------------------------
            // Get a list of the LCDs with a specific name
            // ---------------------------------------------------------------------------
            public List<IMyTerminalBlock> GetLCDsWithName(String tag)
            {
                List<IMyTerminalBlock> allLCDs = new List<IMyTerminalBlock>();
                mypgm.GridTerminalSystem.GetBlocksOfType(allLCDs, (IMyTerminalBlock x) => (
                                                                                       (x.CustomName != null) &&
                                                                                       (x.CustomName.ToUpper().IndexOf(tag.ToUpper()) >= 0) &&
                                                                                       (x is IMyTextSurfaceProvider)
                                                                                      ));
                jdbg.Debug("Found " + allLCDs.Count + " lcds to update with tag " + tag);
                return allLCDs;
            }

            // ---------------------------------------------------------------------------
            // Initialize LCDs
            // ---------------------------------------------------------------------------
            public void InitializeLCDs(List<IMyTerminalBlock> allLCDs, TextAlignment align)
            {
                foreach (var thisLCD in allLCDs)
                {
                    jdbg.Debug("Setting up the font for " + thisLCD.CustomName);

                    // Set it to text / monospace, black background
                    // Also set the padding 
                    IMyTextSurface thisSurface = ((IMyTextSurfaceProvider)thisLCD).GetSurface(0);
                    if (thisSurface == null) continue;
                    thisSurface.Font = "Monospace";
                    thisSurface.ContentType = ContentType.TEXT_AND_IMAGE;
                    thisSurface.BackgroundColor = Color.Black;
                    thisSurface.Alignment = align;
                }
            }

            public void SetLCDFontColour(List<IMyTerminalBlock> allLCDs, Color colour)
            {
                foreach (var thisLCD in allLCDs) {
                    if (thisLCD is IMyTextPanel) {
                        jdbg.Debug("Setting up the color for " + thisLCD.CustomName);
                        ((IMyTextPanel)thisLCD).FontColor = colour;
                    }
                }
            }

            public void SetLCDRotation(List<IMyTerminalBlock> allLCDs, float Rotation)
            {
                foreach (var thisLCD in allLCDs) {
                    if (thisLCD is IMyTextPanel) {
                        jdbg.Debug("Setting up the rotation for " + thisLCD.CustomName);
                        thisLCD.SetValueFloat("Rotate", Rotation);
                    }
                }
            }

            // ---------------------------------------------------------------------------
            // Set the font and display properties of each display UNLESS [LOCKED]
            // ---------------------------------------------------------------------------
            public void SetupFont(List<IMyTerminalBlock> allLCDs, int rows, int cols, bool mostlySpecial)
            {
                SetupFontCalc(allLCDs, rows, cols, mostlySpecial, 0.05F, 0.05F);
            }
            public void SetupFontCustom(List<IMyTerminalBlock> allLCDs, int rows, int cols, bool mostlySpecial, float size, float incr)
            {
                SetupFontCalc(allLCDs, rows, cols, mostlySpecial, size,incr);
            }

            public void SetupFontCalc(List<IMyTerminalBlock> allLCDs, int rows, int cols, bool mostlySpecial, float startSize, float startIncr)
            {
                foreach (var thisLCD in allLCDs)
                {
                    jdbg.Debug("Setting up font on screen: " + thisLCD.CustomName + " (" + rows + " x " + cols + ")");
                    IMyTextSurface thisSurface = ((IMyTextSurfaceProvider)thisLCD).GetSurface(0);
                    if (thisSurface == null) continue;

                    // Now work out the font size
                    float size = startSize;
                    float incr = startIncr;

                    // Get a list of cols strings
                    StringBuilder teststr = new StringBuilder("".PadRight(cols, (mostlySpecial? solidcolor["BLACK"] : 'W')));
                    Vector2 actualScreenSize = thisSurface.TextureSize;
                    while (true)
                    {
                        thisSurface.FontSize = size;
                        Vector2 actualSize = thisSurface.TextureSize;

                        Vector2 thisSize = thisSurface.MeasureStringInPixels(teststr, thisSurface.Font, size);
                        //jdbg.Debug("..with size " + size + " width is " + thisSize.X + " max " + actualSize.X);
                        //jdbg.Debug("..with size " + size + " height is " + thisSize.Y + " max " + actualSize.Y);

                        int displayrows = (int)Math.Floor(actualScreenSize.Y / thisSize.Y);

                        if ((thisSize.X < actualSize.X) && (displayrows > rows))
                        {
                            size += incr;
                        }
                        else
                        {
                            break;
                        }
                    }
                    thisSurface.FontSize = size - incr;
                    jdbg.Debug("Calc size of " + thisSurface.FontSize);

                    // BUG? Corner LCDs are a factor of 4 out - no idea why but try *4
                    if (thisLCD.DefinitionDisplayNameText.Contains("Corner LCD")) {
                        jdbg.Debug("INFO: Avoiding bug, multiplying by 4: " + thisLCD.DefinitionDisplayNameText);
                        thisSurface.FontSize *= 4;
                    }
                }
            }

            // ---------------------------------------------------------------------------
            // Update the programmable block with the script name
            // ---------------------------------------------------------------------------
            public void UpdateFullScreen(IMyTerminalBlock block, String text)
            {
                List<IMyTerminalBlock> lcds = new List<IMyTerminalBlock> { block };
                InitializeLCDs(lcds, TextAlignment.CENTER);
                SetupFont(lcds, 1, text.Length + 4, false);
                WriteToAllLCDs(lcds, text, false);
            }

            // ---------------------------------------------------------------------------
            // Write a message to all related LCDs
            // ---------------------------------------------------------------------------
            public void WriteToAllLCDs(List<IMyTerminalBlock> allLCDs, String msg, bool append)
            {
                //if (allLCDs == null) {
                    //jdbg.Debug("WARNING: No screen to write to");
                    //return;
                //}
                foreach (var thisLCD in allLCDs)
                {
                    if (!this.suppressDebug) jdbg.Debug("Writing to display " + thisLCD.CustomName);
                    IMyTextSurface thisSurface = ((IMyTextSurfaceProvider)thisLCD).GetSurface(0);
                    if (thisSurface == null) continue;
                    thisSurface.WriteText(msg, append);
                }
            }

            // ---------------------------------------------------------------------------
            // Convert r g b to a character - Credit 
            //    to https://github.com/Whiplash141/Whips-Image-Converter
            // ---------------------------------------------------------------------------
            public char ColorToChar(byte r, byte g, byte b)
            {
                const double bitSpacing = 255.0 / 7.0;
                return (char)(0xe100 + ((int)Math.Round(r / bitSpacing) << 6) + ((int)Math.Round(g / bitSpacing) << 3) + (int)Math.Round(b / bitSpacing));
            }

        }
    }
}
