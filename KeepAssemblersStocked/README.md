Keep Assemblers stocked with ingots
-----------------------------------
We had a few problems that assemblers werent pulling the ores they needed as fast as they could, for example
- problem whereby ingots were being pushed into assemblers and filling their inventory up so they couldnt pull more
- all the ore being in one assembler, and the other wouldnt pull ore from another assembler
- times when it was really slow pulling ore, not clear why

Installation
------------
1. Put script in programmable block
2. Add in custom data the tag for this script, eg
[custom]
tag=ingots
3. Tag all the cargo containers you are happy for the ingots to be moved to with [ingots] somewhere in the name
4. Tag all the assemblers you want kept stocked with ingots with [ingots] somewhere in the name
5. Anything you do not want things moved from/to can be tagged with [LOCKED]
6. Recompile and run the script

This script does 2 things

1. Pulls all ingots from any non-assembler, non-reactor into a set of cargo containers 

2. Ensures the assemblers are kept with min-500 and max-700 ingots of each ore type


Notes...
- It wouldnt be difficult to make the min/max configurable, potentially even per-ore type