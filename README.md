subs2srs for Linux
==================

I'm working on getting this to run on Linux. So far the basic features seem
to work.

To run on Linux, you will need to:

* Install a recent-ish version of Mono. (Mono 4.x is known to do poorly with this program.)
* Install "Microsoft Sans Serif" font.
* Install various video tools. The binaries that this program can call are:
  * mkvinfo and mkvextract (part of mkvtoolnix)
  * ffmpeg (includes ffplay, required for audio preview)
  * mp3gain

Version
-------
Currently this is based off of v29.6 from Sourceforge, which was the latest
version as of December 2016. There is a version v29.7 which seems to only
include newer build of ffmpeg, but this version requires that you provide
your own ffmpeg.

Build
-----
Prerequisites (Debian/Ubuntu):

- mono-complete (includes WinForms, libgdiplus, msbuild)
- ffmpeg
- mkvtoolnix (for mkvinfo/mkvextract)
- mp3gain
- Microsoft core fonts (for “Microsoft Sans Serif”)

Example install:

- sudo apt update
- sudo apt install mono-complete ffmpeg mkvtoolnix mp3gain ttf-mscorefonts-installer

Build with Mono (from repo root):

- msbuild subs2srs.sln /p:Configuration=MonoRelease
  - On older Mono: xbuild subs2srs.sln /p:Configuration=MonoRelease

Artifacts:

- subs2srs/bin/MonoRelease/subs2srs.exe

Run
---
- mono subs2srs/bin/MonoRelease/subs2srs.exe

Notes
-----
- You can also build a debug variant: msbuild subs2srs.sln /p:Configuration=MonoDebug
- Ensure ffmpeg, mkvinfo, mkvextract, and mp3gain are in your PATH.
  - ffplay must be available in PATH for the Preview Audio feature.
- If UI rendering looks off, verify libgdiplus is present (installed with mono-complete) and run under X11.
