using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;

namespace subs2srs
{
  public partial class DialogMkvExtract : Form
  {
    private List<string> selectedMkvFiles = new List<string>();
    private string outDir = "";
    private int invalidCount = 0;
    private string trackTypeToExtract = "";

    public DialogMkvExtract()
    {
      InitializeComponent();

      this.comboBoxTrackTypeToExtract.SelectedIndex = 0;
      this.textBoxNumFilesSelected.Text = "";
      this.textBoxOutDir.Text = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
    }


    private void DialogMkvExtract_FormClosing(object sender, FormClosingEventArgs e)
    {
      this.backgroundWorkerMain.CancelAsync();
    }

    private string convertFilelistToStr(List<string> fileList)
    {
      string outStr = "";

      foreach (string file in fileList)
      {
        outStr += String.Format("\"{0}\", ", Path.GetFileName(file));
      }

      outStr = outStr.Trim(new char[] { ',', ' ' });

      return outStr;
    }


    private void updateFilelistDisplay(List<string> fileList)
    {
      this.textBoxMkvFiles.Text = this.convertFilelistToStr(fileList);

      if (fileList.Count == 1)
      {
        this.textBoxNumFilesSelected.Text = String.Format("{0} file selected", fileList.Count.ToString());
      }
      else
      {
        this.textBoxNumFilesSelected.Text = String.Format("{0} files selected", fileList.Count.ToString());
      }
    }


    private void buttonMkvFiles_Click(object sender, EventArgs e)
    {
      DialogResult result = this.openFileDialogMkv.ShowDialog();

      if (result == DialogResult.OK)
      {
        this.selectedMkvFiles.Clear();

        foreach (string file in this.openFileDialogMkv.FileNames)
        {
          if (Path.GetExtension(file) == ".mkv")
          {
            this.selectedMkvFiles.Add(file);
          }
        }

        this.updateFilelistDisplay(this.selectedMkvFiles);
      }
    }


    private void buttonOutDir_Click(object sender, EventArgs e)
    {
      DialogResult result = this.folderBrowserDialogOut.ShowDialog();

      if (result == DialogResult.OK)
      {
        this.textBoxOutDir.Text = this.folderBrowserDialogOut.SelectedPath;
      }
    }


    private void resetGuiToStartingState()
    {
      this.buttonExtract.Text = "&Extract";
      this.labelProgress.Visible = false;
      this.progressBarEpisode.Visible = false;
      this.progressBarTrack.Visible = false;
    }


    private void buttonExtract_Click(object sender, EventArgs e)
    {
      if (this.buttonExtract.Text == "&Extract")
      {
        this.errorProviderMain.Clear();

        if (this.validateForm())
        {
          this.buttonExtract.Text = "&Stop";
          this.labelProgress.Text = "";
          this.labelProgress.Visible = true;
          this.progressBarEpisode.Value = 0;
          this.progressBarEpisode.Visible = true;
          this.progressBarTrack.Value = 0;
          this.progressBarTrack.Visible = true;

          this.trackTypeToExtract = this.comboBoxTrackTypeToExtract.Text;
          this.outDir = this.textBoxOutDir.Text;


          this.backgroundWorkerMain.RunWorkerAsync();
        }
      }
      else // "Stop" was clicked
      {
        this.backgroundWorkerMain.CancelAsync();
      }
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


    private void textBoxMkvFiles_Validating(object sender, CancelEventArgs e)
    {
      string error = null;

      if (this.selectedMkvFiles.Count == 0)
      {
        error = "Please enter one or more MKV files.";
        invalidCount++;
      }

      this.errorProviderMain.SetError((Control)sender, error);
    }


    private void textBoxOutDir_Validating(object sender, CancelEventArgs e)
    {
      string error = null;

      if (!Directory.Exists(this.textBoxOutDir.Text))
      {
        error = "Please enter a valid output directory.";
        invalidCount++;
      }

      this.errorProviderMain.SetError((Control)sender, error);
    }


    private void backgroundWorkerMain_DoWork(object sender, DoWorkEventArgs e)
    {
      BackgroundWorker worker = sender as BackgroundWorker;

      MkvExtractProgress progress = new MkvExtractProgress();
      progress.MaxEpisode = this.selectedMkvFiles.Count;

      foreach (string file in this.selectedMkvFiles)
      {
        progress.CurEpisode++;

        List<MkvTrack> tracks = null;

        if (this.trackTypeToExtract == "All subtitle tracks")
        {
          tracks = UtilsMkv.getSubtitleTrackList(file);
        }
        else if (this.trackTypeToExtract == "All audio tracks")
        {
          tracks = UtilsMkv.getAudioTrackList(file);
        }
        else
        {
          tracks = UtilsMkv.getTrackList(file);
        }

        progress.CurTrack = 0;
        progress.MaxTrack = tracks.Count;

        foreach (MkvTrack track in tracks)
        {
          if (worker.CancellationPending)
          {
            e.Cancel = true;
            return;
          }

          progress.CurTrack++;

          worker.ReportProgress(0, progress);

          string displayLang = UtilsLang.LangThreeLetter2Full(track.Lang);

          if (displayLang == "")
          {
            displayLang = "Unknown";
          }

          string fileName = String.Format("{0}{1}{2} - Track {3:00} - {4}.{5}", 
            this.outDir,
            Path.DirectorySeparatorChar,
            Path.GetFileNameWithoutExtension(file),
            Convert.ToInt32(track.TrackID),
            displayLang,
            track.Extension);

          UtilsMkv.extractTrack(file, track.TrackID, fileName);
        }
      }      
    }


    private void backgroundWorkerMain_ProgressChanged(object sender, ProgressChangedEventArgs e)
    {
      try
      {
        MkvExtractProgress progress = (MkvExtractProgress)e.UserState;

        this.progressBarEpisode.Maximum = progress.MaxEpisode;
        this.progressBarEpisode.Value = progress.CurEpisode;

        this.progressBarTrack.Maximum = progress.MaxTrack;
        this.progressBarTrack.Value = progress.CurTrack;

        this.labelProgress.Text = String.Format("Extracting track {0}/{1} from file {2}/{3}...",
          progress.CurTrack,
          progress.MaxTrack,
          progress.CurEpisode,
          progress.MaxEpisode);

        this.Update();
      }
      catch
      {
        // Don't care
      }
    }


    private void backgroundWorkerMain_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
      if (e.Cancelled)
      {
        // Don't care
      }
      else if (e.Error != null)
      {
        UtilsMsg.showErrMsg("Something went wrong: " + e.Error.Message);
      }
      else
      {
        UtilsMsg.showInfoMsg("Extraction complete.");
      }

      this.resetGuiToStartingState();
    }
  }


  public class MkvExtractProgress
  {
    public int CurEpisode { get; set; }
    public int MaxEpisode { get; set; }
    public int CurTrack { get; set; }
    public int MaxTrack { get; set; }
  }

}
