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
        String thisScript = "ReStacker";

        // Development time flags
        bool debug = false;
        bool stayRunning = true;

        // Private variables
        private JDBG jdbg = null;
        private JINV jinv = null;
        private JLCD jlcd = null;

        // -------------------------------------------
        /* Example custom data in programming block: 
               None
        */
        // -------------------------------------------

        // -------------------------------------------
        // Table of component name -> blueprintid
        // -------------------------------------------

        // -------------------------------------------
        Dictionary<String, String> compToBlueprint = new Dictionary<String, String> { };

        // My configuration
        String alerttag = "ALERT"; // Wanr to alert LCDs
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

            // Grab a list of what we can restack
            jinv.addBluePrints(JINV.BLUEPRINT_TYPES.COMPONENTS, ref compToBlueprint);
            jinv.addBluePrints(JINV.BLUEPRINT_TYPES.AMMO, ref compToBlueprint);
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
                // Dummy debug - so we do the lookup of lcds
                jdbg.ClearDebugLCDs();

                // ---------------------------------------------------------------------------
                // We only get here if we are refreshing
                // ---------------------------------------------------------------------------
                jdbg.DebugAndEcho("Main from " + thisScript + " running..." + DateTime.Now.ToString());

                // ---------------------------------------------------------------------------
                // Get my custom data and parse to get the config - NoOP
                // ---------------------------------------------------------------------------

                // -----------------------------------------------------------------
                // Real work starts here
                // -----------------------------------------------------------------

                // Get a list of all cargo containers 
                List<IMyTerminalBlock> allBlocks = new List<IMyTerminalBlock>();
                GridTerminalSystem.GetBlocksOfType(allBlocks, (IMyTerminalBlock x) => (x.HasInventory &&
                                                                                       (x.CustomName.ToUpper().IndexOf("[LOCKED]") < 0) &&
                                                                                       !(x is IMyRefinery)
                                                                                      ));
                jdbg.Debug("Found " + allBlocks.Count + " blocks with inventories to work through");

                //// Calculate current totals & move if necessary
                int actionsCount = 0;
                int itemsScanned = 0;
                foreach (var thisblock in allBlocks)
                {
                    // Sum up produced items
                    if (thisblock.HasInventory)
                    {
                        //jdbg.Debug("Processing block: " + thisblock.CustomName);
                        for (int invCount = 0; invCount < thisblock.InventoryCount; invCount++)
                        {
                            List<MyInventoryItem> allItemsInInventory = new List<MyInventoryItem>();

                            Dictionary<String, bool> foundYet = new Dictionary<String, bool>();
                            IMyInventory inv = thisblock.GetInventory(invCount);
                            inv.GetItems(allItemsInInventory);

                            for (int j = 0; j < allItemsInInventory.Count; j++)
                            {
                                itemsScanned++;
                                String name = allItemsInInventory[j].Type.ToString();

                                // Only track the components
                                if (!compToBlueprint.ContainsKey(name))
                                {
                                    //jdbg.Debug("Not tracking for stacking: " + name);
                                    continue;
                                }

                                // Dont worry on first hit
                                if (!foundYet.ContainsKey(name))
                                {
                                    //jdbg.Debug(".. First: " + name);
                                    foundYet[name] = true;
                                    continue;
                                }

                                // If we get here, its a 2nd instance of something
                                jdbg.Debug("Found : '" + name + "' x " + allItemsInInventory[j].Amount.ToIntSafe() + " in " + thisblock.CustomName);
                                inv.TransferItemTo(inv, allItemsInInventory[j]);
                                actionsCount++;
                            }
                        }
                    }
                }
                jdbg.DebugAndEcho("Scanned " + itemsScanned + " items in " + allBlocks.Count + " inventories - " + actionsCount + " things moved");

                jdbg.Alert("Completed - " + actionsCount + " actions performed", "GREEN", alerttag, thisScript);
            }
            catch (Exception ex)
            {
                jdbg.Alert("Exception - " + ex.ToString() + "\n" + ex.StackTrace, "RED", alerttag, thisScript);
            }
        }
        // ----------------------------- CUT -------------------------------------
    }
}
