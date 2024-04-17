using Sandbox.Game.EntityComponents;
using Sandbox.Game.GameSystems.Electricity;
using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;
using SpaceEngineers.Game.ModAPI.Ingame;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml.Schema;
using VRage;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.GUI.TextPanel;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ModAPI.Ingame.Utilities;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.GameServices;
using VRage.Utils;
using VRageMath;
using static IngameScript.Program;

namespace IngameScript
{
    partial class Program : MyGridProgram
    {
        // ----------------------------- CUT -------------------------------------
        String thisScript = "CountOre";

        // Development or user config time flags
        bool debug = true;
        bool stayRunning = false;

        // Data read from program config
        String myTag = "IDONTCARE";    // Which LCDs to output the details to
        int refreshSpeed = 60;         // Only once a minute by default

        // Private variables
        private JDBG jdbg = null;
        private JINV jinv = null;
        private JLCD jlcd = null;
        private String alertTag = "alert";    // TODO: Could move into config

        public enum SCREEN {
            DEFAULT, WIND, NUCLEAR, HYDROGEN, BATTERY, SOLAR,
            GRAPHDEFAULT, GRAPHWIND, GRAPHNUCLEAR, GRAPHHYDROGEN, GRAPHBATTERY, GRAPHSOLAR,
        };


        // -------------------------------------------
        /* Example custom data in programming block: 
[Config]
tag=power
refreshSpeed=10
        */
        // -------------------------------------------

        // Internals
        DateTime lastCheck = new DateTime(0);
        int chartLength = 25;

