using Sandbox.Game.EntityComponents;
using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;
using SpaceEngineers.Game.ModAPI.Ingame;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
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
using static IngameScript.Program;

namespace IngameScript
{
    partial class Program : MyGridProgram
    {
        //String thisScript = "ControllerTesting";

        // Development or user config time flags
        bool debug = true;  

        // Private variables
        private JDBG jdbg = null;
        //private JINV jinv = null;
        private JLCD jlcd = null;
        private JCTRL jctrl = null;
        List<IMyTerminalBlock> displays = null;
        IMyShipController controller = null;
        String mytag;

        public Program()
        {
            Echo("Start");
            jdbg = new JDBG(this, debug);
            jlcd = new JLCD(this, jdbg, false);
            jctrl = new JCTRL(this, jdbg, false);

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

            // ---------------------------------------------------------------------------
            // Find the seat
            // ---------------------------------------------------------------------------
            List<IMyTerminalBlock> Controllers = new List<IMyTerminalBlock>();
            GridTerminalSystem.GetBlocksOfType(Controllers, (IMyTerminalBlock x) => (
                                                                                   (x.CustomName != null) &&
                                                                                   (x.CustomName.IndexOf("[" + mytag + "SEAT]") >= 0) &&
                                                                                   (x is IMyShipController)
                                                                                  ));
            Echo("Found " + Controllers.Count + " controllers");

            if (Controllers.Count > 0) {
                foreach (var thisblock in Controllers) {
                    jdbg.Debug("- " + thisblock.CustomName);
                }
                if (Controllers.Count > 1) {
                    Echo("Too many controllers");
                    return;
                }
                controller = (IMyShipController)Controllers[0];
            } else if (Controllers.Count == 0) {
                Echo("No controllers");
                return;
            }

            // Get all the LCDs we are going to output to
            displays = jlcd.GetLCDsWithTag(mytag + "SCREEN");
            Echo("Found " + displays.Count + " displays");

            // Note width/height reversed as screen is rotated 90%
            jlcd.SetupFontCustom(displays, 40, 80, false, 0.001F, 0.001F);

            // Initialize the LCDs
            jlcd.InitializeLCDs(displays, TextAlignment.LEFT);

            // Run every tick
            Runtime.UpdateFrequency = UpdateFrequency.Update1;

        }

        public void Save()
        {
        }

        public void Main(string argument, UpdateType updateSource)
        {
            jdbg.ClearDebugLCDs();
            if (argument == null) {
                jdbg.DebugAndEcho("Launched with empty parms" + argument);
            } else {
                jdbg.DebugAndEcho("Launched with parms '" + argument + "'");
            }

            String screen = "Currently pressed:\n";
            screen += " - Move: " + controller.MoveIndicator + "\n";
            screen += " - Roll: " + controller.RollIndicator + "\n";
            screen += " - Rotation: " + controller.RotationIndicator + "\n";

            if (jctrl.IsLeft(controller)) {
                screen += " - LEFT\n";
            } else {
                screen += "\n";
            }
            if (jctrl.IsRight(controller)) {
                screen += " - RIGHT\n";
            } else {
                screen += "\n";
            }
            if (jctrl.IsUp(controller)) {
                screen += " - UP\n";
            } else {
                screen += "\n";
            }
            if (jctrl.IsDown(controller)) {
                screen += " - DOWN\n";
            } else {
                screen += "\n";
            }
            if (jctrl.IsSpace(controller)) {
                screen += " - SPACE\n";
            } else {
                screen += "\n";
            }
            if (jctrl.IsCrouch(controller)) {
                screen += " - CROUCH\n";
            } else {
                screen += "\n";
            }
            if (jctrl.IsRollLeft(controller)) {
                screen += " - Q\n";
            } else {
                screen += "\n";
            }
            if (jctrl.IsRollRight(controller)) {
                screen += " - E\n";
            } else {
                screen += "\n";
            }
            if (jctrl.IsArrowRight(controller)) {
                screen += " - Arrow R\n";
            } else {
                screen += "\n";
            }
            if (jctrl.IsArrowLeft(controller)) {
                screen += " - Arrow L\n";
            } else {
                screen += "\n";
            }
            if (jctrl.IsArrowDown(controller)) {
                screen += " - Arrow D\n";
            } else {
                screen += "\n";
            }
            if (jctrl.IsArrowUp(controller)) {
                screen += " - Arrow U\n";
            } else {
                screen += "\n";
            }
            jlcd.WriteToAllLCDs(displays, screen, false);

        }
    }
}
