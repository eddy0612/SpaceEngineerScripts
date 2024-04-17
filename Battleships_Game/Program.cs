using Sandbox.Game.EntityComponents;
using Sandbox.Game.Gui;
using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;
using SpaceEngineers.Game.ModAPI.Ingame;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel.Design;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using VRage;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.GUI.TextPanel;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ModAPI.Ingame.Utilities;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Game.VisualScripting.Utils;
using VRageMath;
using static IngameScript.Program;

namespace IngameScript
{
    partial class Program : MyGridProgram
    {
        // ----------------------------- CUT -------------------------------------
        String thisScript = "Battleships";

        // Development time flags
        bool debug = false;
        bool stayRunning = true;

        // Private variables
        private JDBG jdbg = null;
        private JINV jinv = null;
        private JLCD jlcd = null;
        private JCTRL jctrl = null;

        // Game configuration
        private static int ROWS = 10;
        private static int COLS = 10;
        private int[] SHIPS = { 5, 4, 3, 2, 2 };   // Max 10 ships!!
        // Useful when debugging:
        //private int[] SHIPS = { 3,3 }; 
        int skipFrames = 0;
        private static int RHS_OFFSET = 5; // rows down that the right hand side board appears

        private static char COL_BOARD_BACKGROUND = JLCD.COLOUR_BLACK;
        private static char COL_BOARD_OUTERBORDER = JLCD.COLOUR_GREY;
        private static char COL_BOARD_OUTERBORDER_GO = JLCD.COLOUR_GREEN;
        private static char COL_BOARD_INNERBORDER = JLCD.COLOUR_GREY;
        private static char COL_BOARD_CROSS = JLCD.COLOUR_WHITE;

        private static char COL_BOARD_CURSOR = JLCD.COLOUR_PURPLE;
        private static char COL_BOARD_CURSOR_GO = JLCD.COLOUR_GREEN;

        private static char COL_BOARD_HIT = JLCD.COLOUR_RED;
        private static char COL_BOARD_MISS = JLCD.COLOUR_CYAN;

        private static char COL_BOARD_PLACED = JLCD.COLOUR_GREEN;
        private static char COL_BOARD_PLACING = JLCD.COLOUR_BLUE;
        private static char COL_BOARD_INVALID = JLCD.COLOUR_RED;

        // Internals
        DateTime lastCheck = new DateTime(0);
        String mytag = "IDONTCARE";
        int curFrame = 0;

        // State info
        enum Stage { Launched, SelectPlayers, PlaceShips, StartingGame, Player1Shoot, Player2Shoot, GameOver };
        Stage currentState = Stage.GameOver;

        bool canBeTwoPlayer = true;
        bool isTwoPlayerGame = true;
        Random rnd = new Random();

        List<IMyTerminalBlock> p1LCDs = null;
        IMyShipController p1Ctrl = null;
        List<IMyTerminalBlock> p2LCDs = null;
        IMyShipController p2Ctrl = null;

        // Data used for player boards
        //    0..9 == ship 0..9
        //    O = Miss
        static char BOARD_MISS = 'O';
        //    X = Hit
        static char BOARD_HIT = 'X';
        //    ' ' = Nothing
        static char BOARD_EMPTY = ' ';
        //    'c' = Current position when placing
        static char BOARD_CUR = 'c';
        //    'b' = Bad position when placing
        static char BOARD_BAD = 'b';

        char[] p1Board = null;
        char[] p2Board = null;

        // Cursor for big screen for p1/p2 - adding ships or playing
        int p1X = (COLS-1) / 2;
        int p1Y = (ROWS-1) / 2;
        bool p1Horiz = true;

        int p2X = COLS / 2;
        int p2Y = ROWS / 2;
        bool p2Horiz = true;

        // When adding ships
        int p1NextShip = 0;
        int p2NextShip = 0;
        int p1SquaresLeft = 0;
        int p2SquaresLeft = 0;
        bool p1OK = false;
        bool p2OK = false;
        bool p1AwaitRelease = false;
        bool p2AwaitRelease = false;

        // When playing
        bool changes = false;
        bool p1Ready = false;
        bool p2Ready = false;

        bool play1 = false;
        bool play2 = false;

