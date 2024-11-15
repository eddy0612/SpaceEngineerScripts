## Intel 8080 Arcade Machine Emulator instructions

### Multiple game arcade machine emulator (Intel 8080)

This script emulates the 8080 used in the arcade machines very early on, in the era of Space 
Invaders. It can play multiple games and (on my machine) hits pretty much original speed. 
Unfortunately all arcade machines were subtly different so you do need to tell the emulator 
which game you are running, and that is done through the ROM filename (as known to MAME)

This is a very intensive script and updates an LCD at up to 60 frames per second, and so is
not a good idea to run this on a multiplayer server. I also suggest you turn the programmable 
block off when player leaves the controls as well for example. The games play without sound
as there was no way to play such sounds in a programmable block presently.

Code available on github: https://github.com/eddy0612/SpaceEngineerScripts

Q: What is the point of this and why did I do it?  I wrote some simple games as a joke, and then
got bored writing them one by one and wondered if this was possible... I found it very much was and then
got addicted to getting more games running in this engine

All feedback welcome, please use the discussion area of my github repo
       
```
+--------------------------------------------------------------------------------------------------
| THIS SCRIPT DOES NOT COME WITH ANY GAMES PREINSTALLED due to copyright. 
|
| Instructions provided below, BUT you need to get the roms and their associated strings which 
| requires (on windows) use of command line (cmd.exe), and one of
|   - python
|   - 2 utilities usually provided by eg cygwin - unzip and base64.
+--------------------------------------------------------------------------------------------------
```

## Installation instructions

1. Create empty programmable block named "[GAME.PGM] Block". Add this script to it, and modify the custom data to say:

```
    [config]
	tag=GAME
```

+ Note: (The prefix `GAME` is used for these instructions, you can use what you like)
2. Add an LCD, and name it "[GAME.SCREEN] LCD"
3. Add something that controls a ship in front of it, eg a cockpit or even better a helm - A 
   good idea to place it where you can see the screen!. Name it "[GAME.SEAT] Controller". Sit in 
   that cockpit, and press <tab> until the HUD is clear.
4. While still in that seat, Press G and type 'GAME'. Drag the '[GAME.PGM] Block' down to the 
   toolbar buttons, and chose the 'Run' option. In the popup, you can enter the parameter of the 
   filename of the game you wish to start.  I set up one button per filename, but you only need
   a single one to get it working.
5. Obtain the rom(s) you can play (from the list below) from the internet. A search on MAME with 
   filename is usually a good hit, see below. Remember what directory you saved the ROM under,
   which on windows is usually something like `c:\users\myuserid\downloads`
+ NOTE: You need the EXACT names listed below for this to work, similar files will not be
   handled correctly and will not work

-------------------------------------
EITHER Step 6 using python...
   You need to have manually installed python first from https://www.python.org/downloads/windows/
   (Pick the Windows Installer (64-bit) one, and run the installer)

6. Download GetRomData.py (or paste in from bottom of these instructions) and save it in the same
   directory as you saved the rom under. 
   
+  At this point you need to use a command prompt. Launch cmd.exe... you should get a black windowed 
    screen. In there type `cd /d "path_to_rom"` eg `cd /d "c:\users\myuser\downloads"`
+  Then for each game, run one of the following.. If the command fails, try the other one. 
     
			 python3 GetRomData.py invaders.zip    OR
			 python GetRomData.py invaders.zip

+ This will create a text file alongside the zip file with the contents you need later on, eg `invaders.zip.txt`
+ Copy the contents of this text file to the clipboard, which can be simply done in the command prompt by
      issuing `clip < invaders.zip.txt` or whatever the ROM name was.
 
-------------------------------------
OR Step 6 using cygwin... 
   You need to have manually installed cygwin first from https://www.cygwin.com/install.html
   (Pick the Windows x86_64 one, and run the installer)

6. Using cygwin's unzip and base64, in a comment prompt, do the following, for example:
			 `unzip -p invaders.zip | base64 --wrap=0 > invaders.zip.txt`

