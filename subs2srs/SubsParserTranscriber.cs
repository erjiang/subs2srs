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
using System.Text;
using System.Xml;


namespace subs2srs
{
  /// <summary>
  /// Parser for Transcriber (.trs) files.
  /// </summary>
  class SubsParserTranscriber: SubsParser
  {
    public SubsParserTranscriber(string file, Encoding subsEncoding)
    {
      this.File = file;
      this.SubsEncoding = subsEncoding;
    }


    /// <summary>
    /// Parse the subtitle file and return a list of lines.
    /// </summary>
    override public List<InfoLine> parse()
    {
      List<InfoLine> lineInfos = new List<InfoLine>();
      StreamReader subFile = new StreamReader(this.File, this.SubsEncoding);

      XmlTextReader xmlReader = new XmlTextReader(subFile);
      xmlReader.XmlResolver = null; // Ignore dtd

      DateTime startTime = new DateTime();
      DateTime endTime = new DateTime();
      DateTime curTime = new DateTime();
      DateTime turnEndTime = new DateTime();
      string dialogText = "";
      int syncCount = 0;

      while (xmlReader.Read())
      {
        if (xmlReader.NodeType == XmlNodeType.Element)
        {
          if (xmlReader.Name.ToLower() == "sync")
          {
            // Example sync: <Sync time="1.027"/>
            syncCount++;

            string timeStr = xmlReader.GetAttribute("time");

            try
            {
              curTime = new DateTime();
              curTime = curTime.AddSeconds(UtilsLang.toDouble(timeStr));
            }
            catch (Exception e1)
            {
              throw new Exception(String.Format("Incorrect time format detected: {0}\n\n{1}", timeStr, e1));
            }

            if (syncCount == 1)
            {
              startTime = curTime;
            }
            else if (syncCount == 2)
            {
              endTime = curTime;
            }
            else
            {
              startTime = endTime;
              endTime = curTime;
            }

            // If this is not the first time and the length isn't blank
            if ((syncCount > 1) && (dialogText.Length != 0))
            {
              lineInfos.Add(this.createLineInfo(dialogText, startTime, endTime));
              dialogText = "";
            }
          }
          else if(xmlReader.Name.ToLower() == "turn")
          {
            // Example turn: <Turn speaker="spk1" startTime="2.263" endTime="25.566">

            string timeStr = xmlReader.GetAttribute("endTime");

            turnEndTime = new DateTime();
            turnEndTime = curTime.AddSeconds(UtilsLang.toDouble(timeStr));
          }
        }
        else if(xmlReader.NodeType == XmlNodeType.Text)
        {
          dialogText += xmlReader.Value;
        }
        else if (xmlReader.NodeType == XmlNodeType.EndElement)
        {
          // This section handles the final line before the </turn>.
          // We need to do this because there isn't a Sync tag after the final line.

          if (xmlReader.Name.ToLower() == "turn") // </Turn>
          {
            const double MAX_FINAL_LINE_DURATION = 10.0;
            startTime = endTime;
            endTime = turnEndTime;

            // Calculate the difference between the startTime and turnEndTime and make sure that
            // it is a reasonable length. We do this because the turn end time isn't necessarily
            // the end of the line. If it is not a reasonable length, set it to MAX_FINAL_LINE_DURATION.
            double diffTime = endTime.TimeOfDay.TotalSeconds - startTime.TimeOfDay.TotalSeconds;

            if (diffTime > MAX_FINAL_LINE_DURATION)
            {
              endTime = startTime;
              endTime = endTime.AddSeconds(MAX_FINAL_LINE_DURATION);
            }

            if ((syncCount >= 1) && (dialogText.Length != 0))
            {
              lineInfos.Add(this.createLineInfo(dialogText, startTime, endTime));
              dialogText = "";
            }

            // If needed, restore endtime to the turnEndTime
            if (diffTime >= MAX_FINAL_LINE_DURATION)
            {
              endTime = turnEndTime;
            }
          }
        }
      }

      subFile.Close();

      // Since the dialog lines don't have to be in chronological order, sort by the start time
      lineInfos.Sort();

      return lineInfos;
    }


    /// <summary>
    /// Create a line info object based on the given parameters.
    /// </summary>
    private InfoLine createLineInfo(string lineText, DateTime startTime, DateTime endTime)
    {
      lineText = lineText.Replace("\r\n", " ");
      lineText = lineText.Replace("\r", " ");
      lineText = lineText.Replace("\n", " ");
      lineText = lineText.Replace("\t", " ");
      lineText = lineText.Trim();

      InfoLine info = new InfoLine(startTime, endTime, lineText);

      return info;
    }


  }
}
