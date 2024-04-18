# SpaceEngineerScripts

This repository is a list of scripts written when learning Space Engineers - Feel free to use them, 
copy them, edit them, ignore them. I absolutely recognize that a number of these have far better 
solutions in the workshop which I am not trying to replace, and they came about when learning the 
game and playing with no additional mods or scripts. I make no promises on lack of bugs nor good 
quality code :-)

The following are the various tools and their current status:

| Name | Status | Description                                               |
| ---- | ------ | --------------------------------------------------------- |
| AmmoHandler  | Working | Keep ammo at user requested levels and distributions |
| ArcadeEmulation | Complete | Intel 8080 Arcade Machine Emulator (eg Space Invaders) |
| Battleships_game | Complete  | One or 2 player battleships game |
| Breakout_game | Working | Simple game of breakout |
| ControllerTesting | Working | Displays how the inputs from a controller work |
| CountOre | Complete | Outputs to LCDs the current summary of the Ores, Ingots etc |
| DataPadhandler | InProgress | Aim to dedupe datapads, moving dupes away |
| DismantleTools | Complete | Periodically requeues any tools up to user specifed level to an assembler to be dismantled |
| JumpDrive | Complete | Display on LCD the limited info you can pull regarding jump drives |
| KeepAssemblersStocked | Complete | Keep assemblers at a certain level of ingots to reduce wait to pull and issues if too much pushed in |
| KeepBuilding | Working | Keep a certain amount of items built, display on LCD status, and queue items to build in batches |
| MoveBottles | Complete | Move bottles from most inventories to tanks to refill them |
| PowerData | InProgress | Display some data about the power usage |
| ReStacker | Finished | Periodically run through the inventories, combining stacks where possible |
| RenameShip | Finished | Rename all blocks on grid connected to a specific connector with a user prefix |
| StatusLCDs | Finished | Display assembler status beside the assembler plus a central status deisplay |
| Z80Emulator | Paused | Shared library for ZXSpectrum emulator
| ZXSpectrum | Paused | ZXSpectrum emulator


Note the following are shared libraries used by the above:

| Name | Description                                               |
| ---- | --------------------------------------------------------- |
| 8080Emulator | Used by the ArcadeEmulation project for 8080 support |
| JSharedUtils | Common classes used by most of my scripts |
 
Constructive feedback welcome!