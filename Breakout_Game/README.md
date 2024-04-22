BREAKOUT
========

This is a very simple implementation of Breakout - move the bat and keep the ball alive. I wrote this when learning about how to update screens (and the lack of graphics)

Source available via https://github.com/eddy0612/SpaceEngineerScripts

Instructions
------------
1. Create a programmable block, name it something like `[GAME.PGM]`. Add custom data as follows - the value of the tag can be anything but you need to use it consistently everywhere as a prefix

```
[config]
tag=game
```

2. Create an LCD, change its name to `[GAME.SCREEN] Player1 lcd`  (only the tag [..] bit is important)
3. In front of that add either a helm or cockpit, but in such a way that when in the cockpit you can see the whole screen. Change its 
       name to `[GAME.SEAT] Player1`
4. Add the script to the programmable block, recompile and run.

Controls
--------

A - Left
D - Right