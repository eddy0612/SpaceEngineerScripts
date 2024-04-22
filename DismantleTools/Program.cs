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


namespace IngameScript
{
    partial class Program : MyGridProgram
    {

        // ----------------------------- CUT -------------------------------------
        String thisScript = "DismantleTools";

        // Development or user config time flags
        bool debug = false;
        bool stayRunning = true;

        // Data read from program config
        int welder = -1;
        int grinder = -1;
        int handdrill = -1;
        String disasmtag = "IDONTCARE";    /* Which assembler to move them to in order to be dismantled */

        // Private variables
        private JDBG jdbg = null;
        private JINV jinv = null;
        private JLCD jlcd = null;
        private String alertTag = "alert";    // TODO: Could move into config

        private Dictionary<String, String> bottleTool2Blueprints = null;

        // -------------------------------------------
        /* Example custom data in programming block: 
[Config]
; Ask assembler to disassemble tools
; 0==ignore, 1..4 are each level of the normal,enhanced, proficient, elite etc
; Set to 0 or dont list things you dont want to dismantle
; Remember 1 == normal, 2 == ^, 3 == ^^ and 4 = ^^^
tag=disasm
welder=1
grinder=1
handdrill=2
        */
        // -------------------------------------------

        // My configuration
        int refreshSpeed = 60;     // Only once a minute

        // Internals
        DateTime lastCheck = new DateTime(0);
        IMyAssembler whichDisasm = null;

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

            // Do one off initialization
            bottleTool2Blueprints = new Dictionary<String, String>();
            jinv.addBluePrints(JINV.BLUEPRINT_TYPES.TOOLS, ref bottleTool2Blueprints);
            jdbg.Debug("comp2blueprints size " + bottleTool2Blueprints.Count);
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

                // ---------------------------------------------------------------------------
                // We only get here if we are refreshing
                // ---------------------------------------------------------------------------
                jdbg.DebugAndEcho("Main from " + thisScript + " running..." + DateTime.Now.ToString());

                // ---------------------------------------------------------------------------
                // Get my custom data and parse to get the config
                // ---------------------------------------------------------------------------
                MyIniParseResult result;
                MyIni _ini = new MyIni();
                if (!_ini.TryParse(Me.CustomData, out result))
                    throw new Exception(result.ToString());

                // Get the required value of the "tag" key under the "config" section.
                disasmtag = _ini.Get("config", "tag").ToString();
                if (disasmtag != null) {
                    disasmtag = (disasmtag.Split(';')[0]).Trim();  // Remove any trailing comments
                    Echo("Using tag of " + disasmtag);
                } else {
                    Echo("No tag configured\nPlease add [config] for tag=<substring>");
                    return;
                }
                jdbg.Debug("Config: tag=" + disasmtag);

                // Get the value of the "refreshSpeed" key under the "config" section.
                welder = _ini.Get("config", "welder").ToInt32(-1);
                Echo("Welder config : " + welder);
                if ((welder < -1) || (welder > 3)) {
                    // Allow -1 as internally meaning not to do
                    Echo("Invalid welder config, please set to 0,1,2 or 3");
                    return;
                }
                jdbg.Debug("Config: welder=" + welder);

                handdrill = _ini.Get("config", "handdrill").ToInt32(-1);
                Echo("Handdrill config : " + handdrill);
                if ((handdrill < -1) || (handdrill > 3)) {
                    // Allow -1 as internally meaning not to do
                    Echo("Invalid handdrill config, please set to 0,1,2 or 3");
                    return;
                }
                jdbg.Debug("Config: handdrill=" + handdrill);

                grinder = _ini.Get("config", "grinder").ToInt32(-1);
                Echo("Grinder config : " + grinder);
                if ((grinder < -1) || (grinder > 3)) {
                    // Allow -1 as internally meaning not to do
                    Echo("Invalid grinder config, please set to 0,1,2 or 3");
                    return;
                }
                jdbg.Debug("Config: grinder=" + grinder);

