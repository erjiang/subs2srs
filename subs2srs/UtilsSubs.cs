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
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Text;

namespace subs2srs
{
  /// <summary>
  /// Utilities related to subtitles.
  /// </summary>
  class UtilsSubs
  {

    /// <summary>
    /// Calculate the overlap for a line in Subs1 with timings (t1s, t1e) to a line in Subs2 with timings (t2s, t2e).
    /// This will be a number between 0 (no overlap) and 1 (perfect overlap). If no overlap exists, a negative number
    /// will be returned. The more negative it is, the more it didn't overlap.
    /// </summary>
    public static double getOverlap(DateTime t1s, DateTime t1e, DateTime t2s, DateTime t2e)
    {
      double t1s_ms = t1s.TimeOfDay.TotalMilliseconds;
      double t1e_ms = t1e.TimeOfDay.TotalMilliseconds;
      double t2s_ms = t2s.TimeOfDay.TotalMilliseconds;
      double t2e_ms = t2e.TimeOfDay.TotalMilliseconds;

      double overlap = (Math.Min(t1e_ms, t2e_ms) - Math.Max(t1s_ms, t2s_ms)) / (t1e_ms - t1s_ms);

      return overlap;
    }


    /// <summary>
    /// Get the midpoint of the two provided times.
    /// </summary>
    public static DateTime getMidpointTime(DateTime startTime, DateTime endTime)
    {
      TimeSpan midTimeSpan = endTime.Subtract(startTime);
      DateTime midTime = new DateTime();

      midTime = midTime.AddMilliseconds(midTimeSpan.TotalMilliseconds * 0.5 + startTime.TimeOfDay.TotalMilliseconds);

      return midTime;
    }


    /// <summary>
    /// Get the difference/duration between the two provided times.
    /// </summary>
    public static DateTime getDurationTime(DateTime startTime, DateTime endTime)
    {
      TimeSpan durationTimeSpan = endTime.Subtract(startTime);
      DateTime durationTime = new DateTime();

      durationTime = durationTime.AddMilliseconds(Math.Max(0, durationTimeSpan.TotalMilliseconds));

      return durationTime;
    }


    /// <summary>
    /// Apply padding (in milliseconds) to the given time.
    /// </summary>
    public static DateTime applyTimePad(DateTime time, int pad)
    {
      DateTime paddedTime = time;

      if (time.TimeOfDay.TotalMilliseconds + pad >= 0)
      {
        paddedTime = time.AddMilliseconds(pad);
      }
      else
      {
        paddedTime = new DateTime();
      }

      return paddedTime;
    }


    /// <summary>
    /// Apply a time shift to the given time.
    /// </summary>
    public static DateTime shiftTiming(DateTime time, int shift)
    {
      return applyTimePad(time, shift);
    }


    /// <summary>
    /// Create a DateTime object from the provided "h:mm:ss: formatted string.
    /// </summary>
    public static DateTime stringToTime(string timeStr)
    {
      DateTime time = new DateTime();
      Match timeMatch = Regex.Match(timeStr, @"^(?<Hours>\d):(?<Mins>[0-5]\d):(?<Secs>[0-5]\d)$");

      if (!timeMatch.Success)
      {
        throw new Exception("Invalid time format in span (No Match):" + timeStr);
      }

      try
      {
        time = time.AddHours(Int32.Parse(timeMatch.Groups["Hours"].ToString().Trim()));
      }
      catch
      {
        throw new Exception("Invalid time format in span (parsing hours):" + timeStr);
      }

      try
      {
        time = time.AddMinutes(Int32.Parse(timeMatch.Groups["Mins"].ToString().Trim()));
      }
      catch
      {
        throw new Exception("Invalid time format in span (parsing mins):" + timeStr);
      }

      try
      {
        time = time.AddSeconds(Int32.Parse(timeMatch.Groups["Secs"].ToString().Trim()));
      }
      catch
      {
        throw new Exception("Invalid time format in span (parsing secs):" + timeStr);
      }

      return time;
    }


    /// <summary>
    /// Create a string from a DateTime object in "h:mm:ss: format.
    /// </summary>
    public static string timeToString(DateTime date)
    {
      return String.Format("{0:00.}", date.Hour) + ":" + String.Format("{0:00.}", 
        date.Minute) + ":" + String.Format("{0:00.}", date.Second);
    }


    /// <summary>
    /// Get subtitle parser to use for the provided subtitle file.
    /// </summary>
    public static SubsParser getSubtitleParserType(WorkerVars workerVars, string filename, 
      int stream, int episode, int subsNum, Encoding subsEncoding)
    {
      SubsParser parser;
      string ext = filename.Substring(filename.LastIndexOf("."));
      ext = ext.ToLower();

      if (ext == ".ass" || ext == ".ssa")
      {
        parser = new SubsParserASS(workerVars, filename, subsEncoding, subsNum);
      }
      else if (ext == ".srt")
      {
        parser = new SubsParserSRT(filename, subsEncoding);
      }
      else if (ext == ".sub" || ext == ".idx")
      {
        parser = new SubsParserVOBSUB(workerVars, filename, stream, episode, subsNum);
      }
      else if (ext == ".lrc")
      {
        parser = new SubsParserLyrics(filename, subsEncoding);
      }
      else if (ext == ".trs")
      {
        parser = new SubsParserTranscriber(filename, subsEncoding);
      }
      else
      {
        parser = null;
      }

      return parser;
    }


    /// <summary>
    /// Does the expanded file pattern contain vobsubs? 
    /// </summary>
    public static bool filePatternContainsVobsubs(string filePattern)
    {
      bool containsVobsubs = false;

      string[] subsFiles = UtilsCommon.getNonHiddenFiles(filePattern);

      foreach(string file in subsFiles)
      {
        string ext = file.Substring(file.LastIndexOf(".")).ToLower();

        if (ext == ".idx")
        {
          containsVobsubs = true;
          break;
        }
      }
 
      return containsVobsubs;
    }


