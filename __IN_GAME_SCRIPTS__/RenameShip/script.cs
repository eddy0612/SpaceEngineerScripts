/*
 * RenameShip
 * ==========
 * This is a useful utility to help idenitfy which block comes from where by providing a docking port, and when a ship docks to it you can
 * select a prefix which is then applied to all blocks connected to that docking port. 
 * 
 * In effect, you would dock a ship, select the prefix on the buttons, then apply the prefix and all blocks on that ship get renamed
 * 
 * Source available via https://github.com/eddy0612/SpaceEngineerScripts
 * 
 * Instructions
 * ------------
 * 1. Add a programmable block, add config similar to the following
 * ```
 * [Config]
 * connector=[rename] Connector
 * 4buttonpanel=Rename 4 button[renamelcd]
 * dockedlcd = [renamelcd] Docked LCD
 * ```
 * 
 * 2. Add a connector and give it the exact name listed in the config
 * 3. Add an LCD where the name of the ship currently docked will be displayed - Must have the exact name listed in the config
 * 4. Add a 4 button panel which is where you will set the prefix and say go... 
 * 5. Add the script to the programmable block, click recompile/run
 * 
 * Usage
 * -----
 * Dock a ship - if the connector of the ship already has a prefix, that will be displayed on the LCD
 * and the text over the 4 button pannel will display it, otherwise it will display unknown and the buttons
 * start blank
 * 
 * Use the buttons to select an up to 3 character prefix. Press a button rotates that letter by one - if you need
 * to go backwards you have to wait for it to fully rotate!
 * 
 * Once you have the buttons showing the prefix you want, press the 4th button and it will apply it - EVERY block in the grid connected
 * to the connector will be renamed
 */

// ----------------------------- CUT -------------------------------------
String thisScript = "RenameShip";

// Development time flags
bool debug = false;
bool stayRunning = true;

// Private variables
private JDBG jdbg = null;
private JINV jinv = null;
private JLCD jlcd = null;
private JCTRL jctrl = null;
private String alertTag = "alert";    // TODO: Could move into config

// Example custom data in programming block:
// [Config]
// tag=rename

// Data read from program config
String mytag = "IDONTCARE";    /* Which LCDs to display status on */

// My configuration
int refreshSpeed = 10;
String originalName = "";
bool isConfirm = false;

// Internals
DateTime lastCheck = new DateTime(0);