        public Program()
        {
            // As a game, we need to be updated every tick
            if (stayRunning) {
                Runtime.UpdateFrequency = UpdateFrequency.Update1;
            }

            // Initialize the utility classes
            jdbg = new JDBG(this, debug);
            jlcd = new JLCD(this, jdbg, true);
            jinv = new JINV(jdbg);
            jctrl = new JCTRL(this, jdbg, true);

            // ---------------------------------------------------------------------------
            // Get my custom data and parse to get the config
            // ---------------------------------------------------------------------------
            MyIniParseResult result;
            MyIni _ini = new MyIni();
            if (!_ini.TryParse(Me.CustomData, out result))
                throw new Exception(result.ToString());

            // Get the value of the "tag" key under the "config" section.
            mytag = _ini.Get("config", "tag").ToString();
            if (mytag != null) {
                mytag = (mytag.Split(';')[0]).Trim();
                Echo("Using tag of " + mytag);
            } else {
                Echo("No tag configured\nPlease add [config] for tag=<substring>");
                return;
            }

            // ---------------------------------------------------------------------------
            // We need to find one or two seats tagSEAT(2) and one or two LCDs tagSCREEN(2)
            // If only one seat or one LCD, then will play in single player
            // ---------------------------------------------------------------------------
            p1LCDs = jlcd.GetLCDsWithTag(mytag + "SCREEN");
            if (p1LCDs.Count == 0) {
                Echo("ERROR: No screen found. Please tag a screen with " + mytag + "SCREEN");
                throw new Exception("No screens found with tag " + mytag + "SCREEN");
            }
            p2LCDs = jlcd.GetLCDsWithTag(mytag + "SCREEN2");
            if (p2LCDs.Count == 0) {
                jdbg.DebugAndEcho("No screen found for player 2 - setting as one player game only");
                canBeTwoPlayer = false;
            }
            jdbg.DebugAndEcho("Found " + p1LCDs.Count + " p1 LCDs and " + p2LCDs.Count + " p2 LCDs");

            List<IMyTerminalBlock> p1CTRLs = jctrl.GetCTRLsWithTag(mytag + "SEAT");
            if (p1CTRLs.Count != 1) {
                Echo("ERROR: " + p1CTRLs.Count + " controllers found for player 1. Please tag a seat with " + mytag + "SEAT");
                throw new Exception("Could not identify controller for p1 with tag " + mytag + "SEAT");
            } else {
                p1Ctrl = (IMyShipController)p1CTRLs[0];
            }

            List<IMyTerminalBlock> p2CTRLs = jctrl.GetCTRLsWithTag(mytag + "SEAT2");
            if (p2CTRLs.Count == 0) {
                jdbg.DebugAndEcho("No seat found for player 2 - setting as one player game only");
                canBeTwoPlayer = false;
            } else if (p2CTRLs.Count > 1) {
                Echo("ERROR: " + p1CTRLs.Count + " controllers found for player 1. Please tag only 1 seat with " + mytag + "SEAT2");
                throw new Exception("Could not identify controller for p2 with tag " + mytag + "SEAT2");
            } else {
                p2Ctrl = (IMyShipController)p2CTRLs[0];
            }

            // Do one time initialization
            currentState = Stage.GameOver;

            // Resize all the screens
            jlcd.InitializeLCDs(p1LCDs, TextAlignment.LEFT);
            DrawGameOverScreen(p1LCDs, true, false);

            jlcd.InitializeLCDs(p2LCDs, TextAlignment.LEFT);
            DrawGameOverScreen(p2LCDs, true, false);

            jdbg.DebugAndEcho("Object created");
        }

        public void Save()
        {
            // Not used currently
        }

