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
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;

namespace subs2srs
{
  /// <summary>
  /// Used for extract information from .mkv files.
  /// </summary>
  public class UtilsMkv
  {
    public enum TrackType
    {
      UNKNOWN,
      SUBTITLES,
      AUDIO
    }

    // Used by getTrackList().
    private enum TrackListParseState
    {
      SEARCH_FOR_START_OF_TRACKS, // |+ Segment tracks
      GET_NEXT_TRACK,             // | + A track
      GET_TRACK_NUM,              // |  + Track number: 3 (track ID for mkvmerge & mkvextract: 2)
      GET_TRACK_TYPE,             // |  + Track type: subtitles
      GET_CODEC_ID,               // |  + Codec ID: S_TEXT/ASS
      GET_LANG,                   // |  + Language: eng
    }

    /// <summary>
    /// Get list of audio and subtitle tracks in the provided .mkv file.
    /// </summary>
    public static List<MkvTrack> getTrackList(string mkvFile)
    {
      List<MkvTrack> trackList = new List<MkvTrack>();

      if (Path.GetExtension(mkvFile) != ".mkv")
      {
        return trackList;
      }

      string args = String.Format("\"{0}\"", mkvFile);
      string mkvIinfo = UtilsCommon.startProcessAndGetStdout(ConstantSettings.PathMkvInfoExeRel, ConstantSettings.PathMkvInfoExeFull, args);

      if (mkvIinfo == "Error.")
      {
        return trackList;
      }

      mkvIinfo = mkvIinfo.Replace("\r\r", "");

      string[] lines = mkvIinfo.Split('\n');

      TrackListParseState state = TrackListParseState.SEARCH_FOR_START_OF_TRACKS;

      MkvTrack curTrack = new MkvTrack();

      foreach (string line in lines)
      {
        switch (state)
        {
          case TrackListParseState.SEARCH_FOR_START_OF_TRACKS:
          {
            if (line.StartsWith("|+ Segment tracks"))
            {
              state = TrackListParseState.GET_NEXT_TRACK;
            }

            break;
          }

          case TrackListParseState.GET_NEXT_TRACK:
          {
            // Reset track info
            curTrack = new MkvTrack();

            if (line.StartsWith("| + A track"))
            {
              state = TrackListParseState.GET_TRACK_NUM;
            }

            break;
          }

          case TrackListParseState.GET_TRACK_NUM:
          {
            Match match = Regex.Match(line, @"^\|  \+ Track number: \d+ \(track ID for mkvmerge & mkvextract: (?<TrackNum>\d+)\)");

            if (match.Success)
            {
              curTrack.TrackID = match.Groups["TrackNum"].ToString().Trim();
              state = TrackListParseState.GET_TRACK_TYPE;
            }

            break;
          }

          case TrackListParseState.GET_TRACK_TYPE:
          {
            Match match = Regex.Match(line, @"^\|  \+ Track type: (?<TrackType>\w+)");

            if (match.Success)
            {
              string trackType = match.Groups["TrackType"].ToString().Trim();

              if (trackType == "subtitles")
              {
                curTrack.TrackType = TrackType.SUBTITLES;
                state = TrackListParseState.GET_CODEC_ID;
              }
              else if (trackType == "audio")
              {
                curTrack.TrackType = TrackType.AUDIO;
                state = TrackListParseState.GET_CODEC_ID;
              }
              else
              {
                state = TrackListParseState.GET_NEXT_TRACK;
              }
            }

            break;
          }

          case TrackListParseState.GET_CODEC_ID:
          {
            Match match = Regex.Match(line, @"^\|  \+ Codec ID: (?<CodecID>.+)");

            if (match.Success)
            {
              string codecID = match.Groups["CodecID"].ToString().Trim();

              curTrack.Extension = codecID;

              // See http://matroska.org/technical/specs/codecid/index.html

              // Try subtitle Codec IDs first
              if (codecID == "S_VOBSUB")
              {
                curTrack.Extension = "sub";
                state = TrackListParseState.GET_LANG;
              }
              else if (codecID == "S_TEXT/UTF8")
              {
                curTrack.Extension = "srt";
                state = TrackListParseState.GET_LANG;
              }
              else if (codecID == "S_TEXT/ASS")
              {
                curTrack.Extension = "ass";
                state = TrackListParseState.GET_LANG;
              }
              else if (codecID == "S_TEXT/SSA")
              {
                curTrack.Extension = "ssa";
                state = TrackListParseState.GET_LANG;
              }
              //
              // Now try audio Codec IDs
              //
              else if (codecID == "A_MPEG/L3")
              {
                curTrack.Extension = "mp3";
                state = TrackListParseState.GET_LANG;
              }
              else if (codecID == "A_MPEG/L2")
              {
                curTrack.Extension = "mp2";
                state = TrackListParseState.GET_LANG;
              }
              else if (codecID == "A_MPEG/L1")
              {
                curTrack.Extension = "mp1";
                state = TrackListParseState.GET_LANG;
              }
              else if (codecID.StartsWith("A_PCM"))
              {
                curTrack.Extension = "wav";
                state = TrackListParseState.GET_LANG;
              }
              else if (codecID == "A_MPC")
              {
                curTrack.Extension = "mpc";
                state = TrackListParseState.GET_LANG;
              }
              else if (codecID.StartsWith("A_AC3"))
              {
                curTrack.Extension = "ac3";
                state = TrackListParseState.GET_LANG;
              }
              else if (codecID.StartsWith("A_ALAC"))
              {
                curTrack.Extension = "m4a";
                state = TrackListParseState.GET_LANG;
              }
              else if (codecID.StartsWith("A_DTS"))
              {
                curTrack.Extension = "dts";
                state = TrackListParseState.GET_LANG;
              }
              else if (codecID == "A_VORBIS")
              {
                curTrack.Extension = "ogg";
                state = TrackListParseState.GET_LANG;
              }
              else if (codecID == "A_FLAC")
              {
                curTrack.Extension = "flac";
                state = TrackListParseState.GET_LANG;
              }
              else if (codecID.StartsWith("A_REAL"))
              {
                curTrack.Extension = "rm";
                state = TrackListParseState.GET_LANG;
              }
              else if (codecID.StartsWith("A_AAC"))
              {
                curTrack.Extension = "aac";
                state = TrackListParseState.GET_LANG;
              }
              else if (codecID.StartsWith("A_QUICKTIME"))
              {
                curTrack.Extension = "aiff";
                state = TrackListParseState.GET_LANG;
              }
              else if (codecID == "A_TTA1")
              {
                curTrack.Extension = "tta";
                state = TrackListParseState.GET_LANG;
              }
              else if (codecID == "A_WAVPACK4")
              {
                curTrack.Extension = "wv";
                state = TrackListParseState.GET_LANG;
              }
              else
              {
                state = TrackListParseState.GET_NEXT_TRACK;
              }
            }

            break;
          }

          // Note: Lang is optional
          case TrackListParseState.GET_LANG:
          {
            Match match = Regex.Match(line, @"^\|  \+ Language: (?<Lang>\w+)");

            if (match.Success)
            {
              curTrack.Lang = match.Groups["Lang"].ToString().Trim();

              // All required info for this track was found, add it to the list
              trackList.Add(curTrack);

              state = TrackListParseState.GET_NEXT_TRACK;
            }
            else
            {
              if (line.StartsWith("| + A track"))
              {
                // Lang is missing, add what we have
                trackList.Add(curTrack);

                state = TrackListParseState.GET_TRACK_NUM;

                // Reset track info
                curTrack = new MkvTrack();
              }
              else if (line.StartsWith("|+")) // Are we past the track section?
              {
                // Lang is missing, add what we have
                trackList.Add(curTrack);
                goto lbl_end_parse;
              }
            }

            break;
          }
        }
      }

      lbl_end_parse:

      return trackList;
    }


