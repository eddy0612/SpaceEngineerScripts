using IngameScript;
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
        private bool debug = true;

        private Core mycore = null;
        private JDBG jdbg = null;
        private JLCD jlcd = null;
        private JCTRL jctrl = null;

        List<IMyTerminalBlock> p1LCDs = null;
        IMyShipController p1Ctrl = null;

        public Program()
        {
            jdbg = new JDBG(this, debug);
            jlcd = new JLCD(this, jdbg, true);
            jctrl = new JCTRL(this, jdbg, true);

            mycore = new Core(this);   /* Creates and initializes the cpu with default 48K rom */
            /* TODO: Load game */

            p1LCDs = jlcd.GetLCDsWithTag("GAMESCREEN");
            if (p1LCDs.Count == 0) {
                Echo("ERROR: No screens found - Please tag screen as GAMESCREEN");
                throw new Exception("Could not identify screen for p1 with tag GAMESCREEN");
            } else if (p1LCDs.Count != 1) {
                Echo("ERROR: " + p1LCDs.Count + " screens found - Only 1 supported");
                throw new Exception("Too many screens for p1 with tag GAMESCREEN");
            }

            List<IMyTerminalBlock> p1CTRLs = jctrl.GetCTRLsWithTag("GAMESEAT");
            if (p1CTRLs.Count != 1) {
                Echo("ERROR: " + p1CTRLs.Count + " controllers found for player 1. Please tag a seat with GAMESEAT");
                throw new Exception("Could not identify controller for p1 with tag GAMESEAT");
            } else {
                p1Ctrl = (IMyShipController)p1CTRLs[0];
            }

            jlcd.InitializeLCDs(p1LCDs, TextAlignment.LEFT);
            jlcd.SetupFontCustom(p1LCDs, 312, 416, true, 0.001F, 0.001F);
            //Runtime.UpdateFrequency = UpdateFrequency.Update1;
        }

        public void Save()
        {
        }

        public void Main(string argument, UpdateType updateSource)
        {
            Echo("Main called");
            /* Convert controller to keys */
            /* IO_KeyPress...*/
            /* Run some processor instructions */
            mycore.CPU_Execute_Block();

            /* Draw text */
            String screen = new string(mycore.videoScreenDataLCD);
            Echo("Drawing: " + screen.Length + " chars");
            jlcd.WriteToAllLCDs(p1LCDs, screen, false);
            Echo("Drawn");
        }


    }
}