        public void Main(string argument, UpdateType updateSource)
        {
            try {

                if (argument != null && !argument.Equals("")) jdbg.Debug("Running with arg: " +  argument);

                // On first run only begin when START is supplied
                if (currentState == Stage.GameOver && argument.Equals("START")) {
                    ReInitializeClass();
                    currentState = Stage.Launched;
                    changes = true;
                }

                // If we dont display every frame, return until ready to display
                else if (curFrame > 0) {
                    curFrame--;
                    return;
                } else {
                    curFrame = skipFrames;
                }

                Echo("State: " + currentState.ToString() + " in " + thisScript);

                // ----------------------------------------------
                // Unknown -> Game Over screen
                // ----------------------------------------------
                if (currentState == Stage.GameOver) {
                    return;
                }

                // -------------------------------------------------------------
                // Launched means start running - Show waiting on players screen
                // -------------------------------------------------------------
                if (currentState == Stage.Launched) {
                    bool checkKeys = HandleWaitingScreen();

                    if (checkKeys) {
                        if (jctrl.AnyKey(p1Ctrl, false) || (canBeTwoPlayer && jctrl.AnyKey(p2Ctrl, false))) {
                            jdbg.Debug("Key pressed - time for next stage");
                            currentState = Stage.PlaceShips;
                            if (canBeTwoPlayer && jctrl.IsOccupied(p2Ctrl)) isTwoPlayerGame = true;
                            else isTwoPlayerGame = false;
                            InitializeGame(isTwoPlayerGame);
                            changes = true;
                        } else {
                            //jdbg.Debug("No key pressed yet...");
                        }
                    }
                }

                // -------------------------------------------------------------
                // PlaceShips means both sides need to sort out their ships
                // -------------------------------------------------------------
                if (currentState == Stage.PlaceShips) {
                    bool wasChanges = changes;
                    bool newChanges = false;
                    HandleAddShips(p1LCDs, p1Ctrl, ref p1X, ref p1Y, ref p1Horiz, 
                                   ref p1NextShip, p1Ready, ref p1SquaresLeft, ref p1OK,
                                   ref p1Board, ref newChanges, ref p1AwaitRelease);
                    if (p1NextShip == SHIPS.Length) p1Ready = true;
                    HandleAddShips(p2LCDs, p2Ctrl, ref p2X, ref p2Y, ref p2Horiz,
                                   ref p2NextShip, p1Ready, ref p2SquaresLeft, ref p2OK,
                                   ref p2Board, ref newChanges, ref p2AwaitRelease);
                    if (p2NextShip == SHIPS.Length) p2Ready = true;
                    changes = newChanges;

                    if (p1Ready && p2Ready) {
                        currentState = Stage.Player1Shoot;
                        p1X = COLS / 2;
                        p1Y = ROWS / 2;
                        p2X = COLS / 2;
                        p2Y = ROWS / 2;
                        changes = true;
                        // Setup font for play time
                        jdbg.ClearDebugLCDs();
                        jlcd.SetupFont(p1LCDs, (4 * ROWS) + 5, (4 * COLS) + 5 + COLS + 3, true);
                        if (p2LCDs != null && p2LCDs.Count != 0) jlcd.SetupFont(p2LCDs, (4 * ROWS) + 5, (4 * COLS) + 5 + COLS + 3, true);
                    }
                }

                // -------------------------------------------------------------
                // Starting Game means draw both sides and set to p1
                // -------------------------------------------------------------
                if ((currentState == Stage.Player1Shoot) ||
                    (currentState == Stage.Player2Shoot)) {

                    // Only redraw if things have changed
                    if (changes) {
                        DrawBoard(p1LCDs, p1Board, p2Board, p1X, p1Y, (currentState == Stage.Player1Shoot), true, 0, p2SquaresLeft);
                        DrawBoard(p2LCDs, p2Board, p1Board, p2X, p2Y, (currentState == Stage.Player2Shoot), true, 0, p1SquaresLeft);
                        changes = false;
                    }

                    // Move cursor - returns true if fire pressed
                    bool fired = false;
                    bool crouched = false;
                    bool keyChanged = false;

                    if (currentState == Stage.Player1Shoot) {
                        bool canFire = (currentState == Stage.Player1Shoot);

                        if ((p2Board[(p1Y * COLS)+p1X] == BOARD_HIT) ||
                            (p2Board[(p1Y * COLS) + p1X] == BOARD_MISS)) {
                            canFire = false;  // Already shot there
                        }

                        fired = HandleJoystick(p1Ctrl, ref p1AwaitRelease, ref p1X, ref p1Y, canFire, false, ref crouched, ref keyChanged);

                    } else {
                        // If 1 player, so p2Ctrl is null, pick a random spot to fire to
                        if (!isTwoPlayerGame) {
                            fired = ComputerAIMakeGuess(ref p2X, ref p2Y);
                            changes = true;
                        } else {
                            bool canFire = (currentState == Stage.Player2Shoot);
                            if ((p1Board[(p2Y * COLS) + p2X] == BOARD_HIT) ||
                                (p1Board[(p2Y * COLS) + p2X] == BOARD_MISS)) {
                                canFire = false;  // Already shot there
                            }

                            fired = HandleJoystick(p2Ctrl, ref p2AwaitRelease, ref p2X, ref p2Y, canFire, false, ref crouched, ref keyChanged);
                        }
                    }
                    if (keyChanged) changes = true;

                    if (fired) {
                        // Do the shot
                        if (currentState == Stage.Player1Shoot) {
                            HandleFire(ref p2Board, p1X, p1Y, ref p2SquaresLeft);
                        } else {
                            HandleFire(ref p1Board, p2X, p2Y, ref p1SquaresLeft);
                        }

                        // Switch sides unless game is over
                        if (currentState != Stage.GameOver) {
                            if (currentState == Stage.Player2Shoot) {
                                currentState = Stage.Player1Shoot;
                            } else {
                                currentState = Stage.Player2Shoot;
                            }
                        }
                    }
                }

                // -------------------------------------------------------------
                // GameOver means we have literally just won/lost
                // -------------------------------------------------------------
                if (currentState == Stage.GameOver) {
                    DrawGameOverScreen(p1LCDs, false, p2SquaresLeft == 0);
                    DrawGameOverScreen(p2LCDs, false, p1SquaresLeft == 0);
                }
            }
            catch (Exception ex) {
                jdbg.DebugAndEcho("Exception - " + ex.ToString() + "\n" + ex.StackTrace);
                throw ex;
            }
        }

