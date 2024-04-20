KeepBuilding
============
This is an extremely useful script which you can set running on a base, and based on configuration you provide, it will
ensure there is always a certain amount of each item type that is available pre-built. If you are low on items, it queues
up in batches to ensure you dont block up the assemblers. in addition it can output to an LCD the current status of
how it is coping.

Source available via https://github.com/eddy0612/SpaceEngineerScripts

Instructions
------------
1. Add a programmable block. In its custom data, add the following (but edit the numbers - see below)

```
[Config]
; Update any LCD/cargo with Name [saveitems]
tag=saveitems
; Update the assembler to queue against with [qhere]
qtag=qhere
; Update every 10 seconds
refreshSpeed=10
; Bar length
barSize=20
; Bar under the item line?
barUnder=false
; item.x=component_name,total_wanted,queue_batch_size
item.1="BULLETPROOF GLASS",250,25
item.2="COMPUTER",1000,50
item.3="CONSTRUCTION COMP.",3000,100
item.4="DETECTOR COMP.",50,5
item.5="DISPLAY",250,25
item.6="EXPLOSIVES",50,5
item.7="GIRDER",2000,10011
item.8="GRAVITY COMP.",50,5
item.9="INTERIOR PLATE",3000,100
item.10="LARGE STEEL TUBES",1000,100
item.11="MEDICAL COMP.",50,5
item.12="METAL GRID",500,50
item.13="MOTOR",1000,50
item.14="POWER CELL",400,50
item.15="RADIO-COMM COMP.",50,5
item.16="REACTOR COMP",50,5
item.17="SMALL STEEL TUBE",3000,100
item.18="SOLAR CELL",250,25
item.19="STEEL PLATE",5000,200
item.20="SUPERCONDUCTOR",150,25
item.21="THRUSTER COMP.",250,25
```

The item.x lines need to start from 1 and go upwards numerically but all items do not need to be listed. The two numbers
on the end of the item.x lines is the amount you want to ensure are available, and the batch size to queue up build requests
as.

2. Tag the assembler you want things queued at with `[qhere]` - We usually have multiple assemblers where all but one are
configured to run in co-op mode, and the one that isnt is where you queue the items.

3. On any LCD where you would like a graphical status of how the script is keeping up, add `[saveitems]` to the name

4. Tag a cargo container where the produced items should all be moved into as `[saveitems]` 