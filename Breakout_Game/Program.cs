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

// TODO: Use JLCD for display/font selection and colours

namespace IngameScript
{
    partial class Program : MyGridProgram
    {

        // ----------------------------- CUT -------------------------------------
        //String thisScript = "Breakout_Game";

        // Development time flags
        public bool debug = false;
        bool stayRunning = true;
        //String thisScript = "TestSpace";
        bool dontdie = false;
        bool allowspeed = false;

        // My configuration
        /* Example custom data in programming block:
[Config]
tag=GAME
        */

        const char cYELLOW = '';
        const char cRED = '';
        const char cORANGE = '';
        const char cGREEN = '';
        const char cCYAN = '';
        const char cPURPLE = '';
        const char cBLUE = '';
        const char cWHITE = '';
        const char cBLACK = '';
        const char cBALL = cBLUE;

        // My configuration
        //int refreshSpeed = 5;                       // Default to 5 seconds if not provided
        String mytag = "IDONTCARE";
        //String alerttag = "ALERT";
        IMyShipController controller = null;
        IMyTextSurface lcdscreen = null;

        // Game config
        int pauseSecs = 3;           // How long to display the level screen or restart for
        int brickRows = 4;           // Number of brick rows
        int brickLength = 4;         // Chars per brick
        int batLength = 8;           // Size of bat (3 min)
        int cols = 40;               // Cols of text 

        // State info
        enum Stage { Launched, LevelScreen, Playing, LevelClear, Dead, GameOver };
        Stage currentState = Stage.GameOver;
        int lives = 3;               // Lives left
        int currentLevel = -1;
        DateTime pauseStart = new DateTime(0);
        StringBuilder thisScreen = null;
        char[] brickColours = null;
        int batStart = 10;
        int colSizeChars = 0;
        int rows = 20;               // Rows of text (Calculated)
        int batRow = 18;             // Row to show bat on (0 indexed, calculated)
        Random rnd = new Random();
        Vector2 ball;               // Ball position
        Vector2 ballDirection;      // Ball direction
        float ballSpeed;            // Speed it goes in that direction
        int bricksLeft = 0;
        float bestFontSize = 1.0F;

        int curFrame = 0;
        int skipFrames = 0;

        // ---------------------------------------------------------------------------
        // Constructor
        // ---------------------------------------------------------------------------
        public Program()
        {
            // Run every 100 ticks, but relies on internal check to only actually
            // perform on a defined frequency
            if (stayRunning)
            {
                Runtime.UpdateFrequency = UpdateFrequency.Update1; 
            }

            // ---------------------------------------------------------------------------
            // Get my custom data and parse to get the config
            // ---------------------------------------------------------------------------
            MyIniParseResult result;
            MyIni _ini = new MyIni();
            if (!_ini.TryParse(Me.CustomData, out result))
                throw new Exception(result.ToString());

            // Get the value of the "tag" key under the "config" section.
            mytag = _ini.Get("config", "tag").ToString();
            if (mytag != null)
            {
                mytag = (mytag.Split(';')[0]).Trim();
                Echo("Using tag of " + mytag);
            }
            else
            {
                Echo("No tag configured\nPlease add [config] for tag=<substring>");
                return;
            }

            // ---------------------------------------------------------------------------
            // Find the screens to output to
            // ---------------------------------------------------------------------------
            List<IMyTerminalBlock> drawLCDs = new List<IMyTerminalBlock>();
            GridTerminalSystem.GetBlocksOfType(drawLCDs, (IMyTerminalBlock x) => (
                                                                                   (x.CustomName != null) &&
                                                                                   (x.CustomName.IndexOf("[" + mytag + "SCREEN]") >= 0) &&
                                                                                   (x is IMyTextSurfaceProvider)
                                                                                  ));
            Echo("Found " + drawLCDs.Count + " screens");

            if (drawLCDs.Count > 0)
            {
                foreach (var thisblock in drawLCDs)
                {
                    IMyTextSurface thisLCD = ((IMyTextSurfaceProvider)thisblock).GetSurface(0);
                    Debug("- " + thisblock.CustomName);
                    if ((thisLCD != null) && (lcdscreen == null))
                    {
                        thisLCD.Font = "Monospace";
                        thisLCD.ContentType = ContentType.TEXT_AND_IMAGE;
                        thisLCD.TextPadding = 0;
                        thisLCD.BackgroundColor = Color.Black;
                        thisLCD.ChangeInterval = 1;
                        lcdscreen = thisLCD;
                    }
                }
                if (drawLCDs.Count > 1)
                {
                    Echo("Too many screens");
                    return;
                }
            }
            else if (drawLCDs.Count == 0)
            {
                Echo("No screen");
                return;
            }

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
                foreach (var thisblock in Controllers)
                {
                    Debug("- " + thisblock.CustomName);
                }
                if (Controllers.Count > 1)
                {
                    Echo("Too many controllers");
                    return;
                }
                controller = (IMyShipController) Controllers[0];
            }
            else if (Controllers.Count == 0) {
                Echo("No controllers"); 
                return;
            }

