using Sandbox.Game;
using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using VRage.Game.GUI.TextPanel;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ModAPI.Ingame.Utilities;
using static IngameScript.Program;

namespace IngameScript
{
    partial class Program : MyGridProgram
    {
        // ----------------------------- CUT -------------------------------------
        String thisScript = "RenameShip";

        // Development time flags
        bool debug = false;
        bool stayRunning = true;

        // Private variables
        private JDBG jdbg = null;
        private JINV jinv = null;
        private JLCD jlcd = null;
        private JCTRL jctrl = null;
        private String alertTag = "alert";    // TODO: Could move into config

        // Example custom data in programming block:
        // [Config]
        // tag=rename

        // Data read from program config
        String mytag = "IDONTCARE";    /* Which LCDs to display status on */

        // My configuration
        int refreshSpeed = 10;
        String originalName = "";
        bool isConfirm = false;
        int maxSize = 4;

        // Internals
        DateTime lastCheck = new DateTime(0);

        // ---------------------------------------------------------------------------
        // Constructor
        // ---------------------------------------------------------------------------
        public Program()
        {
            // Run every 100 ticks, but relies on internal check to only actually
            // perform on a defined frequency
            if (stayRunning) Runtime.UpdateFrequency = UpdateFrequency.Update100;

            // Initialize the utility classes
            jdbg = new JDBG(this, debug);
            jlcd = new JLCD(this, jdbg, true);
            jinv = new JINV(jdbg);
            jctrl = new JCTRL(this, jdbg, true);
            jlcd.UpdateFullScreen(Me, thisScript);
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
            // Simplify code slightly
            if (argument == null) argument = "";

            // ---------------------------------------------------------------------------
            // Decide whether to actually do anything, and update status every 10 seconds
            // ---------------------------------------------------------------------------
            if (stayRunning && argument.Equals("")) {
                TimeSpan timeSinceLastCheck = DateTime.Now - lastCheck;
                if (timeSinceLastCheck.TotalSeconds >= refreshSpeed) {
                    lastCheck = DateTime.Now;
                } else {
                    return;  // Nothing to do
                }
            }

            try {
                // ---------------------------------------------------------------------------
                // We only get here if we are refreshing
                // ---------------------------------------------------------------------------
                // Dummy debug - so we do the lookup of lcds
                jdbg.ClearDebugLCDs();  // Clear the debug screens

                jdbg.DebugAndEcho(thisScript + " running at " + DateTime.Now.ToShortTimeString() + " with arg: " + argument);

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
                    mytag = (mytag.Split(';')[0]).Trim().ToUpper();  // Remove any trailing comments
                    Echo("Using tag of " + mytag);
                } else {
                    Echo("No tag configured\nPlease add [config] for tag=<substring>");
                    return;
                }
                jdbg.Debug("Config: tag=" + mytag);

                // -----------------------------------
                // Identify the connector to work with
                // -----------------------------------
                IMyShipConnector myShipConnector = null;
                List<IMyTerminalBlock> connectors = new List<IMyTerminalBlock>();
                GridTerminalSystem.GetBlocksOfType(connectors, (IMyTerminalBlock x) => (
                                                                                       (x is IMyShipConnector) &&
                                                                                       (x.CustomName.ToUpper().IndexOf("[" + mytag.ToUpper() + "]") >= 0) &&
                                                                                       (x.CubeGrid.Equals(Me.CubeGrid))
                                                                                      ));
                jdbg.Debug("Found " + connectors.Count + " connectors with the tag");
                if (connectors.Count == 0) {
                    jdbg.DebugAndEcho("ERROR: No connector with the tag [" + mytag + "]");
                    return;
                } else if (connectors.Count > 1) {
                    jdbg.DebugAndEcho("ERROR: Too many connectors with the tag [" + mytag + "]");
                    return;
                } else { 
                    jdbg.DebugAndEcho("Using connector " + connectors[0].CustomName);
                    myShipConnector = connectors[0] as IMyShipConnector;
                }

                // -------------------------------------------
                // Identify the lcd to update with docked info
                // -------------------------------------------
                List<IMyTerminalBlock> myDockedStatusLCDs = jlcd.GetLCDsWithTag(mytag);
                if (myDockedStatusLCDs.Count == 0) {
                    jdbg.DebugAndEcho("Warning LCD with tag '" + mytag + "' does not exist, so no status display possible");
                }

                // -------------------------------------------
                // Identify the 4 button panels to use for input
                // -------------------------------------------
                //IMyTerminalBlock my4ButtonPanel = null;
                List<IMyTerminalBlock> ordered4butpanels = new List<IMyTerminalBlock>();

                bool finished = false;
                int counter = 1;
                do {
                    List <IMyTerminalBlock> but4panels = new List<IMyTerminalBlock>();
                    String lookforTag = "[" + mytag.ToUpper() +
                                               ((counter > 1) ? ":" + counter : "") + "]";

                    GridTerminalSystem.GetBlocksOfType(but4panels, (IMyTerminalBlock x) => (
                                                                                           (x is IMyTextSurfaceProvider) &&
                                                                                           (x.CustomName.ToUpper().IndexOf(lookforTag) >= 0) &&
                                                                                           (x.CubeGrid.Equals(Me.CubeGrid)) &&
                                                                                           (((IMyTextSurfaceProvider)x).SurfaceCount == 4)
                                                                                          ));
                    jdbg.Debug("Found " + but4panels.Count + " 4 button panels with the tag " + lookforTag);
                    if (but4panels.Count == 0) {
                        jdbg.DebugAndEcho("No 4 button panel with the tag " + lookforTag);
                        finished = true;
                    } else if (but4panels.Count > 1) {
                        jdbg.DebugAndEcho("ERROR: Too many 4 button panels with the tag " + lookforTag);
                        finished = true;
                        return;
                    } else {
                        jdbg.DebugAndEcho("... which is " + but4panels[0].CustomName);
                        finished = false;
                        ordered4butpanels.Add(but4panels[0]);
                    }
                    counter++;
                } while (finished == false);

                if (counter == 1) {
                    jdbg.DebugAndEcho("ERROR: No suitable 4 button panel found - aborting");
                    return;
                }

                // --------------------------------------------------------------
                // Remove the connector and the 4 button panel from the LCD list!
                // --------------------------------------------------------------
                jdbg.Debug("LCD Count before: " + myDockedStatusLCDs.Count);
                foreach (var thisPanel in ordered4butpanels) {
                    myDockedStatusLCDs.Remove(thisPanel);
                }
                jdbg.Debug("LCD Count removed 4buts: " + myDockedStatusLCDs.Count);
                myDockedStatusLCDs.Remove(myShipConnector);
                jdbg.Debug("LCD Count removed connector: " + myDockedStatusLCDs.Count);

                // ----------------------------------------------------------
                // Identify the divider syntax to use (Expected: [] {} or  .)
                // ----------------------------------------------------------
                String divName = _ini.Get("config", "divider").ToString(" .");
                if ((divName == null) || (divName.Equals(""))) divName = " .";

                if (divName.Length != 2) {
                    jdbg.DebugAndEcho("Invalid divider - must be 2 'unique' chars");
                    return;
                }

                Char leftDiv = divName[0];
                Char rightDiv = divName[1];

                // ---------------------------------------------------------------------------
                // vvv                   Now do the actual work                           vvvv
                // ---------------------------------------------------------------------------
                maxSize = (ordered4butpanels.Count * 4) - 1;

                jdbg.Debug("Doing the work now");
                // 1. See docked status has changed - if so update the LCD and connector data
                String newName = "";

                if (!(myShipConnector.IsConnected)) {
                    jdbg.Debug("Nothing connected");
                    originalName = "";
                    isConfirm = false;

                    // To save processing time, just wipe the customData
                    myShipConnector.CustomData = "";
                    jdbg.Debug("Updating screens");
                    updateStatusLCD(myDockedStatusLCDs, "Nothing Connected");
                    update4But("".PadRight(maxSize, '-'), ordered4butpanels, isConfirm, true);
                    jdbg.DebugAndEcho("Nothing connected - ending");
                    return;
                } else {

                    // If no custom data, set it
                    if ((originalName.Equals("")) ||
                        (myShipConnector.CustomData == null) ||
                        (myShipConnector.CustomData.Equals(""))) {

                        jdbg.Debug("No custom data yet, looking it up from the docked ship");
                        newName = myShipConnector.OtherConnector.CustomName;

                        String nameLeft = newName;
                        if (leftDiv != ' ' && leftDiv == newName[0]) {
                            nameLeft = newName.Substring(1);  // Strip first char
                        }

                        // If we match the right as well, we know the chars to use
                        if (newName.IndexOf(rightDiv) >= 0) {
                            newName = newName.Substring(0, newName.IndexOf(rightDiv));
                            originalName = newName;
                        } else {
                            newName = "";
                            originalName = "???";
                        }

                        if (newName.Length > maxSize) newName = newName.Substring(0, maxSize);

                        myShipConnector.CustomData = newName;
                        jdbg.Debug("Calculated result of " + newName);
                    } else {
                        newName = myShipConnector.CustomData;
                        jdbg.Debug("Read in custom data of " + newName);
                    }

                    jdbg.Debug("Updating screens");
                    updateStatusLCD(myDockedStatusLCDs, "Docked Ship: " + originalName);
                }

                // Poll refresh - Just update the buttons
                if (argument.Equals("")) {
                    jdbg.Debug("No Parms");
                    update4But(newName, ordered4butpanels, isConfirm, false);

                    // Expected button press
                } else if (argument.StartsWith("B")) {
                    isConfirm = false;
                    jdbg.Debug("Handling button " + argument);
                    String number = argument.Substring(1);
                    int idx = int.Parse(number);
                    newName = handlePress(newName, idx);
                    jdbg.Debug("Calc name of " + newName);
                    update4But(newName, ordered4butpanels, isConfirm, false);
                    myShipConnector.CustomData = newName;
                    jdbg.DebugAndEcho("Button handled... ending");
                    return;

                } else if (argument.Equals("GO") && !isConfirm) {
                    jdbg.Debug("Was GO");
                    isConfirm = true;
                    update4But(newName, ordered4butpanels, isConfirm, false);

                } else if (argument.Equals("GO") && isConfirm) {
                    jdbg.Debug("Was GO but confirming!");
                    // Work out the CubeGrid of the connected ship
                    IMyCubeGrid processGrid = myShipConnector.OtherConnector.CubeGrid;
                    List<IMyTerminalBlock> blocks = new List<IMyTerminalBlock>();
                    GridTerminalSystem.GetBlocksOfType(blocks, (IMyTerminalBlock x) => (
                                                                          x.CubeGrid.Equals(processGrid)
                                                                                       ));

                    int count = 0;
                    foreach (var thisblock in blocks) {
                        if (count < 5) jdbg.Debug("Processing block: " + thisblock.CustomName);

                        String currentName = thisblock.CustomName;
                        jdbg.Debug("Original: " + currentName);

                        // Remove any old tag
                        if ((leftDiv == ' ' || currentName.StartsWith("" + leftDiv)) && (currentName.IndexOf(rightDiv) >= 0)) {
                            currentName = currentName.Substring(currentName.IndexOf(rightDiv) + 1);
                            jdbg.Debug("Stripped: " + currentName);
                        }

                        if (argument != null && !newName.Equals("") && !newName.Equals("---")) {
                            if (leftDiv != ' ') {
                                currentName = "" + leftDiv + newName + rightDiv + currentName;
                            } else {
                                currentName = "" + newName + rightDiv + currentName;
                            }
                        }
                        jdbg.Debug("NewName: " + currentName);
                        thisblock.CustomName = currentName;

                        count++;
                    }
                    jdbg.Debug("Processed " + count + " blocks in total");

                    originalName = newName;
                    isConfirm = false;
                    update4But(newName, ordered4butpanels, isConfirm, false);
                    updateStatusLCD(myDockedStatusLCDs, "Docked Ship: " + originalName);
                } else {
                    jdbg.DebugAndEcho("ERROR: Unexpected arg: " + argument);
                    return;
                }
                jdbg.Alert("Completed", "GREEN", alertTag, thisScript);
            }
            catch (Exception ex) {
                jdbg.Alert("Exception - " + ex.ToString() + "\n" + ex.StackTrace, "RED", alertTag, thisScript);
            }
        }

