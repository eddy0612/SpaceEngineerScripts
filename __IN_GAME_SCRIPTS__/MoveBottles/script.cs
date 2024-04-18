/*
 * MoveBottles
 * -----------
 * 
 * This is a simple script which runs periodically and moves any oxygen or hydrogen bottles it can find over to a set of nominated oxygen or hydrogen 
 * tanks (or O2H2 Generator). Anything with an inventory with the tag [LOCKED] is ignored. Be aware as coded it will grab from anywhere it can and is
 * not filtered to the current grid. This would be a trivial change
 * 
 * 
 * Instructions
 * ------------
 * 
 * 1. Create a programmable block, name it something like `[MOVEBOTTLESPGM]`. Add custom data as follows - the value of the tag can be anything but you need to use it consistently everywhere as a prefix
 * 
 * ```
 * [config]
 * tag=MOVEBOTTLES
 * ```
 * 
 * 2. Idenitfy one or move of both oxygen and hydrogen tanks, or O2/H2 generators, and add to their name `[MOVEBOTTLES]`
 * 3. Add the script to the programmable block, recompile and run.
 */


// ----------------------------- CUT -------------------------------------
String thisScript = "MoveBottles";

// Development or user config time flags
bool debug = false;
bool stayRunning = true;

// Data read from program config
String mytag = "IDONTCARE";    /* Which tanks to move things to [mytag] */

// Private variables
private JDBG jdbg = null;
private JINV jinv = null;
private JLCD jlcd = null;
private String alertTag = "alert";    // TODO: Could move into config

private Dictionary<String, String> bottleComp2Blueprints = null;

// -------------------------------------------
/* Example custom data in programming block:
[Config]
; Move bottles to these tanks / generators
tag=refill
        */
// -------------------------------------------

// My configuration
int refreshSpeed = 60;     // Only once a minute

