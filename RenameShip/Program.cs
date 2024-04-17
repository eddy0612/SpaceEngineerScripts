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
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Net.Sockets;
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
using VRage.Game.ObjectBuilders.Definitions;
using VRageMath;
//using static VRage.Game.MyObjectBuilder_CurveDefinition;
//using static VRage.Game.ObjectBuilders.Definitions.MyObjectBuilder_GameDefinition;
//using static VRage.Game.ObjectBuilders.Definitions.MyObjectBuilder_GameDefinition;

namespace IngameScript
{
    partial class Program : MyGridProgram
    {

        // ----------------------------- CUT -------------------------------------
        // Example custom data in programming block:
        // [Config]
        // connector=[rename] Connector
        // 4buttonpanel=Rename 4 button[renamelcd]
        // dockedlcd = [renamelcd] Docked LCD

        // Development time flags
        bool debug = false;
        bool stayRunning = true;
        String thisScript = "RenameShip";
        String alerttag = "alert";

        // My configuration
        // <None>
        int refreshSpeed = 10;
        String originalName = "";
        bool isConfirm = false;

        // Internals
        DateTime lastCheck = new DateTime(0);

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


        // ---------------------------------------------------------------------------
        // Constructor
        // ---------------------------------------------------------------------------
        public Program()
        {
            // Run every 100 ticks, but relies on internal check to only actually
            // perform on a defined frequency
            if (stayRunning) Runtime.UpdateFrequency = UpdateFrequency.Update100;
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
            // Decide whether to actually do anything
            // ---------------------------------------------------------------------------
            // Dummy debug - so we do the lookup of lcds
            if (stayRunning && argument.Equals(""))
            {
                TimeSpan timeSinceLastCheck = DateTime.Now - lastCheck;
                if (timeSinceLastCheck.TotalSeconds >= refreshSpeed)
                {
                    lastCheck = DateTime.Now;
                }
                else
                {
                    return;  // Nothing to do
                }
            }

            // ---------------------------------------------------------------------------
            // We only get here if we are refreshing
            // ---------------------------------------------------------------------------
            // Dummy debug - so we do the lookup of lcds
            WriteToAllLCDs(debugLCDs, "", false);  // Clear the debug screens

            DebugAndEcho(thisScript + " running at " + lastCheck.ToString() + " with arg: " + argument);

            // ---------------------------------------------------------------------------
            // Get my custom data and parse to get the config
            // ---------------------------------------------------------------------------
            MyIniParseResult result;
            MyIni _ini = new MyIni();
            if (!_ini.TryParse(Me.CustomData, out result))
                throw new Exception(result.ToString());

            // -----------------------------------
            // Identify the connector to work with
            // -----------------------------------
            IMyShipConnector myShipConnector = null;
            String connName = _ini.Get("config", "connector").ToString();
            if (connName != null)
            {
                if (connName.Equals(""))
                {
                    DebugAndEcho("ERROR: No connector defined");
                    return;
                } else
                {
                    DebugAndEcho("Using connector " + connName);
                    myShipConnector = GridTerminalSystem.GetBlockWithName(connName) as IMyShipConnector;
                    if (myShipConnector == null)
                    {
                        DebugAndEcho("ERROR: No connector defined");
                        return;
                    }
                }
            }
            else
            {
                DebugAndEcho("Error: No prefix configured\nPlease add [config] for prefix=<string>");
                return;
            }

            // -------------------------------------------
            // Identify the lcd to update with docked info
            // -------------------------------------------
            String myLCD = _ini.Get("config", "dockedlcd").ToString();
            IMyTextSurfaceProvider myDockedLCD = null;
            if ((myLCD != null) && (!myLCD.Equals("")))
            {
                myDockedLCD = GridTerminalSystem.GetBlockWithName(myLCD) as IMyTextSurfaceProvider;
                if (myDockedLCD == null)
                {
                    DebugAndEcho("Configure error - LCD '" + myLCD + "' does not exist. Aborting");
                    return;
                }
                else
                {
                    DebugAndEcho("My lcd is '" + myLCD + "'");
                }
            }
            else
            {
                DebugAndEcho("No LCD configured - ignoring display");
                return;
            }

            // -------------------------------------------
            // Identify the 4 button panel to use for input
            // -------------------------------------------
            String my4butname = _ini.Get("config", "4buttonpanel").ToString();
            IMyTerminalBlock my4butPanels = null;
            if ((my4butname != null) && (!my4butname.Equals("")))
            {
                my4butPanels = GridTerminalSystem.GetBlockWithName(my4butname) as IMyTerminalBlock;
                if (my4butPanels == null)
                {
                    DebugAndEcho("Configure error - 4 but panel '" + my4butname + "' does not exist. Aborting");
                    return;
                }
                else
                {
                    DebugAndEcho("My 4 button block is '" + my4butname + "'");
                }
            }
            else
            {
                DebugAndEcho("No 4 button panel configured - Aborting");
                return;
            }

            // ----------------------------------------------------------
            // Identify the divider syntax to use (Expected: [] {} or  .)
            // ----------------------------------------------------------
            String divName = _ini.Get("config", "divider").ToString();
            if ((divName != null) && (!divName.Equals(""))) divName = " .";

            if (divName.Length != 2)
            {
                DebugAndEcho("Invalid divider - must be 2 'unique' chars");
                return;
            }

            Char leftDiv =  divName[0];
            Char  rightDiv = divName[1];

            // ---------------------------------------------------------------------------
            // vvv                   Now do the actual work                           vvvv
            // ---------------------------------------------------------------------------

            Debug("Doing the work now");
            // 1. See docked status has changed - if so update the LCD and connector data
            String newName = "---";

            if (!(myShipConnector.IsConnected))
            {
                Debug("Nothing connected");
                originalName = "";
                isConfirm = false;

                // To save processing time, just wipe the customData
                myShipConnector.CustomData = "";
                Debug("Updating screens");
                updateLCD(myDockedLCD, "Nothing Connected");
                update4But("---", my4butPanels, isConfirm, true);
                DebugAndEcho("Nothing connected - ending");
                return;
            }
            else
            {

                // If no custom data, set it
                if ((originalName.Equals("")) ||
                    (myShipConnector.CustomData == null) || 
                    (myShipConnector.Equals("")) ||
                    (myShipConnector.CustomData.Length != 3))
                {
                    Debug("No custom data yet, looking it up from the docked ship");
                    newName = myShipConnector.OtherConnector.CustomName;
                    if (newName.Length < 5) newName = newName + "     ";

                    String nameLeft = newName;
                    if (leftDiv != ' ' && leftDiv == newName[0])
                    {
                        nameLeft = newName.Substring(1);  // Strip first char
                    }

                    // If we match the right as well, we know the chars to use
                    if (rightDiv == newName[3])
                    {
                        newName = newName.Substring(0, 3);
                        originalName = newName;
                    }
                    else
                    {
                        newName = "---";
                        originalName = "???";
                    }
                    myShipConnector.CustomData = newName;
                    Debug("Calculated result of " + newName);
                }
                else
                {
                    newName = myShipConnector.CustomData;
                    Debug("Read in custom data of " + newName);
                }

                Debug("Updating screens");
                updateLCD(myDockedLCD, "Docked Ship: " + originalName);
            }

            // Poll refresh - Just update the buttons
            if (argument.Equals(""))
            {
                Debug("No Parms");
                update4But(newName, my4butPanels, isConfirm, false);

                // Expected button press
            }
            else if (argument.StartsWith("B"))
            {
                isConfirm = false;
                Debug("Handling button " + argument);
                int idx = argument[1] - '1';
                newName = handlePress(newName, idx);
                Debug("Calc name of " + newName);
                update4But(newName, my4butPanels, isConfirm, false);
                myShipConnector.CustomData = newName;
                DebugAndEcho("Button handled... ending");
                return;
            }
            else if (argument.Equals("GO") && !isConfirm)
            {
                Debug("Was GO");
                isConfirm = true;
                update4But(newName, my4butPanels, isConfirm, false);
            }
            else if (argument.Equals("GO") && isConfirm)
            {
                Debug("Was GO but confirming!");
                // Work out the CubeGrid of the connected ship
                IMyCubeGrid processGrid = myShipConnector.OtherConnector.CubeGrid;
                List<IMyTerminalBlock> blocks = new List<IMyTerminalBlock>();
                GridTerminalSystem.GetBlocksOfType(blocks, (IMyTerminalBlock x) => (
                                                                      x.CubeGrid.Equals(processGrid)
                                                                                   ));

                int count = 0;
                foreach (var thisblock in blocks)
                {
                    if (count < 5) Debug("Processing block: " + thisblock.CustomName);

                    String currentName = thisblock.CustomName;
                    Debug("Original: " + currentName);

                    // Remove any old tag
                    if ((leftDiv == ' ' || currentName.StartsWith(""+leftDiv)) && (currentName.IndexOf(rightDiv) >= 0))
                    {
                        currentName = currentName.Substring(currentName.IndexOf(rightDiv) + 1);
                        Debug("Stripped: " + currentName);
                    }

                    if (argument != null && !newName.Equals("") && !newName.Equals("---"))
                    {
                        if (leftDiv != ' ')
                        {
                            currentName = "" + leftDiv + newName + rightDiv + currentName;
                        } else
                        {
                            currentName = "" + newName + rightDiv + currentName;
                        }
                    }
                    Debug("NewName: " + currentName);
                    thisblock.CustomName = currentName;

                    count++;
                }
                Debug("Processed " + count + " blocks in total");

                originalName = newName;
                isConfirm = false;
                update4But(newName, my4butPanels, isConfirm, false);
                updateLCD(myDockedLCD, "Docked Ship: " + originalName);

            }
            else
            {
                DebugAndEcho("ERROR: Unexpected arg: " + argument);
                return;
            }
        }

