namespace subs2srs
{
  partial class DialogSelectMkvTrack
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.comboBoxTrack = new System.Windows.Forms.ComboBox();
      this.buttonExtract = new System.Windows.Forms.Button();
      this.labelTrack = new System.Windows.Forms.Label();
      this.labelProgress = new System.Windows.Forms.Label();
      this.progressBarMain = new System.Windows.Forms.ProgressBar();
      this.backgroundWorkerMain = new System.ComponentModel.BackgroundWorker();
      this.SuspendLayout();
      // 
      // comboBoxTrack
      // 
      this.comboBoxTrack.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.comboBoxTrack.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboBoxTrack.DropDownWidth = 230;
      this.comboBoxTrack.FormattingEnabled = true;
      this.comboBoxTrack.Location = new System.Drawing.Point(15, 41);
      this.comboBoxTrack.MaxDropDownItems = 15;
      this.comboBoxTrack.Name = "comboBoxTrack";
      this.comboBoxTrack.Size = new System.Drawing.Size(261, 21);
      this.comboBoxTrack.TabIndex = 3;
      // 
      // buttonExtract
      // 
      this.buttonExtract.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonExtract.Location = new System.Drawing.Point(201, 91);
      this.buttonExtract.Name = "buttonExtract";
      this.buttonExtract.Size = new System.Drawing.Size(75, 23);
      this.buttonExtract.TabIndex = 4;
      this.buttonExtract.Text = "&Extract";
      this.buttonExtract.UseVisualStyleBackColor = true;
      this.buttonExtract.Click += new System.EventHandler(this.buttonExtract_Click);
      // 
      // labelTrack
      // 
      this.labelTrack.AutoSize = true;
      this.labelTrack.Location = new System.Drawing.Point(12, 25);
      this.labelTrack.Name = "labelTrack";
      this.labelTrack.Size = new System.Drawing.Size(161, 13);
      this.labelTrack.TabIndex = 5;
      this.labelTrack.Text = "Select MKV subtitle track to use:";
      // 
      // labelProgress
      // 
      this.labelProgress.AutoSize = true;
      this.labelProgress.Location = new System.Drawing.Point(12, 75);
      this.labelProgress.Name = "labelProgress";
      this.labelProgress.Size = new System.Drawing.Size(126, 13);
      this.labelProgress.TabIndex = 5;
      this.labelProgress.Text = "Extracting subtitle track...";
      this.labelProgress.Visible = false;
      // 
      // progressBarMain
      // 
      this.progressBarMain.Location = new System.Drawing.Point(15, 91);
      this.progressBarMain.Name = "progressBarMain";
      this.progressBarMain.Size = new System.Drawing.Size(158, 23);
      this.progressBarMain.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
      this.progressBarMain.TabIndex = 6;
      this.progressBarMain.Visible = false;
      // 
      // backgroundWorkerMain
      // 
      this.backgroundWorkerMain.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorkerMain_DoWork);
      this.backgroundWorkerMain.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorkerMain_RunWorkerCompleted);
      // 
      // DialogSelectMkvTrack
      // 
      this.AcceptButton = this.buttonExtract;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(288, 126);
      this.Controls.Add(this.progressBarMain);
      this.Controls.Add(this.labelProgress);
      this.Controls.Add(this.labelTrack);
      this.Controls.Add(this.buttonExtract);
      this.Controls.Add(this.comboBoxTrack);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "DialogSelectMkvTrack";
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.Text = "Select MKV Subtitle Track";
      this.Load += new System.EventHandler(this.DialogSelectMkvTrack_Load);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.ComboBox comboBoxTrack;
    private System.Windows.Forms.Button buttonExtract;
    private System.Windows.Forms.Label labelTrack;
    private System.Windows.Forms.Label labelProgress;
    private System.Windows.Forms.ProgressBar progressBarMain;
    private System.ComponentModel.BackgroundWorker backgroundWorkerMain;
  }
}