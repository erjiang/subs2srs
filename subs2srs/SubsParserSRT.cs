﻿//  Copyright (C) 2009-2016 Christopher Brochtrup
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
  /// Parser for Subrip (.srt) files.
  /// </summary>
  class SubsParserSRT : SubsParser
  {

    /// <summary>
    /// States of the parser's state machine.
    /// </summary>
    private enum ParseStep
    {
      LineNum = 0,
      Time,
      Text
    }

    public SubsParserSRT(string file, Encoding subsEncoding)
    {
      this.File = file;
      this.SubsEncoding = subsEncoding;
    }


    /// <summary>
    /// Parse the subtitle file and return a list of lines.
    /// </summary>
    override public List<InfoLine> parse()
    {
      List<InfoLine> lineInfos = new List<InfoLine>(2000);
      StreamReader subFile = new StreamReader(this.File, this.SubsEncoding);
      string rawLine;
      ParseStep parseStep = ParseStep.LineNum;
      Match match;
      string rawStartTime = "";
      string rawEndTime = "";
      string lineText = "";

      // Fill in lineInfos
      while ((rawLine = subFile.ReadLine()) != null)
      {
        switch (parseStep)
        {
          case ParseStep.LineNum:
            // Skip past line number and anything before it
            match = Regex.Match(rawLine, @"^\d+$", RegexOptions.IgnoreCase | RegexOptions.Compiled);
    
            if (match.Success)
            {
              parseStep = ParseStep.Time;
            }

            continue;

          case ParseStep.Time:
            // Match time
            match = Regex.Match(rawLine, @"^(?<StartTime>.*?)\s-->\s(?<EndTime>.*)$", RegexOptions.IgnoreCase | RegexOptions.Compiled);

            if (!match.Success)
            {
              // Something went wrong - there's something between line number and time info
              continue;
            }

            rawStartTime = match.Groups["StartTime"].ToString().Trim();
            rawEndTime = match.Groups["EndTime"].ToString().Trim();

            parseStep = ParseStep.Text;

            continue;

          case ParseStep.Text:
            // Match text
            match = Regex.Match(rawLine, @"^(?<Text>.*)$", RegexOptions.IgnoreCase | RegexOptions.Compiled);

            string text = match.Groups["Text"].ToString().Trim();

            // Keep parsing subs until a blank line is encountered
            if (text == "")
            {
              parseStep = ParseStep.LineNum;
              lineInfos.Add(this.createLineInfo(lineText, rawStartTime, rawEndTime));
              lineText = "";
            }
            else
            {
              // Add space between each line of a multiline subtitle
              lineText += text + " ";
            }

            continue; 

          default:
            // Should never get here
            break;
        }
      }

      // Handle the last line in the file
      if (lineText.Trim().Length > 0)
      {
        lineInfos.Add(this.createLineInfo(lineText, rawStartTime, rawEndTime));
      }

      lineInfos.Sort();

      return lineInfos;
    }


    /// <summary>
    /// Create a line info object based on the given parameters.
    /// </summary>
    private InfoLine createLineInfo(string lineText, string rawStartTime, string rawEndTime)
    {
      DateTime startTime = this.parseTime(rawStartTime);
      DateTime endTime = this.parseTime(rawEndTime);

      lineText = lineText.Replace("\t", " ");
      lineText = Regex.Replace(lineText, "</?[ibuIBU]>", "").Trim();

      InfoLine info = new InfoLine(startTime, endTime, lineText);

      return info;
    }


    /// <summary>
    /// Parse a .srt formmated timestamp.
    /// </summary>
    private DateTime parseTime(string rawTime)
    {
      DateTime time = new DateTime();

      // Format: 
      // "hour:min:sec,msec" (00:00:00,000)

      Match match = Regex.Match(rawTime,
        @"^(?<Hours>\d\d):(?<Mins>\d\d):(?<Secs>\d\d),(?<MSecs>\d\d\d)$", RegexOptions.IgnoreCase | RegexOptions.Compiled);

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
        time = time.AddMilliseconds(Int32.Parse(match.Groups["MSecs"].ToString().Trim()));
      }
      catch
      {
        throw new Exception("Invalid time format");
      }

      return time;
    }

  }
}
