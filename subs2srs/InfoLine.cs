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
using System.Text;

namespace subs2srs
{
  /// <summary>
  /// Represents a single subtitle line.
  /// </summary>
  [Serializable]
  public class InfoLine : IComparable<InfoLine>
  {
    private DateTime startTime;
    private DateTime endTime;
    private string text;
    private string actor;


    /// <summary>
    /// The start time of the line.
    /// </summary>
    public DateTime StartTime
    {
      get { return startTime; }
      set { startTime = value; }
    }

    /// <summary>
    /// The end time of the line.
    /// </summary>
    public DateTime EndTime
    {
      get { return endTime; }
      set { endTime = value; }
    }

    /// <summary>
    /// The actual subtitle text. For Vobsubs, it's the file name of the extracted image file for this line.
    /// </summary>
    public string Text
    {
      get { return text; }
      set { text = value; }
    }

    /// <summary>
    /// Actor is a field unique to .ass subtitles.
    /// </summary>
    public string Actor
    {
      get { return actor; }
      set { actor = value; }
    }

    /// <summary>
    /// Constructor.
    /// </summary>
    public InfoLine()
    {
      this.startTime = new DateTime();
      this.endTime = new DateTime();
      this.text = "";
      this.actor = "";
    }

    public InfoLine(DateTime startTime, DateTime endTime, string text)
    {
      this.startTime = startTime;
      this.endTime = endTime;
      this.text = text;
      this.actor = "";
    }

    public InfoLine(DateTime startTime, DateTime endTime, string text, string actor)
    {
      this.startTime = startTime;
      this.endTime = endTime;
      this.text = text;
      this.actor = actor;
    }


    /// <summary>
    /// Compare lines based on their Start Times.
    /// </summary>
    public int CompareTo(InfoLine other)
    {
      return StartTime.CompareTo(other.StartTime);
    }

    public override string ToString()
    {
      return text + " " + UtilsSubs.timeToString(startTime) + ", " + UtilsSubs.timeToString(endTime);
    }


  }
}
