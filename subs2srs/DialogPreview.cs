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
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace subs2srs
{
  /// <summary>
  /// The Preview dialog.
  /// </summary>
  public partial class DialogPreview : Form
  {
    private Color activeColor = Color.MintCream;
    private Color inactiveColor = Color.Pink;
    private Color longLineWarningColor = Color.Red;

    private DialogProgress dialogProgress = null;
    private WorkerVars previewWorkerVars;

    // Does a new audio preview need to be generated? (Set to true when index has changed)
    private bool audioPreviewNeedsUpdating = false;

    // Should the list view's item selected event do anything?
    private bool enableListViewItemSelected = true;

    // The currently selected (or last selected or nothing is currently selected) combined info
    private InfoCombined currentInfoComb = null;

    // Has a line's text or Active/Inactive status been updated?
    private bool changesHaveBeenMade = false;

    // The list index of the last successfull find
    private int lastFindIndex = 0;

    // For video player preview
    private Process videoPlayerProcess = null;

    // For audio preview
    private int selectedEpisodeIndex = 0;

    /// <summary>
    /// Event occurs whenever the preview needs to be regerated.
    /// </summary>
    public event EventHandler GeneretePreview;


    public DialogPreview()
    {
      InitializeComponent();
    }


    private void DialogPreview_Load(object sender, EventArgs e)
    {
      populateEpisodeComboBox();
      
      generatePreview();

      if (this.listViewLines.Items.Count > 0)
      {
        this.listViewLines.Items[0].Selected = true;
        this.listViewLines.Select();
      }

      checkVideoPlayerConfigFile();
    }


    /// <summary>
    /// Check if the video player config file setting exists, if it does, unhide the video preview controls.
    /// </summary>
    private bool checkVideoPlayerConfigFile()
    {
      bool videoPlayerConfigFileExists = false;

      if (File.Exists(ConstantSettings.VideoPlayer))
      {
        videoPlayerConfigFileExists = true;
      }

      this.buttonPreviewVideo.Visible = videoPlayerConfigFileExists;

      return videoPlayerConfigFileExists;
    }


    /// <summary>
    /// Update the episode combo box with all episodes. Uses the First Episode field.
    /// </summary>
    private void populateEpisodeComboBox()
    {
      int numEpisodes = UtilsSubs.getNumSubsFiles(Settings.Instance.Subs[0].FilePattern);
      int firstEpisode = Settings.Instance.EpisodeStartNumber;

      int lastSelectedEpisodeIndex = 0;

      if (comboBoxEpisode.SelectedIndex != -1)
      {
        lastSelectedEpisodeIndex = comboBoxEpisode.SelectedIndex;
      }

      comboBoxEpisode.Items.Clear();

      for (int i = 0; i < numEpisodes; i++)
      {
        int episodeNum = i + firstEpisode;
        comboBoxEpisode.Items.Add(episodeNum);
      }

      comboBoxEpisode.SelectedIndex = lastSelectedEpisodeIndex;
    }


    /// <summary>
    /// Reprocess the subtitles and discard active/inactive and text changes made by the user.
    /// </summary>
    private void buttonRegenerate_Click(object sender, EventArgs e)
    {
      string oldSubs1 = "-----";
      string oldSubs2 = "-----";

      if (changesHaveBeenMade)
      {
        bool confirmed = UtilsMsg.showConfirm("Regenerate the preview and discard the changes that you have made?");

        if (!confirmed)
        {
          return;
        }
      }

      changesHaveBeenMade = false;

      // Save the currently selected item
      if (currentInfoComb != null)
      {
        InfoCombined selectedComb = currentInfoComb;

        oldSubs1 = selectedComb.Subs1.Text;
        oldSubs2 = selectedComb.Subs2.Text;
      }

      generatePreview();

      int indexOfItemToSelect = 0;

      // Try to select the saved item
      for (int i = 0; i < this.listViewLines.Items.Count; i++)
      {
        InfoCombined curComb = (InfoCombined)this.listViewLines.Items[i].Tag;

        if ((curComb.Subs1.Text == oldSubs1) && (curComb.Subs2.Text == oldSubs2))
        {
          indexOfItemToSelect = i;
          break;
        }
      }

      if (this.listViewLines.Items.Count > 0)
      {
        this.listViewLines.Focus();
        this.listViewLines.Items[indexOfItemToSelect].Selected = true;
        this.listViewLines.EnsureVisible(indexOfItemToSelect);
      }
    }


    private void buttonCancel_Click(object sender, EventArgs e)
    {
      this.Close();
    }


    private void DialogPreview_FormClosed(object sender, FormClosedEventArgs e)
    {
      if (previewWorkerVars != null)
      {
        if (Directory.Exists(previewWorkerVars.MediaDir))
        {
          try
          {
            Directory.Delete(previewWorkerVars.MediaDir, true);
          }
          catch
          {
            UtilsMsg.showErrMsg("Unable to delete the temporary directory at:\n" + previewWorkerVars.MediaDir);
          }
        }
      }


      // Kill the video player in case this dialog is killed by the main form
      if (videoPlayerProcess != null)
      {
        if (!videoPlayerProcess.HasExited)
        {
          videoPlayerProcess.Kill();
        }
      }
    }

    /// <summary>
    /// Update the Subs1 and Subs2 text boxes with the provided combined info.
    /// </summary>
    private void updateTextPreview(InfoCombined selectedComb)
    {
      if (selectedComb == null)
      {
        return;
      }

      if (selectedComb.Active)
      {
        this.textBoxSubs1.BackColor = activeColor;
        this.textBoxSubs2.BackColor = activeColor;
        this.pictureBoxSubs1.BackColor = activeColor;
        this.pictureBoxSubs2.BackColor = activeColor;
        this.panelSubs1.BackColor = activeColor;
        this.panelSubs2.BackColor = activeColor;
      }
      else
      {
        this.textBoxSubs1.BackColor = inactiveColor;
        this.textBoxSubs2.BackColor = inactiveColor;
        this.pictureBoxSubs1.BackColor = inactiveColor;
        this.pictureBoxSubs2.BackColor = inactiveColor;
        this.panelSubs1.BackColor = inactiveColor;
        this.panelSubs2.BackColor = inactiveColor;
      }

      // Disable subs2 textbox if there aren't any subs2 files
      this.textBoxSubs2.Enabled = (Settings.Instance.Subs[1].Files.Length != 0);
      this.pictureBoxSubs2.Enabled = (Settings.Instance.Subs[1].Files.Length != 0);
      this.panelSubs2.Enabled = (Settings.Instance.Subs[1].Files.Length != 0);

      if (selectedComb.Subs1.Text.EndsWith("png" + ConstantSettings.SrsVobsubFilenameSuffix))
      {
        this.textBoxSubs1.Visible = false;
        this.pictureBoxSubs1.Visible = true;

        try
        {
          // Multiple vobsub image can be shown in a single line, so extract each image
          // and concatenate them before displaying.
          List<string> vobsubImages = UtilsSubs.extractVobsubFilesFromText(selectedComb.Subs1.Text);       
          List<Image> vobSubImages = new List<Image>();

          foreach (string vobsubImage in vobsubImages)
          {
            Image image = UtilsSnapshot.getImageFromFile(previewWorkerVars.MediaDir + Path.DirectorySeparatorChar + vobsubImage);
            vobSubImages.Add(image);
          }

          if (vobSubImages.Count != 0)
          {
            Image image;

            if (vobSubImages.Count == 0)
            {
              image = vobSubImages[0];
            }
            else
            {
              image = UtilsSnapshot.concatImages(vobSubImages);
            }

            this.pictureBoxSubs1.Image = image;
          }
        }
        catch
        {
          // Don't care
        }
      }
      else
      {
        this.textBoxSubs1.Visible = true;
        this.pictureBoxSubs1.Visible = false;
        this.textBoxSubs1.Text = selectedComb.Subs1.Text;
      }

      if (Settings.Instance.Subs[1].Files.Length != 0)
      {
        if (selectedComb.Subs2.Text.EndsWith("png" + ConstantSettings.SrsVobsubFilenameSuffix))
        {
          this.textBoxSubs2.Visible = false;
          this.pictureBoxSubs2.Visible = true;

          try
          {
            // Multiple vobsub image can be shown in a single line, so extract each image
            // and concatenate them before displaying.
            List<string> vobsubImages = UtilsSubs.extractVobsubFilesFromText(selectedComb.Subs2.Text);
            List<Image> vobSubImages = new List<Image>();

            foreach (string vobsubImage in vobsubImages)
            {
              Image image = UtilsSnapshot.getImageFromFile(previewWorkerVars.MediaDir + Path.DirectorySeparatorChar + vobsubImage);
              vobSubImages.Add(image);
            }

            if (vobSubImages.Count != 0)
            {
              Image image;

              if (vobSubImages.Count == 0)
              {
                image = vobSubImages[0];
              }
              else
              {
                image = UtilsSnapshot.concatImages(vobSubImages);
              }

              this.pictureBoxSubs2.Image = image;
            }
          }
          catch
          {

          }
        }
        else
        {
          this.textBoxSubs2.Visible = true;
          this.pictureBoxSubs2.Visible = false;
          this.textBoxSubs2.Text = selectedComb.Subs2.Text;
        }
      }
      else
      {
        this.textBoxSubs2.Visible = true;
        this.pictureBoxSubs2.Visible = false;
        this.textBoxSubs2.Text = "";
      }
    }

    /// <summary>
    /// Update the image preview.
    /// </summary>
    private void updateImagePreview(InfoCombined selectedComb)
    {
      if (selectedComb == null)
      {
        return;
      }

      // Update the snapshot
      if (Settings.Instance.VideoClips.Files.Length > 0 && this.checkBoxSnapshotPreview.Checked)
      {
        string videoFileName = Settings.Instance.VideoClips.Files[this.comboBoxEpisode.SelectedIndex];
        DateTime midTime = UtilsSubs.getMidpointTime(selectedComb.Subs1.StartTime, selectedComb.Subs1.EndTime);
        string outFile = previewWorkerVars.MediaDir + Path.DirectorySeparatorChar + ConstantSettings.TempImageFilename;

        try
        {
          File.Delete(outFile);
        }
        catch
        {
          // File is locked
          return;
        }

        UtilsSnapshot.takeSnapshotFromVideo(videoFileName, midTime, Settings.Instance.Snapshots.Size,
              Settings.Instance.Snapshots.Crop, outFile);

        if (File.Exists(outFile))
        {
          Image image = UtilsSnapshot.getImageFromFile(outFile);

          image = UtilsSnapshot.fitImageToSize(image, new Size(this.pictureBoxImage.Width, this.pictureBoxImage.Height));

          this.pictureBoxImage.Image = image;
        }
      }
    }

    
    private void updateGuiWithInfo(InfoCombined selectedComb)
    {
      if (selectedComb == null)
      {
        return;
      }

      currentInfoComb = selectedComb;

      updateTextPreview(currentInfoComb);
      updateImagePreview(currentInfoComb);
      audioPreviewNeedsUpdating = true;

      this.textBoxTimings.Text = this.formatTimeForDisplay(currentInfoComb.Subs1.StartTime) 
        + "  -  " + this.formatTimeForDisplay(currentInfoComb.Subs1.EndTime);
    }


    private void listViewLines_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
    {
      if ((this.listViewLines.SelectedItems.Count != 1) || (!enableListViewItemSelected))
      {
        return;
      }
      
      updateGuiWithInfo((InfoCombined)this.listViewLines.SelectedItems[0].Tag);
      
      // Update the find index
      for (int i = 0; i < this.listViewLines.Items.Count; i++)
      {
        if (((InfoCombined)this.listViewLines.Items[i].Tag) == ((InfoCombined)this.listViewLines.SelectedItems[0].Tag))
        {
          lastFindIndex = i;
        }
      }  
    }


    /// <summary>
    /// Generate the preview in seperate thread.
    /// </summary>
    private void generatePreview()
    {
      // Fire event to tell MainForm to update the settings
      if (GeneretePreview != null)
      {
        GeneretePreview(this, EventArgs.Empty);
      }

      populateEpisodeComboBox();

      string tempPreviewDir = Path.GetTempPath() + ConstantSettings.TempPreviewDirName;

      if (Directory.Exists(tempPreviewDir))
      {
        try
        {
          Directory.Delete(tempPreviewDir, true);
        }
        catch
        {
          //UtilsMsg.showErrMsg("Unable to delete the temporary directory at:\n" + tempPreviewDir);
        }
      }

      // Create the temporary directory
      try
      {
        Directory.CreateDirectory(tempPreviewDir);
      }
      catch
      {
        UtilsMsg.showErrMsg("Cannot write to " + tempPreviewDir + "\nTry checking the directory's permissions.");
        return;
      }

      Logger.Instance.info("Preview: GO!");
      Logger.Instance.writeSettingsToLog();

      // Start the worker thread
      try
      {
        WorkerVars workerVars = new WorkerVars(null, tempPreviewDir, WorkerVars.SubsProcessingType.Preview);

        // Create a background thread
        BackgroundWorker bw = new BackgroundWorker();
        bw.DoWork += new DoWorkEventHandler(bw_DoWork);
        bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);

        // Create a progress dialog on the UI thread
        dialogProgress = new DialogProgress();

        bw.RunWorkerAsync(workerVars);

        // Lock up the UI with this modal progress form
        dialogProgress.ShowDialog();
        dialogProgress = null;
      }
      catch (Exception e1)
      {
        UtilsMsg.showErrMsg("Something went wrong before preview could be generated.\n" + e1);
        return;
      }      
    }


    /// <summary>
    /// Does the work of the preview thread. Performs the processing on subtitles.
    /// </summary>
    private void bw_DoWork(object sender, DoWorkEventArgs e)
    {
      WorkerVars workerVars = e.Argument as WorkerVars;
      List<List<InfoCombined>> combinedAll = new List<List<InfoCombined>>();
      WorkerSubs subsWorker = new WorkerSubs();
      int totalLines = 0;

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

      foreach (List<InfoCombined> combArray in workerVars.CombinedAll)
      {
        totalLines += combArray.Count;
      }

      if (totalLines == 0)
      {
        UtilsMsg.showErrMsg("No lines of dialog could be parsed from the subtitle files.\nPlease check that they are valid.");
        e.Cancel = true;
        return;
      }

      // Remove lines
      try
      {
        combinedAll = subsWorker.inactivateLines(workerVars, dialogProgress);

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
        UtilsMsg.showErrMsg("Something went wrong while removing lines.\n" + e1);
        e.Cancel = true;
        return;
      }

      totalLines = 0;
      foreach (List<InfoCombined> combArray in workerVars.CombinedAll)
      {
        totalLines += combArray.Count;
      }

      if (totalLines == 0)
      {
        UtilsMsg.showErrMsg("No lines will be processed. Please check your settings to make\nsure that you are not mistakenly pruning too many lines.");
        e.Cancel = true;
        return;
      }

      e.Result = workerVars;
    }


    /// <summary>
    /// Gets called when all preview's subtitle processing is finished (or cancelled).
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
        return;
      }

      WorkerVars workerVars = e.Result as WorkerVars;
      previewWorkerVars = workerVars;

      updateStats();
      populateLinesListView(this.comboBoxEpisode.SelectedIndex);

      Logger.Instance.info("Preview: COMPLETED");
    }


    /// <summary>
    /// Show full-sized preview of snapshot.
    /// </summary>
    private void pictureBoxImage_Click(object sender, EventArgs e)
    {
      if (previewWorkerVars == null)
      {
        return;
      }

      string snapshotFile = previewWorkerVars.MediaDir + Path.DirectorySeparatorChar + ConstantSettings.TempImageFilename;

      if (File.Exists(snapshotFile))
      {
        DialogPreviewSnapshot dlg = new DialogPreviewSnapshot();

        dlg.Snapshot = UtilsSnapshot.getImageFromFile(snapshotFile);

        dlg.ShowDialog();
      }
    }



    private void backgroundWorkerAudio_DoWork(object sender, DoWorkEventArgs e)
    {
      InfoCombined selectedComb = currentInfoComb;
      DateTime startTime = selectedComb.Subs1.StartTime;
      DateTime endTime = selectedComb.Subs1.EndTime;
      string outMp3File = Path.Combine(previewWorkerVars.MediaDir, ConstantSettings.TempAudioFilename);
      string outWavFile = Path.Combine(previewWorkerVars.MediaDir, ConstantSettings.TempAudioPreviewFilename);


      if (audioPreviewNeedsUpdating)
      {
        audioPreviewNeedsUpdating = false;

        if (Settings.Instance.AudioClips.UseAudioFromVideo)
        {
          if (Settings.Instance.VideoClips.Files.Length > 0)
          {
            string videoFileName = Settings.Instance.VideoClips.Files[selectedEpisodeIndex];

            // Apply pad (if requested)
            if (Settings.Instance.AudioClips.PadEnabled)
            {
              startTime = UtilsSubs.applyTimePad(selectedComb.Subs1.StartTime, -Settings.Instance.AudioClips.PadStart);
              endTime = UtilsSubs.applyTimePad(selectedComb.Subs1.EndTime, Settings.Instance.AudioClips.PadEnd);
            }

            UtilsAudio.ripAudioFromVideo(videoFileName,
               Settings.Instance.VideoClips.AudioStream.Num,
               startTime, endTime,
               Settings.Instance.AudioClips.Bitrate, outMp3File, null);
          }
        }
        else // Audio file is provided
        {
          if (Settings.Instance.AudioClips.Files.Length > 0)
          {
            string fileToCut = Settings.Instance.AudioClips.Files[selectedEpisodeIndex];
            bool inputFileIsMp3 = (Path.GetExtension(fileToCut).ToLower() == ".mp3");

            // Apply pad (if requested)
            if (Settings.Instance.AudioClips.PadEnabled)
            {
              startTime = UtilsSubs.applyTimePad(startTime, -Settings.Instance.AudioClips.PadStart);
              endTime = UtilsSubs.applyTimePad(endTime, Settings.Instance.AudioClips.PadEnd);
            }

            if (ConstantSettings.ReencodeBeforeSplittingAudio || !inputFileIsMp3)
            {
              UtilsAudio.ripAudioFromVideo(fileToCut,
               "0",
               startTime, endTime,
               Settings.Instance.AudioClips.Bitrate, outMp3File, null);
            }
            else
            {
              UtilsAudio.cutAudio(fileToCut, startTime, endTime, outMp3File);
            }
          }
        }

        // If no external audio player is specified, convert the .mp3 to .wav so that it can be played with SoundPlayer
        if (File.Exists(outMp3File))
        {
          UtilsAudio.convertAudioFormat(outMp3File, outWavFile, 2);
        }
      }
    }

    private void backgroundWorkerAudio_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
      string outWavFile = Path.Combine(previewWorkerVars.MediaDir, ConstantSettings.TempAudioPreviewFilename);

      // Reset audio preview button
      this.buttonPreviewAudio.Enabled = true;
      this.buttonPreviewAudio.Text = "Preview &Audio";

      try
      {
        // Play the audio using .Net's built-in audio player (only supports .wav)
        if (File.Exists(outWavFile))
        {
          SoundPlayer player = new SoundPlayer(outWavFile);
          player.Play();
        }
      }
      catch
      {
        return;
      }
    }

    
    /// <summary>
    /// Preview the audio for the selected line.
    /// </summary>
    private void buttonPreviewAudio_Click(object sender, EventArgs e)
    {
      if (currentInfoComb == null || previewWorkerVars == null)
      {
        return;
      }

      if (!this.backgroundWorkerAudio.IsBusy)
      {
        this.selectedEpisodeIndex = this.comboBoxEpisode.SelectedIndex;

        this.buttonPreviewAudio.Enabled = false;
        this.buttonPreviewAudio.Text = "Extracting Audio...";

        this.backgroundWorkerAudio.RunWorkerAsync();
      }
    }


    private void comboBoxEpisode_SelectedIndexChanged(object sender, EventArgs e)
    {
      updateStats();
      populateLinesListView(this.comboBoxEpisode.SelectedIndex);
      lastFindIndex = 0;
    }


    /// <summary>
    /// Format a time for display in the list. Example: 0:00:36.160.
    /// </summary>
    private string formatTimeForDisplay(DateTime time)
    {
      return String.Format("{0}:{1:00.}:{2:00.}.{3:000.}",
                      (int)time.TimeOfDay.TotalHours,
                      time.TimeOfDay.Minutes,
                      time.TimeOfDay.Seconds,
                      time.TimeOfDay.Milliseconds);
    }


    /// <summary>
    /// Is the specified line too long as determined by the "Long Clip Warning" preference?
    /// </summary>
    private bool isLineTooLong(InfoCombined comb)
    {
      DateTime durationTime = UtilsSubs.getDurationTime(comb.Subs1.StartTime, comb.Subs1.EndTime);

      if ((ConstantSettings.LongClipWarningSeconds > 0) 
        && (durationTime.TimeOfDay.TotalMilliseconds > (ConstantSettings.LongClipWarningSeconds * 1000)))
      {
        return true;
      }

      return false;
    }


    /// <summary>
    /// Set the list to the lines of the provided episode.
    /// </summary>
    private void populateLinesListView(int episodeIndex)
    {
      if (previewWorkerVars == null)
      {
        return;
      }

      this.listViewLines.BeginUpdate();

      this.listViewLines.Items.Clear();

      // Create a new row for each comb
      foreach (InfoCombined comb in previewWorkerVars.CombinedAll[episodeIndex])
      {
        // Create the text for both columns
        string[] entryToAdd = new String[5];
        entryToAdd[0] = comb.Subs1.Text;

        if (Settings.Instance.Subs[1].Files.Length != 0)
        {
          entryToAdd[1] = comb.Subs2.Text;
        }

        entryToAdd[2] = this.formatTimeForDisplay(comb.Subs1.StartTime);
        entryToAdd[3] = this.formatTimeForDisplay(comb.Subs1.EndTime);

        if (this.isLineTooLong(comb))
        {
          entryToAdd[4] = this.formatTimeForDisplay(UtilsSubs.getDurationTime(comb.Subs1.StartTime, comb.Subs1.EndTime)) + " (Long!)";
        }
        else
        {
          entryToAdd[4] = this.formatTimeForDisplay(UtilsSubs.getDurationTime(comb.Subs1.StartTime, comb.Subs1.EndTime));
        }

        ListViewItem item = new ListViewItem(entryToAdd);

        // Embed a comb into each item
        item.Tag = comb;
        this.listViewLines.Items.Add(item);
      }

      for (int i = 0; i < this.listViewLines.Items.Count; i++)
      {
        InfoCombined comb = (InfoCombined)this.listViewLines.Items[i].Tag;

        if (comb.Active)
        {
          this.listViewLines.Items[i].BackColor = activeColor;
        }
        else
        {
          this.listViewLines.Items[i].BackColor = inactiveColor;
        }

        if (this.isLineTooLong(comb))
        {
          this.listViewLines.Items[i].ForeColor = longLineWarningColor;
        }
        else
        {
          this.listViewLines.Items[i].ForeColor = Color.Black;
        }
      }

      this.listViewLines.EndUpdate();
    }


    /// <summary>
    /// Update the statistics box.
    /// </summary>
    private void updateStats()
    {
      int linesInCurrentEpisode = 0;
      int activeLinesInCurrentEpisode = 0;
      int inactiveLinesInCurrentEpisode = 0;
      int totalLines = 0;
      int totalActiveLines = 0;
      int totalInactiveLines = 0;
   
      if (previewWorkerVars == null)
      {
        return;
      }

      linesInCurrentEpisode = previewWorkerVars.CombinedAll[this.comboBoxEpisode.SelectedIndex].Count;

      foreach (InfoCombined comb in previewWorkerVars.CombinedAll[this.comboBoxEpisode.SelectedIndex])
      {
        if (comb.Active)
        {
          activeLinesInCurrentEpisode++;
        }
        else
        {
          inactiveLinesInCurrentEpisode++;
        }
      }

      foreach (List<InfoCombined> combArray in previewWorkerVars.CombinedAll)
      {
        totalLines += combArray.Count;

        foreach (InfoCombined comb in combArray)
        {
          if (comb.Active)
          {
            totalActiveLines++;
          }
          else
          {
            totalInactiveLines++;
          }
        }
      }

      this.labelStatsLinesInCurrentEpisodeNum.Text = linesInCurrentEpisode.ToString();
      this.labelStatsActiveLinesInCurrentEpisodeNum.Text = activeLinesInCurrentEpisode.ToString();
      this.labelStatsInactiveLinesInCurrentEpisodeNum.Text = inactiveLinesInCurrentEpisode.ToString();
      this.labelStatsTotalLinesNum.Text = totalLines.ToString();
      this.labelStatsTotalActiveLinesNum.Text = totalActiveLines.ToString();
      this.labelStatsTotalInactiveLinesNum.Text = totalInactiveLines.ToString();
    }


    private void buttonSelectAll_Click(object sender, EventArgs e)
    {
      selectAll();
    }

    private void selectAll()
    {
      this.listViewLines.Focus();

      enableListViewItemSelected = false;

      for (int i = 0; i < this.listViewLines.Items.Count; i++)
      {
        this.listViewLines.Items[i].Selected = true;
      }

      enableListViewItemSelected = true;

      if (this.listViewLines.SelectedItems.Count > 0)
      {
        updateGuiWithInfo((InfoCombined)this.listViewLines.SelectedItems[0].Tag);
      }
    }


    private void buttonSelectNone_Click(object sender, EventArgs e)
    {
      this.listViewLines.Focus();

      enableListViewItemSelected = false;

      for (int i = 0; i < this.listViewLines.Items.Count; i++)
      {
        this.listViewLines.Items[i].Selected = false;
      }

      enableListViewItemSelected = true;
    }


    private void buttonInvert_Click(object sender, EventArgs e)
    {
      this.listViewLines.Focus();

      enableListViewItemSelected = false;

      for (int i = 0; i < this.listViewLines.Items.Count; i++)
      {
        this.listViewLines.Items[i].Selected = !this.listViewLines.Items[i].Selected;
      }

      enableListViewItemSelected = true;

      if (this.listViewLines.SelectedItems.Count > 0)
      {
        updateGuiWithInfo((InfoCombined)this.listViewLines.SelectedItems[0].Tag);
      }
    }


    private void buttonDeactivate_Click(object sender, EventArgs e)
    {
      if ((this.listViewLines.SelectedItems.Count == 0) || (previewWorkerVars == null))
      {
        return;
      }

      this.listViewLines.Focus();

      for (int i = 0; i < this.listViewLines.Items.Count; i++)
      {
        if(this.listViewLines.Items[i].Selected)
        {
          previewWorkerVars.CombinedAll[this.comboBoxEpisode.SelectedIndex][i].Active = false;

          this.listViewLines.Items[i].BackColor = inactiveColor;
        }
      }

      updateTextPreview(currentInfoComb);
      updateStats();
      changesHaveBeenMade = true;
    }


    private void buttonActivate_Click(object sender, EventArgs e)
    {
      if ((this.listViewLines.SelectedItems.Count == 0) || (previewWorkerVars == null))
      {
        return;
      }

      this.listViewLines.Focus();

      for (int i = 0; i < this.listViewLines.Items.Count; i++)
      {
        if (this.listViewLines.Items[i].Selected)
        {
          previewWorkerVars.CombinedAll[this.comboBoxEpisode.SelectedIndex][i].Active = true;

          this.listViewLines.Items[i].BackColor = activeColor;
        }
      }

      updateTextPreview(currentInfoComb);
      updateStats();
      changesHaveBeenMade = true;
    }


    /// <summary>
    /// Get the total number of long active lines.
    /// </summary>
    private int getNumberOfLongActiveLines(List<List<InfoCombined>> combinedAll)
    {
      int longLineCount = 0;

      foreach (List<InfoCombined> combList in combinedAll)
      {
        foreach (InfoCombined comb in combList)
        {
          if (comb.Active && this.isLineTooLong(comb))
          {
            longLineCount++;
          }
        }
      }

      return longLineCount;
    }


    private void buttonGo_Click(object sender, EventArgs e)
    {
      int activeLongLineCount = this.getNumberOfLongActiveLines(previewWorkerVars.CombinedAll);

      if (activeLongLineCount > 0)
      {
        bool cont = UtilsMsg.showConfirm(String.Format("There are {0} active lines that have a long duration (> {1} seconds).\r\n\r\n"
          + "Do you wish to continue?\r\n\r\n"
          + "(These long lines are colored red. See \"Preferences > Misc > Long Clip Warning\" to configure threshold.)", 
          activeLongLineCount,
          ConstantSettings.LongClipWarningSeconds));

        if (!cont)
        {
          return;
        }
      }

      SubsProcessor subsProcessor = new SubsProcessor();

      if ((previewWorkerVars != null) && (previewWorkerVars.CombinedAll != null))
      {
        subsProcessor.start(previewWorkerVars.CombinedAll);
      }
    }


    private void DialogPreview_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (changesHaveBeenMade)
      {
        if (MessageBox.Show("Close the preview and discard the changes you have made?", "Exit Preview?",
          MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel)
        {
          e.Cancel = true;
        }
      }
    }

    private void textBoxSubs1_TextChanged(object sender, EventArgs e)
    { 
      if (previewWorkerVars == null || currentInfoComb == null)
      {
        return;
      }

      for (int i = 0; i < this.listViewLines.Items.Count; i++)
      {
        if ((this.listViewLines.Items[i].Tag == currentInfoComb) 
          && (((InfoCombined)this.listViewLines.Items[i].Tag).Subs1.Text != this.textBoxSubs1.Text))
        {
          currentInfoComb.Subs1.Text = this.textBoxSubs1.Text;
          ((InfoCombined)this.listViewLines.Items[i].Tag).Subs1.Text = this.textBoxSubs1.Text;
          this.listViewLines.Items[i].Text = this.textBoxSubs1.Text;
          changesHaveBeenMade = true;
          break;
        }
      }
    }


    private void textBoxSubs2_TextChanged(object sender, EventArgs e)
    {
      if (previewWorkerVars == null || currentInfoComb == null || (Settings.Instance.Subs[1].Files.Length == 0))
      {
        return;
      }

      for (int i = 0; i < this.listViewLines.Items.Count; i++)
      {
        if ((this.listViewLines.Items[i].Tag == currentInfoComb)
          && (((InfoCombined)this.listViewLines.Items[i].Tag).Subs2.Text != this.textBoxSubs2.Text))
        {
          currentInfoComb.Subs2.Text = this.textBoxSubs2.Text;
          ((InfoCombined)this.listViewLines.Items[i].Tag).Subs2.Text = this.textBoxSubs2.Text;
          this.listViewLines.Items[i].SubItems[1].Text = this.textBoxSubs2.Text;
          changesHaveBeenMade = true;
          break;
        }
      }
    }


    private void buttonFind_Click(object sender, EventArgs e)
    {
      findNext(this.textBoxFind.Text);
    }


    /// <summary>
    /// Select the next line matching the provided text.
    /// </summary>
    private void findNext(string searchText)
    {
      bool found = false;

      if ((searchText.Length == 0) || (this.listViewLines.Items.Count == 0))
      {
        return;
      }

      if (lastFindIndex > this.listViewLines.Items.Count)
      {
        lastFindIndex = 0;
      }

      searchText = searchText.ToLower();

      bool searchActive = false;
      bool searchInactive = false;

      if (searchText.StartsWith("a:"))
      {
        searchActive = true;
        searchText = searchText.Remove(0, 2);
      }
      else if(searchText.StartsWith("i:"))
      {
        searchInactive = true;
        searchText = searchText.Remove(0, 2);
      }
    
      for (int i = lastFindIndex; i < this.listViewLines.Items.Count; i++)
      {
        this.listViewLines.Items[i].Selected = false;
      }

      for (int i = Math.Min(lastFindIndex + 1, this.listViewLines.Items.Count); i < this.listViewLines.Items.Count; i++)
      {
        if (searchActive)
        {
          if ((((InfoCombined)this.listViewLines.Items[i].Tag).Active) && 
            ((((InfoCombined)this.listViewLines.Items[i].Tag).Subs1.Text.ToLower().Contains(searchText))
            || (((InfoCombined)this.listViewLines.Items[i].Tag).Subs2.Text.ToLower().Contains(searchText)) || searchText == ""))
          {
            found = true;
          }
        }
        else if(searchInactive)
        {
          if (!(((InfoCombined)this.listViewLines.Items[i].Tag).Active) &&
            ((((InfoCombined)this.listViewLines.Items[i].Tag).Subs1.Text.ToLower().Contains(searchText))
            || (((InfoCombined)this.listViewLines.Items[i].Tag).Subs2.Text.ToLower().Contains(searchText)) || searchText == ""))
          {
            found = true;
          }
        }
        else
        {
          if ((((InfoCombined)this.listViewLines.Items[i].Tag).Subs1.Text.ToLower().Contains(searchText))
            || (((InfoCombined)this.listViewLines.Items[i].Tag).Subs2.Text.ToLower().Contains(searchText)))
          {
            found = true;
          }
        }

        if(found)
        {
          this.listViewLines.Items[i].Selected = true;
          this.listViewLines.EnsureVisible(i);
          lastFindIndex = i;
          found = true;
          break;
        }
      }

      if (!found)
      {
        for (int i = 0; i <= lastFindIndex; i++)
        {
          if (searchActive)
          {
            if ((((InfoCombined)this.listViewLines.Items[i].Tag).Active) &&
              ((((InfoCombined)this.listViewLines.Items[i].Tag).Subs1.Text.ToLower().Contains(searchText))
              || (((InfoCombined)this.listViewLines.Items[i].Tag).Subs2.Text.ToLower().Contains(searchText)) || searchText == ""))
            {
              found = true;
            }
          }
          else if (searchInactive)
          {
            if (!(((InfoCombined)this.listViewLines.Items[i].Tag).Active) &&
              ((((InfoCombined)this.listViewLines.Items[i].Tag).Subs1.Text.ToLower().Contains(searchText))
              || (((InfoCombined)this.listViewLines.Items[i].Tag).Subs2.Text.ToLower().Contains(searchText)) || searchText == ""))
            {
              found = true;
            }
          }
          else
          {
            if ((((InfoCombined)this.listViewLines.Items[i].Tag).Subs1.Text.ToLower().Contains(searchText))
              || (((InfoCombined)this.listViewLines.Items[i].Tag).Subs2.Text.ToLower().Contains(searchText)))
            {
              found = true;
            }
          }

          if (found)
          {
            this.listViewLines.Items[i].Selected = true;
            this.listViewLines.EnsureVisible(i);
            lastFindIndex = i;
            found = true;
            break;
          }
        }
      }
    }


    private void textBoxFind_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode == Keys.Enter)
      {
        findNext(this.textBoxFind.Text);
      }
    }


    /// <summary>
    /// Don't beep at the user when enter is pressed.
    /// </summary>
    private void textBoxFind_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (e.KeyChar == (char)Keys.Enter)
      {
        e.Handled = true;
      }

      base.OnKeyPress(e);
    }


    private void listViewLines_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.Control && (e.KeyCode == Keys.A) )
      {
        selectAll();
      }
    }
    

    /// <summary>
    /// Experimental video preview on the selected line.
    /// </summary>
    private void buttonPreviewVideo_Click(object sender, EventArgs e)
    {
      string videoConfigPlayer = "";

      if (previewWorkerVars == null || currentInfoComb == null || Settings.Instance.VideoClips.Files.Length == 0)
      {
        return;
      }

      videoConfigPlayer = ConstantSettings.VideoPlayer;

      // Check if the player exists
      if (!File.Exists(videoConfigPlayer))
      {
        UtilsMsg.showErrMsg(String.Format("The video player \"{0}\" \nprovided in {1} does not exist!",
          ConstantSettings.VideoPlayer, ConstantSettings.SettingsFilename));

        return;
      }

      // Kill the player before running it again.
      // For some reason the players that I tested (MPC-HC, ZoomPlayer, VLC) don't 
      // read the start time arg correctly unless they are killed first.
      if (videoPlayerProcess != null)
      {
        if (!videoPlayerProcess.HasExited)
        {
          videoPlayerProcess.Kill();
        }
      }

      DateTime startTime = currentInfoComb.Subs1.StartTime;
      DateTime endTime = currentInfoComb.Subs1.EndTime;

      // Apply pad (if requested)
      if (Settings.Instance.VideoClips.PadEnabled)
      {
        startTime = UtilsSubs.applyTimePad(startTime, -Settings.Instance.VideoClips.PadStart);
        endTime = UtilsSubs.applyTimePad(endTime, Settings.Instance.VideoClips.PadEnd);
      }

      UtilsName name = new UtilsName(Settings.Instance.DeckName, Settings.Instance.VideoClips.Files.Length,
        0, endTime, Settings.Instance.VideoClips.Size.Width, Settings.Instance.VideoClips.Size.Height);

      string videoConfigArgs = name.createName(ConstantSettings.VideoPlayerArgs, 
        this.comboBoxEpisode.SelectedIndex + Settings.Instance.EpisodeStartNumber - 1, 0, startTime, endTime, "", "");
    
      string videoFileName = Settings.Instance.VideoClips.Files[this.comboBoxEpisode.SelectedIndex];
      string procArgs = String.Format("{0} \"{1}\"", videoConfigArgs, videoFileName);

      // Play the video
      videoPlayerProcess = new Process();
      videoPlayerProcess.StartInfo.FileName = videoConfigPlayer;
      videoPlayerProcess.StartInfo.Arguments = procArgs;
      videoPlayerProcess.StartInfo.UseShellExecute = false;
      videoPlayerProcess.StartInfo.CreateNoWindow = true;
      videoPlayerProcess.Start();
    }



  }
}
