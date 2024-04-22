/*
 * Current Ore Status Display
 * ==========================
 * This script displays on any tagged LCD a report on the current ore, ingot, H2 and O2 status
 * 
 * Note: This script only looks for anything with an inventory on the same grid as the programmable
 * block. (This could be changed by removing the lines mentioning CubeGrid)
 * 
 * Source available via https://github.com/eddy0612/SpaceEngineerScripts
 * 
 * Installation Instructions
 * =========================
 * 1. Set the programmable block customdata to identify a tag of which lcds to output this data to,
 * and some control over the format and speed of updates. (See below for example)
 * 
 * 2. Any LCD you wish this to be displayed on, add the tag using the LCD identifier from the 
 * custom data, eg "[OreStat] LCD"
 * 
 * 3. Add script to the programmable block, recompile and run.
 * 
 * Example custom data - Display long form on all LCDs tagged [OreStat] and update every 10 seconds
 * ===================
 * 
 * [Config]
 * LCD=OreStat
 * refreshSpeed=10
 * format=long
 * 
 * Server Impact
 * =============
 * - Very minimal... only checks once every eg. 10 seconds
 */


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
                                                                                  (x.CustomName.ToUpper().IndexOf("[" + alertTag.ToUpper() + "]") >= 0) &&
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
        { "MyObjectBuilder_Component/BulletproofGlass", "MyObjectBuilder_BlueprintDefinition/BulletproofGlass"},
        { "MyObjectBuilder_Component/Canvas", "MyObjectBuilder_BlueprintDefinition/Position0030_Canvas"},
        { "MyObjectBuilder_Component/Computer", "MyObjectBuilder_BlueprintDefinition/ComputerComponent"},
        { "MyObjectBuilder_Component/Construction", "MyObjectBuilder_BlueprintDefinition/ConstructionComponent"},
        { "MyObjectBuilder_Component/Detector", "MyObjectBuilder_BlueprintDefinition/DetectorComponent"},
        { "MyObjectBuilder_Component/Display", "MyObjectBuilder_BlueprintDefinition/Display"},
        { "MyObjectBuilder_Component/Explosives", "MyObjectBuilder_BlueprintDefinition/ExplosivesComponent"},
        { "MyObjectBuilder_Component/Girder", "MyObjectBuilder_BlueprintDefinition/GirderComponent"},
        { "MyObjectBuilder_Component/GravityGenerator", "MyObjectBuilder_BlueprintDefinition/GravityGeneratorComponent"},
        { "MyObjectBuilder_Component/InteriorPlate", "MyObjectBuilder_BlueprintDefinition/InteriorPlate"},
        { "MyObjectBuilder_Component/LargeTube", "MyObjectBuilder_BlueprintDefinition/LargeTube"},
        { "MyObjectBuilder_Component/Medical", "MyObjectBuilder_BlueprintDefinition/MedicalComponent"},
        { "MyObjectBuilder_Component/MetalGrid", "MyObjectBuilder_BlueprintDefinition/MetalGrid"},
        { "MyObjectBuilder_Component/Motor", "MyObjectBuilder_BlueprintDefinition/MotorComponent"},
        { "MyObjectBuilder_Component/PowerCell", "MyObjectBuilder_BlueprintDefinition/PowerCell"},
        { "MyObjectBuilder_Component/Reactor", "MyObjectBuilder_BlueprintDefinition/ReactorComponent"},
        { "MyObjectBuilder_Component/RadioCommunication", "MyObjectBuilder_BlueprintDefinition/RadioCommunicationComponent"},
        { "MyObjectBuilder_Component/SmallTube", "MyObjectBuilder_BlueprintDefinition/SmallTube"},
        { "MyObjectBuilder_Component/SolarCell", "MyObjectBuilder_BlueprintDefinition/SolarCell"},
        { "MyObjectBuilder_Component/SteelPlate", "MyObjectBuilder_BlueprintDefinition/SteelPlate"},
        { "MyObjectBuilder_Component/Superconductor", "MyObjectBuilder_BlueprintDefinition/Superconductor"},
        { "MyObjectBuilder_Component/Thrust", "MyObjectBuilder_BlueprintDefinition/ThrustComponent"},
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
    public const char COLOUR_YELLOW = '';
    public const char COLOUR_RED = '';
    public const char COLOUR_ORANGE = '';
    public const char COLOUR_GREEN = '';
    public const char COLOUR_CYAN = '';
    public const char COLOUR_PURPLE = '';
    public const char COLOUR_BLUE = '';
    public const char COLOUR_WHITE = '';
    public const char COLOUR_BLACK = '';
    public const char COLOUR_GREY = '';

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
        _SetupFontCalc(allLCDs, ref rows, cols, mostlySpecial, 0.05F, 0.05F);
    }
    public int SetupFontWidthOnly(List<IMyTerminalBlock> allLCDs, int cols, bool mostlySpecial)
    {
        int rows = -1;
        _SetupFontCalc(allLCDs, ref rows, cols, mostlySpecial, 0.05F, 0.05F);
        return rows;
    }
    public void SetupFontCustom(List<IMyTerminalBlock> allLCDs, int rows, int cols, bool mostlySpecial, float size, float incr)
    {
        _SetupFontCalc(allLCDs, ref rows, cols, mostlySpecial, size,incr);
    }

    private void _SetupFontCalc(List<IMyTerminalBlock> allLCDs, ref int rows, int cols, bool mostlySpecial, float startSize, float startIncr)
    {
        int bestRows = rows;
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

                if ((thisSize.X < actualSize.X) && (rows == -1 || (displayrows > rows)))
                {
                    size += incr;
                    bestRows = displayrows;
                }
                else
                {
                    break;
                }
            }
            thisSurface.FontSize = size - incr;
            jdbg.Debug("Calc size of " + thisSurface.FontSize);

            /* If we were asked how many rows for given width, return it */
            if (rows == -1) rows = bestRows;

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