    /// <summary>
    /// In the provided file pattern have a corresponding .sub file for each .idx file encountered?
    /// </summary>
    public static bool isVobsubFilePatternCorrect(string filePattern)
    {
      bool isCorrect = false;
      int numIdx = 0;
      int numSub = 0;

      string[] subsFiles = UtilsCommon.getNonHiddenFiles(filePattern);

      foreach (string file in subsFiles)
      {
        string ext = file.Substring(file.LastIndexOf(".")).ToLower();

        if (ext == ".idx")
        {
          numIdx++;

          string fileNoExt = file.Substring(0, file.LastIndexOf("."));
          string subFile = fileNoExt + ".sub";

          if (File.Exists(subFile))
          {
            numSub++;
          }
          else
          {
            break;
          }
        }
      }

      if ((numIdx > 0) && (numIdx == numSub))
      {
        isCorrect = true;
      }
      

      return isCorrect;
    }


    /// <summary>
    /// Get number of subtitles or IDX/SUB pairs for the provided file pattern.
    /// </summary>
    public static int getNumSubsFiles(string filePattern)
    {
      int numSub = 0;
     
      string[] subsFiles = UtilsCommon.getNonHiddenFiles(filePattern);

      foreach (string file in subsFiles)
      {
        if (isSupportedSubtitleFormat(file))
        {
          numSub++;
        }
      }

      return numSub;
    }


    /// <summary>
    /// Get list of subtitle files for the provided file pattern.
    /// </summary>
    public static List<string> getSubsFiles(string filePattern)
    {
      List<string> subsFiles = new List<string>();

      string[] allFiles = UtilsCommon.getNonHiddenFiles(filePattern);

      foreach (string file in allFiles)
      {
        if (isSupportedSubtitleFormat(file))
        {
          subsFiles.Add(file);
        }
      }

      return subsFiles;
    }


    /// <summary>
    /// Is the provided subtitle file a format that subs2srs supports?
    /// </summary>
    public static bool isSupportedSubtitleFormat(string file)
    {
      string ext = file.Substring(file.LastIndexOf(".")).ToLower();
      bool isSuppported = false;

      if ((ext == ".srt") || (ext == ".ass") || (ext == ".ssa") || (ext == ".idx") || (ext == ".lrc") || (ext == ".trs"))
      {
        isSuppported = true;
      }

      return isSuppported;
    }


    /// <summary>
    /// Format a color in .ass subtitle format. Format: &HAABBGGRR.
    /// </summary>
    public static string formatAssColor(Color color, int alpha)
    {
      string outColorStr = "";

      outColorStr = String.Format("&H{0:X2}{1:X2}{2:X2}{3:X2}",
        alpha, color.B, color.G, color.R);

      return outColorStr;
    }


    /// <summary>
    /// Format a time in .ass subtitle format. Example: 0:00:36.16.
    /// </summary>
    public static string formatAssTime(DateTime time)
    {
      string timeAss = String.Format("{0}:{1:00.}:{2:00.}.{3:00.}",
                      (int)time.TimeOfDay.TotalHours,
                      (int)time.TimeOfDay.Minutes,
                      (int)time.TimeOfDay.Seconds,
                      (int)(time.TimeOfDay.Milliseconds / 10));

      return timeAss;
    }


    /// <summary>
    /// Extract the image file(s) from the provided Vobsub subtitle text line.
    /// </summary>
    public static List<string> extractVobsubFilesFromText(string text)
    {
      List<string> images = new List<string>();

      UtilsName name = new UtilsName(Settings.Instance.DeckName, 0, 0, new DateTime(),
        Settings.Instance.VideoClips.Size.Width, Settings.Instance.VideoClips.Size.Height);
      string prefixStr = name.createName(ConstantSettings.SrsVobsubFilenamePrefix, 0, 0, new DateTime(), new DateTime(), "", "");
      string suffixStr = name.createName(ConstantSettings.SrsVobsubFilenameSuffix, 0, 0, new DateTime(), new DateTime(), "", "");

      try
      {
        // Multiple vobsub image can be shown in a single line, so extract each image
        // and concatenate them before displaying.
        MatchCollection matches = Regex.Matches(text,
            prefixStr + "(?<VobsubImage>[\\w.-]*)" + suffixStr,
            RegexOptions.Compiled);

        foreach (Match match in matches)
        {
          images.Add(match.Groups["VobsubImage"].ToString().Trim());
        }
      }
      catch
      {
        return images;
      }

      return images;
    }


    /// <summary>
    /// Get the total number of lines from all episodes.
    /// </summary>
    public static int getTotalLineCount(List<List<InfoCombined>> combinedAll)
    {
      int totalLines = 0;

      // Get the total line count
      foreach (List<InfoCombined> combArray in combinedAll)
      {
        totalLines += combArray.Count;
      }

      return totalLines;
    }


    /// <summary>
    /// Get the last time stamp of all lines in all episodes.
    /// </summary>
    public static DateTime getLastTime(List<List<InfoCombined>> combinedAll)
    {
      DateTime lastTime = new DateTime();

      foreach (List<InfoCombined> combArray in combinedAll)
      {
        foreach (InfoCombined info in combArray)
        {
          if (info.Subs1.EndTime.TimeOfDay.TotalMilliseconds > lastTime.TimeOfDay.TotalMilliseconds)
          {
            lastTime = new DateTime();
            lastTime = lastTime.AddMilliseconds(info.Subs1.EndTime.TimeOfDay.TotalMilliseconds);
          }
        }
      }

      return lastTime;
    }


  }
}
