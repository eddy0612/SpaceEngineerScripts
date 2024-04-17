using Sandbox.Game.EntityComponents;
using Sandbox.Game.Screens.Helpers;
using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;
using SpaceEngineers.Game.ModAPI.Ingame;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using VRage;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.GUI.TextPanel;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ModAPI.Ingame.Utilities;
using VRage.Game.ObjectBuilders.Definitions;
using VRageMath;

namespace IngameScript
{
    partial class Program : MyGridProgram
    {

        // ----------------------------- CUT -------------------------------------
        String thisScript = "StatusLCDs";

        // My configuration
        // Example custom data in programming block:

        // Development or user config time flags
        bool debug = false;
        bool stayRunning = true;

        // Data read from program config
        int refreshSpeed = 15;         // Only once every 15 seconds

        // Private variables
        private JDBG jdbg = null;
        private JINV jinv = null;
        private JLCD jlcd = null;
        private String alertTag = "alert";    // TODO: Could move into config

        // Internals
        DateTime lastCheck = new DateTime(0);
        String mytag = "Status";

        static Dictionary<String, String> oreToText = new Dictionary<String, String>()
        {
            { "U", "Uranium"      },
            { "AG", "Silver"      },
            { "SI", "Silicon"     },
            { "PT", "Platinum"    },
            { "NI", "Nickel"      },
            { "MG", "Magnesium"   },
            { "FE", "Iron"        },
            { "AU", "Gold"        },
            { "CO", "Cobalt"      },
            { "XX", "Overflow"    },
        };

        public Program()
        {
            jdbg = new JDBG(this, debug);
            jlcd = new JLCD(this, jdbg, false);
            jinv = new JINV(jdbg);

            // ---------------------------------------------------------------------------
            // Get my custom data and parse to get the config
            // ---------------------------------------------------------------------------
            MyIni _ini = new MyIni();
            MyIniParseResult result;
            if (!_ini.TryParse(Me.CustomData, out result))
                throw new Exception(result.ToString());

            // Get the value of the "LCD" key under the "config" section.
            String tag = _ini.Get("config", "tag").ToString();
            if (tag != null) {
                Echo("Using tag of '[" + tag + ":...]'");
                mytag = tag.ToUpper();
            } else {
                Echo("Error: No LCD configured\nPlease add [config] for LCD=<substring>");
                return;
            }

            // ---------------------------------------------------------------------------
            // Run every 100 ticks, but relies on internal check to only actually
            // perform on a defined frequency
            // ---------------------------------------------------------------------------
            if (stayRunning) {
                Runtime.UpdateFrequency = UpdateFrequency.Update100;
            }
        }

        public void Save()
        {
        }

