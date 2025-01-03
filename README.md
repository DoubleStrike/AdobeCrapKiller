# AdobeCrapKiller

A way to kill all the junk background processes that Adobe Creative Cloud fills your system with. Hit the Refresh button to load the list of Adobe processes, and then hit the "Kill that crap!" button to kill it. You can use the auto-refresh function to update the list every 3 seconds to make sure Adobe is not trying to use sneaky timer jobs or other techniques to start things in the background.

![ACK](https://github.com/user-attachments/assets/924d5a47-3352-4e59-941a-5832242c5f5f)

## Why?
Adobe could have just let you close things properly, but no, they want to spy on your activity and waste resources doing it.  **It's your computer - don't let others waste your resources or sell your computer activity history!**

## What does this do?
This program forcefully kills any and all Adobe programs/services running in the background. Since some apps are run as administrator by Adobe, it is recommended to run this program as administrator. If you run it normally, it will pop up a question box and ask if you want it to re-launch itself as admin. You don't have to, but be aware that all programs may not be shown or killed if running with lower privileges - that's just how Windows processes work.

When running as administrator, the title text is red and will end with "(Admin Mode)" to make it obvious to you.

## Future To-Do items
* Allow configurable timer for auto-refresh
* Allow user to enable/disable beep on auto-refresh (currently only beeps after manual refresh or kill)

## Disclaimer
This code is not related to Adobe in any way, other than working around their crappy software architecture.  I make absolutely no guarantee and offer no warranties express or implied for this software. Use at your own risk. **I encourage you to look at the code to see what I am doing, as it is really basic stuff and nothing even remotely shady.**
