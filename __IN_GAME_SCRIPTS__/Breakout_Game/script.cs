/*
 * BREAKOUT
 * ========
 * 
 * This is a very simple implementation of Breakout - move the bat and keep the ball alive. I wrote this when learning about how to update screens (and the lack of graphics)
 * 
 * Source available via https://github.com/eddy0612/SpaceEngineerScripts
 * 
 * Instructions
 * ------------
 * 1. Create a programmable block, name it something likr `[GAMEPGM]`. Add custom data as follows - the value of the tag can be anything but you need to use it consistently everywhere as a prefix
 * 
 * ```
 * [config]
 * tag=game
 * ```
 * 
 * 2. Create an LCD, change its name to `[GAMESCREEN] Player1 lcd`  (only the tag [..] bit is important)
 * 3. In front of that add either a helm or cockpit, but in such a way that when in the cockpit you can see the whole screen. Change its name to `[GAMESEAT] Player1`
 * 4. Add the script to the programmable block, recompile and run.
 * 
 * Controls
 * --------
 * 
 * A - Left
 * D - Right
 */


// ----------------------------- CUT -------------------------------------
String thisScript = "Breakout_Game";

// Development time flags
public bool debug = false;
bool stayRunning = true;
bool dontdie = false;
bool allowspeed = false;

// Private variables
private JDBG jdbg = null;
private JINV jinv = null;
private JLCD jlcd = null;

// My configuration
/* Example custom data in programming block:
[Config]
tag=GAME
        */

// Ball colour
const char cBALL = JLCD.COLOUR_BLUE;

// My configuration
//int refreshSpeed = 5;                       // Default to 5 seconds if not provided
String mytag = "IDONTCARE";
//String alerttag = "ALERT";
IMyShipController controller = null;
List<IMyTerminalBlock> drawLCDs = null;
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
    jdbg = new JDBG(this, debug);
    jlcd = new JLCD(this, jdbg, false);
    jinv = new JINV(jdbg);
    jlcd.UpdateFullScreen(Me, thisScript);

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
        jdbg.DebugAndEcho("Using tag of " + mytag);
    }
    else
    {
        jdbg.DebugAndEcho("No tag configured\nPlease add [config] for tag=<substring>");
        return;
    }

    // ---------------------------------------------------------------------------
    // Find the screens to output to
    // ---------------------------------------------------------------------------
    List<IMyTerminalBlock> drawLCDs = jlcd.GetLCDsWithTag("[" + mytag.ToUpper() + ".SCREEN]");
    jdbg.DebugAndEcho("Found " + drawLCDs.Count + " screens");
    if (drawLCDs.Count > 1) {
        jdbg.DebugAndEcho("ERROR: Too many screens");
        return;
    }  else if (drawLCDs.Count == 0) {
        jdbg.DebugAndEcho("ERROR: No screen");
        return;
    } else {
        jlcd.InitializeLCDs(drawLCDs, TextAlignment.CENTER);
        lcdscreen = ((IMyTextSurfaceProvider)drawLCDs[0]).GetSurface(0);
    }

    // ---------------------------------------------------------------------------
    // Find the seat
    // ---------------------------------------------------------------------------
    List<IMyTerminalBlock> Controllers = new List<IMyTerminalBlock>();
    GridTerminalSystem.GetBlocksOfType(Controllers, (IMyTerminalBlock x) => (
                                                                           (x.CustomName != null) &&
                                                                           (x.CustomName.ToUpper().IndexOf("[" + mytag.ToUpper() + ".SEAT]") >= 0) &&
                                                                           (x is IMyShipController)
                                                                          ));
    jdbg.DebugAndEcho("Found " + Controllers.Count + " controllers");
    if (Controllers.Count > 0) {
        foreach (var thisblock in Controllers)
        {
            jdbg.Debug("- " + thisblock.CustomName);
        }
        if (Controllers.Count > 1)
        {
            jdbg.DebugAndEcho("ERROR: Too many controllers");
            return;
        }
        controller = (IMyShipController) Controllers[0];
    }
    else if (Controllers.Count == 0) {
        jdbg.DebugAndEcho("ERROR: No controllers");
        return;
    }

    // ---------------------------------------------------------------------------
    // Game initialization
    // ---------------------------------------------------------------------------
    colSizeChars = (cols + 1);  // Space for \n
    brickColours = new char[]{ JLCD.COLOUR_RED, JLCD.COLOUR_ORANGE, JLCD.COLOUR_YELLOW, JLCD.COLOUR_GREEN };

    // Work out the font size so we fill the screen
    rows = jlcd.SetupFontWidthOnly(drawLCDs, cols, true);
    bestFontSize = lcdscreen.FontSize;
    batRow = rows - 2;
    jdbg.DebugAndEcho("Screen is " + cols + " big");
    thisScreen = new StringBuilder("".PadLeft((rows * colSizeChars), JLCD.COLOUR_BLACK));
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
    jlcd.WriteToAllLCDs(drawLCDs, screenContents, false);
}

