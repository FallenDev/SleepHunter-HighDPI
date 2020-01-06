# SleepHunter-HighDPI
Dark Ages Automation Tool - Written By: ewrogers (Silo)
Modified | Maintained - Paul Buck (FallenDev)

## Requirements
- [Dark Ages](http://www.darkages.com) Client 7.41
- [.NET Framework 4.8](https://dotnet.microsoft.com/download/dotnet-framework/thank-you/net48-web-installer) (or newer)

## Changelog
-----------------
#### Version 1.6.1 (from 1.6.0)
- Fixed some text issues within app

#### Version 1.6.0 (from 1.5.0)
- Updated .NET Framework to 4.8
- Changed .ico to differentiate between HighDPI version
- Added application manifest to enable GDI Scaling
- Forced Administration Mode for executable

#### Version 1.5.0 (from v1.4.x)
- DA Client 7.41 is now supported
- Default window size increased to 1024x768
- Minimum window size increased to 800x600
- Main toolbar icons changed to use Segoe UI Symbol font icons
- Main toolbar text labels slightly modified, now recognize accessor keys (ALT)
- Character list font sizes have been reduced
- Character list health and mana bars are slightly taller (fill)
- Character list selected indicator is now a left-aligned bar instead of entire cell highlight
- Skills tab is no longer scrollable, instead separated by Temuair/Medenia/World sub-tabs
- Spells tab is no longer scrollable, instead separated by Temuair/Medenia/World sub-tabs
- Spell queue is now always visible when not empty (no longer hidden when switching to other tabs)
- Spell queue can now be re-ordered via drag and drop (move up/down buttons have been removed)
- Spell queue now has 'remove' and 'clear all' buttons to make it more clear (DELETE key still removes selected)
- Spell queue list item font sizes have been reduced
- Spell queue list item selected indicator is now a left-aligned bar instead of entire cell highlight
- Spell queue warning indicator (for undefined spells) now flashes and has an improved tooltip message
- Spell queue rotation now defaults to singular order instead of round robin (user settings)
- Flowering tab bottom buttons changed to use Segoe UI Symbol font icons
- Flower queue can now be re-ordered via drag and drop (move up/down buttons have been removed)
- Flower queue now has 'remove' and 'clear all' buttons to make it more clear (DELETE key still removes selected)
- Flower queue list item font sizes have been reduced
- Flower queue list item layout reorganized to be more clear
- Flower queue list item selected indicator is now a left-aligned bar instead of entire cell highlight 
- Skill metadata editor list font sizes have been reduced
- Skill metadata list item selected indicator is now a left-aligned bar with background highlight
- Skill metadata buttons changed to use Segoe UI Symbol font icons
- Spell metadata editor list font sizes have been reduced
- Spell metadata list item selected indicator is now a left-aligned bar with background highlight
- Spell metadata buttons changed to use Segoe UI Symbol font icons
- Staff metadata editor list font sizes have been reduced
- Staff metadata list item layout reorganized to be more clear
- Staff metadata buttons changed to use Segoe UI Symbol font icons
- Color themes have been overhauled with over 60 new color themes based on Google Material palette
- Updated Win32 APIs for better 64-bit support
- Fixed a few UI threading issues that could cause InvalidOperation exceptions
