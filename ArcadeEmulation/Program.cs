using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using VRage.Game.ModAPI.Ingame.Utilities;
using VRage.Game.GUI.TextPanel;
using static IngameScript.Program;
using VRage.Game.ModAPI.Ingame;
using System.IO;
using VRage.Compression;

namespace IngameScript
{
    partial class Program : MyGridProgram
    {

        // ----------------------------- CUT -------------------------------------
        String thisScript = "ArcadeEmulation";

        // Development or user config time flags
        bool debug = false;

        // Data read from program config
        String mytag = "GAME";    /* Which LCDs to display status on, which controller to use */
        String safetag = "SAFE";  /* Where to move coins to                                   */

        // Private variables
        private JDBG jdbg = null;
        private JLCD jlcd = null;

        List<IMyTerminalBlock> displays = null;
        List<IMyTerminalBlock> fpsdisplays = null;
        List<IMyTerminalBlock> namedisplays = null;
        IMyInventory coinInInv = null;
        IMyInventory coinOutInv = null;

        IMyShipController controller = null;
        Arcade8080Machine si = null;
        int Frame = 0;
        int curSec = 0;
        int curFrames = 0;
        int actFrames = 0;
        int lastFrames = 0;
        int cost = 0;
        DateTime inserted;
        DateTime lastCoinCheck;
        Program.GetRomData.Games currentGame = Program.GetRomData.Games.None;
        List<MyInventoryItem> itemList = new List<MyInventoryItem>();
        String notOkToPlay = null;

        // -------------------------------------------
        /* Example custom data in programming block: 
[Config]
; We will display on [<tag>SCREEN] and use controller [<tag>SEAT]
; Optionally FPS on LCD [<tag>FPS]
; Optionally Name on LCD [<tag>NAME]
tag=GAME
; Optional extra features:
; How many space credits per game - If so we NEED a cargo container [<tag>COINS]
; and a cargo container [<safetag>COINS]
cost=5
safetag=SAFE
; How many cycles to allow the game to consume - Default is 45000, you but range
; is 1000-49000. 
speed=40000
        */
        // -------------------------------------------

        // ---------------------------------------------------------------------------
        // Constructor
        // ---------------------------------------------------------------------------
        public Program()
        {
            Echo("Start");
            jdbg = new JDBG(this, debug);
            jlcd = new JLCD(this, jdbg, true);
            jlcd.UpdateFullScreen(Me, thisScript);
            notOkToPlay = null;

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
            } else {
                notOkToPlay = "ERROR: No tag configured\nPlease add [config] for tag=<substring>";
                Echo(notOkToPlay);
                return;
            }
            jdbg.Debug("Config: tag=" + mytag);

            int maxFrames = _ini.Get("config", "speed").ToInt32(45000);
            if (maxFrames > 1000 && maxFrames < 49000) {
                Specifics.maxFrames = maxFrames;
            }

            int renderframe = _ini.Get("config", "renderframe").ToInt32(1);
            if (renderframe > 0 && renderframe < 60) {
                Specifics.skipFrames = renderframe;
            }

            int cost = _ini.Get("config", "cost").ToInt32(0);
            if (cost > 0) {
                this.cost = cost;
                safetag = _ini.Get("config", "safetag").ToString("safe");
            }

            Echo("Using tag of " + mytag);
            Echo("Using cost of " + cost);
            Echo("Using speed of " + maxFrames);

            // ---------------------------------------------------------------------------
            // Find the seat
            // ---------------------------------------------------------------------------
            List<IMyTerminalBlock> Controllers = new List<IMyTerminalBlock>();
            GridTerminalSystem.GetBlocksOfType(Controllers, (IMyTerminalBlock x) => (
                                                                                   (x.CustomName != null) &&
                                                                                   (x.CustomName.ToUpper().IndexOf("[" + mytag.ToUpper() + ".SEAT]") >= 0) &&
                                                                                   (x is IMyShipController)
                                                                                  ));
            Echo("Found " + Controllers.Count + " controllers");

            if (Controllers.Count > 0) {
                foreach (var thisblock in Controllers) {
                    jdbg.Debug("- " + thisblock.CustomName);
                }
                if (Controllers.Count > 1) {
                    notOkToPlay = "ERROR: Too many controllers";
                    Echo(notOkToPlay);
                    return;
                }
                controller = (IMyShipController)Controllers[0];
            } else if (Controllers.Count == 0) {
                notOkToPlay = "ERROR: No controllers tagged as [" + mytag + ".SEAT]";
                Echo(notOkToPlay);
                return;
            }

