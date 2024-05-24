﻿using Sandbox.Game.EntityComponents;
using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;
using SpaceEngineers.Game.ModAPI.Ingame;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel.Design;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
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
        String thisScript = "CountOre";

        // Development or user config time flags
        bool debug = true;
        bool stayRunning = false;

        // Data read from program config
        String myTag = "IDONTCARE";    // Which tags to watch out for
        int cacheTime = 30;            // Refresh config every 30 seconds

        // Private variables
        private JDBG jdbg = null;
        private JINV jinv = null;
        private JLCD jlcd = null;
        private String alertTag = "alert";    // TODO: Could move into config

        /* Example custom data in programming block: 
[Config]
tag=doors
        */
        // -------------------------------------------

        // Internals
        DateTime lastCheck = new DateTime(0);

        public Program()
        {
            jdbg = new JDBG(this, debug);
            jlcd = new JLCD(this, jdbg, false);
            jinv = new JINV(jdbg);
            jlcd.UpdateFullScreen(Me, thisScript);

            // ---------------------------------------------------------------------------
            // Work out our tag
            // ---------------------------------------------------------------------------

            // ---------------------------------------------------------------------------
            // Get my custom data and parse to get the config
            // ---------------------------------------------------------------------------
            MyIni _ini = new MyIni();
            MyIniParseResult result;
            if (!_ini.TryParse(Me.CustomData, out result))
                throw new Exception(result.ToString());

            // Get the value of the "tag" key under the "config" section.
            myTag = _ini.Get("config", "tag").ToString("doors");
            if (myTag == null || myTag.Equals("")) {
                myTag = "DOORS";
            } else {
                myTag = myTag.ToUpper();
                return;
            }
            Echo("Using tag: " + myTag);

            // Populate the config cache
            processGroups();
            lastCheck = DateTime.Now;

            // Run every 100 ticks, but relies on internal check to only actually
            // perform on a defined frequency
            if (stayRunning) {
                Runtime.UpdateFrequency = UpdateFrequency.Update100;
            }



        }

        public void Save()
        {
        }

        public void Main(string argument, UpdateType updateSource)
        {
            try {
                // ---------------------------------------------------------------------------
                // We only get here if we are refreshing
                // ---------------------------------------------------------------------------                
                jdbg.ClearDebugLCDs();  // Clear the debug screens
                jdbg.DebugAndEcho("Main from " + thisScript + " running..." + DateTime.Now.ToString());

                // Do the work now
                int changed = processGroups();

                jdbg.Alert("Completed - OK, " + changed + " things changed", "GREEN", alertTag, thisScript);
            }
            catch (Exception ex) {
                jdbg.Alert("Exception - " + ex.ToString() + "\n" + ex.StackTrace, "RED", alertTag, thisScript);
            }
        }

        // ---------------------------------------------------------------------------
        // Process all the groups being monitored by this script and set door states
        // correctly.
        // Doors can be tagged [IN] (only open if pressurized) {default if none of the others}
        //                    [OUT] (only open if depressurized)
        //                    [INOUT] (Open anytime)
        // We process all groups before touching the doors as a door might be ok to open
        // in one group but not ok to open in the adjacent group.
        // ---------------------------------------------------------------------------
        private int processGroups()
        {
            int changed = 0;

            //Find all groups tagged[door]
            List<IMyBlockGroup> blockGroups = new List<IMyBlockGroup>();
            GridTerminalSystem.GetBlockGroups(blockGroups, (IMyBlockGroup x) => (
                                                                                 (x.Name.ToUpper().Contains("[" + myTag + "]"))
                                                                                ));
            jdbg.DebugAndEcho("Found " + blockGroups.Count + " block groups to process");

            List<IMyDoor> mustClose = new List<IMyDoor>();
            List<IMyDoor> invalidToOpen = new List<IMyDoor>();
            List<IMyDoor> allowedToOpen = new List<IMyDoor>();

            //For each group
            foreach (var thisGroup in blockGroups) {
                jdbg.Debug("Group: " + thisGroup.Name);

                // Find all doors and airvents, ignore anything with[IGNORE]
                // Note: Any door which is not  [out] or [inout] will be assumed to be [in]
                List<IMyTerminalBlock> doors = new List<IMyTerminalBlock>();
                thisGroup.GetBlocksOfType(doors, (IMyTerminalBlock x) => (
                                                                             (x is IMyDoor) &&
                                                                             (!(x.CustomName.ToUpper().Contains("IGNORE")))
                                                                            ));
                jdbg.Debug("  Contains " + doors.Count + " doors");

                // and AIRVENTS
                List<IMyTerminalBlock> airvents = new List<IMyTerminalBlock>();
                thisGroup.GetBlocksOfType(airvents, (IMyTerminalBlock x) => (
                                                                             (x is IMyAirVent)
                                                                            ));
                jdbg.Debug("  and " + airvents.Count + " airvents");

                // ---------------------------------------------------------------------
                // Process air vent(s) to see what we are going to allow the doors to do
                // ---------------------------------------------------------------------
                // Note if there's no air vents, we dont care on the status and its always in the right state
                bool isFullyPressurized = true;     // Based on what we find we will clear one or both of
                bool isFullyDePressurized = true;   // these flags.
                int maxOpenDoors = 0;

                // if there are no airvents, we are treating as airlock
                if (airvents.Count == 0) {
                    isFullyPressurized = true;     // Not true but helps with logic below
                    isFullyDePressurized = true;   // Not true but helps with logic below
                    maxOpenDoors = 1;
                } else {
                    maxOpenDoors = 999999;

                    // Otherwise correct the status from the airvents
                    jdbg.Debug("Calculating airvent state");
                    foreach (var thisVent in airvents) {
                        VentStatus thisStatus = ((IMyAirVent)thisVent).Status;
                        jdbg.Debug(".." + thisVent.Name + " : " + thisStatus);
                        if (!(thisStatus == VentStatus.Pressurized)) {
                            isFullyPressurized = false;
                        }
                        if (!(thisStatus == VentStatus.Depressurized)) {
                            isFullyDePressurized = false;
                        }
                    }
                }
                jdbg.Debug("Result: isPress " + isFullyPressurized + ", isDepress " + isFullyDePressurized + ", maxdoors:" + maxOpenDoors);

                // ---------------------------------------------------------------------
                // Work out what we would like the doors to do for this group
                // ---------------------------------------------------------------------
                int opendoors = 0;

                // for each door
                foreach (var door in doors) {
                    IMyDoor thisDoor = (IMyDoor)door;

                    if (thisDoor.Status == DoorStatus.Open) {   /* OPEN DOORS */
                        opendoors = opendoors + 1;

                        // Ensure door is enabled - no case where an open door should be disabled
                        thisDoor.Enabled = true;

                        // if door is [in] and not fully pressurized OR
                        //    door is [out] and not fully depressurized THEN
                        if ((isIn(door) && !isFullyPressurized) ||
                            (isOut(door) && !isFullyDePressurized)) {
                            if (!mustClose.Contains(thisDoor)) {
                                mustClose.Add(thisDoor);
                            }
                        }

                    } else if (thisDoor.Status == DoorStatus.Closed) {
                        // if opendoors<max doors open AND   /* airlock case */
                        //           fully depressurized AND door is [out] OR   /* CLOSED DOORS room in right state */
                        //           fully pressurized AND door is [in] OR
                        //           door is [inout]
                        //        THEN
                        //            add to 'valid to open' doors
                        if ((opendoors < maxOpenDoors) &&
                            ((isOut(door) && isFullyDePressurized) ||
                             (isIn(door) && isFullyPressurized) ||
                             (isInOut(door)))
                            ) {
                            if (!allowedToOpen.Contains(thisDoor)) {
                                allowedToOpen.Add(thisDoor);
                            }
                        } else {
                            // else   /* CLOSED DOORS room in wrong state */
                            //	add to 'invalid to open' doors
                            if (!invalidToOpen.Contains(thisDoor)) {
                                invalidToOpen.Add(thisDoor);
                            }
                        }
                    }
                }

                // We now have 3 lists, and one door might be in more than one list
                // 1. List of doors we HAVE to close ... set them closing...
                jdbg.Echo("Performing actions:");
                foreach (var door in mustClose) {
                    changed++;
                    jdbg.Echo(" - Force closing " + ((IMyTerminalBlock)door).CustomName);
                    door.Enabled = true;
                    door.CloseDoor();
                }

                // 2. Doors we CANNOT open in the 'invalid to open' list - remove them from the 'valid to open' list, set them to disabled
                foreach (var door in invalidToOpen) {
                    if (allowedToOpen.Contains(door)) {
                        allowedToOpen.Remove(door);
                    }
                    if (door.Enabled) {
                        changed++;
                        jdbg.Echo(" - Disabling " + ((IMyTerminalBlock)door).CustomName);
                        door.Enabled = false;
                    }
                }

                // 3. Doors we allow people to open - set them to enabled
                foreach (var door in allowedToOpen) {
                    if (!door.Enabled) {
                        changed++;
                        jdbg.Echo(" - Enabling " + ((IMyTerminalBlock)door).CustomName);
                        door.Enabled = true;
                    }
                }
            }

            // Return count of things done
            return changed;
        }

        private bool isInOut(IMyTerminalBlock door)
        {
            return (door.CustomName.ToUpper().Contains("[INOUT]"));
        }
        private bool isOut(IMyTerminalBlock door)
        {
            return (door.CustomName.ToUpper().Contains("[OUT]"));
        }
        private bool isIn(IMyTerminalBlock door)
        {
            if (door.CustomName.ToUpper().Contains("[IN]")) {
                return true;
            } else {
                if (isOut(door)) return false;
                if (isInOut(door)) return false;
                return true; // No tag at all
            }
        }
    }
}
