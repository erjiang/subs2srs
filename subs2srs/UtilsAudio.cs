//  Copyright (C) 2009-2016 Christopher Brochtrup
//
//  This file is part of subs2srs.
//
//  subs2srs is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  subs2srs is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with subs2srs.  If not, see <http://www.gnu.org/licenses/>.
//
//////////////////////////////////////////////////////////////////////////////

using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using TagLib;

namespace subs2srs
{
  /// <summary>
  /// Utilities related to audio.
  /// </summary>
  public class UtilsAudio
  {
    /// <summary>
    /// Rip (and re-encode) a portion of the audio from a video file.
    /// </summary>
    public static void ripAudioFromVideo(string inFile, string stream, DateTime startTime, 
      DateTime endTime, int bitrate, string outFile, DialogProgress dialogProgress)
    {
      string audioBitrateArg = UtilsVideo.formatAudioBitrateArg(bitrate);
      string audioMapArg = UtilsVideo.formatAudioMapArg(stream);
      string timeArg = UtilsVideo.formatStartTimeAndDurationArg(startTime, endTime);

      string ffmpegAudioProgArgs = "";

      // Example format: 
      // -vn -y -i "G:\Temp\inputs.mkv" -ac 2 -map 0:1 -ss 00:03:32.420 -t 00:02:03.650 -b:a 128k -threads 0 "output.mp3" 
      ffmpegAudioProgArgs = String.Format("-vn -y -i \"{0}\" -ac 2 {1} {2} {3} -threads 0 \"{4}\"",
                                          // Video file
                                          inFile,              // {0}

                                          // Mapping
                                          audioMapArg,         // {1}

                                          // Time span
                                          timeArg,             // {2}

                                          // Bitrate
                                          audioBitrateArg,     // {3}

                                          // Output file name 
                                          outFile);            // {4}

      if (dialogProgress == null)
      {
        UtilsCommon.startFFmpeg(ffmpegAudioProgArgs, false, true);
      }
      else
      {
        UtilsCommon.startFFmpegProgress(ffmpegAudioProgArgs, dialogProgress);
      }
    }


    /// <summary>
    /// Rip (and re-encode) the entire audio from a video file.
    /// </summary>
    static public void ripAudioFromVideo(string inFile, int bitrate, string outFile)
    {
      string audioBitrateArg = UtilsVideo.formatAudioBitrateArg(bitrate);

      string ffmpegAudioProgArgs = "";

      // Example format:
      // -vn -y -i "G:\Temp\inputs.mkv" -ac 2 -b:a 128k "output.mp3"
      ffmpegAudioProgArgs = String.Format("-vn -y -i \"{1}\" -ac 2 {1} -threads 0 \"{2}\"",
                                          inFile,          // {0}
                                          audioBitrateArg, // {1}
                                          outFile);        // {2}

      UtilsCommon.startFFmpeg(ffmpegAudioProgArgs, true, true);
    }


    /// <summary>
    /// Extract an audio clip from a longer audio clip without re-encoding.
    /// </summary>
    public static void cutAudio(string fileToCut, DateTime startTime, DateTime endTime, string outFile)
    {
      string timeArg = UtilsVideo.formatStartTimeAndDurationArg(startTime, endTime);
      string audioCodecArg = UtilsVideo.formatAudioCodecArg(UtilsVideo.AudioCodec.COPY);

      string ffmpegAudioProgArgs = "";

      // Example format: 
      //-y -i "input.mp3" -ss 00:00:00.000 -t 00:00:01.900 -codec:a copy "output.mp3"
      ffmpegAudioProgArgs = String.Format("-y -i \"{0}\" {1} {2} \"{3}\"",
                                          // Input file                                          
                                          fileToCut,                             // {0}

                                          // Time span                          
                                          timeArg,                               // {1}

                                          // Audio codec
                                          audioCodecArg,                         // {2}

                                          // Output file (including full path)             
                                          outFile);                              // {3}

      UtilsCommon.startFFmpeg(ffmpegAudioProgArgs, false, true);
    }


    /// <summary>
    /// Convert audio file to another format (ex. mp3 -> wav).
    /// </summary>
    public static void convertAudioFormat(string mp3File, string outFile, int numChannels)
    {
      string ffmpegAudioProgArgs = "";

      // Examples:
      // -y -i "input.mp3"" -ac 2 -threads 0 "output.wav"
      ffmpegAudioProgArgs = String.Format("-y -i \"{0}\" -ac {1} -threads 0 \"{2}\"",                          
                                          mp3File,     // {0}
                                          numChannels, // {1}
                                          outFile);    // {2}

      UtilsCommon.startFFmpeg(ffmpegAudioProgArgs, false, true);
    }
    

    /// <summary>
    /// Tag an audio file (currently, only MP3 ID3 tags are supported).
    /// </summary>
    public static void tagAudio(string inFile, string artist, string albumTitle, 
      string songTitle, string genre, string lyrics, int track, int totalTracks)
    {
      try
      {
        TagLib.File f = TagLib.File.Create(inFile);

        f.Tag.Performers = new string[] { artist };
        f.Tag.Album = albumTitle;
        f.Tag.Title = songTitle;
        f.Tag.Genres = new string[] { genre };
        f.Tag.Track = (uint)track;
        f.Tag.TrackCount = (uint)totalTracks;

        if (lyrics.Trim() != "")
        {
          f.Tag.Lyrics = lyrics;
        }

        f.Save();
      }
      catch
      {
        // Ignore and move on
      }
    }


    /// <summary>
    /// Normalize all .mp3 files in the given directory.
    /// http://mp3gain.sourceforge.net/
    /// </summary>
    public static void normalizeAudio(string dir)
    {
      string finalDir = dir;

      if (finalDir.EndsWith(Path.DirectorySeparatorChar.ToString()))
      {
        finalDir = finalDir.TrimEnd(new char[] { Path.DirectorySeparatorChar });
      }

      string args = String.Format(@"{0} ""{1}{2}*.mp3""", 
        ConstantSettings.AudioNormalizeArgs, finalDir, Path.DirectorySeparatorChar);

      UtilsCommon.startProcess(ConstantSettings.PathNormalizeAudioExeRel, 
        ConstantSettings.PathNormalizeAudioExeFull, args);
    }




  }
}
