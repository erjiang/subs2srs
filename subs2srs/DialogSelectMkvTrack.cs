using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace subs2srs
{
  public partial class DialogSelectMkvTrack : Form
  {
    private string mvkFile = "";
    private int subsNum = 1;
    private List<MkvTrack> subTrackList = new List<MkvTrack>();
    private MkvTrack selectedTrack = new MkvTrack();

    public string ExtractedFile { get; set; }

    public DialogSelectMkvTrack(string mvkFile, int subsNum, List<MkvTrack> subTrackList)
    {
      InitializeComponent();
      this.mvkFile = mvkFile;
      this.subsNum = subsNum;
      this.subTrackList = subTrackList;
      ExtractedFile = "";
    }

    private void DialogSelectMkvTrack_Load(object sender, EventArgs e)
    {
      this.labelProgress.Visible = false;
      this.progressBarMain.Visible = false;
      
      foreach (MkvTrack track in this.subTrackList)
      {
        comboBoxTrack.Items.Add(track);
      }

      this.comboBoxTrack.SelectedIndex = 0;
    }


    private void buttonExtract_Click(object sender, EventArgs e)
    {
      this.labelProgress.Visible = true;
      this.progressBarMain.Visible = true;
      this.buttonExtract.Enabled = false;
      this.selectedTrack = (MkvTrack)this.comboBoxTrack.SelectedItem;
      
      this.backgroundWorkerMain.RunWorkerAsync();
    }


    private void backgroundWorkerMain_DoWork(object sender, DoWorkEventArgs e)
    {
      string tempFileName = ConstantSettings.TempMkvExtractSubs1Filename;

      if (subsNum == 2)
      {
        tempFileName = ConstantSettings.TempMkvExtractSubs2Filename;
      }

      this.ExtractedFile = String.Format("{0}{1}.{2}",
        Path.GetTempPath(), tempFileName, this.selectedTrack.Extension);

      UtilsMkv.extractTrack(this.mvkFile, this.selectedTrack.TrackID, this.ExtractedFile);

      // The subs1 and subs2 textboxes, take .idx files rather than .sub files
      if (Path.GetExtension(this.ExtractedFile) == ".sub")
      {
        this.ExtractedFile = Path.ChangeExtension(this.ExtractedFile, ".idx");
      }
    }


    private void backgroundWorkerMain_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
      this.DialogResult = DialogResult.OK;
      this.Close();
    }




  }
}
