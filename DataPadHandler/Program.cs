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
        String thisScript = "DataPadHandler";

        // Development or user config time flags
        bool debug = true;
        bool stayRunning = false;

        // Private variables
        private JDBG jdbg = null;
        private JINV jinv = null;
        private JLCD jlcd = null;
        private String alertTag = "alert";    // TODO: Could move into config

//EarthLike	227.02	120.00	GPS:EarthLike:0.50:0.50:0.50:
//Moon	176.92	19.00	GPS:Moon:16384.50:136384.50:-113615.50:
//Mars	1749.29	120.00	GPS:Mars:1031072.50:131072.50:1631072.50:
//Europa	1835.76	19.00	GPS:Europa:916384.50:16384.50:1616384.50:
//Triton	2542.14	80.25	GPS:Triton:-284463.50:-2434463.50:365536.50:
//Pertam	4079.73	60.00	GPS:Pertam:-3967231.50:-32231.50:-767231.50:
//Alien	5600.00	120.00	GPS:Alien:131072.50:131072.50:5731072.50:
//Titan	5783.85	19.00	GPS:Titan:36384.50:226384.50:5796384.50:

        // -------------------------------------------
        /* Example custom data in programming block: NONE
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

                // ---------------------------------------------------------------------------
                // We only get here if we are refreshing
                // ---------------------------------------------------------------------------
                jdbg.DebugAndEcho("Main from " + thisScript + " running..." + DateTime.Now.ToString());

                // -----------------------------------------------------------------
                // Real work starts here
                // -----------------------------------------------------------------

                // Searches all inventories on my grid
                List<IMyTerminalBlock> allInventories = new List<IMyTerminalBlock>();
                GridTerminalSystem.GetBlocksOfType(allInventories, (IMyTerminalBlock x) => (x.HasInventory &&
                                                                                            x.CubeGrid == Me.CubeGrid
                                                                                            && (x.CustomName.ToUpper().IndexOf("[DATAPADS]") >= 0)
                                                                                      ));

                int pads = 0;
                int uniquepads = 0;

                jdbg.Debug("Found " + allInventories.Count + " blocks with inventories to investigate");
                foreach (var thisblock in allInventories) {
                    if (thisblock.HasInventory) {
                        jdbg.Debug("Processing block: " + thisblock.CustomName);
                        for (int invCount = 0; invCount < thisblock.InventoryCount; invCount++) {
                            List<MyInventoryItem> allItemsInInventory = new List<MyInventoryItem>();
                            IMyInventory inv = thisblock.GetInventory(invCount);
                            inv.GetItems(allItemsInInventory);

                            for (int j = 0; j < allItemsInInventory.Count; j++) {
                                String name = allItemsInInventory[j].Type.ToString();
                                if (name.Contains("Datapad")) {
                                    Echo("Found: " + allItemsInInventory[j]);
                                }
                            }
                        }
                    }
                }

                jdbg.Alert("Completed - " + pads + " pads processed, " + uniquepads + " unique", "GREEN", alertTag, thisScript); // This will echo and debug as well
            }
            catch (Exception ex) {
                jdbg.Alert("Exception - " + ex.ToString() + "\n" + ex.StackTrace, "RED", alertTag, thisScript);
            }
        }

        // ----------------------------- CUT -------------------------------------
    }
}
