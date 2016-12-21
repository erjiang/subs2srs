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
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;


namespace subs2srs
{
  /// <summary>
  /// Responsible for creation of the SRS import file in the worker thread.
  /// </summary>
  class WorkerSrs
  {
    enum FormatType
    {
      Normal,
      Leading,
      Trailing
    }

    private DateTime lastTime = new DateTime();
    private UtilsName name = null;
    private int progessCount = 0;
    private InfoCombined mainComb;


    /// <summary>
    /// Based on timings, determine if this line is within the context range.
    /// So if someone says a line, then it cuts to the OP theme, there shouldn't be
    /// anything for it's trailing line (assuming the credits have been removed from the subs).
    /// </summary>
    private bool isLineInContextRange(List<InfoCombined> combArray, int baseLineIndex, int curContextIdx)
    {
      bool inRange = true;

      // If leading context line
      if (curContextIdx < baseLineIndex)
      {
        // If context range feature is disabled
        if (Settings.Instance.ContextLeadingRange == 0)
        {
          return true;
        }

        for (int i = baseLineIndex; i > curContextIdx; i--)
        {
          int durationMs = (int)UtilsSubs.getDurationTime(combArray[i - 1].Subs1.EndTime, 
            combArray[i].Subs1.StartTime).TimeOfDay.TotalMilliseconds;

          if (durationMs > (Settings.Instance.ContextLeadingRange * 1000))
          {
            inRange = false;
            break;
          }
        }
      }
      else // Trailing context line
      {
        // If context range feature is disabled
        if (Settings.Instance.ContextTrailingRange == 0)
        {
          return true;
        }

        for (int i = baseLineIndex; i < curContextIdx; i++)
        {
          int durationMs = (int)UtilsSubs.getDurationTime(combArray[i].Subs1.EndTime,
            combArray[i + 1].Subs1.StartTime).TimeOfDay.TotalMilliseconds;

          if (durationMs > (Settings.Instance.ContextTrailingRange * 1000))
          {
            inRange = false;
            break;
          }
        }
      }

      return inRange;
    }


    /// <summary>
    /// Generate the SRS import file.
    /// </summary>
    public bool genSrs(WorkerVars workerVars, DialogProgress dialogProgress)
    {
      int episodeIdx = -1;
      int totalEpisodes = workerVars.CombinedAll.Count;
      int lineIdx = -1;
      int totalLines = UtilsSubs.getTotalLineCount(workerVars.CombinedAll);

      lastTime = UtilsSubs.getLastTime(workerVars.CombinedAll);

      name = new UtilsName(Settings.Instance.DeckName, totalEpisodes,
        totalLines, lastTime, Settings.Instance.VideoClips.Size.Width, Settings.Instance.VideoClips.Size.Height);

      string nameStr = name.createName(ConstantSettings.SrsFilenameFormat, 0,
        0, new DateTime(), new DateTime(), "", "");
      
      // Create filename
      // Example: <outdir>\Toki_wo_Kakeru_Shoujo.tsv
      string srsFilename = String.Format("{0}{1}{2}",
                                          Settings.Instance.OutputDir,
                                          Path.DirectorySeparatorChar,
                                          nameStr);
      TextWriter srsWriter = new StreamWriter(srsFilename, false, Encoding.UTF8);
      
      // For each episode
      foreach (List<InfoCombined> combArray in workerVars.CombinedAll)
      {
        episodeIdx++;
        lineIdx = -1;

        // For each line in episode, process
        foreach (InfoCombined comb in combArray)
        {
          progessCount++;
          lineIdx++;
          mainComb = comb;

          // Skip lines that are only needed for context
          if (mainComb.OnlyNeededForContext)
          {
            continue;
          }

          string srsLine = "";

          // Format the current import file line
          srsLine = formatAll(workerVars.CombinedAll, episodeIdx, lineIdx, FormatType.Normal);

          // Add leading context lines
          if (Settings.Instance.ContextLeadingCount > 0)
          {
            for (int i = Settings.Instance.ContextLeadingCount; i > 0; i--)
            {
              int prevLineIdx = lineIdx - i;

              if (prevLineIdx >= 0)
              {
                if (isLineInContextRange(combArray, lineIdx, prevLineIdx))
                {
                  srsLine += formatAll(workerVars.CombinedAll, episodeIdx, prevLineIdx, FormatType.Leading);
                }
                else
                {
                  srsLine += formatContextPlaceholder(FormatType.Leading);
                }
              }
              else
              {
                srsLine += formatContextPlaceholder(FormatType.Leading);
              }
            }
          }

          // Add trailing context lines
          if (Settings.Instance.ContextTrailingCount > 0)
          {
            for (int i = 1; i <= Settings.Instance.ContextTrailingCount; i++)
            {
              int nextLineIdx = lineIdx + i;

              if (nextLineIdx < combArray.Count)
              {
                if (isLineInContextRange(combArray, lineIdx, nextLineIdx))
                {
                  srsLine += formatAll(workerVars.CombinedAll, episodeIdx, nextLineIdx, FormatType.Trailing);
                }
                else
                {
                  srsLine += formatContextPlaceholder(FormatType.Trailing);
                }
              }
              else
              {
                srsLine += formatContextPlaceholder(FormatType.Trailing);
              }
            }
          }

          // Write line to file
          srsWriter.WriteLine(srsLine);

          string progressText = string.Format("Generating SRS (ex. Anki) import file: line {0} of {1}",
                                              progessCount.ToString(),
                                              totalLines.ToString());

          int progress = Convert.ToInt32(progessCount * (100.0 / totalLines));

          DialogProgress.updateProgressInvoke(dialogProgress, progress, progressText);

          // Did the user press the cancel button?
          if (dialogProgress.Cancel)
          {
            srsWriter.Close();
            return false;
          }
        }
      }

      srsWriter.Close();

      return true;
    }