        public void Main(string argument, UpdateType updateSource)
        {
            // ---------------------------------------------------------------------------
            // Decide whether to actually do anything
            // ---------------------------------------------------------------------------
            if (stayRunning) {
                TimeSpan timeSinceLastCheck = DateTime.UtcNow - lastCheck;
                if (timeSinceLastCheck.TotalSeconds >= refreshSpeed) {
                    lastCheck = DateTime.UtcNow;
                } else {
                    return;
                }
            }

            try {
                // ---------------------------------------------------------------------------
                // We only get here if we are refreshing
                // ---------------------------------------------------------------------------                
                jdbg.ClearDebugLCDs();  // Clear the debug screens
                jdbg.DebugAndEcho("Main from " + thisScript + " running..." + DateTime.UtcNow.ToString());

                // ---------------------------------------------------------------------------
                // Enumerate all refineries + LCDs
                // ---------------------------------------------------------------------------
                List<IMyTerminalBlock> refineries = new List<IMyTerminalBlock>();
                GridTerminalSystem.GetBlocksOfType(refineries, (IMyTerminalBlock x) => (
                                                                                     (x.HasInventory) &&
                                                                                     (x.CubeGrid == Me.CubeGrid) &&
                                                                                     (x.CustomName.ToUpper().Contains("[" + mytag + ":")) &&
                                                                                     (x is IMyRefinery)
                                                                                      ));
                jdbg.Debug("Found " + refineries.Count + " blocks with inventories");

                int totalRows = 0;
                int longestRow = 30;
                String fullScreen = "";

                foreach (IMyTerminalBlock refinery in refineries) {
                    // Green    - >90% 
                    // Yellow   - <90%
                    // Red      - Empty

                    String mytagtolocate = "[" + mytag.ToUpper() + ":";
                    String mytagIngot = refinery.CustomName.ToUpper().Substring(
                                        refinery.CustomName.ToUpper().IndexOf(mytagtolocate) + mytagtolocate.Length);
                    mytagIngot = mytagIngot.Substring(0, mytagIngot.IndexOf("]"));

                    // Work out the status colour
                    IMyInventory ores = ((IMyRefinery)refinery).InputInventory;
                    char StatusChar = JLCD.COLOUR_PURPLE;
                    Color StatusColour = Color.Purple;

                    float pctFull = (((float)(ores.CurrentVolume * 100.0F)) / ((float)(ores.MaxVolume)));
                    if (pctFull > 90.0F) {
                        StatusChar = JLCD.COLOUR_GREEN;
                        StatusColour = Color.Green;
                    } else if (pctFull > 0.1F) {
                        StatusChar = JLCD.COLOUR_YELLOW;
                        StatusColour = Color.Yellow;
                    } else {
                        StatusChar = JLCD.COLOUR_RED;
                        StatusColour = Color.Red;
                    }

                    // Work out the status text
                    String middleText = mytagIngot;
                    if (oreToText.ContainsKey(mytagIngot)) {
                        middleText = oreToText[mytagIngot];
                    }
                    String statusEND = " " + StatusChar + StatusChar;
                    String statusText = statusEND + " [" + middleText + "]" + statusEND;
                    jdbg.Debug("StatusText: " + statusText);

                    // Find where to write to and display the text
                    String lcdTag = mytag + ":" + mytagIngot.ToUpper();
                    List<IMyTerminalBlock> drawLCDs = jlcd.GetLCDsWithTag(lcdTag);
                    if (drawLCDs.Count > 0) { 
                        jlcd.InitializeLCDs(drawLCDs, TextAlignment.CENTER);
                        jlcd.SetupFont(drawLCDs, 1, statusText.Length, true);
                        jlcd.SetLCDFontColour(drawLCDs, StatusColour);
                        jlcd.WriteToAllLCDs(drawLCDs, statusText, false);
                    } else {
                        jdbg.DebugAndEcho("ERROR: Couldnt find LCD with tag :'" + lcdTag  +"'");

                        List<IMyTerminalBlock> allLCDs = new List<IMyTerminalBlock>();
                        GridTerminalSystem.GetBlocksOfType(allLCDs, (IMyTerminalBlock x) => (
                                                                                               (x.CustomName != null) &&
                                                                                               (x.CustomName.ToUpper().Contains("[" + lcdTag + "]")) 
                                                                                              ));
                        jdbg.DebugAndEcho("but found " + allLCDs.Count);

                    }

                    if (statusText.Length > longestRow) longestRow = statusText.Length;
                    // Deliberately too long so it goes off the end as sizes of col vs chars cause them to mismatch
                    fullScreen += statusText.PadRight(50,StatusChar) + "\n";
                    totalRows += 1;
                }

                jdbg.Debug("Drawing summary screen on [Status:all]");
                List<IMyTerminalBlock> screenLCDs = jlcd.GetLCDsWithTag(mytag + ":ALL");
                if (screenLCDs.Count > 0) {
                    jlcd.InitializeLCDs(screenLCDs, TextAlignment.LEFT);
                    jlcd.SetupFont(screenLCDs, longestRow, totalRows, true);
                    jlcd.WriteToAllLCDs(screenLCDs, fullScreen, false);
                }

                jdbg.Alert("Completed - OK", "GREEN", alertTag, thisScript);
            }
            catch (Exception ex) {
                jdbg.Alert("Exception - " + ex.ToString() + "\n" + ex.StackTrace, "RED", alertTag, thisScript);
            }
        }
    }
}