            // ---------------------------------------------------------------------------
            // Game initialization
            // ---------------------------------------------------------------------------
            colSizeChars = (cols + 1);  // Space for \n
            brickColours = new char[]{ cRED, cORANGE, cYELLOW, cGREEN };

            // Work out the font size so we fill the screen
            StringBuilder teststr = new StringBuilder("".PadRight(cols, cBLACK));

            float size = 0.05F;
            float incr = 0.05F;

            float bestWidth = 0;

            Echo("Calculating screen size");
            Echo("Tex: " + lcdscreen.TextureSize.ToString());
            Echo("Act: " + lcdscreen.SurfaceSize.ToString());
            lcdscreen.FontSize += 0.5F;
            Echo("Tex: " + lcdscreen.TextureSize.ToString());
            Echo("Act: " + lcdscreen.SurfaceSize.ToString());

            while (true)
            {
                lcdscreen.FontSize = size;
                Vector2 actualSize = lcdscreen.TextureSize;

                Vector2 thisSize = lcdscreen.MeasureStringInPixels(teststr, lcdscreen.Font, size);
                Echo("with size " + size + " width is " + thisSize.X + " max " + actualSize.X);

                if ((thisSize.X > bestWidth) && (thisSize.X < actualSize.X))
                {
                    size += incr;
                }
                else
                {
                    break;
                }
            }
            lcdscreen.FontSize = size;
            bestFontSize = size;
            Vector2 actualScreenSize = lcdscreen.TextureSize;
            Vector2 finalSize = lcdscreen.MeasureStringInPixels(teststr, lcdscreen.Font, size);
            rows = (int)Math.Floor(actualScreenSize.Y / finalSize.Y);
            batRow = rows - 2;
            Echo("Screen is " + cols + " big");
            thisScreen = new StringBuilder("".PadLeft((rows * colSizeChars), cBLACK));
            DisplayGameOver();
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
            try
            {
                if (currentState == Stage.GameOver && argument.Equals("START"))
                {
                    currentState = Stage.Launched;
                } else if (curFrame > 0)
                {
                    curFrame--;
                    return;
                } else {
                    curFrame = skipFrames;
                }

                Echo("State: " + currentState.ToString());

                // ----------------------------------------------
                // Unknown -> Game Over screen
                // ----------------------------------------------
                if (currentState == Stage.GameOver)
                {
                    return;
                }

                // ----------------------------------------------
                // Launched means start running - Initialize everything
                // ----------------------------------------------
                if ((currentState == Stage.Launched) || (currentState == Stage.LevelClear))
                {

                    // Initialize...
                    if (currentState == Stage.Launched) { 
                       currentLevel = 1;
                    }
                    initializeLevel(currentLevel);

                    DrawLCD("\n\n\n\n\n\n         Level " + currentLevel + "\n");
                    pauseStart = DateTime.Now;
                    currentState = Stage.LevelScreen;
                    return;
                }

                // ----------------------------------------------
                // Level display means do nothing for a number of secs
                // ----------------------------------------------
                if ((currentState == Stage.LevelScreen) || (currentState == Stage.Dead))
                { 
                    TimeSpan timeSinceLastCheck = DateTime.Now - pauseStart;
                    if (timeSinceLastCheck.TotalSeconds >= pauseSecs)
                    {
                        // Start the game
                        currentState = Stage.Playing;
                    }
                    else
                    {
                        return;
                    }
                }

                // ----------------------------------------------
                // We only get here if we are mid level
                // ----------------------------------------------
                if (currentState == Stage.Playing)
                {
                    moveBat();
                    moveBall();
                    if (currentState != Stage.GameOver) DrawLCD(thisScreen.ToString());
                }
              }
            catch (Exception ex)
            {
                Echo("Exception - " + ex.ToString() + "\n" + ex.StackTrace);
                throw ex;
            }
        }