        void updateLCD(IMyTextSurfaceProvider lcd, String text)
        {
            IMyTextSurface thisLCD = ((IMyTextSurfaceProvider)lcd).GetSurface(0);
            if (thisLCD != null)
            {
                Debug("Updating lcd with " + text);
                thisLCD.Font = "Monospace";
                thisLCD.ContentType = ContentType.TEXT_AND_IMAGE;
                thisLCD.Alignment = VRage.Game.GUI.TextPanel.TextAlignment.CENTER;
                thisLCD.FontSize = 5.0F;
                thisLCD.WriteText(text, false);
            }
        }
        void update4But(String prefix, IMyTerminalBlock my4butPanels, bool isConfirm, bool isDisconnected)
        {
            for (int i = 0; i < 3; i++)
            {
                IMyTextSurface thisLCD = ((IMyTextSurfaceProvider)my4butPanels).GetSurface(i);
                if (thisLCD != null)
                {
                    Debug("Updating surface " + i + " with " + prefix[i]);
                    thisLCD.Font = "Monospace";
                    thisLCD.ContentType = ContentType.TEXT_AND_IMAGE;
                    thisLCD.Alignment = VRage.Game.GUI.TextPanel.TextAlignment.CENTER;
                    thisLCD.FontSize = 10.0F;
                    thisLCD.WriteText("" + prefix[i], false);
                }
                else
                {
                    Debug("Odd = 4 button panel doesnt have surface " + i);
                }

                // Now action button
                thisLCD = ((IMyTextSurfaceProvider)my4butPanels).GetSurface(3);
                if (thisLCD != null)
                {
                    String text = "GO?";
                    float fontsize = 9.0F;
                    if (prefix.Equals("---"))
                    {
                        text = "Clear?";
                        fontsize = 4.0F;
                    }
                    if (isConfirm)
                    {
                        text = "Really\nGO?";
                        fontsize = 4.0F;
                    }
                    if (isDisconnected) text = "N/A";
                    Debug("Updating surface " + i + " with " + prefix[i]);
                    thisLCD.Font = "Monospace";
                    thisLCD.ContentType = ContentType.TEXT_AND_IMAGE;
                    thisLCD.Alignment = VRage.Game.GUI.TextPanel.TextAlignment.CENTER;
                    thisLCD.FontSize = fontsize;
                    thisLCD.WriteText(text, false);
                }
            }
        }

