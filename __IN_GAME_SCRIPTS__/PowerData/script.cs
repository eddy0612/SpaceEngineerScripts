/*
 * R e a d m e
 * -----------
 * 
 * In this file you can include any instructions or other comments you want to have injected onto the 
 * top of your final script. You can safely delete this file if you do not want any such comments.
 */

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

public class JCTRL
{
    MyGridProgram mypgm = null;
    JDBG jdbg = null;
    bool singleKey = false;

    public JCTRL(MyGridProgram pgm, JDBG dbg, bool SingleKeyOnly)
    {
        mypgm = pgm;
        jdbg = dbg;
    }

    // ---------------------------------------------------------------------------
    // Get a list of the LCDs with a specific tag
    // ---------------------------------------------------------------------------
    public List<IMyTerminalBlock> GetCTRLsWithTag(String tag)
    {
        List<IMyTerminalBlock> allCTRLs = new List<IMyTerminalBlock>();
        mypgm.GridTerminalSystem.GetBlocksOfType(allCTRLs, (IMyTerminalBlock x) => (
                                                                               (x.CustomName != null) &&
                                                                               (x.CustomName.ToUpper().IndexOf("[" + tag.ToUpper() + "]") >= 0) &&
                                                                               (x is IMyShipController)
                                                                              ));
        jdbg.Debug("Found " + allCTRLs.Count + " controllers with tag " + tag);
        return allCTRLs;
    }

    public bool IsOccupied(IMyShipController seat)
    {
        return seat.IsUnderControl;
    }

    public bool AnyKey(IMyShipController seat, bool allowJumpOrCrouch)
    {
        bool pressed = false;
        Vector3 dirn = seat.MoveIndicator;
        if (dirn.X != 0 || (allowJumpOrCrouch && dirn.Y != 0) || dirn.Z != 0) {
            pressed = true;
        }
        return pressed;
    }

    public bool IsLeft(IMyShipController seat)
    {
        Vector3 dirn = seat.MoveIndicator;
        if (singleKey && dirn.X < 0 && dirn.Y == 0 && dirn.Z == 0) return true;
        else if (!singleKey && dirn.X < 0) return true;
        return false;
    }
    public bool IsRight(IMyShipController seat)
    {
        Vector3 dirn = seat.MoveIndicator;
        if (singleKey && dirn.X > 0 && dirn.Y == 0 && dirn.Z == 0) return true;
        else if (!singleKey && dirn.X > 0) return true;
        return false;
    }
    public bool IsUp(IMyShipController seat)
    {
        Vector3 dirn = seat.MoveIndicator;
        if (singleKey && dirn.X == 0 && dirn.Y == 0 && dirn.Z < 0) return true;
        else if (!singleKey && dirn.Z < 0) return true;
        return false;
    }
    public bool IsDown(IMyShipController seat)
    {
        Vector3 dirn = seat.MoveIndicator;
        if (singleKey && dirn.X == 0 && dirn.Y == 0 && dirn.Z > 0) return true;
        else if (!singleKey && dirn.Z > 0) return true;
        return false;
    }
    public bool IsSpace(IMyShipController seat)
    {
        Vector3 dirn = seat.MoveIndicator;
        if (singleKey && dirn.X == 0 && dirn.Y > 0 && dirn.Z == 0) return true;
        else if (!singleKey && dirn.Y > 0) return true;
        return false;
    }
    public bool IsCrouch(IMyShipController seat)
    {
        Vector3 dirn = seat.MoveIndicator;
        if (singleKey && dirn.X == 0 && dirn.Y < 0 && dirn.Z == 0) return true;
        else if (!singleKey && dirn.Y < 0) return true;
        return false;
    }
    public bool IsRollLeft(IMyShipController seat)
    {
        float dirn = seat.RollIndicator;
        if (dirn < 0.0) return true;
        return false;
    }
    public bool IsRollRight(IMyShipController seat)
    {
        float dirn = seat.RollIndicator;
        if (dirn > 0.0) return true;
        return false;
    }
    public bool IsArrowLeft(IMyShipController seat)
    {
        Vector2 dirn = seat.RotationIndicator;
        if (singleKey && dirn.X == 0 && dirn.Y < 0) return true;
        else if (!singleKey && dirn.Y < 0) return true;
        return false;
    }
    public bool IsArrowRight(IMyShipController seat)
    {
        Vector2 dirn = seat.RotationIndicator;
        if (singleKey && dirn.X == 0 && dirn.Y > 0) return true;
        else if (!singleKey && dirn.Y > 0) return true;
        return false;
    }
    public bool IsArrowDown(IMyShipController seat)
    {
        Vector2 dirn = seat.RotationIndicator;
        if (singleKey && dirn.X > 0 && dirn.Y == 0) return true;
        else if (!singleKey && dirn.X > 0) return true;
        return false;
    }
    public bool IsArrowUp(IMyShipController seat)
    {
        Vector2 dirn = seat.RotationIndicator;
        if (singleKey && dirn.X < 0 && dirn.Y == 0) return true;
        else if (!singleKey && dirn.X < 0) return true;
        return false;
    }
}