        // -------------------------------------------------------------
        // Draw a screen asking for players to be ready
        // Optionally check for any (control key) press
        // -------------------------------------------------------------
        private bool HandleWaitingScreen()
        {
            String screenDisplay = "\n\n    BattleShips\n\n";
            bool checkKeys = false;

            bool newPlay1 = jctrl.IsOccupied(p1Ctrl);
            if (!newPlay1) {
                screenDisplay += "      Player 1 : <Unoccupied>\n\n";
            } else {
                screenDisplay += "      Player 1 : Player Available\n\n";
            }

            bool newPlay2 = false;
            if (canBeTwoPlayer) {
                newPlay2 = jctrl.IsOccupied(p2Ctrl);
                if (!newPlay2) {
                    screenDisplay += "      Player 2 : <Unoccupied>\n\n";
                } else {
                    screenDisplay += "      Player 2 : Player Available\n\n";
                }
            }

            if (newPlay1 && newPlay2) {
                screenDisplay += "      Press any direction key to start two player game\n";
                checkKeys = true;
            } else if (newPlay1) {
                screenDisplay += "      Press any direction key to start one player game\n";
                checkKeys = true;
                if (canBeTwoPlayer) {
                    screenDisplay += "          or wait for a second player to join\n";
                }
            }

            // Draw the screen, only if things have changed
            if ((newPlay1 != play1) || (newPlay2 != play2)) {
                play1 = newPlay1;
                play2 = newPlay2;
                jdbg.Debug("Play1: " + newPlay1 + ", Play2:" + newPlay2);
                jlcd.SetupFont(p1LCDs, 10, 70, false);
                jlcd.SetupFont(p2LCDs, 10, 70, false);
                jlcd.WriteToAllLCDs(p1LCDs, screenDisplay, false);
                jlcd.WriteToAllLCDs(p2LCDs, screenDisplay, false);
            }

            //jdbg.Debug("handleWaiting returning " + checkKeys);
            return checkKeys;
        }

        // -------------------------------------------------------------
        // InitializeGame - Called when start is called, before ships
        //    are placed.
        // -------------------------------------------------------------
        private void InitializeGame(bool isTwoPlayer)
        {
            jdbg.Debug("InitializeGame is2p:" + isTwoPlayer);
            // Empty everything
            p1Board = new char[ROWS * COLS];
            p2Board = new char[ROWS * COLS];
            for (int i = 0; i < (ROWS * COLS); i++) {
                p1Board[i] = BOARD_EMPTY;
                p2Board[i] = BOARD_EMPTY;
            }
            p1NextShip = 0;
            p2NextShip = 0;
            p1Ready = false;
            p2Ready = false;
            p1SquaresLeft = 0;
            p2SquaresLeft = 0;

            // Randomly put the ships in for player two if a one player game
            if (!isTwoPlayer) {
                ComputerAIAddShips();
            }

            // Setup font for add time
            jlcd.SetupFont(p1LCDs, (4 * ROWS) + 6, (4 * COLS) + 5, true);
            if (p2LCDs != null && p2LCDs.Count != 0) jlcd.SetupFont(p2LCDs, (4 * ROWS) + 6, (4 * COLS) + 5, true);
        }

