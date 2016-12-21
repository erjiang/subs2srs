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
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace subs2srs
{
  /// <summary>
  /// The Extract Audio From Media dialog.
  /// </summary>
  public partial class DialogExtractAudioFromMedia : Form
  {
    // The global settings prior to entering this dialog. Restored when dialog is exited.
    private SaveSettings oldSettings = new SaveSettings();

    private DialogProgress dialogProgress = null;
    private string lastDirPath = "";
    private int invalidCount = 0;
    private DateTime workerStartTime;

    private string[] mediaFiles;
    private string deckName;
    private string outputDir;
    private int episodeStartNumber;
    private bool isSingleFile;
    private DateTime clipLength;
    private int bitrate;
    private bool useSpan;
    private DateTime spanStart;
    private DateTime spanEnd;
    private InfoStream audioStream;

    public string MediaFilePattern
    {
      set
      {
        this.textBoxMediaFile.Text = value;
      }
    }

    public string Subs1FilePattern
    {
      set
      {
        this.textBoxSubs1File.Text = value;
      }
    }

    public string Subs2FilePattern
    {
      set
      {
        this.textBoxSubs2File.Text = value;
      }
    }

    public string OutputDir
    {
      set
      {
        this.textBoxOutputDir.Text = value;
      }
    }

    public string DeckName
    {
      set
      {
        this.textBoxName.Text = value;
      }
    }

    public int EpisodeStartNumber
    {
      set
      {
        this.numericUpDownEpisodeStartNumber.Value = (decimal)value;
      }
    }


    public bool UseSubs1Timings
    {
      set
      {
        this.radioButtonTimingSubs1.Checked = value;
        this.radioButtonTimingSubs2.Checked = !this.radioButtonTimingSubs1.Checked;
      }
    }

    public bool UseTimeShift
    {
      set
      {
        this.groupBoxCheckTimeShift.Checked = value;
      }
    }

    public int TimeShiftSubs1
    {
      set
      {
        this.numericUpDownTimeShiftSubs1.Value = (decimal)value;
      }
    }

    public int TimeShiftSubs2
    {
      set
      {
        this.numericUpDownTimeShiftSubs2.Value = (decimal)value;
      }
    }

    public int Bitrate
    {
      set
      {
        // Find the index of the provided bitrate
        for (int i = 0; i < this.comboBoxBitrate.Items.Count; i++)
        {
          if (Int32.Parse((string)this.comboBoxBitrate.Items[i]) == value)
          {
            this.comboBoxBitrate.SelectedIndex = i;
          }
        }
      }
    }

    public bool SpanEnabled
    {
      set
      {
        this.groupBoxCheckSpan.Checked = value;
      }
    }

    public string SpanStart
    {
      set
      {
        this.maskedTextBoxSpanStart.Text = value;
      }
    }

    public string SpanEnd
    {
      set
      {
        this.maskedTextBoxSpanEnd.Text = value;
      }
    }

    public string EncodingSubs1
    {
      set
      {
        this.comboBoxEncodingSubs1.Text = value;
      }
    }


    public string EncodingSubs2
    {
      set
      {
        this.comboBoxEncodingSubs2.Text = value;
      }
    }


    // Note: Set AFTER setting the MediaDir so that the Audio Stream combo
    //       has a chance to be updated
    public int AudioStreamIndex
    {
      set
      {
        this.comboBoxStreamAudioFromVideo.SelectedIndex = value;
      }
    }


    public string FileBrowserStartDir
    {
      get 
      { 
        string dir = "";

        if(Directory.Exists(this.lastDirPath))
        {
          dir = lastDirPath;
        }

        return dir;
      }

      set 
      { 
        this.lastDirPath = value; 
      }
    }


    public DialogExtractAudioFromMedia()
    {
      InitializeComponent();

      this.initEncodingsDropdown();

    }


    private void initEncodingsDropdown()
    {
      foreach (InfoEncoding encoding in InfoEncoding.getEncodings())
      {
        this.comboBoxEncodingSubs1.Items.Add(encoding.LongName);
        this.comboBoxEncodingSubs2.Items.Add(encoding.LongName);
      }
    }


    private void DialogAudioRipper_Load(object sender, EventArgs e)
    {
      // Where the cursor should appear on startup
      this.ActiveControl = textBoxMediaFile;

      // Save the global settings
      SaveSettings curSettings = new SaveSettings();
      curSettings.gatherData();
      this.oldSettings = ObjectCopier.Clone<SaveSettings>(curSettings);

      // Fill out some controls with global settings data
      this.checkBoxRemoveLinesWithoutCounterpartSubs1.Checked = Settings.Instance.Subs[0].RemoveNoCounterpart;
      this.checkBoxRemoveLinesWithoutCounterpartSubs2.Checked = Settings.Instance.Subs[1].RemoveNoCounterpart;
      this.checkBoxRemoveStyledLinesSubs1.Checked = Settings.Instance.Subs[0].RemoveStyledLines;
      this.checkBoxRemoveStyledLinesSubs2.Checked = Settings.Instance.Subs[1].RemoveStyledLines;
    }


    private void buttonMediaFile_Click(object sender, EventArgs e)
    {
      textBoxMediaFile.Text = showFileDialog(textBoxMediaFile.Text.Trim(),
        "All files (*.*)|*.*|" +
        "All Common Video and Audio Files (*.avi;*.flv;*.mkv;*.mp4;*.ogm;*.vob;*.mp3;*.ogg;*.wav;*.aac;*.m4a;*.flac;*.wma)|*.avi;*.flv;*.mkv;*.mp4;*.ogm;*.vob;*.mp3;*.ogg;*.wav;*.aac;*.m4a;*.flac;*.wma|" +
        "All Common Video Files (*.avi;*.flv;*.mkv;*.mp4;*.ogm;*.vob)|*.avi;*.flv;*.mkv;*.mp4;*.ogm;*.vob|" +
        "All Common Audio Files (*.aac;*.flac;*.m4a;*.mp3;*.ogg;*.wav;*.wma)|*.aac;*.flac;*.m4a;*.mp3;*.ogg;*.wav;*.wma|" +
        "Audio Video Interleave (*.avi;)|*.avi|" +
        "Flash Video (*.flv)|*.flv|" +
        "Matroska Multimedia (*.mkv)|*.mkv|" +
        "MPEG4 Video (*.mp4)|*.mp4|" +
        "Ogg Multimedia (*.ogm)|*.ogm|" +
        "DVD Video Object (*.vob)|*.vob|" +
        "Advanced Audio Coding (*.aac)|*.aac|" +
        "Free Lossless Audio Codec (*.flac)|*.flac|" +
        "MPEG4 Audio (*.m4a)|*.m4a|" +
        "MPEG3 (*.mp3)|*.mp3|" +
        "Ogg Vorbis (*.ogg)|*.ogg|" +
        "WAV (*.wav)|*.wav|" +
        "Windows Media Audio (*.wma)|*.wma", 2);
    }


    private void buttonOut_Click(object sender, EventArgs e)
    {
      textBoxOutputDir.Text = showFolderDialog(textBoxOutputDir.Text);
    }


    /// <summary>
    /// Open folder dialog (starting at the last folder) and get the selected folder.
    /// </summary>
    private string showFolderDialog(string currentPath)
    {
      string ret = currentPath;

      if (Directory.Exists(currentPath))
      {
        this.folderBrowserDialog.SelectedPath = currentPath;
      }
      else if (lastDirPath != "")
      {
        this.folderBrowserDialog.SelectedPath = lastDirPath;
      }

      if (this.folderBrowserDialog.ShowDialog() == DialogResult.OK)
      {
        ret = this.folderBrowserDialog.SelectedPath;
        lastDirPath = this.folderBrowserDialog.SelectedPath;
      }

      return ret;
    }


    /// <summary>
    /// Open folder dialog (starting at the current file) and get the selected file
    /// </summary>
    private string showFileDialog(string currentFilePattern, string filter, int filterIndex)
    {
      string selectedFile = currentFilePattern;
      string curDir = "";

      this.openFileDialog.FileName = "";

      this.openFileDialog.Filter = filter;

      openFileDialog.FilterIndex = filterIndex;

      try
      {
        curDir = (Path.GetDirectoryName(currentFilePattern));
      }
      catch
      {
        curDir = "";
      }

      if (Directory.Exists(curDir))
      {
        this.openFileDialog.InitialDirectory = curDir;
      }
      else if (lastDirPath != "")
      {
        this.openFileDialog.InitialDirectory = lastDirPath;
      }

      if (this.openFileDialog.ShowDialog() == DialogResult.OK)
      {
        selectedFile = this.openFileDialog.FileName;
        lastDirPath = Path.GetDirectoryName(selectedFile);
      }

      return selectedFile;
    }


    private void radioButtonMultiple_CheckedChanged(object sender, EventArgs e)
    {
      maskedTextBoxClipLength.Enabled = ((RadioButton)sender).Checked;
    }


    private void DialogAudioRipper_FormClosing(object sender, FormClosingEventArgs e)
    {
      // Restore the global settings to the state prior to loading this dialog
      Settings.Instance.loadSettings(this.oldSettings);
    }


    /// <summary>
    /// Update the global settings based on GUI.
    /// </summary>
    private void updateSettings()
    {
      mediaFiles = UtilsCommon.getNonHiddenFiles(textBoxMediaFile.Text.Trim());
      audioStream = (InfoStream)comboBoxStreamAudioFromVideo.SelectedItem;
      outputDir = textBoxOutputDir.Text.Trim();
      deckName = textBoxName.Text.Trim().Replace(" ", "_");
      episodeStartNumber = Convert.ToInt32(numericUpDownEpisodeStartNumber.Text.Trim());
      isSingleFile = radioButtonSingle.Checked;

      if (groupBoxCheckLyrics.Checked)
      {
        Settings.Instance.Subs[0].FilePattern = textBoxSubs1File.Text.Trim();
        Settings.Instance.Subs[0].TimingsEnabled = radioButtonTimingSubs1.Checked;
        Settings.Instance.Subs[0].TimeShift = (int)numericUpDownTimeShiftSubs1.Value;
        Settings.Instance.Subs[0].Files = UtilsSubs.getSubsFiles(Settings.Instance.Subs[0].FilePattern).ToArray();
        Settings.Instance.Subs[0].Encoding = InfoEncoding.longToShort(this.comboBoxEncodingSubs1.Text);
        Settings.Instance.Subs[0].RemoveNoCounterpart = this.checkBoxRemoveLinesWithoutCounterpartSubs1.Checked;
        Settings.Instance.Subs[0].RemoveStyledLines = this.checkBoxRemoveStyledLinesSubs1.Checked;

        Settings.Instance.Subs[1].FilePattern = textBoxSubs2File.Text.Trim();
        Settings.Instance.Subs[1].TimingsEnabled = radioButtonTimingSubs2.Checked;
        Settings.Instance.Subs[1].TimeShift = (int)numericUpDownTimeShiftSubs2.Value;
        Settings.Instance.Subs[1].Encoding = InfoEncoding.longToShort(this.comboBoxEncodingSubs2.Text);
        Settings.Instance.Subs[1].RemoveNoCounterpart = this.checkBoxRemoveLinesWithoutCounterpartSubs2.Checked;
        Settings.Instance.Subs[1].RemoveStyledLines = this.checkBoxRemoveStyledLinesSubs2.Checked;

        if (Settings.Instance.Subs[1].FilePattern.Trim().Length > 0)
        {
          Settings.Instance.Subs[1].Files = UtilsSubs.getSubsFiles(Settings.Instance.Subs[1].FilePattern).ToArray();
        }
        else
        {
          Settings.Instance.Subs[1].Files = new string[0];
        }

        Settings.Instance.TimeShiftEnabled = groupBoxCheckTimeShift.Checked;
      }
      
      if (!isSingleFile)
      {
        clipLength = UtilsSubs.stringToTime(maskedTextBoxClipLength.Text.Trim());
      }

      useSpan = this.groupBoxCheckSpan.Checked;

      if (useSpan)
      {
        spanStart = UtilsSubs.stringToTime(maskedTextBoxSpanStart.Text.Trim());
        spanEnd = UtilsSubs.stringToTime(maskedTextBoxSpanEnd.Text.Trim());
      }

      bitrate = Convert.ToInt32(comboBoxBitrate.Text.Trim());
    }


    /// <summary>
    /// Extract the audio from the media.
    /// </summary>
    private void buttonExtract_Click(object sender, EventArgs e)
    {
      errorProvider1.Clear();

      if (validateForm())
      {
        updateSettings();

        Logger.Instance.info("Extract Audio From Media: GO!");
        Logger.Instance.writeSettingsToLog();

        // Start the worker thread
        try
        {
          WorkerVars workerVars = new WorkerVars(null, Settings.Instance.OutputDir, WorkerVars.SubsProcessingType.Normal);

          // Create a background thread
          BackgroundWorker bw = new BackgroundWorker();
          bw.DoWork += new DoWorkEventHandler(splitAudio);
          bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);

          // Create a progress dialog on the UI thread
          dialogProgress = new DialogProgress();

          this.workerStartTime = DateTime.Now;
          bw.RunWorkerAsync(workerVars);

          // Lock up the UI with this modal progress form
          dialogProgress.ShowDialog();
          dialogProgress = null;
        }
        catch (Exception e1)
        {
          UtilsMsg.showErrMsg("Something went wrong before processing could start.\n" + e1);
          return;
        }
      }
    }


    private void textBoxMediaFile_TextChanged(object sender, EventArgs e)
    {
      string filePattern = this.textBoxMediaFile.Text.Trim();
      string[] files = UtilsCommon.getNonHiddenFiles(filePattern);

      this.comboBoxStreamAudioFromVideo.Enabled = true;
      this.labelAudioStream.Enabled = true;

      this.comboBoxStreamAudioFromVideo.Items.Clear();

      if (files.Length != 0)
      {
        // Based on the first video file in the file pattern, get the list of available streams.
        string firstFile = files[0];
        List<InfoStream> audioStreams = UtilsVideo.getAvailableAudioStreams(firstFile);

        if (audioStreams.Count > 0)
        {
          foreach (InfoStream stream in audioStreams)
          {
            this.comboBoxStreamAudioFromVideo.Items.Add(stream);
          }
        }
        else
        {
          // Show the default stream when the available streams cannot be detected
          this.comboBoxStreamAudioFromVideo.Items.Add(new InfoStream());
        }

        this.comboBoxStreamAudioFromVideo.SelectedIndex = 0;
      }
      else
      {
        this.comboBoxStreamAudioFromVideo.Enabled = false;
        this.labelAudioStream.Enabled = false;
      }
    }


    private void buttonSubs1File_Click(object sender, EventArgs e)
    {
      textBoxSubs1File.Text = showFileDialog(textBoxSubs1File.Text.Trim(),
        "All Subtitle Files (*.ass;*.ssa;*.lrc;*.srt;*.trs;*.mkv)|*.ass;*.ssa;*.lrc;*.srt;*.trs;*.mkv|" +
        "Advanced Substation Alpha (*.ass;*.ssa)|*.ass;*.ssa|" +
        "Lyric (*.lrc)|*.lrc|" +
        "Matroska (*.mkv)|*.mkv|" +
        "SubRip (*.srt)|*.srt|" +
        "Transcriber (*.trs)|*.trs", 1);
    }


    private void buttonSubs2File_Click(object sender, EventArgs e)
    {
      textBoxSubs2File.Text = showFileDialog(textBoxSubs2File.Text.Trim(),
        "All Subtitle Files (*.ass;*.ssa;*.lrc;*.srt;*.trs;*.mkv)|*.ass;*.ssa;*.lrc;*.srt;*.trs;*.mkv|" +
        "Advanced Substation Alpha (*.ass;*.ssa)|*.ass;*.ssa|" +
        "Lyric (*.lrc)|*.lrc|" +
        "Matroska (*.mkv)|*.mkv|" +
        "SubRip (*.srt)|*.srt|" +
        "Transcriber (*.trs)|*.trs", 1);
    }


    /// <summary>
    /// Called when the audio extraction thread completes.
    /// </summary>
    private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
      if (dialogProgress != null)
      {
        dialogProgress.Hide();
        dialogProgress = null;
      }

      if (e.Error != null)
      {
        UtilsMsg.showErrMsg(e.Error.Message);
        return;
      }

      if (e.Cancelled)
      {
        MessageBox.Show("Action cancelled.", UtilsAssembly.Title, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return;
      }

      TimeSpan workerTotalTime = DateTime.Now - this.workerStartTime;

      WorkerVars workerVars = e.Result as WorkerVars;
      string endMessage = String.Format("Audio extraction completed in {0:0.00} minutes.",
        workerTotalTime.TotalMinutes);
      UtilsMsg.showInfoMsg(endMessage);
    }


    /// <summary>
    /// Performs the work of the audio extraction thread.
    /// </summary>
    private void splitAudio(object sender, DoWorkEventArgs e)
    {
      WorkerVars workerVars = e.Argument as WorkerVars;
      List<List<InfoCombined>> combinedAll = new List<List<InfoCombined>>();
      WorkerSubs subsWorker = new WorkerSubs();

      if (groupBoxCheckLyrics.Checked)
      {
        // Parse and combine the subtitles
        try
        {
          combinedAll = subsWorker.combineAllSubs(workerVars, dialogProgress);

          if (combinedAll != null)
          {
            workerVars.CombinedAll = combinedAll;
          }
          else
          {
            e.Cancel = true;
            return;
          }
        }
        catch (Exception e1)
        {
          UtilsMsg.showErrMsg("Something went wrong before processing could start.\n" + e1);
          e.Cancel = true;
          return;
        }
      }

      DateTime mediaStartime = new DateTime();
      DateTime mediaEndtime = new DateTime();
      DateTime mediaDuration = new DateTime();

      int episode = 0;

      DialogProgress.updateProgressInvoke(dialogProgress, 0, "Starting...");


      foreach (string file in mediaFiles)
      {
        episode++;

        try
        {
          mediaEndtime = UtilsVideo.getVideoLength(file);
        }
        catch (Exception e1)
        {
          UtilsMsg.showErrMsg("Something went wrong while determining duration of the media:\n" + e1);
          return;
        }

        if (useSpan)
        {
          mediaStartime = spanStart;

          // If the span end time if not greater than the actual duration of the media
          if (spanEnd < mediaEndtime)
          {
            mediaEndtime = spanEnd;
          }
        }

        UtilsName name = new UtilsName(
          deckName,
          mediaFiles.Length, // Total number of episodes
          1, // Total number of lines (Note: not filled out here)
          mediaEndtime, // Last time
          0, 0 // Width and height (Note: not filled out anywhere)
          );

        mediaDuration = UtilsSubs.getDurationTime(mediaStartime, mediaEndtime);

        string progressText = String.Format("Processing audio from media file {0} of {1}",
                                            episode,
                                            mediaFiles.Length);

        int progress = Convert.ToInt32((episode - 1) * (100.0 / mediaFiles.Length));

        DialogProgress.updateProgressInvoke(dialogProgress, progress, progressText);

        // Enable detail mode in progress dialog
        DialogProgress.enableDetailInvoke(dialogProgress, true);

        // Set the duration of the clip in the progress dialog  (for detail mode)
        DialogProgress.setDuration(dialogProgress, mediaDuration);


        string tempMp3Filename = Path.GetTempPath() + ConstantSettings.TempAudioFilename;

        UtilsAudio.ripAudioFromVideo(mediaFiles[episode - 1],
          audioStream.Num,
          mediaStartime, mediaEndtime,
          bitrate, tempMp3Filename, dialogProgress);

        DialogProgress.enableDetailInvoke(dialogProgress, false);

        if (dialogProgress.Cancel)
        {
          e.Cancel = true;
          File.Delete(tempMp3Filename);
          return;
        }

        int numClips = 1;

        if (!isSingleFile)
        {
          numClips = (int)Math.Ceiling((mediaDuration.TimeOfDay.TotalMilliseconds / (clipLength.TimeOfDay.TotalSeconds * 1000.0)));
        }

        for (int clipIdx = 0; clipIdx < numClips; clipIdx++)
        {
          progressText = String.Format("Splitting segment {0} of {1} from media file {2} of {3}",
                                       clipIdx + 1,
                                       numClips,
                                       episode,
                                       mediaFiles.Length);

          progress = Convert.ToInt32((episode - 1) * (100.0 / mediaFiles.Length));

          DialogProgress.updateProgressInvoke(dialogProgress, progress, progressText);


          if (dialogProgress.Cancel)
          {
            e.Cancel = true;
            File.Delete(tempMp3Filename);
            return;
          }

          // The start and end times used for processing
          DateTime startTime = new DateTime();
          DateTime endTime = new DateTime();

          if (isSingleFile)
          {
            endTime = mediaDuration;
          }
          else
          {
            startTime = startTime.AddSeconds((double)(clipLength.TimeOfDay.TotalSeconds * clipIdx));
            endTime = endTime.AddSeconds((double)(clipLength.TimeOfDay.TotalSeconds * (clipIdx + 1)));

            if (endTime.TimeOfDay.TotalMilliseconds >= mediaDuration.TimeOfDay.TotalMilliseconds)
            {
              endTime = mediaDuration;
            }
          }

          // The start and end times that will be displayed
          DateTime startTimeName = startTime.AddMilliseconds(mediaStartime.TimeOfDay.TotalMilliseconds);
          DateTime endTimeName = endTime.AddMilliseconds(mediaStartime.TimeOfDay.TotalMilliseconds);

          // Fill in the total number of lines with the total number of clips
          name.TotalNumLines = numClips;

          string nameStr = name.createName(ConstantSettings.ExtractMediaAudioFilenameFormat, episode + episodeStartNumber - 1,
            clipIdx + 1, startTimeName, endTimeName, "", "");
          

          string outName = String.Format("{0}{1}{2}",
                                         outputDir,                    // {0}
                                         Path.DirectorySeparatorChar,  // {1}
                                         nameStr);                     // {2}

          UtilsAudio.cutAudio(tempMp3Filename, startTime, endTime, outName);

          nameStr = name.createName(ConstantSettings.AudioId3Artist, episode + episodeStartNumber - 1,
            clipIdx + 1, startTimeName, endTimeName, "", "");

          string tagArtist = String.Format("{0}",
                                          nameStr); // {0}

          nameStr = name.createName(ConstantSettings.AudioId3Album, episode + episodeStartNumber - 1,
            clipIdx + 1, startTimeName, endTimeName, "", "");

          string tagAlbum = String.Format("{0}",
                                          nameStr); // {0}

          nameStr = name.createName(ConstantSettings.AudioId3Title, episode + episodeStartNumber - 1,
            clipIdx + 1, startTimeName, endTimeName, "", "");

          string tagTitle = String.Format("{0}",
                                          nameStr); // {0}

          nameStr = name.createName(ConstantSettings.AudioId3Genre, episode + episodeStartNumber - 1,
            clipIdx + 1, startTimeName, endTimeName, "", "");

          string tagGenre = String.Format("{0}",
                                          nameStr); // {0}

          string tagLyrics = "";

          if (groupBoxCheckLyrics.Checked)
          {
            int totalLyricsLines = 0;
            int curLyricsNum = 1;

            // Precount the number of lyrics lines
            foreach (InfoCombined comb in combinedAll[episode - 1])
            {
              if (comb.Subs1.StartTime.TimeOfDay.TotalMilliseconds >= startTimeName.TimeOfDay.TotalMilliseconds
                && comb.Subs1.StartTime.TimeOfDay.TotalMilliseconds <= endTimeName.TimeOfDay.TotalMilliseconds)
              {
                totalLyricsLines++;
              }
            }

            // Fill in the total number of lyrics lines
            name.TotalNumLines = curLyricsNum;

            // Foreach comb in the current episode, if the comb lies within the
            // current clip, add it to the lryics tag
            foreach (InfoCombined comb in combinedAll[episode - 1])
            {
              if (comb.Subs1.StartTime.TimeOfDay.TotalMilliseconds >= startTimeName.TimeOfDay.TotalMilliseconds
                && comb.Subs1.StartTime.TimeOfDay.TotalMilliseconds <= endTimeName.TimeOfDay.TotalMilliseconds)
              {
                tagLyrics += formatLyricsPair(comb, name, startTimeName, episode + episodeStartNumber - 1, curLyricsNum) + "\r\n";
                curLyricsNum++;
              }
            }
          }

          UtilsAudio.tagAudio(outName,
            tagArtist,
            tagAlbum,
            tagTitle,
            tagGenre,
            tagLyrics,
            clipIdx + 1,
            numClips);
        }
      }

      return;
    }


    /// <summary>
    /// Format a pair of lyrics.
    /// </summary>
    private string formatLyricsPair(InfoCombined comb, UtilsName name, DateTime clipStartTime, int episode, int sequenceNum)
    {
      string pair = "";

      string subs1Text = comb.Subs1.Text;
      string subs2Text = comb.Subs2.Text;

      DateTime lyricStartTime = new DateTime();
      DateTime lyricEndTime = new DateTime();

      lyricStartTime = lyricStartTime.AddMilliseconds(comb.Subs1.StartTime.TimeOfDay.TotalMilliseconds - clipStartTime.TimeOfDay.TotalMilliseconds);
      lyricEndTime = lyricEndTime.AddMilliseconds(comb.Subs1.StartTime.TimeOfDay.TotalMilliseconds - clipStartTime.TimeOfDay.TotalMilliseconds);

      string nameStr = name.createName(ConstantSettings.ExtractMediaLyricsSubs1Format, episode,
        sequenceNum, lyricStartTime, lyricEndTime, subs1Text, subs2Text);


      pair += String.Format("{0}",
                            nameStr);

      if (this.textBoxSubs2File.Text.Trim().Length > 0)
      {
        if (ConstantSettings.ExtractMediaLyricsSubs2Format != "")
        {
          nameStr = name.createName(ConstantSettings.ExtractMediaLyricsSubs2Format, episode,
            sequenceNum, lyricStartTime, lyricEndTime, subs1Text, subs2Text);

          pair += "\r\n";
          pair += String.Format("{0}",
                                nameStr);
        }
      }

      return pair;
    }
  

    /// <summary>
    /// Determine if file is of a supported subtitle format for use in lyrics.
    /// </summary>
    private bool isSupportedSubtitleFormat(string file)
    {
      string ext = file.Substring(file.LastIndexOf(".")).ToLower();
      bool ret = false;

      if ((ext == ".srt") || (ext == ".ass") || (ext == ".ssa") || (ext == ".lrc") || (ext == ".trs"))
      {
        ret = true;
      }

      return ret;
    }


    /// <summary>
    /// Validate GUI. Inform user if they entered invalid data.
    /// </summary>
    private bool validateForm()
    {
      bool status = false;
      invalidCount = 0;

      this.ValidateChildren();

      if (invalidCount == 0)
      {
        status = true;
      }
      else if (invalidCount == 1)
      {
        UtilsMsg.showErrMsg("Please correct the error on this form."
          + "\r\n\r\nHover the mouse over the red error bubble"
          + "\r\nto determine the nature of the error.");
      }
      else
      {
        UtilsMsg.showErrMsg(String.Format("Please correct the {0} errors on this form.", invalidCount)
          + "\r\n\r\nHover the mouse over the red error bubbles"
           + "\r\nto determine the nature of the errors.");
      }

      return status;
    }


    private void textBoxMediaFile_Validating(object sender, CancelEventArgs e)
    {
      string error = null;
      string filePattern = ((TextBox)sender).Text.Trim();
      string[] files = UtilsCommon.getNonHiddenFiles(filePattern);

      // Do error checking here
      
      errorProvider1.SetError((Control)sender, error);
    }


    private void textBoxOut_Validating(object sender, CancelEventArgs e)
    {
      string error = null;
      string dir = ((TextBox)sender).Text.Trim();

      if (!Directory.Exists(dir))
      {
        error = "Directory does not exist";
        invalidCount++;
      }

      errorProvider1.SetError((Control)sender, error);
    }


    private void textBoxName_Validating(object sender, CancelEventArgs e)
    {
      string error = null;
      string name = ((TextBox)sender).Text.Trim();

      if (name == "")
      {
        error = "Must consist of 1 or more characters";
        invalidCount++;
      }
      else if (name.Contains("\\") || name.Contains("/") || name.Contains(":") ||
               name.Contains("*") || name.Contains("?") || name.Contains("\"") ||
               name.Contains("<") || name.Contains(">") || name.Contains("|"))
      {
        error = "Must not contain any of the following characters:\n \\ / : * ? \" < > |";
        invalidCount++;
      }

      errorProvider1.SetError((Control)sender, error);
    }


    private void maskedTextBoxClipLength_Validating(object sender, CancelEventArgs e)
    {
      string error = null;
      string span = ((MaskedTextBox)sender).Text.Trim();
      Regex spanRegex = new Regex(@"^(?<Hours>\d):(?<Mins>[0-5]\d):(?<Secs>[0-5]\d)$");

      if (radioButtonMultiple.Checked)
      {
        if (!spanRegex.IsMatch(span))
        {
          error = "Please use the h:mm:ss format.\n\nRanges:\nh: [0-9]\nmm: [00-59]\nss: [00-59]";
          invalidCount++;
        }
      }

      errorProvider1.SetError((Control)sender, error);
    }


    private void maskedTextBoxSpan_Validating(object sender, CancelEventArgs e)
    {
      string error = null;
      string span = ((MaskedTextBox)sender).Text.Trim();
      Regex spanRegex = new Regex(@"^(?<Hours>\d):(?<Mins>[0-5]\d):(?<Secs>[0-5]\d)$");

      if (groupBoxCheckSpan.Checked)
      {
        if (!spanRegex.IsMatch(span))
        {
          error = "Please use the h:mm:ss format.\n\nRanges:\nh: [0-9]\nmm: [00-59]\nss: [00-59]";
          invalidCount++;
        }
      }

      errorProvider1.SetError((Control)sender, error);
    }


    private void textBoxSubs1File_Validating(object sender, CancelEventArgs e)
    {
      string error = null;
      string filePattern = ((TextBox)sender).Text.Trim();
      string[] files = UtilsCommon.getNonHiddenFiles(filePattern);

      if (groupBoxCheckLyrics.Checked)
      {
        if ((error == null) && (UtilsSubs.getNumSubsFiles(filePattern) == 0))
        {
          error = "Please provide a valid subtitle file. \nOnly .srt, .ass, .ssa, .lrc and .trs are allowed.";
          invalidCount++;
        }

        if ((error == null))
        {
          foreach (string f in files)
          {
            if (!isSupportedSubtitleFormat(f))
            {
              error = "Please provide a valid subtitle file. \nOnly .srt, .ass, .ssa, .lrc and .trs are allowed.";
              invalidCount++;
              break;
            }
          }
        }

        if ((error == null) && (UtilsSubs.getNumSubsFiles(filePattern) != UtilsCommon.getNonHiddenFiles(textBoxMediaFile.Text.Trim()).Length))
        {
          error = "The number of files here must match\nthe number of files in Media";
          invalidCount++;
        }
      }

      errorProvider1.SetError((Control)sender, error);
    }


    private void textBoxSubs2File_Validating(object sender, CancelEventArgs e)
    {
      string error = null;
      string filePattern = ((TextBox)sender).Text.Trim();
      string[] files = UtilsCommon.getNonHiddenFiles(filePattern);

      if (groupBoxCheckLyrics.Checked)
      {
        if ((radioButtonTimingSubs2.Checked) && (filePattern == ""))
        {
          error = "Since you want to use subs2 timings,\nplease enter a valid Subs2 subtitle file here";
          invalidCount++;
        }

        if ((error == null) && (filePattern != ""))
        {
          if ((error == null) && (UtilsSubs.getNumSubsFiles(filePattern) == 0))
          {
            error = "Please provide a valid subtitle file. \nOnly .srt, .ass, .ssa, .lrc and .trs are allowed.";
            invalidCount++;
          }

          if ((error == null))
          {
            foreach (string f in files)
            {
              if (!isSupportedSubtitleFormat(f))
              {
                error = "Please provide a valid subtitle file. \nOnly .srt, .ass, .ssa, .lrc and .trs are allowed.";
                invalidCount++;
                break;
              }
            }
          }

          if ((error == null) && (UtilsSubs.getNumSubsFiles(filePattern) != UtilsCommon.getNonHiddenFiles(textBoxMediaFile.Text.Trim()).Length))
          {
            error = "The number of files here must match\nthe number of files in Media";
            invalidCount++;
          }
        }
      }

      errorProvider1.SetError((Control)sender, error);
    }


    private void textBoxSubs1File_TextChanged(object sender, EventArgs e)
    {
      this.updateSubs(1);
    }


    private void textBoxSubs2File_TextChanged(object sender, EventArgs e)
    {
      this.updateSubs(2);
    }


    /// <summary>
    /// Update the information related to Subs1 or Subs2.
    /// </summary>
    private void updateSubs(int subsNum)
    {
      TextBox textbox;
      string file;

      // Get items that depend on whether it's subs1 or subs2
      if (subsNum == 1)
      {
        textbox = this.textBoxSubs1File;
        file = this.textBoxSubs1File.Text.Trim();
      }
      else
      {
        textbox = this.textBoxSubs2File;
        file = this.textBoxSubs2File.Text.Trim();
      }

      // If input file is an MKV, show the MKV track select dialog
      if (Path.GetExtension(file) == ".mkv")
      {
        List<MkvTrack> allSubTrackList = UtilsMkv.getSubtitleTrackList(file);
        List<MkvTrack> subTrackList = new List<MkvTrack>();

        // Remove VOBSUB tracks
        foreach (MkvTrack subTrack in allSubTrackList)
        {
          if (subTrack.Extension != "sub")
          {
            subTrackList.Add(subTrack);
          }
        }

        if (subTrackList.Count == 0)
        {
          UtilsMsg.showInfoMsg("This .mkv file does not contain any subtitle tracks.");
          textbox.Text = "";
        }
        else
        {
          DialogSelectMkvTrack mkvDlg = new DialogSelectMkvTrack(file, subsNum, subTrackList);
          DialogResult result = mkvDlg.ShowDialog();

          if (result == DialogResult.OK)
          {
            textbox.Text = mkvDlg.ExtractedFile;
          }
          else
          {
            textbox.Text = "";
          }
        }

        return; // Since textbox.Text was changed, this routine will be called again
      }
    }


 


  }
}
