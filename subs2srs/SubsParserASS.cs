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
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace subs2srs
{
  /// <summary>
  /// Parser for Advanced Substation Alpha (.ass) files.
  /// </summary>
  class SubsParserASS : SubsParser
  {
    public SubsParserASS(WorkerVars workerVars, string file, Encoding subsEncoding, int subsNum)
    {
      this.WorkerVars = workerVars;
      this.File = file;
      this.SubsEncoding = subsEncoding;
      this.SubsNum = subsNum;
    }


    /// <summary>
    /// Parse the subtitle file and return a list of lines.
    /// </summary>
    override public List<InfoLine> parse()
    {
      List<string> rawLines = new List<string>(2000);
      List<InfoLine> lineInfos = new List<InfoLine>(2000);
      StreamReader subFile = new StreamReader(this.File, this.SubsEncoding);
      string fileLine;

      // Store all of the file's lines in a list
      while ((fileLine = subFile.ReadLine()) != null)
      {
        rawLines.Add(fileLine.Trim());
      }

      subFile.Close();

      // Get the regex to use with the dialog lines
      string assDialogRegex = getAssDialogRegex(rawLines);

      // Extract info from each dialog line
      foreach (string rawLine in rawLines)
      {
        Match lineMatch = Regex.Match(rawLine,
          assDialogRegex, RegexOptions.IgnoreCase | RegexOptions.Compiled);

        if (!lineMatch.Success)
          continue;

        string rawStartTime = lineMatch.Groups["StartTime"].ToString().Trim();
        string rawEndTime = lineMatch.Groups["EndTime"].ToString().Trim();
        string actor = lineMatch.Groups["Name"].ToString().Trim();
        string text = lineMatch.Groups["Text"].ToString().Trim();

        // Don't parse styled lines
        if (Settings.Instance.Subs[SubsNum - 1].RemoveStyledLines && text.StartsWith("{"))
        {
          continue;
        }

        if ((this.WorkerVars != null) && (this.WorkerVars.ProcessingType != WorkerVars.SubsProcessingType.Dueling))
        {
          // Remove ass-style newlines ("\N")
          text = text.Replace("\\N", " ");
          text = text.Replace("\\n", " ");
        }

        // Remove styling embedded within lines
        text = Regex.Replace(text, "{.*?}", "");

        // Remove tabs
        text = Regex.Replace(text, "\t", " ").Trim();

        if (text == "")
        {
          // No text - try next line
          continue;
        }

        DateTime startTime = parseTime(rawStartTime);
        DateTime endTime = parseTime(rawEndTime);
        InfoLine info = new InfoLine(startTime, endTime, text, actor);
        lineInfos.Add(info);
      }

      // Since the dialog lines don't have to be in chronological order, sort by the start time
      lineInfos.Sort();

      return lineInfos;
    }


    /// <summary>
    /// Parse a .ass style timestamp.
    /// </summary>
    private DateTime parseTime(string rawTime)
    {
      DateTime time = new DateTime();

      // Format: 
      // "hour:min:sec.hsec" (0:00:00.00)

      Match match = Regex.Match(rawTime,
        @"^(?<Hours>\d):(?<Mins>\d\d):(?<Secs>\d\d).(?<HSecs>\d\d)$", RegexOptions.IgnoreCase | RegexOptions.Compiled);

      if (!match.Success)
      {
        throw new Exception("Incorrect time format detected: " + rawTime
          + "\nTo Fix:\n Open the subtitle file with Aegisub and select File | Save Subtitles");
      }

      try
      {
        time = time.AddHours(Int32.Parse(match.Groups["Hours"].ToString().Trim()));
        time = time.AddMinutes(Int32.Parse(match.Groups["Mins"].ToString().Trim()));
        time = time.AddSeconds(Int32.Parse(match.Groups["Secs"].ToString().Trim()));
        time = time.AddMilliseconds(Int32.Parse(match.Groups["HSecs"].ToString().Trim()) * 10);
      }
      catch
      {
        throw new Exception("Invalid time format");
      }

      return time;
    }


    /// <summary>
    /// Create a regex to match against the dialog lines.
    /// </summary>
    private string getAssDialogRegex(List<string> lines)
    {
      string formatLine = "";
      bool eventsEncountered = false;
      string regexFormat = "^Dialogue:";

      // Find and store the format line in the [Events] section
      foreach (string line in lines)
      {
        if (line.StartsWith("[Events]"))
        {
          eventsEncountered = true;
        }

        if (eventsEncountered && line.StartsWith("Format:"))
        {
          formatLine = line;
          break;
        }
      }

      // Create a regex based on the format
      // Example format line:
      // "Format: Layer, Start, End, Style, Name, MarginL, MarginR, MarginV, Effect, Text"
      // "Format: Layer, Start, End, Style, Actor, MarginL, MarginR, MarginV, Effect, Text"
      // "Format: Marked, Start, End, Style, Name, MarginL, MarginR, MarginV, Effect, Text"

      string[] formatItems = formatLine.Split(new char[] {','});

      foreach (string item in formatItems)
      {
        if (item.Contains("Layer"))
        {
          regexFormat += "(?<Layer>.*?),";
        }
        else if (item.Contains("Marked"))
        {
          regexFormat += "(?<Marked>.*?),";
        }
        else if (item.Contains("Start"))
        {
          regexFormat += "(?<StartTime>.*?),";
        }
        else if (item.Contains("End"))
        {
          regexFormat += "(?<EndTime>.*?),";
        }
        else if (item.Contains("Style"))
        {
          regexFormat += "(?<Style>.*?),";
        }
        else if (item.Contains("Name") || item.Contains("Actor"))
        {
          regexFormat += "(?<Name>.*?),";
        }
        else if (item.Contains("MarginL"))
        {
          regexFormat += "(?<MarginL>.*?),";
        }
        else if (item.Contains("MarginR"))
        {
          regexFormat += "(?<MarginR>.*?),";
        }
        else if (item.Contains("MarginV"))
        {
          regexFormat += "(?<MarginV>.*?),";
        }
        else if (item.Contains("Effect"))
        {
          regexFormat += "(?<Effect>.*?),";
        }
      }

      // The "Text" format item is always last (to allow for commas)
      regexFormat += "(?<Text>.*)";

      return regexFormat;
    }


  }
}