// Internals
DateTime lastCheck = new DateTime(0);

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
    if (stayRunning)
    {
        Runtime.UpdateFrequency = UpdateFrequency.Update100;
    }

    // Do one off initialization
    bottleComp2Blueprints = new Dictionary<String, String>();
    jinv.addBluePrints(JINV.BLUEPRINT_TYPES.BOTTLES, ref bottleComp2Blueprints);
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
    if (stayRunning)
    {
        TimeSpan timeSinceLastCheck = DateTime.Now - lastCheck;
        if (timeSinceLastCheck.TotalSeconds >= refreshSpeed)
        {
            lastCheck = DateTime.Now;
        }
        else
        {
            return;
        }
    }

    try
    {
        // ---------------------------------------------------------------------------
        // We only get here if we are refreshing
        // ---------------------------------------------------------------------------
        jdbg.ClearDebugLCDs();  // Clear the debug screens

        // ---------------------------------------------------------------------------
        // We only get here if we are refreshing
        // ---------------------------------------------------------------------------
        jdbg.DebugAndEcho("Main from " + thisScript + " running..." + lastCheck.ToString());

        // ---------------------------------------------------------------------------
        // Get my custom data and parse to get the config
        // ---------------------------------------------------------------------------
        MyIniParseResult result;
        MyIni _ini = new MyIni();
        if (!_ini.TryParse(Me.CustomData, out result))
            throw new Exception(result.ToString());

        // Get the required value of the "tag" key under the "config" section.
        mytag = _ini.Get("config", "tag").ToString();
        if (mytag != null)
        {
            mytag = (mytag.Split(';')[0]).Trim();  // Remove any trailing comments
            Echo("Using tag of " + mytag);
        }
        else
        {
            Echo("No tag configured\nPlease add [config] for tag=<substring>");
            return;
        }
        jdbg.Debug("Config: tag=" + mytag);

        // -----------------------------------------------------------------
        // Real work starts here
        // -----------------------------------------------------------------
        // Get all blocks with the tag [configured_tagname] (Ignoring [LOCKED])
        List<IMyTerminalBlock> allTanks = new List<IMyTerminalBlock>();
        GridTerminalSystem.GetBlocksOfType(allTanks, (IMyTerminalBlock x) => (
                                                                             (x.CustomName.ToUpper().IndexOf("[LOCKED]") < 0) &&
                                                                             (x.CustomName.ToUpper().IndexOf("[" + mytag.ToUpper() + "]") >= 0 ) &&
                                                                             ((x is IMyGasGenerator) || (x is IMyGasTank))
                                                                              ));
        jdbg.Debug("Found " + allTanks.Count + " tanks or h2o2 generators");
        if (allTanks.Count == 0) {
            jdbg.DebugAndEcho("No tanks found! Aborting");
            return;
        }

        // Searches all inventories and looks for hydrogen or oxygen bottles which do not have that tag and not locked
        List<IMyTerminalBlock> allInventories = new List<IMyTerminalBlock>();
        GridTerminalSystem.GetBlocksOfType(allInventories, (IMyTerminalBlock x) => (x.HasInventory &&
                                                                             (x.CustomName.ToUpper().IndexOf("[LOCKED]") < 0) &&
                                                                             (x.CustomName.ToUpper().IndexOf("[" + mytag + "]") < 0)
                                                                              ));
        jdbg.Debug("Found " + allInventories.Count + " blocks with inventories to investigate");
        int movedBottles = 0;
        foreach (var thisblock in allInventories)
        {
            // Sum up produced items
            if (thisblock.HasInventory)
            {
                //jdbg.Debug("Processing block: " + thisblock.CustomName);
                for (int invCount = 0; invCount < thisblock.InventoryCount; invCount++)
                {
                    List<MyInventoryItem> allItemsInInventory = new List<MyInventoryItem>();
                    IMyInventory inv = thisblock.GetInventory(invCount);
                    inv.GetItems(allItemsInInventory);

                    for (int j = 0; j < allItemsInInventory.Count; j++)
                    {
                        //jdbg.Debug("inv: " + allItemsInInventory[j].Type.ToString() + "," + bottleComp2Blueprints.Count);
                        String name = allItemsInInventory[j].Type.ToString();
                        if (bottleComp2Blueprints.ContainsKey(name))
                        {
                            jdbg.Debug("Found bottle " + name + " in " + thisblock.CustomName);

                            // For each found bottle:
                            //    If hydrogen - move ideally to hydrogen tank, if not to H2O2 generator
                            //    If oxygen - move ideally to hydrogen tank, if not to H2O2 generator

                            jdbg.Debug("Found item to transfer: " + name + " in " + thisblock.CustomName);

                            bool isOxygen = (name.Contains("xygen"));

                            Stack <IMyTerminalBlock>validTanks = new Stack <IMyTerminalBlock>();
                            Stack<IMyTerminalBlock> h2o2Tanks = new Stack<IMyTerminalBlock>();

                            /* Seperate all the tanks */
                            foreach (var thistank in allTanks)
                            {
                                /* Only interested in tanks that we could possibly move to */
                                if (!inv.IsConnectedTo(thistank.GetInventory(0))) continue;

                                /* Use a H2/O2 generator if we cannot find an oxygen or hydrogen tank */
                                if (thistank is IMyGasGenerator)
                                {
                                    h2o2Tanks.Push(thistank);
                                    ((IMyGasGenerator)thistank).AutoRefill = true;
                                }
                                else if (thistank is IMyGasTank)
                                {
                                    ((IMyGasTank)thistank).AutoRefillBottles = true;
                                    if ((isOxygen && !thistank.BlockDefinition.SubtypeId.Contains("Hydro")) ||
                                       (!isOxygen && thistank.BlockDefinition.SubtypeId.Contains("Hydro"))) {
                                        validTanks.Push(thistank);
                                    }
                                }
                            }
                            //jdbg.Debug("Options: " + validTanks.Count + "," + h2o2Tanks.Count);

                            /* Find best fit */
                            while ((validTanks.Count > 0) || (h2o2Tanks.Count > 0)) {
                                /* If we found a fit, then we will move it over and make sure it will autorefill */
                                IMyTerminalBlock destBlock = null;
                                if (validTanks.Count > 0) {
                                    destBlock = validTanks.Pop();
                                } else {
                                    destBlock = h2o2Tanks.Pop();
                                }

                                IMyInventory destInv = destBlock.GetInventory(0);

                                // See if enough space
                                MyFixedPoint curVol = destInv.CurrentVolume;
                                MyFixedPoint maxVol = destInv.MaxVolume;
                                jdbg.Debug("Moving to " + destBlock.CustomName + " cur:" + curVol + "max:" + maxVol);

                                if (inv.TransferItemTo(destInv, allItemsInInventory[j])) {
                                    MyFixedPoint newVol = destInv.CurrentVolume;
                                    if (!newVol.Equals(curVol)) {
                                        jdbg.Debug("Moved ok, new vol: " + newVol);
                                        movedBottles++;
                                    } else {
                                        jdbg.Debug("Move failed - assume out of space");
                                        continue;
                                    }
                                    break;
                                } else {
                                    jdbg.Debug("Filed to move - try another");
                                    continue;
                                }
                            }
                        }
                    }
                }
            }
        }

        jdbg.Alert("Completed - " + movedBottles + " bottles moved", "GREEN", alertTag, thisScript); // This will echo and debug as well
    }
    catch (Exception ex)
    {
        jdbg.Alert("Exception - " + ex.ToString() + "\n" + ex.StackTrace, "RED", alertTag, thisScript);
    }
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