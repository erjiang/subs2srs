namespace subs2srs
{
  partial class DialogPreview
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
      System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem(" ");
      System.Windows.Forms.ListViewItem listViewItem2 = new System.Windows.Forms.ListViewItem(" ");
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DialogPreview));
      this.labelNote = new System.Windows.Forms.Label();
      this.buttonCancel = new System.Windows.Forms.Button();
      this.listViewLines = new System.Windows.Forms.ListView();
      this.columnHeaderSubs1 = new System.Windows.Forms.ColumnHeader();
      this.columnHeaderSubs2 = new System.Windows.Forms.ColumnHeader();
      this.columnHeaderStartTime = new System.Windows.Forms.ColumnHeader();
      this.columnHeaderEndTime = new System.Windows.Forms.ColumnHeader();
      this.buttonPreviewAudio = new System.Windows.Forms.Button();
      this.buttonRegenerate = new System.Windows.Forms.Button();
      this.pictureBoxImage = new System.Windows.Forms.PictureBox();
      this.pictureBoxSubs1 = new System.Windows.Forms.PictureBox();
      this.pictureBoxSubs2 = new System.Windows.Forms.PictureBox();
      this.textBoxSubs1 = new System.Windows.Forms.TextBox();
      this.textBoxSubs2 = new System.Windows.Forms.TextBox();
      this.labelSubs2 = new System.Windows.Forms.Label();
      this.labelSubs1 = new System.Windows.Forms.Label();
      this.panelSubs1 = new System.Windows.Forms.Panel();
      this.panelSubs2 = new System.Windows.Forms.Panel();
      this.comboBoxEpisode = new System.Windows.Forms.ComboBox();
      this.labelEpisode = new System.Windows.Forms.Label();
      this.labelStatsLinesInCurrentEpisode = new System.Windows.Forms.Label();
      this.labelStatsActiveLinesInCurrentEpisode = new System.Windows.Forms.Label();
      this.labelStatsTotalActiveLines = new System.Windows.Forms.Label();
      this.labelStatsTotalLines = new System.Windows.Forms.Label();
      this.labelStatsActiveLinesInCurrentEpisodeNum = new System.Windows.Forms.Label();
      this.labelStatsLinesInCurrentEpisodeNum = new System.Windows.Forms.Label();
      this.labelStatsTotalActiveLinesNum = new System.Windows.Forms.Label();
      this.labelStatsTotalLinesNum = new System.Windows.Forms.Label();
      this.panelStats = new System.Windows.Forms.Panel();
      this.labelStatsTotalInactiveLinesNum = new System.Windows.Forms.Label();
      this.labelStatsInactiveLinesInCurrentEpisode = new System.Windows.Forms.Label();
      this.labelStatsInactiveLinesInCurrentEpisodeNum = new System.Windows.Forms.Label();
      this.labelStatsTotalInactiveLines = new System.Windows.Forms.Label();
      this.buttonSelectAll = new System.Windows.Forms.Button();
      this.buttonSelectNone = new System.Windows.Forms.Button();
      this.buttonInvert = new System.Windows.Forms.Button();
      this.buttonActivate = new System.Windows.Forms.Button();
      this.buttonDeactivate = new System.Windows.Forms.Button();
      this.buttonGo = new System.Windows.Forms.Button();
      this.checkBoxSnapshotPreview = new System.Windows.Forms.CheckBox();
      this.textBoxFind = new System.Windows.Forms.TextBox();
      this.buttonFind = new System.Windows.Forms.Button();
      this.buttonPreviewVideo = new System.Windows.Forms.Button();
      this.backgroundWorkerAudio = new System.ComponentModel.BackgroundWorker();
      this.textBoxTimings = new System.Windows.Forms.TextBox();
      this.columnHeaderDuration = new System.Windows.Forms.ColumnHeader();
      ((System.ComponentModel.ISupportInitialize)(this.pictureBoxImage)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSubs1)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSubs2)).BeginInit();
      this.panelSubs1.SuspendLayout();
      this.panelSubs2.SuspendLayout();
      this.panelStats.SuspendLayout();
      this.SuspendLayout();
      // 
      // labelNote
      // 
      this.labelNote.AutoSize = true;
      this.labelNote.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.labelNote.Location = new System.Drawing.Point(9, 5);
      this.labelNote.Name = "labelNote";
      this.labelNote.Size = new System.Drawing.Size(381, 13);
      this.labelNote.TabIndex = 0;
      this.labelNote.Text = "Click \"Regenerate Preview\" to update the preview based on the latest settings.";
      // 
      // buttonCancel
      // 
      this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.buttonCancel.Location = new System.Drawing.Point(711, 449);
      this.buttonCancel.Name = "buttonCancel";
      this.buttonCancel.Size = new System.Drawing.Size(75, 23);
      this.buttonCancel.TabIndex = 15;
      this.buttonCancel.Text = "&Cancel";
      this.buttonCancel.UseVisualStyleBackColor = true;
      this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
      // 
      // listViewLines
      // 
      this.listViewLines.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.listViewLines.BackColor = System.Drawing.Color.White;
      this.listViewLines.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderSubs1,
            this.columnHeaderSubs2,
            this.columnHeaderStartTime,
            this.columnHeaderEndTime,
            this.columnHeaderDuration});
      this.listViewLines.FullRowSelect = true;
      this.listViewLines.GridLines = true;
      this.listViewLines.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
      this.listViewLines.HideSelection = false;
      this.listViewLines.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1,
            listViewItem2});
      this.listViewLines.Location = new System.Drawing.Point(12, 61);
      this.listViewLines.Name = "listViewLines";
      this.listViewLines.ShowGroups = false;
      this.listViewLines.Size = new System.Drawing.Size(693, 186);
      this.listViewLines.TabIndex = 3;
      this.listViewLines.UseCompatibleStateImageBehavior = false;
      this.listViewLines.View = System.Windows.Forms.View.Details;
      this.listViewLines.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.listViewLines_ItemSelectionChanged);
      this.listViewLines.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listViewLines_KeyDown);
      // 
      // columnHeaderSubs1
      // 
      this.columnHeaderSubs1.Text = "Subs1";
      this.columnHeaderSubs1.Width = 336;
      // 
      // columnHeaderSubs2
      // 
      this.columnHeaderSubs2.Text = "Subs2";
      this.columnHeaderSubs2.Width = 322;
      // 
      // columnHeaderStartTime
      // 
      this.columnHeaderStartTime.Text = "Start Time";
      this.columnHeaderStartTime.Width = 80;
      // 
      // columnHeaderEndTime
      // 
      this.columnHeaderEndTime.Text = "End Time";
      this.columnHeaderEndTime.Width = 80;
      // 
      // buttonPreviewAudio
      // 
      this.buttonPreviewAudio.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.buttonPreviewAudio.Location = new System.Drawing.Point(12, 449);
      this.buttonPreviewAudio.Name = "buttonPreviewAudio";
      this.buttonPreviewAudio.Size = new System.Drawing.Size(103, 23);
      this.buttonPreviewAudio.TabIndex = 10;
      this.buttonPreviewAudio.Text = "Preview &Audio";
      this.buttonPreviewAudio.UseVisualStyleBackColor = true;
      this.buttonPreviewAudio.Click += new System.EventHandler(this.buttonPreviewAudio_Click);
      // 
      // buttonRegenerate
      // 
      this.buttonRegenerate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.buttonRegenerate.Location = new System.Drawing.Point(274, 449);
      this.buttonRegenerate.Name = "buttonRegenerate";
      this.buttonRegenerate.Size = new System.Drawing.Size(141, 23);
      this.buttonRegenerate.TabIndex = 13;
      this.buttonRegenerate.Text = "&Regenerate Preview";
      this.buttonRegenerate.UseVisualStyleBackColor = true;
      this.buttonRegenerate.Click += new System.EventHandler(this.buttonRegenerate_Click);
      // 
      // pictureBoxImage
      // 
      this.pictureBoxImage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.pictureBoxImage.BackColor = System.Drawing.Color.DimGray;
      this.pictureBoxImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.pictureBoxImage.Location = new System.Drawing.Point(12, 276);
      this.pictureBoxImage.Name = "pictureBoxImage";
      this.pictureBoxImage.Size = new System.Drawing.Size(250, 167);
      this.pictureBoxImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
      this.pictureBoxImage.TabIndex = 6;
      this.pictureBoxImage.TabStop = false;
      this.pictureBoxImage.Click += new System.EventHandler(this.pictureBoxImage_Click);
      // 
      // pictureBoxSubs1
      // 
      this.pictureBoxSubs1.BackColor = System.Drawing.Color.MintCream;
      this.pictureBoxSubs1.Location = new System.Drawing.Point(3, 3);
      this.pictureBoxSubs1.Name = "pictureBoxSubs1";
      this.pictureBoxSubs1.Size = new System.Drawing.Size(348, 20);
      this.pictureBoxSubs1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
      this.pictureBoxSubs1.TabIndex = 48;
      this.pictureBoxSubs1.TabStop = false;
      this.pictureBoxSubs1.Visible = false;
      // 
      // pictureBoxSubs2
      // 
      this.pictureBoxSubs2.BackColor = System.Drawing.Color.MintCream;
      this.pictureBoxSubs2.Location = new System.Drawing.Point(3, 3);
      this.pictureBoxSubs2.Name = "pictureBoxSubs2";
      this.pictureBoxSubs2.Size = new System.Drawing.Size(348, 20);
      this.pictureBoxSubs2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
      this.pictureBoxSubs2.TabIndex = 49;
      this.pictureBoxSubs2.TabStop = false;
      this.pictureBoxSubs2.Visible = false;
      // 
      // textBoxSubs1
      // 
      this.textBoxSubs1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.textBoxSubs1.BackColor = System.Drawing.Color.MintCream;
      this.textBoxSubs1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.textBoxSubs1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.textBoxSubs1.Location = new System.Drawing.Point(274, 276);
      this.textBoxSubs1.Multiline = true;
      this.textBoxSubs1.Name = "textBoxSubs1";
      this.textBoxSubs1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
      this.textBoxSubs1.Size = new System.Drawing.Size(512, 73);
      this.textBoxSubs1.TabIndex = 11;
      this.textBoxSubs1.TextChanged += new System.EventHandler(this.textBoxSubs1_TextChanged);
      // 
      // textBoxSubs2
      // 
      this.textBoxSubs2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.textBoxSubs2.BackColor = System.Drawing.Color.MintCream;
      this.textBoxSubs2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.textBoxSubs2.Location = new System.Drawing.Point(274, 370);
      this.textBoxSubs2.Multiline = true;
      this.textBoxSubs2.Name = "textBoxSubs2";
      this.textBoxSubs2.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
      this.textBoxSubs2.Size = new System.Drawing.Size(512, 73);
      this.textBoxSubs2.TabIndex = 12;
      this.textBoxSubs2.TextChanged += new System.EventHandler(this.textBoxSubs2_TextChanged);
      // 
      // labelSubs2
      // 
      this.labelSubs2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.labelSubs2.AutoSize = true;
      this.labelSubs2.Location = new System.Drawing.Point(271, 354);
      this.labelSubs2.Name = "labelSubs2";
      this.labelSubs2.Size = new System.Drawing.Size(40, 13);
      this.labelSubs2.TabIndex = 53;
      this.labelSubs2.Text = "Subs2:";
      // 
      // labelSubs1
      // 
      this.labelSubs1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.labelSubs1.AutoSize = true;
      this.labelSubs1.Location = new System.Drawing.Point(271, 260);
      this.labelSubs1.Name = "labelSubs1";
      this.labelSubs1.Size = new System.Drawing.Size(40, 13);
      this.labelSubs1.TabIndex = 54;
      this.labelSubs1.Text = "Subs1:";
      // 
      // panelSubs1
      // 
      this.panelSubs1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.panelSubs1.AutoScroll = true;
      this.panelSubs1.BackColor = System.Drawing.Color.MintCream;
      this.panelSubs1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.panelSubs1.Controls.Add(this.pictureBoxSubs1);
      this.panelSubs1.Location = new System.Drawing.Point(274, 276);
      this.panelSubs1.Name = "panelSubs1";
      this.panelSubs1.Size = new System.Drawing.Size(512, 73);
      this.panelSubs1.TabIndex = 56;
      // 
      // panelSubs2
      // 
      this.panelSubs2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.panelSubs2.AutoScroll = true;
      this.panelSubs2.BackColor = System.Drawing.Color.MintCream;
      this.panelSubs2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.panelSubs2.Controls.Add(this.pictureBoxSubs2);
      this.panelSubs2.Location = new System.Drawing.Point(274, 370);
      this.panelSubs2.Name = "panelSubs2";
      this.panelSubs2.Size = new System.Drawing.Size(512, 73);
      this.panelSubs2.TabIndex = 57;
      // 
      // comboBoxEpisode
      // 
      this.comboBoxEpisode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboBoxEpisode.FormattingEnabled = true;
      this.comboBoxEpisode.Location = new System.Drawing.Point(60, 29);
      this.comboBoxEpisode.Name = "comboBoxEpisode";
      this.comboBoxEpisode.Size = new System.Drawing.Size(68, 21);
      this.comboBoxEpisode.TabIndex = 0;
      this.comboBoxEpisode.SelectedIndexChanged += new System.EventHandler(this.comboBoxEpisode_SelectedIndexChanged);
      // 
      // labelEpisode
      // 
      this.labelEpisode.AutoSize = true;
      this.labelEpisode.Location = new System.Drawing.Point(9, 33);
      this.labelEpisode.Name = "labelEpisode";
      this.labelEpisode.Size = new System.Drawing.Size(48, 13);
      this.labelEpisode.TabIndex = 59;
      this.labelEpisode.Text = "Episode:";
      // 
      // labelStatsLinesInCurrentEpisode
      // 
      this.labelStatsLinesInCurrentEpisode.AutoSize = true;
      this.labelStatsLinesInCurrentEpisode.Location = new System.Drawing.Point(2, 19);
      this.labelStatsLinesInCurrentEpisode.Name = "labelStatsLinesInCurrentEpisode";
      this.labelStatsLinesInCurrentEpisode.Size = new System.Drawing.Size(71, 13);
      this.labelStatsLinesInCurrentEpisode.TabIndex = 61;
      this.labelStatsLinesInCurrentEpisode.Text = "This Episode:";
      // 
      // labelStatsActiveLinesInCurrentEpisode
      // 
      this.labelStatsActiveLinesInCurrentEpisode.AutoSize = true;
      this.labelStatsActiveLinesInCurrentEpisode.ForeColor = System.Drawing.Color.Green;
      this.labelStatsActiveLinesInCurrentEpisode.Location = new System.Drawing.Point(115, 19);
      this.labelStatsActiveLinesInCurrentEpisode.Name = "labelStatsActiveLinesInCurrentEpisode";
      this.labelStatsActiveLinesInCurrentEpisode.Size = new System.Drawing.Size(40, 13);
      this.labelStatsActiveLinesInCurrentEpisode.TabIndex = 62;
      this.labelStatsActiveLinesInCurrentEpisode.Text = "Active:";
      // 
      // labelStatsTotalActiveLines
      // 
      this.labelStatsTotalActiveLines.AutoSize = true;
      this.labelStatsTotalActiveLines.ForeColor = System.Drawing.Color.Green;
      this.labelStatsTotalActiveLines.Location = new System.Drawing.Point(115, 2);
      this.labelStatsTotalActiveLines.Name = "labelStatsTotalActiveLines";
      this.labelStatsTotalActiveLines.Size = new System.Drawing.Size(40, 13);
      this.labelStatsTotalActiveLines.TabIndex = 64;
      this.labelStatsTotalActiveLines.Text = "Active:";
      // 
      // labelStatsTotalLines
      // 
      this.labelStatsTotalLines.AutoSize = true;
      this.labelStatsTotalLines.Location = new System.Drawing.Point(2, 2);
      this.labelStatsTotalLines.Name = "labelStatsTotalLines";
      this.labelStatsTotalLines.Size = new System.Drawing.Size(67, 13);
      this.labelStatsTotalLines.TabIndex = 63;
      this.labelStatsTotalLines.Text = "All Episodes:";
      // 
      // labelStatsActiveLinesInCurrentEpisodeNum
      // 
      this.labelStatsActiveLinesInCurrentEpisodeNum.AutoSize = true;
      this.labelStatsActiveLinesInCurrentEpisodeNum.ForeColor = System.Drawing.Color.Green;
      this.labelStatsActiveLinesInCurrentEpisodeNum.Location = new System.Drawing.Point(156, 19);
      this.labelStatsActiveLinesInCurrentEpisodeNum.Name = "labelStatsActiveLinesInCurrentEpisodeNum";
      this.labelStatsActiveLinesInCurrentEpisodeNum.Size = new System.Drawing.Size(13, 13);
      this.labelStatsActiveLinesInCurrentEpisodeNum.TabIndex = 66;
      this.labelStatsActiveLinesInCurrentEpisodeNum.Text = "0";
      // 
      // labelStatsLinesInCurrentEpisodeNum
      // 
      this.labelStatsLinesInCurrentEpisodeNum.AutoSize = true;
      this.labelStatsLinesInCurrentEpisodeNum.Location = new System.Drawing.Point(74, 19);
      this.labelStatsLinesInCurrentEpisodeNum.Name = "labelStatsLinesInCurrentEpisodeNum";
      this.labelStatsLinesInCurrentEpisodeNum.Size = new System.Drawing.Size(13, 13);
      this.labelStatsLinesInCurrentEpisodeNum.TabIndex = 65;
      this.labelStatsLinesInCurrentEpisodeNum.Text = "0";
      // 
      // labelStatsTotalActiveLinesNum
      // 
      this.labelStatsTotalActiveLinesNum.AutoSize = true;
      this.labelStatsTotalActiveLinesNum.ForeColor = System.Drawing.Color.Green;
      this.labelStatsTotalActiveLinesNum.Location = new System.Drawing.Point(156, 2);
      this.labelStatsTotalActiveLinesNum.Name = "labelStatsTotalActiveLinesNum";
      this.labelStatsTotalActiveLinesNum.Size = new System.Drawing.Size(13, 13);
      this.labelStatsTotalActiveLinesNum.TabIndex = 68;
      this.labelStatsTotalActiveLinesNum.Text = "0";
      // 
      // labelStatsTotalLinesNum
      // 
      this.labelStatsTotalLinesNum.AutoSize = true;
      this.labelStatsTotalLinesNum.Location = new System.Drawing.Point(74, 2);
      this.labelStatsTotalLinesNum.Name = "labelStatsTotalLinesNum";
      this.labelStatsTotalLinesNum.Size = new System.Drawing.Size(13, 13);
      this.labelStatsTotalLinesNum.TabIndex = 67;
      this.labelStatsTotalLinesNum.Text = "0";
      // 
      // panelStats
      // 
      this.panelStats.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.panelStats.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.panelStats.Controls.Add(this.labelStatsTotalInactiveLinesNum);
      this.panelStats.Controls.Add(this.labelStatsInactiveLinesInCurrentEpisode);
      this.panelStats.Controls.Add(this.labelStatsInactiveLinesInCurrentEpisodeNum);
      this.panelStats.Controls.Add(this.labelStatsTotalInactiveLines);
      this.panelStats.Controls.Add(this.labelStatsLinesInCurrentEpisode);
      this.panelStats.Controls.Add(this.labelStatsTotalActiveLinesNum);
      this.panelStats.Controls.Add(this.labelStatsActiveLinesInCurrentEpisode);
      this.panelStats.Controls.Add(this.labelStatsTotalLinesNum);
      this.panelStats.Controls.Add(this.labelStatsTotalLines);
      this.panelStats.Controls.Add(this.labelStatsActiveLinesInCurrentEpisodeNum);
      this.panelStats.Controls.Add(this.labelStatsTotalActiveLines);
      this.panelStats.Controls.Add(this.labelStatsLinesInCurrentEpisodeNum);
      this.panelStats.Location = new System.Drawing.Point(478, 19);
      this.panelStats.Name = "panelStats";
      this.panelStats.Size = new System.Drawing.Size(308, 36);
      this.panelStats.TabIndex = 69;
      // 
      // labelStatsTotalInactiveLinesNum
      // 
      this.labelStatsTotalInactiveLinesNum.AutoSize = true;
      this.labelStatsTotalInactiveLinesNum.ForeColor = System.Drawing.Color.DarkRed;
      this.labelStatsTotalInactiveLinesNum.Location = new System.Drawing.Point(255, 2);
      this.labelStatsTotalInactiveLinesNum.Name = "labelStatsTotalInactiveLinesNum";
      this.labelStatsTotalInactiveLinesNum.Size = new System.Drawing.Size(13, 13);
      this.labelStatsTotalInactiveLinesNum.TabIndex = 72;
      this.labelStatsTotalInactiveLinesNum.Text = "0";
      // 
      // labelStatsInactiveLinesInCurrentEpisode
      // 
      this.labelStatsInactiveLinesInCurrentEpisode.AutoSize = true;
      this.labelStatsInactiveLinesInCurrentEpisode.ForeColor = System.Drawing.Color.DarkRed;
      this.labelStatsInactiveLinesInCurrentEpisode.Location = new System.Drawing.Point(206, 19);
      this.labelStatsInactiveLinesInCurrentEpisode.Name = "labelStatsInactiveLinesInCurrentEpisode";
      this.labelStatsInactiveLinesInCurrentEpisode.Size = new System.Drawing.Size(48, 13);
      this.labelStatsInactiveLinesInCurrentEpisode.TabIndex = 69;
      this.labelStatsInactiveLinesInCurrentEpisode.Text = "Inactive:";
      // 
      // labelStatsInactiveLinesInCurrentEpisodeNum
      // 
      this.labelStatsInactiveLinesInCurrentEpisodeNum.AutoSize = true;
      this.labelStatsInactiveLinesInCurrentEpisodeNum.ForeColor = System.Drawing.Color.DarkRed;
      this.labelStatsInactiveLinesInCurrentEpisodeNum.Location = new System.Drawing.Point(255, 19);
      this.labelStatsInactiveLinesInCurrentEpisodeNum.Name = "labelStatsInactiveLinesInCurrentEpisodeNum";
      this.labelStatsInactiveLinesInCurrentEpisodeNum.Size = new System.Drawing.Size(13, 13);
      this.labelStatsInactiveLinesInCurrentEpisodeNum.TabIndex = 71;
      this.labelStatsInactiveLinesInCurrentEpisodeNum.Text = "0";
      // 
      // labelStatsTotalInactiveLines
      // 
      this.labelStatsTotalInactiveLines.AutoSize = true;
      this.labelStatsTotalInactiveLines.ForeColor = System.Drawing.Color.DarkRed;
      this.labelStatsTotalInactiveLines.Location = new System.Drawing.Point(206, 2);
      this.labelStatsTotalInactiveLines.Name = "labelStatsTotalInactiveLines";
      this.labelStatsTotalInactiveLines.Size = new System.Drawing.Size(48, 13);
      this.labelStatsTotalInactiveLines.TabIndex = 70;
      this.labelStatsTotalInactiveLines.Text = "Inactive:";
      // 
      // buttonSelectAll
      // 
      this.buttonSelectAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonSelectAll.Location = new System.Drawing.Point(711, 61);
      this.buttonSelectAll.Name = "buttonSelectAll";
      this.buttonSelectAll.Size = new System.Drawing.Size(75, 23);
      this.buttonSelectAll.TabIndex = 4;
      this.buttonSelectAll.Text = "&Select All";
      this.buttonSelectAll.UseVisualStyleBackColor = true;
      this.buttonSelectAll.Click += new System.EventHandler(this.buttonSelectAll_Click);
      // 
      // buttonSelectNone
      // 
      this.buttonSelectNone.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonSelectNone.Location = new System.Drawing.Point(711, 90);
      this.buttonSelectNone.Name = "buttonSelectNone";
      this.buttonSelectNone.Size = new System.Drawing.Size(75, 23);
      this.buttonSelectNone.TabIndex = 5;
      this.buttonSelectNone.Text = "Select &None";
      this.buttonSelectNone.UseVisualStyleBackColor = true;
      this.buttonSelectNone.Click += new System.EventHandler(this.buttonSelectNone_Click);
      // 
      // buttonInvert
      // 
      this.buttonInvert.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonInvert.Location = new System.Drawing.Point(711, 119);
      this.buttonInvert.Name = "buttonInvert";
      this.buttonInvert.Size = new System.Drawing.Size(75, 23);
      this.buttonInvert.TabIndex = 6;
      this.buttonInvert.Text = "&Invert";
      this.buttonInvert.UseVisualStyleBackColor = true;
      this.buttonInvert.Click += new System.EventHandler(this.buttonInvert_Click);
      // 
      // buttonActivate
      // 
      this.buttonActivate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonActivate.ForeColor = System.Drawing.Color.Green;
      this.buttonActivate.Location = new System.Drawing.Point(711, 195);
      this.buttonActivate.Name = "buttonActivate";
      this.buttonActivate.Size = new System.Drawing.Size(75, 23);
      this.buttonActivate.TabIndex = 7;
      this.buttonActivate.Text = "A&ctivate";
      this.buttonActivate.UseVisualStyleBackColor = true;
      this.buttonActivate.Click += new System.EventHandler(this.buttonActivate_Click);
      // 
      // buttonDeactivate
      // 
      this.buttonDeactivate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonDeactivate.ForeColor = System.Drawing.Color.DarkRed;
      this.buttonDeactivate.Location = new System.Drawing.Point(711, 224);
      this.buttonDeactivate.Name = "buttonDeactivate";
      this.buttonDeactivate.Size = new System.Drawing.Size(75, 23);
      this.buttonDeactivate.TabIndex = 8;
      this.buttonDeactivate.Text = "&Deactivate";
      this.buttonDeactivate.UseVisualStyleBackColor = true;
      this.buttonDeactivate.Click += new System.EventHandler(this.buttonDeactivate_Click);
      // 
      // buttonGo
      // 
      this.buttonGo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonGo.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.buttonGo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.buttonGo.ForeColor = System.Drawing.Color.DarkGreen;
      this.buttonGo.Location = new System.Drawing.Point(630, 449);
      this.buttonGo.Name = "buttonGo";
      this.buttonGo.Size = new System.Drawing.Size(75, 23);
      this.buttonGo.TabIndex = 14;
      this.buttonGo.Text = "&Go!";
      this.buttonGo.UseVisualStyleBackColor = true;
      this.buttonGo.Click += new System.EventHandler(this.buttonGo_Click);
      // 
      // checkBoxSnapshotPreview
      // 
      this.checkBoxSnapshotPreview.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.checkBoxSnapshotPreview.AutoSize = true;
      this.checkBoxSnapshotPreview.Checked = true;
      this.checkBoxSnapshotPreview.CheckState = System.Windows.Forms.CheckState.Checked;
      this.checkBoxSnapshotPreview.Location = new System.Drawing.Point(12, 259);
      this.checkBoxSnapshotPreview.Name = "checkBoxSnapshotPreview";
      this.checkBoxSnapshotPreview.Size = new System.Drawing.Size(230, 17);
      this.checkBoxSnapshotPreview.TabIndex = 9;
      this.checkBoxSnapshotPreview.Text = "Snapshot preview (click to see actual size):";
      this.checkBoxSnapshotPreview.UseVisualStyleBackColor = true;
      // 
      // textBoxFind
      // 
      this.textBoxFind.Location = new System.Drawing.Point(156, 30);
      this.textBoxFind.Name = "textBoxFind";
      this.textBoxFind.Size = new System.Drawing.Size(241, 20);
      this.textBoxFind.TabIndex = 1;
      this.textBoxFind.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxFind_KeyDown);
      this.textBoxFind.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxFind_KeyPress);
      // 
      // buttonFind
      // 
      this.buttonFind.Location = new System.Drawing.Point(399, 29);
      this.buttonFind.Name = "buttonFind";
      this.buttonFind.Size = new System.Drawing.Size(54, 22);
      this.buttonFind.TabIndex = 2;
      this.buttonFind.Text = "&Find";
      this.buttonFind.UseVisualStyleBackColor = true;
      this.buttonFind.Click += new System.EventHandler(this.buttonFind_Click);
      // 
      // buttonPreviewVideo
      // 
      this.buttonPreviewVideo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.buttonPreviewVideo.Location = new System.Drawing.Point(159, 449);
      this.buttonPreviewVideo.Name = "buttonPreviewVideo";
      this.buttonPreviewVideo.Size = new System.Drawing.Size(103, 23);
      this.buttonPreviewVideo.TabIndex = 70;
      this.buttonPreviewVideo.Text = "Preview &Video";
      this.buttonPreviewVideo.UseVisualStyleBackColor = true;
      this.buttonPreviewVideo.Click += new System.EventHandler(this.buttonPreviewVideo_Click);
      // 
      // backgroundWorkerAudio
      // 
      this.backgroundWorkerAudio.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorkerAudio_DoWork);
      this.backgroundWorkerAudio.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorkerAudio_RunWorkerCompleted);
      // 
      // textBoxTimings
      // 
      this.textBoxTimings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.textBoxTimings.BorderStyle = System.Windows.Forms.BorderStyle.None;
      this.textBoxTimings.Location = new System.Drawing.Point(620, 261);
      this.textBoxTimings.Name = "textBoxTimings";
      this.textBoxTimings.ReadOnly = true;
      this.textBoxTimings.Size = new System.Drawing.Size(166, 13);
      this.textBoxTimings.TabIndex = 71;
      this.textBoxTimings.Text = "0:00:00.000  -  0:00:00.000";
      this.textBoxTimings.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
      // 
      // columnHeaderDuration
      // 
      this.columnHeaderDuration.Text = "Duration";
      this.columnHeaderDuration.Width = 120;
      // 
      // DialogPreview
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.buttonCancel;
      this.ClientSize = new System.Drawing.Size(795, 486);
      this.Controls.Add(this.textBoxTimings);
      this.Controls.Add(this.buttonPreviewVideo);
      this.Controls.Add(this.buttonFind);
      this.Controls.Add(this.textBoxFind);
      this.Controls.Add(this.checkBoxSnapshotPreview);
      this.Controls.Add(this.buttonGo);
      this.Controls.Add(this.buttonDeactivate);
      this.Controls.Add(this.buttonActivate);
      this.Controls.Add(this.buttonInvert);
      this.Controls.Add(this.buttonSelectNone);
      this.Controls.Add(this.buttonSelectAll);
      this.Controls.Add(this.panelStats);
      this.Controls.Add(this.labelEpisode);
      this.Controls.Add(this.comboBoxEpisode);
      this.Controls.Add(this.textBoxSubs2);
      this.Controls.Add(this.textBoxSubs1);
      this.Controls.Add(this.panelSubs2);
      this.Controls.Add(this.panelSubs1);
      this.Controls.Add(this.labelNote);
      this.Controls.Add(this.labelSubs1);
      this.Controls.Add(this.labelSubs2);
      this.Controls.Add(this.listViewLines);
      this.Controls.Add(this.buttonRegenerate);
      this.Controls.Add(this.buttonPreviewAudio);
      this.Controls.Add(this.pictureBoxImage);
      this.Controls.Add(this.buttonCancel);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.MinimumSize = new System.Drawing.Size(782, 478);
      this.Name = "DialogPreview";
      this.Text = "Preview";
      this.Load += new System.EventHandler(this.DialogPreview_Load);
      this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.DialogPreview_FormClosed);
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DialogPreview_FormClosing);
      ((System.ComponentModel.ISupportInitialize)(this.pictureBoxImage)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSubs1)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSubs2)).EndInit();
      this.panelSubs1.ResumeLayout(false);
      this.panelSubs1.PerformLayout();
      this.panelSubs2.ResumeLayout(false);
      this.panelSubs2.PerformLayout();
      this.panelStats.ResumeLayout(false);
      this.panelStats.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Label labelNote;
    private System.Windows.Forms.Button buttonCancel;
    private System.Windows.Forms.ListView listViewLines;
    private System.Windows.Forms.PictureBox pictureBoxImage;
    private System.Windows.Forms.Button buttonPreviewAudio;
    private System.Windows.Forms.Button buttonRegenerate;
    private System.Windows.Forms.ColumnHeader columnHeaderSubs1;
    private System.Windows.Forms.ColumnHeader columnHeaderSubs2;
    private System.Windows.Forms.PictureBox pictureBoxSubs1;
    private System.Windows.Forms.PictureBox pictureBoxSubs2;
    private System.Windows.Forms.TextBox textBoxSubs1;
    private System.Windows.Forms.TextBox textBoxSubs2;
    private System.Windows.Forms.Label labelSubs2;
    private System.Windows.Forms.Label labelSubs1;
    private System.Windows.Forms.Panel panelSubs1;
    private System.Windows.Forms.Panel panelSubs2;
    private System.Windows.Forms.ComboBox comboBoxEpisode;
    private System.Windows.Forms.Label labelEpisode;
    private System.Windows.Forms.Label labelStatsLinesInCurrentEpisode;
    private System.Windows.Forms.Label labelStatsActiveLinesInCurrentEpisode;
    private System.Windows.Forms.Label labelStatsTotalActiveLines;
    private System.Windows.Forms.Label labelStatsTotalLines;
    private System.Windows.Forms.Label labelStatsActiveLinesInCurrentEpisodeNum;
    private System.Windows.Forms.Label labelStatsLinesInCurrentEpisodeNum;
    private System.Windows.Forms.Label labelStatsTotalActiveLinesNum;
    private System.Windows.Forms.Label labelStatsTotalLinesNum;
    private System.Windows.Forms.Panel panelStats;
    private System.Windows.Forms.Button buttonSelectAll;
    private System.Windows.Forms.Button buttonSelectNone;
    private System.Windows.Forms.Button buttonInvert;
    private System.Windows.Forms.Button buttonActivate;
    private System.Windows.Forms.Button buttonDeactivate;
    private System.Windows.Forms.Button buttonGo;
    private System.Windows.Forms.CheckBox checkBoxSnapshotPreview;
    private System.Windows.Forms.Label labelStatsTotalInactiveLinesNum;
    private System.Windows.Forms.Label labelStatsInactiveLinesInCurrentEpisode;
    private System.Windows.Forms.Label labelStatsInactiveLinesInCurrentEpisodeNum;
    private System.Windows.Forms.Label labelStatsTotalInactiveLines;
    private System.Windows.Forms.TextBox textBoxFind;
    private System.Windows.Forms.Button buttonFind;
    private System.Windows.Forms.Button buttonPreviewVideo;
    private System.ComponentModel.BackgroundWorker backgroundWorkerAudio;
    private System.Windows.Forms.ColumnHeader columnHeaderStartTime;
    private System.Windows.Forms.ColumnHeader columnHeaderEndTime;
    private System.Windows.Forms.TextBox textBoxTimings;
    private System.Windows.Forms.ColumnHeader columnHeaderDuration;
  }
}