        // ---------------------------------------------------------------------------
        // Simple wrapper to completely redraw our LCD
        // ---------------------------------------------------------------------------
        void DrawLCD(String screenContents)
        {
            lcdscreen.WriteText(screenContents, false);  // false == clear screen
        }

        // ---------------------------------------------------------------------------
        // Simple wrapper to decide whether to output to the console or not
        // ---------------------------------------------------------------------------
        void Debug(String str)
        {
            if (debug)
            {
                Echo("D:" + str);
            }
        }

        // ---------------------------------------------------------------------------
        // Move the ball, handling bouncing etc
        // ---------------------------------------------------------------------------
        void moveBall()
        {
            Vector2 oldLocation = ball;
            Vector2 newLocation = ball + (ballDirection * ballSpeed);

            // See if leaving screen, if so bounce back
            //Debug("cols: " + cols + ", rows: " + rows);
            //Debug("1:" + (int)oldLocation.X + "," + (int)oldLocation.Y + " -> " + (int)newLocation.X + "," + (int)newLocation.Y);
            if ((newLocation.X < 0) || (newLocation.X >= cols))
            {
                ballDirection.X = -ballDirection.X;
                newLocation.X += (ballDirection.X * ballSpeed);
            }
            if (newLocation.Y < 0)
            {
                ballDirection.Y = -ballDirection.Y;
                newLocation.Y += (ballDirection.Y * ballSpeed);
            }
            // If hit bottom - dead
            if (newLocation.Y >= rows)
            {
                // Temporary just bounce for now so we can see physics
                if (dontdie)
                {
                    ballDirection.Y = -ballDirection.Y;
                    newLocation.Y += (ballDirection.Y * ballSpeed);
                }
                else
                {

                    currentState = Stage.Dead;
                    drawBall(oldLocation, false, true);
                    initializeBatAndBall(currentLevel);
                    pauseStart = DateTime.Now;
                    lives--;
                    if (lives == 0)
                    {
                        DisplayGameOver();
                        currentState = Stage.GameOver;
                    }
                }
                return;
            }
            //Debug("2:" + (int)oldLocation.X + "," + (int)oldLocation.Y + " -> " + (int)newLocation.X + "," + (int)newLocation.Y);


            // If changed spot, remove old char and replace with new char
            int cx = (int)(newLocation.X);
            int cy = (int)(newLocation.Y);
            //Debug("4:" + cx + "," + cy);


            // Whats at that spot on the screen?
            char newSpace = thisScreen[(cy * colSizeChars) + cx];
            switch (newSpace)
            {
                case cBLACK: // Empty space - NOOP
                case cBALL:  // Didnt leave space - NOOP
                    break;
                case cWHITE: // Bounce off paddle
                    ballDirection.Y = -ballDirection.Y;
                    newLocation.Y += (ballDirection.Y * ballSpeed);

                    // If hit left side of bat, make sure new direction X is negative
                    float where = ((float)(cx - batStart) / (float)batLength);
                    if (where < 0.33)
                    {
                        // Result ball is going to be left but faster/slower depending on
                        // original direction
                        ballDirection.X = -(Math.Abs(ballDirection.X - 0.5F));
                        newLocation.X += (ballDirection.X * ballSpeed);
                    }
                    else if (where > 0.66)
                    {
                        ballDirection.X = Math.Abs(ballDirection.X + 0.5F);
                        newLocation.X += (ballDirection.X * ballSpeed);
                    } else
                    {
                        // Hitting middle
                    } 

                    // If hit right side of bat, make sure new direction X is positive
                    break;
                case '\n': throw new Exception("help");
                default:
                    // We've moved into a square with something in it, its a brick

                    // Work out the brick start
                    int brickStart = (cx/(brickLength + 1)) * (brickLength + 1);

                    // Remove it...
                    for (int i = 0; i<brickLength; i++)
                    {
                        thisScreen[(cy * colSizeChars) + brickStart + i] = cBLACK;
                    }
                    bricksLeft -= 1;

                    // Have we cleared the screen?
                    if (bricksLeft == 0) {
                        currentLevel++;
                        currentState = Stage.LevelClear;
                    } else
                    {
                        // Bounce
                        ballDirection.Y = -ballDirection.Y;
                    }

                    break;
            }


            ball = newLocation;
            drawBall(oldLocation, true, true);
        }