            if (cost > 0) {
                // ---------------------------------------------------------------------------
                // Find the coin in
                // ---------------------------------------------------------------------------
                List<IMyTerminalBlock> CoinIn = new List<IMyTerminalBlock>();
                GridTerminalSystem.GetBlocksOfType(CoinIn, (IMyTerminalBlock x) => (
                                                                                       (x.CustomName != null) &&
                                                                                       (x.CustomName.ToUpper().IndexOf("[" + mytag.ToUpper() + ".COINS]") >= 0) &&
                                                                                       (x.HasInventory)
                                                                                      ));
                Echo("Found " + CoinIn.Count + " coin in");
                if (CoinIn.Count != 1) {
                    notOkToPlay = "ERROR: Game has space credit cost but no cargo container tagged [" + mytag + ".COINS]";
                    Echo(notOkToPlay);
                    return;
                } else {
                    coinInInv = CoinIn[0].GetInventory(0);
                }

                // ---------------------------------------------------------------------------
                // Find the safe to move the coin to
                // ---------------------------------------------------------------------------
                List<IMyTerminalBlock> CoinOut = new List<IMyTerminalBlock>();
                GridTerminalSystem.GetBlocksOfType(CoinOut, (IMyTerminalBlock x) => (
                                                                                       (x.CustomName != null) &&
                                                                                       (x.CustomName.ToUpper().IndexOf("[" + safetag.ToUpper() + "]") >= 0) &&
                                                                                       (x.HasInventory)
                                                                                      ));
                Echo("Found " + CoinOut.Count + " coin out");
                if (CoinOut.Count != 1) {
                    notOkToPlay = "ERROR: Game has space credit cost but nowhere to move credit to - Needs a cargo container tagged [" + safetag + "]";
                    Echo(notOkToPlay);
                    return;
                } else {
                    coinOutInv = CoinOut[0].GetInventory(0);
                }

                // ---------------------------------------------------------------------------
                // Check we can move from coin in -> coin out
                // ---------------------------------------------------------------------------
                if (!(coinInInv.IsConnectedTo(coinOutInv) &&
                    coinInInv.CanTransferItemTo(coinOutInv, new MyItemType("MyObjectBuilder_PhysicalObject","SpaceCredit")))) {
                    notOkToPlay = "ERROR: No connection/conveyor system between " + CoinIn[0].CustomName + " and " + CoinOut[0].CustomName;
                    Echo(notOkToPlay);
                    return;
                }
                Echo("All set up as arcade machine with cost of " + cost);

            }

            // ---------------------------------------------------------------------------
            // Game init
            // ---------------------------------------------------------------------------
            // Get all the LCDs we are going to output to
            displays = jlcd.GetLCDsWithTag(mytag + ".SCREEN");
            Echo("Found " + displays.Count + " displays");

            // Note width/height reversed as screen is rotated 90%
            jlcd.SetupFontCustom(displays, Display.HEIGHT, Display.WIDTH, true, 0.001F, 0.001F);

            // Initialize the LCDs
            jlcd.InitializeLCDs(displays, TextAlignment.LEFT);

            // Tollerate an fps LCD
            fpsdisplays = jlcd.GetLCDsWithTag(mytag + ".FPS");
            jlcd.InitializeLCDs(fpsdisplays, TextAlignment.CENTER);
            jlcd.SetupFontCustom(fpsdisplays, 1, 8, false, 0.25F, 0.25F);
            Echo("Found " + fpsdisplays.Count + " fpsdisplays");

            // Tollerate an fps LCD
            namedisplays = jlcd.GetLCDsWithTag(mytag + ".NAME");
            jlcd.InitializeLCDs(namedisplays, TextAlignment.CENTER);
            jlcd.SetupFontCustom(namedisplays, 1, 16, false, 0.25F, 0.25F);
            Echo("Found " + namedisplays.Count + " namedisplays");

            // Run every tick
            Runtime.UpdateFrequency = UpdateFrequency.Update1;
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
            //jdbg.ClearDebugLCDs();
            if (argument == null) {
                jdbg.Echo("Launched with empty parms" + argument);
            } else {
                jdbg.Echo("Launched with parms '" + argument + "'");
            }

