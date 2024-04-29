/*
 * ## Intel 8080 Arcade Machine Emulator instructions
 * 
 * ### Multiple game arcade machine emulator (Intel 8080)
 * 
 * This script emulates the 8080 used in the arcade machines very early on, in the era of Space 
 * Invaders. It can play multiple games and (on my machine) hits pretty much original speed. 
 * Unfortunately all arcade machines were subtely different so you do need to tell the emulator 
 * which game you are running, and that is done through the ROM filename (as known to MAME)
 * 
 * This is a very intensive script and updates an LCD at up to 60 frames per second, and is
 * not a good idea to run this on a multiplayer server. I also suggest you turn the programmable 
 * block off when player leaves the controls as well for example. The games play without sound
 * as there was no way to play such sounds in a programmable block presently.
 * 
 * Code available on github: https://github.com/eddy0612/SpaceEngineerScripts
 * 
 * Q: Why did I do this? I wrote some simple in game scripts, and got bored of writing them one
 *       by one, and started playing... Then I got addicted to getting as much running as I could
 * 
 * All feedback welcome, please use the discussion area of my github repo
 *        
 * ```
 * +--------------------------------------------------------------------------------------------------
 * | THIS SCRIPT DOES NOT COME WITH ANY GAMES PREINSTALLED due to copyright. 
 * |
 * | Instructions provided below, BUT you need to get the roms and their associated strings which 
 * | requires (on windows) use of command line (cmd.exe), and one of
 * |   - python
 * |   - 2 utilities usually provided by eg cygwin - unzip and base64.
 * +--------------------------------------------------------------------------------------------------
 * ```
 * 
 * ## Installation instructions
 * 
 * 1. Create empty programmable block named "[GAME.PGM] Block". Add this script to it, and modify the custom data to say:
 * 
 * ```
 *     [config]
 * 	tag=GAME
 * ```
 * 
 * + Note: (The prefix `GAME` is used for these instructions, you can use what you like)
 * 2. Add an LCD, and name it "[GAME.SCREEN] LCD"
 * 3. Add something that controls a ship in front of it, eg a cockpit or even better a helm - A 
 *    good idea to place it where you can see the screen!. Name it "[GAME.SEAT] Controller". Sit in 
 *    that cockpit, and press <tab> until the HUD is clear.
 * 4. While still in that seat, Press G and type 'GAME'. Drag the '[GAME.PGM] Block' down to the 
 *    toolbar buttons, and chose the 'Run' option. In the popup, you can enter the parameter of the 
 *    filename of the game you wish to start.  I set up one button per filename, but you only need
 *    a single one to get it working.
 * 5. Obtain the rom(s) you can play (from the list below) from the internet. A search on MAME with 
 *    filename is usually a good hit, see below. Remember what directory you saved the ROM under,
 *    which on windows is usually something like `c:\users\myuserid\downloads`
 * + NOTE: You need the EXACT names listed below for this to work, similar files will not be
 *    handled correctly and will not work
 * 
 * -------------------------------------
 * EITHER Step 6 using python...
 *    You need to have manually installed python first from https://www.python.org/downloads/windows/
 *    (Pick the Windows Installer (64-bit) one, and run the installer)
 * 
 * 6. Download GetRomData.py (or paste in from bottom of these instructions) and save it in the same
 *    directory as you saved the rom under. 
 *    
 * +  At this point you need to use a command prompt. Launch cmd.exe... you should get a black windowed 
 *     screen. In there type `cd /d "path_to_rom"` eg `cd /d "c:\users\myuser\downloads"`
 * +  Then for each game, run one of the following.. If the command fails, try the other one. 
 *      
 * 			 python3 GetRomData.py invaders.zip    OR
 * 			 python GetRomData.py invaders.zip
 * 
 * + This will create a text file alongside the zip file with the contents you need later on, eg `invaders.zip.txt`
 * + Copy the contents of this text file to the clipboard, which can be simply done in the command prompt by
 *       issuing `clip < invaders.zip.txt` or whatever the ROM name was.
 *  
 * OR Step 6 using cygwin:
 * 
 * 6. Using cygwin's unzip and base64, in a comment prompt, do the following, for example:
 * 			 `unzip -p invaders.zip | base64 --wrap=0 > invaders.zip.txt`
 * 
 * + This will create a text file alongside the zip file with the contents you need later on, eg `invaders.zip.txt`
 * + Copy the contents of this text file to the clipboard, which can be simply done in the command prompt by
 *       issuing `clip < invaders.zip.txt` or whatever the ROM name was.
 * -------------------------------------
 * 
 * 7. Now go to Space engineers, and create a block... Any block with a custom data will do but I 
 * generally use control panels as they are tiny. Name them `[GAME] filename>`, eg `[GAME] invaders.zip`.
 * + In the custom data, paste in what step 6 stored in the clipboard which is a long string. (I've put the 
 * expected first few characters in the table below)
 * 
 * 8. Go to the programmable block, click recompile then run. 
 * + Check for any error messages in the bottom right of the screen. If is ok to say no game configured
 * 
 * 9. Go back to the helm/cockpit and launch a game by pressing the appropriate helm button
 * + You can switch between running games as well if you have more than one ROM
 * 
 * 10. If things go wrong at any point, you may need to click 'recompile' on the programmable block again to get things reset.
 * 
 * 
 * ## Controls
 * 
 * ```
 *       A - LEFT          W - UP				Q - Add credit
 *       D - RIGHT         S - DOWN			E - Player 1 start
 *       SPACE - Fire      CROUCH - Button 2
 * ```
 *       
 * ## Currently supports: (filename, name, first 10 chars expected, sample place to get it)
 * 
 * | Filename | Game | First chars of string | example download location |
 * | -- | -- | -- | -- |
 * | invaders.zip | Space Invaders			| IMPJFiGEIH | https://www.retrostic.com/roms/mame/space-invaders-space-invaders-m-41304
 * | ballbomb.zip | Balloon Bomber		    | AAAAwxgAAA | https://www.retrostic.com/roms/mame/balloon-bomber-44465
 * | lrescue.zip  | Lunar Rescue			| Dw8PDw8PDw | https://www.retrostic.com/roms/mame/lunar-rescue-43291#google_vignette
 * | schaser.zip  | Space Chaser			| Dw8PDw8PDw | https://www.retrostic.com/roms/mame/space-chaser-47039
 * | spacerng.zip | Space Ranger			| CAgICAgICA | https://www.retrostic.com/roms/mame/space-ranger-51087
 * | vortex.zip   | Vortex					| wicZKqd9Ks | https://www.retrostic.com/roms/mame/vortex-50198
 * | invrvnge.zip | Invaders Revenge		| ////////// | https://www.retrostic.com/roms/mame/invader-s-revenge-46143
 * | galxwars.zip | Galaxy Wars			| CAgICAgICA | https://www.retrostic.com/roms/mame/galaxy-wars-43764
 * | rollingc.zip | Rolling Crash/Moon Base| 88MAQNQYAA | https://www.retrostic.com/roms/mame/rolling-crash-moon-base-49086
 * | lupin3.zip   | Lupin III				| AAAAw0AAAA | https://www.retrostic.com/roms/mame/lupin-iii-44777
 * | polaris.zip  | Polaris				| AAAAw8YFAA | https://www.retrostic.com/roms/mame/polaris-45374
 * | indianbt.zip | Indian Battle			| AAAAw+VfKw | https://www.retrostic.com/roms/mame/indian-battle-46986
 * | astropal.zip | Astropal				| MaAglwDDtg | https://www.retrostic.com/roms/mame/astropal-47538
 * | galactic.zip | Galactica				| AAAAw5sA// | https://www.retrostic.com/roms/mame/galactica-batalha-espacial-45078
 * | attackfc.zip | Attack Force			| PgAyKyLDAA | https://www.retrostic.com/roms/mame/attack-force-47625
 * 
 *  
 * ### Credits:
 *   - This is a majorly reworked version of code based on an emulator created by Pedro Cortés
 *        (BluestormDNA) and found here: https://github.com/BluestormDNA/i8080-Space-Invaders
 *      The CPU is only slighty modified but the rest was reworked to fit within Space Engineers
 *        limitations and fixed/extended for the other games, graphics, cycle handling and colour.
 *    - MAME source was reviewed to assist in getting the variety of games working
 *           https://github.com/mamedev/mame
 * 
 * 
 * ## Additional options / functionality
 * 
 *    - Tag an LCD with eg. "[GAME.FPS] fps display" and it will be updated with an approx frames per
 *        second figure
 *    - Tag an LCD with eg. "[GAME.NAME] name display" and it will be updated with the name of the
 *        current game
 *    - Reduce the (very heavy) LCD panel impact by only drawing every other frame for example
 *        Add to the [config] something like the following (Not supplied or value of 1 means 
 *        draw every frame, 2 means every 2nd frame, 3 means every third etc:
 *        `renderframe=2`
 *    - Set a limit on how many cycles the game can take up - default is 45000, max is 49000. Less 
 *        cycles means slower FPS.  Add to the [config] something like
 *        `speed=40000`
 *    - Work like an arcade machine.. Add to the [config] something like
 * 		`cost=5`
 * 		`safetag=SAFE`
 *      Now, tag one cargo container near the machine with [GAME.COINS] Coin In, and another
 *        (which is connected via a conveyor, ideally with a one way sorter to stop things being
 *         removed!) as [SAFE] Coins Safe
 *      If cost is >0 then 'q' will no longer act as coin in, instead drop the space credits as a single
 *        stack into the "coins in" cargo container, and it should add a credit to the game, and move
 *        the space credits out into the safe... 
 *    
 * #### GetRomData.py code (available at https://github.com/eddy0612/SpaceEngineerScripts/blob/master/ArcadeEmulation/GetRomData.py)
 * ```
 * import sys,os,base64
 * from zipfile import ZipFile
 * def extract_zip(input_zip):
 *     input_zip=ZipFile(input_zip)
 *     return {name: input_zip.read(name) for name in input_zip.namelist()}     
 * zip_name = os.sys.argv[1]
 * zip_data = extract_zip(zip_name)
 * total_data = bytearray()
 * for name,allbytes in zip_data.items(): total_data = total_data + allbytes
 * f = open(str(os.sys.argv[1]) + ".txt", "w")
 * f.write(base64.b64encode(total_data).decode("utf-8"))
 * f.close()
 * print( "File " + str(os.sys.argv[1]) + ".zip created")
 * print( "On windows you can send to clipboard by issuing:")
 * print( "  clip < \""+ str(os.sys.argv[1]) + ".zip\"")
 * ```
 */
