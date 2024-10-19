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


namespace IngameScript
{
    partial class Program : MyGridProgram
    {

        // ----------------------------- CUT -------------------------------------
        String thisScript = "KeepAssemblersStocked";

        // Development or user config time flags
        bool debug = false;
        bool stayRunning = true;

        // Data read from program config
        String mytag = "IDONTCARE";    /* Which tanks to move things to [mytag] */
        int minore = 500;
        int maxore = 1000;
        bool allGrids = false;

        // Private variables
        private JDBG jdbg = null;
        private JINV jinv = null;
        private JLCD jlcd = null;
        private String alertTag = "alert";    // TODO: Could move into config

        // -------------------------------------------
        /* Example custom data in programming block: 
[Config]
; Move ingots to/from cargo with these names
tag=ingots
        */
        // -------------------------------------------

        // My configuration
        int refreshSpeed = 10;     // Only once a minute

        // Internals
        DateTime lastCheck = new DateTime(0);
        Dictionary<String, String> ore2ingots = new Dictionary<String, String>();

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
            ore2ingots = new Dictionary<String, String>();
            jinv.addBluePrints(JINV.BLUEPRINT_TYPES.ORES, ref ore2ingots);
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
                if (mytag != null) {
                    mytag = (mytag.Split(';')[0]).Trim();  // Remove any trailing comments
                    Echo("Using tag of " + mytag);
                } else {
                    Echo("No tag configured\nPlease add [config] for tag=<substring>");
                    return;
                }
                jdbg.Debug("Config: tag=" + mytag);

                // Get the required value of the "allgrids" key under the "config" section.
                String allgridsstr = _ini.Get("config", "allgrids").ToString();
                if (allgridsstr != null) {
                    allgridsstr = (allgridsstr.Split(';')[0]).Trim();  // Remove any trailing comments
                    if (allgridsstr.ToUpper().Equals("YES")) {
                        allGrids = true;
                    }

                    Echo("Using all grids? " + allGrids);
                } else {
                    Echo("No tag configured\nPlease add [config] for tag=<substring>");
                    return;
                }
                jdbg.Debug("Config: tag=" + mytag);

                // -----------------------------------------------------------------
                // Real work starts here
                // -----------------------------------------------------------------
                // Get all blocks with the tag [configured_tagname] to know where to move things from/to
                List<IMyTerminalBlock> allIngots = new List<IMyTerminalBlock>();
                GridTerminalSystem.GetBlocksOfType(allIngots, (IMyTerminalBlock x) => (
                                                                                     (x.CustomName.ToUpper().IndexOf("[LOCKED]") < 0) &&
                                                                                     (x.CustomName.ToUpper().IndexOf("[" + mytag.ToUpper() + "]") >= 0) &&
                                                                                     (allGrids || x.CubeGrid.Equals(Me.CubeGrid))
                                                                                      ));
                jdbg.Debug("Found " + allIngots.Count + " things tagged [" + mytag + "]");
                if (allIngots.Count == 0) {
                    jdbg.DebugAndEcho("Nothing tagged - Aborting");
                    return;
                }

                // Get all assemblers on the cubegrid, which we will keep stocked
                List<IMyTerminalBlock> allAssemblers = new List<IMyTerminalBlock>();
                GridTerminalSystem.GetBlocksOfType(allAssemblers, (IMyTerminalBlock x) => (
                                                                                     (x.CustomName.ToUpper().IndexOf("[LOCKED]") < 0) &&
                                                                                     (x is IMyAssembler) &&
                                                                                     (x.CustomName.ToUpper().IndexOf("[" + mytag.ToUpper() + "]") >= 0) &&
                                                                                     (allGrids || x.CubeGrid.Equals(Me.CubeGrid))
                                                                                      ));
                jdbg.Debug("Found " + allAssemblers.Count + " assemblers");
                if (allAssemblers.Count == 0) {
                    jdbg.DebugAndEcho("No assemblers found - Aborting");
                    return;
                }

                // Move all ingots from any non-assembler, non-reactor
                List<IMyTerminalBlock> allInventories = new List<IMyTerminalBlock>();
                GridTerminalSystem.GetBlocksOfType(allInventories, (IMyTerminalBlock x) => (x.HasInventory &&
                                                                                     (x.CustomName.ToUpper().IndexOf("[LOCKED]") < 0) &&
                                                                                     (x.CustomName.ToUpper().IndexOf("[" + mytag.ToUpper() + "]") < 0) &&
                                                                                     (allGrids || x.CubeGrid.Equals(Me.CubeGrid)) && 
                                                                                     (!((x is IMyAssembler) || (x is IMyReactor)))
                                                                                      ));
                jdbg.Debug("============================================================");
                jdbg.Debug("Moving ingots to central place");
                jdbg.Debug("============================================================");
                jdbg.Debug("Found " + allInventories.Count + " blocks with inventories to investigate");
                foreach (var thisblock in allInventories) {
                    if (thisblock.HasInventory) {
                        for (int invCount = 0; invCount < thisblock.InventoryCount; invCount++) {
                            List<MyInventoryItem> allItemsInInventory = new List<MyInventoryItem>();
                            IMyInventory inv = thisblock.GetInventory(invCount);
                            inv.GetItems(allItemsInInventory);

                            for (int j = 0; j < allItemsInInventory.Count; j++) {

                                String name = allItemsInInventory[j].Type.ToString();
                                //jdbg.Debug("inv: " + allItemsInInventory[j].Type.ToString() + " -> " + ore2ingots.Count());
                                if (ore2ingots.ContainsValue(name)) {
                                    jdbg.Debug("Found ingot " + name + " in " + thisblock.CustomName);
                                    moveTo(inv, allItemsInInventory[j], allIngots, thisblock);
                                }
                            }
                        }
                    }
                }