        // -------------------------------------------------------------
        // ComputerAIAddShips - randomly lay the ships down
        // -------------------------------------------------------------
        private void ComputerAIAddShips()
        {
            int currentShip = 0;
            while (currentShip < SHIPS.Length) {
                jdbg.Debug("On ship " + currentShip);
                int shipsize = SHIPS[currentShip];

                // 0 == Horizontal, 1 == Vertical
                int dirn = rnd.Next(0, 2);
                int newX, newY;
                if (dirn == 0) {
                    newX = rnd.Next(0, (1 + COLS - shipsize));
                    newY = rnd.Next(0, ROWS);
                } else {
                    newX = rnd.Next(0, COLS);
                    newY = rnd.Next(0, (1 + ROWS - shipsize));
                }

                // See if its valid to put the whole ship in
                //jdbg.Debug(" Checking ship " + currentShip + " - " + newX + "," + newY + " - H?" + (dirn==0));
                int testX = newX;
                int testY = newY;
                bool isOK = true;
                for (int i = 0; (isOK && (i < shipsize)); i++) {
                    if (p2Board[(testY * COLS) + testX] != BOARD_EMPTY) {
                        isOK = false;
                        //jdbg.Debug(" ..No Good at " + testX + "," + testY);
                    } else {
                        //jdbg.Debug(" ..OK at " + testX + "," + testY);
                        if (dirn == 0) {
                            testX++;
                        } else {
                            testY++;
                        }
                    }
                }

                if (isOK) {
                    jdbg.Debug("Ship " + currentShip + " - " + newX + "," + newY + " - H?" + (dirn == 0));
                    testX = newX;
                    testY = newY;
                    for (int i = 0; (isOK && (i < shipsize)); i++) {
                        p2Board[(testY * COLS) + testX] = (char)(((short)'0') + currentShip);
                        p2SquaresLeft++;
                        if (dirn == 0) {
                            testX++;
                        } else {
                            testY++;
                        }
                    }
                    currentShip++;
                }
            }
            p2Ready = true;
            p2NextShip = SHIPS.Length;
        }

        // -------------------------------------------------------------
        // ComputerAIMakeGuess - Make a random guess
        //    JJJ TODO: If last was a hit, try some intelligence?
        // -------------------------------------------------------------
        private bool ComputerAIMakeGuess(ref int myX, ref int myY)
        {
            while (true) {
                myX = rnd.Next(0, COLS);
                myY = rnd.Next(0, ROWS);
                if (p1Board[(myY * COLS) + myX] != BOARD_HIT &&
                    p1Board[(myY * COLS) + myX] != BOARD_MISS) {
                    return true;
                }
            }
        }

        // -------------------------------------------------------------
        // HandleAddShips - Display my board, let me place ships
        // -------------------------------------------------------------
        private void HandleAddShips(List<IMyTerminalBlock> lcds, IMyShipController ctrl,
                                    ref int myX, ref int myY, ref bool myHoriz, ref int nextShip, bool ready,
                                    ref int mySquaresLeft, ref bool isOK, ref char[] board, ref bool newchanges,
                                    ref bool awaitRelease)
        {
            if (ctrl == null) {
                // This is player 2 for a one player game so nothing to do
                return;
            }

            // If there are any changes we need to redraw the boards
            if (changes) {
                isOK = DrawBoardWithTmpPlacement(lcds, board, nextShip, myX, myY, myHoriz);
            }

            // Player Handling:
            // Note if we 'fired' then that can be the only press, so x,y and horiz are unchanged
            // because we use the seat in single key mode only
            HandlePlayer(ctrl, ref myX, ref myY, isOK, ref myHoriz, ref nextShip, ref board, ref mySquaresLeft, ref awaitRelease, ref newchanges);

        }