        // ---------------------------------------------------------------------------
        // Constructor
        // ---------------------------------------------------------------------------
        public Program()
        {
            jdbg = new JDBG(this, debug);
            jlcd = new JLCD(this, jdbg, false);
            jinv = new JINV(jdbg);

            // Run every 100 ticks, but relies on internal check to only actually
            // perform on a defined frequency
            if (stayRunning) {
                Runtime.UpdateFrequency = UpdateFrequency.Update100;
            }

            // Parse the history
            if (Storage != null && Storage.Length > 0) {

            } else {

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
                String myLCD = _ini.Get("config", "tag").ToString();
                if (myLCD != null) {
                    Echo("Writing to any LCD with '" + myLCD + "' in the name");
                    myTag = myLCD;
                } else {
                    Echo("Error: No LCD configured\nPlease add [config] for tag=<substring>");
                    return;
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

                // ---------------------------------------------------------------------------
                // Enumerate all blocks which i am interested in:
                //   - Solar Power: IMySolarPanel <- IMyPowerProducer
                //   - Wind Power: IMyWindTurbine <- IMyPowerProducer
                //   - Nuclear Power: IMyReactor <- IMyPowerProducer
                //   - Hydrogen Power: Only IMyGasTank, IMyPowerProducer 
                //
                //   - Battery Block: IMyBatteryBlock
                // ---------------------------------------------------------------------------
                bool IsLargeGrid = (Me.CubeGrid.GridSizeEnum.ToString().Contains("Large"));

                // GridTerminalSystem.GetBlocksOfType<IMyPowerProducer>(allHydroEngines, t => t.BlockDefinition.SubtypeId.Contains("HydrogenEngine"));
                //    which is IMyPowerProducer 
                List<IMyTerminalBlock> blocks = new List<IMyTerminalBlock>();
                GridTerminalSystem.GetBlocksOfType(blocks, (IMyTerminalBlock x) => (
                                                                                     (x.CubeGrid == Me.CubeGrid) &&
                                                                                     (x is IMyPowerProducer)
                                                                                      ));
                jdbg.Debug("Found " + blocks.Count + " blocks which produce power");

                // Lets work out the data per-type
                Hashtable powerData = new Hashtable();
                powerTypeInitialize(ref powerData, "TOTAL");
                powerTypeInitialize(ref powerData, "SOLAR");
                powerTypeInitialize(ref powerData, "WIND");
                powerTypeInitialize(ref powerData, "NUCLEAR");
                powerTypeInitialize(ref powerData, "HYDROGEN");
                powerTypeInitialize(ref powerData, "BATTERY");

                // TODO: DISABLEDMAX, RECHARGEMAX
                foreach (var thisBlock in blocks) {
                    jdbg.Debug("Adding up: " + thisBlock.CustomName);
                    IMyPowerProducer powerBlock = thisBlock as IMyPowerProducer;
                    Hashtable powerType;
                    Hashtable powerTypeT;
                    float maxPossibleSize = powerBlock.MaxOutput;

                    bool isOff = !(powerBlock.Enabled || powerBlock.IsWorking);
                    jdbg.Debug("isOff: " + isOff);
                    bool isRecharge = false;

                    powerTypeT = (Hashtable)powerData["TOTAL"];
                    if (thisBlock is IMySolarPanel) {
                        powerType = (Hashtable)powerData["SOLAR"];
                        // Max output
                        if (IsLargeGrid) {
                            maxPossibleSize = 0.16F;
                        } else {
                            maxPossibleSize = 0.04F;
                        }
                    } else if (thisBlock is IMyWindTurbine) {
                        powerType = (Hashtable)powerData["WIND"];
                        // Nothing extra
                    } else if (thisBlock is IMyReactor) {
                        powerType = (Hashtable)powerData["NUCLEAR"];
                        // Nothing extra
                    } else if (thisBlock.BlockDefinition.SubtypeId.Contains("HydrogenEngine")) {
                        powerType = (Hashtable)powerData["HYDROGEN"];
                        // Nothing extra
                    } else if (thisBlock is IMyBatteryBlock) {
                        powerType = (Hashtable)powerData["BATTERY"];
                        IMyBatteryBlock battery = (IMyBatteryBlock)thisBlock;
                        isRecharge = battery.ChargeMode == ChargeMode.Recharge;

                        // Different
                        if (!isOff) {
                            powerType["CURRENTSTORED"] = (float)powerType["CURRENTSTORED"] + battery.CurrentStoredPower;
                            powerType["MAXSTORED"] = (float)powerType["MAXSTORED"] + battery.MaxStoredPower;
                            powerType["CURRENTINPUT"] = (float)powerType["CURRENTINPUT"] + battery.CurrentInput;
                            powerType["MAXINPUT"] = (float)powerType["MAXINPUT"] + battery.CurrentInput;
                        }
                    } else {
                        jdbg.DebugAndEcho("WARNING: Unexpected block: " + thisBlock.CustomName);
                        continue;
                    }

                    powerType["NUMBER"] = (float)powerType["NUMBER"] + 1.0F;
                    if (!isOff) {
                        // Current output
                        powerType["CURRENTOUTPUT"] = (float)powerType["CURRENTOUTPUT"] + powerBlock.CurrentOutput;
                    } else {

                    }
                    // Max output
                    powerType["MAXOUTPUT"] = (float)powerType["MAXOUTPUT"] + powerBlock.MaxOutput;
                    // Max Possible output
                    powerType["MAXPOSSIBLE"] = (float)powerType["MAXPOSSIBLE"] + maxPossibleSize;
                    if (isOff) {
                        powerType["DISABLEDMAX"] = (float)powerType["DISABLEDMAX"] + maxPossibleSize;
                    }
                    if (isRecharge) {
                        powerType["RECHARGEMAX"] = (float)powerType["RECHARGEMAX"] + maxPossibleSize;
                    }


                    if (!(thisBlock is IMyBatteryBlock)) {
                        powerTypeT["NUMBER"] = (float)powerTypeT["NUMBER"] + 1.0F;
                        if (!isOff && !isRecharge) {
                            powerTypeT["CURRENTOUTPUT"] = (float)powerTypeT["CURRENTOUTPUT"] + powerBlock.CurrentOutput;
                            powerTypeT["MAXOUTPUT"] = (float)powerTypeT["MAXOUTPUT"] + powerBlock.MaxOutput;
                            powerTypeT["MAXPOSSIBLE"] = (float)powerTypeT["MAXPOSSIBLE"] + maxPossibleSize;
                        }
                    }

                    if (isOff) {
                        powerTypeT["DISABLEDMAX"] = (float)powerTypeT["DISABLEDMAX"] + maxPossibleSize;
                    }
                    if (isRecharge) {
                        powerTypeT["RECHARGEMAX"] = (float)powerTypeT["RECHARGEMAX"] + maxPossibleSize;
                    }

                    jdbg.Debug("Max output now " + (float)powerType["MAXOUTPUT"]);
                }

                // Update the history

                List<IMyTerminalBlock> drawLCDs = jlcd.GetLCDsWithTag(myTag);
                jdbg.Debug("Drawing screens - " + drawLCDs.Count() + " to display");

                foreach (var thisLCD in drawLCDs) {
                    SCREEN thisOutput = SCREEN.DEFAULT;

                    if (!(thisLCD.CustomData == null || thisLCD.CustomData.Equals(""))) {
                        // Get custom data 
                        if (_ini.TryParse(thisLCD.CustomData, out result)) {
                            String whichPage = _ini.Get("config", "page").ToString().ToUpper();
                            if (Enum.TryParse<SCREEN>(whichPage, out thisOutput)) {
                                jdbg.Debug(thisLCD.CustomName + " set to " + thisOutput);
                            } else {
                                jdbg.DebugAndEcho("ERROR: Invalid config in " + thisLCD.CustomName + " of " + whichPage);
                            }
                        } else {
                            jdbg.DebugAndEcho("ERROR: Cannot parse config in " + thisLCD.CustomName);
                        }
                    }

                    // Used when drawing screen
                    String screen = "";
                    int maxWidth = 0;
                    int maxDepth = 0;
                    bool isChart = false;
                    buildScreen(thisOutput.ToString(), powerData, ref screen, ref maxDepth, ref maxWidth, ref isChart, false);

                    List<IMyTerminalBlock> justThisLCD = new List<IMyTerminalBlock>();
                    justThisLCD.Add(thisLCD);
                    jlcd.InitializeLCDs(justThisLCD, TextAlignment.LEFT);
                    jlcd.SetupFont(justThisLCD, maxWidth, maxDepth, true);
                    jlcd.WriteToAllLCDs(justThisLCD, screen, false);
                }
                jdbg.Alert("Completed - OK", "GREEN", alertTag, thisScript);
            }
            catch (Exception ex) {
                jdbg.Alert("Exception - " + ex.ToString() + "\n" + ex.StackTrace, "RED", alertTag, thisScript);
            }
        }
        public void powerTypeInitialize(ref Hashtable hash, String power)
        {
            Hashtable thisPower = new Hashtable();
            thisPower.Add("CURRENTOUTPUT", 0.0F);
            thisPower.Add("DISABLEDMAX", 0.0F);
            thisPower.Add("RECHARGEMAX", 0.0F);
            thisPower.Add("MAXOUTPUT", 0.0F);
            thisPower.Add("MAXPOSSIBLE", 0.0F);
            thisPower.Add("NUMBER", 0.0F);
            thisPower.Add("CURRENTSTORED", 0.0F);
            thisPower.Add("MAXSTORED", 0.0F);
            thisPower.Add("CURRENTINPUT", 0.0F);
            thisPower.Add("MAXINPUT", 0.0F);
            hash.Add(power, thisPower);
        }

        public void buildScreen(String type, Hashtable data, ref String screen,
                                ref int maxWidth, ref int curDepth, ref bool isChart,
                                bool isInSummary)
        {
            isChart = false;
            jdbg.Debug("Building screen for " + type + " - " + maxWidth + " - " + curDepth);

            //DEFAULT, WIND, NUCLEAR, HYDROGEN, BATTERY, SOLAR
            Hashtable powerdata;
            String thisLine;
            if (type.Equals("DEFAULT")) {
                powerdata = (Hashtable)data["TOTAL"];
                AddLine(ref maxWidth, ref curDepth, ref screen, "Status of all power types at " + DateTime.Now.ToString());
                AddLine(ref maxWidth, ref curDepth, ref screen, "");

                buildScreen("WIND", data, ref screen, ref maxWidth, ref curDepth, ref isChart, true);
                buildScreen("NUCLEAR", data, ref screen, ref maxWidth, ref curDepth, ref isChart, true);
                buildScreen("HYDROGEN", data, ref screen, ref maxWidth, ref curDepth, ref isChart, true);
                buildScreen("SOLAR", data, ref screen, ref maxWidth, ref curDepth, ref isChart, true);
                buildScreen("BATTERY", data, ref screen, ref maxWidth, ref curDepth, ref isChart, true);

                thisLine = "Overall Totals";
                AddLine(ref maxWidth, ref curDepth, ref screen, thisLine);
                AddLine(ref maxWidth, ref curDepth, ref screen, "================================================");

                thisLine = "Current Output   :   " + powerdata["CURRENTOUTPUT"] + " MW";
                AddLine(ref maxWidth, ref curDepth, ref screen, thisLine);
                float unavail = (float)powerdata["DISABLEDMAX"] + (float)powerdata["RECHARGEMAX"];
                thisLine = "Max. poss Output :   " + powerdata["MAXPOSSIBLE"] + " MW";
                if (unavail > 0.0F) thisLine += "" + unavail + " MW unavailable)";

                AddLine(ref maxWidth, ref curDepth, ref screen, thisLine);

                thisLine = "MAXIMUM: [";
                String statusBar = "";

                AddLine(ref maxWidth, ref curDepth, ref screen, "");

                float disabledTotal = 0.0F;
                float runningTotal = 0.0F;
                runningTotal += (float)((Hashtable)data["WIND"])["MAXPOSSIBLE"];
                runningTotal -= (float)((Hashtable)data["WIND"])["DISABLEDMAX"];
                disabledTotal += (float)((Hashtable)data["WIND"])["DISABLEDMAX"];

                statusBar = statusBar.PadRight((int)Math.Floor(((float)chartLength) *
                    (
                        runningTotal /
                        ((float)((Hashtable)data["TOTAL"])["MAXPOSSIBLE"])
                    )), GetColour("WIND"));
                runningTotal += (float)((Hashtable)data["SOLAR"])["MAXPOSSIBLE"];
                runningTotal -= (float)((Hashtable)data["SOLAR"])["DISABLEDMAX"];
                disabledTotal += (float)((Hashtable)data["SOLAR"])["DISABLEDMAX"];
                statusBar = statusBar.PadRight((int)Math.Floor(((float)chartLength) *
                    (
                        runningTotal /
                        ((float)((Hashtable)data["TOTAL"])["MAXPOSSIBLE"])
                    )), GetColour("SOLAR"));
                runningTotal += (float)((Hashtable)data["NUCLEAR"])["MAXPOSSIBLE"];
                runningTotal -= (float)((Hashtable)data["NUCLEAR"])["DISABLEDMAX"];
                disabledTotal += (float)((Hashtable)data["NUCLEAR"])["DISABLEDMAX"];
                statusBar = statusBar.PadRight((int)Math.Floor(((float)chartLength) *
                    (
                        runningTotal /
                        ((float)((Hashtable)data["TOTAL"])["MAXPOSSIBLE"])
                    )), GetColour("NUCLEAR"));
                runningTotal += (float)((Hashtable)data["HYDROGEN"])["MAXPOSSIBLE"];
                runningTotal -= (float)((Hashtable)data["HYDROGEN"])["DISABLEDMAX"];
                disabledTotal += (float)((Hashtable)data["HYDROGEN"])["DISABLEDMAX"];
                statusBar = statusBar.PadRight((int)Math.Floor(((float)chartLength) *
                    (
                        runningTotal /
                        ((float)((Hashtable)data["TOTAL"])["MAXPOSSIBLE"])
                    )), GetColour("HYDROGEN"));

                runningTotal += disabledTotal;
                statusBar = statusBar.PadRight((int)Math.Floor(((float)chartLength) *
                    (
                        runningTotal /
                        ((float)((Hashtable)data["TOTAL"])["MAXPOSSIBLE"])
                    )), JLCD.COLOUR_RED);

                thisLine = thisLine + statusBar + "]";
                AddLine(ref maxWidth, ref curDepth, ref screen, thisLine);
                AddLine(ref maxWidth, ref curDepth, ref screen, "");

                runningTotal = 0.0F;
                thisLine = "CURRENT: [";
                statusBar = "";

                runningTotal += (float)((Hashtable)data["WIND"])["CURRENTOUTPUT"];
                statusBar = statusBar.PadRight((int)Math.Floor(((float)chartLength) *
                    (
                        runningTotal /
                        ((float)((Hashtable)data["TOTAL"])["CURRENTOUTPUT"])
                    )), GetColour("WIND"));
                runningTotal += (float)((Hashtable)data["SOLAR"])["CURRENTOUTPUT"];
                statusBar = statusBar.PadRight((int)Math.Floor(((float)chartLength) *
                    (
                        runningTotal /
                        ((float)((Hashtable)data["TOTAL"])["CURRENTOUTPUT"])
                    )), GetColour("SOLAR"));
                runningTotal += (float)((Hashtable)data["NUCLEAR"])["CURRENTOUTPUT"];
                statusBar = statusBar.PadRight((int)Math.Floor(((float)chartLength) *
                    (
                        runningTotal /
                        ((float)((Hashtable)data["TOTAL"])["CURRENTOUTPUT"])
                    )), GetColour("NUCLEAR"));
                runningTotal += (float)((Hashtable)data["HYDROGEN"])["CURRENTOUTPUT"];
                statusBar = statusBar.PadRight((int)Math.Floor(((float)chartLength) *
                    (
                        runningTotal /
                        ((float)((Hashtable)data["TOTAL"])["CURRENTOUTPUT"])
                    )), GetColour("HYDROGEN"));

                thisLine = thisLine + statusBar + "]";
                AddLine(ref maxWidth, ref curDepth, ref screen, thisLine);
                AddLine(ref maxWidth, ref curDepth, ref screen, "");

                thisLine = "    Key: " +
                        GetColour("WIND") + " - WIND  " +
                        GetColour("SOLAR") + " - SOLAR" +
                        JLCD.COLOUR_RED + " - DISABLED";
                AddLine(ref maxWidth, ref curDepth, ref screen, thisLine);
                thisLine = "         " +
                        GetColour("NUCLEAR") + " - NUCLEAR " + 
                        GetColour("HYDROGEN") + " - HYDROGEN ";
                AddLine(ref maxWidth, ref curDepth, ref screen, thisLine);

            } else {
                powerdata = (Hashtable)data[type];

                if (!isInSummary) {
                    AddLine(ref maxWidth, ref curDepth, ref screen, "Status of all " + type.ToLower() + " power at " + DateTime.Now.ToString());
                    AddLine(ref maxWidth, ref curDepth, ref screen, "================================================");
                    AddLine(ref maxWidth, ref curDepth, ref screen, "");
                } else {
                    AddLine(ref maxWidth, ref curDepth, ref screen, "All " + type.ToLower() + " power:");
                    AddLine(ref maxWidth, ref curDepth, ref screen, "================================================");
                    AddLine(ref maxWidth, ref curDepth, ref screen, "");
                }

                if (!isInSummary) {
                    // Total
                    thisLine = "No. " + PowerPlantType(type).PadRight(13) + ":   " + ((int)(float)powerdata["NUMBER"]);
                    AddLine(ref maxWidth, ref curDepth, ref screen, thisLine);
                    AddLine(ref maxWidth, ref curDepth, ref screen, "");
                }

                // Current
                thisLine = "Current Output   :   " + powerdata["CURRENTOUTPUT"] + " MW";
                if (isInSummary) thisLine = "  " + thisLine;
                AddLine(ref maxWidth, ref curDepth, ref screen, thisLine);
                if (!isInSummary) AddLine(ref maxWidth, ref curDepth, ref screen, "");

                // Maximum
                if (!type.Equals("BATTERY")) {
                    thisLine = "Maximum Output   :   " + powerdata["MAXOUTPUT"] + " MW";
                    if (isInSummary) thisLine = "  " + thisLine;
                    AddLine(ref maxWidth, ref curDepth, ref screen, thisLine);
                    if (!isInSummary) AddLine(ref maxWidth, ref curDepth, ref screen, "");
                    if (type.Equals("SOLAR")) {
                        thisLine = "Max possible Out :   " + powerdata["MAXPOSSIBLE"] + " MW";
                        if (isInSummary) thisLine = "  " + thisLine;
                        AddLine(ref maxWidth, ref curDepth, ref screen, thisLine);
                    }
                    if ((float)powerdata["DISABLEDMAX"] > 0) {
                        thisLine = "Disabled         :   " + powerdata["DISABLEDMAX"] + " MW";    // TODO: NOTWORKING @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
                        if (isInSummary) thisLine = "  " + thisLine;
                        AddLine(ref maxWidth, ref curDepth, ref screen, thisLine);
                    }
                } else {
                    thisLine = "Current Input    :   " + powerdata["CURRENTINPUT"] + " MW";
                    if (isInSummary) thisLine = "  " + thisLine;
                    AddLine(ref maxWidth, ref curDepth, ref screen, thisLine);
                    if (!isInSummary) AddLine(ref maxWidth, ref curDepth, ref screen, "");
                }

                if (type.Equals("BATTERY")) {
                    if (!isInSummary) {
                        // Maximum
                        thisLine = "Maximum Input    :   " + powerdata["MAXINPUT"] + " MW";
                        if (isInSummary) thisLine = "  " + thisLine;
                        AddLine(ref maxWidth, ref curDepth, ref screen, thisLine);
                        if (!isInSummary) AddLine(ref maxWidth, ref curDepth, ref screen, "");
                    }

                    // Current
                    thisLine = "Current Stored   :   " + powerdata["CURRENTSTORED"] + " MW";
                    if (isInSummary) thisLine = "  " + thisLine;
                    AddLine(ref maxWidth, ref curDepth, ref screen, thisLine);
                    if (!isInSummary) AddLine(ref maxWidth, ref curDepth, ref screen, "");

                    // Maximum
                    thisLine = "Maximum Stored   :   " + powerdata["MAXSTORED"] + " MW";
                    if (isInSummary) thisLine = "  " + thisLine;
                    AddLine(ref maxWidth, ref curDepth, ref screen, thisLine);
                    if (!isInSummary) AddLine(ref maxWidth, ref curDepth, ref screen, "");

                } else if (type.Equals("SOLAR")) {
                    thisLine = "Max possible Out :   " + powerdata["MAXPOSSIBLE"] + " MW";
                    if (isInSummary) thisLine = "  " + thisLine;
                    AddLine(ref maxWidth, ref curDepth, ref screen, thisLine);
                }

                if (!isInSummary) AddLine(ref maxWidth, ref curDepth, ref screen, "");

                if (!isInSummary) {
                    float disabledMax = (float)powerdata["DISABLEDMAX"];
                    float rechargeMax = (float)powerdata["RECHARGEMAX"];
                    float max = (float)powerdata["MAXOUTPUT"];
                    float cur = (float)powerdata["CURRENTOUTPUT"];
                    float maxmax = (float)powerdata["MAXOUTPUT"];
                    jdbg.Debug("Cur:" + cur + ", Max:" + max + ", MaxMax:" + maxmax);
                    if (type.Equals("SOLAR")) {
                        maxmax = (float)powerdata["MAXPOSSIBLE"];
                    }

                    thisLine = "Status           :   ";
                    if (maxmax == 0.0F) {
                        jdbg.Debug("None available");
                        thisLine += "[ None".PadRight(chartLength, JLCD.COLOUR_BLACK) + "]";
                    } else {
                        double disLOG = 0.0F;
                        double curLOG = 0.0F;
                        double maxLOG = 0.0F;
                        if (maxmax > 0) {
                            if (cur > 0) curLOG = Math.Log10(1 + ((cur/maxmax) * 9.0F));
                            jdbg.Debug("6 - " + curLOG);

                            if (max > 0) maxLOG = Math.Log10(1 + ((max / maxmax) * 9.0F));
                            if (disabledMax > 0) disLOG = Math.Log10(1 + ((disabledMax / maxmax) * 9.0F));
                        }
                        int curSQ = (int)Math.Floor(curLOG * chartLength);
                        int maxSQ = (int)Math.Floor(maxLOG * chartLength);

                        jdbg.Debug("Calc:" + curLOG + "," + maxLOG + "," + chartLength);
                        
                        String statusBar;
                        statusBar = "".PadRight(curSQ, JLCD.COLOUR_GREEN);
                        statusBar = statusBar.PadRight(maxSQ, JLCD.COLOUR_WHITE);
                        statusBar = statusBar.PadRight(chartLength, JLCD.COLOUR_BLACK);
                        thisLine += "[" + statusBar + "]";
                    }
                    AddLine(ref maxWidth, ref curDepth, ref screen, thisLine);
                }
                AddLine(ref maxWidth, ref curDepth, ref screen, "");
            }
        }

        public void buildChart(String type, Hashtable data, ref String screen,
                                ref int maxWidth, ref int maxDepth, ref bool isChart)
        {
            isChart = true;
            //GRAPHDEFAULT, GRAPHWIND, GRAPHNUCLEAR, GRAPHHYDROGEN, GRAPHBATTERY, GRAPHSOLAR
        }

        public void AddLine(ref int maxWidth, ref int curDepth, ref String screen, String line)
        {
            screen += line + "\n";
            curDepth += 1;
            if (line.Length > maxWidth) maxWidth = line.Length;
        }

        public String PowerPlantType(String type)
        {
            if (type.Equals("WIND")) return "Wind Turbines";   
            if (type.Equals("NUCLEAR")) return "Reactors";     
            if (type.Equals("HYDROGEN")) return "H2 Engines";  
            if (type.Equals("SOLAR")) return "Solar Panels";   
            if (type.Equals("BATTERY")) return "Batteries";    
            return "???" + type;
        }

        public char GetColour(String type)
        {
            if (type.Equals("WIND")) return JLCD.COLOUR_CYAN;
            if (type.Equals("NUCLEAR")) return JLCD.COLOUR_GREEN;
            if (type.Equals("HYDROGEN")) return jlcd.ColorToChar(0xff, 0x00, 0xff);
            if (type.Equals("SOLAR")) return JLCD.COLOUR_YELLOW;
            if (type.Equals("BATTERY")) return JLCD.COLOUR_BLUE;
            return JLCD.COLOUR_GREY;
        }

    }
}