        /* Update the status LCDs */
        void updateStatusLCD(List<IMyTerminalBlock> lcds, String text)
        {
            jlcd.InitializeLCDs(lcds, TextAlignment.CENTER);
            jlcd.SetupFont(lcds, 1, text.Length + 4, false);  // 4 is just to provide some level of indentation
            jlcd.WriteToAllLCDs(lcds, text, false);
        }

        /* Update the button LCDs - Note this cannot use jlcd due to the multiple surfaces */
        void update4But(String prefix, List<IMyTerminalBlock> ordered4butPanels, bool isConfirm, bool isDisconnected)
        {
            jdbg.Debug("Updating buttons... prefix: " + prefix + " confirm?" + isConfirm + ", isDisc?" + isDisconnected);

            int count = 0;
            prefix = prefix.PadRight(ordered4butPanels.Count * 4, ' ');
            foreach (var my4butPanels in ordered4butPanels) {

                jdbg.Debug("Panel: " + my4butPanels.CustomName);
                jdbg.Debug("Displays: " + ((IMyTextSurfaceProvider)my4butPanels).SurfaceCount);
                for (int i = 0; i < 4; i++) {

                    // Final panel cant use the last button
                    if (i < 3 || (count < (ordered4butPanels.Count - 1))) {

                        IMyTextSurface thisLCD = ((IMyTextSurfaceProvider)my4butPanels).GetSurface(i);
                        if (thisLCD != null) {
                            jdbg.Debug("Updating surface " + i + " with " + prefix[i + (count * 4)]);
                            thisLCD.Font = "Monospace";
                            thisLCD.ContentType = ContentType.TEXT_AND_IMAGE;
                            thisLCD.Alignment = VRage.Game.GUI.TextPanel.TextAlignment.CENTER;
                            thisLCD.FontSize = 10.0F;
                            thisLCD.WriteText("" + prefix[i + (count * 4)], false);
                        } else {
                            jdbg.Debug("Odd = 4 button panel doesnt have surface " + i);
                        }
                    } else {
                        // Now action button
                        IMyTextSurface thisLCD = ((IMyTextSurfaceProvider)my4butPanels).GetSurface(3);
                        if (thisLCD != null) {
                            String text = "GO?";
                            float fontsize = 9.0F;
                            if (prefix.Equals("".PadRight(ordered4butPanels.Count * 4, ' '))) {
                                text = "Clear?";
                                fontsize = 4.0F;
                            }
                            if (isConfirm) {
                                text = "Really\nGO?";
                                fontsize = 4.0F;
                            }
                            if (isDisconnected) text = "N/A";
                            jdbg.Debug("Updating surface " + i + " with :" + text);
                            thisLCD.Font = "Monospace";
                            thisLCD.ContentType = ContentType.TEXT_AND_IMAGE;
                            thisLCD.Alignment = VRage.Game.GUI.TextPanel.TextAlignment.CENTER;
                            thisLCD.FontSize = fontsize;
                            thisLCD.WriteText(text, false);
                        }
                    }
                }
                count = count + 1;
            }
        }

        /* Handle a button being pressed */
        String handlePress(String prefix, int index)
        {
            jdbg.Debug("Handle press of " + index + " with prefix " + prefix + "(" + prefix.Length + ")");
            String allChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_ ";
            char[] prefixChars = prefix.PadRight(maxSize, ' ').ToCharArray();
            char whichChar = prefixChars[index];
            jdbg.Debug("orig char is " + prefixChars[index]);
            int idx = allChars.IndexOf(whichChar);

            if (idx == -1) {
                jdbg.DebugAndEcho("Unexpected data in customdata - ABORTING");
                return prefix;
            }

            idx++;
            if (idx == allChars.Length) idx = 0;
            prefixChars[index] = allChars[idx];
            jdbg.Debug("new char is " + prefixChars[index]);
            return new String(prefixChars).Trim();
        }

        // ----------------------------- CUT -------------------------------------
    }
}