                jdbg.Debug("============================================================");
                jdbg.Debug("Filling assemblers");
                jdbg.Debug("============================================================");
                // Ok at this point all ingots are moved from any non-assembler, non-reactor, non-LOCKED inventory
                // to a central set of cargos... Now work through the assemblers
                foreach (var thisAss in allAssemblers) {

                    jdbg.Debug("Ass: " + thisAss.ToString());
                    // Sum the ingots here already - start with 0 of each
                    Dictionary<String, int> oresum = new Dictionary<String, int>();
                    foreach (KeyValuePair<String, String> entry in ore2ingots) {
                        oresum[entry.Value] = 0;
                    }

                    // Now add the ingots in the inventory
                    List<MyInventoryItem> allItemsInInventory = new List<MyInventoryItem>();
                    IMyInventory inv = thisAss.GetInventory(0);
                    inv.GetItems(allItemsInInventory);
                    for (int j = 0; j < allItemsInInventory.Count; j++) {

                        String name = allItemsInInventory[j].Type.ToString();
                        if (ore2ingots.ContainsValue(name)) {
                            // We have found ingots in the assembler - sum up how many
                            oresum[name] += allItemsInInventory[j].Amount.ToIntSafe();
                        }
                    }
                    foreach (KeyValuePair<String, String> entry in ore2ingots) {
                        jdbg.Debug("Total in assembler: " +entry.Value + "/" + entry.Key + " = "+ oresum[entry.Value]);
                    }

                    // So do we need more ingots moved into this assembler, or move them away?
                    foreach (KeyValuePair<String, int> entry in oresum) {
                        if (oresum[entry.Key] < minore) {
                            // move some here
                            int toMove = minore - oresum[entry.Key];
                            jdbg.Debug("We are " + toMove + " short of : " + entry.Key);


                            // Enumerate the cargos
                            foreach (var thisCargo in allIngots) {
                                if (thisCargo is IMyAssembler) continue;
                                jdbg.Debug("Checking " + thisCargo.CustomName);
                                List<MyInventoryItem> allItemsInCargo = new List<MyInventoryItem>();
                                IMyInventory cargoInv = thisCargo.GetInventory(0);
                                cargoInv.GetItems(allItemsInCargo);

                                for (int j = 0; (toMove > 0) && (j < allItemsInCargo.Count); j++) {
                                    String itemName = allItemsInCargo[j].Type.ToString();
                                    if (itemName.Contains(entry.Key)) {
                                        jdbg.Debug("This has " + entry.Key + " ammount " + allItemsInCargo[j].Amount.ToIntSafe());
                                        int toMoveThisTime = Math.Min(toMove, allItemsInCargo[j].Amount.ToIntSafe());
                                        toMove -= toMoveThisTime;
                                        jdbg.Debug("Moving " + toMoveThisTime);
                                        cargoInv.TransferItemTo(inv, allItemsInCargo[j], toMoveThisTime);
                                    }
                                }
                            }

                        } else if (oresum[entry.Key] > maxore) {
                            // move some from here
                            int toMove = oresum[entry.Key] - maxore;
                            jdbg.Debug("We have " + toMove + " too much of : " + oresum[entry.Key]);
                            for (int j = 0; (toMove > 0) && (j < allItemsInInventory.Count); j++) {
                                String itemName = allItemsInInventory[j].Type.ToString();
                                if (itemName.Contains(entry.Key)) {
                                    int toMoveThisTime = Math.Min(toMove, allItemsInInventory[j].Amount.ToIntSafe());
                                    toMove -= toMoveThisTime;
                                    moveTo(inv, allItemsInInventory[j], allIngots, thisAss);
                                }
                            }
                        }
                    }
                }
                jdbg.Alert("Completed", "GREEN", alertTag, thisScript); // This will echo and debug as well
            }
            catch (Exception ex) {
                jdbg.Alert("Exception - " + ex.ToString() + "\n" + ex.StackTrace, "RED", alertTag, thisScript);
            }
        }

        void moveTo(IMyInventory inv, MyInventoryItem item, List<IMyTerminalBlock> into, IMyTerminalBlock from)
        {
            /* Find best fit - do non assemblers first, otherwise we can fill them with the wrong stuff */
            for (int stage = 0; stage < 2; stage++) {
                foreach (var thisPlace in into) {
                    if ((stage == 0) && (thisPlace is IMyAssembler)) continue;
                    if ((stage == 1) && !(thisPlace is IMyAssembler)) continue;

                    IMyInventory destInv = thisPlace.GetInventory(0);
                    if (!inv.CanTransferItemTo(destInv, item.Type)) {
                        jdbg.Debug("- No conveyor to " + thisPlace.CustomName + " from " + from.DisplayName);
                        continue;
                    }

                    // See if enough space
                    MyFixedPoint curVol = destInv.CurrentVolume;
                    MyFixedPoint maxVol = destInv.MaxVolume;
                    jdbg.Debug("Moving to " + thisPlace.CustomName + " cur:" + curVol + "max:" + maxVol);

                    if (inv.TransferItemTo(destInv, item)) {
                        MyFixedPoint newVol = destInv.CurrentVolume;
                        if (!newVol.Equals(curVol)) {
                            jdbg.Debug("Moved ok, new vol: " + newVol);
                            stage = 3;
                        } else {
                            jdbg.Debug("Move failed - assume out of space");
                            continue;
                        }
                        break;
                    } else {
                        jdbg.Debug("Failed to move - try another");
                        continue;
                    }
                }
            }
        }

        // ----------------------------- CUT -------------------------------------
    }
}