        // -------------------------------------------------------------
        // DrawBoard - Draw one players screen
        //   isGameTime == true means during shooting time
        //                 false means during setup
        // -------------------------------------------------------------
        private void DrawBoard(List<IMyTerminalBlock> lcds, char[] littleBoard, char[] bigBoard,
                               int myX, int myY, bool myGo, bool isGameTime, int SetupShipNo, int ShipsLeft)
        {
            // One player game support:
            if (lcds == null || lcds.Count == 0) return;

            // Cursor of myX,myY is on big board
            bool foundInvalid = false;

            // Start with an empty screen;
            List<String> thisScreen = new List<String>();

            char bigBoardOuterBorder = COL_BOARD_OUTERBORDER;
            char littleBoardOuterBorder = COL_BOARD_OUTERBORDER;
            char cursorColour = COL_BOARD_CURSOR;

            if (myGo && isGameTime) {
                cursorColour = COL_BOARD_CURSOR_GO;
                bigBoardOuterBorder = COL_BOARD_OUTERBORDER_GO;
            } else {
                littleBoardOuterBorder = COL_BOARD_OUTERBORDER_GO;
            }

            // Firstly build the main board
            for (int i = 0;i<ROWS; i++) {
                // Before line
                if ((i == (ROWS/2))) {
                    thisScreen.Add(bigBoardOuterBorder + ("".PadRight((4 * COLS) - 1, COL_BOARD_CROSS)) + bigBoardOuterBorder);
                } else if (i==0) { 
                    thisScreen.Add(bigBoardOuterBorder + ("".PadRight((4 * COLS) - 1, bigBoardOuterBorder))+ bigBoardOuterBorder);
                } else {
                    String thisLine = "" + bigBoardOuterBorder + ("".PadRight(((4 * (COLS/2)) - 1), COL_BOARD_INNERBORDER));
                    thisLine += COL_BOARD_CROSS;
                    thisLine += "".PadRight(((4 * (COLS/2)) - 1), COL_BOARD_INNERBORDER) + bigBoardOuterBorder;
                    thisScreen.Add(thisLine);
                }

                // 3x lines for each row
                for (int a = 0; a < 3; a++) {
                    String thisLine = "";

                    for (int j = 0; j < COLS; j++) {
                        if (j == 0) {
                            thisLine += "" + bigBoardOuterBorder;
                        } else if (j == (COLS/2)) {
                            thisLine += "" + COL_BOARD_CROSS;
                        } else { 
                            thisLine += "" + COL_BOARD_INNERBORDER;
                        }
                        // get boardChar for square
                        char colour = COL_BOARD_BACKGROUND;

                        if (isGameTime) {
                            if (bigBoard[(i * COLS) + j] == BOARD_HIT) colour = COL_BOARD_HIT;
                            if (bigBoard[(i * COLS) + j] == BOARD_MISS) colour = COL_BOARD_MISS;
                        } else {
                            if (bigBoard[(i * COLS) + j] >= '0' && bigBoard[(i * COLS) + j] <= '9') colour = COL_BOARD_PLACED;
                            if (bigBoard[(i * COLS) + j] == BOARD_CUR) colour = COL_BOARD_PLACING;
                            if (bigBoard[(i * COLS) + j] == BOARD_BAD) {
                                colour = COL_BOARD_INVALID;
                                foundInvalid = true;
                            }
                        }

                        // Draw something different for the cursor
                        if (j == myX && i == myY) {
                            if (a==0 || a==2) {
                                thisLine += "" + colour + cursorColour + colour;
                            } else {
                                thisLine += "" + cursorColour + colour + cursorColour;
                            }
                        } else {
                            thisLine += "".PadRight(3, colour);
                        }
                    }
                    thisLine += bigBoardOuterBorder;
                    thisScreen.Add(thisLine);
                }
            }
            // Final line (bottom border)
            thisScreen.Add("".PadRight((4 * COLS) + 1, bigBoardOuterBorder));

            // Blank line
            thisScreen.Add("".PadRight(1, JLCD.COLOUR_BLACK));
            // Message line
            if (isGameTime) {
                if (myGo) {
                    if ((bigBoard[(myY * COLS) + myX] != BOARD_HIT) && (bigBoard[(myY * COLS) + myX] != BOARD_MISS)) {
                        thisScreen.Add("      Your go - Press SPACE/Vertical Up to fire ");
                    } else {
                        thisScreen.Add("      Your go - Invalid square, move cursor to a valid square first");
                    }
                } else {
                    thisScreen.Add("      Waiting on your opponent to fire");
                }
            } else {
                if (SetupShipNo < SHIPS.Length) {
                    thisScreen.Add("  Placing ship " + (SetupShipNo + 1) + " of " + SHIPS.Length);
                    if (foundInvalid) {
                        thisScreen.Add("      Invalid placement - please move with cursors, rotate with crouch");
                    } else {
                        thisScreen.Add("      Press jump to place or move with cursors, rotate with crouch");
                    }
                } else {
                    thisScreen.Add("  All ships placed, waiting on opponent");
                }
            }
            thisScreen.Add("");  // To allow for some padding

            // Now append the little opponents board
            // We will add it to the right hand side of the main board, about 5 lines down
            if (isGameTime) {
                //jdbg.Debug("Drawing little board");
                int startRow = RHS_OFFSET;
                thisScreen[startRow] += ("".PadRight(4, COL_BOARD_BACKGROUND));
                thisScreen[startRow] += "Opponent:";
                startRow++;

                thisScreen[startRow] += ("".PadRight(4, COL_BOARD_BACKGROUND));
                thisScreen[startRow] += ("".PadRight(COLS+2, littleBoardOuterBorder));
                startRow++;

                for (int i = 0; i < ROWS; i++) {
                    String thisLine = thisScreen[startRow];
                    thisLine += ("".PadRight(4, COL_BOARD_BACKGROUND)) + littleBoardOuterBorder;

                    for (int j = 0; j < COLS; j++) {
                        // get boardChar for square
                        char colour = COL_BOARD_BACKGROUND;
                        if (littleBoard[(i * COLS) + j] == BOARD_HIT) colour = COL_BOARD_HIT;
                        if (littleBoard[(i * COLS) + j] == BOARD_MISS) colour = COL_BOARD_MISS;
                        thisLine += colour;
                    }
                    thisLine += littleBoardOuterBorder;
                    thisScreen[startRow] = thisLine;
                    startRow++;
                }
                thisScreen[startRow] += ("".PadRight(4, COL_BOARD_BACKGROUND));
                thisScreen[startRow] += ("".PadRight(COLS + 2, littleBoardOuterBorder));
                startRow++;

                startRow++;
                startRow++;

                thisScreen[startRow] += ("".PadRight(4, COL_BOARD_BACKGROUND));
                thisScreen[startRow] += "Left to find:";
                startRow++;
                thisScreen[startRow] += ("".PadRight(4, COL_BOARD_BACKGROUND));
                thisScreen[startRow] += "   " + ShipsLeft;
                startRow++;

            }

            // Now actually draw the screen
            String screen = "";
            for (int i=0; i<thisScreen.Count; i++) { 
                screen += thisScreen[i] + "\n"; 
            }
            jlcd.WriteToAllLCDs(lcds, screen, false);
        }

