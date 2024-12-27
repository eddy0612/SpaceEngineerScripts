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
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
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

// TODO: Use current grid only - Not convinced... but should we limit the block locate to those
//      connected to the assembler chosen?

namespace IngameScript
{
    partial class Program : MyGridProgram
    {

        // ----------------------------- CUT -------------------------------------
        String thisScript = "AmmoHandler";

        // Development time flags
        bool debug = false;
        bool stayRunning = true;
        bool actuallyQ = true;

        // Private variables
        private JDBG jdbg = null;
        private JINV jinv = null;
        private JLCD jlcd = null;

        // My configuration - See Instructions.readme

        Dictionary<String, String> CompToBlueprint = new Dictionary<String, String>
        {
            { /*"Gatling",*/            "MyObjectBuilder_AmmoMagazine/NATO_25x184mm", "MyObjectBuilder_BlueprintDefinition/Position0080_NATO_25x184mmMagazine" },
            { /*"Autocannon",*/         "MyObjectBuilder_AmmoMagazine/AutocannonClip", "MyObjectBuilder_BlueprintDefinition/Position0090_AutocannonClip" },
            { /*"Rocket",*/             "MyObjectBuilder_AmmoMagazine/Missile200mm", "MyObjectBuilder_BlueprintDefinition/Position0100_Missile200mm" },
            { /*"Assault_Cannon",*/     "MyObjectBuilder_AmmoMagazine/MediumCalibreAmmo", "MyObjectBuilder_BlueprintDefinition/Position0110_MediumCalibreAmmo" },
            { /*"Artillery",*/          "MyObjectBuilder_AmmoMagazine/LargeCalibreAmmo", "MyObjectBuilder_BlueprintDefinition/Position0120_LargeCalibreAmmo" },
            { /*"Small_Railgun",*/      "MyObjectBuilder_AmmoMagazine/SmallRailgunAmmo", "MyObjectBuilder_BlueprintDefinition/Position0130_SmallRailgunAmmo" },
            { /*"Large_Railgun",*/      "MyObjectBuilder_AmmoMagazine/LargeRailgunAmmo", "MyObjectBuilder_BlueprintDefinition/Position0140_LargeRailgunAmmo" },
            // Commented out but put back in if needed
            //{ /*"S-10_Pistol",*/        "MyObjectBuilder_AmmoMagazine/SemiAutoPistolMagazine", "MyObjectBuilder_BlueprintDefinition/Position0010_SemiAutoPistolMagazine" },
            //{ /*"S-10E_Pistol",*/       "MyObjectBuilder_AmmoMagazine/ElitePistolMagazine", "MyObjectBuilder_BlueprintDefinition/Position0030_ElitePistolMagazine" },
            //{ /*"S-20A_Pistol",*/       "MyObjectBuilder_AmmoMagazine/FullAutoPistolMagazine" "MyObjectBuilder_BlueprintDefinition/Position0020_FullAutoPistolMagazine" },
            //{ /*"MR-20_Rifle",*/        "MyObjectBuilder_AmmoMagazine/AutomaticRifleGun_Mag_20rd", "MyObjectBuilder_BlueprintDefinition/Position0040_AutomaticRifleGun_Mag_20rd" },
            //{ /*"MR-30E_Rifle",*/       "MyObjectBuilder_AmmoMagazine/UltimateAutomaticRifleGun_Mag_30rd", "MyObjectBuilder_BlueprintDefinition/Position0070_UltimateAutomaticRifleGun_Mag_30rd" },
            //{ /*"MR-50A_Rifle",*/       "MyObjectBuilder_AmmoMagazine/RapidFireAutomaticRifleGun_Mag_50rd", "MyObjectBuilder_BlueprintDefinition/Position0050_RapidFireAutomaticRifleGun_Mag_50rd" },
            //{ /*"MR-8P_Rifle",*/        "MyObjectBuilder_AmmoMagazine/PreciseAutomaticRifleGun_Mag_5rd", "MyObjectBuilder_BlueprintDefinition/Position0060_PreciseAutomaticRifleGun_Mag_5rd" },
        };
        Dictionary<String, String> BlueprintToComp = new Dictionary<String, String>
        {
            { /*"Gatling",*/            "MyObjectBuilder_BlueprintDefinition/Position0080_NATO_25x184mmMagazine", "MyObjectBuilder_AmmoMagazine/NATO_25x184mm" },
            { /*"Autocannon",*/         "MyObjectBuilder_BlueprintDefinition/Position0090_AutocannonClip", "MyObjectBuilder_AmmoMagazine/AutocannonClip" },
            { /*"Rocket",*/             "MyObjectBuilder_BlueprintDefinition/Position0100_Missile200mm", "MyObjectBuilder_AmmoMagazine/Missile200mm" },
            { /*"Assault_Cannon",*/     "MyObjectBuilder_BlueprintDefinition/Position0110_MediumCalibreAmmo", "MyObjectBuilder_AmmoMagazine/MediumCalibreAmmo" },
            { /*"Artillery",*/          "MyObjectBuilder_BlueprintDefinition/Position0120_LargeCalibreAmmo", "MyObjectBuilder_AmmoMagazine/LargeCalibreAmmo" },
            { /*"Small_Railgun",*/      "MyObjectBuilder_BlueprintDefinition/Position0130_SmallRailgunAmmo", "MyObjectBuilder_AmmoMagazine/SmallRailgunAmmo" },
            { /*"Large_Railgun",*/      "MyObjectBuilder_BlueprintDefinition/Position0140_LargeRailgunAmmo", "MyObjectBuilder_AmmoMagazine/LargeRailgunAmmo" },
            // Commented out but put back in if needed
            //{ /*"S-10_Pistol",*/        "MyObjectBuilder_BlueprintDefinition/Position0010_SemiAutoPistolMagazine", "MyObjectBuilder_AmmoMagazine/SemiAutoPistolMagazine" },
            //{ /*"S-10E_Pistol",*/       "MyObjectBuilder_BlueprintDefinition/Position0030_ElitePistolMagazine", "MyObjectBuilder_AmmoMagazine/ElitePistolMagazine" },
            //{ /*"S-20A_Pistol",*/       "MyObjectBuilder_BlueprintDefinition/Position0020_FullAutoPistolMagazine", "MyObjectBuilder_AmmoMagazine/FullAutoPistolMagazine" },
            //{ /*"MR-20_Rifle",*/        "MyObjectBuilder_BlueprintDefinition/Position0040_AutomaticRifleGun_Mag_20rd", "MyObjectBuilder_AmmoMagazine/AutomaticRifleGun_Mag_20rd" },
            //{ /*"MR-30E_Rifle",*/       "MyObjectBuilder_BlueprintDefinition/Position0070_UltimateAutomaticRifleGun_Mag_30rd", "MyObjectBuilder_AmmoMagazine/UltimateAutomaticRifleGun_Mag_30rd" },
            //{ /*"MR-50A_Rifle",*/       "MyObjectBuilder_BlueprintDefinition/Position0050_RapidFireAutomaticRifleGun_Mag_50rd", "MyObjectBuilder_AmmoMagazine/RapidFireAutomaticRifleGun_Mag_50rd" },
            //{ /*"MR-8P_Rifle",*/        "MyObjectBuilder_BlueprintDefinition/Position0060_PreciseAutomaticRifleGun_Mag_5rd", "MyObjectBuilder_AmmoMagazine/PreciseAutomaticRifleGun_Mag_5rd" },
        };

