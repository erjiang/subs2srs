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
  /// Responsible for processing video in the worker thread.
  /// </summary>
  public class WorkerVideo
  {
    /// <summary>
    /// Generate video clips for all episodes.
    /// </summary>
    public bool genVideoClip(WorkerVars workerVars, DialogProgress dialogProgress)
    {
      int progessCount = 0;
      int episodeCount = 0;
      int totalEpisodes = workerVars.CombinedAll.Count;
      int totalLines = UtilsSubs.getTotalLineCount(workerVars.CombinedAll);
      DateTime lastTime = UtilsSubs.getLastTime(workerVars.CombinedAll);

      UtilsName name = new UtilsName(Settings.Instance.DeckName, totalEpisodes,
        totalLines, lastTime, Settings.Instance.VideoClips.Size.Width, Settings.Instance.VideoClips.Size.Height);

      DialogProgress.updateProgressInvoke(dialogProgress, 0, "Creating video clips.");

      // For each episode
      foreach (List<InfoCombined> combArray in workerVars.CombinedAll)
      {
        episodeCount++;

        // It is possible for all lines in an episode to be set to inactive
        if (combArray.Count == 0)
        {
          // Skip this episode
          continue;
        }

        string progressText = String.Format("Converting video file {0} of {1}",
                                            episodeCount,
                                            totalEpisodes);

        DialogProgress.updateProgressInvoke(dialogProgress, progressText);
        
        DateTime entireClipStartTime = combArray[0].Subs1.StartTime;
        DateTime entireClipEndTime = combArray[combArray.Count - 1].Subs1.EndTime;

        // Apply pad to entire clip timings  (if requested)
        if (Settings.Instance.VideoClips.PadEnabled)
        {
          entireClipStartTime = UtilsSubs.applyTimePad(entireClipStartTime, -Settings.Instance.VideoClips.PadStart);
          entireClipEndTime = UtilsSubs.applyTimePad(entireClipEndTime, Settings.Instance.VideoClips.PadEnd);
        }

        // Enable detail mode in progress dialog
        DialogProgress.enableDetailInvoke(dialogProgress, true);

        // Set the duration of the clip in the progress dialog  (for detail mode)
        DateTime entireClipDuration = UtilsSubs.getDurationTime(entireClipStartTime, entireClipEndTime);
        DialogProgress.setDuration(dialogProgress, entireClipDuration);

        string tempVideoFilename = Path.GetTempPath() + ConstantSettings.TempVideoFilename;
        string videoExtension = ".avi";

        // Convert entire video (from first line to last line) based on user settings (size, crop, bitrate, etc).
        // It will be cut into smaller video clips later.
        if (Settings.Instance.VideoClips.IPodSupport)
        {
          videoExtension = ".mp4";
          tempVideoFilename += videoExtension;

          UtilsVideo.convertVideo(Settings.Instance.VideoClips.Files[episodeCount - 1],
            Settings.Instance.VideoClips.AudioStream.Num,
            entireClipStartTime, entireClipEndTime,
            Settings.Instance.VideoClips.Size, Settings.Instance.VideoClips.Crop,
            Settings.Instance.VideoClips.BitrateVideo, Settings.Instance.VideoClips.BitrateAudio, 
            UtilsVideo.VideoCodec.h264, UtilsVideo.AudioCodec.AAC, 
            UtilsVideo.Profilex264.IPod640, UtilsVideo.Presetx264.SuperFast,
            tempVideoFilename, dialogProgress);
        }
        else
        {
          tempVideoFilename += videoExtension;

          UtilsVideo.convertVideo(Settings.Instance.VideoClips.Files[episodeCount - 1],
            Settings.Instance.VideoClips.AudioStream.Num,
            entireClipStartTime, entireClipEndTime,
            Settings.Instance.VideoClips.Size, Settings.Instance.VideoClips.Crop,
            Settings.Instance.VideoClips.BitrateVideo, Settings.Instance.VideoClips.BitrateAudio,
            UtilsVideo.VideoCodec.MPEG4, UtilsVideo.AudioCodec.MP3,
            UtilsVideo.Profilex264.None, UtilsVideo.Presetx264.None,
            tempVideoFilename, dialogProgress);

        }

        DialogProgress.enableDetailInvoke(dialogProgress, false);

        // Generate a video clip for each line of the episode
        foreach (InfoCombined comb in combArray)
        {
          progessCount++;

          progressText = string.Format("Generating video clip: {0} of {1}",
                                       progessCount.ToString(),
                                       totalLines.ToString());

          int progress = Convert.ToInt32(progessCount * (100.0 / totalLines));

          // Update the progress dialog
          DialogProgress.updateProgressInvoke(dialogProgress, progress, progressText);

          // Did the user press the cancel button?
          if (dialogProgress.Cancel)
          {
            File.Delete(tempVideoFilename);
            return false;
          }

          // Adjust timing to sync with the start of the converted video (which starts at the episode's first line of dialog)
          DateTime startTime = UtilsSubs.shiftTiming(comb.Subs1.StartTime, -((int)entireClipStartTime.TimeOfDay.TotalMilliseconds));
          DateTime endTime = UtilsSubs.shiftTiming(comb.Subs1.EndTime, -((int)entireClipStartTime.TimeOfDay.TotalMilliseconds));

          // Times used in the filename
          DateTime filenameStartTime = comb.Subs1.StartTime;
          DateTime filenameEndTime = comb.Subs1.EndTime;
            
          // Apply pad (if requested)
          if (Settings.Instance.VideoClips.PadEnabled)
          {
            startTime = UtilsSubs.applyTimePad(startTime, -Settings.Instance.VideoClips.PadStart);
            endTime = UtilsSubs.applyTimePad(endTime, Settings.Instance.VideoClips.PadEnd);
            filenameStartTime = UtilsSubs.applyTimePad(comb.Subs1.StartTime, -Settings.Instance.VideoClips.PadStart);
            filenameEndTime = UtilsSubs.applyTimePad(comb.Subs1.EndTime, Settings.Instance.VideoClips.PadEnd);
          }

          // Create output filename
          string nameStr = name.createName(ConstantSettings.VideoFilenameFormat, (int)episodeCount + Settings.Instance.EpisodeStartNumber - 1,
            progessCount, filenameStartTime, filenameEndTime, comb.Subs1.Text, comb.Subs2.Text);

          string outFile = string.Format("{0}{1}{2}{3}",
                                         workerVars.MediaDir,          // {0}
                                         Path.DirectorySeparatorChar,  // {1}
                                         nameStr,                      // {2}
                                         videoExtension);              // {3}

          // Cut video clip for current line
          UtilsVideo.cutVideo(tempVideoFilename, startTime, endTime, outFile);
        }

        File.Delete(tempVideoFilename);
      }

      return true;
    }

 

  }
}