    /// <summary>
    /// Get list of subtitle tracks in the provided .mkv file.
    /// </summary>
    public static List<MkvTrack> getSubtitleTrackList(string mkvFile)
    {
      List<MkvTrack> subtitleTrackList = new List<MkvTrack>();
      List<MkvTrack> allTrackList = UtilsMkv.getTrackList(mkvFile);

      foreach (MkvTrack track in allTrackList)
      {
        if (track.TrackType == TrackType.SUBTITLES)
        {
          subtitleTrackList.Add(track);
        }
      }

      return subtitleTrackList;
    }


    /// <summary>
    /// Get list of audio tracks in the provided .mkv file.
    /// </summary>
    public static List<MkvTrack> getAudioTrackList(string mkvFile)
    {
      List<MkvTrack> audioTrackList = new List<MkvTrack>();
      List<MkvTrack> allTrackList = UtilsMkv.getTrackList(mkvFile);

      foreach (MkvTrack track in allTrackList)
      {
        if (track.TrackType == TrackType.AUDIO)
        {
          audioTrackList.Add(track);
        }
      }

      return audioTrackList;
    }


    /// <summary>
    /// Extract track from the provided mvk file.
    /// </summary>
    public static void extractTrack(string mkvFile, string trackID, string outName)
    {
      string args = String.Format("tracks \"{0}\" {1}:\"{2}\"", mkvFile, trackID, outName);

      UtilsCommon.startProcess(ConstantSettings.PathMkvExtractExeRel, ConstantSettings.PathMkvExtractExeFull, args);
    }
  }


  [Serializable]
  public class MkvTrack
  {
    public string TrackID { get; set; } // 1, 2, etc.
    public UtilsMkv.TrackType TrackType { get; set; }
    public string CodecID { get; set; } // S_TEXT/ASS, A_MPEG/L3, etc.
    public string Extension { get; set; } // ass, mp3, etc.
    public string Lang { get; set; } // eng, jpn, etc.

    public MkvTrack()
    {
      this.TrackID = "";
      this.TrackType = UtilsMkv.TrackType.UNKNOWN;
      this.CodecID = "";
      this.Extension = "";
      this.Lang = "";
    }


    public override string ToString()
    {
      string displayLang = UtilsLang.LangThreeLetter2Full(this.Lang);
      string displayedExtension = this.Extension.ToUpper();

      if ((this.Lang == "und") || (this.Lang == "") || (displayLang == ""))
      {
        displayLang = "Unknown Language";
      }

      if (this.TrackType == UtilsMkv.TrackType.SUBTITLES)
      {
        if (displayedExtension == "SUB")
        {
          displayedExtension = "VOBSUB";
        }
      }

      return String.Format("{0} - {1} ({2})",
        this.TrackID, displayedExtension, displayLang);
    }
  }
}




