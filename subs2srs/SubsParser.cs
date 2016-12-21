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
  /// Represents a subtitle parser.
  /// </summary>
  abstract class SubsParser
  {

    public enum Type
    {
      Ass = 0,     // .ass/.ssa
      Srt,         // .srt
      Vobsub,      // .sub/.idx
      Lyrics,      // .lrc
      Transcriber, // .trs
      Unknown
    }

    private WorkerVars workerVars;
    private string file;
    private int stream;
    private int episode;
    private int subsNum;
    private Encoding subsEncoding;


    public WorkerVars WorkerVars 
    { 
      get
      {
        return workerVars;
      }
      set
      {
        workerVars = value;
      }
    }


    /// <summary>
    /// The file to parse.
    /// </summary>
    public string File 
    { 
      get
      {
        return file;
      }
      set
      {
        file = value;
      }
    }


    /// <summary>
    /// For Vobsubs, the index of the stream to parse.
    /// </summary>
    public int Stream
    {
      get
      {
        return stream;
      }
      set
      {
        stream = value;
      }
    }


    /// <summary>
    /// The episode of the subtitle file.
    /// </summary>
    public int Episode
    {
      get
      {
        return episode;
      }
      set
      {
        episode = value;
      }
    }


    /// <summary>
    /// 1 = Subs1, 2 = Subs2
    /// </summary>
    public int SubsNum
    {
      get
      {
        return subsNum;
      }
      set
      {
        subsNum = value;
      }
    }

    /// <summary>
    /// The encoding of the subtitles file. Not used for Vobsub.
    /// </summary>
    public Encoding SubsEncoding
    {
      get
      {
        return subsEncoding;
      }
      set
      {
        subsEncoding = value;
      }
    }


    /// <summary>
    /// Parse the subtitle file and return a list of lines.
    /// </summary>
    abstract public List<InfoLine> parse();
    
  }
}