            String errorMessage = null;
            if (notOkToPlay != null) {
                errorMessage = notOkToPlay;
            } else if (argument == null || argument.Equals("")) {
                if (currentGame == GetRomData.Games.None) {
                    errorMessage = "ERROR: No game configured - Please see instructions";
                }
                // Otherwise drop through to main logic
            } else {
                List<IMyTerminalBlock> gameData = new List<IMyTerminalBlock>();
                GridTerminalSystem.GetBlocksOfType(gameData, (IMyTerminalBlock x) => (
                                                                                       (x.CustomName != null) &&
                                                                                       (x.CustomName.ToUpper().IndexOf("[" + mytag.ToUpper() + "]") >= 0) &&
                                                                                       (x.CustomName.ToUpper().IndexOf(argument.ToUpper()) >= 0)
                                                                                      ));
                jdbg.Debug("Found " + gameData.Count + " game block with contents  [" + mytag + "] " + argument);
                if (gameData.Count != 1) {
                    errorMessage = "Could not find block with name '[" + mytag + "] " + argument + "')";
                } else {
                    String gameCode;
                    gameCode = gameData[0].CustomData;

                    argument = argument.ToLower().Replace(".zip", "");

                    if (Enum.TryParse(argument, true, out currentGame)) {
                        jdbg.Debug("Recognized as " + currentGame);
                    } else {
                        errorMessage = "ERROR: Invalid parameter: " + argument;
                    }

                    // Validate the ROM data
                    if (!GetRomData.checkRomData(currentGame, gameCode)) {
                        errorMessage = "ERROR: Data does not match expected for that game " + currentGame;
                    }

                    if (errorMessage == null) {

                        jlcd.WriteToAllLCDs(namedisplays, "" + currentGame.ToString().ToUpperInvariant(), false);

                        jdbg.Debug("Creating CPU");
                        Specifics specs = new Specifics();
                        specs.mypgm = this;
                        specs.controller = controller;
                        specs.jdbg = jdbg;
                        specs.jctrl = new JCTRL(this, jdbg, false);

                        si = new Arcade8080Machine(specs, currentGame, gameCode, cost);
                        Runtime.UpdateFrequency = UpdateFrequency.Update1;
                    }
                }
            }

            if (errorMessage != null) {
                Echo(errorMessage);
                jlcd.WriteToAllLCDs(displays, errorMessage, false);
                Runtime.UpdateFrequency = UpdateFrequency.None;
                return;
            }

            // ---------------------------------------------------------------------------
            // We only get here if we are refreshing
            // ---------------------------------------------------------------------------
            int now = DateTime.UtcNow.Second;
            if (now != curSec) {
                lastFrames = curFrames;
                if (Specifics.skipFrames > 1) {
                    jlcd.WriteToAllLCDs(fpsdisplays, "" + actFrames + " of " + lastFrames, false);
                } else {
                    jlcd.WriteToAllLCDs(fpsdisplays, "" + lastFrames, false);
                }
                curSec = now;
                curFrames = 0;
                actFrames = 0;
            }

            jdbg.Echo("Frame " + Frame++ + ", fps: " + lastFrames + ", state:" + si.State);

            // -----------------------------------------------------------------
            // Real work starts here
            // -----------------------------------------------------------------
            // Every 2 seconds, check to see if a credit has been deposited.
            // If it has, insert a coin into the machine, and move the credit to the 
            // safe.
            if ((cost > 0) && (Frame > 50) && (si != null)) {
                DateTime rightNow = DateTime.UtcNow;

                // Do we need to release the press of the coin down
                if ((inserted != DateTime.MinValue) && ((rightNow - inserted).TotalSeconds >= 1)) {
                    jdbg.Debug("Released coin button");
                    inserted = DateTime.MinValue;
                    si.insertCoin(false);
                    lastCoinCheck = rightNow; // reset timer
                }

                // Do we need to check the coin in container?
                else if ((rightNow-lastCoinCheck).TotalSeconds >= 2) {
                    itemList.Clear();
                    // Are there enough coins in the inventory?
                    coinInInv.GetItems(itemList, 
                                            b => (b.Type.ToString().Contains("SpaceCredit") &&
                                                  b.Amount >= cost)                                                
                                      );
                    if (itemList.Count > 0) {
                        jdbg.Debug("Found Coins - Moving and pressing coin button");
                        // Move the credits to the safe
                        coinInInv.TransferItemTo(coinOutInv, itemList[0], cost);
                        jdbg.Debug("Moved & press coin button");

                        // Press the coin in key and remember the time it was pressed
                        inserted = rightNow;
                        si.insertCoin(true);

                    }
                    lastCoinCheck = rightNow;
                }
            }

            // Run the CPU as normal for whatever is level of this cycle
            try {
                si.part_exe(ref curFrames, ref actFrames);
            }
            catch (Exception ex) {
                jdbg.DebugAndEcho("Exception: " + ex.ToString());
                Echo("Exception: " + ex.ToString());
                throw ex;
            }

            // ----------------------------- CUT -------------------------------------
        }
    }
}
