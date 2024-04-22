using Sandbox.ModAPI.Ingame;
using System;
using System.Collections;
using System.Collections.Generic;
using VRage;
using VRage.Game.GUI.TextPanel;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ModAPI.Ingame.Utilities;

namespace IngameScript
{
    partial class Program : MyGridProgram
    {

        // ----------------------------- CUT -------------------------------------
        String thisScript = "CountOre";

        // My configuration
        // Example custom data in programming block:

        // Development or user config time flags
        bool debug = false;
        bool stayRunning = true;

        // Data read from program config
        String lcdId = "IDONTCARE";    // Which LCDs to output the details to
        int refreshSpeed = 60;         // Only once a minute by default
        bool isShort = false;          // Default to 5 seconds if not provided

        // Private variables
        private JDBG jdbg = null;
        private JINV jinv = null;
        private JLCD jlcd = null;
        private String alertTag = "alert";    // TODO: Could move into config

        // -------------------------------------------
        /* Example custom data in programming block: 
[Config]
LCD=OreStat
refreshSpeed=10
format=short  (or long)
        */
        // -------------------------------------------

        // Internals
        DateTime lastCheck = new DateTime(0);

        // Things to display
        static Dictionary<String, String> gasses = new Dictionary<String, String>()
        {
            { "Hydrogen",   "H2" },
            { "Oxygen",     "O2" },
        };

        static Dictionary<String, String> ores = new Dictionary<String, String>()
        {
            { "Uranium",    "U " },
            { "Silver",     "Ag" },
            { "Silicon",    "Si" },
            { "Platinum",   "Pt" },
            { "Nickel",     "Ni" },
            { "Magnesium",  "Mg" },
            { "Iron",       "Fe" },
            { "Gold",       "Au" },
            { "Cobalt",     "Co" },

            { "Stone",      "St" },
            { "Scrap",      "Sc" },
            { "Ice",        "Ice" },
        };

        static Dictionary<String, int> oresTage = new Dictionary<String, int>()
        {
            { "Cobalt",     1 },
            { "Gold",       1 },
            { "Iron",       1 },
            { "Magnesium",  1 },
            { "Nickel",     1 },
            { "Platinum",   1 },
            { "Silicon",    1 },
            { "Silver",     1 },
            { "Uranium",    1 },

            { "Stone",      2 },   

            { "Ice",        3 },
            { "Scrap",      3 },
        };

        // ---------------------------------------------------------------------------
        // Constructor
        // ---------------------------------------------------------------------------
        public Program()
        {
            jdbg = new JDBG(this, debug);
            jlcd = new JLCD(this, jdbg, false);
            jinv = new JINV(jdbg);
            jlcd.UpdateFullScreen(Me, thisScript);

            // Run every 100 ticks, but relies on internal check to only actually
            // perform on a defined frequency
            if (stayRunning) {
                Runtime.UpdateFrequency = UpdateFrequency.Update100;
            }
        }

        // ---------------------------------------------------------------------------
        // Save
        // ---------------------------------------------------------------------------
        public void Save()
        {
            // Nothing needed
        }

