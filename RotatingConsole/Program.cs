using Sandbox.Game.EntityComponents;
using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;
using SpaceEngineers.Game.ModAPI.Ingame;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
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
        String thisScript = "RotatingConsole";

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
        int frameSkip = 1;
        int xRotation = 1;
        int yRotation = 0;
        int zRotation = 0;

        // My configuration
        int refreshSpeed = 1;
        String originalName = "";
        bool isConfirm = false;

        // Internals
        DateTime lastCheck = new DateTime(0);
        int counter = 0;

        // ---------------------------------------------------------------------------
        // Constructor
        // ---------------------------------------------------------------------------
        public Program()
        {
            // Run every 100 ticks, but relies on internal check to only actually
            // perform on a defined frequency
            if (stayRunning) Runtime.UpdateFrequency = UpdateFrequency.Update1;

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
        }

        // ---------------------------------------------------------------------------
        // Mainline code called every update frequency
        // ---------------------------------------------------------------------------
        public void Main(string argument, UpdateType updateSource)
        {
            // ---------------------------------------------------------------------------
            // Decide whether to actually do anything, and update status every 10 seconds
            // ---------------------------------------------------------------------------
            /*            if (stayRunning && argument.Equals("")) {
                            TimeSpan timeSinceLastCheck = DateTime.Now - lastCheck;
                            if (timeSinceLastCheck.TotalSeconds >= refreshSpeed) {
                                lastCheck = DateTime.Now;
                            } else {
                                return;  // Nothing to do
                            }
                        } */

            if (counter > 0) {counter--; return; }
            counter = 2;

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

                // Frame Skip
                frameSkip = _ini.Get("config", "frameskip").ToInt16(1);
                if (frameSkip < 1) {
                    Echo("frameSkip set invalidly - must be >= 1");
                    return;
                }

                // Frame Skip
                xRotation = _ini.Get("config", "xrotation").ToInt16(1);
                if (xRotation < -180 || xRotation > 180) {
                    Echo("xrotation set invalidly - must be between -180 .. +180");
                    return;
                }
                yRotation = _ini.Get("config", "yrotation").ToInt16(0);
                if (yRotation < -180 || yRotation > 180) {
                    Echo("yRotation set invalidly - must be between -180 .. +180");
                    return;
                }
                zRotation = _ini.Get("config", "zrotation").ToInt16(0);
                if (zRotation < -180 || zRotation > 180) {
                    Echo("zRotation set invalidly - must be between -180 .. +180");
                    return;
                }

                // -----------------------------------
                // Identify the consoles to work with
                // -----------------------------------
                List<IMyProjector> consoles = new List<IMyProjector>();
                GridTerminalSystem.GetBlocksOfType(consoles, (IMyProjector x) => (
                                                                                       (x is IMyProjector) &&
                                                                                       (x.CustomName.ToUpper().IndexOf("[" + mytag.ToUpper() + "]") >= 0) &&
                                                                                       (x.CubeGrid.Equals(Me.CubeGrid))
                                                                                      ));
                jdbg.Debug("Found " + consoles.Count + " consoles with the tag");
                if (consoles.Count == 0) {
                    jdbg.DebugAndEcho("ERROR: No connector with the tag [" + mytag + "]");
                    return;
                }

                foreach (var thisLCD in consoles) {
                    jdbg.Debug("Block: " + thisLCD.CustomName);
                    jdbg.Debug("     : " + thisLCD.ProjectionRotation);
                    
                    Vector3I rot = thisLCD.ProjectionRotation;
                    rot.X += xRotation; if (rot.X > 180) rot.X -= 360;
                    rot.Y += yRotation; if (rot.Y > 180) rot.Y -= 360;
                    rot.Z += zRotation; if (rot.Z > 180) rot.Z -= 360;
                    thisLCD.ProjectionRotation = rot;
                    jdbg.Debug(" ->  : " + thisLCD.ProjectionRotation);
                    thisLCD.UpdateOffsetAndRotation();
                }

                jdbg.Alert("Completed", "GREEN", alertTag, thisScript);
            }
            catch (Exception ex) {
                jdbg.Alert("Exception - " + ex.ToString() + "\n" + ex.StackTrace, "RED", alertTag, thisScript);
            }
        }
    }
}
