namespace subs2srs
{
  partial class DialogMkvExtract
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
      this.components = new System.ComponentModel.Container();
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DialogMkvExtract));
      this.labelOutDir = new System.Windows.Forms.Label();
      this.buttonOutDir = new System.Windows.Forms.Button();
      this.textBoxOutDir = new System.Windows.Forms.TextBox();
      this.buttonCancel = new System.Windows.Forms.Button();
      this.buttonExtract = new System.Windows.Forms.Button();
      this.labelMkvFiles = new System.Windows.Forms.Label();
      this.buttonMkvFiles = new System.Windows.Forms.Button();
      this.textBoxMkvFiles = new System.Windows.Forms.TextBox();
      this.textBoxHelp = new System.Windows.Forms.TextBox();
      this.comboBoxTrackTypeToExtract = new System.Windows.Forms.ComboBox();
      this.labelTrackTypeToExtract = new System.Windows.Forms.Label();
      this.openFileDialogMkv = new System.Windows.Forms.OpenFileDialog();
      this.folderBrowserDialogOut = new System.Windows.Forms.FolderBrowserDialog();
      this.textBoxNumFilesSelected = new System.Windows.Forms.TextBox();
      this.errorProviderMain = new System.Windows.Forms.ErrorProvider(this.components);
      this.progressBarEpisode = new System.Windows.Forms.ProgressBar();
      this.labelProgress = new System.Windows.Forms.Label();
      this.backgroundWorkerMain = new System.ComponentModel.BackgroundWorker();
      this.progressBarTrack = new System.Windows.Forms.ProgressBar();
      ((System.ComponentModel.ISupportInitialize)(this.errorProviderMain)).BeginInit();
      this.SuspendLayout();
      // 
      // labelOutDir
      // 
      this.labelOutDir.AutoSize = true;
      this.labelOutDir.Location = new System.Drawing.Point(9, 132);
      this.labelOutDir.Name = "labelOutDir";
      this.labelOutDir.Size = new System.Drawing.Size(248, 13);
      this.labelOutDir.TabIndex = 16;
      this.labelOutDir.Text = "Directory where the extracted tracks will be placed:";
      // 
      // buttonOutDir
      // 
      this.buttonOutDir.Location = new System.Drawing.Point(12, 147);
      this.buttonOutDir.Name = "buttonOutDir";
      this.buttonOutDir.Size = new System.Drawing.Size(56, 22);
      this.buttonOutDir.TabIndex = 17;
      this.buttonOutDir.Text = "&Output...";
      this.buttonOutDir.UseVisualStyleBackColor = true;
      this.buttonOutDir.Click += new System.EventHandler(this.buttonOutDir_Click);
      // 
      // textBoxOutDir
      // 
      this.textBoxOutDir.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.textBoxOutDir.Location = new System.Drawing.Point(73, 148);
      this.textBoxOutDir.Name = "textBoxOutDir";
      this.textBoxOutDir.Size = new System.Drawing.Size(471, 20);
      this.textBoxOutDir.TabIndex = 18;
      this.textBoxOutDir.Validating += new System.ComponentModel.CancelEventHandler(this.textBoxOutDir_Validating);
      // 
      // buttonCancel
      // 
      this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.buttonCancel.Location = new System.Drawing.Point(469, 224);
      this.buttonCancel.Name = "buttonCancel";
      this.buttonCancel.Size = new System.Drawing.Size(75, 23);
      this.buttonCancel.TabIndex = 20;
      this.buttonCancel.Text = "&Cancel";
      this.buttonCancel.UseVisualStyleBackColor = true;
      // 
      // buttonExtract
      // 
      this.buttonExtract.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.buttonExtract.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.buttonExtract.ForeColor = System.Drawing.Color.DarkGreen;
      this.buttonExtract.Location = new System.Drawing.Point(230, 215);
      this.buttonExtract.Name = "buttonExtract";
      this.buttonExtract.Size = new System.Drawing.Size(108, 32);
      this.buttonExtract.TabIndex = 19;
      this.buttonExtract.Text = "&Extract";
      this.buttonExtract.UseVisualStyleBackColor = true;
      this.buttonExtract.Click += new System.EventHandler(this.buttonExtract_Click);
      // 
      // labelMkvFiles
      // 
      this.labelMkvFiles.AutoSize = true;
      this.labelMkvFiles.Location = new System.Drawing.Point(9, 37);
      this.labelMkvFiles.Name = "labelMkvFiles";
      this.labelMkvFiles.Size = new System.Drawing.Size(146, 13);
      this.labelMkvFiles.TabIndex = 13;
      this.labelMkvFiles.Text = "Select one or more .mkv files:\r\n";
      // 
      // buttonMkvFiles
      // 
      this.buttonMkvFiles.Location = new System.Drawing.Point(12, 52);
      this.buttonMkvFiles.Name = "buttonMkvFiles";
      this.buttonMkvFiles.Size = new System.Drawing.Size(56, 22);
      this.buttonMkvFiles.TabIndex = 14;
      this.buttonMkvFiles.Text = "&Files...";
      this.buttonMkvFiles.UseVisualStyleBackColor = true;
      this.buttonMkvFiles.Click += new System.EventHandler(this.buttonMkvFiles_Click);
      // 
      // textBoxMkvFiles
      // 
      this.textBoxMkvFiles.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.textBoxMkvFiles.Location = new System.Drawing.Point(73, 53);
      this.textBoxMkvFiles.Name = "textBoxMkvFiles";
      this.textBoxMkvFiles.ReadOnly = true;
      this.textBoxMkvFiles.Size = new System.Drawing.Size(471, 20);
      this.textBoxMkvFiles.TabIndex = 15;
      this.textBoxMkvFiles.Validating += new System.ComponentModel.CancelEventHandler(this.textBoxMkvFiles_Validating);
      // 
      // textBoxHelp
      // 
      this.textBoxHelp.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.textBoxHelp.BackColor = System.Drawing.Color.LemonChiffon;
      this.textBoxHelp.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.textBoxHelp.Location = new System.Drawing.Point(12, 9);
      this.textBoxHelp.Multiline = true;
      this.textBoxHelp.Name = "textBoxHelp";
      this.textBoxHelp.ReadOnly = true;
      this.textBoxHelp.Size = new System.Drawing.Size(532, 18);
      this.textBoxHelp.TabIndex = 115;
      this.textBoxHelp.TabStop = false;
      this.textBoxHelp.Text = "Use this tool to extract all subtitle and/or audio tracks from an MKV file.";
      this.textBoxHelp.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
      // 
      // comboBoxTrackTypeToExtract
      // 
      this.comboBoxTrackTypeToExtract.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboBoxTrackTypeToExtract.FormattingEnabled = true;
      this.comboBoxTrackTypeToExtract.Items.AddRange(new object[] {
            "All subtitle tracks",
            "All audio tracks",
            "All subtitle and audio tracks"});
      this.comboBoxTrackTypeToExtract.Location = new System.Drawing.Point(12, 100);
      this.comboBoxTrackTypeToExtract.Name = "comboBoxTrackTypeToExtract";
      this.comboBoxTrackTypeToExtract.Size = new System.Drawing.Size(179, 21);
      this.comboBoxTrackTypeToExtract.TabIndex = 116;
      // 
      // labelTrackTypeToExtract
      // 
      this.labelTrackTypeToExtract.AutoSize = true;
      this.labelTrackTypeToExtract.Location = new System.Drawing.Point(9, 84);
      this.labelTrackTypeToExtract.Name = "labelTrackTypeToExtract";
      this.labelTrackTypeToExtract.Size = new System.Drawing.Size(90, 13);
      this.labelTrackTypeToExtract.TabIndex = 13;
      this.labelTrackTypeToExtract.Text = "Tracks to extract:";
      // 
      // openFileDialogMkv
      // 
      this.openFileDialogMkv.Filter = "Matroska files (*.mkv)|*.mkv";
      this.openFileDialogMkv.Multiselect = true;
      this.openFileDialogMkv.Title = "Select One or More MKV Files";
      // 
      // textBoxNumFilesSelected
      // 
      this.textBoxNumFilesSelected.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.textBoxNumFilesSelected.BorderStyle = System.Windows.Forms.BorderStyle.None;
      this.textBoxNumFilesSelected.Location = new System.Drawing.Point(419, 79);
      this.textBoxNumFilesSelected.Name = "textBoxNumFilesSelected";
      this.textBoxNumFilesSelected.ReadOnly = true;
      this.textBoxNumFilesSelected.Size = new System.Drawing.Size(125, 13);
      this.textBoxNumFilesSelected.TabIndex = 15;
      this.textBoxNumFilesSelected.Text = "24 files selected";
      this.textBoxNumFilesSelected.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
      // 
      // errorProviderMain
      // 
      this.errorProviderMain.ContainerControl = this;
      // 
      // progressBarEpisode
      // 
      this.progressBarEpisode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.progressBarEpisode.Location = new System.Drawing.Point(12, 200);
      this.progressBarEpisode.Name = "progressBarEpisode";
      this.progressBarEpisode.Size = new System.Drawing.Size(176, 23);
      this.progressBarEpisode.Step = 1;
      this.progressBarEpisode.TabIndex = 117;
      this.progressBarEpisode.Visible = false;
      // 
      // labelProgress
      // 
      this.labelProgress.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.labelProgress.AutoSize = true;
      this.labelProgress.Location = new System.Drawing.Point(9, 184);
      this.labelProgress.Name = "labelProgress";
      this.labelProgress.Size = new System.Drawing.Size(181, 13);
      this.labelProgress.TabIndex = 13;
      this.labelProgress.Text = "Extracting track 2/3 from file 23/24...";
      this.labelProgress.Visible = false;
      // 
      // backgroundWorkerMain
      // 
      this.backgroundWorkerMain.WorkerReportsProgress = true;
      this.backgroundWorkerMain.WorkerSupportsCancellation = true;
      this.backgroundWorkerMain.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorkerMain_DoWork);
      this.backgroundWorkerMain.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorkerMain_RunWorkerCompleted);
      this.backgroundWorkerMain.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorkerMain_ProgressChanged);
      // 
      // progressBarTrack
      // 
      this.progressBarTrack.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.progressBarTrack.Location = new System.Drawing.Point(12, 224);
      this.progressBarTrack.Name = "progressBarTrack";
      this.progressBarTrack.Size = new System.Drawing.Size(176, 23);
      this.progressBarTrack.Step = 1;
      this.progressBarTrack.TabIndex = 117;
      this.progressBarTrack.Visible = false;
      // 
      // DialogMkvExtract
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.buttonCancel;
      this.ClientSize = new System.Drawing.Size(564, 259);
      this.Controls.Add(this.progressBarTrack);
      this.Controls.Add(this.progressBarEpisode);
      this.Controls.Add(this.comboBoxTrackTypeToExtract);
      this.Controls.Add(this.textBoxHelp);
      this.Controls.Add(this.labelOutDir);
      this.Controls.Add(this.buttonOutDir);
      this.Controls.Add(this.textBoxOutDir);
      this.Controls.Add(this.buttonCancel);
      this.Controls.Add(this.buttonExtract);
      this.Controls.Add(this.labelProgress);
      this.Controls.Add(this.labelTrackTypeToExtract);
      this.Controls.Add(this.labelMkvFiles);
      this.Controls.Add(this.buttonMkvFiles);
      this.Controls.Add(this.textBoxNumFilesSelected);
      this.Controls.Add(this.textBoxMkvFiles);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "DialogMkvExtract";
      this.Text = "MKV Extract Tool";
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DialogMkvExtract_FormClosing);
      ((System.ComponentModel.ISupportInitialize)(this.errorProviderMain)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Label labelOutDir;
    private System.Windows.Forms.Button buttonOutDir;
    private System.Windows.Forms.TextBox textBoxOutDir;
    private System.Windows.Forms.Button buttonCancel;
    private System.Windows.Forms.Button buttonExtract;
    private System.Windows.Forms.Label labelMkvFiles;
    private System.Windows.Forms.Button buttonMkvFiles;
    private System.Windows.Forms.TextBox textBoxMkvFiles;
    private System.Windows.Forms.TextBox textBoxHelp;
    private System.Windows.Forms.ComboBox comboBoxTrackTypeToExtract;
    private System.Windows.Forms.Label labelTrackTypeToExtract;
    private System.Windows.Forms.OpenFileDialog openFileDialogMkv;
    private System.Windows.Forms.FolderBrowserDialog folderBrowserDialogOut;
    private System.Windows.Forms.TextBox textBoxNumFilesSelected;
    private System.Windows.Forms.ErrorProvider errorProviderMain;
    private System.Windows.Forms.ProgressBar progressBarEpisode;
    private System.Windows.Forms.Label labelProgress;
    private System.ComponentModel.BackgroundWorker backgroundWorkerMain;
    private System.Windows.Forms.ProgressBar progressBarTrack;
  }
}