    /// <summary>
    /// Format a context placeholder line.
    /// </summary>
    private string formatContextPlaceholder(FormatType formatType)
    {
      string outText = "";

      if (Settings.Instance.AudioClips.Enabled)
      {
        if (((formatType == FormatType.Leading) && Settings.Instance.ContextLeadingIncludeAudioClips) 
          || ((formatType == FormatType.Trailing) && Settings.Instance.ContextTrailingIncludeAudioClips))
        {
          outText += ConstantSettings.SrsDelimiter;
        }
      }

      if (Settings.Instance.Snapshots.Enabled)
      {
        if (((formatType == FormatType.Leading) && Settings.Instance.ContextLeadingIncludeSnapshots)
          || ((formatType == FormatType.Trailing) && Settings.Instance.ContextTrailingIncludeSnapshots))
        {
          outText += ConstantSettings.SrsDelimiter;
        }
      }

      if (Settings.Instance.VideoClips.Enabled)
      {
        if (((formatType == FormatType.Leading) && Settings.Instance.ContextLeadingIncludeVideoClips)
          || ((formatType == FormatType.Trailing) && Settings.Instance.ContextTrailingIncludeVideoClips))
        {
          outText += ConstantSettings.SrsDelimiter;
        }
      }

      outText += ConstantSettings.SrsDelimiter;

      if (Settings.Instance.Subs[1].FilePattern != "")
      {
        outText += ConstantSettings.SrsDelimiter;
      }

      return outText;
    }