        // ---------------------------------------------------------------------------
        // Move the bat
        // ---------------------------------------------------------------------------
        void moveBat()
        {
            Vector3 dirn = controller.MoveIndicator;
            bool doMoveBat = false;
            if (dirn.X < 0 && batStart > 0)
            {
                batStart--;
                doMoveBat = true;
                //thisScreen[0] = 'L';
            }
            if (dirn.X > 0 && batStart < (cols - batLength))
            {
                batStart++;
                doMoveBat = true;
                //thisScreen[0] = 'R';
            }
            if (dirn.X == 0)
            {
                //thisScreen[0] = 'C';
            }

            if (allowspeed)
            {
                if (dirn.Z < 0) ballSpeed = ballSpeed * 2;
                if (dirn.Z > 0) ballSpeed = ballSpeed / 2;
            }
            if (doMoveBat) drawBat();
        }

        // ---------------------------------------------------------------------------
        // Redraw the bat line
        // ---------------------------------------------------------------------------
        void drawBat()
        {
            // Clear the whole bat row and draw the bat in the middle
            for (int j = 0; j < cols; j++)
            {
                if ((j >= batStart) && (j <= batStart + batLength))
                {
                    thisScreen[(batRow * colSizeChars) + j] = cWHITE;
                }
                else
                {
                    thisScreen[(batRow * colSizeChars) + j] = cBLACK;
                }
            }
        }

        // ---------------------------------------------------------------------------
        // Re-Initialize the bat at start of screen or death
        // ---------------------------------------------------------------------------
        void initializeBatAndBall(int lvlNumber)
        {
            batStart = (cols / 2) - (batLength / 2);

            // Draw the bat row
            drawBat();


            // Now initialize the ball position, speed etc
            ball = new Vector2((float)cols / 2, (float)rows / 2);
            ballDirection = new Vector2(((float)rnd.Next(-20, 20))/10, (float)1.0);

            ballSpeed = ((float)lvlNumber * 0.15F);

            // Draw the ball
            drawBall(new Vector2(), true, false);
        }

        // ---------------------------------------------------------------------------
        // Remove the ball from the old location and redraw in the new location
        // ---------------------------------------------------------------------------
        void drawBall(Vector2 oldLocation, bool add, bool remove)
        {

            if (remove)
            {
                // Remove it from the old location
                thisScreen[((int)oldLocation.Y * colSizeChars) + (int)oldLocation.X] = cBLACK;
            }

            if (add)
            {
                // Draw it at the new location
                thisScreen[((int)ball.Y * colSizeChars) + (int)ball.X] = cBALL;
            }
        }

        // ---------------------------------------------------------------------------
        // Initialize all variables and the screen array ready to play a level
        // ---------------------------------------------------------------------------
        void initializeLevel(int lvlNumber)
        {
            lives = 3;
            lcdscreen.FontSize = bestFontSize;
            lcdscreen.Alignment = TextAlignment.LEFT;

            // Clear the screen completely
            int idx = 0;
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    thisScreen[idx++] = cBLACK;
                }
                thisScreen[idx++] = '\n';
            }

            // Now add the bricks... So start at row (level - 1)
            bricksLeft = 0;
            // Level is {xxxBxxxBxxxB, blank line}*4  
            for (int i = 0; i < brickRows; i++)
            {
                int row = (lvlNumber - 1) + (i * 2);   // Slowly approach the user
                char rowColour = brickColours[((lvlNumber - 1) + i) % brickColours.Length];

                int col = 0;
                while (col < (cols - (brickLength - 1)))   // We dont need the space at the end
                {
                    bricksLeft++;
                    for (int j = 0; j < brickLength; j++)
                    {
                        thisScreen[(row * colSizeChars) + col] = rowColour;
                        col++;
                    }
                    col++; // Spacer which is black from above
                }
            }

            initializeBatAndBall(lvlNumber);
        }

        // ---------------------------------------------------------------------------
        // Display GameOver
        // ---------------------------------------------------------------------------
        void DisplayGameOver()
        {
            lcdscreen.FontSize = 1.0F;
            lcdscreen.Alignment = TextAlignment.CENTER;

            lcdscreen.WriteText("GAME OVER\nPress START to play\n", false);
        }

        // ----------------------------- CUT -------------------------------------
    }
}
