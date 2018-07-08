# ZUGABE

**This project is inactive for now, since I'm working on a tool for Mac and Linux. Please use [auto-h-encore](https://github.com/noahc3/auto-h-encore) for Windows.**

This tool automates much of the installation of h-encore.

[![Project Status: Inactive – The project has reached a stable, usable state but is no longer being actively developed; support/maintenance will be provided as time allows.](http://www.repostatus.org/badges/latest/inactive.svg)](http://www.repostatus.org/#inactive)

## About
This tool automates the install process of thefl0w's h-encore exploit for PS Vita, at least everything that can be done on the PC. 
Since I can't code in C or C++, I was unable to use OpenCMA as a backend, so I used QCMA. It's automatically downloaded and does not require to be installed. 
Configuration of QCMA is done via registry edits. The application also detects existing QCMA installations and restores the original settings after the exploit was installed.

**Attention: This tool only supports connection of the PSVita using Wi-Fi, therefore it's currently only compatible with PSVita/PSTV on Firmware 3.68!**

## Usage
Download from the releases section, extract it and run the executable.

## Building
Build using Visual Studio 2017, this was tested. Other C# IDEs might work as well.

## Todo
- Mac/Linux compatibility
- Fix bugs (you tell me!)

## Thanks
 - thefl0w for h-encore and all their work in the vita scene
 - yifanlu for psvimgtools and all their other work in the vita scene
 - mmozeiko for pkg2zip
 - noahc3 for auto-h-encore
 - xxyz for pngshot
 - All the contributors of the vita hacking scene