// ---------------------------------------------------------------------------
// Constructor
// ---------------------------------------------------------------------------
public Program()
{
    // Run every 100 ticks, but relies on internal check to only actually
    // perform on a defined frequency
    if (stayRunning) Runtime.UpdateFrequency = UpdateFrequency.Update100;

    // Initialize the utility classes
    jdbg = new JDBG(this, debug);
    jlcd = new JLCD(this, jdbg, true);
    jinv = new JINV(jdbg);
    jctrl = new JCTRL(this, jdbg, true);
    jlcd.UpdateFullScreen(Me, thisScript);
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
    // Simplify code slightly
    if (argument == null) argument = "";

    // ---------------------------------------------------------------------------
    // Decide whether to actually do anything, and update status every 10 seconds
    // ---------------------------------------------------------------------------
    if (stayRunning && argument.Equals("")) {
        TimeSpan timeSinceLastCheck = DateTime.Now - lastCheck;
        if (timeSinceLastCheck.TotalSeconds >= refreshSpeed) {
            lastCheck = DateTime.Now;
        } else {
            return;  // Nothing to do
        }
    }

    try {
        // ---------------------------------------------------------------------------
        // We only get here if we are refreshing
        // ---------------------------------------------------------------------------
        // Dummy debug - so we do the lookup of lcds
        jdbg.ClearDebugLCDs();  // Clear the debug screens

        jdbg.DebugAndEcho(thisScript + " running at " + DateTime.Now.ToShortTimeString() + " with arg: " + argument);

        // ---------------------------------------------------------------------------
        // Get my custom data and parse to get the config
        // ---------------------------------------------------------------------------
        MyIniParseResult result;
        MyIni _ini = new MyIni();
        if (!_ini.TryParse(Me.CustomData, out result))
            throw new Exception(result.ToString());

        // Get the required value of the "tag" key under the "config" section.
        mytag = _ini.Get("config", "tag").ToString();
        if (mytag != null) {
            mytag = (mytag.Split(';')[0]).Trim().ToUpper();  // Remove any trailing comments
            Echo("Using tag of " + mytag);
        } else {
            Echo("No tag configured\nPlease add [config] for tag=<substring>");
            return;
        }
        jdbg.Debug("Config: tag=" + mytag);

        // -----------------------------------
        // Identify the connector to work with
        // -----------------------------------
        IMyShipConnector myShipConnector = null;
        List<IMyTerminalBlock> connectors = new List<IMyTerminalBlock>();
        GridTerminalSystem.GetBlocksOfType(connectors, (IMyTerminalBlock x) => (
                                                                               (x is IMyShipConnector) &&
                                                                               (x.CustomName.ToUpper().IndexOf("[" + mytag.ToUpper() + "]") >= 0) &&
                                                                               (x.CubeGrid.Equals(Me.CubeGrid))
                                                                              ));
        jdbg.Debug("Found " + connectors.Count + " connectors with the tag");
        if (connectors.Count == 0) {
            jdbg.DebugAndEcho("ERROR: No connector with the tag [" + mytag + "]");
            return;
        } else if (connectors.Count > 1) {
            jdbg.DebugAndEcho("ERROR: Too many connectors with the tag [" + mytag + "]");
            return;
        } else {
            jdbg.DebugAndEcho("Using connector " + connectors[0].CustomName);
            myShipConnector = connectors[0] as IMyShipConnector;
        }

        // -------------------------------------------
        // Identify the lcd to update with docked info
        // -------------------------------------------
        List<IMyTerminalBlock> myDockedStatusLCDs = jlcd.GetLCDsWithTag(mytag);
        if (myDockedStatusLCDs.Count == 0) {
            jdbg.DebugAndEcho("Warning LCD with tag '" + mytag + "' does not exist, so no status display possible");
        }

        // -------------------------------------------
        // Identify the 4 button panel to use for input
        // -------------------------------------------
        IMyTerminalBlock my4ButtonPanel = null;
        List<IMyTerminalBlock> but4panels = new List<IMyTerminalBlock>();
        GridTerminalSystem.GetBlocksOfType(but4panels, (IMyTerminalBlock x) => (
                                                                               (x is IMyTextSurfaceProvider) &&
                                                                               (x.CustomName.ToUpper().IndexOf("[" + mytag.ToUpper() + "]") >= 0) &&
                                                                               (x.CubeGrid.Equals(Me.CubeGrid)) &&
                                                                               (((IMyTextSurfaceProvider)x).SurfaceCount == 4)
                                                                              ));
        jdbg.Debug("Found " + but4panels.Count + " 4 button panels with the tag");
        if (but4panels.Count == 0) {
            jdbg.DebugAndEcho("ERROR: No 4 button panel with the tag [" + mytag + "]");
            return;
        } else if (but4panels.Count > 1) {
            jdbg.DebugAndEcho("ERROR: Too many 4 button panels with the tag [" + mytag + "]");
            return;
        } else {
            jdbg.DebugAndEcho("Using 4 button panel " + but4panels[0].CustomName);
            my4ButtonPanel = but4panels[0] as IMyTerminalBlock;
        }

        // --------------------------------------------------------------
        // Remove the connector and the 4 button panel from the LCD list!
        // --------------------------------------------------------------
        jdbg.Debug("LCD Count before: " + myDockedStatusLCDs.Count);
        myDockedStatusLCDs.Remove(my4ButtonPanel);
        jdbg.Debug("LCD Count mid: " + myDockedStatusLCDs.Count);
        myDockedStatusLCDs.Remove(myShipConnector);
        jdbg.Debug("LCD Count after: " + myDockedStatusLCDs.Count);

        // ----------------------------------------------------------
        // Identify the divider syntax to use (Expected: [] {} or  .)
        // ----------------------------------------------------------
        String divName = _ini.Get("config", "divider").ToString();
        if ((divName != null) && (!divName.Equals(""))) divName = " .";

        if (divName.Length != 2) {
            jdbg.DebugAndEcho("Invalid divider - must be 2 'unique' chars");
            return;
        }

        Char leftDiv = divName[0];
        Char rightDiv = divName[1];

        // ---------------------------------------------------------------------------
        // vvv                   Now do the actual work                           vvvv
        // ---------------------------------------------------------------------------

        jdbg.Debug("Doing the work now");
        // 1. See docked status has changed - if so update the LCD and connector data
        String newName = "---";

        if (!(myShipConnector.IsConnected)) {
            jdbg.Debug("Nothing connected");
            originalName = "";
            isConfirm = false;

            // To save processing time, just wipe the customData
            myShipConnector.CustomData = "";
            jdbg.Debug("Updating screens");
            updateStatusLCD(myDockedStatusLCDs, "Nothing Connected");
            update4But("---", my4ButtonPanel, isConfirm, true);
            jdbg.DebugAndEcho("Nothing connected - ending");
            return;
        } else {

            // If no custom data, set it
            if ((originalName.Equals("")) ||
                (myShipConnector.CustomData == null) ||
                (myShipConnector.Equals("")) ||
                (myShipConnector.CustomData.Length != 3)) {
                jdbg.Debug("No custom data yet, looking it up from the docked ship");
                newName = myShipConnector.OtherConnector.CustomName;
                if (newName.Length < 5) newName = newName + "     ";

                String nameLeft = newName;
                if (leftDiv != ' ' && leftDiv == newName[0]) {
                    nameLeft = newName.Substring(1);  // Strip first char
                }

                // If we match the right as well, we know the chars to use
                if (rightDiv == newName[3]) {
                    newName = newName.Substring(0, 3);
                    originalName = newName;
                } else {
                    newName = "---";
                    originalName = "???";
                }
                myShipConnector.CustomData = newName;
                jdbg.Debug("Calculated result of " + newName);
            } else {
                newName = myShipConnector.CustomData;
                jdbg.Debug("Read in custom data of " + newName);
            }

            jdbg.Debug("Updating screens");
            updateStatusLCD(myDockedStatusLCDs, "Docked Ship: " + originalName);
        }

        // Poll refresh - Just update the buttons
        if (argument.Equals("")) {
            jdbg.Debug("No Parms");
            update4But(newName, my4ButtonPanel, isConfirm, false);

            // Expected button press
        } else if (argument.StartsWith("B")) {
            isConfirm = false;
            jdbg.Debug("Handling button " + argument);
            int idx = argument[1] - '1';
            newName = handlePress(newName, idx);
            jdbg.Debug("Calc name of " + newName);
            update4But(newName, my4ButtonPanel, isConfirm, false);
            myShipConnector.CustomData = newName;
            jdbg.DebugAndEcho("Button handled... ending");
            return;
        } else if (argument.Equals("GO") && !isConfirm) {
            jdbg.Debug("Was GO");
            isConfirm = true;
            update4But(newName, my4ButtonPanel, isConfirm, false);
        } else if (argument.Equals("GO") && isConfirm) {
            jdbg.Debug("Was GO but confirming!");
            // Work out the CubeGrid of the connected ship
            IMyCubeGrid processGrid = myShipConnector.OtherConnector.CubeGrid;
            List<IMyTerminalBlock> blocks = new List<IMyTerminalBlock>();
            GridTerminalSystem.GetBlocksOfType(blocks, (IMyTerminalBlock x) => (
                                                                  x.CubeGrid.Equals(processGrid)
                                                                               ));

            int count = 0;
            foreach (var thisblock in blocks) {
                if (count < 5) jdbg.Debug("Processing block: " + thisblock.CustomName);

                String currentName = thisblock.CustomName;
                jdbg.Debug("Original: " + currentName);

                // Remove any old tag
                if ((leftDiv == ' ' || currentName.StartsWith("" + leftDiv)) && (currentName.IndexOf(rightDiv) >= 0)) {
                    currentName = currentName.Substring(currentName.IndexOf(rightDiv) + 1);
                    jdbg.Debug("Stripped: " + currentName);
                }

                if (argument != null && !newName.Equals("") && !newName.Equals("---")) {
                    if (leftDiv != ' ') {
                        currentName = "" + leftDiv + newName + rightDiv + currentName;
                    } else {
                        currentName = "" + newName + rightDiv + currentName;
                    }
                }
                jdbg.Debug("NewName: " + currentName);
                thisblock.CustomName = currentName;

                count++;
            }
            jdbg.Debug("Processed " + count + " blocks in total");

            originalName = newName;
            isConfirm = false;
            update4But(newName, my4ButtonPanel, isConfirm, false);
            updateStatusLCD(myDockedStatusLCDs, "Docked Ship: " + originalName);
        } else {
            jdbg.DebugAndEcho("ERROR: Unexpected arg: " + argument);
            return;
        }
        jdbg.Alert("Completed", "GREEN", alertTag, thisScript);
    }
    catch (Exception ex) {
        jdbg.Alert("Exception - " + ex.ToString() + "\n" + ex.StackTrace, "RED", alertTag, thisScript);
    }
}

