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
  /// Parser for Lyric (.lrc) files.
  /// </summary>
  class SubsParserLyrics : SubsParser
  {
    public SubsParserLyrics(string file, Encoding subsEncoding)
    {
      this.File = file;
      this.SubsEncoding = subsEncoding;
    }


    /// <summary>
    /// Parse the subtitle file and return a list of lines.
    /// </summary>
    override public List<InfoLine> parse()
    {
      List<string> rawLines = new List<string>(100);
      List<InfoLine> lineInfos = new List<InfoLine>(100);
      StreamReader subFile = new StreamReader(this.File, this.SubsEncoding);
      string fileLine;

      // Store all of the file's lines in a list
      while ((fileLine = subFile.ReadLine()) != null)
      {
        rawLines.Add(fileLine.Trim());
      }

      subFile.Close();

      // Extract info from each dialog line
      foreach (string rawLine in rawLines)
      {
        string curLine = rawLine;

        MatchCollection timestampMatches = Regex.Matches(curLine,
          @"\[(?<Min>\d\d):(?<Sec>\d\d)\.(?<HSec>\d\d)\]",
          RegexOptions.IgnoreCase | RegexOptions.Compiled);

        string text = Regex.Replace(curLine, @"\[(?<Min>\d\d):(?<Sec>\d\d)\.(?<HSec>\d\d)\]", 
          "", RegexOptions.Compiled).Trim();

        foreach (Match timestampMatch in timestampMatches)
        {
          if (!timestampMatch.Success)
          {
            continue;
          }

          int startTimeMin = Int32.Parse(timestampMatch.Groups["Min"].ToString().Trim());
          int startTimeSec = Int32.Parse(timestampMatch.Groups["Sec"].ToString().Trim());
          int startTimeHSec = Int32.Parse(timestampMatch.Groups["HSec"].ToString().Trim());

          DateTime startTime = new DateTime();
          startTime = startTime.AddMinutes(startTimeMin);
          startTime = startTime.AddSeconds(startTimeSec);
          startTime = startTime.AddMilliseconds(startTimeHSec * 10);
          DateTime endTime = new DateTime();

          text = text.Replace("\t", " ").Trim();

          InfoLine info = new InfoLine(startTime, endTime, text);
          lineInfos.Add(info);
        }
      }

      // Since the dialog lines don't have to be in chronological order, sort by the start time
      lineInfos.Sort();

      // Fill-in the endtimes
      for (int lineIdx = 0; lineIdx < lineInfos.Count; lineIdx++)
      {
        InfoLine infoLineCur = lineInfos[lineIdx];
        InfoLine infoLineNext = lineInfos[Math.Min(lineInfos.Count - 1, lineIdx + 1)];

        infoLineCur.EndTime = infoLineNext.StartTime;
      }

      // Make the endtime of the final lyric an arbitrary 10 seconds long
      lineInfos[lineInfos.Count - 1].EndTime = new DateTime().AddSeconds(lineInfos[lineInfos.Count - 1].StartTime.TimeOfDay.TotalSeconds + 10);

      // Remove lines that don't contains lyrics
      for (int lineIdx = lineInfos.Count - 1; lineIdx >= 0; lineIdx--)
      {
        InfoLine infoLineCur = lineInfos[lineIdx];

        // Remove blank lines
        // Remove lines with the same start and end time because it is probably non-lyric meta information
        // Remove line with a colon because it is probably non-lyric meta information
        // remove lines with a website because it is probably just an advertisement
        if ((infoLineCur.Text.Trim().Length == 0)
          || (infoLineCur.StartTime.TimeOfDay.TotalMilliseconds == infoLineCur.EndTime.TimeOfDay.TotalMilliseconds)
          || (infoLineCur.Text.Contains(":"))
          || (infoLineCur.Text.Contains("："))
          || (infoLineCur.Text.Contains("www.")))
        {
          lineInfos.RemoveAt(lineIdx);
        }
      }

      return lineInfos;
    }











  }
}