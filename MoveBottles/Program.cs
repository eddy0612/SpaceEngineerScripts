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
            jlcd.UpdateFullScreen(Me, thisScript);

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
                                                                                     (x.CustomName.ToUpper().IndexOf("[" + mytag.ToUpper() + "]") < 0)
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

        // ----------------------------- CUT -------------------------------------
    }
}
