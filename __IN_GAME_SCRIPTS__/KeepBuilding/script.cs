/*
 * KeepBuilding
 * ============
 * This is an extremely useful script which you can set running on a base, and based on configuration you provide, it will
 * ensure there is always a certain amount of each item type that is available pre-built. If you are low on items, it queues
 * up in batches to ensure you dont block up the assemblers. in addition it can output to an LCD the current status of
 * how it is coping.
 * 
 * Source available via https://github.com/eddy0612/SpaceEngineerScripts
 * 
 * Instructions
 * ------------
 * 1. Add a programmable block. In its custom data, add the following (but edit the numbers - see below)
 * 
 * ```
 * [Config]
 * ; Update any LCD/cargo with Name [saveitems]
 * tag=saveitems
 * ; Update the assembler to queue against with [qhere]
 * qtag=qhere
 * ; Update every 10 seconds
 * refreshSpeed=10
 * ; Bar length
 * barSize=20
 * ; Bar under the item line?
 * barUnder=false
 * ; item.x=component_name,total_wanted,queue_batch_size
 * item.1="BULLETPROOF GLASS",250,25
 * item.2="COMPUTER",1000,50
 * item.3="CONSTRUCTION COMP.",3000,100
 * item.4="DETECTOR COMP.",50,5
 * item.5="DISPLAY",250,25
 * item.6="EXPLOSIVES",50,5
 * item.7="GIRDER",2000,10011
 * item.8="GRAVITY COMP.",50,5
 * item.9="INTERIOR PLATE",3000,100
 * item.10="LARGE STEEL TUBES",1000,100
 * item.11="MEDICAL COMP.",50,5
 * item.12="METAL GRID",500,50
 * item.13="MOTOR",1000,50
 * item.14="POWER CELL",400,50
 * item.15="RADIO-COMM COMP.",50,5
 * item.16="REACTOR COMP",50,5
 * item.17="SMALL STEEL TUBE",3000,100
 * item.18="SOLAR CELL",250,25
 * item.19="STEEL PLATE",5000,200
 * item.20="SUPERCONDUCTOR",150,25
 * item.21="THRUSTER COMP.",250,25
 * ```
 * 
 * The item.x lines need to start from 1 and go upwards numerically but all items do not need to be listed. The two numbers
 * on the end of the item.x lines is the amount you want to ensure are available, and the batch size to queue up build requests
 * as.
 * 
 * 2. Tag the assembler you want things queued at with `[qhere]` - We usually have multiple assemblers where all but one are
 * configured to run in co-op mode, and the one that isnt is where you queue the items.
 * 
 * 3. On any LCD where you would like a graphical status of how the script is keeping up, add `[saveitems]` to the name
 * 
 * 4. Tag a cargo container where the produced items should all be moved into as `[saveitems]`
 */


// ----------------------------- CUT -------------------------------------
String thisScript = "KeepBuilding";

// Development time flags
bool debug = false;
bool stayRunning = true;

// Private variables
private JDBG jdbg = null;
private JINV jinv = null;
private JLCD jlcd = null;

// My configuration

// -------------------------------------------
/* Example custom data in programming block:
        // -------------------------------------------

// -------------------------------------------
// For assembling various components:
// -------------------------------------------
[Config]
; Update any LCD/cargo with Name [saveitems]
tag=saveitems
; Update the assembler to queue against with [qhere]
qtag=qhere
; Update every 10 seconds
refreshSpeed=10
; Bar length
barSize=20
; Bar under the item line?
barUnder=false
; item.x=component_name,total_wanted,queue_batch_size
item.1="BULLETPROOF GLASS",250,25
item.2="COMPUTER",1000,50
item.3="CONSTRUCTION COMP.",3000,100
item.4="DETECTOR COMP.",50,5
item.5="DISPLAY",250,25
item.6="EXPLOSIVES",50,5
item.7="GIRDER",2000,10011
item.8="GRAVITY COMP.",50,5
item.9="INTERIOR PLATE",3000,100
item.10="LARGE STEEL TUBES",1000,100
item.11="MEDICAL COMP.",50,5
item.12="METAL GRID",500,50
item.13="MOTOR",1000,50
item.14="POWER CELL",400,50
item.15="RADIO-COMM COMP.",50,5
item.16="REACTOR COMP",50,5
item.17="SMALL STEEL TUBE",3000,100
item.18="SOLAR CELL",250,25
item.19="STEEL PLATE",5000,200
item.20="SUPERCONDUCTOR",150,25
item.21="THRUSTER COMP.",250,25
                */