public class JDBG
{
    public bool debug = false;                          /* Are we logging dbg messages */

    private MyGridProgram mypgm = null;                 /* Main pgm if needed */
    private JLCD jlcd = null;                           /* JLCD class */

    private bool inDebug = false;                       /* Avoid recursion: */
    private static List<IMyTerminalBlock> debugLCDs = null;    /* LCDs to write to */

    // ---------------------------------------------------------
    // Constructor
    // ---------------------------------------------------------
    public JDBG(MyGridProgram pgm, bool debugState)
    {
        mypgm = pgm;
        debug = debugState;
        jlcd = new JLCD(pgm, this, false);
    }

    // ---------------------------------------------------------
    // Echo - write a message to the console
    // ---------------------------------------------------------
    public void Echo(string str)
    {
        mypgm.Echo("JDBG: " + str);
    }

    // ---------------------------------------------------------
    // Debug - write a message to the debug LCD
    // ---------------------------------------------------------
    public void Debug(String str)
    {
        Debug(str, true);
    }
    public void Debug(String str, bool consoledbg)
    {
        if (debug && !inDebug)
        {
            inDebug = true;

            if (debugLCDs == null)
            {
                Echo("First run - working out debug panels");
                initializeDBGLCDs();
                ClearDebugLCDs();
            }

            Echo("D:" + str);
            jlcd.WriteToAllLCDs(debugLCDs, str + "\n", true);
            inDebug = false;
        }
    }

    // ---------------------------------------------------------
    // Debug - write a message to the debug LCD
    // ---------------------------------------------------------
    public void DebugAndEcho(String str)
    {
        Echo(str);          // Will always come out
        Debug(str, false);  // Will only come out if debug is on
    }

    // ---------------------------------------------------------
    // InitializeLCDs - cache the debug LCD
    // ---------------------------------------------------------
    private void initializeDBGLCDs()
    {
        inDebug = true;
        debugLCDs = jlcd.GetLCDsWithTag("DEBUG");
        jlcd.InitializeLCDs(debugLCDs, TextAlignment.LEFT);
        inDebug = false;
    }

    // ---------------------------------------------------------
    // ClearDebugLCDs - clear the debug LCD
    // ---------------------------------------------------------
    public void ClearDebugLCDs()
    {
        if (debug) {
            if (debugLCDs == null) {
                Echo("First runC - working out debug panels");
                initializeDBGLCDs();
            }
            jlcd.WriteToAllLCDs(debugLCDs, "", false);  // Clear the debug screens
        }
    }

    // ---------------------------------------------------------------------------
    // Simple wrapper to write to a central alert - Needs work to support wrapping
    // ---------------------------------------------------------------------------
    public void Alert(String alertMsg, String colour, String alertTag, String thisScript)
    {
        List<IMyTerminalBlock> allBlocksWithLCDs = new List<IMyTerminalBlock>();
        mypgm.GridTerminalSystem.GetBlocksOfType(allBlocksWithLCDs, (IMyTerminalBlock x) => (
                                                                                  (x.CustomName != null) &&
                                                                                  (x.CustomName.IndexOf("[" + alertTag + "]") >= 0) &&
                                                                                  (x is IMyTextSurfaceProvider)
                                                                                 ));
        DebugAndEcho("Found " + allBlocksWithLCDs.Count + " lcds with '" + alertTag + "' to alert to");

        String alertOutput = JLCD.solidcolor[colour] + " " +
                             DateTime.Now.ToShortTimeString() + ":" +
                             thisScript + " " +
                             alertMsg + "\n";
        DebugAndEcho("ALERT: " + alertMsg);
        if (allBlocksWithLCDs.Count > 0)
        {
            jlcd.WriteToAllLCDs(allBlocksWithLCDs, alertOutput, true);
        }
    }