+ This will create a text file alongside the zip file with the contents you need later on, eg `invaders.zip.txt`
+ Copy the contents of this text file to the clipboard, which can be simply done in the command prompt by
      issuing `clip < invaders.zip.txt` or whatever the ROM name was.
-------------------------------------

7. Now go to Space engineers, and create a block... Any block with a custom data will do but I 
generally use control panels as they are tiny. Name them `[GAME] filename>`, eg `[GAME] invaders.zip`.
+ In the custom data, paste in what step 6 stored in the clipboard which is a long string. (I've put the 
expected first few characters in the table below)

8. Go to the programmable block, click recompile then run. 
+ Check for any error messages in the bottom right of the screen. If is ok to say no game configured

9. Go back to the helm/cockpit and launch a game by pressing the appropriate helm button
+ You can switch between running games as well if you have more than one ROM

10. If things go wrong at any point, you may need to click 'recompile' on the programmable block again to get things reset.


## Controls

```
      A - LEFT          W - UP				Q - Add credit
      D - RIGHT         S - DOWN			E - Player 1 start
      SPACE - Fire      CROUCH - Button 2
```
      
## Currently supports: (filename, name, first 10 chars expected, sample place to get it)

| Filename | Game | First chars of string | example download location |
| -- | -- | -- | -- |
| invaders.zip | Space Invaders			| IMPJFiGEIH | https://www.retrostic.com/roms/mame/space-invaders-space-invaders-m-41304
| ballbomb.zip | Balloon Bomber		    | AAAAwxgAAA | https://www.retrostic.com/roms/mame/balloon-bomber-44465
| lrescue.zip  | Lunar Rescue			| Dw8PDw8PDw | https://www.retrostic.com/roms/mame/lunar-rescue-43291#google_vignette
| schaser.zip  | Space Chaser			| Dw8PDw8PDw | https://www.retrostic.com/roms/mame/space-chaser-47039
| spacerng.zip | Space Ranger			| CAgICAgICA | https://www.retrostic.com/roms/mame/space-ranger-51087
| vortex.zip   | Vortex					| wicZKqd9Ks | https://www.retrostic.com/roms/mame/vortex-50198
| invrvnge.zip | Invaders Revenge		| ////////// | https://www.retrostic.com/roms/mame/invader-s-revenge-46143
| galxwars.zip | Galaxy Wars			| CAgICAgICA | https://www.retrostic.com/roms/mame/galaxy-wars-43764
| rollingc.zip | Rolling Crash/Moon Base| 88MAQNQYAA | https://www.retrostic.com/roms/mame/rolling-crash-moon-base-49086
| lupin3.zip   | Lupin III				| AAAAw0AAAA | https://www.retrostic.com/roms/mame/lupin-iii-44777
| polaris.zip  | Polaris				| AAAAw8YFAA | https://www.retrostic.com/roms/mame/polaris-45374
| indianbt.zip | Indian Battle			| AAAAw+VfKw | https://www.retrostic.com/roms/mame/indian-battle-46986
| astropal.zip | Astropal				| MaAglwDDtg | https://www.retrostic.com/roms/mame/astropal-47538
| galactic.zip | Galactica				| AAAAw5sA// | https://www.retrostic.com/roms/mame/galactica-batalha-espacial-45078
| attackfc.zip | Attack Force			| PgAyKyLDAA | https://www.retrostic.com/roms/mame/attack-force-47625
| maze.zip     | Amazing Maze			| AcMECDodIO | https://www.retrostic.com/roms/mame/amazing-maze-44504
| New in v2: |  |  |  |
| 280zzzap.zip | 280-zzzap              | ghSRFB8Tmx | https://www.retrostic.com/roms/mame/280-zzzap-46240
| boothill.zip | Boot Hill              | pzJIIPxBGH | https://www.retrostic.com/roms/mame/boot-hill-44141
| checkmat.zip | Checkmate              | DB3NWQzAIR | https://www.retrostic.com/roms/gbc/checkmate-16676
| bowler.zip   | Bowling Alley          | wDqsIrfKp0 | https://www.retrostic.com/roms/mame/bowling-alley-47394
| lagunar.zip  | Laguna Racer           | wvwXASAANv | https://www.retrostic.com/roms/mame/laguna-racer-48036
| gmissile.zip | Guided Missile         | vBIhBhHNTB | https://www.retrostic.com/roms/mame/guided-missile-48682
| m4.zip       | M-4 Tank Battle        | aAYYA1UYaE | https://www.retrostic.com/roms/mame/m-4-47533
| yosakdon.zip | Yosaku To Donbei       | w7AOAP8A/w | https://www.retrostic.com/roms/mame/yosaku-to-donbei-48248
| shuttlei.zip | Shuttle Invader        | MQBEr9P/0/ | https://www.retrostic.com/roms/mame/shuttle-invader-48506
| skylove.zip  | Sky Love               | rzKQYdP/0/ | https://www.retrostic.com/roms/mame/sky-love-49022
| cosmo.zip    | Cosmo                  | AAAAAADDGw | https://www.retrostic.com/roms/mame/cosmo-47900
| steelwkr.zip | Steel Worker           | MQAkw1AB// | https://www.retrostic.com/roms/mame/steel-worker-47543



 
### Credits:
  - This is a majorly reworked version of code based on an emulator created by Pedro Cort�s
       (BluestormDNA) and found here: https://github.com/BluestormDNA/i8080-Space-Invaders
     The CPU is only slighty modified but the rest was reworked to fit within Space Engineers
       limitations and fixed/extended for the other games, graphics, cycle handling and colour.
   - MAME source was reviewed to assist in getting the variety of games working
          https://github.com/mamedev/mame


