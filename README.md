# SigmaNotificationApp
A small app to check if a Sigma speedometer is available in its docking station and to read its data.

## Prerequisites
- Windows 10/11 64bit
- Sigma Sport Docking Station TL2012
- Sigma Sport speedometer(*)
- BikeDB2026 to store the data (recommended, can be found on my Github account)

(*) currently BC12.12, BC16.12 and BC16.12 STS are supported

## Installation
Directory: \bin\Release\v1.0.0.0 (initial release)
Just copy the directory and run the exe from any location that you want.
Or: use setup.exe to create an entry in the start menu and allow Windows to uninstall the app.
Directory \publish\setup.exe -> install the latest version

## Features
- Can be minimized and waits idle beneath the clock on a Windows system.
- Languages: German and English
- Help.html which can be browsed via the help menu. Available only in German, but with much more information about the features.
- Stores data in a JSON file, optimized to be imported into BikeDB2026, but could also be used for other applications.

## Screenshots
![German main window of the application](app_german.png)
![English main window of the application](app_english.png)

The DateTimePicker is dependent on the Operating System used. On my Windows 11 it will always show the German date. If you've got an English OS, that output may vary.
OUTDATED: The DateTimePicker now also shows the time

## Console app
In the directory ConsoleApp you can also find the basic C#-code to access the cradle aka Docking Station. 
This was used for debugging and learn the protocol of the speedometer. 
It might be useful for anyone trying to program their own application.

## Compatibility
In 2026 BikeDB Trax was introduced. This is primarily a GPS-tracker to record routes. But it can also be used as a speedometer on an Android smartphone. 
BikeDB Trax uses the same JSON file format as the SigmaNotificationApp. 
Thus, if you don't have a Sigma speedometer, you can use BikeDB Trax instead and still get ride data that can be imported into BikeDB 2026.
In this case you wouldn't need the SigmaNotificationApp, although results might not be as consistent and reliable as with using a bike computer.

## Credits
The original work was done by Alfonso Martone. Check out his original work at:
https://gitlab.com/ciofeca/sdsdata

## License
Because Alfonso used it, I publish my work also under the MIT License. Feel free to do what you want.

# Copyright
Besides the license, this application is the original work of Dipl.-Biol. Björn Zedroßer, Cologne, Germany.
(c) 2025-2026