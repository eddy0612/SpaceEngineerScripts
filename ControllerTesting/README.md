ControllerTesting
=================
This is a simple script to display on an LCD the input from a cockpit/helm and map it to key presses

Source available via https://github.com/eddy0612/SpaceEngineerScripts

Instructions
------------
1. Create programmable block. In block add customdata of
```
[config]
tag=controller
```

2. Add to an LCD the tag of [controllerSCREEN] into its custom name

3. Add to a controller/seat/helm the tag of [controllerSEAT] into its custom name

4. Add script to programmable block, recompile/run

Now, when you sit in the seat and use your controls, you should see on the display a mapping to key presses