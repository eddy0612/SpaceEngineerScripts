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

// TODO: Use JINV for item blueprints
// TODO: Use JLCD for colours/LCD interactions

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
        //private JINV jinv = null;
        //private JLCD jlcd = null;

        // -------------------------------------------
        /* Example custom data in programming block: 
               None
        */
        // -------------------------------------------

        // -------------------------------------------
        // Table of component name -> blueprintid
        // -------------------------------------------

        // -------------------------------------------
        Dictionary<String, String> compToBlueprint = new Dictionary<String, String>
        {
            /* Components */
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
            { /*"5.56x45mm NATO magazine",*/ "MyObjectBuilder_AmmoMagazine/NATO_5p56x45mm", null }



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
                // Dummy debug - so we do the lookup of lcds
                Debug(thisScript + " running");
                WriteToAllLCDs(debugLCDs, "", false);  // Clear the debug screens

                // ---------------------------------------------------------------------------
                // We only get here if we are refreshing
                // ---------------------------------------------------------------------------
                jdbg.Echo("Main from " + thisScript + " running..." + lastCheck.ToString());
                Debug("Main from " + thisScript + " running..." + lastCheck.ToString());


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
                Debug("Found " + allBlocks.Count + " blocks with inventories to work through");

                //// Calculate current totals & move if necessary
                int actionsCount = 0;
                int itemsScanned = 0;
                foreach (var thisblock in allBlocks)
                {
                    // Sum up produced items
                    if (thisblock.HasInventory)
                    {
                        //Debug("Processing block: " + thisblock.CustomName);
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
                                    //Debug("Not tracking for stacking: " + name);
                                    continue;
                                }

                                // Dont worry on first hit
                                if (!foundYet.ContainsKey(name))
                                {
                                    //Debug(".. First: " + name);
                                    foundYet[name] = true;
                                    continue;
                                }

                                // If we get here, its a 2nd instance of something
                                Debug("Found : '" + name + "' x " + allItemsInInventory[j].Amount.ToIntSafe() + " in " + thisblock.CustomName);
                                inv.TransferItemTo(inv, allItemsInInventory[j]);
                                actionsCount++;
                            }
                        }
                    }
                }
                Debug("Scanned " + itemsScanned + " items in " + allBlocks.Count + " inventories - " + actionsCount + " things moved");

                Alert("Completed - " + actionsCount + " actions performed", "GREEN");
            }
            catch (Exception ex)
            {
                Alert("Exception - " + ex.ToString() + "\n" + ex.StackTrace, "RED");
            }
        }

        // ---------------------------------------------------------------------------
        // Simple wrapper to write to a central alert
        // ---------------------------------------------------------------------------
        void Alert(String alertMsg, String colour)
        {
            List<IMyTerminalBlock> allBlocksWithLCDs = new List<IMyTerminalBlock>();
            GridTerminalSystem.GetBlocksOfType(allBlocksWithLCDs, (IMyTerminalBlock x) => (
                                                                                      (x.CustomName != null) &&
                                                                                      (x.CustomName.IndexOf("[" + alerttag + "]") >= 0) &&
                                                                                      (x is IMyTextSurfaceProvider)
                                                                                     ));

            String alertOutput = DateTime.Now.ToShortTimeString() + " : " + solidcolor["YELLOW"] + " : " + thisScript + " in " + Me.CustomName + "\n";
            alertOutput += "   " + alertMsg + "\n";
            Echo("Found " + allBlocksWithLCDs.Count + " lcds with '" + alerttag + "' to alert to");
            Debug("Found " + allBlocksWithLCDs.Count + " lcds with '" + alerttag + "' to alert to");
            if (allBlocksWithLCDs.Count == 0)
            {
                Echo("ALERT: " + alertMsg);
            }
            else
            {
                WriteToAllLCDs(allBlocksWithLCDs, alertOutput, true);
            }
        }

        // ---------------------------------------------------------------------------
        // Simple wrapper to decide whether to output to the console or not
        // ---------------------------------------------------------------------------
        List<IMyTerminalBlock> debugLCDs = null;
        bool inDebug = false;
        void Debug(String str)
        {
            if (debug)
            {
                bool wasInDebug = inDebug;
                if (!wasInDebug) inDebug = true;

                if (debugLCDs == null)
                {
                    Echo("First run - working out debug panels");
                    debugLCDs = new List<IMyTerminalBlock>();
                    GridTerminalSystem.GetBlocksOfType(debugLCDs, (IMyTerminalBlock x) => (
                                                                                           (x.CustomName != null) &&
                                                                                           (x.CustomName.IndexOf("[debug]") >= 0) &&
                                                                                           (x is IMyTextSurfaceProvider)
                                                                                          ));
                    Echo("Found " + debugLCDs.Count + " lcds with 'debug' to debug to");
                    WriteToAllLCDs(debugLCDs, "", false);  // Clear the debug screens
                }

                Echo("D:" + str);
                if (!wasInDebug)
                {
                    WriteToAllLCDs(debugLCDs, str + "\n", true);
                }
                if (!wasInDebug) inDebug = false;
            }
        }

        // ---------------------------------------------------------------------------
        // Simple wrapper to write to LCDs
        // ---------------------------------------------------------------------------
        void WriteToAllLCDs(List<IMyTerminalBlock> allBlocksWithLCDs, String msg, bool append)
        {
            if (allBlocksWithLCDs != null && allBlocksWithLCDs.Count > 0)
            {
                foreach (var thisblock in allBlocksWithLCDs)
                {
                    IMyTextSurface thisLCD = ((IMyTextSurfaceProvider)thisblock).GetSurface(0);
                    if (thisLCD != null)
                    {
                        thisLCD.Font = "Monospace";
                        thisLCD.ContentType = ContentType.TEXT_AND_IMAGE;
                        thisLCD.WriteText(msg, append);
                    }
                }
            }
        }
        // ----------------------------- CUT -------------------------------------
    }
}