## Additional options / functionality

   - Tag an LCD with eg. "[GAME.FPS] fps display" and it will be updated with an approx frames per
       second figure
   - Tag an LCD with eg. "[GAME.NAME] name display" and it will be updated with the name of the
       current game
   - Reduce the (very heavy) LCD panel impact by only drawing every other frame for example
       Add to the [config] something like the following (Not supplied or value of 1 means 
       draw every frame, 2 means every 2nd frame, 3 means every third etc:
       `renderframe=2`
   - Set a limit on how many cycles the game can take up - default is 45000, max is 49000. Less 
       cycles means slower FPS.  Add to the [config] something like
       `speed=40000`
   - Work like an arcade machine.. Add to the [config] something like
		`cost=5`
		`safetag=SAFE`
     Now, tag one cargo container near the machine with [GAME.COINS] Coin In, and another
       (which is connected via a conveyor, ideally with a one way sorter to stop things being
        removed!) as [SAFE] Coins Safe
     If cost is >0 then 'q' will no longer act as coin in, instead drop the space credits as a single
       stack into the "coins in" cargo container, and it should add a credit to the game, and move
       the space credits out into the safe... 
   
#### GetRomData.py code (available at https://github.com/eddy0612/SpaceEngineerScripts/blob/master/ArcadeEmulation/GetRomData.py)
```
import sys,os,base64
from zipfile import ZipFile
def extract_zip(input_zip):
    input_zip=ZipFile(input_zip)
    return {name: input_zip.read(name) for name in input_zip.namelist()}     
zip_name = os.sys.argv[1]
zip_data = extract_zip(zip_name)
total_data = bytearray()
for name,allbytes in zip_data.items(): total_data = total_data + allbytes
f = open(str(os.sys.argv[1]) + ".txt", "w")
f.write(base64.b64encode(total_data).decode("utf-8"))
f.close()
print( "File " + str(os.sys.argv[1]) + ".zip created")
print( "On windows you can send to clipboard by issuing:")
print( "  clip < \""+ str(os.sys.argv[1]) + ".zip\"")
```