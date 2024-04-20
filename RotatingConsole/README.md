RotatingConsole
===============
This is a fun little script which takes a projection on a flat bed console, and gently rotates it around over time, so it can be better
seen as a 3D model from all sides.

Source available via https://github.com/eddy0612/SpaceEngineerScripts

Instructions
------------
1. Add a flat projection console, and on the name add a tag, for example [rotateme]
2. Add a programmable block, and inside set the custom data to something like:
```
[config]
tag=rotateme
xrotation=1
frameskip=2
```
3. Add the script to the programmable block, click recompile/run

Config Options
--------------
`xrotation` - Set to a number which is how many degrees should the projection be rotated in the x axis each time
`yrotation` - Set to a number which is how many degrees should the projection be rotated in the y axis each time
`zrotation` - Set to a number which is how many degrees should the projection be rotated in the z axis each time
`frameskip` - Set to a positive number which is how many frames apart should these updates be made. The higher the number, the lower the impact on the server but the rotation is less smooth