        // -------------------------------------------------------------
        // DrawBoardWithTmpPlacement - Draw using a copy of the board and
        //   place the currently fitted ship in temporarily
        // -------------------------------------------------------------
        public bool DrawBoardWithTmpPlacement(List<IMyTerminalBlock> lcds, char[] board, int nextShip,
                                              int myX, int myY, bool myHoriz)
        {
            char[] tmpboard = (char[])board.Clone();
            bool isOK = true;

            if (nextShip < SHIPS.Length) {

                char boardChar = BOARD_CUR;
                int tmpX = myX;
                int tmpY = myY;
                //jdbg.Debug("Testing: " + myX + "," + myY + "," + myHoriz + "(" + SHIPS[nextShip] + ")");
                for (int i = 0; i < SHIPS[nextShip]; i++) {
                    if (tmpboard[(tmpY * COLS) + tmpX] != BOARD_EMPTY) {
                        boardChar = BOARD_BAD;
                        isOK = false;
                        jdbg.Debug("Testing failed at " + i);
                        break;
                    }
                    // Is there space to move on
                    if (i < SHIPS[nextShip]-1) { 
                        if (myHoriz) {
                            if (tmpX < (COLS - 1)) tmpX++;
                            else {
                                boardChar = BOARD_BAD;
                                isOK = false;
                                jdbg.Debug("Testing failed at " + i);
                            }
                        } else {
                            if (tmpY < (ROWS - 1)) tmpY++;
                            else {
                                boardChar = BOARD_BAD;
                                isOK = false;
                                jdbg.Debug("Testing failed at " + i);
                            }
                        }
                    }
                }

                tmpX = myX;
                tmpY = myY;

                //jdbg.Debug("Updating: " + isOK + ", col:" + boardChar);
                for (int i = 0; i < SHIPS[nextShip]; i++) {
                    tmpboard[(tmpY * COLS) + tmpX] = boardChar;
                    if (myHoriz) {
                        if (tmpX < (COLS - 1)) tmpX++;
                    } else {
                        if (tmpY < (ROWS - 1)) tmpY++;
                    }
                }
            }
            DrawBoard(lcds, null, tmpboard, myX, myY, true, false, nextShip, 0);
            return isOK;
        }