    /// <summary>
    /// Format a line for the SRS import file.
    /// </summary>
    private string formatAll(List<List<InfoCombined>> combinedAll, int episodeIndex, int lineIndex, FormatType formatType)
    {
      List<InfoCombined> combArray = combinedAll[episodeIndex];
      InfoCombined comb = combArray[lineIndex];
      string outText = "";

      // Add tag and sequence marker
      if (formatType == FormatType.Normal)
      {
        if (ConstantSettings.SrsTagFormat != "")
        {
          outText += formatTag(comb, episodeIndex);
        }

        if ((ConstantSettings.SrsTagFormat != "") && (ConstantSettings.SrsSequenceMarkerFormat != ""))
        {
          outText += ConstantSettings.SrsDelimiter;
        }

        if (ConstantSettings.SrsSequenceMarkerFormat != "")
        {
          outText += formatSequenceMarker(comb, episodeIndex);
        }
      }

      // Add audio clip
      if (Settings.Instance.AudioClips.Enabled)
      {
        if (formatType == FormatType.Normal
          || ((formatType == FormatType.Leading) && Settings.Instance.ContextLeadingIncludeAudioClips)
          || ((formatType == FormatType.Trailing) && Settings.Instance.ContextTrailingIncludeAudioClips))
        {
          outText += ConstantSettings.SrsDelimiter + formatAudioClip(comb, episodeIndex);
        }
      }

      // Add snapshot clip
      if (Settings.Instance.Snapshots.Enabled)
      {
        if (formatType == FormatType.Normal
          || ((formatType == FormatType.Leading) && Settings.Instance.ContextLeadingIncludeSnapshots)
          || ((formatType == FormatType.Trailing) && Settings.Instance.ContextTrailingIncludeSnapshots))
        {
          outText += ConstantSettings.SrsDelimiter + formatSnapshot(comb, episodeIndex);
        }
      }

      // Add video clip
      if (Settings.Instance.VideoClips.Enabled)
      {
        if (formatType == FormatType.Normal
          || ((formatType == FormatType.Leading) && Settings.Instance.ContextLeadingIncludeVideoClips)
          || ((formatType == FormatType.Trailing) && Settings.Instance.ContextTrailingIncludeVideoClips))
        {
          outText += ConstantSettings.SrsDelimiter + formatVideoClip(comb, episodeIndex);
        }
      }

      // Add line from Subs1
      outText += ConstantSettings.SrsDelimiter + formatSubs1(comb, episodeIndex);

      // Add line from Subs2
      if (Settings.Instance.Subs[1].FilePattern != "")
      {
        outText += ConstantSettings.SrsDelimiter + formatSubs2(comb, episodeIndex);
      }

      // Remove any initial tab
      if (formatType == FormatType.Normal)
      {
        if (outText.StartsWith("\t"))
        {
          outText = outText.Substring(1);
        }
      }

      return outText;
    }


    /// <summary>
    /// Format the tag.
    /// </summary>
    private string formatTag(InfoCombined comb, int episodeIndex)
    {
      string outText = "";

      DateTime startTime = comb.Subs1.StartTime;
      DateTime endTime = comb.Subs1.EndTime;

      string nameStr = name.createName(ConstantSettings.SrsTagFormat, episodeIndex + Settings.Instance.EpisodeStartNumber,
         progessCount, startTime, endTime, comb.Subs1.Text, comb.Subs2.Text);

      outText = String.Format("{0}",
                              nameStr);

      return outText;
    }


    /// <summary>
    /// Format the sequence marker.
    /// </summary>
    private string formatSequenceMarker(InfoCombined comb, int episodeIndex)
    {
      DateTime startTime = comb.Subs1.StartTime;
      DateTime endTime = comb.Subs1.EndTime;
      string outText = "";

      string nameStr = name.createName(ConstantSettings.SrsSequenceMarkerFormat, episodeIndex + Settings.Instance.EpisodeStartNumber,
        progessCount, startTime, endTime, comb.Subs1.Text, comb.Subs2.Text);

      outText = String.Format("{0}",
                              nameStr); // {0}  

      return outText;
    }


    /// <summary>
    /// Format a audio clip.
    /// </summary>
    private string formatAudioClip(InfoCombined comb, int episodeIndex)
    {
      DateTime startTime;
      DateTime endTime;
      string outText = "";

      // Apply pad (if requested)
      if (Settings.Instance.AudioClips.PadEnabled)
      {
        startTime = UtilsSubs.applyTimePad(comb.Subs1.StartTime, -Settings.Instance.AudioClips.PadStart);
        endTime = UtilsSubs.applyTimePad(comb.Subs1.EndTime, Settings.Instance.AudioClips.PadEnd);
      }
      else
      {
        startTime = comb.Subs1.StartTime;
        endTime = comb.Subs1.EndTime;
      }

      string prefixStr = name.createName(ConstantSettings.SrsAudioFilenamePrefix, episodeIndex + Settings.Instance.EpisodeStartNumber,
         progessCount, startTime, endTime, comb.Subs1.Text, comb.Subs2.Text);
      
      string nameStr = name.createName(ConstantSettings.AudioFilenameFormat, episodeIndex + Settings.Instance.EpisodeStartNumber,
         progessCount, startTime, endTime, comb.Subs1.Text, comb.Subs2.Text);

      string suffixStr = name.createName(ConstantSettings.SrsAudioFilenameSuffix, episodeIndex + Settings.Instance.EpisodeStartNumber,
         progessCount, startTime, endTime, comb.Subs1.Text, comb.Subs2.Text);

      outText = String.Format("{0}{1}{2}",
                              prefixStr,    // {0}
                              nameStr,      // {1}
                              suffixStr);   // {2}

      return outText;
    }