        String handlePress(String prefix, int index)
        {
            String allChars = "ABDCEFGHIJKLMNOPQRSTUVWXYZ0123456789-";
            char[] prefixChars = prefix.ToCharArray();
            char whichChar = prefixChars[index];
            Debug("orig char is " + prefixChars[index]);
            int idx = allChars.IndexOf(whichChar);

            if (idx == -1)
            {
                DebugAndEcho("Unexpected data in customdata - ABORTING");
                return prefix;
            }

            idx++;
            if (idx == allChars.Length) idx = 0;
            prefixChars[index] = allChars[idx];
            Debug("new char is " + prefixChars[index]);
            return new String(prefixChars);
        }

        // ===========================================================================
        // Common functions from here downwards
        // ===========================================================================


        // ---------------------------------------------------------------------------
        // Simple wrapper to completely redraw our LCD
        // ---------------------------------------------------------------------------
        void DrawLCD(String tag, String screenContents)
        {
            List<IMyTerminalBlock> drawLCDs = new List<IMyTerminalBlock>();
            GridTerminalSystem.GetBlocksOfType(drawLCDs, (IMyTerminalBlock x) => (
                                                                                   (x.CustomName != null) &&
                                                                                   (x.CustomName.IndexOf("[" + tag + "]") >= 0) &&
                                                                                   (x is IMyTextSurfaceProvider)
                                                                                  ));
            WriteToAllLCDs(drawLCDs, screenContents, false);
        }

        // ---------------------------------------------------------------------------
        // Simple wrapper to decide whether to output to the console or not
        // ---------------------------------------------------------------------------
        List<IMyTerminalBlock> debugLCDs = null;
        bool inDebug = false;
        void DebugAndEcho(String str)
        {
            Debug(str);
            Echo(str);
        }

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
                WriteToAllLCDs(allBlocksWithLCDs, alertOutput, false); //JJJ
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
