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
        String thisScript = "JumpDrive";

        // Development or user config time flags
        bool debug = false;
        bool stayRunning = true;

        int MAXCOLS = 50;
        int INDENT = 5;

        // Data read from program config
        String mytag = "IDONTCARE";    /* Which LCDs to display status on */

        // Private variables
        private JDBG jdbg = null;
        private JINV jinv = null;
        private JLCD jlcd = null;
        private String alertTag = "alert";    // TODO: Could move into config

        // -------------------------------------------
        /* Example custom data in programming block: 
[Config]
; Which LCDs to add display onto
tag=jumpdrive
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
                jdbg.DebugAndEcho("Main from " + thisScript + " running..." + DateTime.Now.ToShortTimeString());

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
                    mytag = (mytag.Split(';')[0]).Trim().ToUpper();  // Remove any trailing comments
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

                // Get all the LCDs we are going to output to
                List<IMyTerminalBlock> displays = jlcd.GetLCDsWithTag(mytag);

                // Initialize the LCDs
                jlcd.InitializeLCDs(displays, TextAlignment.LEFT);

                // Get all jump drive blocks on current grid
                List<IMyTerminalBlock> AllDrives = new List<IMyTerminalBlock>();
                GridTerminalSystem.GetBlocksOfType(AllDrives, (IMyTerminalBlock x) => (
                                                                                       (x is IMyJumpDrive) 
                                                                                      ));
                jdbg.Debug("Found " + AllDrives.Count + " jump drives");
                AllDrives.Sort((a, b) =>
                {
                    if (a == null) {
                        if (b == null) return 0;
                        else return -1;
                    } else if (b == null) {
                        return 1;
                    } else if (a.CubeGrid == b.CubeGrid) {
                        // Same grid... look at Name
                        return a.CustomName.CompareTo(b.CustomName);
                    } else if (a.CubeGrid == Me.CubeGrid) {
                        return -1;
                    } else if (b.CubeGrid == Me.CubeGrid) {
                        return 1;
                    } else 
                        // Sort into 'ships' (grids)
                        return (a.CubeGrid.EntityId < b.CubeGrid.EntityId ? -1 : 1);
                });

                // Resize all the displays so we can show MAXROWS and MAXCOLS
                jlcd.SetupFont(displays, (3 * (AllDrives.Count)) + 10, MAXCOLS+INDENT, false);

                String fullScreen = "";
                fullScreen += thisScript + "    " + DateTime.Now.ToString() + "\n\n";
                bool doneOtherHdr = false;
                bool doneLocalHdr = false;

                String[] outputLines = new string[AllDrives.Count];
                int count = 0;
                foreach (var thisDrive in AllDrives)
                {
                    IMyJumpDrive drive = (IMyJumpDrive) thisDrive;

                    if (!doneLocalHdr && drive.CubeGrid == Me.CubeGrid) {
                        outputLines[count++] = "  Jump drives on current ship:\n\n";
                        doneLocalHdr = true;
                    }
                    if (!doneOtherHdr && drive.CubeGrid != Me.CubeGrid) {
                        outputLines[count++] = "  Jump drives on connected ships:\n\n";
                        doneOtherHdr = true;
                    }

                    String line = "";
                    float perc = (100.0F * drive.CurrentStoredPower / drive.MaxStoredPower);
                    if (drive.CurrentStoredPower == drive.MaxStoredPower)
                    {
                        line += JLCD.solidcolor["GREEN"];
                        perc = 100.0F;
                    } else
                    {
                        line += JLCD.solidcolor["RED"];
                    }
                    line += " " + drive.CustomName.PadRight(25);

                    line += "(" + Math.Floor(perc).ToString().PadLeft(3) + "%)";

                    if (drive.Status == MyJumpDriveStatus.Ready)
                    {
                        line += " READY";

                    }
                    jdbg.Debug(line);
                    outputLines[count++] = line;

                    // Add progress bar
                    String newline = "   [";
                    newline += GetLine((int)Math.Floor(perc), 30);
                    newline += "]";
                    // Deliberately add it twice to make it stand out
                    outputLines[count++] = newline;
                    outputLines[count++] = newline;
                }

                // Sort by name
                for (int i=0; i< outputLines.Length; i++)
                {
                    fullScreen = fullScreen + ("".PadRight(INDENT, ' ')) + outputLines[i] + "\n";
                }

                jlcd.WriteToAllLCDs(displays, fullScreen, false);

                jdbg.Alert("Completed - " + AllDrives.Count + " drives processed", "GREEN", alertTag, thisScript); // This will echo and debug as well
            }
            catch (Exception ex)
            {
                jdbg.Alert("Exception - " + ex.ToString() + "\n" + ex.StackTrace, "RED", alertTag, thisScript);
            }
        }

        String GetLine(int perc, int barSize)
        {
            String bar = "".PadRight(barSize);
            char[] barchars = bar.ToCharArray();

            double squaresize = (double)((double)perc / (double)barSize);

            // Never round up completed, as 99/100 shouldnt show as a full bar
            // However we do end up with 0.9999R which is a pain, so allow 0.01 leeway
            int c_count = (int)Math.Floor((double)0.01 + (double)((double)perc / (double)squaresize));

            var bari = 0;
            for (int i = 0; i < c_count; i++) {
                barchars[i] = JLCD.solidcolor["GREEN"];
                bari++;
            }
            return new string(barchars);
        }

        // ----------------------------- CUT -------------------------------------
    }
}