    /// <summary>
    /// Format a snapshot.
    /// </summary>
    private string formatSnapshot(InfoCombined comb, int episodeIndex)
    {
      DateTime startTime = comb.Subs1.StartTime;
      DateTime endTime = comb.Subs1.EndTime;
      DateTime midTime = UtilsSubs.getMidpointTime(comb.Subs1.StartTime, comb.Subs1.EndTime);
      string outText = "";

      string prefixStr = name.createName(ConstantSettings.SrsSnapshotFilenamePrefix, episodeIndex + Settings.Instance.EpisodeStartNumber,
        progessCount, startTime, endTime, comb.Subs1.Text, comb.Subs2.Text);

      string nameStr = name.createName(ConstantSettings.SnapshotFilenameFormat, episodeIndex + Settings.Instance.EpisodeStartNumber,
        progessCount, startTime, endTime, comb.Subs1.Text, comb.Subs2.Text);

      string suffixStr = name.createName(ConstantSettings.SrsSnapshotFilenameSuffix, episodeIndex + Settings.Instance.EpisodeStartNumber,
        progessCount, startTime, endTime, comb.Subs1.Text, comb.Subs2.Text);

      outText = String.Format("{0}{1}{2}",
                              prefixStr,    // {0}
                              nameStr,      // {1}
                              suffixStr);   // {2}

      return outText;
    }


    /// <summary>
    /// Format a video clip.
    /// </summary>
    private string formatVideoClip(InfoCombined comb, int episodeIndex)
    {
      DateTime startTime;
      DateTime endTime;
      string outText = "";

      // Apply pad (if requested)
      if (Settings.Instance.VideoClips.PadEnabled)
      {
        startTime = UtilsSubs.applyTimePad(comb.Subs1.StartTime, -Settings.Instance.VideoClips.PadStart);
        endTime = UtilsSubs.applyTimePad(comb.Subs1.EndTime, Settings.Instance.VideoClips.PadEnd);
      }
      else
      {
        startTime = comb.Subs1.StartTime;
        endTime = comb.Subs1.EndTime;
      }

      string videoExtension = ".avi";

      if (Settings.Instance.VideoClips.IPodSupport)
      {
        videoExtension = ".mp4";
      }

      string prefixStr = name.createName(ConstantSettings.SrsVideoFilenamePrefix, episodeIndex + Settings.Instance.EpisodeStartNumber,
        progessCount, startTime, endTime, comb.Subs1.Text, comb.Subs2.Text);

      string nameStr = name.createName(ConstantSettings.VideoFilenameFormat, episodeIndex + Settings.Instance.EpisodeStartNumber,
        progessCount, startTime, endTime, comb.Subs1.Text, comb.Subs2.Text);

      string suffixStr = name.createName(ConstantSettings.SrsVideoFilenameSuffix, episodeIndex + Settings.Instance.EpisodeStartNumber,
        progessCount, startTime, endTime, comb.Subs1.Text, comb.Subs2.Text);

      outText = String.Format("{0}{1}{2}{3}",
                              prefixStr,      // {0}
                              nameStr,        // {1}
                              videoExtension, // {2}
                              suffixStr);     // {3}
         
      return outText;
    }


    /// <summary>
    /// Format Subs1 text.
    /// </summary>
    private string formatSubs1(InfoCombined comb, int episodeIndex)
    {
      DateTime startTime = comb.Subs1.StartTime;
      DateTime endTime = comb.Subs1.EndTime;
      string outText = "";

      string nameStr = name.createName(ConstantSettings.SrsSubs1Format, episodeIndex + Settings.Instance.EpisodeStartNumber,
        progessCount, startTime, endTime, comb.Subs1.Text, comb.Subs2.Text);

      outText = String.Format("{0}",
                              nameStr); // {0}

      return outText;
    }


    /// <summary>
    /// Format Subs2 text.
    /// </summary>
    private string formatSubs2(InfoCombined comb, int episodeIndex)
    {
      DateTime startTime = comb.Subs1.StartTime;
      DateTime endTime = comb.Subs1.EndTime;
      string outText = "";

      string nameStr = name.createName(ConstantSettings.SrsSubs2Format, episodeIndex + Settings.Instance.EpisodeStartNumber,
        progessCount, startTime, endTime, comb.Subs1.Text, comb.Subs2.Text);

      outText = String.Format("{0}",
                              nameStr); // {0}

      return outText;
    }



  }
}
