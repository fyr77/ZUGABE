# ZUGABE
This tool automates much of the installation of h-encore.

[![Project Status: Active – The project has reached a stable, usable state and is being actively developed.](http://www.repostatus.org/badges/latest/active.svg)](http://www.repostatus.org/#active)

## About
This tool automates the install process of thefl0w's h-encore exploit for PS Vita, at least everything that can be done on the PC. 
Since I can't code in C or C++, I was unable to use OpenCMA as a backend, so I used QCMA. It's automatically downloaded and does not require to be installed. 
Configuration of QCMA is done via registry edits. The application also detects existing QCMA installations and restores the original settings after the exploit was installed.

**Attention: This tool only supports connection of the PSVita using Wi-Fi!**

## Usage
Download from the releases section, extract it and run the executable.

## Building
Build using Visual Studio 2017, this was tested. Other C# IDEs might work as well.

## Todo
- Translations
- Mac/Linux Compatibilty
- Fix bugs (you tell me!)

## Thanks
 - thefl0w for h-encore and all their work in the vita scene
 - yifanlu for psvimgtools and all their other work in the vita scene
 - mmozeiko for pkg2zip
 - noahc3 for auto-h-encore
 - xxyz for pngshot
 - All the contributors of the vita hacking scene