        // Mapping of all ammos we add to custom data to internal component name
        Dictionary<String, String> friendlyitemname = new Dictionary<String, String>
        {
            { "Gatling",            "MyObjectBuilder_AmmoMagazine/NATO_25x184mm" },
            { "Autocannon",         "MyObjectBuilder_AmmoMagazine/AutocannonClip" },
            { "Rocket",             "MyObjectBuilder_AmmoMagazine/Missile200mm" },
            { "Assault_Cannon",     "MyObjectBuilder_AmmoMagazine/MediumCalibreAmmo" },
            { "Artillery",          "MyObjectBuilder_AmmoMagazine/LargeCalibreAmmo" },
            { "Small_Railgun",      "MyObjectBuilder_AmmoMagazine/SmallRailgunAmmo" },
            { "Large_Railgun",      "MyObjectBuilder_AmmoMagazine/LargeRailgunAmmo" },
            // Commented out but put back in if needed
            //{ "S-10_Pistol",        "MyObjectBuilder_AmmoMagazine/SemiAutoPistolMagazine" },
            //{ "S-10E_Pistol",       "MyObjectBuilder_AmmoMagazine/ElitePistolMagazine" },
            //{ "S-20A_Pistol",       "MyObjectBuilder_AmmoMagazine/FullAutoPistolMagazine" },
            //{ "MR-20_Rifle",        "MyObjectBuilder_AmmoMagazine/AutomaticRifleGun_Mag_20rd" },
            //{ "MR-30E_Rifle",       "MyObjectBuilder_AmmoMagazine/UltimateAutomaticRifleGun_Mag_30rd" },
            //{ "MR-50A_Rifle",       "MyObjectBuilder_AmmoMagazine/RapidFireAutomaticRifleGun_Mag_50rd" },
            //{ "MR-8P_Rifle",        "MyObjectBuilder_AmmoMagazine/PreciseAutomaticRifleGun_Mag_5rd" },
        };

