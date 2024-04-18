Dismantle Tools
---------------
This script monitors for hand tools which have been acquired (usually by people dying and coming back
with just basic ones), and tries to move them over to a reserved assembler which is in deconstruct 
mode and disassembles them.

Note: This script looks for anything it can find, irrespective of grids.


Installation Instructions
=========================
1. Set the programmable block customdata to identify a tag of which assembler to send these to, and
what level of each of the welder, grinder and handdrill you want to automatically transfer for
disassembling. (See below for example)

2. Set an (usually basic) assembler to be in disassemble mode and add the tag to the item name, 
eg "[disasm] Disassembler"

3. Add script to the programmable block, recompile and run.

Example custom data - Dismantle normal grinder and handdrill, and normal+enhanced welder:
===================

[Config]
; Ask assembler to disassemble tools
; 0==ignore, 1..4 are each level of the normal,enhanced, proficient, elite etc
; Set to 0 or dont list things you dont want to dismantle
; Remember 1 == normal, 2 == ^, 3 == ^^ and 4 = ^^^
tag=disasm
welder=2
grinder=1
handdrill=1

Server Impact
=============
- Very minimal... only checks once every minute