    // ---------------------------------------------------------------------------
    // Useful for complex scripts to see how far through the cpu slice you are
    // ---------------------------------------------------------------------------
    public void EchoCurrentInstructionCount(String tag)
    {
        Echo(tag + " instruction count: " + mypgm.Runtime.CurrentInstructionCount + "," + mypgm.Runtime.CurrentCallChainDepth);
    }
    public void EchoMaxInstructionCount()
    {
        Echo("Max instruction count: " + mypgm.Runtime.MaxInstructionCount + "," + mypgm.Runtime.MaxCallChainDepth);
    }
}

public class JINV
{
    private JDBG jdbg = null;

    public enum BLUEPRINT_TYPES { BOTTLES, COMPONENTS, AMMO, TOOLS, OTHER, ORES };

    /* Ore to Ingot */
    Dictionary<String, String> oreToIngot = new Dictionary<String, String>
    {
        { "MyObjectBuilder_Ore/Cobalt", "MyObjectBuilder_Ingot/Cobalt" },
        { "MyObjectBuilder_Ore/Gold", "MyObjectBuilder_Ingot/Gold" },
        { "MyObjectBuilder_Ore/Stone", "MyObjectBuilder_Ingot/Stone" },
        { "MyObjectBuilder_Ore/Iron", "MyObjectBuilder_Ingot/Iron" },
        { "MyObjectBuilder_Ore/Magnesium", "MyObjectBuilder_Ingot/Magnesium" },
        { "MyObjectBuilder_Ore/Nickel", "MyObjectBuilder_Ingot/Nickel" },
        { "MyObjectBuilder_Ore/Platinum", "MyObjectBuilder_Ingot/Platinum" },
        { "MyObjectBuilder_Ore/Silicon", "MyObjectBuilder_Ingot/Silicon" },
        { "MyObjectBuilder_Ore/Silver", "MyObjectBuilder_Ingot/Silver" },
        { "MyObjectBuilder_Ore/Uranium", "MyObjectBuilder_Ingot/Uranium" },

        // Not needing these at present:
        //{ "MyObjectBuilder_Ore/Scrap", "MyObjectBuilder_Ingot/Scrap" },
        //{ "MyObjectBuilder_Ore/Ice", "?" },
        //{ "MyObjectBuilder_Ore/Organic", "?" },
    };

    /* Other stuff */
    Dictionary<String, String> otherCompToBlueprint = new Dictionary<String, String>
    {
        { "MyObjectBuilder_BlueprintDefinition/Position0040_Datapad", "MyObjectBuilder_Datapad/Datapad" },
    };

    /* Tools */
    Dictionary<String, String> toolsCompToBlueprint = new Dictionary<String, String>
    {
        { "MyObjectBuilder_PhysicalGunObject/AngleGrinderItem" , "MyObjectBuilder_BlueprintDefinition/Position0010_AngleGrinder" },
        { "MyObjectBuilder_PhysicalGunObject/AngleGrinder2Item", "MyObjectBuilder_BlueprintDefinition/Position0020_AngleGrinder2" },
        { "MyObjectBuilder_PhysicalGunObject/AngleGrinder3Item" , "MyObjectBuilder_BlueprintDefinition/Position0030_AngleGrinder3" },
        { "MyObjectBuilder_PhysicalGunObject/AngleGrinder4Item" , "MyObjectBuilder_BlueprintDefinition/Position0040_AngleGrinder4" },

        { "MyObjectBuilder_PhysicalGunObject/WelderItem" , "MyObjectBuilder_BlueprintDefinition/Position0090_Welder" },
        { "MyObjectBuilder_PhysicalGunObject/Welder2Item" , "MyObjectBuilder_BlueprintDefinition/Position0100_Welder2" },
        { "MyObjectBuilder_PhysicalGunObject/Welder3Item" , "MyObjectBuilder_BlueprintDefinition/Position0110_Welder3" },
        { "MyObjectBuilder_PhysicalGunObject/Welder4Item" , "MyObjectBuilder_BlueprintDefinition/Position0120_Welder4" },

        { "MyObjectBuilder_PhysicalGunObject/HandDrillItem", "MyObjectBuilder_BlueprintDefinition/Position0050_HandDrill" },
        { "MyObjectBuilder_PhysicalGunObject/HandDrill2Item", "MyObjectBuilder_BlueprintDefinition/Position0060_HandDrill2" },
        { "MyObjectBuilder_PhysicalGunObject/HandDrill3Item" , "MyObjectBuilder_BlueprintDefinition/Position0070_HandDrill3" },
        { "MyObjectBuilder_PhysicalGunObject/HandDrill4Item" , "MyObjectBuilder_BlueprintDefinition/Position0080_HandDrill4" },

    };