        // My configuration
        int refreshSpeed = 5;                       // Default to 5 seconds if not provided
        int barSize = 15;
        String mytag = "IDONTCARE";
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
            try
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

                // ---------------------------------------------------------------------------
                // We only get here if we are refreshing
                // ---------------------------------------------------------------------------               
                jdbg.ClearDebugLCDs();  // Clear the debug screens
                jdbg.DebugAndEcho("Main Running..." + DateTime.Now.ToString());

                // ---------------------------------------------------------------------------
                // Get my custom data and parse to get the config
                // ---------------------------------------------------------------------------
                MyIniParseResult result;
                MyIni _ini = new MyIni();
                if (!_ini.TryParse(Me.CustomData, out result))
                    throw new Exception(result.ToString());

                // Get the value of the "tag" key under the "config" section.
                mytag = _ini.Get("config", "tag").ToString();
                if (mytag != null)
                {
                    mytag = (mytag.Split(';')[0]).Trim();
                    jdbg.DebugAndEcho("Using tag of " + mytag);
                }
                else
                {
                    jdbg.DebugAndEcho("No tag configured\nPlease add [config] for tag=<substring>");
                    return;
                }

                // Get the value of the "refreshSpeed" key under the "config" section.
                int newrefreshSpeed = _ini.Get("config", "refreshSpeed").ToInt32();
                jdbg.DebugAndEcho("New refresh speed will be " + newrefreshSpeed);
                if (newrefreshSpeed < 1)
                {
                    jdbg.DebugAndEcho("Invalid refresh speed or not defined - defaulting to 5 seconds");
                    refreshSpeed = 5;
                }
                else
                {
                    refreshSpeed = newrefreshSpeed;
                }

                // Get the value of the "refreshSpeed" key under the "config" section.
                int newBarSize = _ini.Get("config", "barSize").ToInt32();
                Echo("Bar Size will be " + newBarSize);
                if (newBarSize < 1)
                {
                    jdbg.DebugAndEcho("Invalid bar size or not defined - defaulting to 25 seconds");
                    barSize = 25;
                }
                else
                {
                    barSize = newBarSize;
                }

                // ---------------------------------------------------------------------------
                // vvv                   Now do the actual work                           vvvv
                // ---------------------------------------------------------------------------
                // Logic:
                // Enumerate through all the cargos and work out whats needed
                // Enumerate through all the assemblers and
                //   - Add up any ammo queued
                //   - Distribute any in its inventory, move to a cargo that needs it
                // Queue up whats missing in batches
                // Build and display status on LCD 

                // Work out where to queue things up to 
                var assemblers = new List<IMyAssembler>();
                IMyAssembler qAssembler = null;
                GridTerminalSystem.GetBlocksOfType<IMyAssembler>(assemblers, (IMyAssembler x) => (
                                                                                                    x.CustomName.ToUpper().IndexOf("[" + mytag.ToUpper() + "]") >= 0
                                                                                                 ));
                if (assemblers.Count == 0)
                {
                    jdbg.DebugAndEcho("No assembler found with [" + mytag + "] in the name - guessing");
                    GridTerminalSystem.GetBlocksOfType<IMyAssembler>(assemblers, (IMyAssembler x) => (
                                                                                                        x.CanUseBlueprint(MyDefinitionId.Parse(CompToBlueprint[friendlyitemname["Gatling"]]))
                                                                                                     ));
                    if (assemblers.Count == 0)
                    {
                        jdbg.DebugAndEcho("ERROR: Guess failed... aborting");
                        return;
                    }
                }

                if (assemblers.Count > 1)
                {
                    jdbg.DebugAndEcho("Multiple assemblers found with [" + mytag + "] in the name - using first");
                }
                qAssembler = assemblers[0];
                jdbg.DebugAndEcho("Will queue on " + qAssembler.CustomName);