// Mapping from nice names to internal names
// Slight cheat as I also use it to go from blueprintname -> internal comp name as well
Dictionary<String, String> friendlyitemname = new Dictionary<String, String>
{
    { "BULLETPROOF GLASS",      "BulletproofGlass" },
    { "BULLETPROOFGLASS",       "BulletproofGlass" },
    { "CANVAS",                 "Canvas" },
    { "POSITION0030_CANVAS",    "Canvas" },
    { "COMPUTER",               "Computer" },
    { "COMPUTERCOMPONENT",      "Computer" },
    { "CONSTRUCTION",           "Construction"},
    { "CONSTRUCTION COMP.",     "Construction"},
    { "CONSTRUCTIONCOMPONENT",  "Construction"},
    { "DETECTOR COMP.",         "Detector" },
    { "DETECTOR",               "Detector" },
    { "DETECTORCOMPONENT",      "Detector" },
    { "DISPLAY",                "Display" },
    { "EXPLOSIVES",             "Explosives" },
    { "EXPLOSIVESCOMPONENT",    "Explosives" },
    { "GIRDER",                 "Girder" },
    { "GIRDERCOMPONENT",        "Girder" },
    { "GRAVITY",                "GravityGenerator" },
    { "GRAVITY COMP.",          "GravityGenerator" },
    { "GRAVITYCOMPONENT",       "GravityGenerator" },
    { "GRAVITYGENERATORCOMPONENT",  "GravityGenerator" },
    { "INTERIOR PLATE",         "InteriorPlate" },
    { "INTERIORPLATE",          "InteriorPlate" },
    { "LARGE STEEL TUBES",      "LargeTube" },
    { "LARGETUBE",              "LargeTube" },
    { "MEDICAL",                "Medical"},
    { "MEDICAL COMP.",          "Medical"},
    { "MEDICALCOMPONENT",       "Medical"},
    { "METAL GRID",             "MetalGrid"},
    { "METALGRID",              "MetalGrid"},
    { "MOTOR",                  "Motor" },
    { "MOTORCOMPONENT",         "Motor" },
    { "POWER CELL",             "PowerCell" },
    { "POWERCELL",              "PowerCell" },
    { "RADIO-COMM COMP.",       "RadioCommunication" },
    { "RADIOCOMMUNICATIONCOMPONENT",    "RadioCommunication" },
    { "REACTOR",                "Reactor" },
    { "REACTOR COMP",           "Reactor" },
    { "REACTOR COMP.",          "Reactor" },
    { "REACTORCOMPONENT",       "Reactor" },
    { "SMALL STEEL TUBE",       "SmallTube"},
    { "SMALLTUBE",              "SmallTube"},
    { "SOLAR CELL",             "SolarCell" },
    { "SOLARCELL",              "SolarCell" },
    { "STEEL PLATE",            "SteelPlate" },
    { "STEELPLATE",             "SteelPlate" },
    { "SUPERCONDUCTOR",         "Superconductor" },
    { "THRUSTER COMP.",         "Thrust" },
    { "THRUSTERCOMPONENT",      "Thrust" },
    { "THRUSTCOMPONENT",      "Thrust" },
};

Dictionary<String, String> compToBlueprint = new Dictionary<String, String>
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

Dictionary<String, char> solidcolor = new Dictionary<String, char>
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
};


