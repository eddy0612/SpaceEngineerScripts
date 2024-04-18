MoveBottles
-----------

This is a simple script which runs periodically and moves any oxygen or hydrogen bottles it can find over to a set of nominated oxygen or hydrogen 
tanks (or O2H2 Generator). Anything with an inventory with the tag [LOCKED] is ignored. Be aware as coded it will grab from anywhere it can and is
not filtered to the current grid. This would be a trivial change


Instructions
------------

1. Create a programmable block, name it something like `[MOVEBOTTLESPGM]`. Add custom data as follows - the value of the tag can be anything but you need to use it consistently everywhere as a prefix

```
[config]
tag=MOVEBOTTLES
```

2. Idenitfy one or move of both oxygen and hydrogen tanks, or O2/H2 generators, and add to their name `[MOVEBOTTLES]`
3. Add the script to the programmable block, recompile and run.