                Dictionary<String, int> total_expected = new Dictionary<String, int>();
                Dictionary<String, int> total_existing = new Dictionary<String, int>();
                Dictionary<String, int> total_needed = new Dictionary<String, int>();
                foreach (var ammoName in friendlyitemname)
                {
                    total_expected[ammoName.Key] = 0;
                    total_existing[ammoName.Key] = 0;
                    total_needed[ammoName.Key] = 0;
                }

                // Cargo containers to move to / populate
                List<IMyTerminalBlock> cargosToFill = new List<IMyTerminalBlock>();
                GridTerminalSystem.GetBlocksOfType(cargosToFill, (IMyTerminalBlock x) => (
                                                                                          (x.CustomName != null) &&
                                                                                          (x.CustomName.ToUpper().IndexOf("[" + mytag.ToUpper() + "]") >= 0) &&
                                                                                          /* We are going to fill locked as well : (x.CustomName.ToUpper().IndexOf("[LOCKED]") < 0) && */
                                                                                          (x.HasInventory) && 
                                                                                          !(x is IMyAssembler)
                                                                                         ));
                Dictionary<String, Dictionary<IMyTerminalBlock, int>> needingAmmo = new Dictionary<String, Dictionary<IMyTerminalBlock, int>>();


                // Calculate current totals & move if necessary
                foreach (var thisblock in cargosToFill)
                {
                    jdbg.Debug("Processing block: " + thisblock.CustomName);
                    MyIni _blockini = new MyIni();
                    if (!_blockini.TryParse(thisblock.CustomData, out result))
                    {
                        jdbg.DebugAndEcho("Block: " + thisblock.CustomName + " has invalid CustomData - ignoring");
                        continue;
                    }

                    if (!_blockini.ContainsSection("ammo"))
                    {
                        jdbg.DebugAndEcho("Block: " + thisblock.CustomName + " has no ammo section... adding it");
                        _blockini.AddSection("ammo");
                        Echo("Now populating it");
                        foreach (var ammoName in friendlyitemname)
                        {
                            jdbg.Debug("Now populating it with " + ammoName.Key);
                            _blockini.Set("ammo", ammoName.Key, "0");
                        }
                        thisblock.CustomData = _blockini.ToString();
                        // Nothing to do for this block now - Everything is zero
                        continue;
                    }
                    else
                    {
                        jdbg.Debug("Block: " + thisblock.CustomName + " has an ammo section... ");
                        foreach (var ammoName in friendlyitemname)
                        {
                            int ammowanted = _blockini.Get("ammo", ammoName.Key).ToInt32(0);
                            total_expected[ammoName.Key] += ammowanted;

                            if (ammowanted > 0)
                            {
                                jdbg.Debug("We want " + ammowanted + " of " + ammoName.Key);

                                // Now see how many we have across the inventories
                                for (int invCount = 0; invCount < thisblock.InventoryCount; invCount++)
                                {
                                    List<MyInventoryItem> allItemsInInventory = new List<MyInventoryItem>();
                                    IMyInventory inv = thisblock.GetInventory(invCount);
                                    inv.GetItems(allItemsInInventory, (MyInventoryItem x) => (
                                                                                            (x.Type.ToString().Equals(friendlyitemname[ammoName.Key]))
                                                                                             ));
                                    int totalFound = 0;
                                    foreach (var invitem in allItemsInInventory)
                                    {
                                        jdbg.Debug("Found: " + invitem.Type.ToString() + " count " + invitem.Amount.ToIntSafe());
                                        totalFound += invitem.Amount.ToIntSafe();
                                        total_existing[ammoName.Key] += invitem.Amount.ToIntSafe();
                                    }
                                    if (totalFound < ammowanted)
                                    {
                                        int needed = ammowanted - totalFound;
                                        jdbg.Debug("Block " + thisblock.CustomName + " needs " + needed + " " + ammoName.Key);
                                        total_needed[ammoName.Key] += needed;
                                        if (!needingAmmo.ContainsKey(ammoName.Key))
                                        {
                                            needingAmmo[ammoName.Key] = new Dictionary<IMyTerminalBlock, int>();
                                        }
                                        needingAmmo[ammoName.Key][thisblock] = needed;
                                    }
                                    else
                                    {
                                        jdbg.Debug("Block " + thisblock.CustomName + " has enough " + ammoName.Key + "(" + totalFound + ")");
                                    }
                                }
                            }
                        }
                    }
                }

