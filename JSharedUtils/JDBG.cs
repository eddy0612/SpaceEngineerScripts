using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using VRage.Game.GUI.TextPanel;

namespace IngameScript
{
    partial class Program
    {
        public class JDBG
        {
            public bool debug = false;                          /* Are we logging dbg messages */

            private MyGridProgram mypgm = null;                 /* Main pgm if needed */
            private JLCD jlcd = null;                           /* JLCD class */

            private bool inDebug = false;                       /* Avoid recursion: */
            private static List<IMyTerminalBlock> debugLCDs = null;    /* LCDs to write to */

            // ---------------------------------------------------------
            // Constructor
            // ---------------------------------------------------------
            public JDBG(MyGridProgram pgm, bool debugState)
            {
                mypgm = pgm;
                debug = debugState;
                jlcd = new JLCD(pgm, this, false);
            }

            // ---------------------------------------------------------
            // Echo - write a message to the console
            // ---------------------------------------------------------
            public void Echo(string str)
            {
                mypgm.Echo("JDBG: " + str);
            }

            // ---------------------------------------------------------
            // Debug - write a message to the debug LCD
            // ---------------------------------------------------------
            public void Debug(String str)
            {
                Debug(str, true);
            }
            public void Debug(String str, bool consoledbg)
            {
                if (debug && !inDebug)
                {
                    inDebug = true;

                    if (debugLCDs == null)
                    {
                        Echo("First run - working out debug panels");
                        initializeDBGLCDs();
                        ClearDebugLCDs();
                    }

                    Echo("D:" + str);
                    jlcd.WriteToAllLCDs(debugLCDs, str + "\n", true);
                    inDebug = false;
                }
            }

            // ---------------------------------------------------------
            // Debug - write a message to the debug LCD
            // ---------------------------------------------------------
            public void DebugAndEcho(String str)
            {
                Echo(str);          // Will always come out
                Debug(str, false);  // Will only come out if debug is on
            }

            // ---------------------------------------------------------
            // InitializeLCDs - cache the debug LCD
            // ---------------------------------------------------------
            private void initializeDBGLCDs()
            {
                inDebug = true;
                debugLCDs = jlcd.GetLCDsWithTag("DEBUG");
                jlcd.InitializeLCDs(debugLCDs, TextAlignment.LEFT);
                inDebug = false;
            }

            // ---------------------------------------------------------
            // ClearDebugLCDs - clear the debug LCD
            // ---------------------------------------------------------
            public void ClearDebugLCDs()
            {
                if (debug) {
                    if (debugLCDs == null) {
                        Echo("First runC - working out debug panels");
                        initializeDBGLCDs();
                    }
                    jlcd.WriteToAllLCDs(debugLCDs, "", false);  // Clear the debug screens
                }
            }

            // ---------------------------------------------------------------------------
            // Simple wrapper to write to a central alert - Needs work to support wrapping
            // ---------------------------------------------------------------------------
            public void Alert(String alertMsg, String colour, String alertTag, String thisScript)
            {
                List<IMyTerminalBlock> allBlocksWithLCDs = new List<IMyTerminalBlock>();
                mypgm.GridTerminalSystem.GetBlocksOfType(allBlocksWithLCDs, (IMyTerminalBlock x) => (
                                                                                          (x.CustomName != null) &&
                                                                                          (x.CustomName.IndexOf("[" + alertTag + "]") >= 0) &&
                                                                                          (x is IMyTextSurfaceProvider)
                                                                                         ));
                DebugAndEcho("Found " + allBlocksWithLCDs.Count + " lcds with '" + alertTag + "' to alert to");

                String alertOutput = JLCD.solidcolor[colour] + " " +
                                     DateTime.Now.ToShortTimeString() + ":" +
                                     thisScript + " " +
                                     alertMsg + "\n";
                DebugAndEcho("ALERT: " + alertMsg);
                if (allBlocksWithLCDs.Count > 0)
                {
                    jlcd.WriteToAllLCDs(allBlocksWithLCDs, alertOutput, true);
                }
            }

            // ---------------------------------------------------------------------------
            // Useful for complex scripts to see how far through the cpu slice you are
            // ---------------------------------------------------------------------------
            public void EchoCurrentInstructionCount(String tag)
            {
                Echo(tag + " instruction count: " + mypgm.Runtime.CurrentInstructionCount + "," + mypgm.Runtime.CurrentCallChainDepth);
            }
            public void EchoMaxInstructionCount()
            {
                Echo("Max instruction count: " + mypgm.Runtime.MaxInstructionCount + "," + mypgm.Runtime.MaxCallChainDepth);
            }
        }
    }
}