// My configuration
MyIni _ini = new MyIni();
int refreshSpeed = 5;                       // Default to 5 seconds if not provided
int barSize = 15;
bool barUnder = false;
String mytag = "IDONTCARE";
String myqtag = "IDONTCARE";
String alerttag = "ALERT";

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
        jdbg.DebugAndEcho("Main Running..." + lastCheck.ToString());

        // ---------------------------------------------------------------------------
        // Get my custom data and parse to get the config
        // ---------------------------------------------------------------------------
        MyIniParseResult result;
        if (!_ini.TryParse(Me.CustomData, out result))
            throw new Exception(result.ToString());

        // Get the value of the "tag" key under the "config" section.
        mytag = _ini.Get("config", "tag").ToString();
        if (mytag != null)
        {
            mytag = (mytag.Split(';')[0]).Trim();
            Echo("Using tag of " + mytag);
        }
        else
        {
            Echo("No tag configured\nPlease add [config] for tag=<substring>");
            return;
        }

        // Get the value of the "qtag" key under the "config" section.
        myqtag = _ini.Get("config", "qtag").ToString();
        if (myqtag != null)
        {
            mytag = (mytag.Split(';')[0]).Trim();
            Echo("Using qtag of " + myqtag);
        }
        else
        {
            Echo("No qtag configured, will work it out instead");
        }

        // Get the value of the "refreshSpeed" key under the "config" section.
        int newrefreshSpeed = _ini.Get("config", "refreshSpeed").ToInt32();
        Echo("New refresh speed will be " + newrefreshSpeed);
        if (newrefreshSpeed < 1)
        {
            Echo("Invalid refresh speed or not defined - defaulting to 5 seconds");
            refreshSpeed = 5;
        }
        else
        {
            Echo("Refresh speed set to " + newrefreshSpeed + " sceconds");
            refreshSpeed = newrefreshSpeed;
        }

        // Get the value of the "refreshSpeed" key under the "config" section.
        int newBarSize = _ini.Get("config", "barSize").ToInt32();
        Echo("Bar Size will be " + newBarSize);
        if (newBarSize < 1)
        {
            Echo("Invalid bar size or not defined - defaulting to 25 seconds");
            barSize = 25;
        }
        else
        {
            Echo("Bar size set to " + barSize);
            barSize = newBarSize;
        }

        // Get the value of the "refreshSpeed" key under the "config" section.
        bool newBarUnder = _ini.Get("config", "barUnder").ToBoolean(false);
        Echo("Bar Under will be " + newBarUnder);
        barUnder = newBarUnder;

        // -----------------------------------------------------------------
        // Real work starts here
        // -----------------------------------------------------------------

        Dictionary<String, String> BlueprintToComp = new Dictionary<String, String>();

        // Look for all tagged blocks for this script
        bool moveMode = false;
        // Cargo containers to move to / populate
        List<IMyTerminalBlock> taggedBlocks = new List<IMyTerminalBlock>();
        GridTerminalSystem.GetBlocksOfType(taggedBlocks, (IMyTerminalBlock x) => (
                                                                                  (x.CustomName != null) &&
                                                                                  (x.CustomName.IndexOf("[" + mytag + "]") >= 0)
                                                                                 ));

        // Work out what we want to keep building
        Dictionary<String, int> itemExpected = new Dictionary<String, int>();
        Dictionary<String, int> itemBatch = new Dictionary<String, int>();
        Dictionary<String, int> itemBuilding = new Dictionary<String, int>();
        Dictionary<String, int> itemsFound = new Dictionary<String, int>();

        int itemcount = 0;
        bool finished = false;
        while (!finished)
        {
            String nextItem = _ini.Get("config", "item." + (itemcount + 1).ToString()).ToString();
            if (nextItem == null || nextItem.Equals("")) { finished = true; break; }
            jdbg.Debug("Found item " + (itemcount + 1).ToString() + " - " + nextItem);
            string[] parts = nextItem.Split(',');
            if (parts.Length != 3)
            {
                Echo("Invalid configuration for item." + (itemcount + 1).ToString() + " - Must have 3 components");
                return;
            }

            // Line is <item type>,<expected count>,<production batch size>

            // Strip quotes around item name
            parts[0] = parts[0].Trim('"');

            // Get maxItems
            int maxItems = -1;
            try
            {
                maxItems = Int32.Parse(parts[1]);
            }
            catch (FormatException)
            {
                Echo("Invalid configuration for item." + (itemcount + 1).ToString() + " - Num items not numeric");
                return;
            }
            if (maxItems <= 0) { maxItems = 0; }

            // Get batch Size - default to 10%
            int batchItems = -1;
            if (parts.Length > 2)
            {
                try
                {
                    batchItems = Int32.Parse(parts[2]);
                }
                catch (FormatException)
                {
                    Echo("Invalid configuration for item." + (itemcount + 1).ToString() + " - Batch size not numeric");
                    return;
                }
            } else
            {
                batchItems = (maxItems * 10) / 100;
            }
            if (batchItems <= 0) { batchItems = 0; }

            // Find type name to speed up later
            String typeName = getTypeName(parts[0]);
            if (typeName == null)
            {
                Echo("ERROR in config - program not support type " + parts[0] + " yet");
                jdbg.Alert("ERROR in config - program not support type " + parts[0] + " yet", "RED", alerttag, thisScript);
                return;
            }
            itemExpected[typeName] = maxItems;
            itemBatch[typeName] = batchItems;
            itemBuilding[typeName] = 0;
            itemsFound[typeName] = 0;
            jdbg.Debug("'" + parts[0] + "' : " + maxItems + "," + batchItems);

            itemcount++;
        }


        if (itemcount > 0)
        {
            Echo("Monitoring " + itemcount + " items");
        }
        else
        {
            Echo("Nothing configured to monitor");
            return;
        }

        // Do some debug output
        moveMode = false;

        foreach (var thisblock in taggedBlocks)
        {
            if (thisblock.HasInventory)
            {
                jdbg.Debug("Found " + thisblock.CustomName + " as cargo ");
                moveMode = true;
            }
            if (thisblock is IMyTextPanel)
            {
                jdbg.Debug("Found " + thisblock.CustomName + " as lcd ");
            }
        }

        if (moveMode)
        {
            Echo("Running in move mode");
        }
        else
        {
            Echo("Running in no-move mode");
        }

        // ---------------------------------------------------------------------------
        // vvv                   Now do the actual work                           vvvv
        // ---------------------------------------------------------------------------
        // Logic:
        //   Look through all the blocks with inventory for item types that we are monitoring
        //      Sum up number of all items
        //      In move mode, if they arent in [saveitems], move them
        //   Look through all the blocks with production queues
        //      Sum up number of all items in queue into separate list
        // For each configured item which is requested to be kept at a minimum
        //   If we have enough in storage, do nothing, otherwise kick in a batch to be built
        // Build and display on LCD
        int actionsCount = 0;

        // Cargo containers to move to / populate
        List<IMyTerminalBlock> allBlocks = new List<IMyTerminalBlock>();
        GridTerminalSystem.GetBlocksOfType(allBlocks, (IMyTerminalBlock x) => (x.HasInventory &&
                                                                               ((x.CustomName.ToUpper().IndexOf("[LOCKED]") < 0) &&
                                                                               (x.CustomName.ToUpper().IndexOf("[IGNORE]") < 0))
                                                                              ));

        //// Calculate current totals & move if necessary
        foreach (var thisblock in allBlocks)
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
                    IMyInventory destBlock = null;

                    for (int j = 0; j < allItemsInInventory.Count; j++)
                    {
                        String name = allItemsInInventory[j].Type.SubtypeId.ToString();

                        if (!itemExpected.ContainsKey(name))
                        {
                            //jdbg.Debug(".. not tracking : " + name);
                            continue;
                        }
                        jdbg.Debug("Found : '" + name + "' x " + allItemsInInventory[j].Amount.ToIntSafe());

                        bool move = moveMode;
                        foreach (var saveitemblock in taggedBlocks)
                        {
                            if (saveitemblock.HasInventory && saveitemblock == thisblock)
                            {
                                jdbg.Debug("Already in destination");
                                move = false;
                            }
                            else if (saveitemblock.HasInventory && !saveitemblock.GetInventory(0).IsFull)
                            {
                                destBlock = saveitemblock.GetInventory(0);
                            }
                        }

                        // Sum them up
                        if (itemsFound.ContainsKey(name))
                        {
                            itemsFound[name] += allItemsInInventory[j].Amount.ToIntSafe();
                        }
                        else
                        {
                            itemsFound[name] = allItemsInInventory[j].Amount.ToIntSafe();
                        }
                        if (move && itemExpected[name] > 0)
                        {
                            jdbg.Debug("Need to move this block! From " + thisblock.CustomName + " inv " + j);
                            inv.TransferItemTo(destBlock, allItemsInInventory[j]);
                            actionsCount++;
                        }

                    }
                }
            }

        }

        // Add on items in production queue
        var assemblers = new List<IMyAssembler>();
        GridTerminalSystem.GetBlocksOfType<IMyAssembler>(assemblers);
        IMyAssembler firstAssembler = null;
        jdbg.Debug("Working out assembler");
        foreach (var thisAss in assemblers)
        {
            // If we find the one we tag, use it
            if (thisAss.CustomName.IndexOf("[" + myqtag + "]") >= 0)
            {
                firstAssembler = thisAss;
            }
            else
            {
                // We ideally want an assembler which is not in cooperative mode (master) vs checked (slave)
                jdbg.Debug("Looking at " + thisAss.CustomName + " is coop?:" + thisAss.CooperativeMode);
                if (firstAssembler == null)
                {
                    jdbg.Debug("No choice, yet");
                }
                else
                {
                    jdbg.Debug("Current choice: " + firstAssembler.CustomName);
                }
                // Pick any assembler initially
                if (firstAssembler == null) firstAssembler = thisAss;
                if (firstAssembler.CooperativeMode && !thisAss.CooperativeMode) firstAssembler = thisAss;
            }

            // Now add up the items building
            var prodQ = new List<MyProductionItem>();
            thisAss.GetQueue(prodQ);
            foreach (var queued_item in prodQ)
            {
                var name = queued_item.BlueprintId.SubtypeId.ToString();
                var compname = getTypeName(name);
                jdbg.Debug("Found in q: " + compname + " : " + queued_item.Amount.ToIntSafe());
                if (!itemExpected.ContainsKey(compname))
                {
                    //jdbg.Debug(".. not tracking : " + compname );
                    continue;
                }
                itemBuilding[compname] += queued_item.Amount.ToIntSafe();
            }
        }

        String debugComp = null; // "Computer";
        if (firstAssembler != null)
        {
            ClearDisplay();
            AddDisplay("Components     " + DateTime.Now.ToString());
            AddDisplay("==========");
            bool row = false;
            foreach (var itemCount in itemsFound.OrderBy(p => p.Key))
            {
                String name = itemCount.Key;
                if (debugComp != null && name.Equals(debugComp)) debug = true;
                int count = itemCount.Value;
                int wanted = itemExpected[itemCount.Key];
                int batch = itemBatch[itemCount.Key];
                int queued = itemBuilding[itemCount.Key];

                if (queued >= batch)
                {
                    Echo("We have at least a batch queued of " + name);
                }
                else if ((count + queued) < wanted)
                {
                    jdbg.Debug("Total : " + name + " : " + count);
                    jdbg.Debug("Queued: " + queued);
                    jdbg.Debug("Wanted: " + wanted + " / " + batch);

                    int toQueue = wanted - (count + queued);
                    if (wanted > batch) toQueue = batch;
                    Echo("Qing up " + toQueue + "of " + name + " on " + firstAssembler.CustomName + " as we have " + count);
                    MyFixedPoint mfp = toQueue;
                    MyItemType component_type = new MyItemType("MyObjectBuilder_Component", name);
                    MyDefinitionId blueprint = MyDefinitionId.Parse(compToBlueprint[component_type.ToString()]);
                    if (blueprint == null)
                    {
                        Echo("ERROR: Cannot create blueprint " + name);
                        jdbg.Alert("ERROR: Cannot create blueprint " + name, "RED", alerttag, thisScript);
                        return;
                    }
                    firstAssembler.AddQueueItem(blueprint, mfp);
                    actionsCount++;
                    queued += mfp.ToIntSafe();
                }
                else
                {
                    Echo("We have enough " + name + " : " + count);
                }
                AddDisplay(GetLine(name, count, queued, wanted, row));
                row = !row;
                if (debugComp != null && name.Equals(debugComp)) debug = false;
            }
            DrawLCD(mytag, screen);
        }
        else
        {
            Echo("ERROR: No assembler found!");
            jdbg.Alert("ERROR: No assembler found", "RED", alerttag, thisScript);
            return;
        }
        jdbg.Alert("Completed - " + actionsCount + " actions performed", "GREEN", alerttag, thisScript );
    }
    catch (Exception ex)
    {
        jdbg.Alert("Exception - " + ex.ToString() + "\n" + ex.StackTrace, "RED", alerttag, thisScript);
    }
}