                // When we get here we know what ammo we need to discover... Now see if there's any in any of
                // the assemblers (Note we might q to one but coop might distribute the production) we can move
                // and queue up the remainder needed
                var allAssemblers = new List<IMyAssembler>();
                /* Previously I only stuck to the same grid but this is problematic if one assembler is in coop mode on 
                   an adjacent grid, because then its inventory isnt seen so we ignore what its doing, resuling in 
                   far more than we needed
                 */
                GridTerminalSystem.GetBlocksOfType<IMyAssembler>(allAssemblers); //, (IMyAssembler x) => (x.CubeGrid.Equals(Me.CubeGrid)));
                int totalQueued = 0;
                foreach (var ammoName in friendlyitemname)
                {
                    int toBuild = total_needed[ammoName.Key];
                    if (toBuild > 0)
                    {
                        jdbg.Debug("We need to find " + toBuild + " of " + ammoName.Key);

                        // Find any built ammo, and move it over
                        foreach (var thisAssembler in allAssemblers)
                        {
                            jdbg.Debug("Checking assembler: " + thisAssembler.CustomName);
                            List<MyInventoryItem> foundAmmo = new List<MyInventoryItem>();
                            // Cheat as assemblers only have one inventory for output for now
                            thisAssembler.GetInventory(1).GetItems(foundAmmo, (MyInventoryItem x) => (
                                                                                                    (x.Type.ToString().Equals(friendlyitemname[ammoName.Key]))
                                                                                                     ));
                            if (foundAmmo.Count > 0)
                            {
                                jdbg.Debug("Got " + foundAmmo.Count + " when looking for " + friendlyitemname[ammoName.Key]);
                                foreach (var thisAmmo in foundAmmo)
                                {
                                    // Now redistribute them - start with the inventory needing the most
                                    int ammoFound = thisAmmo.Amount.ToIntSafe();
                                    jdbg.Debug("We have found some produced ammo to distribute count " + ammoFound);

                                    // We need to build less
                                    toBuild = toBuild - ammoFound;

                                    Dictionary<IMyTerminalBlock, int> needingThisAmmo = null;
                                    if (needingAmmo.ContainsKey(ammoName.Key)) needingThisAmmo = needingAmmo[ammoName.Key];

                                    while ((ammoFound > 0) && (needingThisAmmo != null) && (needingThisAmmo.Count > 0))
                                    {
                                        foreach (var requirement in needingThisAmmo.OrderByDescending(p => p.Value))
                                        {
                                            int moveTo = 0;

                                            // Can we satisty? Update the hash as we go
                                            if (ammoFound >= requirement.Value)
                                            {
                                                jdbg.Debug("We can satisfy " + requirement.Key.CustomName + " which needs " + requirement.Value);
                                                total_existing[ammoName.Key] += requirement.Value;
                                                moveTo = requirement.Value;
                                                needingThisAmmo.Remove(requirement.Key);
                                                if (needingThisAmmo.Count == 0)
                                                {
                                                    needingThisAmmo = null;
                                                }
                                            }
                                            else
                                            {
                                                jdbg.Debug("We can only satisfy " + ammoFound + " of " + requirement.Key.CustomName + " which needs " + requirement.Value);
                                                total_existing[ammoName.Key] += ammoFound;
                                                moveTo = ammoFound;
                                                needingThisAmmo[requirement.Key] -= moveTo;
                                            }
                                            ammoFound -= moveTo;

                                            // Now move what we found
                                            if (moveTo > 0)
                                            {
                                                jdbg.Debug("Need to move this block! From " + thisAssembler.CustomName + "\n  to " + requirement.Key.CustomName + " " + moveTo + " of " + ammoName.Key);
                                                if (thisAssembler.GetInventory(1).TransferItemTo(requirement.Key.GetInventory(), thisAmmo, moveTo))
                                                {
                                                    jdbg.Debug("Transferred ok");
                                                }
                                                else
                                                {
                                                    jdbg.Debug("Failed to transfer?");
                                                }
                                            }

                                            // Stop if we have nothing left
                                            if (ammoFound == 0) break;
                                        }
                                    }
                                }
                            }

                            // Now handle queued ammo - we dont want to overqueue
                            var prodQ = new List<MyProductionItem>();
                            thisAssembler.GetQueue(prodQ);
                            //Debug("Got " + prodQ.Count + " queued up");
                            foreach (var thisQItem in prodQ)
                            {
                                if (thisQItem.BlueprintId.ToString().Equals(CompToBlueprint[friendlyitemname[ammoName.Key]]))
                                {
                                    jdbg.Debug("... found " + thisQItem.Amount + " of " + ammoName.Key + " items");
                                    toBuild = toBuild - thisQItem.Amount.ToIntSafe();
                                }
                            }
                        }

                        if (toBuild > 0)
                        {
                            jdbg.Debug("Now we know how many to queue up: " + toBuild);
                            // Now queue up what else is needed
                            MyDefinitionId blueprint = MyDefinitionId.Parse(CompToBlueprint[friendlyitemname[ammoName.Key]]);
                            if (blueprint == null)
                            {
                                Echo("ERROR: Cannot create blueprint " + CompToBlueprint[friendlyitemname[ammoName.Key]]);
                                return;
                            }
                            MyFixedPoint mfp = toBuild;
                            if (actuallyQ) {
                                Echo("Queueing up " + toBuild + " of " + ammoName.Key);
                                qAssembler.AddQueueItem(blueprint, mfp);
                            } else {
                                Echo("DRYRUN: Not Queueing up " + toBuild + " of " + ammoName.Key);
                            }
                            totalQueued += toBuild;
                        }
                        else if (toBuild < 0)
                        {
                            Echo("We have a surplus of " + ammoName.Key+ " - unexpected but ignorable");
                        } else
                        {
                            jdbg.Debug("We have the right number of " + ammoName.Key);
                        }

                    }
                    else
                    {
                        jdbg.Debug("We have enough " + ammoName.Key);
                    }
                }