    /* Bottles */
    Dictionary<String, String> bottlesCompToBlueprint = new Dictionary<String, String>
    {
        { "MyObjectBuilder_GasContainerObject/HydrogenBottle", "MyObjectBuilder_BlueprintDefinition/Position0020_HydrogenBottle" },
        { "MyObjectBuilder_OxygenContainerObject/OxygenBottle", "MyObjectBuilder_BlueprintDefinition/HydrogenBottlesRefill" },
    };

    /* Components */
    Dictionary<String, String> componentsCompToBlueprint = new Dictionary<String, String>
    {
        { "myobjectbuilder_component/bulletproofglass", "myobjectbuilder_blueprintdefinition/bulletproofglass"},
        { "myobjectbuilder_component/canvas", "myobjectbuilder_blueprintdefinition/position0030_canvas"},
        { "myobjectbuilder_component/computer", "myobjectbuilder_blueprintdefinition/computercomponent"},
        { "myobjectbuilder_component/construction", "myobjectbuilder_blueprintdefinition/constructioncomponent"},
        { "myobjectbuilder_component/detector", "myobjectbuilder_blueprintdefinition/detectorcomponent"},
        { "myobjectbuilder_component/display", "myobjectbuilder_blueprintdefinition/display"},
        { "myobjectbuilder_component/explosives", "myobjectbuilder_blueprintdefinition/explosivescomponent"},
        { "myobjectbuilder_component/girder", "myobjectbuilder_blueprintdefinition/girdercomponent"},
        { "myobjectbuilder_component/gravitygenerator", "myobjectbuilder_blueprintdefinition/gravitygeneratorcomponent"},
        { "myobjectbuilder_component/interiorplate", "myobjectbuilder_blueprintdefinition/interiorplate"},
        { "myobjectbuilder_component/largetube", "myobjectbuilder_blueprintdefinition/largetube"},
        { "myobjectbuilder_component/medical", "myobjectbuilder_blueprintdefinition/medicalcomponent"},
        { "myobjectbuilder_component/metalgrid", "myobjectbuilder_blueprintdefinition/metalgrid"},
        { "myobjectbuilder_component/motor", "myobjectbuilder_blueprintdefinition/motorcomponent"},
        { "myobjectbuilder_component/powercell", "myobjectbuilder_blueprintdefinition/powercell"},
        { "myobjectbuilder_component/reactor", "myobjectbuilder_blueprintdefinition/reactorcomponent"},
        { "myobjectbuilder_component/radiocommunication", "myobjectbuilder_blueprintdefinition/radiocommunicationcomponent"},
        { "myobjectbuilder_component/smalltube", "myobjectbuilder_blueprintdefinition/smalltube"},
        { "myobjectbuilder_component/solarcell", "myobjectbuilder_blueprintdefinition/solarcell"},
        { "myobjectbuilder_component/steelplate", "myobjectbuilder_blueprintdefinition/steelplate"},
        { "myobjectbuilder_component/superconductor", "myobjectbuilder_blueprintdefinition/superconductor"},
        { "myobjectbuilder_component/thrust", "myobjectbuilder_blueprintdefinition/thrustcomponent"},
    };

