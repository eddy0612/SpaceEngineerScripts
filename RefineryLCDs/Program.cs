using Sandbox.Game.Entities.Cube;
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
using static IngameScript.Program;

namespace IngameScript
{
    partial class Program : MyGridProgram
    {

        // ----------------------------- CUT -------------------------------------
        String thisScript = "RefineryLCDs";

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
        Dictionary<String, String> ore2ingots = new Dictionary<String, String>();

        // Internals
        DateTime lastCheck = new DateTime(0);
        String mytag = "Status";

        public Program()
        {
            jdbg = new JDBG(this, debug);
            jlcd = new JLCD(this, jdbg, false);
            jinv = new JINV(jdbg);
            jlcd.UpdateFullScreen(Me, thisScript);

            // ---------------------------------------------------------------------------
            // Get my custom data and parse to get the config
            // ---------------------------------------------------------------------------
            MyIni _ini = new MyIni();
            MyIniParseResult result;
            if (!_ini.TryParse(Me.CustomData, out result))
                throw new Exception(result.ToString());

            // Get the value of the "tag" key under the "config" section.
            String tag = _ini.Get("config", "tag").ToString();
            if (tag != null) {
                Echo("Using tag of '[" + tag + "]'");
                mytag = tag.ToUpper();
            } else {
                Echo("Error: No LCD configured\nPlease add [config] for LCD=<substring>");
                return;
            }

            // Do one off initialization
            ore2ingots = new Dictionary<String, String>();
            jinv.addBluePrints(JINV.BLUEPRINT_TYPES.ORES, ref ore2ingots);

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

                List<IMyTerminalBlock> refineriesstatusLCDs = new List<IMyTerminalBlock>();
                GridTerminalSystem.GetBlocksOfType(refineriesstatusLCDs, (IMyTerminalBlock x) => (
                                                                                     // (x.CubeGrid == Me.CubeGrid) &&    -- Dont care I believe
                                                                                     (x.CustomName.ToUpper().Contains("[" + mytag.ToUpper() + "]")) &&
                                                                                     (x is IMyTextSurfaceProvider)
                                                                                      ));
                jdbg.Debug("Found " + refineriesstatusLCDs.Count + " refinery LCDs to update ");
                if (refineriesstatusLCDs.Count == 0) {
                    return;
                }

                foreach (IMyTerminalBlock statusLCD in refineriesstatusLCDs) {
                    jdbg.Debug("Processing: " + statusLCD.ToString());

                    MyIni _ini = new MyIni();
                    MyIniParseResult result;

                    bool finished = false;
                    String msg = "??";

                    if (!_ini.TryParse(statusLCD.CustomData, out result))
                        throw new Exception(result.ToString());

                    // Get the value of the "refinery" key under the "config" section.
                    String refName = _ini.Get("config", "refinery").ToString();
                    if (refName != null) {
                        Echo("Using refinery name of '" + refName + "'");
                    } else {
                        finished = true;
                        msg = "ERR: No refinery linked";
                    }

                    if (!finished) {
                        // Find all refineries with this name - if not one, write error
                        List<IMyTerminalBlock> refineries = new List<IMyTerminalBlock>();
                        GridTerminalSystem.GetBlocksOfType(refineries, (IMyTerminalBlock x) => (
                                                                                             // (x.CubeGrid == Me.CubeGrid) &&    -- Dont care I believe
                                                                                             (x.CustomName.Equals(refName)) && 
                                                                                             (x is IMyRefinery)
                                                                                              ));
                        jdbg.Debug("Found " + refineries.Count + " refineries with that name ");

                        if (refineries.Count == 0) {
                            finished = true;
                            msg = "ERR: Linked refinery not found";
                        } else if (refineries.Count > 1) {
                            finished = true;
                            msg = "ERR: Multiple linked refineries found";
                        } else {
                            msg = "";

                            // Work out the status colour
                            IMyInventory ores = ((IMyRefinery)refineries[0]).InputInventory;

                            // Add a status character
                            char StatusChar;
                            Color StatusColour;
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
                            msg = " " + StatusChar + " - ";

                            // Parse the inventory
                            List<MyInventoryItem> allOresInInventory = new List<MyInventoryItem>();
                            ores.GetItems(allOresInInventory);

                            for (int j = 0; j < allOresInInventory.Count; j++) {
                                String name = allOresInInventory[j].Type.ToString();
                                name = name.Replace("MyObjectBuilder_Ore/", "");
                                jdbg.Debug("inv: " + allOresInInventory[j].Type.ToString());
                                if (j > 0) msg += ",";
                                msg += name;
                            }
                            finished = true;
                        }
                    }

                    if (finished) {
                        List<IMyTerminalBlock> drawLCDs = new List<IMyTerminalBlock> ();
                        drawLCDs.Add(statusLCD);
                        jlcd.InitializeLCDs(drawLCDs, TextAlignment.CENTER);
                        jlcd.SetupFont(drawLCDs, 1, msg.Length, true);
                        jlcd.SetLCDFontColour(drawLCDs, Color.White);
                        jlcd.WriteToAllLCDs(drawLCDs, msg, false);
                    }

                }
                jdbg.Alert("Completed - OK", "GREEN", alertTag, thisScript);
            }
            catch (Exception ex) {
                jdbg.Alert("Exception - " + ex.ToString() + "\n" + ex.StackTrace, "RED", alertTag, thisScript);
            }
        }
    }
}