                ClearDisplay();
                AddDisplay("Ammo Status at " + DateTime.Now.ToString());
                AddDisplay("===========");
                int totalexisting = 0;
                int totalexpected = 0;
                foreach (var ammoName in friendlyitemname)
                {
                    totalexisting += total_existing[ammoName.Key];
                    totalexpected += total_expected[ammoName.Key];

                    if (total_expected[ammoName.Key] > 0)
                    {
                        AddDisplay(GetLine(ammoName.Key, total_existing[ammoName.Key], 0, total_expected[ammoName.Key]));
                    }
                }
                DrawLCD(mytag, screen);

                int percentComplete = 0;
                percentComplete = (totalexisting * 100) / (totalexpected);

                jdbg.Alert("Completed - " + percentComplete + "% complete, queued: " + totalQueued, "GREEN", alerttag, thisScript);
            } catch (Exception ex)
            {
                jdbg.Alert("Exception - " + ex.ToString() + "\n" + ex.StackTrace, "RED", alerttag, thisScript);
            }
        }
    
        string GetLine(String name, int count, int queued, int wanted)
        {
            String result = name.PadRight(20);

            result = result + ">"; // Could be [ or >

            String bar = "".PadRight(barSize);
            char[] barchars = bar.ToCharArray();

            // Set limits, as count or queued or both may take us over the limit
            if (count > wanted) count = wanted;
            if ((count + queued) > wanted) queued = (wanted - count);

            double squaresize = (double)wanted / (double)barSize;
            // Never round up completed, as 99/100 shouldnt show as a full bar
            int c_count = (int)Math.Floor((double)count / (double)squaresize);

            // We can round up the queued ones, but need to ensure we dont go over the max
            int q_count = (int)Math.Round((double)queued / (double)squaresize);
            if ((c_count + q_count) > barSize) q_count = q_count - 1;

            jdbg.Debug("getline: " + name + "," + count + "," + queued + "," + wanted + " = " + c_count + "/" + q_count);
            var bari = 0;
            for (int i = 0; i < c_count; i++)
            {
                barchars[bari] = JLCD.solidcolor["CYAN"];
                bari++;
            }
            for (int i = 0; i < q_count; i++)
            {
                barchars[bari] = JLCD.solidcolor["YELLOW"];
                bari++;
            }
            for (int i = bari; i < barSize; i++)
            {
                barchars[i] = JLCD.solidcolor["RED"];
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

        // ---------------------------------------------------------------------------
        // Simple wrapper to completely redraw our LCD
        // ---------------------------------------------------------------------------
        void DrawLCD(String tag, String screenContents)
        {
            jdbg.Debug("Drawing the screens");
            List<IMyTerminalBlock> drawLCDs = jlcd.GetLCDsWithTag(tag);
            jlcd.InitializeLCDs(drawLCDs, TextAlignment.LEFT);
            jlcd.SetupFont(drawLCDs, screenContents.Split('\n').Length + 1, MAXCOLS, true);
            jlcd.WriteToAllLCDs(drawLCDs, screenContents, false);
        }

        // ----------------------------- CUT -------------------------------------
    }
}
