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
  /// Responsible for processing snapshots in the worker thread.
  /// </summary>
  public class WorkerSnapshot
  {
    /// <summary>
    /// Generate snapshots for all episodes.
    /// </summary>
    public bool genSnapshots(WorkerVars workerVars, DialogProgress dialogProgress)
    {
      int progessCount = 0;
      int episodeCount = 0;
      int totalEpisodes = workerVars.CombinedAll.Count;
      int totalLines = UtilsSubs.getTotalLineCount(workerVars.CombinedAll);
      DateTime lastTime = UtilsSubs.getLastTime(workerVars.CombinedAll);

      UtilsName name = new UtilsName(Settings.Instance.DeckName, totalEpisodes,
        totalLines, lastTime, Settings.Instance.VideoClips.Size.Width, Settings.Instance.VideoClips.Size.Height);

      // For each episode
      foreach (List<InfoCombined> combArray in workerVars.CombinedAll)
      {
        episodeCount++;

        // For each line in episode, generate a snapshot
        for (int i = 0; i < combArray.Count; i++)
        {
          progessCount++;

          string progressText = string.Format("Generating snapshot: {0} of {1}",
                                              progessCount.ToString(),
                                              totalLines.ToString());

          int progress = Convert.ToInt32(progessCount * (100.0 / totalLines));

          // Update the progress dialog
          DialogProgress.updateProgressInvoke(dialogProgress, progress, progressText);

          InfoCombined comb = combArray[i];
          DateTime startTime = comb.Subs1.StartTime;
          DateTime endTime = comb.Subs1.EndTime;
          DateTime midTime = UtilsSubs.getMidpointTime(startTime, endTime);

          string videoFileName = Settings.Instance.VideoClips.Files[episodeCount - 1];

          // Create output filename
          string nameStr = name.createName(ConstantSettings.SnapshotFilenameFormat, 
            (int)episodeCount + Settings.Instance.EpisodeStartNumber - 1,
            progessCount, startTime, endTime, comb.Subs1.Text, comb.Subs2.Text);

          string outFile = string.Format("{0}{1}{2}",
                                          workerVars.MediaDir,          // {0}
                                          Path.DirectorySeparatorChar,  // {1}
                                          nameStr);                     // {2}

          // Generate snapshot
          UtilsSnapshot.takeSnapshotFromVideo(videoFileName, midTime, Settings.Instance.Snapshots.Size,
            Settings.Instance.Snapshots.Crop, outFile);

          // Did the user press the cancel button?
          if (dialogProgress.Cancel)
          {
            return false;
          }                                                               
        }
      }

      return true;
    }



  }
}
