subs2srs for Linux
==================

I'm working on getting this to run on Linux. So far the basic features seem
to work.

To run on Linux, you will need to:

* Install a recent-ish version of Mono. (Mono 4.x is known to do poorly with this program.)
* Install "Microsoft Sans Serif" font.
* Install various video tools. The binaries that this program can call are:
  * mkvinfo
  * mkvextract
  * ffmpeg
  * mp3gain

Version
-------
Currently this is based off of v29.6 from Sourceforge, which was the latest
version as of December 2016. There is a version v29.7 which seems to only
include newer build of ffmpeg, but this version requires that you provide
your own ffmpeg.