                // -----------------------------------------------------------------
                // Real work starts here
                // -----------------------------------------------------------------
                // Work out the assembler configured to disassemble (Ignore [LOCKED])
                whichDisasm = null;
                List<IMyTerminalBlock> allAssemblers = new List<IMyTerminalBlock>();
                GridTerminalSystem.GetBlocksOfType(allAssemblers, (IMyTerminalBlock x) => (
                                                                                     (x.CustomName.ToUpper().IndexOf("[LOCKED]") < 0) &&
                                                                                     (x.CustomName.ToUpper().IndexOf("[" + disasmtag.ToUpper() + "]") >= 0) &&
                                                                                     (x is IMyAssembler)
                                                                                          ));
                jdbg.Debug("Found " + allAssemblers.Count + " assemblers - checking for one which is in disasm mode");
                if (allAssemblers.Count == 0) {
                    jdbg.DebugAndEcho("No tagged assemblers found! Aborting");
                    return;
                }

                foreach (var thisBlock in allAssemblers) {
                    IMyAssembler thisAss = (IMyAssembler)thisBlock;
                    if (thisAss.Mode == MyAssemblerMode.Disassembly) {
                        whichDisasm = thisAss;
                        break;
                    }
                }
                if (whichDisasm == null) {
                    jdbg.DebugAndEcho("Tagged assemblers not in disasm mode! Aborting");
                    return;
                }

                // Searches all inventories and looks for tools
                List<IMyTerminalBlock> allInventories = new List<IMyTerminalBlock>();
                GridTerminalSystem.GetBlocksOfType(allInventories, (IMyTerminalBlock x) => (x.HasInventory &&
                                                                                     (x.CustomName.ToUpper().IndexOf("[LOCKED]") < 0) &&
                                                                                     (x.CustomName.ToUpper().IndexOf("[" + disasmtag.ToUpper() + "]") < 0)
                                                                                      ));
                jdbg.Debug("Found " + allInventories.Count + " blocks with inventories to investigate");
                
                int movedItems = 0;
                foreach (var thisblock in allInventories) {
                    // Sum up produced items
                    if (thisblock.HasInventory) {
                        //jdbg.Debug("Processing block: " + thisblock.CustomName);
                        for (int invCount = 0; invCount < thisblock.InventoryCount; invCount++) {
                            List<MyInventoryItem> allItemsInInventory = new List<MyInventoryItem>();
                            IMyInventory inv = thisblock.GetInventory(invCount);
                            inv.GetItems(allItemsInInventory);
                            if (!inv.IsConnectedTo(whichDisasm.GetInventory())) continue;  /* No point looking if we cant move it there */

                            for (int j = 0; j < allItemsInInventory.Count; j++) {
                                String name = allItemsInInventory[j].Type.ToString();

                                int max = -1;
                                String prefix = "???";
                                if (name.Contains("MyObjectBuilder_PhysicalGunObject/Welder")) {
                                    prefix = "Welder";
                                    max = welder;
                                    jdbg.Debug("Found welder " + name + " max is now " + max);
                                } else if (name.Contains("MyObjectBuilder_PhysicalGunObject/HandDrill")) {
                                    prefix = "HandDrill";
                                    max = handdrill;
                                } else if (name.Contains("MyObjectBuilder_PhysicalGunObject/AngleGrinder")) {
                                    prefix = "AngleGrinder";
                                    max = grinder;
                                }

                                bool moveIt = false;
                                for (int i = 0; !moveIt && (i <= max); i++) {
                                    if ((i == 0) && (name.Contains(prefix + "Item"))) moveIt = true;
                                    else if (name.Contains(prefix + welder + "Item")) moveIt = true;
                                }

                                if (moveIt) {
                                    jdbg.Debug("Moving " + name + " to " + whichDisasm.CustomName);

                                    if (whichDisasm.GetId() != thisblock.GetId()) {
                                        if (inv.TransferItemTo(whichDisasm.OutputInventory, allItemsInInventory[j])) {
                                            jdbg.Debug("Transferred ok");
                                        } else {
                                            jdbg.Debug("Failed to transfer?");
                                        }
                                    }
                                    MyFixedPoint moveOne = new MyFixedPoint();
                                    moveOne = 1;
                                    whichDisasm.AddQueueItem(MyDefinitionId.Parse(bottleTool2Blueprints[name]), moveOne);
                                    movedItems++;
                                }
                            }
                        }
                    }
                }

                jdbg.Alert("Completed - " + movedItems + " tools moved to be disassembled", "GREEN", alertTag, thisScript); // This will echo and debug as well
            }
            catch (Exception ex) {
                jdbg.Alert("Exception - " + ex.ToString() + "\n" + ex.StackTrace, "RED", alertTag, thisScript);
            }
        }

        // ----------------------------- CUT -------------------------------------
    }
}
