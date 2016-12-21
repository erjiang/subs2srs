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
  /// Responsible for processing audio in the worker thread.
  /// </summary>
  public class WorkerAudio
  {

    /// <summary>
    /// Generate Audio clips for all episodes.
    /// </summary>
    public bool genAudioClip(WorkerVars workerVars, DialogProgress dialogProgress)
    {
      int progessCount = 0;
      int episodeCount = 0;
      int totalEpisodes = workerVars.CombinedAll.Count;
      int curEpisodeCount = 0;
      int totalLines = UtilsSubs.getTotalLineCount(workerVars.CombinedAll);
      DateTime lastTime = UtilsSubs.getLastTime(workerVars.CombinedAll);

      UtilsName name = new UtilsName(Settings.Instance.DeckName, totalEpisodes,
        totalLines, lastTime, Settings.Instance.VideoClips.Size.Width, Settings.Instance.VideoClips.Size.Height);

      DialogProgress.updateProgressInvoke(dialogProgress, 0, "Creating audio clips.");

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

        // Is the audio input an mp3 file?
        bool inputFileIsMp3 = (Settings.Instance.AudioClips.Files.Length > 0) 
          && (Path.GetExtension(Settings.Instance.AudioClips.Files[episodeCount - 1]).ToLower() == ".mp3");

        DateTime entireClipStartTime = combArray[0].Subs1.StartTime;
        DateTime entireClipEndTime = combArray[combArray.Count - 1].Subs1.EndTime;
        string tempMp3Filename = Path.GetTempPath() + ConstantSettings.TempAudioFilename;

        // Apply pad to entire clip timings  (if requested)
        if (Settings.Instance.AudioClips.PadEnabled)
        {
          entireClipStartTime = UtilsSubs.applyTimePad(entireClipStartTime, -Settings.Instance.AudioClips.PadStart);
          entireClipEndTime = UtilsSubs.applyTimePad(entireClipEndTime, Settings.Instance.AudioClips.PadEnd);
        }

        // Do we need to extract the audio from the video file?
        if (Settings.Instance.AudioClips.UseAudioFromVideo)
        {
          string progressText = String.Format("Extracting audio from video file {0} of {1}",
                                              episodeCount,    // {0}
                                              totalEpisodes);  // {1}

          bool success = convertToMp3(
            Settings.Instance.VideoClips.Files[episodeCount - 1],
            Settings.Instance.VideoClips.AudioStream.Num,
            progressText,
            dialogProgress,
            entireClipStartTime,
            entireClipEndTime,
            tempMp3Filename);

          if (!success)
          {
            UtilsMsg.showErrMsg("Failed to extract the audio from the video.\n" +
                                "Make sure that the video does not have any DRM restrictions.");
            return false;
          }
        }
        // If the reencode option is set or the input audio is not an mp3, reencode to mp3
        else if (ConstantSettings.ReencodeBeforeSplittingAudio || !inputFileIsMp3)
        {
          string progressText = String.Format("Reencoding audio file {0} of {1}",
                                          episodeCount,
                                          totalEpisodes);

          bool success = convertToMp3(
            Settings.Instance.AudioClips.Files[episodeCount - 1],
            "0",
            progressText,
            dialogProgress,
            entireClipStartTime,
            entireClipEndTime,
            tempMp3Filename);

          if (!success)
          {
            UtilsMsg.showErrMsg("Failed to reencode the audio file.\n" +
                                "Make sure that the audio file does not have any DRM restrictions.");
            return false;
          }
        }

        curEpisodeCount = 0; // Reset

        // For each line in episode, generate an audio clip
        foreach (InfoCombined comb in combArray)
        {
          progessCount++;
          curEpisodeCount++;

          int progress = Convert.ToInt32(progessCount * (100.0 / totalLines));

          string progressText = string.Format("Generating audio clip: {0} of {1}",
                                              progessCount.ToString(),
                                              totalLines.ToString());

          // Update the progress dialog
          DialogProgress.updateProgressInvoke(dialogProgress, progress, progressText);

          // Did the user press the cancel button?
          if (dialogProgress.Cancel)
          {
            File.Delete(tempMp3Filename);
            return false;
          }

          DateTime startTime = comb.Subs1.StartTime;
          DateTime endTime = comb.Subs1.EndTime;
          DateTime filenameStartTime = comb.Subs1.StartTime;
          DateTime filenameEndTime = comb.Subs1.EndTime;
          string fileToCut = "";

          if (Settings.Instance.AudioClips.UseAudioFromVideo || ConstantSettings.ReencodeBeforeSplittingAudio || !inputFileIsMp3)
          {
            startTime = UtilsSubs.shiftTiming(startTime, -((int)entireClipStartTime.TimeOfDay.TotalMilliseconds));
            endTime = UtilsSubs.shiftTiming(endTime, -((int)entireClipStartTime.TimeOfDay.TotalMilliseconds));
            fileToCut = tempMp3Filename;
          }
          else
          {
            fileToCut = Settings.Instance.AudioClips.Files[episodeCount - 1];
          }

          // Apply pad (if requested)
          if (Settings.Instance.AudioClips.PadEnabled)
          {
            startTime = UtilsSubs.applyTimePad(startTime, -Settings.Instance.AudioClips.PadStart);
            endTime = UtilsSubs.applyTimePad(endTime, Settings.Instance.AudioClips.PadEnd);
            filenameStartTime = UtilsSubs.applyTimePad(comb.Subs1.StartTime, -Settings.Instance.AudioClips.PadStart);
            filenameEndTime = UtilsSubs.applyTimePad(comb.Subs1.EndTime, Settings.Instance.AudioClips.PadEnd);
          }

          string lyricSubs2 = "";

          // Set the Subs2 lyric if it exists
          if (Settings.Instance.Subs[1].Files.Length != 0)
          {
            lyricSubs2 = comb.Subs2.Text.Trim();
          }

          // Create output filename
          string nameStr = name.createName(ConstantSettings.AudioFilenameFormat, (int)episodeCount + Settings.Instance.EpisodeStartNumber - 1,
             progessCount, filenameStartTime, filenameEndTime, comb.Subs1.Text, lyricSubs2);

          string outName = String.Format("{0}{1}{2}",
                          workerVars.MediaDir,           // {0}
                          Path.DirectorySeparatorChar,   // {1}
                          nameStr);                      // {2}   

          // Create audio clip
          UtilsAudio.cutAudio(fileToCut, startTime, endTime, outName);

          // Tag the audio clip
          this.tagAudio(name, outName, episodeCount, curEpisodeCount, progessCount, combArray.Count,
            filenameStartTime, filenameEndTime, comb.Subs1.Text, lyricSubs2);          
        }

        File.Delete(tempMp3Filename);
      }

      // Normalize all mp3 files in the media directory
      if (Settings.Instance.AudioClips.Normalize)
      {
        DialogProgress.updateProgressInvoke(dialogProgress, -1, "Normalizing audio...");

        UtilsAudio.normalizeAudio(workerVars.MediaDir);
      }

      return true;
    }


    /// <summary>
    /// Apply tag to audio file.
    /// </summary>
    private void tagAudio(UtilsName name, string outName, int episodeCount, int curEpisodeCount, int progessCount, int totalTracks,
      DateTime filenameStartTime, DateTime filenameEndTime, string lyricSubs1, string lyricSubs2)
    {
      int episodeNum = episodeCount + Settings.Instance.EpisodeStartNumber - 1;

      string tagArtist = name.createName(ConstantSettings.AudioId3Artist, episodeNum,
        progessCount, filenameStartTime, filenameEndTime, lyricSubs1, lyricSubs2);

      string tagAlbum = name.createName(ConstantSettings.AudioId3Album, episodeNum,
        progessCount, filenameStartTime, filenameEndTime, lyricSubs1, lyricSubs2);

      string tagTitle = name.createName(ConstantSettings.AudioId3Title, episodeNum,
        progessCount, filenameStartTime, filenameEndTime, lyricSubs1, lyricSubs2);

      string tagGenre = name.createName(ConstantSettings.AudioId3Genre, episodeNum,
        progessCount, filenameStartTime, filenameEndTime, lyricSubs1, lyricSubs2);

      string tagLyrics = name.createName(ConstantSettings.AudioId3Lyrics, episodeNum,
        progessCount, filenameStartTime, filenameEndTime, lyricSubs1, lyricSubs2);

      UtilsAudio.tagAudio(outName,
        tagArtist,
        tagAlbum,
        tagTitle,
        tagGenre,
        tagLyrics,
        curEpisodeCount,
        totalTracks);
    }


    /// <summary>
    /// Convert audio or video to mp3 and display progress dialog.
    /// </summary>
    private bool convertToMp3(string file, string stream, string progressText, DialogProgress dialogProgress, 
      DateTime entireClipStartTime, DateTime entireClipEndTime, string tempMp3Filename)
    {
      bool status = true;

      DateTime entireClipDuration = UtilsSubs.getDurationTime(entireClipStartTime, entireClipEndTime);

      DialogProgress.updateProgressInvoke(dialogProgress, progressText);

      // Enable detail mode in progress dialog
      DialogProgress.enableDetailInvoke(dialogProgress, true);

      // Set the duration of the clip in the progress dialog  (for detail mode)
      DialogProgress.setDuration(dialogProgress, entireClipDuration);

      // Rip the audio to a temporary mp3 file
      UtilsAudio.ripAudioFromVideo(file,
        stream,
        entireClipStartTime, entireClipEndTime,
        Settings.Instance.AudioClips.Bitrate, tempMp3Filename, dialogProgress);

      DialogProgress.enableDetailInvoke(dialogProgress, false);

      FileInfo fileInfo = new FileInfo(tempMp3Filename);

      // Error if the temporary mp3 file doesn't exist or is zero bytes
      if (!File.Exists(tempMp3Filename) || fileInfo.Length == 0)
      {
        status = false;
      }

      return status;
    }



  }
}
