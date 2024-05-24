using EmptyKeys.UserInterface.Generated.DataTemplatesStoreBlock_Bindings;
using Sandbox.Engine.Platform;
using Sandbox.Game.EntityComponents;
using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;
using SpaceEngineers.Game.ModAPI.Ingame;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
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

// TODO: If custom data missing item section, add it but default all to 0
// TODO: Default to 10% if 2nd number missing
// TODO: Use JINV for blueprints
// TODO: Use JLCD for screen interactions, font sizes etc
// TODO: Default batch size to 10%
// TODO: Only look on current grid

namespace IngameScript
{
    partial class Program : MyGridProgram
    {

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
            jlcd.UpdateFullScreen(Me, thisScript);

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

        // ----------------------------- CUT -------------------------------------
    }
}