// ---------------------------------------------------------------------------
// Move the ball, handling bouncing etc
// ---------------------------------------------------------------------------
void moveBall()
{
    Vector2 oldLocation = ball;
    Vector2 newLocation = ball + (ballDirection * ballSpeed);

    // See if leaving screen, if so bounce back
    //jdbg.Debug("cols: " + cols + ", rows: " + rows);
    //jdbg.Debug("1:" + (int)oldLocation.X + "," + (int)oldLocation.Y + " -> " + (int)newLocation.X + "," + (int)newLocation.Y);
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
    //jdbg.Debug("2:" + (int)oldLocation.X + "," + (int)oldLocation.Y + " -> " + (int)newLocation.X + "," + (int)newLocation.Y);


    // If changed spot, remove old char and replace with new char
    int cx = (int)(newLocation.X);
    int cy = (int)(newLocation.Y);
    //jdbg.Debug("4:" + cx + "," + cy);


    // Whats at that spot on the screen?
    char newSpace = thisScreen[(cy * colSizeChars) + cx];
    switch (newSpace)
    {
        case JLCD.COLOUR_BLACK: // Empty space - NOOP
        case cBALL:  // Didnt leave space - NOOP
            break;
        case JLCD.COLOUR_WHITE: // Bounce off paddle
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
                thisScreen[(cy * colSizeChars) + brickStart + i] = JLCD.COLOUR_BLACK;
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
            thisScreen[(batRow * colSizeChars) + j] = JLCD.COLOUR_WHITE;
        }
        else
        {
            thisScreen[(batRow * colSizeChars) + j] = JLCD.COLOUR_BLACK;
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
        thisScreen[((int)oldLocation.Y * colSizeChars) + (int)oldLocation.X] = JLCD.COLOUR_BLACK;
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
            thisScreen[idx++] = JLCD.COLOUR_BLACK;
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

public class JCTRL
{
    MyGridProgram mypgm = null;
    JDBG jdbg = null;
    bool singleKey = false;

    public JCTRL(MyGridProgram pgm, JDBG dbg, bool SingleKeyOnly)
    {
        mypgm = pgm;
        jdbg = dbg;
    }

    // ---------------------------------------------------------------------------
    // Get a list of the LCDs with a specific tag
    // ---------------------------------------------------------------------------
    public List<IMyTerminalBlock> GetCTRLsWithTag(String tag)
    {
        List<IMyTerminalBlock> allCTRLs = new List<IMyTerminalBlock>();
        mypgm.GridTerminalSystem.GetBlocksOfType(allCTRLs, (IMyTerminalBlock x) => (
                                                                               (x.CustomName != null) &&
                                                                               (x.CustomName.ToUpper().IndexOf("[" + tag.ToUpper() + "]") >= 0) &&
                                                                               (x is IMyShipController)
                                                                              ));
        jdbg.Debug("Found " + allCTRLs.Count + " controllers with tag " + tag);
        return allCTRLs;
    }

    public bool IsOccupied(IMyShipController seat)
    {
        return seat.IsUnderControl;
    }

    public bool AnyKey(IMyShipController seat, bool allowJumpOrCrouch)
    {
        bool pressed = false;
        Vector3 dirn = seat.MoveIndicator;
        if (dirn.X != 0 || (allowJumpOrCrouch && dirn.Y != 0) || dirn.Z != 0) {
            pressed = true;
        }
        return pressed;
    }

    public bool IsLeft(IMyShipController seat)
    {
        Vector3 dirn = seat.MoveIndicator;
        if (singleKey && dirn.X < 0 && dirn.Y == 0 && dirn.Z == 0) return true;
        else if (!singleKey && dirn.X < 0) return true;
        return false;
    }
    public bool IsRight(IMyShipController seat)
    {
        Vector3 dirn = seat.MoveIndicator;
        if (singleKey && dirn.X > 0 && dirn.Y == 0 && dirn.Z == 0) return true;
        else if (!singleKey && dirn.X > 0) return true;
        return false;
    }
    public bool IsUp(IMyShipController seat)
    {
        Vector3 dirn = seat.MoveIndicator;
        if (singleKey && dirn.X == 0 && dirn.Y == 0 && dirn.Z < 0) return true;
        else if (!singleKey && dirn.Z < 0) return true;
        return false;
    }
    public bool IsDown(IMyShipController seat)
    {
        Vector3 dirn = seat.MoveIndicator;
        if (singleKey && dirn.X == 0 && dirn.Y == 0 && dirn.Z > 0) return true;
        else if (!singleKey && dirn.Z > 0) return true;
        return false;
    }
    public bool IsSpace(IMyShipController seat)
    {
        Vector3 dirn = seat.MoveIndicator;
        if (singleKey && dirn.X == 0 && dirn.Y > 0 && dirn.Z == 0) return true;
        else if (!singleKey && dirn.Y > 0) return true;
        return false;
    }
    public bool IsCrouch(IMyShipController seat)
    {
        Vector3 dirn = seat.MoveIndicator;
        if (singleKey && dirn.X == 0 && dirn.Y < 0 && dirn.Z == 0) return true;
        else if (!singleKey && dirn.Y < 0) return true;
        return false;
    }
    public bool IsRollLeft(IMyShipController seat)
    {
        float dirn = seat.RollIndicator;
        if (dirn < 0.0) return true;
        return false;
    }
    public bool IsRollRight(IMyShipController seat)
    {
        float dirn = seat.RollIndicator;
        if (dirn > 0.0) return true;
        return false;
    }
    public bool IsArrowLeft(IMyShipController seat)
    {
        Vector2 dirn = seat.RotationIndicator;
        if (singleKey && dirn.X == 0 && dirn.Y < 0) return true;
        else if (!singleKey && dirn.Y < 0) return true;
        return false;
    }
    public bool IsArrowRight(IMyShipController seat)
    {
        Vector2 dirn = seat.RotationIndicator;
        if (singleKey && dirn.X == 0 && dirn.Y > 0) return true;
        else if (!singleKey && dirn.Y > 0) return true;
        return false;
    }
    public bool IsArrowDown(IMyShipController seat)
    {
        Vector2 dirn = seat.RotationIndicator;
        if (singleKey && dirn.X > 0 && dirn.Y == 0) return true;
        else if (!singleKey && dirn.X > 0) return true;
        return false;
    }
    public bool IsArrowUp(IMyShipController seat)
    {
        Vector2 dirn = seat.RotationIndicator;
        if (singleKey && dirn.X < 0 && dirn.Y == 0) return true;
        else if (!singleKey && dirn.X < 0) return true;
        return false;
    }
}

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
                                                                                  (x.CustomName.ToUpper().IndexOf("[" + alertTag.ToUpper() + "]") >= 0) &&
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

public class JINV
{
    private JDBG jdbg = null;

    public enum BLUEPRINT_TYPES { BOTTLES, COMPONENTS, AMMO, TOOLS, OTHER, ORES };

    /* Ore to Ingot */
    Dictionary<String, String> oreToIngot = new Dictionary<String, String>
    {
        { "MyObjectBuilder_Ore/Cobalt", "MyObjectBuilder_Ingot/Cobalt" },
        { "MyObjectBuilder_Ore/Gold", "MyObjectBuilder_Ingot/Gold" },
        { "MyObjectBuilder_Ore/Stone", "MyObjectBuilder_Ingot/Stone" },
        { "MyObjectBuilder_Ore/Iron", "MyObjectBuilder_Ingot/Iron" },
        { "MyObjectBuilder_Ore/Magnesium", "MyObjectBuilder_Ingot/Magnesium" },
        { "MyObjectBuilder_Ore/Nickel", "MyObjectBuilder_Ingot/Nickel" },
        { "MyObjectBuilder_Ore/Platinum", "MyObjectBuilder_Ingot/Platinum" },
        { "MyObjectBuilder_Ore/Silicon", "MyObjectBuilder_Ingot/Silicon" },
        { "MyObjectBuilder_Ore/Silver", "MyObjectBuilder_Ingot/Silver" },
        { "MyObjectBuilder_Ore/Uranium", "MyObjectBuilder_Ingot/Uranium" },

        // Not needing these at present:
        //{ "MyObjectBuilder_Ore/Scrap", "MyObjectBuilder_Ingot/Scrap" },
        //{ "MyObjectBuilder_Ore/Ice", "?" },
        //{ "MyObjectBuilder_Ore/Organic", "?" },
    };

    /* Other stuff */
    Dictionary<String, String> otherCompToBlueprint = new Dictionary<String, String>
    {
        { "MyObjectBuilder_BlueprintDefinition/Position0040_Datapad", "MyObjectBuilder_Datapad/Datapad" },
    };

    /* Tools */
    Dictionary<String, String> toolsCompToBlueprint = new Dictionary<String, String>
    {
        { "MyObjectBuilder_PhysicalGunObject/AngleGrinderItem" , "MyObjectBuilder_BlueprintDefinition/Position0010_AngleGrinder" },
        { "MyObjectBuilder_PhysicalGunObject/AngleGrinder2Item", "MyObjectBuilder_BlueprintDefinition/Position0020_AngleGrinder2" },
        { "MyObjectBuilder_PhysicalGunObject/AngleGrinder3Item" , "MyObjectBuilder_BlueprintDefinition/Position0030_AngleGrinder3" },
        { "MyObjectBuilder_PhysicalGunObject/AngleGrinder4Item" , "MyObjectBuilder_BlueprintDefinition/Position0040_AngleGrinder4" },

        { "MyObjectBuilder_PhysicalGunObject/WelderItem" , "MyObjectBuilder_BlueprintDefinition/Position0090_Welder" },
        { "MyObjectBuilder_PhysicalGunObject/Welder2Item" , "MyObjectBuilder_BlueprintDefinition/Position0100_Welder2" },
        { "MyObjectBuilder_PhysicalGunObject/Welder3Item" , "MyObjectBuilder_BlueprintDefinition/Position0110_Welder3" },
        { "MyObjectBuilder_PhysicalGunObject/Welder4Item" , "MyObjectBuilder_BlueprintDefinition/Position0120_Welder4" },

        { "MyObjectBuilder_PhysicalGunObject/HandDrillItem", "MyObjectBuilder_BlueprintDefinition/Position0050_HandDrill" },
        { "MyObjectBuilder_PhysicalGunObject/HandDrill2Item", "MyObjectBuilder_BlueprintDefinition/Position0060_HandDrill2" },
        { "MyObjectBuilder_PhysicalGunObject/HandDrill3Item" , "MyObjectBuilder_BlueprintDefinition/Position0070_HandDrill3" },
        { "MyObjectBuilder_PhysicalGunObject/HandDrill4Item" , "MyObjectBuilder_BlueprintDefinition/Position0080_HandDrill4" },

    };

    /* Bottles */
    Dictionary<String, String> bottlesCompToBlueprint = new Dictionary<String, String>
    {
        { "MyObjectBuilder_GasContainerObject/HydrogenBottle", "MyObjectBuilder_BlueprintDefinition/Position0020_HydrogenBottle" },
        { "MyObjectBuilder_OxygenContainerObject/OxygenBottle", "MyObjectBuilder_BlueprintDefinition/HydrogenBottlesRefill" },
    };

    /* Components */
    Dictionary<String, String> componentsCompToBlueprint = new Dictionary<String, String>
    {
        { "MyObjectBuilder_Component/BulletproofGlass", "MyObjectBuilder_BlueprintDefinition/BulletproofGlass"},
        { "MyObjectBuilder_Component/Canvas", "MyObjectBuilder_BlueprintDefinition/Position0030_Canvas"},
        { "MyObjectBuilder_Component/Computer", "MyObjectBuilder_BlueprintDefinition/ComputerComponent"},
        { "MyObjectBuilder_Component/Construction", "MyObjectBuilder_BlueprintDefinition/ConstructionComponent"},
        { "MyObjectBuilder_Component/Detector", "MyObjectBuilder_BlueprintDefinition/DetectorComponent"},
        { "MyObjectBuilder_Component/Display", "MyObjectBuilder_BlueprintDefinition/Display"},
        { "MyObjectBuilder_Component/Explosives", "MyObjectBuilder_BlueprintDefinition/ExplosivesComponent"},
        { "MyObjectBuilder_Component/Girder", "MyObjectBuilder_BlueprintDefinition/GirderComponent"},
        { "MyObjectBuilder_Component/GravityGenerator", "MyObjectBuilder_BlueprintDefinition/GravityGeneratorComponent"},
        { "MyObjectBuilder_Component/InteriorPlate", "MyObjectBuilder_BlueprintDefinition/InteriorPlate"},
        { "MyObjectBuilder_Component/LargeTube", "MyObjectBuilder_BlueprintDefinition/LargeTube"},
        { "MyObjectBuilder_Component/Medical", "MyObjectBuilder_BlueprintDefinition/MedicalComponent"},
        { "MyObjectBuilder_Component/MetalGrid", "MyObjectBuilder_BlueprintDefinition/MetalGrid"},
        { "MyObjectBuilder_Component/Motor", "MyObjectBuilder_BlueprintDefinition/MotorComponent"},
        { "MyObjectBuilder_Component/PowerCell", "MyObjectBuilder_BlueprintDefinition/PowerCell"},
        { "MyObjectBuilder_Component/Reactor", "MyObjectBuilder_BlueprintDefinition/ReactorComponent"},
        { "MyObjectBuilder_Component/RadioCommunication", "MyObjectBuilder_BlueprintDefinition/RadioCommunicationComponent"},
        { "MyObjectBuilder_Component/SmallTube", "MyObjectBuilder_BlueprintDefinition/SmallTube"},
        { "MyObjectBuilder_Component/SolarCell", "MyObjectBuilder_BlueprintDefinition/SolarCell"},
        { "MyObjectBuilder_Component/SteelPlate", "MyObjectBuilder_BlueprintDefinition/SteelPlate"},
        { "MyObjectBuilder_Component/Superconductor", "MyObjectBuilder_BlueprintDefinition/Superconductor"},
        { "MyObjectBuilder_Component/Thrust", "MyObjectBuilder_BlueprintDefinition/ThrustComponent"},
    };

    /* Ammo */
    Dictionary<String, String> ammoCompToBlueprint = new Dictionary<String, String>
    {
        /* Ammo */
        { /*"Gatling",*/            "MyObjectBuilder_AmmoMagazine/NATO_25x184mm", "MyObjectBuilder_BlueprintDefinition/Position0080_NATO_25x184mmMagazine" },
        { /*"Autocannon",*/         "MyObjectBuilder_AmmoMagazine/AutocannonClip", "MyObjectBuilder_BlueprintDefinition/Position0090_AutocannonClip" },
        { /*"Rocket",*/             "MyObjectBuilder_AmmoMagazine/Missile200mm", "MyObjectBuilder_BlueprintDefinition/Position0100_Missile200mm" },
        { /*"Assault_Cannon",*/     "MyObjectBuilder_AmmoMagazine/MediumCalibreAmmo", "MyObjectBuilder_BlueprintDefinition/Position0110_MediumCalibreAmmo" },
        { /*"Artillery",*/          "MyObjectBuilder_AmmoMagazine/LargeCalibreAmmo", "MyObjectBuilder_BlueprintDefinition/Position0120_LargeCalibreAmmo" },
        { /*"Small_Railgun",*/      "MyObjectBuilder_AmmoMagazine/SmallRailgunAmmo", "MyObjectBuilder_BlueprintDefinition/Position0130_SmallRailgunAmmo" },
        { /*"Large_Railgun",*/      "MyObjectBuilder_AmmoMagazine/LargeRailgunAmmo", "MyObjectBuilder_BlueprintDefinition/Position0140_LargeRailgunAmmo" },
        { /*"S-10_Pistol",*/        "MyObjectBuilder_AmmoMagazine/SemiAutoPistolMagazine", "MyObjectBuilder_BlueprintDefinition/Position0010_SemiAutoPistolMagazine" },
        { /*"S-10E_Pistol",*/       "MyObjectBuilder_AmmoMagazine/ElitePistolMagazine", "MyObjectBuilder_BlueprintDefinition/Position0030_ElitePistolMagazine" },
        { /*"S-20A_Pistol",*/       "MyObjectBuilder_AmmoMagazine/FullAutoPistolMagazine", "MyObjectBuilder_BlueprintDefinition/Position0020_FullAutoPistolMagazine" },
        { /*"MR-20_Rifle",*/        "MyObjectBuilder_AmmoMagazine/AutomaticRifleGun_Mag_20rd", "MyObjectBuilder_BlueprintDefinition/Position0040_AutomaticRifleGun_Mag_20rd" },
        { /*"MR-30E_Rifle",*/       "MyObjectBuilder_AmmoMagazine/UltimateAutomaticRifleGun_Mag_30rd", "MyObjectBuilder_BlueprintDefinition/Position0070_UltimateAutomaticRifleGun_Mag_30rd" },
        { /*"MR-50A_Rifle",*/       "MyObjectBuilder_AmmoMagazine/RapidFireAutomaticRifleGun_Mag_50rd", "MyObjectBuilder_BlueprintDefinition/Position0050_RapidFireAutomaticRifleGun_Mag_50rd" },
        { /*"MR-8P_Rifle",*/        "MyObjectBuilder_AmmoMagazine/PreciseAutomaticRifleGun_Mag_5rd", "MyObjectBuilder_BlueprintDefinition/Position0060_PreciseAutomaticRifleGun_Mag_5rd" },
        { /*"5.56x45mm NATO magazine",*/ "MyObjectBuilder_AmmoMagazine/NATO_5p56x45mm", null /* Not Craftable */ },
    };
    public JINV(JDBG dbg)
    {
        jdbg = dbg;
    }
    public void addBluePrints(BLUEPRINT_TYPES types, ref Dictionary<String, String> into)
    {
        switch (types)
        {
            case BLUEPRINT_TYPES.BOTTLES:
                into = into.Concat(bottlesCompToBlueprint).ToDictionary(x => x.Key, x => x.Value);
                break;
            case BLUEPRINT_TYPES.COMPONENTS:
                into = into.Concat(componentsCompToBlueprint).ToDictionary(x => x.Key, x => x.Value);
                break;
            case BLUEPRINT_TYPES.AMMO:
                into = into.Concat(ammoCompToBlueprint).ToDictionary(x => x.Key, x => x.Value);
                break;
            case BLUEPRINT_TYPES.TOOLS:
                into = into.Concat(toolsCompToBlueprint).ToDictionary(x => x.Key, x => x.Value);
                break;
            case BLUEPRINT_TYPES.OTHER:
                into = into.Concat(otherCompToBlueprint).ToDictionary(x => x.Key, x => x.Value);
                break;
            case BLUEPRINT_TYPES.ORES:
                into = into.Concat(oreToIngot).ToDictionary(x => x.Key, x => x.Value);
                break;
        }
    }
}

public class JLCD
{
    public MyGridProgram mypgm = null;
    public JDBG jdbg = null;
    bool suppressDebug = false;

    // Useful for lookup via colour
    public static Dictionary<String, char> solidcolor = new Dictionary<String, char>
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
        { "GREY", '' }
    };

    // Useful for direct code
    public const char COLOUR_YELLOW = '';
    public const char COLOUR_RED = '';
    public const char COLOUR_ORANGE = '';
    public const char COLOUR_GREEN = '';
    public const char COLOUR_CYAN = '';
    public const char COLOUR_PURPLE = '';
    public const char COLOUR_BLUE = '';
    public const char COLOUR_WHITE = '';
    public const char COLOUR_BLACK = '';
    public const char COLOUR_GREY = '';

    public JLCD(MyGridProgram pgm, JDBG dbg, bool suppressDebug)
    {
        this.mypgm = pgm;
        this.jdbg = dbg;
        this.suppressDebug = suppressDebug; /* Suppress the 'Writing to...' msg */
    }

    // ---------------------------------------------------------------------------
    // Get a list of the LCDs with a specific tag
    // ---------------------------------------------------------------------------
    public List<IMyTerminalBlock> GetLCDsWithTag(String tag)
    {
        List<IMyTerminalBlock> allLCDs = new List<IMyTerminalBlock>();
        mypgm.GridTerminalSystem.GetBlocksOfType(allLCDs, (IMyTerminalBlock x) => (
                                                                               (x.CustomName != null) &&
                                                                               (x.CustomName.ToUpper().IndexOf("[" + tag.ToUpper() + "]") >= 0) &&
                                                                               (x is IMyTextSurfaceProvider)
                                                                              ));
        jdbg.Debug("Found " + allLCDs.Count + " lcds to update with tag " + tag);
        return allLCDs;
    }

    // ---------------------------------------------------------------------------
    // Get a list of the LCDs with a specific name
    // ---------------------------------------------------------------------------
    public List<IMyTerminalBlock> GetLCDsWithName(String tag)
    {
        List<IMyTerminalBlock> allLCDs = new List<IMyTerminalBlock>();
        mypgm.GridTerminalSystem.GetBlocksOfType(allLCDs, (IMyTerminalBlock x) => (
                                                                               (x.CustomName != null) &&
                                                                               (x.CustomName.ToUpper().IndexOf(tag.ToUpper()) >= 0) &&
                                                                               (x is IMyTextSurfaceProvider)
                                                                              ));
        jdbg.Debug("Found " + allLCDs.Count + " lcds to update with tag " + tag);
        return allLCDs;
    }

    // ---------------------------------------------------------------------------
    // Initialize LCDs
    // ---------------------------------------------------------------------------
    public void InitializeLCDs(List<IMyTerminalBlock> allLCDs, TextAlignment align)
    {
        foreach (var thisLCD in allLCDs)
        {
            jdbg.Debug("Setting up the font for " + thisLCD.CustomName);

            // Set it to text / monospace, black background
            // Also set the padding
            IMyTextSurface thisSurface = ((IMyTextSurfaceProvider)thisLCD).GetSurface(0);
            if (thisSurface == null) continue;
            thisSurface.Font = "Monospace";
            thisSurface.ContentType = ContentType.TEXT_AND_IMAGE;
            thisSurface.BackgroundColor = Color.Black;
            thisSurface.Alignment = align;
        }
    }

    public void SetLCDFontColour(List<IMyTerminalBlock> allLCDs, Color colour)
    {
        foreach (var thisLCD in allLCDs) {
            if (thisLCD is IMyTextPanel) {
                jdbg.Debug("Setting up the color for " + thisLCD.CustomName);
                ((IMyTextPanel)thisLCD).FontColor = colour;
            }
        }
    }

    public void SetLCDRotation(List<IMyTerminalBlock> allLCDs, float Rotation)
    {
        foreach (var thisLCD in allLCDs) {
            if (thisLCD is IMyTextPanel) {
                jdbg.Debug("Setting up the rotation for " + thisLCD.CustomName);
                thisLCD.SetValueFloat("Rotate", Rotation);
            }
        }
    }

    // ---------------------------------------------------------------------------
    // Set the font and display properties of each display UNLESS [LOCKED]
    // ---------------------------------------------------------------------------
    public void SetupFont(List<IMyTerminalBlock> allLCDs, int rows, int cols, bool mostlySpecial)
    {
        _SetupFontCalc(allLCDs, ref rows, cols, mostlySpecial, 0.05F, 0.05F);
    }
    public int SetupFontWidthOnly(List<IMyTerminalBlock> allLCDs, int cols, bool mostlySpecial)
    {
        int rows = -1;
        _SetupFontCalc(allLCDs, ref rows, cols, mostlySpecial, 0.05F, 0.05F);
        return rows;
    }
    public void SetupFontCustom(List<IMyTerminalBlock> allLCDs, int rows, int cols, bool mostlySpecial, float size, float incr)
    {
        _SetupFontCalc(allLCDs, ref rows, cols, mostlySpecial, size,incr);
    }

    private void _SetupFontCalc(List<IMyTerminalBlock> allLCDs, ref int rows, int cols, bool mostlySpecial, float startSize, float startIncr)
    {
        int bestRows = rows;
        foreach (var thisLCD in allLCDs)
        {
            jdbg.Debug("Setting up font on screen: " + thisLCD.CustomName + " (" + rows + " x " + cols + ")");
            IMyTextSurface thisSurface = ((IMyTextSurfaceProvider)thisLCD).GetSurface(0);
            if (thisSurface == null) continue;

            // Now work out the font size
            float size = startSize;
            float incr = startIncr;

            // Get a list of cols strings
            StringBuilder teststr = new StringBuilder("".PadRight(cols, (mostlySpecial? solidcolor["BLACK"] : 'W')));
            Vector2 actualScreenSize = thisSurface.TextureSize;
            while (true)
            {
                thisSurface.FontSize = size;
                Vector2 actualSize = thisSurface.TextureSize;

                Vector2 thisSize = thisSurface.MeasureStringInPixels(teststr, thisSurface.Font, size);
                //jdbg.Debug("..with size " + size + " width is " + thisSize.X + " max " + actualSize.X);
                //jdbg.Debug("..with size " + size + " height is " + thisSize.Y + " max " + actualSize.Y);

                int displayrows = (int)Math.Floor(actualScreenSize.Y / thisSize.Y);

                if ((thisSize.X < actualSize.X) && (rows == -1 || (displayrows > rows)))
                {
                    size += incr;
                    bestRows = displayrows;
                }
                else
                {
                    break;
                }
            }
            thisSurface.FontSize = size - incr;
            jdbg.Debug("Calc size of " + thisSurface.FontSize);

            /* If we were asked how many rows for given width, return it */
            if (rows == -1) rows = bestRows;

            // BUG? Corner LCDs are a factor of 4 out - no idea why but try *4
            if (thisLCD.DefinitionDisplayNameText.Contains("Corner LCD")) {
                jdbg.Debug("INFO: Avoiding bug, multiplying by 4: " + thisLCD.DefinitionDisplayNameText);
                thisSurface.FontSize *= 4;
            }
        }
    }

    // ---------------------------------------------------------------------------
    // Update the programmable block with the script name
    // ---------------------------------------------------------------------------
    public void UpdateFullScreen(IMyTerminalBlock block, String text)
    {
        List<IMyTerminalBlock> lcds = new List<IMyTerminalBlock> { block };
        InitializeLCDs(lcds, TextAlignment.CENTER);
        SetupFont(lcds, 1, text.Length + 4, false);
        WriteToAllLCDs(lcds, text, false);
    }

    // ---------------------------------------------------------------------------
    // Write a message to all related LCDs
    // ---------------------------------------------------------------------------
    public void WriteToAllLCDs(List<IMyTerminalBlock> allLCDs, String msg, bool append)
    {
        //if (allLCDs == null) {
            //jdbg.Debug("WARNING: No screen to write to");
            //return;
        //}
        foreach (var thisLCD in allLCDs)
        {
            if (!this.suppressDebug) jdbg.Debug("Writing to display " + thisLCD.CustomName);
            IMyTextSurface thisSurface = ((IMyTextSurfaceProvider)thisLCD).GetSurface(0);
            if (thisSurface == null) continue;
            thisSurface.WriteText(msg, append);
        }
    }

    // ---------------------------------------------------------------------------
    // Convert r g b to a character - Credit
    //    to https://github.com/Whiplash141/Whips-Image-Converter
    // ---------------------------------------------------------------------------
    public char ColorToChar(byte r, byte g, byte b)
    {
        const double bitSpacing = 255.0 / 7.0;
        return (char)(0xe100 + ((int)Math.Round(r / bitSpacing) << 6) + ((int)Math.Round(g / bitSpacing) << 3) + (int)Math.Round(b / bitSpacing));
    }

}