        // -------------------------------------------------------------
        // HandleJoystick - Allow cursor to move and optionally if it is
        //   their go, to fire. 
        // -------------------------------------------------------------
        private bool HandleJoystick(IMyShipController ctrl, ref bool awaitRelease, ref int myX, ref int myY, bool canFire, bool canCrouch, ref bool myHoriz, ref bool anyChanges)
        {
            bool fired = false;
            if (ctrl == null) { return false; };    // 2nd player in a one player game
            //jdbg.Debug("Can fire: " + canFire);

            if (awaitRelease) { 
                if (!jctrl.AnyKey(ctrl, true)) {
                    awaitRelease = false;
                } else {
                    return false;
                }
            }
            // set changes = true if we change myX, myY or we fire
            if ((myX > 0) && jctrl.IsLeft(ctrl)) {
                myX--;
                anyChanges = true;
            } 
            if ((myX < (COLS-1)) && jctrl.IsRight(ctrl)) {
                myX++;
                anyChanges = true;
            }
            if ((myY > 0) && jctrl.IsUp(ctrl)) {
                myY--;
                anyChanges = true;
            }
            if ((myY < (ROWS-1)) && jctrl.IsDown(ctrl)) {
                myY++;
                anyChanges = true;
            }
            if (canFire && jctrl.IsSpace(ctrl)) {
                anyChanges = true;
                fired = true;
            }
            if (canCrouch && jctrl.IsCrouch(ctrl)) {
                anyChanges = true;
                myHoriz = !myHoriz;
            }
            if (anyChanges) awaitRelease = true;
            return fired;
        }

        // -------------------------------------------------------------
        // HandleFire - Fire was clicked
        // -------------------------------------------------------------
        private void HandleFire(ref char[] board, int myX, int myY, ref int bitsLeft)
        {
            char oldChar = board[(myY * COLS) + myX];
            // Note: We should be firing at either an empty square or one with a ship
            //    so no need to handle the hit or miss case
            if (oldChar == BOARD_EMPTY) {
                board[(myY * COLS) + myX] = BOARD_MISS;
            } else {
                board[(myY * COLS) + myX] = BOARD_HIT;
                bitsLeft--;
                if (bitsLeft == 0) currentState = Stage.GameOver;
            }
        }

        // -------------------------------------------------------------
        // HandleFire - Fire was clicked
        // -------------------------------------------------------------
        private void DrawGameOverScreen(List<IMyTerminalBlock> lcds, bool isStart, bool didWin)
        {
            // One player game support:
            if (lcds == null || lcds.Count == 0) return;

            String result;
            if (isStart) {
                result = "\n\n\n\n\nPress START to play";
            } else {
                result = "\n\n\n\n\nGAME OVER - YOU ";
                if (didWin) {
                    result += "WON";
                } else {
                    result += "LOST";
                }
                result += "\n\nPress START to play again";
            }
            jlcd.SetupFont(lcds, 10, 50, false);
            jlcd.WriteToAllLCDs(lcds, result, false);           
        }

        // -------------------------------------------------------------
        // HandlePlayer during ship setup
        // -------------------------------------------------------------
        private void HandlePlayer(IMyShipController ctrl, ref int myX, ref int myY, bool myOK, 
                                  ref bool myHoriz, ref int myNextShip, ref char[] myBoard,
                                  ref int mySquaresLeft, ref bool awaitRelease, ref bool newChanges)
        {
            bool keyChanged = false;

            if (HandleJoystick(ctrl, ref awaitRelease, ref myX, ref myY, myOK, true, ref myHoriz, ref keyChanged)) {

                jdbg.Debug("Fire pressed");
                int tmpX = myX;
                int tmpY = myY;
                bool tmpHoriz = myHoriz;
                for (int i = 0; i < SHIPS[myNextShip]; i++) {
                    if (myBoard[(tmpY * ROWS) + tmpX] != BOARD_EMPTY) {
                        Echo("Odd - board should be clear");
                        throw new Exception("Board error 1");
                    } else {
                        myBoard[(tmpY * ROWS) + tmpX] = (char)(((short)'0') + myNextShip);
                        mySquaresLeft++;
                        if (tmpHoriz) tmpX++;
                        else tmpY++;
                    }
                }
                newChanges = true;
                myNextShip++;
            } else if (keyChanged) {
                newChanges = true;
            }
        }

        // -------------------------------------------------------------
        // When a game starts again, set up all the key variables
        // -------------------------------------------------------------
        private void ReInitializeClass()
        {
            play1 = false;
            play2 = false;
            p1X = (COLS - 1) / 2;
            p1Y = (ROWS - 1) / 2;
            p1Horiz = true;

            p2X = COLS / 2;
            p2Y = ROWS / 2;
            p2Horiz = true;

        }
    }
}