        // ---------------------------------------------------------------------------
        // Mainline code called every update frequency
        // ---------------------------------------------------------------------------
        public void Main(string argument, UpdateType updateSource)
        {
            // ---------------------------------------------------------------------------
            // Decide whether to actually do anything
            // ---------------------------------------------------------------------------
            if (stayRunning) {
                TimeSpan timeSinceLastCheck = DateTime.Now - lastCheck;
                if (timeSinceLastCheck.TotalSeconds >= refreshSpeed) {
                    lastCheck = DateTime.Now;
                } else {
                    return;
                }
            }

            try {
                // ---------------------------------------------------------------------------
                // We only get here if we are refreshing
                // ---------------------------------------------------------------------------                
                jdbg.ClearDebugLCDs();  // Clear the debug screens
                jdbg.DebugAndEcho("Main from " + thisScript + " running..." + DateTime.Now.ToString());

                // ---------------------------------------------------------------------------
                // Get my custom data and parse to get the config
                // ---------------------------------------------------------------------------
                MyIni _ini = new MyIni();
                MyIniParseResult result;
                if (!_ini.TryParse(Me.CustomData, out result))
                    throw new Exception(result.ToString());

                // Get the value of the "LCD" key under the "config" section.
                String myLCD = _ini.Get("config", "LCD").ToString();
                if (myLCD != null) {
                    Echo("Writing to any LCD with '" + myLCD + "' in the name");
                    lcdId = myLCD;
                } else {
                    Echo("Error: No LCD configured\nPlease add [config] for LCD=<substring>");
                    return;
                }

                // Get the value of the "LCD" key under the "config" section.
                String myFormat = _ini.Get("config", "format").ToString();
                if (myFormat != null) {
                    if (myFormat.Equals("short")) {
                        isShort = true;
                        Echo("Using short format names");
                    } else if (myFormat.Equals("long")) {
                        isShort = false;
                        Echo("Using long format names");
                    } else {
                        Echo("Error: format must be short or long - aborting");
                        return;
                    }
                } else {
                    Echo("No format configured - defaulting to long");
                    isShort = false;
                }

                // Get the value of the "refreshSpeed" key under the "config" section.
                int newrefreshSpeed = _ini.Get("config", "refreshSpeed").ToInt32();
                Echo("New refresh speed will be " + newrefreshSpeed);
                if (newrefreshSpeed < 1) {
                    Echo("Invalid refresh speed or not defined - defaulting to 5 seconds");
                    refreshSpeed = 5;
                } else {
                    Echo("Refresh speed set to " + newrefreshSpeed + " sceconds");
                    refreshSpeed = newrefreshSpeed;
                }

                // -----------------------------------------------------------------
                // Real work starts here
                // -----------------------------------------------------------------

                // Empty the running total
                Dictionary<String, MyFixedPoint> allIngots = new Dictionary<String, MyFixedPoint>();
                Dictionary<String, MyFixedPoint> allOres = new Dictionary<String, MyFixedPoint>();
                foreach (KeyValuePair<String, String> kvp in ores) {
                    allIngots[kvp.Key] = 0;
                    allOres[kvp.Key] = 0;
                }

                // ---------------------------------------------------------------------------
                // Enumerate all blocks - On current grid and has an inventory
                // ---------------------------------------------------------------------------
                List<IMyTerminalBlock> blocks = new List<IMyTerminalBlock>();
                GridTerminalSystem.GetBlocksOfType(blocks, (IMyTerminalBlock x) => (
                                                                                     (x.HasInventory) &&
                                                                                     (x.CubeGrid == Me.CubeGrid)
                                                                                      ));
                jdbg.Debug("Found " + blocks.Count + " blocks with inventories");

                foreach (var thisblock in blocks) {
                    // ---------------------------------------------------------------------------
                    // Sum all items
                    // ---------------------------------------------------------------------------
                    for (int invCount = 0; invCount < thisblock.InventoryCount; invCount++) {
                        List<MyInventoryItem> allItemsInInventory = new List<MyInventoryItem>();
                        IMyInventory inv = thisblock.GetInventory(invCount);
                        inv.GetItems(allItemsInInventory);

                        for (int j = 0; j < allItemsInInventory.Count; j++) {
                            String name = allItemsInInventory[j].Type.SubtypeId.ToString();

                            // For resources:
                            // Type.TypeId = Ore or Ingots
                            // Subtype = Type of resource

                            if (allItemsInInventory[j].Type.TypeId.ToString().Equals("MyObjectBuilder_Ingot")) {
                                if (allIngots.ContainsKey(name)) {
                                    allIngots[name] += allItemsInInventory[j].Amount;
                                } else {
                                    if (!ores.ContainsKey(name)) {
                                        Echo("Error: Unrecognized ore: '" + name + "'");
                                        return;
                                    }
                                    allIngots[name] = allItemsInInventory[j].Amount;
                                }
                            }

                            if (allItemsInInventory[j].Type.TypeId.ToString().Equals("MyObjectBuilder_Ore")) {
                                if (allOres.ContainsKey(name)) {
                                    allOres[name] += allItemsInInventory[j].Amount;
                                } else {
                                    if (!ores.ContainsKey(name)) {
                                        Echo("Error: Unrecognized ore: '" + name + "'");
                                        return;
                                    }
                                    allOres[name] = allItemsInInventory[j].Amount;
                                }
                            }
                        }
                    }
                }

                // Initialize as no gas resourced
                Dictionary<String, double> gasCapacity = new Dictionary<String, double>();
                Dictionary<String, double> gasCurrent = new Dictionary<String, double>();
                foreach (KeyValuePair<String, String> kvp in gasses) {
                    gasCapacity[kvp.Key] = 0.0F;
                    gasCurrent[kvp.Key] = 0.0F;
                }

                // ---------------------------------------------------------------------------
                // Enumerate all GasTanks - On current grid and is a GasTank
                // ---------------------------------------------------------------------------
                List<IMyTerminalBlock> tanks = new List<IMyTerminalBlock>();
                GridTerminalSystem.GetBlocksOfType(tanks, (IMyTerminalBlock x) => (
                                                                                     (x is IMyGasTank) &&
                                                                                     (x.CubeGrid == Me.CubeGrid)
                                                                                      ));
                jdbg.Debug("Found " + tanks.Count + " gas tanks");

                jdbg.Debug("Enumerating tanks");
                foreach (var thisTank in tanks) {
                    String Key = "?";
                    if (thisTank.ToString().ToUpper().Contains("OXYGEN")) {
                        Key = "Oxygen";
                    } else if (thisTank.ToString().ToUpper().Contains("HYDROGEN")) {
                        Key = "Hydrogen";
                    } else {
                        throw new Exception("Unexpected : " + thisTank);
                    }
                    IMyGasTank tank = (IMyGasTank)thisTank;

                    gasCapacity[Key] += tank.Capacity;
                    gasCurrent[Key] += (tank.Capacity * tank.FilledRatio);
                }

                // ---------------------------------------------------------------------------
                // HARD: Enumerate generators and identify Hydrogen generators
                // ---------------------------------------------------------------------------
                List<IMyTerminalBlock> engines = new List<IMyTerminalBlock>();
                GridTerminalSystem.GetBlocksOfType(engines, (IMyTerminalBlock x) => (
                                                                                     (x is IMyPowerProducer) &&
                                                                                     (x.BlockDefinition.ToString().Contains("HydrogenEngine")) && 
                                                                                     (x.CubeGrid == Me.CubeGrid)
                                                                                      ));
                jdbg.Debug("Found " + tanks.Count + " hydrogen engines");

                jdbg.Debug("Enumerating engines");
                foreach (var thisEngine in engines) {
                    String Key = "Hydrogen";

                    /* Details contains "Filled: 100.0% (100000L/100000L)" */
                    //jdbg.Debug("Details: " + thisEngine.DetailedInfo);
                    string detailedInfo = thisEngine.DetailedInfo;
                    detailedInfo = detailedInfo.Substring(detailedInfo.IndexOf("(") + 1);
                    String filled = detailedInfo.Substring(0, detailedInfo.IndexOf("L"));
                    detailedInfo = detailedInfo.Substring(detailedInfo.IndexOf("/") + 1);
                    String capacity = detailedInfo.Substring(0, detailedInfo.IndexOf("L"));

                    gasCapacity[Key] += double.Parse(capacity);
                    gasCurrent[Key] += double.Parse(filled);
                }

                // ---------------------------------------------------------------------------
                // List out the result
                // ---------------------------------------------------------------------------
                jdbg.Debug("Building final screen");

                String textToOutput = thisScript  + " status at " + lastCheck.ToString() + ":\n";
                int pad = 12;
                if (isShort) {
                    pad = 3;
                }
                for (int Stage = 0; Stage < 4; Stage++) {
                    foreach (KeyValuePair<String, MyFixedPoint> kvp in allIngots) {
                        if (oresTage[kvp.Key] == Stage) {
                            if (!isShort) {
                                textToOutput += (kvp.Key).PadRight(pad);
                            } else {
                                textToOutput += ores[kvp.Key].PadRight(pad);
                            }
                            textToOutput += ("" + allOres[kvp.Key].ToIntSafe()).PadLeft(10) + " Kg";

                            if (Stage < 3) {
                                textToOutput += ": " + ("" + kvp.Value.ToIntSafe()).PadLeft(10) + " Kg";
                            }
                            textToOutput += "\n";
                        }
                    }
                }
                textToOutput += "\n";
                textToOutput += "\n";

                // Add Hydrogen and Oxygen status
                foreach (KeyValuePair<String, String> kvp in gasses) {
                    if (!isShort) {
                        textToOutput += (kvp.Key).PadRight(pad);
                    } else {
                        textToOutput += gasses[kvp.Key].PadRight(pad);
                    }
                    double percent = (gasCurrent[kvp.Key] * 100) / (gasCapacity[kvp.Key]);
                    percent = Math.Truncate(percent * 100) / 100;                           // Helps 2 digit %            
                    String pctLine = " (" + string.Format("{0:N2}%", percent);
                    pctLine += ")    ";
                    pctLine += FormatNumber(gasCurrent[kvp.Key]) + " / " + FormatNumber(gasCapacity[kvp.Key]);
                    textToOutput += pctLine + "\n";
                }

                int lineLength = 40;
                if (!isShort) lineLength = 50;

                DrawLCD(lcdId, textToOutput, lineLength);
                jdbg.Debug(textToOutput);
                jdbg.Alert("Completed - OK", "GREEN", alertTag, thisScript);
            }
            catch (Exception ex) {
                jdbg.Alert("Exception - " + ex.ToString() + "\n" + ex.StackTrace, "RED", alertTag, thisScript);
            }
        }

        private static string FormatNumber(double num)
        {
            // Ensure number has max 3 significant digits (no rounding up can happen)
            double i = (double)Math.Pow(10, (int)Math.Max(0, Math.Log10(num) - 2));
            num = num / i * i;

            if (num >= 1000000000)
                return (num / 1000000000D).ToString("0.##") + "B";
            if (num >= 1000000)
                return (num / 1000000D).ToString("0.##") + "M";
            if (num >= 1000)
                return (num / 1000D).ToString("0.##") + "K";

            return num.ToString("#,0");
        }

        void DrawLCD(String tag, String screenContents, int maxLen)
        {
            List<IMyTerminalBlock> drawLCDs = jlcd.GetLCDsWithTag(tag);
            jlcd.InitializeLCDs(drawLCDs, TextAlignment.LEFT);
            jlcd.SetupFont(drawLCDs, screenContents.Split('\n').Length + 1, maxLen, false);
            jlcd.WriteToAllLCDs(drawLCDs, screenContents, false);
        }
        // ----------------------------- CUT -------------------------------------
    }
}
