RenameShip
==========
This is a useful utility to help idenitfy which block comes from where by providing a docking port, and when a ship docks to it you can
select a prefix which is then applied to all blocks connected to that docking port. 

In effect, you would dock a ship, select the prefix on the buttons, then apply the prefix and all blocks on that ship get renamed

Instructions
------------
1. Add a programmable block, add config similar to the following
```
[Config]
connector=[rename] Connector
4buttonpanel=Rename 4 button[renamelcd]
dockedlcd = [renamelcd] Docked LCD
```

2. Add a connector and give it the exact name listed in the config
3. Add an LCD where the name of the ship currently docked will be displayed - Must have the exact name listed in the config
4. Add a 4 button panel which is where you will set the prefix and say go... 
5. Add the script to the programmable block, click recompile/run

Usage
-----
Dock a ship - if the connector of the ship already has a prefix, that will be displayed on the LCD
and the text over the 4 button pannel will display it, otherwise it will display unknown and the buttons
start blank

Use the buttons to select an up to 3 character prefix. Press a button rotates that letter by one - if you need
to go backwards you have to wait for it to fully rotate!

Once you have the buttons showing the prefix you want, press the 4th button and it will apply it - EVERY block in the grid connected
to the connector will be renamed