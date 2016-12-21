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
using System.Linq;
using System.Text;

namespace subs2srs
{
  /// <summary>
  /// Represents an audio or vobsub stream.
  /// </summary>
  [Serializable]
  public class InfoStream
  {
    private string num;
    private string displayNum;
    private string lang;
    private string type;

    /// <summary>
    /// The index of the stream.
    /// For audio, this is the actual stream number according to FFmpeg.
    /// </summary>
    public string Num 
    {
      get { return num; }
      set { num = value; } 
    }


    /// <summary>
    /// The stream number to actual display.
    /// </summary>
    public string DisplayNum
    {
      get { return displayNum; }
      set { displayNum = value; } 
    }


    /// <summary>
    /// The language of the stream.
    /// </summary>
    public string Lang
    {
      get { return lang; }
      set { lang = value; } 
    }


    /// <summary>
    /// Type of stream. Put anything here. Not used.
    /// For audio stream, the raw FFmpeg information can be used.
    /// </summary>
    public string Type
    {
      get { return type; }
      set { type = value; }
    }


    public InfoStream()
    {
      this.num = "-";
      this.displayNum = "-";
      this.lang = "-";
      this.type = "-";
    }

    public InfoStream(string num, string displayNum, string lang, string type)
    {
      this.num = num;
      this.displayNum = displayNum;
      this.lang = lang;
      this.type = type;
    }


    public override string ToString()
    {
      string ret = "(Default)";
      string displayLang = lang;

      if (num != "-")
      {
        if (lang.Trim() == "")
        {
          displayLang = "???";
        }

        ret = displayNum + " - (" + displayLang + ")";
      }

      return ret;
    }








  }
}
