JumpDrive
=========

This is a very simple script which outputs to a display the current status of any Jump drive it finds on the local grid onto an LCD. It was started to
be a lot more functional but I then discovered that the API in Space Engineers to interface with JumpDrives is extremely limited, and this was all I could
do for now

Source available via https://github.com/eddy0612/SpaceEngineerScripts

Instructions
------------

1. Create a programmable block, name it something like `[JUMPDRIVEPGM]`. Add custom data as follows - the value of the tag can be anything but you need to use it consistently everywhere as a prefix

```
[config]
tag=JUMPDRIVE
```

2. Create an LCD, change its name to `[JUMPDRIVE] JumpDrive lcd`  (only the tag [..] bit is important)
3. Add the script to the programmable block, recompile and run.
