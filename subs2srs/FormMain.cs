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
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace subs2srs
{
  public partial class FormMain : Form
  {
    private DialogAdvancedSubtitleOptions dlgAdvSubs;
    private DialogPreview dlgPreview;
    private DialogVideoDimensionsChooser dlgSnapshotDimensionsChooser;
    private DialogVideoDimensionsChooser dlgVideoClipDimensionsChooser;

    private int invalidCount = 0;
    private string lastDirPath = "";

    public FormMain()
    {
      InitializeComponent();

      this.Text = UtilsAssembly.Title;

      dlgAdvSubs = new DialogAdvancedSubtitleOptions();
      dlgSnapshotDimensionsChooser = new DialogVideoDimensionsChooser(DialogVideoDimensionsChooser.VideoDimesionsChoooserType.Snapshot);
      dlgVideoClipDimensionsChooser = new DialogVideoDimensionsChooser(DialogVideoDimensionsChooser.VideoDimesionsChoooserType.Video);

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


    /// <summary>
    /// Read the preferences file and set global variables
    /// </summary>
    private void readPreferencesFile()
    {
      // Read the settings file
      PrefIO.read();

      Settings.Instance.AudioClips.Enabled = ConstantSettings.DefaultEnableAudioClipGeneration;
      Settings.Instance.Snapshots.Enabled = ConstantSettings.DefaultEnableSnapshotsGeneration;
      Settings.Instance.VideoClips.Enabled = ConstantSettings.DefaultEnableVideoClipsGeneration;
      Settings.Instance.AudioClips.Bitrate = ConstantSettings.DefaultAudioClipBitrate;
      Settings.Instance.AudioClips.Normalize = ConstantSettings.DefaultAudioNormalize;
      Settings.Instance.VideoClips.BitrateVideo = ConstantSettings.DefaultVideoClipVideoBitrate;
      Settings.Instance.VideoClips.BitrateAudio = ConstantSettings.DefaultVideoClipAudioBitrate;
      Settings.Instance.VideoClips.IPodSupport = ConstantSettings.DefaultIphoneSupport;
      Settings.Instance.Subs[0].Encoding = ConstantSettings.DefaultEncodingSubs1;
      Settings.Instance.Subs[1].Encoding = ConstantSettings.DefaultEncodingSubs2;
      Settings.Instance.ContextLeadingCount = ConstantSettings.DefaultContextNumLeading;
      Settings.Instance.ContextTrailingCount = ConstantSettings.DefaultContextNumTrailing;
      Settings.Instance.ContextLeadingRange = ConstantSettings.DefaultContextLeadingRange;
      Settings.Instance.ContextTrailingRange = ConstantSettings.DefaultContextTrailingRange;

      if(Directory.Exists(ConstantSettings.DefaultFileBrowserStartDir))
      {
        lastDirPath = ConstantSettings.DefaultFileBrowserStartDir;
      }

      Settings.Instance.Subs[0].RemoveNoCounterpart = ConstantSettings.DefaultRemoveNoCounterpartSubs1;
      Settings.Instance.Subs[1].RemoveNoCounterpart = ConstantSettings.DefaultRemoveNoCounterpartSubs2;
      Settings.Instance.Subs[0].RemoveStyledLines = ConstantSettings.DefaultRemoveStyledLinesSubs1;
      Settings.Instance.Subs[1].RemoveStyledLines = ConstantSettings.DefaultRemoveStyledLinesSubs2;
      Settings.Instance.Subs[0].IncludedWords = UtilsCommon.removeExtraSpaces(ConstantSettings.DefaultIncludeTextSubs1.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries));
      Settings.Instance.Subs[1].IncludedWords = UtilsCommon.removeExtraSpaces(ConstantSettings.DefaultIncludeTextSubs2.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries));
      Settings.Instance.Subs[0].ExcludedWords = UtilsCommon.removeExtraSpaces(ConstantSettings.DefaultExcludeTextSubs1.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries));
      Settings.Instance.Subs[1].ExcludedWords = UtilsCommon.removeExtraSpaces(ConstantSettings.DefaultExcludeTextSubs2.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries));
      Settings.Instance.Subs[0].ExcludeDuplicateLinesEnabled = ConstantSettings.DefaultExcludeDuplicateLinesSubs1;
      Settings.Instance.Subs[1].ExcludeDuplicateLinesEnabled = ConstantSettings.DefaultExcludeDuplicateLinesSubs2;
      Settings.Instance.Subs[0].ExcludeFewerEnabled = ConstantSettings.DefaultExcludeLinesFewerThanCharsSubs1;
      Settings.Instance.Subs[1].ExcludeFewerEnabled = ConstantSettings.DefaultExcludeLinesFewerThanCharsSubs2;
      Settings.Instance.Subs[0].ExcludeFewerCount = ConstantSettings.DefaultExcludeLinesFewerThanCharsNumSubs1;
      Settings.Instance.Subs[1].ExcludeFewerCount = ConstantSettings.DefaultExcludeLinesFewerThanCharsNumSubs2;
      Settings.Instance.Subs[0].ExcludeShorterThanTimeEnabled = ConstantSettings.DefaultExcludeLinesShorterThanMsSubs1;
      Settings.Instance.Subs[1].ExcludeShorterThanTimeEnabled = ConstantSettings.DefaultExcludeLinesShorterThanMsSubs2;
      Settings.Instance.Subs[0].ExcludeShorterThanTime = ConstantSettings.DefaultExcludeLinesShorterThanMsNumSubs1;
      Settings.Instance.Subs[1].ExcludeShorterThanTime = ConstantSettings.DefaultExcludeLinesShorterThanMsNumSubs2;
      Settings.Instance.Subs[0].ExcludeLongerThanTimeEnabled = ConstantSettings.DefaultExcludeLinesLongerThanMsSubs1;
      Settings.Instance.Subs[1].ExcludeLongerThanTimeEnabled = ConstantSettings.DefaultExcludeLinesLongerThanMsSubs2;
      Settings.Instance.Subs[0].ExcludeLongerThanTime = ConstantSettings.DefaultExcludeLinesLongerThanMsNumSubs1;
      Settings.Instance.Subs[1].ExcludeLongerThanTime = ConstantSettings.DefaultExcludeLinesLongerThanMsNumSubs2;
      Settings.Instance.Subs[0].JoinSentencesEnabled = ConstantSettings.DefaultJoinSentencesSubs1;
      Settings.Instance.Subs[1].JoinSentencesEnabled = ConstantSettings.DefaultJoinSentencesSubs2;
      Settings.Instance.Subs[0].JoinSentencesCharList = ConstantSettings.DefaultJoinSentencesCharListSubs1;
      Settings.Instance.Subs[1].JoinSentencesCharList = ConstantSettings.DefaultJoinSentencesCharListSubs2;

      this.ClientSize = new Size(ConstantSettings.MainWindowWidth, ConstantSettings.MainWindowHeight);
    }


    /// <summary>
    /// Event handler for when the preview needs to be regenerated.
    /// </summary>
    private void updatePreview(object sender, EventArgs e)
    {
      errorProviderMain.Clear();

      if (validateForm())
      {
        updateSettings();
      }
    }


    /// <summary>
    /// File -> New. Set GUI to default values.
    /// </summary>
    private void newToolStripMenuItem_Click(object sender, EventArgs e)
    {
      Settings.Instance.reset();

      this.readPreferencesFile();

      this.updateGUI();
      this.errorProviderMain.Clear();

      if (dlgPreview != null)
      {
        dlgPreview.Close();
      }
    }


    private void quitToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this.Close();
    }


    private void ripAudioFromVideoToolStripMenuItem_Click(object sender, EventArgs e)
    {
      try
      {
        DialogExtractAudioFromMedia dialog = new DialogExtractAudioFromMedia();

        dialog.MediaFilePattern = this.textBoxVideoFile.Text.Trim();
        dialog.Subs1FilePattern = this.textBoxSubs1File.Text.Trim();
        dialog.Subs2FilePattern = this.textBoxSubs2File.Text.Trim();
        dialog.OutputDir = this.textBoxOutputDir.Text.Trim();
        dialog.DeckName = this.textBoxName.Text.Trim();
        dialog.EpisodeStartNumber = (int)this.numericUpDownEpisodeStartNumber.Value;
        dialog.UseSubs1Timings = this.radioButtonTimingSubs1.Checked;
        dialog.UseTimeShift = this.groupBoxCheckTimeShift.Checked;
        dialog.TimeShiftSubs1 = (int)this.numericUpDownTimeShiftSubs1.Value;
        dialog.TimeShiftSubs2 = (int)this.numericUpDownTimeShiftSubs2.Value;
        dialog.SpanEnabled = this.groupBoxCheckSpan.Checked;
        dialog.SpanStart = this.maskedTextBoxSpanStart.Text.Trim();
        dialog.SpanEnd = this.maskedTextBoxSpanEnd.Text.Trim();
        dialog.EncodingSubs1 = this.comboBoxEncodingSubs1.Text;
        dialog.EncodingSubs2 = this.comboBoxEncodingSubs2.Text;
        dialog.AudioStreamIndex = this.comboBoxStreamAudioFromVideo.SelectedIndex;
        dialog.FileBrowserStartDir = this.lastDirPath;

        int audioBitrate;

        if (!Int32.TryParse(((string)this.comboBoxAudioClipBitrate.SelectedItem).Trim(), out audioBitrate))
        {
          audioBitrate = 128;
        }

        dialog.Bitrate = audioBitrate;

        dialog.ShowDialog();

        this.lastDirPath = dialog.FileBrowserStartDir;

      }
      catch (Exception e1)
      {
        UtilsMsg.showErrMsg("Something went wrong while using the Extract Audio from Video tool\n" + e1);
      }
    }

    private void duelingSubtitlesToolStripMenuItem_Click(object sender, EventArgs e)
    {
      try
      {
        DialogDuelingSubtitles dialog = new DialogDuelingSubtitles();

        dialog.Subs1FilePattern = this.textBoxSubs1File.Text.Trim();
        dialog.Subs2FilePattern = this.textBoxSubs2File.Text.Trim();
        dialog.OutputDir = this.textBoxOutputDir.Text.Trim();
        dialog.DeckName = this.textBoxName.Text.Trim();
        dialog.EpisodeStartNumber = (int)this.numericUpDownEpisodeStartNumber.Value;
        dialog.UseSubs1Timings = this.radioButtonTimingSubs1.Checked;
        dialog.UseTimeShift = this.groupBoxCheckTimeShift.Checked;
        dialog.TimeShiftSubs1 = (int)this.numericUpDownTimeShiftSubs1.Value;
        dialog.TimeShiftSubs2 = (int)this.numericUpDownTimeShiftSubs2.Value;
        dialog.FileBrowserStartDir = this.lastDirPath;
        dialog.EncodingSubs1 = this.comboBoxEncodingSubs1.Text;
        dialog.EncodingSubs2 = this.comboBoxEncodingSubs2.Text;

        dialog.ShowDialog();

        this.lastDirPath = dialog.FileBrowserStartDir;


      }
      catch (Exception e1)
      {
        UtilsMsg.showErrMsg("Something went wrong while using the Dueling Subtitles tool\n" + e1);
      }

    }

    private void subsReTimerToolStripMenuItem_Click(object sender, EventArgs e)
    {
      System.Diagnostics.Process.Start(ConstantSettings.PathSubsReTimerFull);
    }

    private void mKVExtractToolToolStripMenuItem_Click(object sender, EventArgs e)
    {
      DialogMkvExtract dlg = new DialogMkvExtract();

      dlg.ShowDialog();
    }

    private void usageToolStripMenuItem_Click(object sender, EventArgs e)
    {
      try
      {
        System.Diagnostics.Process.Start(ConstantSettings.HelpPage);
      }
      catch
      {
        UtilsMsg.showErrMsg("Help page not found.");
      }
    }

    private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
    {
      DialogAbout dlgAbout = new DialogAbout();

      dlgAbout.ShowDialog();
    }

    private void buttonSubs1File_Click(object sender, EventArgs e)
    {
      textBoxSubs1File.Text = showFileDialog(textBoxSubs1File.Text.Trim(),
        "All Subtitle Files (*.ass;*.ssa;*.lrc;*.srt;*.trs;*.idx)|*.ass;*.ssa;*.lrc;*.srt;*.trs;*.idx;*.mkv|" +
        "Advanced Substation Alpha (*.ass;*.ssa)|*.ass;*.ssa|" + 
        "Lyric (*.lrc)|*.lrc|" +
        "Matroska (*.mkv)|*.mkv|" +
        "SubRip (*.srt)|*.srt|" +
        "Transcriber (*.trs)|*.trs|" +
        "VobSub (*.idx)|*.idx", 1);

      // Remove the error that pops up the first time
      errorProviderMain.SetError((Control)textBoxSubs1File, null);
    }

    private void buttonSubs2File_Click(object sender, EventArgs e)
    {
      textBoxSubs2File.Text = showFileDialog(textBoxSubs2File.Text.Trim(),
        "All Subtitle Files (*.ass;*.ssa;*.lrc;*.srt;*.trs;*.idx)|*.ass;*.ssa;*.lrc;*.srt;*.trs;*.idx;*.mkv|" +
        "Advanced Substation Alpha (*.ass;*.ssa)|*.ass;*.ssa|" +
        "Lyric (*.lrc)|*.lrc|" +
        "Matroska (*.mkv)|*.mkv|" +
        "SubRip (*.srt)|*.srt|" +
        "Transcriber (*.trs)|*.trs|" +
        "VobSub (*.idx)|*.idx", 1);
    }

    private void linkLabelAdvancedSubtitleOptions_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
      try
      {
        dlgAdvSubs.Subs1FilePattern = this.textBoxSubs1File.Text.Trim();
        dlgAdvSubs.Subs2FilePattern = this.textBoxSubs2File.Text.Trim();
        dlgAdvSubs.Subs1Encoding = this.comboBoxEncodingSubs1.Text;
        dlgAdvSubs.Subs2Encoding = this.comboBoxEncodingSubs2.Text;

        if (dlgAdvSubs.ShowDialog() == DialogResult.OK)
        {

        }
      }
      catch (Exception e1)
      {
        UtilsMsg.showErrMsg("Something went wrong while accessing the advanced subtitle options.\n" + e1);
      }
    }

    private void buttonAudioFile_Click(object sender, EventArgs e)
    {
      textBoxAudioFile.Text = showFileDialog(textBoxAudioFile.Text,
        "All files (*.*)|*.*|" +
        "All Common Audio Files (*.aac;*.flac;*.m4a;*.mp3;*.ogg;*.wav;*.wma)|*.aac;*.flac;*.m4a;*.mp3;*.ogg;*.wav;*.wma|" +
        "Ogg Multimedia (*.ogm)|*.ogm|" +
        "Advanced Audio Coding (*.aac)|*.aac|" +
        "Free Lossless Audio Codec (*.flac)|*.flac|" +
        "MPEG4 Audio (*.m4a)|*.m4a|" +
        "MPEG3 (*.mp3)|*.mp3|" +
        "Ogg Vorbis (*.ogg)|*.ogg|" +
        "WAV (*.wav)|*.wav|" +
        "Windows Media Audio (*.wma)|*.wma", 2);
    }

    private void buttonVideoFile_Click(object sender, EventArgs e)
    {
      textBoxVideoFile.Text = showFileDialog(textBoxVideoFile.Text.Trim(),
        "All files (*.*)|*.*|" +
        "All Common Video Files (*.avi;*.flv;*.mkv;*.mp4;*.ogm;*.vob)|*.avi;*.flv;*.mkv;*.mp4;*.ogm;*.vob|" +
        "Audio Video Interleave (*.avi;)|*.avi|" +
        "Flash Video (*.flv)|*.flv|" +
        "Matroska Multimedia (*.mkv)|*.mkv|" +
        "MPEG4 Video (*.mp4)|*.mp4|" +
        "OGG Media (*.ogm)|*.ogm|" +
        "DVD Video Object (*.vob)|*.vob", 2);
    }

    private void buttonOutputDir_Click(object sender, EventArgs e)
    {
      textBoxOutputDir.Text = showFolderDialog(textBoxOutputDir.Text);
    }


    /// <summary>
    /// Update the information related to Subs1 or Subs2.
    /// </summary>
    private void updateSubs(int subsNum)
    {
      Label theLabel;
      ComboBox comboVobsub;
      ComboBox comboEncoding;
      TextBox textbox;
      string file;

      // Get items that depend on whether it's subs1 or subs2
      if (subsNum == 1)
      {
        comboVobsub = this.comboBoxStreamSubs1;
        theLabel = this.labelVobsubEncoding1;
        comboEncoding = this.comboBoxEncodingSubs1;
        textbox = this.textBoxSubs1File;
        file = this.textBoxSubs1File.Text.Trim();
      }
      else
      {
        comboVobsub = this.comboBoxStreamSubs2;
        theLabel = this.labelVobsubEncoding2;
        comboEncoding = this.comboBoxEncodingSubs2;
        textbox = this.textBoxSubs2File;
        file = this.textBoxSubs2File.Text.Trim();
      }

      if (!File.Exists(file))
      {
        return;
      }

      // If input file is an MKV, show the MKV track select dialog
      if (Path.GetExtension(file) == ".mkv")
      {
        List<MkvTrack> subTrackList = UtilsMkv.getSubtitleTrackList(file);

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


      // Clear actors
      if (Settings.Instance.Subs[subsNum - 1].ActorsEnabled)
      {
        dlgAdvSubs.clearActorListbox();
      }

      theLabel.Text = String.Format("Subs{0} Encoding:", subsNum);

      if (UtilsSubs.filePatternContainsVobsubs(file))
      {
        theLabel.Enabled = true;
        comboVobsub.Visible = true;
        comboVobsub.Enabled = true;
        comboEncoding.Visible = false;
        comboEncoding.Enabled = false;

        theLabel.Text = String.Format("Subs{0} Stream:", subsNum);

        comboVobsub.Items.Clear();

        SubtitleCreator.SUP sup = SubtitleCreator.SUP.Instance;

        try
        {
          List<InfoStream> subStreams = new List<InfoStream>();
          string[] subFiles = UtilsCommon.getNonHiddenFiles(file);
          string[] langs = null;
          string firstIdxFile = "";

          foreach (string subFile in subFiles)
          {
            if (subFile.Trim().EndsWith(".idx"))
            {
              firstIdxFile = subFile;
              break;
            }
          }

          // Get stream info from the first vobsub file
          if (firstIdxFile.Length > 0)
          {
            langs = sup.RetrieveLanguageTableFromIdxOrSub(firstIdxFile);

            if (langs.Length > 0)
            {
              for (int i = 0; i < langs.Length; i++)
              {
                SubtitleCreator.SUP.StreamsData streamData = (SubtitleCreator.SUP.StreamsData)sup.LangStreamsData[i];
                subStreams.Add(new InfoStream(streamData.VobSubId.ToString(), i.ToString(), langs[i], "sub/idx"));
              }
            }
            else
            {
              langs = null;
            }
          }

          foreach (InfoStream stream in subStreams)
          {
            comboVobsub.Items.Add(stream);
          }

          comboVobsub.SelectedIndex = 0;
        }
        catch (Exception e1)
        {
          UtilsMsg.showErrMsg(String.Format("Something went wrong when selecting subtitle stream {0}.\n" + e1, subsNum));
        }
      }
      else if (UtilsSubs.getNumSubsFiles(file) > 0)
      {
        theLabel.Enabled = true;
        comboVobsub.Visible = false;
        comboVobsub.Enabled = false;
        comboEncoding.Visible = true;
        comboEncoding.Enabled = true;
      }
      else
      {
        theLabel.Enabled = false;
        comboVobsub.Visible = false;
        comboVobsub.Enabled = false;
        comboEncoding.Visible = true;
        comboEncoding.Enabled = false;
      }
    }


    private void textBoxSubs1File_TextChanged(object sender, EventArgs e)
    {
      this.updateSubs(1);
    }


    private void textBoxSubs2File_TextChanged(object sender, EventArgs e)
    {
      this.updateSubs(2);
    }


    private void textBoxVideoFile_TextChanged(object sender, EventArgs e)
    {
      string filePattern = this.textBoxVideoFile.Text.Trim();
      string[] files = UtilsCommon.getNonHiddenFiles(filePattern);

      this.comboBoxStreamAudioFromVideo.Enabled = true;
      this.labelVideoStream.Enabled = true;

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
        this.labelVideoStream.Enabled = false;
      }
    }


    private void radioButtonExtractAudioFromVideo_CheckedChanged(object sender, EventArgs e)
    {
      comboBoxAudioClipBitrate.Enabled = radioButtonExtractAudioFromVideo.Checked;
    }


    private void radioButtonUseExistingAudio_CheckedChanged(object sender, EventArgs e)
    {
      textBoxAudioFile.Enabled = radioButtonUseExistingAudio.Checked;
      buttonAudioFile.Enabled = radioButtonUseExistingAudio.Checked;
    }



    private void buttonPreview_Click(object sender, EventArgs e)
    {
      // Make sure that we only have one preview dialog at any one time
      if (dlgPreview != null && !dlgPreview.IsDisposed)
      {
        return;
      }

      errorProviderMain.Clear();

      if (validateForm())
      {
        if (updateSettings())
        {
          dlgPreview = new DialogPreview();
          dlgPreview.GeneretePreview += new EventHandler(updatePreview);
          dlgPreview.FormClosed += new FormClosedEventHandler(dlgPreview_FormClosed);
          // Disable the main form's go! button while the preview dialog is up
          this.buttonGo.Enabled = false;
          dlgPreview.Show();
        }
      }

    }


    void dlgPreview_FormClosed(object sender, FormClosedEventArgs e)
    {
      this.buttonGo.Enabled = true;
    }


    private void buttonSnapshotDimensionsChoose_Click(object sender, EventArgs e)
    {
      ImageSize dimensions = new ImageSize();
      string filePattern = this.textBoxVideoFile.Text.Trim();
      string[] files = UtilsCommon.getNonHiddenFiles(filePattern);

      if (files.Length != 0)
      {
        // Based on the first video file in the file pattern, get the resolution
        string firstFile = files[0];

        dimensions = UtilsVideo.getVideoResolution(firstFile);
      }
      else
      {
        dimensions.Width = 720;
        dimensions.Height = 480;
      }

      dlgSnapshotDimensionsChooser.VideoDimensions = dimensions;

      if (dlgSnapshotDimensionsChooser.ShowDialog() == DialogResult.OK)
      {
        this.numericUpDownSnapshotWidth.Value = (decimal)dlgSnapshotDimensionsChooser.NewDimensions.Width;
        this.numericUpDownSnapshotHeight.Value = (decimal)dlgSnapshotDimensionsChooser.NewDimensions.Height;
      }
    }


    private void buttonVideoClipDimensionsChoose_Click(object sender, EventArgs e)
    {
      ImageSize dimensions = new ImageSize();
      string filePattern = this.textBoxVideoFile.Text.Trim();
      string[] files = UtilsCommon.getNonHiddenFiles(filePattern);

      if (files.Length != 0)
      {
        // Based on the first video file in the file pattern, get the resolution
        string firstFile = files[0];

        dimensions = UtilsVideo.getVideoResolution(firstFile);
      }
      else
      {
        dimensions.Width = 720;
        dimensions.Height = 480;
      }

      dlgVideoClipDimensionsChooser.VideoDimensions = dimensions;

      if (dlgVideoClipDimensionsChooser.ShowDialog() == DialogResult.OK)
      {
        this.numericUpDownVideoWidth.Value = (decimal)dlgVideoClipDimensionsChooser.NewDimensions.Width;
        this.numericUpDownVideoHeight.Value = (decimal)dlgVideoClipDimensionsChooser.NewDimensions.Height;
      }
    }


    // Load a .s2s settings file
    private void loadSettingsFile(string settingsFile)
    {
      try
      {
        Stream fileStream = new FileStream(settingsFile, FileMode.Open, FileAccess.Read);
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        SaveSettings saveSettings = (SaveSettings)(binaryFormatter.Deserialize(fileStream));
        fileStream.Close();
        Settings.Instance.loadSettings(saveSettings);
        updateGUI();
        errorProviderMain.Clear();

        if (dlgPreview != null)
        {
          dlgPreview.Close();
        }
      }
      catch
      {
        UtilsMsg.showErrMsg(
          String.Format("Could not load the .{0} file.\nPerhaps it is from an old version of subs2srs.", ConstantSettings.SaveExt));
      }
    }


    // Open
    private void openToolStripMenuItem_Click(object sender, EventArgs e)
    {
      OpenFileDialog openDialog = new OpenFileDialog();

      openDialog.Filter = string.Format("subs2srs files (*.{0})|*.{0}", ConstantSettings.SaveExt);
      openDialog.FilterIndex = 1;
      openDialog.RestoreDirectory = true;
      openDialog.Multiselect = false;

      if (openDialog.ShowDialog() == DialogResult.OK)
      {
        if (File.Exists(openDialog.FileName))
        {
          this.loadSettingsFile(openDialog.FileName);
        }
      }
    }


    // Save
    private void saveToolStripMenuItem_Click(object sender, EventArgs e)
    {
      SaveFileDialog saveDialog = new SaveFileDialog();

      saveDialog.Filter = string.Format("subs2srs files (*.{0})|*.{0}", ConstantSettings.SaveExt);
      saveDialog.FilterIndex = 1;
      saveDialog.RestoreDirectory = true;

      try
      {
        if (saveDialog.ShowDialog() == DialogResult.OK)
        {
          FileStream fileStream = new FileStream(saveDialog.FileName, FileMode.Create);
          BinaryFormatter binaryFormatter = new BinaryFormatter();
          updateSettings();
          SaveSettings saveSettings = new SaveSettings();
          saveSettings.gatherData();
          binaryFormatter.Serialize(fileStream, saveSettings);
          fileStream.Close();
        }
      }
      catch(Exception e1)
      {
        UtilsMsg.showErrMsg("Could not save file.\n" + e1);
      }
    }


    // Open folder dialog (starting at the last folder) and get the selected folder
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


    // Open folder dialog (starting at the current file) and get the selected file
    private string showFileDialog(string currentFilePattern, string filter, int filterIndex)
    {
      string selectedFile = currentFilePattern;
      string curDir = "";

      this.openFileDialog.FileName = "";

      this.openFileDialog.Filter = filter;

      openFileDialog.FilterIndex = filterIndex;

      try
      {
        curDir = Path.GetDirectoryName(currentFilePattern);
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


    // Start subtitle processing!
    private void buttonGo_Click(object sender, EventArgs e)
    {
      errorProviderMain.Clear();

      if (validateForm())
      {
        if (updateSettings())
        {
          SubsProcessor subsProcessor = new SubsProcessor();
          subsProcessor.start();
        }
      }
    }


 
    // Validate GUI, collect file lists
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


    private bool updateSettings()
    {
      try
      {
        Settings.Instance.Subs[0].FilePattern = textBoxSubs1File.Text.Trim();
        Settings.Instance.Subs[0].VobsubStream = (InfoStream)comboBoxStreamSubs1.SelectedItem;
        Settings.Instance.Subs[0].Encoding = InfoEncoding.longToShort(comboBoxEncodingSubs1.Text);
        Settings.Instance.Subs[0].TimingsEnabled = radioButtonTimingSubs1.Checked;
        Settings.Instance.Subs[0].TimeShift = (int)numericUpDownTimeShiftSubs1.Value;

        Settings.Instance.Subs[0].Files = UtilsSubs.getSubsFiles(Settings.Instance.Subs[0].FilePattern).ToArray();

        Settings.Instance.Subs[1].FilePattern = textBoxSubs2File.Text.Trim();
        Settings.Instance.Subs[1].VobsubStream = (InfoStream)comboBoxStreamSubs2.SelectedItem;
        Settings.Instance.Subs[1].Encoding = InfoEncoding.longToShort(comboBoxEncodingSubs2.Text);
        Settings.Instance.Subs[1].TimingsEnabled = radioButtonTimingSubs2.Checked;
        Settings.Instance.Subs[1].TimeShift = (int)numericUpDownTimeShiftSubs2.Value;

        if (Settings.Instance.Subs[1].FilePattern.Trim().Length > 0)
        {
          Settings.Instance.Subs[1].Files = UtilsSubs.getSubsFiles(Settings.Instance.Subs[1].FilePattern).ToArray();
        }
        else
        {
          Settings.Instance.Subs[1].Files = new string[0];
        }

        Settings.Instance.VideoClips.Enabled = groupBoxCheckGenerateVideoClips.Checked;
        Settings.Instance.VideoClips.Size.Width = (int)numericUpDownVideoWidth.Value;
        Settings.Instance.VideoClips.Size.Height = (int)numericUpDownVideoHeight.Value;
        Settings.Instance.VideoClips.Crop.Bottom = (int)numericUpDownVideoCropBottom.Value;
        Settings.Instance.VideoClips.BitrateVideo = (int)numericUpDownVideoClipBitrateVideo.Value;
        Settings.Instance.VideoClips.BitrateAudio = Convert.ToInt32(comboBoxVideoClipBitrateAudio.Text.Trim());
        Settings.Instance.VideoClips.PadEnabled = groupBoxCheckVideoPad.Checked;
        Settings.Instance.VideoClips.PadStart = (int)numericUpDownVideoPadStart.Value;
        Settings.Instance.VideoClips.PadEnd = (int)numericUpDownVideoPadEnd.Value;

        Settings.Instance.VideoClips.FilePattern = textBoxVideoFile.Text.Trim();
        Settings.Instance.VideoClips.AudioStream = (InfoStream)comboBoxStreamAudioFromVideo.SelectedItem;
        Settings.Instance.VideoClips.Files = UtilsCommon.getNonHiddenFiles(Settings.Instance.VideoClips.FilePattern);

        Settings.Instance.AudioClips.Enabled = groupBoxCheckGenerateAudioClips.Checked;
        Settings.Instance.AudioClips.Bitrate = Convert.ToInt32(comboBoxAudioClipBitrate.Text.Trim());
        Settings.Instance.AudioClips.UseAudioFromVideo = radioButtonExtractAudioFromVideo.Checked;
        Settings.Instance.AudioClips.UseExistingAudio = radioButtonUseExistingAudio.Checked;
        Settings.Instance.AudioClips.PadEnabled = groupBoxCheckPadTimings.Checked;
        Settings.Instance.AudioClips.PadStart = (int)numericUpDownAudioPadStart.Value;
        Settings.Instance.AudioClips.PadEnd = (int)numericUpDownAudioPadEnd.Value;
        Settings.Instance.AudioClips.Normalize = checkBoxNormalizeAudio.Checked;
        Settings.Instance.VideoClips.IPodSupport = checkBoxIPod.Checked;

        Settings.Instance.AudioClips.filePattern = textBoxAudioFile.Text.Trim();
        Settings.Instance.AudioClips.Files = UtilsCommon.getNonHiddenFiles(Settings.Instance.AudioClips.filePattern);

        Settings.Instance.Snapshots.Enabled = groupBoxCheckGenerateSnapshots.Checked;
        Settings.Instance.Snapshots.Size.Width = (int)numericUpDownSnapshotWidth.Value;
        Settings.Instance.Snapshots.Size.Height = (int)numericUpDownSnapshotHeight.Value;
        Settings.Instance.Snapshots.Crop.Bottom = (int)numericUpDownSnapshotCropBottom.Value;

        Settings.Instance.OutputDir = textBoxOutputDir.Text.Trim();

        Settings.Instance.TimeShiftEnabled = groupBoxCheckTimeShift.Checked;

        Settings.Instance.SpanEnabled = groupBoxCheckSpan.Checked;
        if (Settings.Instance.SpanEnabled)
        {
          Settings.Instance.SpanStart = UtilsSubs.stringToTime(maskedTextBoxSpanStart.Text.Trim());
          Settings.Instance.SpanEnd = UtilsSubs.stringToTime(maskedTextBoxSpanEnd.Text.Trim());
        }

        Settings.Instance.DeckName = textBoxName.Text.Trim();
        Settings.Instance.EpisodeStartNumber = (int)numericUpDownEpisodeStartNumber.Value;
      }
      catch (Exception e1)
      {
        UtilsMsg.showErrMsg("Something went wrong while gathering interface data:\n" + e1);
        return false;
      }

      return true;
    }


    private void updateGUI()
    {
      textBoxSubs1File.Text = Settings.Instance.Subs[0].FilePattern;

      this.updateSubs(1);
      this.updateSubs(2);

      if (Settings.Instance.Subs[0].VobsubStream != null)
      {
        for (int i = 0; i < comboBoxStreamSubs1.Items.Count; i++)
        {
          if (((InfoStream)comboBoxStreamSubs1.Items[i]).Num == Settings.Instance.Subs[0].VobsubStream.Num)
          {
            comboBoxStreamSubs1.SelectedIndex = i;
          }
        }
      }

      radioButtonTimingSubs1.Checked = Settings.Instance.Subs[0].TimingsEnabled;
      numericUpDownTimeShiftSubs1.Value = (decimal)Settings.Instance.Subs[0].TimeShift;

      textBoxSubs2File.Text = Settings.Instance.Subs[1].FilePattern;

      if (Settings.Instance.Subs[1].VobsubStream != null)
      {
        for (int i = 0; i < comboBoxStreamSubs2.Items.Count; i++)
        {
          if (((InfoStream)comboBoxStreamSubs2.Items[i]).Num == Settings.Instance.Subs[1].VobsubStream.Num)
          {
            comboBoxStreamSubs2.SelectedIndex = i;
          }
        }
      }

      comboBoxEncodingSubs1.Text = InfoEncoding.shortToLong(Settings.Instance.Subs[0].Encoding);
      comboBoxEncodingSubs2.Text = InfoEncoding.shortToLong(Settings.Instance.Subs[1].Encoding);


      radioButtonTimingSubs2.Checked = Settings.Instance.Subs[1].TimingsEnabled;

      groupBoxCheckGenerateVideoClips.Checked = Settings.Instance.VideoClips.Enabled;
      textBoxVideoFile.Text = Settings.Instance.VideoClips.FilePattern;

      for(int i = 0; i < comboBoxStreamAudioFromVideo.Items.Count; i++)
      {
        if (((InfoStream)comboBoxStreamAudioFromVideo.Items[i]).Num == Settings.Instance.VideoClips.AudioStream.Num)
        {
          comboBoxStreamAudioFromVideo.SelectedIndex = i;
        }
      }

      Settings.Instance.VideoClips.AudioStream = (InfoStream)comboBoxStreamAudioFromVideo.SelectedItem;

      numericUpDownVideoWidth.Value = (decimal)Settings.Instance.VideoClips.Size.Width;
      numericUpDownVideoHeight.Value = (decimal)Settings.Instance.VideoClips.Size.Height;
      numericUpDownVideoCropBottom.Value = (decimal)Settings.Instance.VideoClips.Crop.Bottom;
      numericUpDownVideoClipBitrateVideo.Value = (decimal)Settings.Instance.VideoClips.BitrateVideo;
      comboBoxVideoClipBitrateAudio.Text = Settings.Instance.VideoClips.BitrateAudio.ToString();
      groupBoxCheckVideoPad.Checked = Settings.Instance.VideoClips.PadEnabled;
      numericUpDownVideoPadStart.Value = (decimal)Settings.Instance.VideoClips.PadStart;
      numericUpDownVideoPadEnd.Value = (decimal)Settings.Instance.VideoClips.PadEnd;
      checkBoxIPod.Checked = Settings.Instance.VideoClips.IPodSupport;

      groupBoxCheckGenerateAudioClips.Checked = Settings.Instance.AudioClips.Enabled;
      textBoxAudioFile.Text = Settings.Instance.AudioClips.filePattern;
      groupBoxCheckPadTimings.Checked = Settings.Instance.AudioClips.PadEnabled;
      numericUpDownAudioPadStart.Value = (decimal)Settings.Instance.AudioClips.PadStart;
      numericUpDownAudioPadEnd.Value = (decimal)Settings.Instance.AudioClips.PadEnd;
      comboBoxAudioClipBitrate.Text = Settings.Instance.AudioClips.Bitrate.ToString();
      radioButtonExtractAudioFromVideo.Checked = Settings.Instance.AudioClips.UseAudioFromVideo;
      checkBoxNormalizeAudio.Checked = Settings.Instance.AudioClips.Normalize;

      radioButtonUseExistingAudio.Checked = Settings.Instance.AudioClips.UseExistingAudio;
      groupBoxCheckGenerateSnapshots.Checked = Settings.Instance.Snapshots.Enabled;
      numericUpDownSnapshotWidth.Value = (decimal)Settings.Instance.Snapshots.Size.Width;
      numericUpDownSnapshotHeight.Value = (decimal)Settings.Instance.Snapshots.Size.Height;
      numericUpDownSnapshotCropBottom.Value = (decimal)Settings.Instance.Snapshots.Crop.Bottom;

      textBoxOutputDir.Text = Settings.Instance.OutputDir;

      groupBoxCheckTimeShift.Checked = Settings.Instance.TimeShiftEnabled;

      groupBoxCheckSpan.Checked = Settings.Instance.SpanEnabled;
      maskedTextBoxSpanStart.Text = UtilsSubs.timeToString(Settings.Instance.SpanStart).Replace(":", "").Remove(0, 1);
      maskedTextBoxSpanEnd.Text = UtilsSubs.timeToString(Settings.Instance.SpanEnd).Replace(":", "").Remove(0, 1);

      textBoxName.Text = Settings.Instance.DeckName;
      numericUpDownEpisodeStartNumber.Value = (decimal)Settings.Instance.EpisodeStartNumber;
    }


    private void checkCommandLineArgs()
    {
      string[] commandLineArgs = Environment.GetCommandLineArgs();

      if (commandLineArgs.Length > 1)
      {
        string savedSettingsFile = commandLineArgs[1];

        // If a valid .s2s settings file was provided, load it
        if (File.Exists(savedSettingsFile))
        {
          Logger.Instance.info("checkCommandLineArgs: " + savedSettingsFile);
          loadSettingsFile(savedSettingsFile);
        }
      }
    }


    private void FormMain_Load(object sender, EventArgs e)
    {
      Settings.Instance.reset();
      readPreferencesFile();

      Logger.Instance.info("FormMain_Load");

      updateGUI();

      checkCommandLineArgs();

      // Intial focus/Where the cursor should appear on startup
      this.ActiveControl = textBoxSubs1File;      
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

      errorProviderMain.SetError((Control)sender, error);
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

      errorProviderMain.SetError((Control)sender, error);
    }


    private void textBoxSubsOutputDir_Validating(object sender, CancelEventArgs e)
    {
      string error = null;
      string dir = ((TextBox)sender).Text.Trim();

      if (!Directory.Exists(dir))
      {
        error = "Directory does not exist";
        invalidCount++;
      }

      errorProviderMain.SetError((Control)sender, error);
    }


    private void textBoxAudioFile_Validating(object sender, CancelEventArgs e)
    {
      string error = null;
      string filePattern = ((TextBox)sender).Text.Trim();
      string[] files = UtilsCommon.getNonHiddenFiles(filePattern);

      if ((groupBoxCheckGenerateAudioClips.Checked) && (radioButtonUseExistingAudio.Checked))
      {
        if ((error == null) && (files.Length != UtilsSubs.getNumSubsFiles(textBoxSubs1File.Text.Trim())))
        {
          error = "The number of audio files must match\nthe number of subtitles files in Subs1";
          invalidCount++;
        }
      }

      errorProviderMain.SetError((Control)sender, error);
    }


    private void textBoxSubs1File_Validating(object sender, CancelEventArgs e)
    {
      string error = null;
      string filePattern = ((TextBox)sender).Text.Trim();
      string[] files = UtilsCommon.getNonHiddenFiles(filePattern);

      if ((error == null) && (UtilsSubs.getNumSubsFiles(filePattern) == 0))
      {
        error = "Please provide a valid subtitle file. \nOnly .srt, .ass, .ssa, .lrc, .trs and .idx are allowed.";
        invalidCount++;
      }

      if ((error == null) && (UtilsSubs.filePatternContainsVobsubs(filePattern)))
      {
        if (!UtilsSubs.isVobsubFilePatternCorrect(filePattern))
        {
          error = "The number of .idx and .sub files must match";
          invalidCount++;
        }
      }

      errorProviderMain.SetError((Control)sender, error);
    }


    private void textBoxSubs2File_Validating(object sender, CancelEventArgs e)
    {
      string error = null;
      string filePattern = ((TextBox)sender).Text.Trim();
      string[] files = UtilsCommon.getNonHiddenFiles(filePattern);

      if ((radioButtonTimingSubs2.Checked) && (filePattern == ""))
      {
        error = "Since you want to use subs2 timings,\nplease enter a valid Subs2 subtitle file here";
        invalidCount++;
      }

      if((error == null) && (filePattern != ""))
      {
        if ((error == null) && (UtilsSubs.getNumSubsFiles(filePattern) == 0))
        {
          error = "Please provide a valid subtitle file. \nOnly .srt, .ass, .ssa, .lrc, .trs and .idx are allowed.";
          invalidCount++;
        }

        if ((error == null) && (UtilsSubs.filePatternContainsVobsubs(filePattern)))
        {
          if (!UtilsSubs.isVobsubFilePatternCorrect(filePattern))
          {
            error = "The number of .idx and .sub files must match";
            invalidCount++;
          }
        }

        if ((error == null) && (UtilsSubs.getNumSubsFiles(filePattern) != UtilsSubs.getNumSubsFiles(textBoxSubs1File.Text.Trim())))
        {
          error = "The number of subtitle files here must match\nthe number of subtitle files in Subs1";
          invalidCount++;
        }
      }   

      errorProviderMain.SetError((Control)sender, error);
    }


    private void textBoxVideoFile_Validating(object sender, CancelEventArgs e)
    {
      string error = null;
      string filePattern = ((TextBox)sender).Text.Trim();
      string[] files = UtilsCommon.getNonHiddenFiles(filePattern);

      if ((radioButtonExtractAudioFromVideo.Checked) && (groupBoxCheckGenerateAudioClips.Checked) && (filePattern == ""))
      {
        error = "Since you want to extract audio from the video,\nplease enter a valid video file here";
        invalidCount++;
      }

      if ((error == null) && (groupBoxCheckGenerateSnapshots.Checked) && (filePattern == ""))
      {
        error = "Since you want to generate snapshots,\nplease enter a valid video file here";
        invalidCount++;
      }

      if ((error == null) && (groupBoxCheckGenerateVideoClips.Checked) && (filePattern == ""))
      {
        error = "Since you want to generate video clips,\nplease enter a valid video file here";
        invalidCount++;
      }

      if ((error == null) && (filePattern != ""))
      {
        if ((error == null) && (files.Length != UtilsSubs.getNumSubsFiles(textBoxSubs1File.Text.Trim())))
        {
          error = "The number of video files must match\nthe number of subtitle files in Subs1";
          invalidCount++;
        }
      }

      errorProviderMain.SetError((Control)sender, error);
    }


    // Force to closest increment (rounding down)
    private void numericUpDownForceIncrement(object sender, EventArgs e)
    {
      NumericUpDown ctrl = (NumericUpDown)sender;
      int width = Convert.ToInt32(ctrl.Text.Trim());
      int increment = (int)ctrl.Increment;

      if (width % increment != 0)
      {
        width = UtilsCommon.getNearestMultiple(width, increment);
      }

      ctrl.Text = width.ToString();
    }

    private void preferencesToolStripMenuItem_Click(object sender, EventArgs e)
    {
      DialogPref dlgPref = new DialogPref();
      dlgPref.ShowDialog();
    }

    private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
    {
      Logger.Instance.info("FormMain_FormClosing");
      // Write all log info to the log file
      Logger.Instance.flush();
    }

 










  }
}