string GetLine(String name, int count, int queued, int wanted, bool alternate)
{
    String result = name.PadRight(20);

    if (barUnder) result = result + "\n" + "".PadRight(2);
    result = result + ">"; // Could be [ or >

    String bar = "".PadRight(barSize);
    char[] barchars = bar.ToCharArray();

    // Set limits, as count or queued or both may take us over the limit
    if (count > wanted) count = wanted;
    if ((count + queued) > wanted) queued = (wanted - count);

    double squaresize = (double)((double)wanted / (double)barSize);
    // Never round up completed, as 99/100 shouldnt show as a full bar
    // However we do end up with 0.9999R which is a pain, so allow 0.01 leeway
    int c_count = (int)Math.Floor((double)0.01 + (double)((double)count / (double)squaresize));

    // We can round up the queued ones, but need to ensure we dont go over the max
    int q_count = (int)Math.Round((double)0.01 + (double)((double)queued / (double)squaresize));
    if ((c_count + q_count) > barSize) q_count = q_count - 1;

    jdbg.Debug("getline: " + name + "," + count + "," + queued + "," + wanted + " = " + c_count + "/" + q_count + " sq: " + squaresize);
    jdbg.Debug("approx: " + (double)((double)count / (double)squaresize));

    var bari = 0;
    for (int i = 0; i < c_count; i++)
    {
        if (alternate) {
            barchars[bari] = jlcd.ColorToChar(0, 128, 128);
        } else {
            barchars[bari] = jlcd.ColorToChar(0, 255, 255);   //solidcolor["CYAN"];
        }
        bari++;
    }
    for (int i = 0; i < q_count; i++)
    {
        barchars[bari] = solidcolor["YELLOW"];
        bari++;
    }
    for (int i = bari; i < barSize; i++)
    {
        barchars[i] = solidcolor["RED"];
    }
    result = result + new string(barchars);
    result = result + "<"; // Could be ] or <
    return result;
}

// ---------------------------------------------------------------------------
// The following functions are for updating the screen
// ---------------------------------------------------------------------------
// Utility variables/function used when building the screen to display
int MAXCOLS = 80;
String screen = "";
void AddDisplay(String textToOutput)
{
    if (textToOutput.Length > MAXCOLS) MAXCOLS = textToOutput.Length;
    screen = screen + "\n" + textToOutput;
}

void ClearDisplay()
{
    screen = "";
    MAXCOLS = barSize + 10;
}

void DrawLCD(String tag, String screenContents)
{
    List<IMyTerminalBlock> drawLCDs = jlcd.GetLCDsWithTag(tag);
    jlcd.InitializeLCDs(drawLCDs, TextAlignment.LEFT);
    jlcd.SetupFont(drawLCDs, MAXCOLS, screenContents.Split('\n').Length + 1, true);
    jlcd.WriteToAllLCDs(drawLCDs, screenContents, false);
}

String getTypeName(String type)
{
    if (friendlyitemname.ContainsKey(type.ToUpper()))
    {
        return friendlyitemname[type.ToUpper()];
    }
    else
    {
        return "Unknown_" + type;
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