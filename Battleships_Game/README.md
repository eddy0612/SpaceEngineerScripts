Battleships
===========
This implements a one or two player battleships game

Setup Instructions
------------------
1. Create a programmable block, name it something like `[GAMEPGM]`. Add custom data as follows - the value of the tag can be anything but you need to use it consistently everywhere as a prefix

```
[config]
tag=game
```

2. Create an LCD, change its name to `[GAMESCREEN] Player1 lcd`  (only the tag [..] bit is important)
3. In front of that add either a helm or cockpit, but in such a way that when in the cockpit you can see the whole screen. Change its name to `[GAMESEAT] Player1`
4. Add the script to the programmable block, recompile and run.

At this point you should be able to play in one player mode. However the default AI is rubbish/non-existant, this was more for testing.

For a 2 player mode, somewhere where the players cant see each other (eg another room on the ship/base):

5. Create an LCD, change its name to `[GAMESCREEN2] Player2 lcd`  (only the tag [..] bit is important)
6. In front of that add either a helm or cockpit, but in such a way that when in the cockpit you can see the whole screen. Change its name to `[GAMESEAT2] Player2`
7. On the programmable block click recompile

Controls
--------

```
   W       Crouch/Control - rotate during ship placement stage
A     D           Space   - fire/place
   S
```

Game play
---------
Initially you will place your ships. Once all ships are placed, you will then be shooting at yout partners ships. Kill all theirs before they hit yours!