/* Update the status LCDs */
void updateStatusLCD(List<IMyTerminalBlock> lcds, String text)
{
    jlcd.InitializeLCDs(lcds, TextAlignment.CENTER);
    jlcd.SetupFont(lcds, 1, text.Length + 4, false);  // 4 is just to provide some level of indentation
    jlcd.WriteToAllLCDs(lcds, text, false);
}

/* Update the button LCDs - Note this cannot use jlcd due to the multiple surfaces */
void update4But(String prefix, IMyTerminalBlock my4butPanels, bool isConfirm, bool isDisconnected)
{
    jdbg.Debug("Updating buttons... prefix: " + prefix + " confirm?" + isConfirm + ", isDisc?" + isDisconnected);
    jdbg.Debug("Panel: " + my4butPanels.CustomName);
    jdbg.Debug("Displays: " + ((IMyTextSurfaceProvider)my4butPanels).SurfaceCount);
    for (int i = 0; i < 3; i++) {
        IMyTextSurface thisLCD = ((IMyTextSurfaceProvider)my4butPanels).GetSurface(i);
        if (thisLCD != null) {
            jdbg.Debug("Updating surface " + i + " with " + prefix[i]);
            thisLCD.Font = "Monospace";
            thisLCD.ContentType = ContentType.TEXT_AND_IMAGE;
            thisLCD.Alignment = VRage.Game.GUI.TextPanel.TextAlignment.CENTER;
            thisLCD.FontSize = 10.0F;
            thisLCD.WriteText("" + prefix[i], false);
        } else {
            jdbg.Debug("Odd = 4 button panel doesnt have surface " + i);
        }

        // Now action button
        thisLCD = ((IMyTextSurfaceProvider)my4butPanels).GetSurface(3);
        if (thisLCD != null) {
            String text = "GO?";
            float fontsize = 9.0F;
            if (prefix.Equals("---")) {
                text = "Clear?";
                fontsize = 4.0F;
            }
            if (isConfirm) {
                text = "Really\nGO?";
                fontsize = 4.0F;
            }
            if (isDisconnected) text = "N/A";
            jdbg.Debug("Updating surface " + i + " with " + prefix[i]);
            thisLCD.Font = "Monospace";
            thisLCD.ContentType = ContentType.TEXT_AND_IMAGE;
            thisLCD.Alignment = VRage.Game.GUI.TextPanel.TextAlignment.CENTER;
            thisLCD.FontSize = fontsize;
            thisLCD.WriteText(text, false);
        }
    }
}

/* Handle a button being pressed */
String handlePress(String prefix, int index)
{
    String allChars = "ABDCEFGHIJKLMNOPQRSTUVWXYZ0123456789-";
    char[] prefixChars = prefix.ToCharArray();
    char whichChar = prefixChars[index];
    jdbg.Debug("orig char is " + prefixChars[index]);
    int idx = allChars.IndexOf(whichChar);

    if (idx == -1) {
        jdbg.DebugAndEcho("Unexpected data in customdata - ABORTING");
        return prefix;
    }

    idx++;
    if (idx == allChars.Length) idx = 0;
    prefixChars[index] = allChars[idx];
    jdbg.Debug("new char is " + prefixChars[index]);
    return new String(prefixChars);
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