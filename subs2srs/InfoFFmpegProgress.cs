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
using System.Text.RegularExpressions;

namespace subs2srs
{
  /// <summary>
  /// Represents FFmpeg progress.
  /// </summary>
  public class InfoFFmpegProgress
  {
    int frame;
    int fps;
    double q;
    int fileSize;
    DateTime time;
    double bitrate;
    bool videoProgess; // False = Audio progress

    /// <summary>
    /// The current frame being processed.
    /// </summary>
    public int Frame
    {
      get { return frame; }
      set { frame = value; }
    }


    /// <summary>
    /// The Frames Per Second the video is being encoded at.
    /// </summary>
    public int FPS
    {
      get { return fps; }
      set { fps = value; }
    }


    /// <summary>
    /// Quantizer.
    /// </summary>
    public double Q
    {
      get { return q; }
      set { q = value; }
    }


    /// <summary>
    /// File size.
    /// </summary>
    public int FileSize
    {
      get { return fileSize; }
      set { fileSize = value; }
    }


    /// <summary>
    /// The timestamp of the frame currently being processed.
    /// </summary>
    public DateTime Time
    {
      get { return time; }
      set { time = value; }
    }


    /// <summary>
    /// The (average?) bitrate being used to encode the file.
    /// </summary>
    public double Bitrate
    {
      get { return bitrate; }
      set { bitrate = value; }
    }


    public bool VideoProgress
    {
      get { return videoProgess; }
      set { videoProgess = value; }
    }


    public InfoFFmpegProgress()
    {
      this.frame = 0;
      this.fps = 0;
      this.q = 0;
      this.fileSize = 0;
      this.time = new DateTime();
      this.bitrate = 0;
      this.videoProgess = false;
    }

    /// <summary>
    /// Parse the output of FFmpeg and extract progress information.
    /// </summary>
    public bool parseFFmpegProgress(string text)
    {
      // frame= 1499 fps= 62 q=2.0 size=    6884kB time=00:01:02.43 bitrate= 903.2kbits/s
      // or 
      // size=    6884kB time=00:01:02.43 bitrate= 903.2kbits/s
      Match match = Regex.Match(text,
       @"^(?:frame=\s*?(?<Frame>\d+?)\s*?fps=\s*?(?<FPS>\d+?)\s*q=(?<Q>\d.*?)\s*?)?" + 
       @"size=\s*(?<Size>\d+?)kB\s*?time=(?<Hours>\d\d):(?<Minutes>\d\d):(?<Seconds>\d\d).(?<CentiSeconds>\d\d)\s*?bitrate=\s*?(?<Bitrate>\d.*?)kbits/s\s*$", 
       RegexOptions.Compiled);

      if (!match.Success)
      {
        return false;
      }

      try
      {
        if (match.Groups["Frame"].Success)
        {
          this.videoProgess = true;
          this.frame = Convert.ToInt32(match.Groups["Frame"].ToString().Trim());
          this.fps = Convert.ToInt32(match.Groups["FPS"].ToString().Trim());
          this.q = Convert.ToDouble(match.Groups["Q"].ToString().Trim());
        }
        else
        {
          this.videoProgess = false;
        }

        this.fileSize = Convert.ToInt32(match.Groups["Size"].ToString().Trim());
        this.time = new DateTime();
        this.time = this.time.AddHours(Convert.ToInt32(match.Groups["Hours"].ToString().Trim()));
        this.time = this.time.AddMinutes(Convert.ToInt32(match.Groups["Minutes"].ToString().Trim()));
        this.time = this.time.AddSeconds(Convert.ToInt32(match.Groups["Seconds"].ToString().Trim()));
        this.time = this.time.AddMilliseconds(10 * Convert.ToInt32(match.Groups["CentiSeconds"].ToString().Trim()));
        this.bitrate = Convert.ToDouble(match.Groups["Bitrate"].ToString().Trim());
      }
      catch
      {
        return false;
      }

      return true;
    }


  }
}