String thisScript="ArcadeEmulation";bool debug=false;String mytag="GAME";String safetag="SAFE";JDBG jdbg=null;JLCD jlcd=
null;List<IMyTerminalBlock>displays=null;List<IMyTerminalBlock>fpsdisplays=null;List<IMyTerminalBlock>namedisplays=null;
IMyInventory coinInInv=null;IMyInventory coinOutInv=null;IMyShipController controller=null;Arcade8080Machine si=null;int Frame=0;int
curSec=0;int curFrames=0;int actFrames=0;int lastFrames=0;int cost=0;DateTime inserted;DateTime lastCoinCheck;Program.
GetRomData.Games currentGame=Program.GetRomData.Games.None;List<MyInventoryItem>itemList=new List<MyInventoryItem>();String
notOkToPlay=null;Program(){Echo("Start");jdbg=new JDBG(this,debug);jlcd=new JLCD(this,jdbg,true);jlcd.UpdateFullScreen(Me,
thisScript);notOkToPlay=null;MyIniParseResult result;MyIni _ini=new MyIni();if(!_ini.TryParse(Me.CustomData,out result))throw new
Exception(result.ToString());mytag=_ini.Get("config","tag").ToString();if(mytag!=null){mytag=(mytag.Split(';')[0]).Trim().ToUpper
();}else{notOkToPlay="ERROR: No tag configured\nPlease add [config] for tag=<substring>";Echo(notOkToPlay);return;}jdbg.
Debug("Config: tag="+mytag);int maxFrames=_ini.Get("config","speed").ToInt32(45000);if(maxFrames>1000&&maxFrames<49000){
Specifics.maxFrames=maxFrames;}int renderframe=_ini.Get("config","renderframe").ToInt32(1);if(renderframe>0&&renderframe<60){
Specifics.skipFrames=renderframe;}int cost=_ini.Get("config","cost").ToInt32(0);if(cost>0){this.cost=cost;safetag=_ini.Get(
"config","safetag").ToString("safe");}Echo("Using tag of "+mytag);Echo("Using cost of "+cost);Echo("Using speed of "+maxFrames);
List<IMyTerminalBlock>Controllers=new List<IMyTerminalBlock>();GridTerminalSystem.GetBlocksOfType(Controllers,(
IMyTerminalBlock x)=>((x.CustomName!=null)&&(x.CustomName.ToUpper().IndexOf("["+mytag.ToUpper()+".SEAT]")>=0)&&(x is IMyShipController))
);Echo("Found "+Controllers.Count+" controllers");if(Controllers.Count>0){foreach(var thisblock in Controllers){jdbg.
Debug("- "+thisblock.CustomName);}if(Controllers.Count>1){notOkToPlay="ERROR: Too many controllers";Echo(notOkToPlay);return;
}controller=(IMyShipController)Controllers[0];}else if(Controllers.Count==0){notOkToPlay=
"ERROR: No controllers tagged as ["+mytag+".SEAT]";Echo(notOkToPlay);return;}if(cost>0){List<IMyTerminalBlock>CoinIn=new List<IMyTerminalBlock>();
GridTerminalSystem.GetBlocksOfType(CoinIn,(IMyTerminalBlock x)=>((x.CustomName!=null)&&(x.CustomName.ToUpper().IndexOf("["+mytag.ToUpper()
+".COINS]")>=0)&&(x.HasInventory)));Echo("Found "+CoinIn.Count+" coin in");if(CoinIn.Count!=1){notOkToPlay=
"ERROR: Game has space credit cost but no cargo container tagged ["+mytag+".COINS]";Echo(notOkToPlay);return;}else{coinInInv=CoinIn[0].GetInventory(0);}List<IMyTerminalBlock>CoinOut=new
List<IMyTerminalBlock>();GridTerminalSystem.GetBlocksOfType(CoinOut,(IMyTerminalBlock x)=>((x.CustomName!=null)&&(x.
CustomName.ToUpper().IndexOf("["+safetag.ToUpper()+"]")>=0)&&(x.HasInventory)));Echo("Found "+CoinOut.Count+" coin out");if(
CoinOut.Count!=1){notOkToPlay=
"ERROR: Game has space credit cost but nowhere to move credit to - Needs a cargo container tagged ["+safetag+"]";Echo(notOkToPlay);return;}else{coinOutInv=CoinOut[0].GetInventory(0);}if(!(coinInInv.IsConnectedTo(
coinOutInv)&&coinInInv.CanTransferItemTo(coinOutInv,new MyItemType("MyObjectBuilder_PhysicalObject","SpaceCredit")))){notOkToPlay=
"ERROR: No connection/conveyor system between "+CoinIn[0].CustomName+" and "+CoinOut[0].CustomName;Echo(notOkToPlay);return;}Echo(
"All set up as arcade machine with cost of "+cost);}displays=jlcd.GetLCDsWithTag(mytag+".SCREEN");Echo("Found "+displays.Count+" displays");jlcd.SetupFontCustom(
displays,Display.HEIGHT,Display.WIDTH,true,0.001F,0.001F);jlcd.InitializeLCDs(displays,TextAlignment.LEFT);fpsdisplays=jlcd.
GetLCDsWithTag(mytag+".FPS");jlcd.InitializeLCDs(fpsdisplays,TextAlignment.CENTER);jlcd.SetupFontCustom(fpsdisplays,1,8,false,0.25F,
0.25F);Echo("Found "+fpsdisplays.Count+" fpsdisplays");namedisplays=jlcd.GetLCDsWithTag(mytag+".NAME");jlcd.InitializeLCDs(
namedisplays,TextAlignment.CENTER);jlcd.SetupFontCustom(namedisplays,1,16,false,0.25F,0.25F);Echo("Found "+namedisplays.Count+
" namedisplays");Runtime.UpdateFrequency=UpdateFrequency.Update1;}void Save(){}void Main(string argument,UpdateType updateSource){if(
argument==null){jdbg.Echo("Launched with empty parms"+argument);}else{jdbg.Echo("Launched with parms '"+argument+"'");}String
errorMessage=null;if(notOkToPlay!=null){errorMessage=notOkToPlay;}else if(argument==null||argument.Equals("")){if(currentGame==
GetRomData.Games.None){errorMessage="ERROR: No game configured - Please see instructions";}}else{List<IMyTerminalBlock>gameData=
new List<IMyTerminalBlock>();GridTerminalSystem.GetBlocksOfType(gameData,(IMyTerminalBlock x)=>((x.CustomName!=null)&&(x.
CustomName.ToUpper().IndexOf("["+mytag.ToUpper()+"]")>=0)&&(x.CustomName.ToUpper().IndexOf(argument.ToUpper())>=0)));jdbg.Debug(
"Found "+gameData.Count+" game block with contents  ["+mytag+"] "+argument);if(gameData.Count!=1){errorMessage=
"Could not find block with name '["+mytag+"] "+argument+"')";}else{String gameCode;gameCode=gameData[0].CustomData;argument=argument.ToLower().Replace(
".zip","");if(Enum.TryParse(argument,true,out currentGame)){jdbg.Debug("Recognized as "+currentGame);}else{errorMessage=
"ERROR: Invalid parameter: "+argument;}if(!GetRomData.checkRomData(currentGame,gameCode)){errorMessage=
"ERROR: Data does not match expected for that game "+currentGame;}if(errorMessage==null){jlcd.WriteToAllLCDs(namedisplays,""+currentGame.ToString().ToUpperInvariant(),false
);jdbg.Debug("Creating CPU");Specifics specs=new Specifics();specs.mypgm=this;specs.controller=controller;specs.jdbg=jdbg
;specs.jctrl=new JCTRL(this,jdbg,false);si=new Arcade8080Machine(specs,currentGame,gameCode,cost);Runtime.UpdateFrequency
=UpdateFrequency.Update1;}}}if(errorMessage!=null){Echo(errorMessage);jlcd.WriteToAllLCDs(displays,errorMessage,false);
Runtime.UpdateFrequency=UpdateFrequency.None;return;}int now=DateTime.UtcNow.Second;if(now!=curSec){lastFrames=curFrames;if(
Specifics.skipFrames>1){jlcd.WriteToAllLCDs(fpsdisplays,""+actFrames+" of "+lastFrames,false);}else{jlcd.WriteToAllLCDs(
fpsdisplays,""+lastFrames,false);}curSec=now;curFrames=0;actFrames=0;}jdbg.Echo("Frame "+Frame+++", fps: "+lastFrames+", state:"+si
.State);if((cost>0)&&(Frame>50)&&(si!=null)){DateTime rightNow=DateTime.UtcNow;if((inserted!=DateTime.MinValue)&&((
rightNow-inserted).TotalSeconds>=1)){jdbg.Debug("Released coin button");inserted=DateTime.MinValue;si.insertCoin(false);
lastCoinCheck=rightNow;}else if((rightNow-lastCoinCheck).TotalSeconds>=2){itemList.Clear();coinInInv.GetItems(itemList,b=>(b.Type.
ToString().Contains("SpaceCredit")&&b.Amount>=cost));if(itemList.Count>0){jdbg.Debug(
"Found Coins - Moving and pressing coin button");coinInInv.TransferItemTo(coinOutInv,itemList[0],cost);jdbg.Debug("Moved & press coin button");inserted=rightNow;si.
insertCoin(true);}lastCoinCheck=rightNow;}}try{si.part_exe(ref curFrames,ref actFrames);}catch(Exception ex){jdbg.DebugAndEcho(
"Exception: "+ex.ToString());Echo("Exception: "+ex.ToString());throw ex;}}class Arcade8080Machine{Cpu cpu;Memory memory;Display
display;Bus iobus;public byte[]keyBits;public Specifics specifics;bool needsProcessing=false;int processIndex=0;public int cost
=0;public bool showStates=false;public Arcade8080Machine(Specifics specs,GetRomData.Games gameType,String gameData,int
cost){specifics=specs;specs.am=this;int rotate270=270;memory=new Memory();byte backCol=0;Display.paletteType palType=Display
.paletteType.RBG;byte port_shift_result=0xFF;byte port_shift_data=0xFF;byte port_shift_offset=0xFF;byte port_input=0xFF;
this.cost=cost;memory.LoadRom(ref keyBits,ref rotate270,gameType,gameData,ref backCol,ref needsProcessing,ref palType,ref
port_shift_result,ref port_shift_data,ref port_shift_offset,ref port_input);iobus=new Bus(keyBits,port_shift_result,port_shift_data,
port_shift_offset,port_input);cpu=new Cpu(memory,iobus);display=new Display(memory,specifics,this);display.palType=palType;display.rotate
=rotate270;Display.backGroundCol=backCol;specifics.display=display;DirectBitmap.Palette=null;State=-3;}public int State=-
3;public void part_exe(ref int curFrames,ref int actFrames){if(State==-3){if(needsProcessing){processIndex=0;State=-2;}
else{State=-1;}}if(State==-2){if(GetRomData.processRom(ref processIndex,ref memory.allProms,specifics,Memory.game)){cpu.
memory=memory.allProms[0];State=-1;}else{return;}}specifics.CheckKeys();bool seenBegin=false;while(true){if(State>=2){if(
specifics.GetInstructionCount()<Specifics.maxFrames){bool finished=specifics.drawAndRenderFrame(ref State,ref actFrames);if(
finished){curFrames++;State=-1;if(showStates)specifics.Echo("Moved to state "+State+" - "+specifics.GetInstructionCount());}else
{return;}}else{return;}}if(State==-1){if(seenBegin)return;seenBegin=true;State=0;if(showStates)specifics.Echo(
"Moved to state "+State+" - "+specifics.GetInstructionCount());}while((cpu.cycles<16666)&&(specifics.GetInstructionCount()<Specifics.
maxFrames)){cpu.exe();}if(specifics.GetInstructionCount()>=Specifics.maxFrames)return;if(cpu.cycles>=16666){cpu.cycles=0;if(State
==0){cpu.handleInterrupt(1);State=1;if(showStates)specifics.Echo("Moved to state "+State+" - "+specifics.
GetInstructionCount());}else{cpu.handleInterrupt(2);State=2;if(showStates)specifics.Echo("Moved to state "+State+" - "+specifics.
GetInstructionCount());}}}}public void insertCoin(bool pushed){byte whichBit=keyBits[(int)Program.GetRomData.KeyIndex.q];specifics.
DebugAndEcho("Coin inserted: "+whichBit+"/"+pushed+" at "+DateTime.Now);if((keyBits[(int)Program.GetRomData.KeyIndex.initmask]&
whichBit)>0){if(!pushed){iobus.input|=whichBit;}else{iobus.input&=(byte)~whichBit;}}else{if(pushed){iobus.input|=whichBit;}else{
iobus.input&=(byte)~whichBit;}}}public void handleInput(byte b,Boolean pushed){if((keyBits[(int)Program.GetRomData.KeyIndex.
initmask]&b)>0){if(!pushed){iobus.input|=b;}else{iobus.input&=(byte)~b;}}else{if(pushed){iobus.input|=b;}else{iobus.input&=(byte
)~b;}}}}class Bus{short shift;byte offset;public Cpu cpu;bool testMode=false;public bool test_finished=false;byte
port_shift_result=0xFF;byte port_shift_data=0xFF;byte port_shift_offset=0xFF;byte port_input=0xFF;private bool BIT(int x,int n){return((x
>>n)&0x01)>0;}public Bus(byte[]keyBits,byte port_shift_result,byte port_shift_data,byte port_shift_offset,byte port_input)
{if(keyBits==null){testMode=true;}else{input=keyBits[(int)Program.GetRomData.KeyIndex.initmask];}this.port_shift_result=
port_shift_result;this.port_shift_data=port_shift_data;this.port_shift_offset=port_shift_offset;this.port_input=port_input;}public byte
input{get;set;}byte lower3bitMask=0x07;public static bool rollingc_saved=false;public byte Read(byte b){if(testMode)return 0;
byte answer=0x00;if(b==port_input){answer=input;}else if(b==port_shift_result){answer=(byte)((shift>>offset)&0xff);}else{
switch(b){case 0x00:if(Memory.game==GetRomData.Games.galxwars){answer=0x40;}else if(Memory.game==GetRomData.Games.astropal){
answer=0x1;}else if(Memory.game==GetRomData.Games.rollingc){int newVal=(input&0x80);input=(byte)(input&0x7F);if(newVal>0)
rollingc_saved=!rollingc_saved;if(rollingc_saved)return 0x02;else return 0x04;}else if(Memory.game==GetRomData.Games.spacerng){answer=
0xf5;}else if(Memory.game==GetRomData.Games.lupin3){answer=0x03;}else if(Memory.game==GetRomData.Games.polaris){answer=(byte
)(input&0xf8);}else if(Memory.game==GetRomData.Games.indianbt){if(cpu.PC==0x5fec){answer=0x10;}else if(cpu.PC==0x5ffb){
answer=0x00;}}else if(Memory.game==GetRomData.Games.vortex){answer=0x80;}break;case 0x02:if(Memory.game==GetRomData.Games.
indianbt){answer=0x03;}else if(Memory.game==GetRomData.Games.vortex){answer=0xff;}else{answer=0;}break;case 0x03:if(Memory.game
==GetRomData.Games.astropal){answer=0x80;}break;}}return answer;}public void Write(byte b,byte A){if(b==port_shift_offset)
{offset=(byte)((~A)&lower3bitMask);}else if(b==port_shift_data){shift=(short)((shift>>8)|(((short)A)<<7));}else{switch(b)
{case 0x03:if((Memory.game==GetRomData.Games.ballbomb)||(Memory.game==GetRomData.Games.lrescue)||(Memory.game==GetRomData
.Games.spacerng)||(Memory.game==GetRomData.Games.schaser)||(Memory.game==GetRomData.Games.rollingc)){Display.isRed=(A&
0x04)>0;}break;case 0x04:if(Memory.game==GetRomData.Games.galxwars){Display.isRed=(A&0x04)>0;}break;case 0x05:if(Memory.game
==GetRomData.Games.schaser){Display.backgroundDisable=((A>>3)&1)!=0;Display.backgroundSelect=((A>>4)&1)!=0;}else if(Memory
.game==GetRomData.Games.invrvnge){Display.isRed=(A&0x10)>0;}else if(Memory.game==GetRomData.Games.lupin3){Display.
m_color_map=(A&0x40)>0;}else if(Memory.game==GetRomData.Games.galxwars||Memory.game==GetRomData.Games.rollingc||Memory.game==
GetRomData.Games.ballbomb){Display.m_color_map=BIT(A,5);}break;}}}}class DirectBitmap{public static char[]Pixels;public static
char[][]QuickPix;public static char[]Palette;private static bool lastRollingCGame;private Specifics specifics;public int
Height{get;private set;}public int Width{get;private set;}public int Memory_Width{get;private set;}public DirectBitmap(int
width,int height,Specifics spec,Display.paletteType pal){specifics=spec;Width=width;Memory_Width=width+2;Height=height;if(
Pixels==null){Pixels=new char[Memory_Width*Height];for(int i=0;i<Height;i++){Pixels[(i*Memory_Width)+Width]='\x0d';Pixels[(i*
Memory_Width)+(Width+1)]='\x0a';}}if(Palette==null||Bus.rollingc_saved!=lastRollingCGame){Palette=new char[16];lastRollingCGame=Bus.
rollingc_saved;specifics.mypgm.Echo("Building pallette again");if(Memory.game==GetRomData.Games.rollingc){for(int i=0;i<16;i++){int
intensity=128;if(Bus.rollingc_saved)intensity=255;if(i>7)intensity=255;if(!Bus.rollingc_saved&&i==5){Palette[i]=specifics.mypgm.
jlcd.ColorToChar(0xff,0x00,0x80);}else if(!Bus.rollingc_saved&&i==6){Palette[i]=specifics.mypgm.jlcd.ColorToChar(0xff,0x80,
0x00);}else{Palette[i]=specifics.mypgm.jlcd.ColorToChar(((i&4)>0)?(byte)intensity:(byte)0,((i&2)>0)?(byte)intensity:(byte)0,
((i&1)>0)?(byte)intensity:(byte)0);}}}else{for(int i=0;i<8;i++){if(pal==Display.paletteType.RBG){Palette[i]=specifics.
mypgm.jlcd.ColorToChar(((i&1)>0)?(byte)255:(byte)0,((i&4)>0)?(byte)255:(byte)0,((i&2)>0)?(byte)255:(byte)0);}else if(pal==
Display.paletteType.RGB){Palette[i]=specifics.mypgm.jlcd.ColorToChar(((i&1)>0)?(byte)255:(byte)0,((i&2)>0)?(byte)255:(byte)0,((
i&4)>0)?(byte)255:(byte)0);}else if(pal==Display.paletteType.MONO){if(i==0){Palette[i]=specifics.mypgm.jlcd.ColorToChar(
0x00,0x00,0x00);}else{Palette[i]=specifics.mypgm.jlcd.ColorToChar(0xff,0xff,0xff);}}}}QuickPix=new char[265][];for(int i=0;i
<256;i++){QuickPix[i]=new char[8];for(byte b=0;b<8;b++){if((i&(0x1<<b))!=0){QuickPix[i][b]=JLCD.COLOUR_WHITE;}else{
QuickPix[i][b]=JLCD.COLOUR_BLACK;}}}}}public void Set8PixelsBW(int x,int y,byte b){Array.Copy(QuickPix[b],0,Pixels,(y*
Memory_Width)+(x),8);}public void Set8PixelsCol(int x,int y,byte fore,byte back,byte data,bool isRed){int index=x+(y*Memory_Width);
char whichCol;for(int b=0;b<8;b++,index++){if((data&0x1)!=0){if(isRed){whichCol=JLCD.COLOUR_RED;}else{if(Memory.game==
GetRomData.Games.rollingc){whichCol=Palette[fore&0x0f];}else{whichCol=Palette[fore&0x07];}}}else{whichCol=Palette[back];}Pixels[
index]=whichCol;data=(byte)(data>>1);}}public void SetPixel(int x,int y,byte fore,byte back,byte data,int bit,bool isRed){int
index=x+(y*Memory_Width);char whichCol;if((data&(0x1<<bit))!=0){if(isRed){whichCol=JLCD.COLOUR_RED;}else{whichCol=Palette[
fore];}}else{whichCol=Palette[back];}Pixels[index]=whichCol;}static int oldRotate=-1;public void FinishScreen(int rotate){if
(rotate!=oldRotate){specifics.setRotation(rotate);oldRotate=rotate;}}}class Display{public enum paletteType{MONO,RGB,RBG,
CUSTOM};public static bool isRed;public static bool backgroundDisable;public static bool backgroundSelect;public int rotate=
270;public static byte backGroundCol=0;public static bool m_color_map=false;private bool useOptimizations=false;public
paletteType palType;public static bool oldIsRed=false;public const int WIDTH=224;public const int HEIGHT=256;private const ushort
videoRamStart=0x2400;private const ushort videoRamEnd=0x4000;private const ushort MW8080BW_VCOUNTER_START_NO_VBLANK=0x20;private
Memory memory;private Specifics specifics;private Arcade8080Machine am=null;public Display(Memory memory,Specifics specs,
Arcade8080Machine mach){this.memory=memory;this.specifics=specs;this.am=mach;isRed=false;backgroundDisable=false;backgroundSelect=false;
backGroundCol=0;m_color_map=false;oldIsRed=false;if(Memory.game==GetRomData.Games.schaser){useOptimizations=false;}else{
useOptimizations=true;}}public DirectBitmap Screen=null;int screenPosn;int[]prevMem=null;int[]curMem=null;int[]curCol=null;int[]prevCol=
null;int[]curCol2=null;int[]prevCol2=null;public bool generateFrameToDisplay(ref int State){if(State==2){Screen=new
DirectBitmap(256,224,specifics,palType);screenPosn=0;curMem=new int[(videoRamEnd-videoRamStart)/sizeof(int)];Buffer.BlockCopy(memory
.allProms[0],videoRamStart,curMem,0,(videoRamEnd-videoRamStart));if(Memory.game==GetRomData.Games.schaser){int blockSize=
0x1C00;curCol=new int[blockSize/sizeof(int)];Buffer.BlockCopy(memory.allProms[0],0xc400,curCol,0,blockSize);}if(Memory.game==
GetRomData.Games.rollingc){int blockSize=0x1C00;curCol=new int[blockSize/sizeof(int)];Buffer.BlockCopy(memory.allProms[0],0xa400,
curCol,0,blockSize);curCol2=new int[blockSize/sizeof(int)];Buffer.BlockCopy(memory.allProms[0],0xe400,curCol2,0,blockSize);}
State=3;if(am.showStates)specifics.Echo("Moved to state "+State);screenPosn=0;if(memory.allProms[1]==null)useOptimizations=
true;}byte[]mem=memory.allProms[0];byte[]col=memory.allProms[1];byte[]extramem=memory.allProms[2];for(;screenPosn<((
videoRamEnd-videoRamStart)/sizeof(int));screenPosn++){if(specifics.GetInstructionCount()>=Specifics.maxFrames)return false;int y=(
screenPosn*sizeof(int))/32;bool redrawthisbyte=false;if(prevMem==null){redrawthisbyte=true;}else if(oldIsRed!=isRed){
redrawthisbyte=true;}else if(Memory.game==GetRomData.Games.rollingc){int coloffs=((((y>>2)<<7)|((screenPosn*sizeof(int))&0x1f)))/
sizeof(int);if((curCol[coloffs]!=prevCol[coloffs])||(curCol2[coloffs]!=prevCol2[coloffs])||(curMem[screenPosn]!=prevMem[
screenPosn])){redrawthisbyte=true;}}else if(Memory.game==GetRomData.Games.schaser){int foreOffs=((((y>>2)<<7)|((screenPosn*sizeof(
int))&0x1f)))/sizeof(int);if((curCol[foreOffs]!=prevCol[foreOffs])||(curMem[screenPosn]!=prevMem[screenPosn])){
redrawthisbyte=true;}}else if(useOptimizations&&(curMem[screenPosn]!=prevMem[screenPosn])){redrawthisbyte=true;}if(redrawthisbyte){for
(int j=0;j<sizeof(int);j++){if(palType==paletteType.MONO){Screen.Set8PixelsBW((((sizeof(int)*screenPosn)+j)%32)*8,y,mem[(
(sizeof(int)*screenPosn)+j)+videoRamStart]);continue;}else{int screenByte=(screenPosn*sizeof(int))+j;byte data=mem[
screenByte+videoRamStart];int colIdx=(screenByte>>8<<5)|(screenByte&0x1f);byte foreColour;byte backColour;byte bgr;if(col==null){
bgr=0xff;}else{int color_map_base=m_color_map?0x0480:0x0080;bgr=col[color_map_base+colIdx];}if(Memory.game==GetRomData.
Games.rollingc){foreColour=(byte)(mem[0xa400+(screenByte&0x1f00)|(screenByte&0x1f)]&0x0f);backColour=(byte)(mem[0xe400+(
screenByte&0x1f00)|(screenByte&0x1f)]&0x0f);}else if(Memory.game==GetRomData.Games.vortex){bool coldata=(mem[((screenByte+1)&
0x1fff)+videoRamStart]&0x01)>0;foreColour=(byte)((coldata?0x00:0x01)|(coldata?0x02:0x00)|(((8*(screenByte%32))&0x20)>0?0x04:
0x00));backColour=0;}else if(Memory.game==GetRomData.Games.schaser){foreColour=(byte)(mem[0xc400+(((y>>2)<<7)|(screenByte&
0x1f))]&0x07);if(Display.backgroundDisable){backColour=0;}else{backColour=(byte)(((((bgr&0x0c)==0x0c)&&Display.
backgroundSelect))?4:2);}}else if(Memory.game==GetRomData.Games.polaris){foreColour=(byte)(~((mem[0xc400+(((y>>2)<<7)|(screenByte&0x1f))
])&0x07));backColour=(byte)((((bgr&0x01)==0x01))?6:2);}else{backColour=backGroundCol;foreColour=(byte)(bgr&0x07);}Screen.
Set8PixelsCol(((screenByte%32)*8),y,foreColour,backColour,data,isRed);}}}}prevMem=curMem;prevCol=curCol;prevCol2=curCol2;oldIsRed=
isRed;Screen.FinishScreen(rotate);return true;}}class GetRomData{public enum Games{None,ballbomb,invaders,lrescue,spacerng,
vortex,invrvnge,galxwars,rollingc,schaser,lupin3,polaris,indianbt,astropal,galactic,attackfc,};public static String getRomData
(Games game){return"";}private static String start_ballbomb="AAAAwxgAAA";private static String start_invaders=
"IMPJFiGEIH";private static String start_lrescue="Dw8PDw8PDw";private static String start_spacerng="CAgICAgICA";private static
String start_vortex="wicZKqd9Ks";private static String start_invrvnge="//////////";private static String start_galxwars=
"CAgICAgICA";private static String start_rollingc="88MAQNQYAA";private static String start_schaser="Dw8PDw8PDw";private static
String start_lupin3="AAAAw0AAAA";private static String start_polaris="AAAAw8YFAA";private static String start_indianbt=
"AAAAw+VfKw";private static String start_astropal="MaAglwDDtg";private static String start_galactic="AAAAw5sA//";private static
String start_attackfc="PgAyKyLDAA";public static bool checkRomData(Games game,String gameData){String expected="";switch(game)
{case Games.ballbomb:expected=start_ballbomb;break;case Games.invaders:expected=start_invaders;break;case Games.lrescue:
expected=start_lrescue;break;case Games.spacerng:expected=start_spacerng;break;case Games.vortex:expected=start_vortex;break;
case Games.invrvnge:expected=start_invrvnge;break;case Games.galxwars:expected=start_galxwars;break;case Games.rollingc:
expected=start_rollingc;break;case Games.schaser:expected=start_schaser;break;case Games.lupin3:expected=start_lupin3;break;case
Games.polaris:expected=start_polaris;break;case Games.indianbt:expected=start_indianbt;break;case Games.astropal:expected=
start_astropal;break;case Games.galactic:expected=start_galactic;break;case Games.attackfc:expected=start_attackfc;break;default:throw
new Exception("Unexpected game: "+game);}return(expected.Equals(gameData.Substring(0,10)));}public static byte[][]
getRomData(Games gameType,String gameBytes,ref byte[]keyports,ref int rotate,ref byte backgroundCol,ref bool needsProcessing,ref
Display.paletteType palType,ref byte port_shift_result,ref byte port_shift_data,ref byte port_shift_offset,ref byte port_input)
{byte[][]roms=new byte[3][];byte[]rom=Convert.FromBase64String(gameBytes);int curPos=0;byte[]main=new byte[0x10000];byte[
]colour=new byte[2048];byte[]extra=new byte[2048];rotate=270;backgroundCol=0;if(gameType==Games.invaders){loadrom(rom,ref
curPos,ref main,0x1800,0x0800);loadrom(rom,ref curPos,ref main,0x1000,0x0800);loadrom(rom,ref curPos,ref main,0x0800,0x0800);
loadrom(rom,ref curPos,ref main,0x0000,0x0800);roms[0]=main;palType=Display.paletteType.MONO;port_shift_data=0x04;
port_shift_offset=0x02;port_input=0x01;port_shift_result=0x03;}else if(gameType==Games.ballbomb){loadrom(rom,ref curPos,ref main,0x0000,
0x0800*4);loadrom(rom,ref curPos,ref main,0x4000,0x0800);loadrom(rom,ref curPos,ref colour,0x0000,0x0800);roms[0]=main;roms[1]
=colour;backgroundCol=2;palType=Display.paletteType.RBG;port_shift_data=0x04;port_shift_offset=0x02;port_input=0x01;
port_shift_result=0x03;}else if(gameType==Games.lrescue){loadrom(rom,ref curPos,ref colour,0x0000,0x0400);curPos-=0x400;loadrom(rom,ref
curPos,ref colour,0x0400,0x0400);loadrom(rom,ref curPos,ref main,0x0000,0x0800*4);loadrom(rom,ref curPos,ref main,0x4000,
0x0800*2);roms[0]=main;roms[1]=colour;palType=Display.paletteType.RBG;port_shift_data=0x04;port_shift_offset=0x02;port_input=
0x01;port_shift_result=0x03;}else if(gameType==Games.schaser){loadrom(rom,ref curPos,ref colour,0x0000,0x0400);loadrom(rom,
ref curPos,ref main,0x0000,0x0400*8);loadrom(rom,ref curPos,ref main,0x4000,0x0400*2);roms[0]=main;roms[1]=colour;palType=
Display.paletteType.RBG;port_shift_data=0x04;port_shift_offset=0x02;port_input=0x01;port_shift_result=0x03;}else if(gameType==
Games.invrvnge){loadrom(rom,ref curPos,ref colour,0x0000,0x0800);loadrom(rom,ref curPos,ref main,0x1800,0x0800);loadrom(rom,
ref curPos,ref main,0x1000,0x0800);loadrom(rom,ref curPos,ref main,0x0800,0x0800);loadrom(rom,ref curPos,ref main,0x0000,
0x0800);roms[0]=main;roms[1]=colour;palType=Display.paletteType.RBG;port_shift_data=0x04;port_shift_offset=0x02;port_input=
0x01;port_shift_result=0x03;}else if(gameType==Games.attackfc){loadrom(rom,ref curPos,ref main,0x0000,0x0400);loadrom(rom,
ref curPos,ref main,0x0800,0x0400);loadrom(rom,ref curPos,ref main,0x1000,0x0400);loadrom(rom,ref curPos,ref main,0x1800,
0x0400);loadrom(rom,ref curPos,ref main,0x0400,0x0400);loadrom(rom,ref curPos,ref main,0x0c00,0x0400);loadrom(rom,ref curPos,
ref main,0x1c00,0x0400);rotate=0;roms[0]=main;needsProcessing=true;palType=Display.paletteType.MONO;port_shift_data=0x03;
port_shift_offset=0x07;port_input=0x00;port_shift_result=0x03;}else if(gameType==Games.spacerng){loadrom(rom,ref curPos,ref colour,0x0000
,0x0400*2);loadrom(rom,ref curPos,ref main,0x0000,0x0800*4);roms[0]=main;roms[1]=colour;rotate=90;palType=Display.
paletteType.RBG;port_shift_data=0x04;port_shift_offset=0x02;port_input=0x01;port_shift_result=0x03;}else if(gameType==Games.
indianbt){loadrom(rom,ref curPos,ref main,0x0000,0x0800*4);loadrom(rom,ref curPos,ref main,0x4000,0x0800*4);loadrom(rom,ref
curPos,ref colour,0x0000,0x0400*2);roms[0]=main;roms[1]=colour;palType=Display.paletteType.RGB;port_shift_data=0x04;
port_shift_offset=0x02;port_input=0x01;port_shift_result=0x03;}else if(gameType==Games.astropal){loadrom(rom,ref curPos,ref main,0x0000,
0x0400*8);roms[0]=main;rotate=0;palType=Display.paletteType.MONO;port_shift_data=0x04;port_shift_offset=0x02;port_input=0x01;
port_shift_result=0x03;}else if(gameType==Games.galxwars){loadrom(rom,ref curPos,ref colour,0x0000,0x0400*2);loadrom(rom,ref curPos,ref
main,0x4000,0x0400*2);loadrom(rom,ref curPos,ref main,0x0000,0x0400*4);roms[0]=main;roms[1]=colour;palType=Display.
paletteType.RBG;port_shift_data=0x04;port_shift_offset=0x02;port_input=0x01;port_shift_result=0x03;}else if(gameType==Games.lupin3)
{loadrom(rom,ref curPos,ref main,0x0000,0x0800*4);loadrom(rom,ref curPos,ref main,0x4000,0x0800*2);loadrom(rom,ref curPos
,ref colour,0x0000,0x0400*2);roms[0]=main;roms[1]=colour;palType=Display.paletteType.RGB;port_shift_data=0x04;
port_shift_offset=0x02;port_input=0x01;port_shift_result=0x03;}else if(gameType==Games.galactic){loadrom(rom,ref curPos,ref main,0x0000,
0x0800*4);loadrom(rom,ref curPos,ref main,0x4000,0x0800*3);roms[0]=main;palType=Display.paletteType.MONO;port_shift_data=0x04;
port_shift_offset=0x02;port_input=0x01;port_shift_result=0x03;}else if(gameType==Games.rollingc){loadrom(rom,ref curPos,ref main,0x0000,
0x0400*8);loadrom(rom,ref curPos,ref main,0x4000,0x0800*4);roms[0]=main;palType=Display.paletteType.CUSTOM;port_shift_data=
0x04;port_shift_offset=0x02;port_input=0x01;port_shift_result=0x03;}else if(gameType==Games.polaris){loadrom(rom,ref curPos,
ref main,0x0000,0x0800);loadrom(rom,ref curPos,ref main,0x1000,0x0800);loadrom(rom,ref curPos,ref main,0x1800,0x0800);
loadrom(rom,ref curPos,ref main,0x4000,0x0800);loadrom(rom,ref curPos,ref extra,0x0000,0x0100);loadrom(rom,ref curPos,ref
colour,0x0000,0x0400);loadrom(rom,ref curPos,ref main,0x0800,0x0800);loadrom(rom,ref curPos,ref main,0x4800,0x0800);loadrom(
rom,ref curPos,ref main,0x5000,0x0800);roms[0]=main;roms[1]=colour;roms[2]=extra;palType=Display.paletteType.RBG;
port_shift_data=0x03;port_shift_offset=0x00;port_input=0x01;port_shift_result=0x03;}else if(gameType==Games.vortex){loadrom(rom,ref
curPos,ref main,0x0000,0x0800*4);loadrom(rom,ref curPos,ref main,0x4000,0x0800*2);needsProcessing=true;roms[0]=main;palType=
Display.paletteType.RGB;port_shift_data=0x06;port_shift_offset=0x00;port_input=0x03;port_shift_result=0x01;}else{return null;}
keyports=getKeyBits(gameType);return roms;}private static byte[]newmain;public static bool processRom(ref int index,ref byte[][]
roms,Specifics specifics,Games gameType){if(gameType==Games.attackfc){if(index==0){newmain=new byte[roms[0].Length];}int[]
reorder=new int[]{15,14,13,12,11,10,8,9,7,6,5,4,3,2,1,0};for(;index<(Math.Min(roms[0].Length,0xffff));index++){if(specifics.
GetInstructionCount()>=Specifics.maxFrames)return false;int newval=ReorderAsShort(index,reorder);newmain[newval]=roms[0][index];}roms[0]=
newmain;return true;}else if(gameType==Games.vortex){if(index==0){newmain=new byte[roms[0].Length];}for(;index<roms[0].Length;
index++){if(specifics.GetInstructionCount()>=Specifics.maxFrames)return false;int addr=index;switch(index&0xE000){case 0x0000
:case 0x2000:addr^=0x0209;break;case 0x4000:addr^=0x0209;break;case 0x6000:case 0x8000:case 0xa000:case 0xc000:case
0xe000:addr^=0x0208;break;}newmain[addr]=roms[0][index];}roms[0]=newmain;return true;}throw new Exception("Unexpected game: "+
gameType);}public static int ReorderAsShort(int a,int[]bits){int b=0;for(int i=0;i<16;i++){if((a&(0x01<<i))>0){b|=(0x01<<bits[15
-i]);}}return b;}private static void loadrom(byte[]rom,ref int curPos,ref byte[]mem,int Start,int Len){Array.Copy(rom,
curPos,mem,Start,Len);curPos+=Len;}public enum KeyIndex{keyup=1,keydown=2,keyleft=3,keyright=4,space=5,crouch=6,q=7,e=8,
initmask=9};private static byte[]getKeyBits(Games gameType){byte[]indexes=new byte[16];switch(gameType){case Games.astropal:case
Games.indianbt:indexes[(int)KeyIndex.q]=0x01;indexes[(int)KeyIndex.e]=0x04;indexes[(int)KeyIndex.crouch]=0x08;indexes[(int)
KeyIndex.space]=0x10;indexes[(int)KeyIndex.keyleft]=0x20;indexes[(int)KeyIndex.keyright]=0x40;indexes[(int)KeyIndex.initmask]=
0x81;break;case Games.schaser:indexes[(int)KeyIndex.keyup]=0x01;indexes[(int)KeyIndex.keyleft]=0x02;indexes[(int)KeyIndex.
keydown]=0x04;indexes[(int)KeyIndex.keyright]=0x08;indexes[(int)KeyIndex.space]=0x10;indexes[(int)KeyIndex.e]=0x40;indexes[(int
)KeyIndex.q]=0x80;break;case Games.lupin3:indexes[(int)KeyIndex.q]=0x01;indexes[(int)KeyIndex.e]=0x04;indexes[(int)
KeyIndex.space]=0x08;indexes[(int)KeyIndex.keyright]=0x10;indexes[(int)KeyIndex.keydown]=0x20;indexes[(int)KeyIndex.keyleft]=
0x40;indexes[(int)KeyIndex.keyup]=0x80;break;case Games.attackfc:indexes[(int)KeyIndex.keyright]=0x01;indexes[(int)KeyIndex.
keyleft]=0x02;indexes[(int)KeyIndex.space]=0x04;indexes[(int)KeyIndex.q]=0x80;indexes[(int)KeyIndex.initmask]=0xFF;break;case
Games.galxwars:indexes[(int)KeyIndex.q]=0x01;indexes[(int)KeyIndex.e]=0x04;indexes[(int)KeyIndex.space]=0x10;indexes[(int)
KeyIndex.keyleft]=0x20;indexes[(int)KeyIndex.keyright]=0x40;indexes[(int)KeyIndex.initmask]=0x89;break;case Games.polaris:
indexes[(int)KeyIndex.q]=0x01;indexes[(int)KeyIndex.e]=0x04;indexes[(int)KeyIndex.space]=0x08;indexes[(int)KeyIndex.keyright]=
0x10;indexes[(int)KeyIndex.keydown]=0x20;indexes[(int)KeyIndex.keyleft]=0x40;indexes[(int)KeyIndex.keyup]=0x80;indexes[(int)
KeyIndex.initmask]=0x01;break;case Games.rollingc:indexes[(int)KeyIndex.q]=0x01;indexes[(int)KeyIndex.e]=0x04;indexes[(int)
KeyIndex.space]=0x10;indexes[(int)KeyIndex.crouch]=0x80;indexes[(int)KeyIndex.keyleft]=0x20;indexes[(int)KeyIndex.keyright]=0x40
;break;case Games.galactic:indexes[(int)KeyIndex.q]=0x01;indexes[(int)KeyIndex.e]=0x04;indexes[(int)KeyIndex.space]=0x10;
indexes[(int)KeyIndex.keyleft]=0x20;indexes[(int)KeyIndex.keyright]=0x40;indexes[(int)KeyIndex.initmask]=0x01;break;case Games.
vortex:indexes[(int)KeyIndex.q]=0x01;indexes[(int)KeyIndex.e]=0x04;indexes[(int)KeyIndex.space]=0x10;indexes[(int)KeyIndex.
keyleft]=0x20;indexes[(int)KeyIndex.keyright]=0x40;indexes[(int)KeyIndex.initmask]=0x81;break;default:indexes[(int)KeyIndex.q]=
0x01;indexes[(int)KeyIndex.e]=0x04;indexes[(int)KeyIndex.space]=0x10;indexes[(int)KeyIndex.keyleft]=0x20;indexes[(int)
KeyIndex.keyright]=0x40;break;}return indexes;}}class Cpu{private Bus iobus;public ushort PC;public ushort SP;private bool
interruptPin;public byte[]memory;public short ro_mem=0x2000;public int _2Mhz=2000000;public long cycles{get;set;}private int[]
cyclesValue={4,10,7,5,5,5,7,4,4,10,7,5,5,5,7,4,4,10,7,5,5,5,7,4,4,10,7,5,5,5,7,4,4,10,16,5,5,5,7,4,4,10,16,5,5,5,7,4,4,10,13,5,10,
10,10,4,4,10,13,5,5,5,7,4,5,5,5,5,5,5,7,5,5,5,5,5,5,5,7,5,5,5,5,5,5,5,7,5,5,5,5,5,5,5,7,5,5,5,5,5,5,5,7,5,5,5,5,5,5,5,7,5,
7,7,7,7,7,7,7,7,5,5,5,5,5,5,7,5,4,4,4,4,4,4,7,4,4,4,4,4,4,4,7,4,4,4,4,4,4,4,7,4,4,4,4,4,4,4,7,4,4,4,4,4,4,4,7,4,4,4,4,4,4
,4,7,4,4,4,4,4,4,4,7,4,4,4,4,4,4,4,7,4,5,10,10,10,11,11,7,11,5,10,10,10,11,17,7,11,5,10,10,10,11,11,7,11,5,10,10,10,11,17
,7,11,5,10,10,18,11,11,7,11,5,5,10,4,11,17,7,11,5,10,10,4,11,11,7,11,5,5,10,4,11,17,7,11,};public byte A,B,C,D,E,F,H,L;
public ushort AF{get{return combineRegisters(A,F);}set{A=(byte)(value>>8);F=(byte)(value&0xD7|0x2);}}public ushort BC{get{
return combineRegisters(B,C);}set{B=(byte)(value>>8);C=(byte)value;}}public ushort DE{get{return combineRegisters(D,E);}set{D=
(byte)(value>>8);E=(byte)value;}}public ushort HL{get{return combineRegisters(H,L);}set{H=(byte)(value>>8);L=(byte)value;
}}public bool FlagS{get{return(F&0x80)!=0;}set{F=value?(byte)(F|0x80):(byte)(F&~0x80);}}public bool FlagZ{get{return(F&
0x40)!=0;}set{F=value?(byte)(F|0x40):(byte)(F&~0x40);}}public bool FlagAC{get{return(F&0x10)!=0;}set{F=value?(byte)(F|0x10):
(byte)(F&~0x10);}}public bool FlagP{get{return(F&0x4)!=0;}set{F=value?(byte)(F|0x4):(byte)(F&~0x4);}}public bool
Flag0x2_1{get{return(F&0x2)!=0;}set{F=value?(byte)(F|0x2):(byte)(F&~0x2);}}public bool FlagC{get{return(F&0x1)!=0;}set{F=value?(
byte)(F|0x1):(byte)(F&~0x1);}}public byte M{get{return memory[HL];}set{if(HL>=ro_mem)memory[HL]=value;}}public byte Data8{
get{return memory[PC];}}public ushort Data16{get{return BitConverter.ToUInt16(memory,PC);}}public Cpu(Memory memory,Bus
iobus){this.memory=memory.allProms[0];this.iobus=iobus;iobus.cpu=this;Flag0x2_1=true;}public void exe(){byte opcode=memory[PC
++];cycles+=cyclesValue[opcode];switch(opcode){case 0x00:break;case 0x01:BC=Data16;PC+=2;break;case 0x02:memory[BC]=A;
break;case 0x03:BC+=1;break;case 0x04:B=INR(B);break;case 0x05:B=DCR(B);break;case 0x06:B=Data8;PC+=1;break;case 0x07:FlagC=(
(A&0x80)!=0);A=(byte)((A<<1)|(A>>7));break;case 0x08:break;case 0x09:DAD(BC);break;case 0x0A:A=memory[BC];break;case 0x0B
:BC-=1;break;case 0x0C:C=INR(C);break;case 0x0D:C=DCR(C);break;case 0x0E:C=Data8;PC+=1;;break;case 0x0F:FlagC=((A&0x1)!=0
);A=(byte)((A>>1)|(A<<7));break;case 0x10:break;case 0x11:DE=Data16;PC+=2;;break;case 0x12:memory[DE]=A;break;case 0x13:
DE+=1;break;case 0x14:D=INR(D);break;case 0x15:D=DCR(D);break;case 0x16:D=Data8;PC+=1;;break;case 0x17:bool prevC=FlagC;
FlagC=((A&0x80)!=0);A=(byte)((A<<1)|(prevC?1:0));break;case 0x18:break;case 0x19:DAD(DE);break;case 0x1A:A=memory[DE];break;
case 0x1B:DE-=1;break;case 0x1C:E=INR(E);break;case 0x1D:E=DCR(E);break;case 0x1E:E=Data8;PC+=1;;break;case 0x1F:bool preC=
FlagC;FlagC=((A&0x1)!=0);A=(byte)((A>>1)|(preC?0x80:0));break;case 0x20:break;case 0x21:HL=Data16;PC+=2;;break;case 0x22:
memory[Data16]=L;memory[Data16+1]=H;PC+=2;;break;case 0x23:HL+=1;break;case 0x24:H=INR(H);break;case 0x25:H=DCR(H);break;case
0x26:H=Data8;PC+=1;;break;case 0x27:int daa=A;if((daa&0x0F)>0x9||FlagAC){FlagAC=(((daa&0x0F)+0x06)&0xF0)!=0;daa+=0x06;if((
daa&0xFF00)!=0){FlagC=true;}}if((daa&0xF0)>0x90||FlagC){daa+=0x60;if((daa&0xFF00)!=0){FlagC=true;}}SetFlagsSZP(daa);A=(byte
)daa;break;case 0x28:break;case 0x29:DAD(HL);break;case 0x2A:L=memory[Data16];H=memory[Data16+1];PC+=2;;break;case 0x2B:
HL-=1;break;case 0x2C:L=INR(L);break;case 0x2D:L=DCR(L);break;case 0x2E:L=Data8;PC+=1;;break;case 0x2F:A=(byte)~A;break;
case 0x30:break;case 0x31:SP=Data16;PC+=2;;break;case 0x32:memory[Data16]=A;PC+=2;break;case 0x33:SP+=1;break;case 0x34:M=
INR(M);break;case 0x35:M=DCR(M);break;case 0x36:M=Data8;PC+=1;;break;case 0x37:FlagC=true;break;case 0x38:break;case 0x39:
DAD(SP);break;case 0x3A:A=memory[Data16];PC+=2;break;case 0x3B:SP-=1;break;case 0x3C:A=INR(A);break;case 0x3D:A=DCR(A);
break;case 0x3E:A=Data8;PC+=1;;break;case 0x3F:FlagC=!FlagC;break;case 0x40:break;case 0x41:B=C;break;case 0x42:B=D;break;
case 0x43:B=E;break;case 0x44:B=H;break;case 0x45:B=L;break;case 0x46:B=M;break;case 0x47:B=A;break;case 0x48:C=B;break;case
0x49:break;case 0x4A:C=D;break;case 0x4B:C=E;break;case 0x4C:C=H;break;case 0x4D:C=L;break;case 0x4E:C=M;break;case 0x4F:C=A
;break;case 0x50:D=B;break;case 0x51:D=C;break;case 0x52:break;case 0x53:D=E;break;case 0x54:D=H;break;case 0x55:D=L;
break;case 0x56:D=M;break;case 0x57:D=A;break;case 0x58:E=B;break;case 0x59:E=C;break;case 0x5A:E=D;break;case 0x5B:break;
case 0x5C:E=H;break;case 0x5D:E=L;break;case 0x5E:E=M;break;case 0x5F:E=A;break;case 0x60:H=B;break;case 0x61:H=C;break;case
0x62:H=D;break;case 0x63:H=E;break;case 0x64:break;case 0x65:H=L;break;case 0x66:H=M;break;case 0x67:H=A;break;case 0x68:L=B
;break;case 0x69:L=C;break;case 0x6A:L=D;break;case 0x6B:L=E;break;case 0x6C:L=H;break;case 0x6D:break;case 0x6E:L=M;
break;case 0x6F:L=A;break;case 0x70:M=B;break;case 0x71:M=C;break;case 0x72:M=D;break;case 0x73:M=E;break;case 0x74:M=H;break
;case 0x75:M=L;break;case 0x76:PC--;break;case 0x77:M=A;break;case 0x78:A=B;break;case 0x79:A=C;break;case 0x7A:A=D;break
;case 0x7B:A=E;break;case 0x7C:A=H;break;case 0x7D:A=L;break;case 0x7E:A=M;break;case 0x7F:break;case 0x80:ADD(B);break;
case 0x81:ADD(C);break;case 0x82:ADD(D);break;case 0x83:ADD(E);break;case 0x84:ADD(H);break;case 0x85:ADD(L);break;case 0x86
:ADD(M);break;case 0x87:ADD(A);break;case 0x88:ADC(B);break;case 0x89:ADC(C);break;case 0x8A:ADC(D);break;case 0x8B:ADC(E
);break;case 0x8C:ADC(H);break;case 0x8D:ADC(L);break;case 0x8E:ADC(M);break;case 0x8F:ADC(A);break;case 0x90:SUB(B);
break;case 0x91:SUB(C);break;case 0x92:SUB(D);break;case 0x93:SUB(E);break;case 0x94:SUB(H);break;case 0x95:SUB(L);break;case
0x96:SUB(M);break;case 0x97:SUB(A);break;case 0x98:SBB(B);break;case 0x99:SBB(C);break;case 0x9A:SBB(D);break;case 0x9B:SBB(
E);break;case 0x9C:SBB(H);break;case 0x9D:SBB(L);break;case 0x9E:SBB(M);break;case 0x9F:SBB(A);break;case 0xA0:ANA(B);
break;case 0xA1:ANA(C);break;case 0xA2:ANA(D);break;case 0xA3:ANA(E);break;case 0xA4:ANA(H);break;case 0xA5:ANA(L);break;case
0xA6:ANA(M);break;case 0xA7:ANA(A);break;case 0xA8:XRA(B);break;case 0xA9:XRA(C);break;case 0xAA:XRA(D);break;case 0xAB:XRA(
E);break;case 0xAC:XRA(H);break;case 0xAD:XRA(L);break;case 0xAE:XRA(M);break;case 0xAF:XRA(A);break;case 0xB0:ORA(B);
break;case 0xB1:ORA(C);break;case 0xB2:ORA(D);break;case 0xB3:ORA(E);break;case 0xB4:ORA(H);break;case 0xB5:ORA(L);break;case
0xB6:ORA(M);break;case 0xB7:ORA(A);break;case 0xB8:CMP(B);break;case 0xB9:CMP(C);break;case 0xBA:CMP(D);break;case 0xBB:CMP(
E);break;case 0xBC:CMP(H);break;case 0xBD:CMP(L);break;case 0xBE:CMP(M);break;case 0xBF:CMP(A);break;case 0xC0:
RETURNCONDITIONAL(!FlagZ);break;case 0xC1:BC=POP();break;case 0xC2:JUMP(!FlagZ);break;case 0xC3:JUMP(true);break;case 0xC4:
CALLCONDITIONAL(!FlagZ);break;case 0xC5:PUSH(BC);break;case 0xC6:ADD(Data8);PC+=1;break;case 0xC7:RST(0x0);break;case 0xC8:
RETURNCONDITIONAL(FlagZ);break;case 0xC9:RETURNALWAYS();break;case 0xCA:JUMP(FlagZ);break;case 0xCB:JUMP(true);break;case 0xCC:
CALLCONDITIONAL(FlagZ);break;case 0xCD:CALLALWAYS();break;case 0xCE:ADC(Data8);PC+=1;;break;case 0xCF:RST(0x8);break;case 0xD0:
RETURNCONDITIONAL(!FlagC);break;case 0xD1:DE=POP();break;case 0xD2:JUMP(!FlagC);break;case 0xD3:iobus.Write(Data8,A);PC+=1;break;case
0xD4:CALLCONDITIONAL(!FlagC);break;case 0xD5:PUSH(DE);break;case 0xD6:SUB(Data8);PC+=1;;break;case 0xD7:RST(0x10);break;case
0xD8:RETURNCONDITIONAL(FlagC);break;case 0xD9:RETURNALWAYS();break;case 0xDA:JUMP(FlagC);break;case 0xDB:A=iobus.Read(Data8)
;PC+=1;break;case 0xDC:CALLCONDITIONAL(FlagC);break;case 0xDD:CALLALWAYS();break;case 0xDE:SBB(Data8);PC+=1;;break;case
0xDF:RST(0x18);;break;case 0xE0:RETURNCONDITIONAL(!FlagP);break;case 0xE1:HL=POP();break;case 0xE2:JUMP(!FlagP);break;case
0xE3:L^=memory[SP];memory[SP]^=L;L^=memory[SP];H^=memory[SP+1];memory[SP+1]^=H;H^=memory[SP+1];break;case 0xE4:
CALLCONDITIONAL(!FlagP);break;case 0xE5:PUSH(HL);break;case 0xE6:ANA(Data8);PC+=1;;break;case 0xE7:RST(0x20);break;case 0xE8:
RETURNCONDITIONAL(FlagP);break;case 0xE9:PC=HL;break;case 0xEA:JUMP(FlagP);break;case 0xEB:HL^=DE;DE^=HL;HL^=DE;break;case 0xEC:
CALLCONDITIONAL(FlagP);break;case 0xED:CALLALWAYS();break;case 0xEE:XRA(Data8);PC+=1;;break;case 0xEF:RST(0x28);break;case 0xF0:
RETURNCONDITIONAL(!FlagS);break;case 0xF1:AF=POP();break;case 0xF2:JUMP(!FlagS);break;case 0xF3:interruptPin=false;break;case 0xF4:
CALLCONDITIONAL(!FlagS);break;case 0xF5:PUSH(AF);break;case 0xF6:ORA(Data8);PC+=1;;break;case 0xF7:RST(0x30);break;case 0xF8:
RETURNCONDITIONAL(FlagS);break;case 0xF9:SP=HL;break;case 0xFA:JUMP(FlagS);break;case 0xFB:interruptPin=true;break;case 0xFC:
CALLCONDITIONAL(FlagS);break;case 0xFD:CALLALWAYS();break;case 0xFE:CMP(Data8);PC+=1;;break;case 0xFF:RST(0x38);break;default:
warnUnsupportedOpcode(opcode);break;}}private byte INR(byte b){int result=b+1;SetFlagAC(b,1);SetFlagsSZP(result);return(byte)result;}private
byte DCR(byte b){int result=b-1;SetFlagACSub(b,1);SetFlagsSZP(result);return(byte)result;}private void ADD(byte b){int
result=A+b;SetFlagAC(A,b);SetFlagC(result);SetFlagsSZP(result);A=(byte)result;}private void ADC(byte b){int carry=FlagC?1:0;
int result=A+b+carry;if(FlagC)SetFlagACCarry(A,b);else SetFlagAC(A,b);SetFlagC(result);SetFlagsSZP(result);A=(byte)result;}
private void SUB(byte b){int result=A-b;SetFlagACSub(A,b);SetFlagC(result);SetFlagsSZP(result);A=(byte)result;}private void SBB
(byte b){int carry=FlagC?1:0;int result=A-b-carry;if(FlagC)SetFlagACSubCarry(A,b);else SetFlagACSub(A,b);SetFlagC(result)
;SetFlagsSZP(result);A=(byte)result;}private void ANA(byte b){byte result=(byte)(A&b);FlagAC=((A|b)&0x08)!=0;FlagC=false;
SetFlagsSZP(result);A=result;}private void XRA(byte b){byte result=(byte)(A^b);FlagAC=false;FlagC=false;SetFlagsSZP(result);A=
result;}private void ORA(byte b){byte result=(byte)(A|b);FlagAC=false;FlagC=false;SetFlagsSZP(result);A=result;}private void
CMP(byte b){int result=A-b;SetFlagACSub(A,b);SetFlagC(result);SetFlagsSZP(result);}private void DAD(ushort w){int result=HL
+w;FlagC=result>>16!=0;HL=(ushort)result;}private void RETURNALWAYS(){PC=POP();}private void RETURNCONDITIONAL(bool flag)
{PC=flag?POP():PC;if(flag)cycles+=6;}private void CALLCONDITIONAL(bool flag){if(flag){PUSH((ushort)(PC+2));PC=Data16;
cycles+=6;}else{PC+=2;}}private void CALLALWAYS(){PUSH((ushort)(PC+2));PC=Data16;}private void JUMP(bool flag){PC=flag?Data16:
PC+=2;;}private void RST(byte b){PUSH((ushort)(PC));PC=b;}private void PUSH(ushort d16){SP-=2;byte[]bytes=BitConverter.
GetBytes(d16);Array.Copy(bytes,0,memory,SP,bytes.Length);}private ushort POP(){ushort ret=combineRegisters(memory[SP+1],memory[
SP]);SP+=2;return ret;}private ushort combineRegisters(byte b1,byte b2){return(ushort)(b1<<8|b2);}private void SetFlagC(
int i){FlagC=(i>>8)!=0;}private void SetFlagsSZP(int i){byte b=(byte)i;FlagS=(b&0x80)!=0;FlagZ=b==0;FlagP=parity(b);}
private void SetFlagAC(byte b1,byte b2){FlagAC=((b1&0xF)+(b2&0xF))>0xF;}private void SetFlagACCarry(byte b1,byte b2){FlagAC=((
b1&0xF)+(b2&0xF))>=0xF;}private void SetFlagACSub(byte b1,byte b2){FlagAC=(b2&0xF)<=(b1&0xF);}private void
SetFlagACSubCarry(byte b1,byte b2){FlagAC=(b2&0xF)<(b1&0xF);}private bool parity(byte b){byte bits=0;for(int i=0;i<8;i++){if((b&0x80>>i)
!=0){bits+=1;}}return(bits%2==0);}public void handleInterrupt(byte b){if(interruptPin){PUSH(PC);PC=(ushort)(8*b);
interruptPin=false;}}private void warnUnsupportedOpcode(byte opcode){throw new Exception((PC-1).ToString("x4")+
" Unsupported operation "+opcode.ToString("x2"));}}class JCTRL{MyGridProgram mypgm=null;JDBG jdbg=null;bool singleKey=false;public JCTRL(
MyGridProgram pgm,JDBG dbg,bool SingleKeyOnly){mypgm=pgm;jdbg=dbg;}public List<IMyTerminalBlock>GetCTRLsWithTag(String tag){List<
IMyTerminalBlock>allCTRLs=new List<IMyTerminalBlock>();mypgm.GridTerminalSystem.GetBlocksOfType(allCTRLs,(IMyTerminalBlock x)=>((x.
CustomName!=null)&&(x.CustomName.ToUpper().IndexOf("["+tag.ToUpper()+"]")>=0)&&(x is IMyShipController)));jdbg.Debug("Found "+
allCTRLs.Count+" controllers with tag "+tag);return allCTRLs;}public bool IsOccupied(IMyShipController seat){return seat.
IsUnderControl;}public bool AnyKey(IMyShipController seat,bool allowJumpOrCrouch){bool pressed=false;Vector3 dirn=seat.MoveIndicator;
if(dirn.X!=0||(allowJumpOrCrouch&&dirn.Y!=0)||dirn.Z!=0){pressed=true;}return pressed;}public bool IsLeft(
IMyShipController seat){Vector3 dirn=seat.MoveIndicator;if(singleKey&&dirn.X<0&&dirn.Y==0&&dirn.Z==0)return true;else if(!singleKey&&dirn
.X<0)return true;return false;}public bool IsRight(IMyShipController seat){Vector3 dirn=seat.MoveIndicator;if(singleKey&&
dirn.X>0&&dirn.Y==0&&dirn.Z==0)return true;else if(!singleKey&&dirn.X>0)return true;return false;}public bool IsUp(
IMyShipController seat){Vector3 dirn=seat.MoveIndicator;if(singleKey&&dirn.X==0&&dirn.Y==0&&dirn.Z<0)return true;else if(!singleKey&&dirn
.Z<0)return true;return false;}public bool IsDown(IMyShipController seat){Vector3 dirn=seat.MoveIndicator;if(singleKey&&
dirn.X==0&&dirn.Y==0&&dirn.Z>0)return true;else if(!singleKey&&dirn.Z>0)return true;return false;}public bool IsSpace(
IMyShipController seat){Vector3 dirn=seat.MoveIndicator;if(singleKey&&dirn.X==0&&dirn.Y>0&&dirn.Z==0)return true;else if(!singleKey&&dirn
.Y>0)return true;return false;}public bool IsCrouch(IMyShipController seat){Vector3 dirn=seat.MoveIndicator;if(singleKey
&&dirn.X==0&&dirn.Y<0&&dirn.Z==0)return true;else if(!singleKey&&dirn.Y<0)return true;return false;}public bool IsRollLeft
(IMyShipController seat){float dirn=seat.RollIndicator;if(dirn<0.0)return true;return false;}public bool IsRollRight(
IMyShipController seat){float dirn=seat.RollIndicator;if(dirn>0.0)return true;return false;}public bool IsArrowLeft(IMyShipController
seat){Vector2 dirn=seat.RotationIndicator;if(singleKey&&dirn.X==0&&dirn.Y<0)return true;else if(!singleKey&&dirn.Y<0)return
true;return false;}public bool IsArrowRight(IMyShipController seat){Vector2 dirn=seat.RotationIndicator;if(singleKey&&dirn.X
==0&&dirn.Y>0)return true;else if(!singleKey&&dirn.Y>0)return true;return false;}public bool IsArrowDown(IMyShipController
seat){Vector2 dirn=seat.RotationIndicator;if(singleKey&&dirn.X>0&&dirn.Y==0)return true;else if(!singleKey&&dirn.X>0)return
true;return false;}public bool IsArrowUp(IMyShipController seat){Vector2 dirn=seat.RotationIndicator;if(singleKey&&dirn.X<0
&&dirn.Y==0)return true;else if(!singleKey&&dirn.X<0)return true;return false;}}class JDBG{public bool debug=false;private
MyGridProgram mypgm=null;private JLCD jlcd=null;private bool inDebug=false;private static List<IMyTerminalBlock>debugLCDs=null;public
JDBG(MyGridProgram pgm,bool debugState){mypgm=pgm;debug=debugState;jlcd=new JLCD(pgm,this,false);}public void Echo(string
str){mypgm.Echo("JDBG: "+str);}public void Debug(String str){Debug(str,true);}public void Debug(String str,bool consoledbg)
{if(debug&&!inDebug){inDebug=true;if(debugLCDs==null){Echo("First run - working out debug panels");initializeDBGLCDs();
ClearDebugLCDs();}Echo("D:"+str);jlcd.WriteToAllLCDs(debugLCDs,str+"\n",true);inDebug=false;}}public void DebugAndEcho(String str){
Echo(str);Debug(str,false);}private void initializeDBGLCDs(){inDebug=true;debugLCDs=jlcd.GetLCDsWithTag("DEBUG");jlcd.
InitializeLCDs(debugLCDs,TextAlignment.LEFT);inDebug=false;}public void ClearDebugLCDs(){if(debug){if(debugLCDs==null){Echo(
"First runC - working out debug panels");initializeDBGLCDs();}jlcd.WriteToAllLCDs(debugLCDs,"",false);}}public void Alert(String alertMsg,String colour,String
alertTag,String thisScript){List<IMyTerminalBlock>allBlocksWithLCDs=new List<IMyTerminalBlock>();mypgm.GridTerminalSystem.
GetBlocksOfType(allBlocksWithLCDs,(IMyTerminalBlock x)=>((x.CustomName!=null)&&(x.CustomName.ToUpper().IndexOf("["+alertTag.ToUpper()+
"]")>=0)&&(x is IMyTextSurfaceProvider)));DebugAndEcho("Found "+allBlocksWithLCDs.Count+" lcds with '"+alertTag+
"' to alert to");String alertOutput=JLCD.solidcolor[colour]+" "+DateTime.Now.ToShortTimeString()+":"+thisScript+" "+alertMsg+"\n";
DebugAndEcho("ALERT: "+alertMsg);if(allBlocksWithLCDs.Count>0){jlcd.WriteToAllLCDs(allBlocksWithLCDs,alertOutput,true);}}public void
EchoCurrentInstructionCount(String tag){Echo(tag+" instruction count: "+mypgm.Runtime.CurrentInstructionCount+","+mypgm.Runtime.
CurrentCallChainDepth);}public void EchoMaxInstructionCount(){Echo("Max instruction count: "+mypgm.Runtime.MaxInstructionCount+","+mypgm.
Runtime.MaxCallChainDepth);}}class JINV{private JDBG jdbg=null;public enum BLUEPRINT_TYPES{BOTTLES,COMPONENTS,AMMO,TOOLS,OTHER,
ORES};Dictionary<String,String>oreToIngot=new Dictionary<String,String>{{"MyObjectBuilder_Ore/Cobalt",
"MyObjectBuilder_Ingot/Cobalt"},{"MyObjectBuilder_Ore/Gold","MyObjectBuilder_Ingot/Gold"},{"MyObjectBuilder_Ore/Stone","MyObjectBuilder_Ingot/Stone"},
{"MyObjectBuilder_Ore/Iron","MyObjectBuilder_Ingot/Iron"},{"MyObjectBuilder_Ore/Magnesium",
"MyObjectBuilder_Ingot/Magnesium"},{"MyObjectBuilder_Ore/Nickel","MyObjectBuilder_Ingot/Nickel"},{"MyObjectBuilder_Ore/Platinum",
"MyObjectBuilder_Ingot/Platinum"},{"MyObjectBuilder_Ore/Silicon","MyObjectBuilder_Ingot/Silicon"},{"MyObjectBuilder_Ore/Silver",
"MyObjectBuilder_Ingot/Silver"},{"MyObjectBuilder_Ore/Uranium","MyObjectBuilder_Ingot/Uranium"},};Dictionary<String,String>otherCompToBlueprint=new
Dictionary<String,String>{{"MyObjectBuilder_BlueprintDefinition/Position0040_Datapad","MyObjectBuilder_Datapad/Datapad"},};
Dictionary<String,String>toolsCompToBlueprint=new Dictionary<String,String>{{"MyObjectBuilder_PhysicalGunObject/AngleGrinderItem",
"MyObjectBuilder_BlueprintDefinition/Position0010_AngleGrinder"},{"MyObjectBuilder_PhysicalGunObject/AngleGrinder2Item",
"MyObjectBuilder_BlueprintDefinition/Position0020_AngleGrinder2"},{"MyObjectBuilder_PhysicalGunObject/AngleGrinder3Item",
"MyObjectBuilder_BlueprintDefinition/Position0030_AngleGrinder3"},{"MyObjectBuilder_PhysicalGunObject/AngleGrinder4Item",
"MyObjectBuilder_BlueprintDefinition/Position0040_AngleGrinder4"},{"MyObjectBuilder_PhysicalGunObject/WelderItem","MyObjectBuilder_BlueprintDefinition/Position0090_Welder"},{
"MyObjectBuilder_PhysicalGunObject/Welder2Item","MyObjectBuilder_BlueprintDefinition/Position0100_Welder2"},{"MyObjectBuilder_PhysicalGunObject/Welder3Item",
"MyObjectBuilder_BlueprintDefinition/Position0110_Welder3"},{"MyObjectBuilder_PhysicalGunObject/Welder4Item","MyObjectBuilder_BlueprintDefinition/Position0120_Welder4"},{
"MyObjectBuilder_PhysicalGunObject/HandDrillItem","MyObjectBuilder_BlueprintDefinition/Position0050_HandDrill"},{"MyObjectBuilder_PhysicalGunObject/HandDrill2Item",
"MyObjectBuilder_BlueprintDefinition/Position0060_HandDrill2"},{"MyObjectBuilder_PhysicalGunObject/HandDrill3Item","MyObjectBuilder_BlueprintDefinition/Position0070_HandDrill3"},{
"MyObjectBuilder_PhysicalGunObject/HandDrill4Item","MyObjectBuilder_BlueprintDefinition/Position0080_HandDrill4"},};Dictionary<String,String>bottlesCompToBlueprint=new
Dictionary<String,String>{{"MyObjectBuilder_GasContainerObject/HydrogenBottle",
"MyObjectBuilder_BlueprintDefinition/Position0020_HydrogenBottle"},{"MyObjectBuilder_OxygenContainerObject/OxygenBottle","MyObjectBuilder_BlueprintDefinition/HydrogenBottlesRefill"},};
Dictionary<String,String>componentsCompToBlueprint=new Dictionary<String,String>{{"MyObjectBuilder_Component/BulletproofGlass",
"MyObjectBuilder_BlueprintDefinition/BulletproofGlass"},{"MyObjectBuilder_Component/Canvas","MyObjectBuilder_BlueprintDefinition/Position0030_Canvas"},{
"MyObjectBuilder_Component/Computer","MyObjectBuilder_BlueprintDefinition/ComputerComponent"},{"MyObjectBuilder_Component/Construction",
"MyObjectBuilder_BlueprintDefinition/ConstructionComponent"},{"MyObjectBuilder_Component/Detector","MyObjectBuilder_BlueprintDefinition/DetectorComponent"},{
"MyObjectBuilder_Component/Display","MyObjectBuilder_BlueprintDefinition/Display"},{"MyObjectBuilder_Component/Explosives",
"MyObjectBuilder_BlueprintDefinition/ExplosivesComponent"},{"MyObjectBuilder_Component/Girder","MyObjectBuilder_BlueprintDefinition/GirderComponent"},{
"MyObjectBuilder_Component/GravityGenerator","MyObjectBuilder_BlueprintDefinition/GravityGeneratorComponent"},{"MyObjectBuilder_Component/InteriorPlate",
"MyObjectBuilder_BlueprintDefinition/InteriorPlate"},{"MyObjectBuilder_Component/LargeTube","MyObjectBuilder_BlueprintDefinition/LargeTube"},{
"MyObjectBuilder_Component/Medical","MyObjectBuilder_BlueprintDefinition/MedicalComponent"},{"MyObjectBuilder_Component/MetalGrid",
"MyObjectBuilder_BlueprintDefinition/MetalGrid"},{"MyObjectBuilder_Component/Motor","MyObjectBuilder_BlueprintDefinition/MotorComponent"},{
"MyObjectBuilder_Component/PowerCell","MyObjectBuilder_BlueprintDefinition/PowerCell"},{"MyObjectBuilder_Component/Reactor",
"MyObjectBuilder_BlueprintDefinition/ReactorComponent"},{"MyObjectBuilder_Component/RadioCommunication","MyObjectBuilder_BlueprintDefinition/RadioCommunicationComponent"},{
"MyObjectBuilder_Component/SmallTube","MyObjectBuilder_BlueprintDefinition/SmallTube"},{"MyObjectBuilder_Component/SolarCell",
"MyObjectBuilder_BlueprintDefinition/SolarCell"},{"MyObjectBuilder_Component/SteelPlate","MyObjectBuilder_BlueprintDefinition/SteelPlate"},{
"MyObjectBuilder_Component/Superconductor","MyObjectBuilder_BlueprintDefinition/Superconductor"},{"MyObjectBuilder_Component/Thrust",
"MyObjectBuilder_BlueprintDefinition/ThrustComponent"},};Dictionary<String,String>ammoCompToBlueprint=new Dictionary<String,String>{{
"MyObjectBuilder_AmmoMagazine/NATO_25x184mm","MyObjectBuilder_BlueprintDefinition/Position0080_NATO_25x184mmMagazine"},{
"MyObjectBuilder_AmmoMagazine/AutocannonClip","MyObjectBuilder_BlueprintDefinition/Position0090_AutocannonClip"},{"MyObjectBuilder_AmmoMagazine/Missile200mm",
"MyObjectBuilder_BlueprintDefinition/Position0100_Missile200mm"},{"MyObjectBuilder_AmmoMagazine/MediumCalibreAmmo","MyObjectBuilder_BlueprintDefinition/Position0110_MediumCalibreAmmo"
},{"MyObjectBuilder_AmmoMagazine/LargeCalibreAmmo","MyObjectBuilder_BlueprintDefinition/Position0120_LargeCalibreAmmo"},{
"MyObjectBuilder_AmmoMagazine/SmallRailgunAmmo","MyObjectBuilder_BlueprintDefinition/Position0130_SmallRailgunAmmo"},{"MyObjectBuilder_AmmoMagazine/LargeRailgunAmmo",
"MyObjectBuilder_BlueprintDefinition/Position0140_LargeRailgunAmmo"},{"MyObjectBuilder_AmmoMagazine/SemiAutoPistolMagazine",
"MyObjectBuilder_BlueprintDefinition/Position0010_SemiAutoPistolMagazine"},{"MyObjectBuilder_AmmoMagazine/ElitePistolMagazine",
"MyObjectBuilder_BlueprintDefinition/Position0030_ElitePistolMagazine"},{"MyObjectBuilder_AmmoMagazine/FullAutoPistolMagazine",
"MyObjectBuilder_BlueprintDefinition/Position0020_FullAutoPistolMagazine"},{"MyObjectBuilder_AmmoMagazine/AutomaticRifleGun_Mag_20rd",
"MyObjectBuilder_BlueprintDefinition/Position0040_AutomaticRifleGun_Mag_20rd"},{"MyObjectBuilder_AmmoMagazine/UltimateAutomaticRifleGun_Mag_30rd",
"MyObjectBuilder_BlueprintDefinition/Position0070_UltimateAutomaticRifleGun_Mag_30rd"},{"MyObjectBuilder_AmmoMagazine/RapidFireAutomaticRifleGun_Mag_50rd",
"MyObjectBuilder_BlueprintDefinition/Position0050_RapidFireAutomaticRifleGun_Mag_50rd"},{"MyObjectBuilder_AmmoMagazine/PreciseAutomaticRifleGun_Mag_5rd",
"MyObjectBuilder_BlueprintDefinition/Position0060_PreciseAutomaticRifleGun_Mag_5rd"},{"MyObjectBuilder_AmmoMagazine/NATO_5p56x45mm",null},};public JINV(JDBG dbg){jdbg=dbg;}public void addBluePrints(
BLUEPRINT_TYPES types,ref Dictionary<String,String>into){switch(types){case BLUEPRINT_TYPES.BOTTLES:into=into.Concat(
bottlesCompToBlueprint).ToDictionary(x=>x.Key,x=>x.Value);break;case BLUEPRINT_TYPES.COMPONENTS:into=into.Concat(componentsCompToBlueprint).
ToDictionary(x=>x.Key,x=>x.Value);break;case BLUEPRINT_TYPES.AMMO:into=into.Concat(ammoCompToBlueprint).ToDictionary(x=>x.Key,x=>x.
Value);break;case BLUEPRINT_TYPES.TOOLS:into=into.Concat(toolsCompToBlueprint).ToDictionary(x=>x.Key,x=>x.Value);break;case
BLUEPRINT_TYPES.OTHER:into=into.Concat(otherCompToBlueprint).ToDictionary(x=>x.Key,x=>x.Value);break;case BLUEPRINT_TYPES.ORES:into=
into.Concat(oreToIngot).ToDictionary(x=>x.Key,x=>x.Value);break;}}}class JLCD{public MyGridProgram mypgm=null;public JDBG
jdbg=null;bool suppressDebug=false;public static Dictionary<String,char>solidcolor=new Dictionary<String,char>{{"YELLOW",''
},{"RED",''},{"ORANGE",''},{"GREEN",''},{"CYAN",''},{"PURPLE",''},{"BLUE",''},{"WHITE",''},{"BLACK",''},{"GREY",
''}};public const char COLOUR_YELLOW='';public const char COLOUR_RED='';public const char COLOUR_ORANGE='';public const
char COLOUR_GREEN='';public const char COLOUR_CYAN='';public const char COLOUR_PURPLE='';public const char COLOUR_BLUE=
'';public const char COLOUR_WHITE='';public const char COLOUR_BLACK='';public const char COLOUR_GREY='';public JLCD(
MyGridProgram pgm,JDBG dbg,bool suppressDebug){this.mypgm=pgm;this.jdbg=dbg;this.suppressDebug=suppressDebug;}public List<
IMyTerminalBlock>GetLCDsWithTag(String tag){List<IMyTerminalBlock>allLCDs=new List<IMyTerminalBlock>();mypgm.GridTerminalSystem.
GetBlocksOfType(allLCDs,(IMyTerminalBlock x)=>((x.CustomName!=null)&&(x.CustomName.ToUpper().IndexOf("["+tag.ToUpper()+"]")>=0)&&(x is
IMyTextSurfaceProvider)));jdbg.Debug("Found "+allLCDs.Count+" lcds to update with tag "+tag);return allLCDs;}public List<IMyTerminalBlock>
GetLCDsWithName(String tag){List<IMyTerminalBlock>allLCDs=new List<IMyTerminalBlock>();mypgm.GridTerminalSystem.GetBlocksOfType(allLCDs
,(IMyTerminalBlock x)=>((x.CustomName!=null)&&(x.CustomName.ToUpper().IndexOf(tag.ToUpper())>=0)&&(x is
IMyTextSurfaceProvider)));jdbg.Debug("Found "+allLCDs.Count+" lcds to update with tag "+tag);return allLCDs;}public void InitializeLCDs(List<
IMyTerminalBlock>allLCDs,TextAlignment align){foreach(var thisLCD in allLCDs){jdbg.Debug("Setting up the font for "+thisLCD.CustomName);
IMyTextSurface thisSurface=((IMyTextSurfaceProvider)thisLCD).GetSurface(0);if(thisSurface==null)continue;thisSurface.Font="Monospace";
thisSurface.ContentType=ContentType.TEXT_AND_IMAGE;thisSurface.BackgroundColor=Color.Black;thisSurface.Alignment=align;thisSurface.
TextPadding=0;}}public void SetLCDFontColour(List<IMyTerminalBlock>allLCDs,Color colour){foreach(var thisLCD in allLCDs){if(thisLCD
is IMyTextPanel){jdbg.Debug("Setting up the color for "+thisLCD.CustomName);((IMyTextPanel)thisLCD).FontColor=colour;}}}
public void SetLCDRotation(List<IMyTerminalBlock>allLCDs,float Rotation){foreach(var thisLCD in allLCDs){if(thisLCD is
IMyTextPanel){jdbg.Debug("Setting up the rotation for "+thisLCD.CustomName);thisLCD.SetValueFloat("Rotate",Rotation);}}}public void
SetupFont(List<IMyTerminalBlock>allLCDs,int rows,int cols,bool mostlySpecial){_SetupFontCalc(allLCDs,ref rows,cols,mostlySpecial,
0.05F,0.05F);}public int SetupFontWidthOnly(List<IMyTerminalBlock>allLCDs,int cols,bool mostlySpecial){int rows=-1;
_SetupFontCalc(allLCDs,ref rows,cols,mostlySpecial,0.05F,0.05F);return rows;}public void SetupFontCustom(List<IMyTerminalBlock>allLCDs
,int rows,int cols,bool mostlySpecial,float size,float incr){_SetupFontCalc(allLCDs,ref rows,cols,mostlySpecial,size,incr
);}private void _SetupFontCalc(List<IMyTerminalBlock>allLCDs,ref int rows,int cols,bool mostlySpecial,float startSize,
float startIncr){int bestRows=rows;foreach(var thisLCD in allLCDs){jdbg.Debug("Setting up font on screen: "+thisLCD.
CustomName+" ("+rows+" x "+cols+")");IMyTextSurface thisSurface=((IMyTextSurfaceProvider)thisLCD).GetSurface(0);if(thisSurface==
null)continue;float size=startSize;float incr=startIncr;StringBuilder teststr=new StringBuilder("".PadRight(cols,(
mostlySpecial?solidcolor["BLACK"]:'W')));Vector2 actualScreenSize=thisSurface.TextureSize;while(true){thisSurface.FontSize=size;
Vector2 actualSize=thisSurface.TextureSize;Vector2 thisSize=thisSurface.MeasureStringInPixels(teststr,thisSurface.Font,size);
int displayrows=(int)Math.Floor(actualScreenSize.Y/thisSize.Y);if((thisSize.X<actualSize.X)&&(rows==-1||(displayrows>rows))
){size+=incr;bestRows=displayrows;}else{break;}}thisSurface.FontSize=size-incr;jdbg.Debug("Calc size of "+thisSurface.
FontSize);if(rows==-1)rows=bestRows;if(thisLCD.DefinitionDisplayNameText.Contains("Corner LCD")){jdbg.Debug(
"INFO: Avoiding bug, multiplying by 4: "+thisLCD.DefinitionDisplayNameText);thisSurface.FontSize*=4;}}}public void UpdateFullScreen(IMyTerminalBlock block,
String text){List<IMyTerminalBlock>lcds=new List<IMyTerminalBlock>{block};InitializeLCDs(lcds,TextAlignment.CENTER);SetupFont(
lcds,1,text.Length+4,false);WriteToAllLCDs(lcds,text,false);}public void WriteToAllLCDs(List<IMyTerminalBlock>allLCDs,String
msg,bool append){foreach(var thisLCD in allLCDs){if(!this.suppressDebug)jdbg.Debug("Writing to display "+thisLCD.CustomName
);IMyTextSurface thisSurface=((IMyTextSurfaceProvider)thisLCD).GetSurface(0);if(thisSurface==null)continue;thisSurface.
WriteText(msg,append);}}public char ColorToChar(byte r,byte g,byte b){const double bitSpacing=255.0/7.0;return(char)(0xe100+((int
)Math.Round(r/bitSpacing)<<6)+((int)Math.Round(g/bitSpacing)<<3)+(int)Math.Round(b/bitSpacing));}}class Memory{public
byte[][]allProms=new byte[3][];public bool isColour=false;public static GetRomData.Games game;public Memory(){game=
GetRomData.Games.None;allProms[0]=new byte[0x10000];}public void LoadRom(ref byte[]keyBits,ref int rotate,GetRomData.Games newgame
,String gameData,ref byte backCol,ref bool needsProcessing,ref Display.paletteType palType,ref byte port_shift_result,ref
byte port_shift_data,ref byte port_shift_offset,ref byte port_input){game=newgame;allProms=GetRomData.getRomData(game,
gameData.Equals("")?GetRomData.getRomData(game):gameData,ref keyBits,ref rotate,ref backCol,ref needsProcessing,ref palType,ref
port_shift_result,ref port_shift_data,ref port_shift_offset,ref port_input);if(allProms[1]!=null){isColour=true;}else{isColour=false;}}}
class Specifics{public Arcade8080Machine am;public Display display;public JDBG jdbg;public JCTRL jctrl;public
IMyShipController controller;public Program mypgm;public static int maxFrames=45000;public static int skipFrames=0;public static int
skipFramesLeft=0;bool space=false;bool left=false;bool right=false;bool crouch=false;bool up=false;bool down=false;bool q=false;bool e
=false;public void CheckKeys(){if(left!=jctrl.IsLeft(controller)){left=!left;am.handleInput(am.keyBits[(int)GetRomData.
KeyIndex.keyleft],left);}if(right!=jctrl.IsRight(controller)){right=!right;am.handleInput(am.keyBits[(int)GetRomData.KeyIndex.
keyright],right);}if(up!=jctrl.IsUp(controller)){up=!up;am.handleInput(am.keyBits[(int)GetRomData.KeyIndex.keyup],up);}if(down!=
jctrl.IsDown(controller)){down=!down;am.handleInput(am.keyBits[(int)GetRomData.KeyIndex.keydown],down);}if(space!=jctrl.
IsSpace(controller)){space=!space;am.handleInput(am.keyBits[(int)GetRomData.KeyIndex.space],space);}if(crouch!=jctrl.IsCrouch(
controller)){crouch=!crouch;am.handleInput(am.keyBits[(int)GetRomData.KeyIndex.crouch],crouch);}if(am.cost==0&&(q!=jctrl.
IsRollLeft(controller))){q=!q;am.handleInput(am.keyBits[(int)GetRomData.KeyIndex.q],q);}if(e!=jctrl.IsRollRight(controller)){e=!e;
am.handleInput(am.keyBits[(int)GetRomData.KeyIndex.e],e);}}public void DebugAndEcho(String s){jdbg.DebugAndEcho(s);}public
void Echo(String s){jdbg.Echo(s);}public int GetInstructionCount(){return mypgm.Runtime.CurrentInstructionCount;}public bool
drawAndRenderFrame(ref int State,ref int actualDraws){bool didComplete=true;if(skipFramesLeft<=1){didComplete=display.
generateFrameToDisplay(ref State);if(!didComplete)return false;actualDraws++;mypgm.jlcd.WriteToAllLCDs(mypgm.displays,new String(DirectBitmap.
Pixels),false);skipFramesLeft=skipFrames;}else{skipFramesLeft--;}return true;}public void setRotation(int rotate){mypgm.jlcd.
SetLCDRotation(mypgm.displays,(float)rotate);}}