    /* Ammo */
    Dictionary<String, String> ammoCompToBlueprint = new Dictionary<String, String>
    {
        /* Ammo */
        { /*"Gatling",*/            "MyObjectBuilder_AmmoMagazine/NATO_25x184mm", "MyObjectBuilder_BlueprintDefinition/Position0080_NATO_25x184mmMagazine" },
        { /*"Autocannon",*/         "MyObjectBuilder_AmmoMagazine/AutocannonClip", "MyObjectBuilder_BlueprintDefinition/Position0090_AutocannonClip" },
        { /*"Rocket",*/             "MyObjectBuilder_AmmoMagazine/Missile200mm", "MyObjectBuilder_BlueprintDefinition/Position0100_Missile200mm" },
        { /*"Assault_Cannon",*/     "MyObjectBuilder_AmmoMagazine/MediumCalibreAmmo", "MyObjectBuilder_BlueprintDefinition/Position0110_MediumCalibreAmmo" },
        { /*"Artillery",*/          "MyObjectBuilder_AmmoMagazine/LargeCalibreAmmo", "MyObjectBuilder_BlueprintDefinition/Position0120_LargeCalibreAmmo" },
        { /*"Small_Railgun",*/      "MyObjectBuilder_AmmoMagazine/SmallRailgunAmmo", "MyObjectBuilder_BlueprintDefinition/Position0130_SmallRailgunAmmo" },
        { /*"Large_Railgun",*/      "MyObjectBuilder_AmmoMagazine/LargeRailgunAmmo", "MyObjectBuilder_BlueprintDefinition/Position0140_LargeRailgunAmmo" },
        { /*"S-10_Pistol",*/        "MyObjectBuilder_AmmoMagazine/SemiAutoPistolMagazine", "MyObjectBuilder_BlueprintDefinition/Position0010_SemiAutoPistolMagazine" },
        { /*"S-10E_Pistol",*/       "MyObjectBuilder_AmmoMagazine/ElitePistolMagazine", "MyObjectBuilder_BlueprintDefinition/Position0030_ElitePistolMagazine" },
        { /*"S-20A_Pistol",*/       "MyObjectBuilder_AmmoMagazine/FullAutoPistolMagazine", "MyObjectBuilder_BlueprintDefinition/Position0020_FullAutoPistolMagazine" },
        { /*"MR-20_Rifle",*/        "MyObjectBuilder_AmmoMagazine/AutomaticRifleGun_Mag_20rd", "MyObjectBuilder_BlueprintDefinition/Position0040_AutomaticRifleGun_Mag_20rd" },
        { /*"MR-30E_Rifle",*/       "MyObjectBuilder_AmmoMagazine/UltimateAutomaticRifleGun_Mag_30rd", "MyObjectBuilder_BlueprintDefinition/Position0070_UltimateAutomaticRifleGun_Mag_30rd" },
        { /*"MR-50A_Rifle",*/       "MyObjectBuilder_AmmoMagazine/RapidFireAutomaticRifleGun_Mag_50rd", "MyObjectBuilder_BlueprintDefinition/Position0050_RapidFireAutomaticRifleGun_Mag_50rd" },
        { /*"MR-8P_Rifle",*/        "MyObjectBuilder_AmmoMagazine/PreciseAutomaticRifleGun_Mag_5rd", "MyObjectBuilder_BlueprintDefinition/Position0060_PreciseAutomaticRifleGun_Mag_5rd" },
        { /*"5.56x45mm NATO magazine",*/ "MyObjectBuilder_AmmoMagazine/NATO_5p56x45mm", null /* Not Craftable */ },
    };
    public JINV(JDBG dbg)
    {
        jdbg = dbg;
    }
    public void addBluePrints(BLUEPRINT_TYPES types, ref Dictionary<String, String> into)
    {
        switch (types)
        {
            case BLUEPRINT_TYPES.BOTTLES:
                into = into.Concat(bottlesCompToBlueprint).ToDictionary(x => x.Key, x => x.Value);
                break;
            case BLUEPRINT_TYPES.COMPONENTS:
                into = into.Concat(componentsCompToBlueprint).ToDictionary(x => x.Key, x => x.Value);
                break;
            case BLUEPRINT_TYPES.AMMO:
                into = into.Concat(ammoCompToBlueprint).ToDictionary(x => x.Key, x => x.Value);
                break;
            case BLUEPRINT_TYPES.TOOLS:
                into = into.Concat(toolsCompToBlueprint).ToDictionary(x => x.Key, x => x.Value);
                break;
            case BLUEPRINT_TYPES.OTHER:
                into = into.Concat(otherCompToBlueprint).ToDictionary(x => x.Key, x => x.Value);
                break;
            case BLUEPRINT_TYPES.ORES:
                into = into.Concat(oreToIngot).ToDictionary(x => x.Key, x => x.Value);
                break;
        }
    }
}

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