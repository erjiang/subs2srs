namespace subs2srs
{
  partial class DialogExtractAudioFromMedia
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DialogExtractAudioFromMedia));
      this.labelMediaFile = new System.Windows.Forms.Label();
      this.buttonMediaFile = new System.Windows.Forms.Button();
      this.textBoxMediaFile = new System.Windows.Forms.TextBox();
      this.groupBoxOptions = new System.Windows.Forms.GroupBox();
      this.groupBoxCheckLyrics = new subs2srs.GroupBoxCheck();
      this.groupBoxRemoveLinesWithoutCounterpart = new System.Windows.Forms.GroupBox();
      this.checkBoxRemoveLinesWithoutCounterpartSubs2 = new System.Windows.Forms.CheckBox();
      this.checkBoxRemoveLinesWithoutCounterpartSubs1 = new System.Windows.Forms.CheckBox();
      this.label1RequiredSubs1 = new System.Windows.Forms.Label();
      this.groupBoxRemoveStyledLines = new System.Windows.Forms.GroupBox();
      this.checkBoxRemoveStyledLinesSubs2 = new System.Windows.Forms.CheckBox();
      this.checkBoxRemoveStyledLinesSubs1 = new System.Windows.Forms.CheckBox();
      this.comboBoxEncodingSubs1 = new System.Windows.Forms.ComboBox();
      this.labelOptionalSubs2 = new System.Windows.Forms.Label();
      this.groupBoxUseTimingsFrom = new System.Windows.Forms.GroupBox();
      this.radioButtonTimingSubs1 = new System.Windows.Forms.RadioButton();
      this.radioButtonTimingSubs2 = new System.Windows.Forms.RadioButton();
      this.labelEncodingSubs1 = new System.Windows.Forms.Label();
      this.comboBoxEncodingSubs2 = new System.Windows.Forms.ComboBox();
      this.labelEncodingSubs2 = new System.Windows.Forms.Label();
      this.groupBoxCheckTimeShift = new subs2srs.GroupBoxCheck();
      this.labelTimeShiftSubs2Units = new System.Windows.Forms.Label();
      this.numericUpDownTimeShiftSubs2 = new System.Windows.Forms.NumericUpDown();
      this.labelTimeShiftSubs2 = new System.Windows.Forms.Label();
      this.labelTimeShiftSubs1 = new System.Windows.Forms.Label();
      this.numericUpDownTimeShiftSubs1 = new System.Windows.Forms.NumericUpDown();
      this.labelTimeShiftSubs1Units = new System.Windows.Forms.Label();
      this.labelSubs2File = new System.Windows.Forms.Label();
      this.labelSubs1File = new System.Windows.Forms.Label();
      this.textBoxSubs1File = new System.Windows.Forms.TextBox();
      this.buttonSubs2File = new System.Windows.Forms.Button();
      this.buttonSubs1File = new System.Windows.Forms.Button();
      this.textBoxSubs2File = new System.Windows.Forms.TextBox();
      this.groupBoxCheckSpan = new subs2srs.GroupBoxCheck();
      this.labelSpanStart = new System.Windows.Forms.Label();
      this.maskedTextBoxSpanEnd = new System.Windows.Forms.MaskedTextBox();
      this.maskedTextBoxSpanStart = new System.Windows.Forms.MaskedTextBox();
      this.labelSpanEnd = new System.Windows.Forms.Label();
      this.groupBoxBitrate = new System.Windows.Forms.GroupBox();
      this.comboBoxBitrate = new System.Windows.Forms.ComboBox();
      this.labelBitrateUnits = new System.Windows.Forms.Label();
      this.groupBoxFormat = new System.Windows.Forms.GroupBox();
      this.radioButtonSingle = new System.Windows.Forms.RadioButton();
      this.maskedTextBoxClipLength = new System.Windows.Forms.MaskedTextBox();
      this.radioButtonMultiple = new System.Windows.Forms.RadioButton();
      this.buttonExtract = new System.Windows.Forms.Button();
      this.buttonCancel = new System.Windows.Forms.Button();
      this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
      this.buttonOutput = new System.Windows.Forms.Button();
      this.textBoxOutputDir = new System.Windows.Forms.TextBox();
      this.labelOutputDir = new System.Windows.Forms.Label();
      this.labelName = new System.Windows.Forms.Label();
      this.textBoxName = new System.Windows.Forms.TextBox();
      this.labelEpisodeStartNumber = new System.Windows.Forms.Label();
      this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
      this.labelAudioStream = new System.Windows.Forms.Label();
      this.comboBoxStreamAudioFromVideo = new System.Windows.Forms.ComboBox();
      this.numericUpDownEpisodeStartNumber = new System.Windows.Forms.NumericUpDown();
      this.groupBoxName = new System.Windows.Forms.GroupBox();
      this.labelRequiredName = new System.Windows.Forms.Label();
      this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
      this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
      this.labelRequiredMedia = new System.Windows.Forms.Label();
      this.labelRequiremedOutput = new System.Windows.Forms.Label();
      this.textBoxHelp = new System.Windows.Forms.TextBox();
      this.groupBoxOptions.SuspendLayout();
      this.groupBoxCheckLyrics.SuspendLayout();
      this.groupBoxRemoveLinesWithoutCounterpart.SuspendLayout();
      this.groupBoxRemoveStyledLines.SuspendLayout();
      this.groupBoxUseTimingsFrom.SuspendLayout();
      this.groupBoxCheckTimeShift.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownTimeShiftSubs2)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownTimeShiftSubs1)).BeginInit();
      this.groupBoxCheckSpan.SuspendLayout();
      this.groupBoxBitrate.SuspendLayout();
      this.groupBoxFormat.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownEpisodeStartNumber)).BeginInit();
      this.groupBoxName.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
      this.SuspendLayout();
      // 
      // labelMediaFile
      // 
      this.labelMediaFile.AutoSize = true;
      this.labelMediaFile.Location = new System.Drawing.Point(9, 37);
      this.labelMediaFile.Name = "labelMediaFile";
      this.labelMediaFile.Size = new System.Drawing.Size(127, 13);
      this.labelMediaFile.TabIndex = 1;
      this.labelMediaFile.Text = "The media file to convert:";
      this.toolTip1.SetToolTip(this.labelMediaFile, resources.GetString("labelMediaFile.ToolTip"));
      // 
      // buttonMediaFile
      // 
      this.buttonMediaFile.Location = new System.Drawing.Point(12, 52);
      this.buttonMediaFile.Name = "buttonMediaFile";
      this.buttonMediaFile.Size = new System.Drawing.Size(56, 22);
      this.buttonMediaFile.TabIndex = 2;
      this.buttonMediaFile.Text = "&Media...";
      this.toolTip1.SetToolTip(this.buttonMediaFile, "Select a media file");
      this.buttonMediaFile.UseVisualStyleBackColor = true;
      this.buttonMediaFile.Click += new System.EventHandler(this.buttonMediaFile_Click);
      // 
      // textBoxMediaFile
      // 
      this.textBoxMediaFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.textBoxMediaFile.Location = new System.Drawing.Point(73, 53);
      this.textBoxMediaFile.Name = "textBoxMediaFile";
      this.textBoxMediaFile.Size = new System.Drawing.Size(397, 20);
      this.textBoxMediaFile.TabIndex = 3;
      this.toolTip1.SetToolTip(this.textBoxMediaFile, resources.GetString("textBoxMediaFile.ToolTip"));
      this.textBoxMediaFile.TextChanged += new System.EventHandler(this.textBoxMediaFile_TextChanged);
      this.textBoxMediaFile.Validating += new System.ComponentModel.CancelEventHandler(this.textBoxMediaFile_Validating);
      // 
      // groupBoxOptions
      // 
      this.groupBoxOptions.Controls.Add(this.groupBoxCheckLyrics);
      this.groupBoxOptions.Controls.Add(this.groupBoxCheckSpan);
      this.groupBoxOptions.Controls.Add(this.groupBoxBitrate);
      this.groupBoxOptions.Controls.Add(this.groupBoxFormat);
      this.groupBoxOptions.Location = new System.Drawing.Point(12, 120);
      this.groupBoxOptions.Name = "groupBoxOptions";
      this.groupBoxOptions.Size = new System.Drawing.Size(589, 264);
      this.groupBoxOptions.TabIndex = 9;
      this.groupBoxOptions.TabStop = false;
      this.groupBoxOptions.Text = "Options:";
      // 
      // groupBoxCheckLyrics
      // 
      this.groupBoxCheckLyrics.Checked = false;
      this.groupBoxCheckLyrics.Controls.Add(this.groupBoxRemoveLinesWithoutCounterpart);
      this.groupBoxCheckLyrics.Controls.Add(this.label1RequiredSubs1);
      this.groupBoxCheckLyrics.Controls.Add(this.groupBoxRemoveStyledLines);
      this.groupBoxCheckLyrics.Controls.Add(this.comboBoxEncodingSubs1);
      this.groupBoxCheckLyrics.Controls.Add(this.labelOptionalSubs2);
      this.groupBoxCheckLyrics.Controls.Add(this.groupBoxUseTimingsFrom);
      this.groupBoxCheckLyrics.Controls.Add(this.labelEncodingSubs1);
      this.groupBoxCheckLyrics.Controls.Add(this.comboBoxEncodingSubs2);
      this.groupBoxCheckLyrics.Controls.Add(this.labelEncodingSubs2);
      this.groupBoxCheckLyrics.Controls.Add(this.groupBoxCheckTimeShift);
      this.groupBoxCheckLyrics.Controls.Add(this.labelSubs2File);
      this.groupBoxCheckLyrics.Controls.Add(this.labelSubs1File);
      this.groupBoxCheckLyrics.Controls.Add(this.textBoxSubs1File);
      this.groupBoxCheckLyrics.Controls.Add(this.buttonSubs2File);
      this.groupBoxCheckLyrics.Controls.Add(this.buttonSubs1File);
      this.groupBoxCheckLyrics.Controls.Add(this.textBoxSubs2File);
      this.groupBoxCheckLyrics.Location = new System.Drawing.Point(7, 90);
      this.groupBoxCheckLyrics.Name = "groupBoxCheckLyrics";
      this.groupBoxCheckLyrics.Padding = new System.Windows.Forms.Padding(0, 3, 0, 0);
      this.groupBoxCheckLyrics.Size = new System.Drawing.Size(575, 169);
      this.groupBoxCheckLyrics.TabIndex = 3;
      this.groupBoxCheckLyrics.TabStop = false;
      this.groupBoxCheckLyrics.Text = "Lyrics:";
      this.toolTip1.SetToolTip(this.groupBoxCheckLyrics, "Enable/Disable adding lyrics to each generated .mp3\'s ID3 Lyrics tag");
      // 
      // groupBoxRemoveLinesWithoutCounterpart
      // 
      this.groupBoxRemoveLinesWithoutCounterpart.Controls.Add(this.checkBoxRemoveLinesWithoutCounterpartSubs2);
      this.groupBoxRemoveLinesWithoutCounterpart.Controls.Add(this.checkBoxRemoveLinesWithoutCounterpartSubs1);
      this.groupBoxRemoveLinesWithoutCounterpart.Location = new System.Drawing.Point(259, 99);
      this.groupBoxRemoveLinesWithoutCounterpart.Name = "groupBoxRemoveLinesWithoutCounterpart";
      this.groupBoxRemoveLinesWithoutCounterpart.Size = new System.Drawing.Size(172, 65);
      this.groupBoxRemoveLinesWithoutCounterpart.TabIndex = 4;
      this.groupBoxRemoveLinesWithoutCounterpart.TabStop = false;
      this.groupBoxRemoveLinesWithoutCounterpart.Text = "Remove Lines w/o Counterpart:";
      this.toolTip1.SetToolTip(this.groupBoxRemoveLinesWithoutCounterpart, resources.GetString("groupBoxRemoveLinesWithoutCounterpart.ToolTip"));
      // 
      // checkBoxRemoveLinesWithoutCounterpartSubs2
      // 
      this.checkBoxRemoveLinesWithoutCounterpartSubs2.AutoSize = true;
      this.checkBoxRemoveLinesWithoutCounterpartSubs2.Checked = true;
      this.checkBoxRemoveLinesWithoutCounterpartSubs2.CheckState = System.Windows.Forms.CheckState.Checked;
      this.checkBoxRemoveLinesWithoutCounterpartSubs2.Location = new System.Drawing.Point(6, 42);
      this.checkBoxRemoveLinesWithoutCounterpartSubs2.Name = "checkBoxRemoveLinesWithoutCounterpartSubs2";
      this.checkBoxRemoveLinesWithoutCounterpartSubs2.Size = new System.Drawing.Size(56, 17);
      this.checkBoxRemoveLinesWithoutCounterpartSubs2.TabIndex = 1;
      this.checkBoxRemoveLinesWithoutCounterpartSubs2.Text = "Subs2";
      this.toolTip1.SetToolTip(this.checkBoxRemoveLinesWithoutCounterpartSubs2, "Remove lines from Subs2 that do not have a counterpart in Subs1");
      this.checkBoxRemoveLinesWithoutCounterpartSubs2.UseVisualStyleBackColor = true;
      // 
      // checkBoxRemoveLinesWithoutCounterpartSubs1
      // 
      this.checkBoxRemoveLinesWithoutCounterpartSubs1.AutoSize = true;
      this.checkBoxRemoveLinesWithoutCounterpartSubs1.Checked = true;
      this.checkBoxRemoveLinesWithoutCounterpartSubs1.CheckState = System.Windows.Forms.CheckState.Checked;
      this.checkBoxRemoveLinesWithoutCounterpartSubs1.Location = new System.Drawing.Point(6, 21);
      this.checkBoxRemoveLinesWithoutCounterpartSubs1.Name = "checkBoxRemoveLinesWithoutCounterpartSubs1";
      this.checkBoxRemoveLinesWithoutCounterpartSubs1.Size = new System.Drawing.Size(56, 17);
      this.checkBoxRemoveLinesWithoutCounterpartSubs1.TabIndex = 0;
      this.checkBoxRemoveLinesWithoutCounterpartSubs1.Text = "Subs1";
      this.toolTip1.SetToolTip(this.checkBoxRemoveLinesWithoutCounterpartSubs1, "Remove lines from Subs1 that do not have a counterpart in Subs2");
      this.checkBoxRemoveLinesWithoutCounterpartSubs1.UseVisualStyleBackColor = true;
      // 
      // label1RequiredSubs1
      // 
      this.label1RequiredSubs1.AutoSize = true;
      this.label1RequiredSubs1.ForeColor = System.Drawing.Color.Red;
      this.label1RequiredSubs1.Location = new System.Drawing.Point(180, 17);
      this.label1RequiredSubs1.Name = "label1RequiredSubs1";
      this.label1RequiredSubs1.Size = new System.Drawing.Size(51, 13);
      this.label1RequiredSubs1.TabIndex = 111;
      this.label1RequiredSubs1.Text = "(required)";
      // 
      // groupBoxRemoveStyledLines
      // 
      this.groupBoxRemoveStyledLines.Controls.Add(this.checkBoxRemoveStyledLinesSubs2);
      this.groupBoxRemoveStyledLines.Controls.Add(this.checkBoxRemoveStyledLinesSubs1);
      this.groupBoxRemoveStyledLines.Location = new System.Drawing.Point(436, 99);
      this.groupBoxRemoveStyledLines.Name = "groupBoxRemoveStyledLines";
      this.groupBoxRemoveStyledLines.Size = new System.Drawing.Size(126, 65);
      this.groupBoxRemoveStyledLines.TabIndex = 112;
      this.groupBoxRemoveStyledLines.TabStop = false;
      this.groupBoxRemoveStyledLines.Text = "Remove Styled Lines:";
      this.toolTip1.SetToolTip(this.groupBoxRemoveStyledLines, resources.GetString("groupBoxRemoveStyledLines.ToolTip"));
      // 
      // checkBoxRemoveStyledLinesSubs2
      // 
      this.checkBoxRemoveStyledLinesSubs2.AutoSize = true;
      this.checkBoxRemoveStyledLinesSubs2.Checked = true;
      this.checkBoxRemoveStyledLinesSubs2.CheckState = System.Windows.Forms.CheckState.Checked;
      this.checkBoxRemoveStyledLinesSubs2.Location = new System.Drawing.Point(6, 42);
      this.checkBoxRemoveStyledLinesSubs2.Name = "checkBoxRemoveStyledLinesSubs2";
      this.checkBoxRemoveStyledLinesSubs2.Size = new System.Drawing.Size(56, 17);
      this.checkBoxRemoveStyledLinesSubs2.TabIndex = 1;
      this.checkBoxRemoveStyledLinesSubs2.Text = "Subs2";
      this.toolTip1.SetToolTip(this.checkBoxRemoveStyledLinesSubs2, "Remove styled lines from Subs2");
      this.checkBoxRemoveStyledLinesSubs2.UseVisualStyleBackColor = true;
      // 
      // checkBoxRemoveStyledLinesSubs1
      // 
      this.checkBoxRemoveStyledLinesSubs1.AutoSize = true;
      this.checkBoxRemoveStyledLinesSubs1.Checked = true;
      this.checkBoxRemoveStyledLinesSubs1.CheckState = System.Windows.Forms.CheckState.Checked;
      this.checkBoxRemoveStyledLinesSubs1.Location = new System.Drawing.Point(6, 21);
      this.checkBoxRemoveStyledLinesSubs1.Name = "checkBoxRemoveStyledLinesSubs1";
      this.checkBoxRemoveStyledLinesSubs1.Size = new System.Drawing.Size(56, 17);
      this.checkBoxRemoveStyledLinesSubs1.TabIndex = 0;
      this.checkBoxRemoveStyledLinesSubs1.Text = "Subs1";
      this.toolTip1.SetToolTip(this.checkBoxRemoveStyledLinesSubs1, "Remove styled lines from Subs1");
      this.checkBoxRemoveStyledLinesSubs1.UseVisualStyleBackColor = true;
      // 
      // comboBoxEncodingSubs1
      // 
      this.comboBoxEncodingSubs1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboBoxEncodingSubs1.DropDownWidth = 250;
      this.comboBoxEncodingSubs1.FormattingEnabled = true;
      this.comboBoxEncodingSubs1.Location = new System.Drawing.Point(436, 32);
      this.comboBoxEncodingSubs1.Name = "comboBoxEncodingSubs1";
      this.comboBoxEncodingSubs1.Size = new System.Drawing.Size(131, 21);
      this.comboBoxEncodingSubs1.TabIndex = 2;
      // 
      // labelOptionalSubs2
      // 
      this.labelOptionalSubs2.AutoSize = true;
      this.labelOptionalSubs2.ForeColor = System.Drawing.Color.SeaGreen;
      this.labelOptionalSubs2.Location = new System.Drawing.Point(272, 57);
      this.labelOptionalSubs2.Name = "labelOptionalSubs2";
      this.labelOptionalSubs2.Size = new System.Drawing.Size(50, 13);
      this.labelOptionalSubs2.TabIndex = 111;
      this.labelOptionalSubs2.Text = "(optional)";
      // 
      // groupBoxUseTimingsFrom
      // 
      this.groupBoxUseTimingsFrom.Controls.Add(this.radioButtonTimingSubs1);
      this.groupBoxUseTimingsFrom.Controls.Add(this.radioButtonTimingSubs2);
      this.groupBoxUseTimingsFrom.Location = new System.Drawing.Point(9, 99);
      this.groupBoxUseTimingsFrom.Name = "groupBoxUseTimingsFrom";
      this.groupBoxUseTimingsFrom.Size = new System.Drawing.Size(110, 65);
      this.groupBoxUseTimingsFrom.TabIndex = 6;
      this.groupBoxUseTimingsFrom.TabStop = false;
      this.groupBoxUseTimingsFrom.Text = "Use Timings From:";
      this.toolTip1.SetToolTip(this.groupBoxUseTimingsFrom, "Use the timings from either Subs1 or Subs2. You \r\nshould use the timings from whi" +
              "chever subtitle file \r\nmost closely matches the video.");
      // 
      // radioButtonTimingSubs1
      // 
      this.radioButtonTimingSubs1.AutoSize = true;
      this.radioButtonTimingSubs1.Checked = true;
      this.radioButtonTimingSubs1.Location = new System.Drawing.Point(10, 19);
      this.radioButtonTimingSubs1.Name = "radioButtonTimingSubs1";
      this.radioButtonTimingSubs1.Size = new System.Drawing.Size(55, 17);
      this.radioButtonTimingSubs1.TabIndex = 0;
      this.radioButtonTimingSubs1.TabStop = true;
      this.radioButtonTimingSubs1.Text = "Subs1";
      this.toolTip1.SetToolTip(this.radioButtonTimingSubs1, "Use the dialog timings from Subs1");
      this.radioButtonTimingSubs1.UseVisualStyleBackColor = true;
      // 
      // radioButtonTimingSubs2
      // 
      this.radioButtonTimingSubs2.AutoSize = true;
      this.radioButtonTimingSubs2.Location = new System.Drawing.Point(10, 40);
      this.radioButtonTimingSubs2.Name = "radioButtonTimingSubs2";
      this.radioButtonTimingSubs2.Size = new System.Drawing.Size(55, 17);
      this.radioButtonTimingSubs2.TabIndex = 1;
      this.radioButtonTimingSubs2.TabStop = true;
      this.radioButtonTimingSubs2.Text = "Subs2";
      this.toolTip1.SetToolTip(this.radioButtonTimingSubs2, "Use the dialog timings from Subs2");
      this.radioButtonTimingSubs2.UseVisualStyleBackColor = true;
      // 
      // labelEncodingSubs1
      // 
      this.labelEncodingSubs1.AutoSize = true;
      this.labelEncodingSubs1.Location = new System.Drawing.Point(433, 16);
      this.labelEncodingSubs1.Name = "labelEncodingSubs1";
      this.labelEncodingSubs1.Size = new System.Drawing.Size(88, 13);
      this.labelEncodingSubs1.TabIndex = 34;
      this.labelEncodingSubs1.Text = "Subs1 Encoding:";
      this.toolTip1.SetToolTip(this.labelEncodingSubs1, "Encoding to use for Subs1");
      // 
      // comboBoxEncodingSubs2
      // 
      this.comboBoxEncodingSubs2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboBoxEncodingSubs2.DropDownWidth = 250;
      this.comboBoxEncodingSubs2.FormattingEnabled = true;
      this.comboBoxEncodingSubs2.Location = new System.Drawing.Point(436, 72);
      this.comboBoxEncodingSubs2.Name = "comboBoxEncodingSubs2";
      this.comboBoxEncodingSubs2.Size = new System.Drawing.Size(131, 21);
      this.comboBoxEncodingSubs2.TabIndex = 5;
      // 
      // labelEncodingSubs2
      // 
      this.labelEncodingSubs2.AutoSize = true;
      this.labelEncodingSubs2.Location = new System.Drawing.Point(433, 56);
      this.labelEncodingSubs2.Name = "labelEncodingSubs2";
      this.labelEncodingSubs2.Size = new System.Drawing.Size(88, 13);
      this.labelEncodingSubs2.TabIndex = 34;
      this.labelEncodingSubs2.Text = "Subs2 Encoding:";
      this.toolTip1.SetToolTip(this.labelEncodingSubs2, "Encoding to use for Subs2");
      // 
      // groupBoxCheckTimeShift
      // 
      this.groupBoxCheckTimeShift.Checked = false;
      this.groupBoxCheckTimeShift.Controls.Add(this.labelTimeShiftSubs2Units);
      this.groupBoxCheckTimeShift.Controls.Add(this.numericUpDownTimeShiftSubs2);
      this.groupBoxCheckTimeShift.Controls.Add(this.labelTimeShiftSubs2);
      this.groupBoxCheckTimeShift.Controls.Add(this.labelTimeShiftSubs1);
      this.groupBoxCheckTimeShift.Controls.Add(this.numericUpDownTimeShiftSubs1);
      this.groupBoxCheckTimeShift.Controls.Add(this.labelTimeShiftSubs1Units);
      this.groupBoxCheckTimeShift.Location = new System.Drawing.Point(125, 99);
      this.groupBoxCheckTimeShift.Name = "groupBoxCheckTimeShift";
      this.groupBoxCheckTimeShift.Padding = new System.Windows.Forms.Padding(0, 3, 0, 0);
      this.groupBoxCheckTimeShift.Size = new System.Drawing.Size(128, 65);
      this.groupBoxCheckTimeShift.TabIndex = 7;
      this.groupBoxCheckTimeShift.TabStop = false;
      this.groupBoxCheckTimeShift.Text = "Time Shift:";
      this.toolTip1.SetToolTip(this.groupBoxCheckTimeShift, resources.GetString("groupBoxCheckTimeShift.ToolTip"));
      // 
      // labelTimeShiftSubs2Units
      // 
      this.labelTimeShiftSubs2Units.AutoSize = true;
      this.labelTimeShiftSubs2Units.Enabled = false;
      this.labelTimeShiftSubs2Units.Location = new System.Drawing.Point(105, 42);
      this.labelTimeShiftSubs2Units.Name = "labelTimeShiftSubs2Units";
      this.labelTimeShiftSubs2Units.Size = new System.Drawing.Size(20, 13);
      this.labelTimeShiftSubs2Units.TabIndex = 55;
      this.labelTimeShiftSubs2Units.Text = "ms";
      this.toolTip1.SetToolTip(this.labelTimeShiftSubs2Units, "Shift Subs2 dialog timings. \r\nNegative shifts may be used.");
      // 
      // numericUpDownTimeShiftSubs2
      // 
      this.numericUpDownTimeShiftSubs2.Enabled = false;
      this.numericUpDownTimeShiftSubs2.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
      this.numericUpDownTimeShiftSubs2.Location = new System.Drawing.Point(47, 40);
      this.numericUpDownTimeShiftSubs2.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
      this.numericUpDownTimeShiftSubs2.Minimum = new decimal(new int[] {
            99999,
            0,
            0,
            -2147483648});
      this.numericUpDownTimeShiftSubs2.Name = "numericUpDownTimeShiftSubs2";
      this.numericUpDownTimeShiftSubs2.Size = new System.Drawing.Size(55, 20);
      this.numericUpDownTimeShiftSubs2.TabIndex = 1;
      this.toolTip1.SetToolTip(this.numericUpDownTimeShiftSubs2, "Shift Subs2 dialog timings. \r\nNegative shifts may be used.");
      // 
      // labelTimeShiftSubs2
      // 
      this.labelTimeShiftSubs2.AutoSize = true;
      this.labelTimeShiftSubs2.Location = new System.Drawing.Point(6, 43);
      this.labelTimeShiftSubs2.Name = "labelTimeShiftSubs2";
      this.labelTimeShiftSubs2.Size = new System.Drawing.Size(40, 13);
      this.labelTimeShiftSubs2.TabIndex = 53;
      this.labelTimeShiftSubs2.Text = "Subs2:";
      this.toolTip1.SetToolTip(this.labelTimeShiftSubs2, "Shift Subs2 dialog timings. \r\nNegative shifts may be used.");
      // 
      // labelTimeShiftSubs1
      // 
      this.labelTimeShiftSubs1.AutoSize = true;
      this.labelTimeShiftSubs1.Location = new System.Drawing.Point(6, 19);
      this.labelTimeShiftSubs1.Name = "labelTimeShiftSubs1";
      this.labelTimeShiftSubs1.Size = new System.Drawing.Size(40, 13);
      this.labelTimeShiftSubs1.TabIndex = 52;
      this.labelTimeShiftSubs1.Text = "Subs1:";
      this.toolTip1.SetToolTip(this.labelTimeShiftSubs1, "Shift Subs1 dialog timings. \r\nNegative shifts may be used.");
      // 
      // numericUpDownTimeShiftSubs1
      // 
      this.numericUpDownTimeShiftSubs1.Enabled = false;
      this.numericUpDownTimeShiftSubs1.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
      this.numericUpDownTimeShiftSubs1.Location = new System.Drawing.Point(47, 16);
      this.numericUpDownTimeShiftSubs1.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
      this.numericUpDownTimeShiftSubs1.Minimum = new decimal(new int[] {
            99999,
            0,
            0,
            -2147483648});
      this.numericUpDownTimeShiftSubs1.Name = "numericUpDownTimeShiftSubs1";
      this.numericUpDownTimeShiftSubs1.Size = new System.Drawing.Size(55, 20);
      this.numericUpDownTimeShiftSubs1.TabIndex = 0;
      this.toolTip1.SetToolTip(this.numericUpDownTimeShiftSubs1, "Shift Subs1 dialog timings. \r\nNegative shifts may be used.");
      // 
      // labelTimeShiftSubs1Units
      // 
      this.labelTimeShiftSubs1Units.AutoSize = true;
      this.labelTimeShiftSubs1Units.Enabled = false;
      this.labelTimeShiftSubs1Units.Location = new System.Drawing.Point(105, 18);
      this.labelTimeShiftSubs1Units.Name = "labelTimeShiftSubs1Units";
      this.labelTimeShiftSubs1Units.Size = new System.Drawing.Size(20, 13);
      this.labelTimeShiftSubs1Units.TabIndex = 0;
      this.labelTimeShiftSubs1Units.Text = "ms";
      this.toolTip1.SetToolTip(this.labelTimeShiftSubs1Units, "Shift Subs1 dialog timings. \r\nNegative shifts may be used.");
      // 
      // labelSubs2File
      // 
      this.labelSubs2File.AutoSize = true;
      this.labelSubs2File.Location = new System.Drawing.Point(6, 57);
      this.labelSubs2File.Name = "labelSubs2File";
      this.labelSubs2File.Size = new System.Drawing.Size(264, 13);
      this.labelSubs2File.TabIndex = 28;
      this.labelSubs2File.Text = "The corresponding subtitle file in your native language:";
      this.toolTip1.SetToolTip(this.labelSubs2File, resources.GetString("labelSubs2File.ToolTip"));
      // 
      // labelSubs1File
      // 
      this.labelSubs1File.AutoSize = true;
      this.labelSubs1File.Location = new System.Drawing.Point(6, 17);
      this.labelSubs1File.Name = "labelSubs1File";
      this.labelSubs1File.Size = new System.Drawing.Size(172, 13);
      this.labelSubs1File.TabIndex = 29;
      this.labelSubs1File.Text = "Subtitle file in your target language:";
      this.toolTip1.SetToolTip(this.labelSubs1File, resources.GetString("labelSubs1File.ToolTip"));
      // 
      // textBoxSubs1File
      // 
      this.textBoxSubs1File.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.textBoxSubs1File.Location = new System.Drawing.Point(71, 33);
      this.textBoxSubs1File.Name = "textBoxSubs1File";
      this.textBoxSubs1File.Size = new System.Drawing.Size(346, 20);
      this.textBoxSubs1File.TabIndex = 1;
      this.toolTip1.SetToolTip(this.textBoxSubs1File, resources.GetString("textBoxSubs1File.ToolTip"));
      this.textBoxSubs1File.TextChanged += new System.EventHandler(this.textBoxSubs1File_TextChanged);
      this.textBoxSubs1File.Validating += new System.ComponentModel.CancelEventHandler(this.textBoxSubs1File_Validating);
      // 
      // buttonSubs2File
      // 
      this.buttonSubs2File.Location = new System.Drawing.Point(9, 72);
      this.buttonSubs2File.Name = "buttonSubs2File";
      this.buttonSubs2File.Size = new System.Drawing.Size(56, 22);
      this.buttonSubs2File.TabIndex = 3;
      this.buttonSubs2File.Text = "Su&bs2...";
      this.toolTip1.SetToolTip(this.buttonSubs2File, "Select a subtitle file");
      this.buttonSubs2File.UseVisualStyleBackColor = true;
      this.buttonSubs2File.Click += new System.EventHandler(this.buttonSubs2File_Click);
      // 
      // buttonSubs1File
      // 
      this.buttonSubs1File.Location = new System.Drawing.Point(9, 32);
      this.buttonSubs1File.Name = "buttonSubs1File";
      this.buttonSubs1File.Size = new System.Drawing.Size(56, 22);
      this.buttonSubs1File.TabIndex = 0;
      this.buttonSubs1File.Text = "&Subs1...";
      this.toolTip1.SetToolTip(this.buttonSubs1File, "Select a subtitle file");
      this.buttonSubs1File.UseVisualStyleBackColor = true;
      this.buttonSubs1File.Click += new System.EventHandler(this.buttonSubs1File_Click);
      // 
      // textBoxSubs2File
      // 
      this.textBoxSubs2File.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.textBoxSubs2File.Location = new System.Drawing.Point(71, 73);
      this.textBoxSubs2File.Name = "textBoxSubs2File";
      this.textBoxSubs2File.Size = new System.Drawing.Size(346, 20);
      this.textBoxSubs2File.TabIndex = 4;
      this.toolTip1.SetToolTip(this.textBoxSubs2File, resources.GetString("textBoxSubs2File.ToolTip"));
      this.textBoxSubs2File.TextChanged += new System.EventHandler(this.textBoxSubs2File_TextChanged);
      this.textBoxSubs2File.Validating += new System.ComponentModel.CancelEventHandler(this.textBoxSubs2File_Validating);
      // 
      // groupBoxCheckSpan
      // 
      this.groupBoxCheckSpan.Checked = false;
      this.groupBoxCheckSpan.Controls.Add(this.labelSpanStart);
      this.groupBoxCheckSpan.Controls.Add(this.maskedTextBoxSpanEnd);
      this.groupBoxCheckSpan.Controls.Add(this.maskedTextBoxSpanStart);
      this.groupBoxCheckSpan.Controls.Add(this.labelSpanEnd);
      this.groupBoxCheckSpan.Location = new System.Drawing.Point(7, 19);
      this.groupBoxCheckSpan.Name = "groupBoxCheckSpan";
      this.groupBoxCheckSpan.Padding = new System.Windows.Forms.Padding(0, 3, 0, 0);
      this.groupBoxCheckSpan.Size = new System.Drawing.Size(122, 65);
      this.groupBoxCheckSpan.TabIndex = 0;
      this.groupBoxCheckSpan.TabStop = false;
      this.groupBoxCheckSpan.Text = "Span (h:mm:ss):";
      this.toolTip1.SetToolTip(this.groupBoxCheckSpan, "Only process lines that start within the specified span of time.");
      // 
      // labelSpanStart
      // 
      this.labelSpanStart.AutoSize = true;
      this.labelSpanStart.Enabled = false;
      this.labelSpanStart.Location = new System.Drawing.Point(6, 19);
      this.labelSpanStart.Name = "labelSpanStart";
      this.labelSpanStart.Size = new System.Drawing.Size(32, 13);
      this.labelSpanStart.TabIndex = 0;
      this.labelSpanStart.Text = "Start:";
      // 
      // maskedTextBoxSpanEnd
      // 
      this.maskedTextBoxSpanEnd.Enabled = false;
      this.maskedTextBoxSpanEnd.Location = new System.Drawing.Point(39, 40);
      this.maskedTextBoxSpanEnd.Mask = "0:00:00";
      this.maskedTextBoxSpanEnd.Name = "maskedTextBoxSpanEnd";
      this.maskedTextBoxSpanEnd.Size = new System.Drawing.Size(45, 20);
      this.maskedTextBoxSpanEnd.TabIndex = 3;
      this.maskedTextBoxSpanEnd.Text = "00500";
      this.maskedTextBoxSpanEnd.Validating += new System.ComponentModel.CancelEventHandler(this.maskedTextBoxSpan_Validating);
      // 
      // maskedTextBoxSpanStart
      // 
      this.maskedTextBoxSpanStart.Enabled = false;
      this.maskedTextBoxSpanStart.Location = new System.Drawing.Point(39, 16);
      this.maskedTextBoxSpanStart.Mask = "0:00:00";
      this.maskedTextBoxSpanStart.Name = "maskedTextBoxSpanStart";
      this.maskedTextBoxSpanStart.Size = new System.Drawing.Size(45, 20);
      this.maskedTextBoxSpanStart.TabIndex = 1;
      this.maskedTextBoxSpanStart.Text = "00130";
      this.maskedTextBoxSpanStart.Validating += new System.ComponentModel.CancelEventHandler(this.maskedTextBoxSpan_Validating);
      // 
      // labelSpanEnd
      // 
      this.labelSpanEnd.AutoSize = true;
      this.labelSpanEnd.Enabled = false;
      this.labelSpanEnd.Location = new System.Drawing.Point(9, 43);
      this.labelSpanEnd.Name = "labelSpanEnd";
      this.labelSpanEnd.Size = new System.Drawing.Size(29, 13);
      this.labelSpanEnd.TabIndex = 2;
      this.labelSpanEnd.Text = "End:";
      // 
      // groupBoxBitrate
      // 
      this.groupBoxBitrate.Controls.Add(this.comboBoxBitrate);
      this.groupBoxBitrate.Controls.Add(this.labelBitrateUnits);
      this.groupBoxBitrate.Location = new System.Drawing.Point(135, 20);
      this.groupBoxBitrate.Name = "groupBoxBitrate";
      this.groupBoxBitrate.Size = new System.Drawing.Size(94, 64);
      this.groupBoxBitrate.TabIndex = 1;
      this.groupBoxBitrate.TabStop = false;
      this.groupBoxBitrate.Text = "Bitrate:";
      this.toolTip1.SetToolTip(this.groupBoxBitrate, "Select a bitrate for the converted .mp3 files.\r\n\r\nHigher bitrates mean better qua" +
              "lity at the expense of larger file size.");
      // 
      // comboBoxBitrate
      // 
      this.comboBoxBitrate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboBoxBitrate.FormattingEnabled = true;
      this.comboBoxBitrate.Items.AddRange(new object[] {
            "32",
            "40",
            "48",
            "56",
            "64",
            "80",
            "96",
            "112",
            "128",
            "144",
            "160",
            "192",
            "224",
            "256",
            "320"});
      this.comboBoxBitrate.Location = new System.Drawing.Point(9, 26);
      this.comboBoxBitrate.Name = "comboBoxBitrate";
      this.comboBoxBitrate.Size = new System.Drawing.Size(47, 21);
      this.comboBoxBitrate.TabIndex = 0;
      this.toolTip1.SetToolTip(this.comboBoxBitrate, "Select a bitrate for the converted .mp3 files.\r\n\r\nHigher bitrates mean better qua" +
              "lity at the expense of larger file size.");
      // 
      // labelBitrateUnits
      // 
      this.labelBitrateUnits.AutoSize = true;
      this.labelBitrateUnits.Location = new System.Drawing.Point(59, 29);
      this.labelBitrateUnits.Name = "labelBitrateUnits";
      this.labelBitrateUnits.Size = new System.Drawing.Size(29, 13);
      this.labelBitrateUnits.TabIndex = 1;
      this.labelBitrateUnits.Text = "kb/s";
      this.toolTip1.SetToolTip(this.labelBitrateUnits, "Select a bitrate for the converted .mp3 files.\r\n\r\nHigher bitrates mean better qua" +
              "lity at the expense of larger file size.");
      // 
      // groupBoxFormat
      // 
      this.groupBoxFormat.Controls.Add(this.radioButtonSingle);
      this.groupBoxFormat.Controls.Add(this.maskedTextBoxClipLength);
      this.groupBoxFormat.Controls.Add(this.radioButtonMultiple);
      this.groupBoxFormat.Location = new System.Drawing.Point(234, 20);
      this.groupBoxFormat.Name = "groupBoxFormat";
      this.groupBoxFormat.Size = new System.Drawing.Size(348, 64);
      this.groupBoxFormat.TabIndex = 2;
      this.groupBoxFormat.TabStop = false;
      this.groupBoxFormat.Text = "Format:";
      // 
      // radioButtonSingle
      // 
      this.radioButtonSingle.AutoSize = true;
      this.radioButtonSingle.Location = new System.Drawing.Point(9, 15);
      this.radioButtonSingle.Name = "radioButtonSingle";
      this.radioButtonSingle.Size = new System.Drawing.Size(215, 17);
      this.radioButtonSingle.TabIndex = 0;
      this.radioButtonSingle.Text = "Extract entire audio track as a single clip";
      this.radioButtonSingle.UseVisualStyleBackColor = true;
      // 
      // maskedTextBoxClipLength
      // 
      this.maskedTextBoxClipLength.Location = new System.Drawing.Point(295, 39);
      this.maskedTextBoxClipLength.Mask = "0:00:00";
      this.maskedTextBoxClipLength.Name = "maskedTextBoxClipLength";
      this.maskedTextBoxClipLength.Size = new System.Drawing.Size(45, 20);
      this.maskedTextBoxClipLength.TabIndex = 2;
      this.maskedTextBoxClipLength.Text = "00500";
      this.maskedTextBoxClipLength.Validating += new System.ComponentModel.CancelEventHandler(this.maskedTextBoxClipLength_Validating);
      // 
      // radioButtonMultiple
      // 
      this.radioButtonMultiple.AutoSize = true;
      this.radioButtonMultiple.Checked = true;
      this.radioButtonMultiple.Location = new System.Drawing.Point(9, 40);
      this.radioButtonMultiple.Name = "radioButtonMultiple";
      this.radioButtonMultiple.Size = new System.Drawing.Size(285, 17);
      this.radioButtonMultiple.TabIndex = 1;
      this.radioButtonMultiple.TabStop = true;
      this.radioButtonMultiple.Text = "Break audio track into multiple clips of length (h:mm:ss):";
      this.radioButtonMultiple.UseVisualStyleBackColor = true;
      this.radioButtonMultiple.CheckedChanged += new System.EventHandler(this.radioButtonMultiple_CheckedChanged);
      // 
      // buttonExtract
      // 
      this.buttonExtract.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.buttonExtract.ForeColor = System.Drawing.Color.DarkGreen;
      this.buttonExtract.Location = new System.Drawing.Point(255, 453);
      this.buttonExtract.Name = "buttonExtract";
      this.buttonExtract.Size = new System.Drawing.Size(108, 32);
      this.buttonExtract.TabIndex = 11;
      this.buttonExtract.Text = "&Extract Audio";
      this.buttonExtract.UseVisualStyleBackColor = true;
      this.buttonExtract.Click += new System.EventHandler(this.buttonExtract_Click);
      // 
      // buttonCancel
      // 
      this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.buttonCancel.Location = new System.Drawing.Point(526, 462);
      this.buttonCancel.Name = "buttonCancel";
      this.buttonCancel.Size = new System.Drawing.Size(75, 23);
      this.buttonCancel.TabIndex = 12;
      this.buttonCancel.Text = "&Cancel";
      this.buttonCancel.UseVisualStyleBackColor = true;
      // 
      // buttonOutput
      // 
      this.buttonOutput.Location = new System.Drawing.Point(12, 92);
      this.buttonOutput.Name = "buttonOutput";
      this.buttonOutput.Size = new System.Drawing.Size(56, 22);
      this.buttonOutput.TabIndex = 7;
      this.buttonOutput.Text = "&Output...";
      this.toolTip1.SetToolTip(this.buttonOutput, "Select an output directory");
      this.buttonOutput.UseVisualStyleBackColor = true;
      this.buttonOutput.Click += new System.EventHandler(this.buttonOut_Click);
      // 
      // textBoxOutputDir
      // 
      this.textBoxOutputDir.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.textBoxOutputDir.Location = new System.Drawing.Point(73, 93);
      this.textBoxOutputDir.Name = "textBoxOutputDir";
      this.textBoxOutputDir.Size = new System.Drawing.Size(397, 20);
      this.textBoxOutputDir.TabIndex = 8;
      this.toolTip1.SetToolTip(this.textBoxOutputDir, "Directory where the extracted. converted and split .mp3 files will be placed");
      this.textBoxOutputDir.Validating += new System.ComponentModel.CancelEventHandler(this.textBoxOut_Validating);
      // 
      // labelOutputDir
      // 
      this.labelOutputDir.AutoSize = true;
      this.labelOutputDir.Location = new System.Drawing.Point(9, 77);
      this.labelOutputDir.Name = "labelOutputDir";
      this.labelOutputDir.Size = new System.Drawing.Size(241, 13);
      this.labelOutputDir.TabIndex = 6;
      this.labelOutputDir.Text = "Directory where the generated files will be placed:";
      this.toolTip1.SetToolTip(this.labelOutputDir, "Directory where the extracted, converted and split .mp3 files will be placed");
      // 
      // labelName
      // 
      this.labelName.AutoSize = true;
      this.labelName.Location = new System.Drawing.Point(4, 16);
      this.labelName.Name = "labelName";
      this.labelName.Size = new System.Drawing.Size(38, 13);
      this.labelName.TabIndex = 0;
      this.labelName.Text = "Name:";
      this.toolTip1.SetToolTip(this.labelName, "The name to use with the extracted .mp3 files");
      // 
      // textBoxName
      // 
      this.textBoxName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.textBoxName.Location = new System.Drawing.Point(7, 32);
      this.textBoxName.Name = "textBoxName";
      this.textBoxName.Size = new System.Drawing.Size(486, 20);
      this.textBoxName.TabIndex = 1;
      this.toolTip1.SetToolTip(this.textBoxName, "The name to use with the extracted .mp3 files");
      this.textBoxName.Validating += new System.ComponentModel.CancelEventHandler(this.textBoxName_Validating);
      // 
      // labelEpisodeStartNumber
      // 
      this.labelEpisodeStartNumber.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.labelEpisodeStartNumber.AutoSize = true;
      this.labelEpisodeStartNumber.Location = new System.Drawing.Point(514, 16);
      this.labelEpisodeStartNumber.Name = "labelEpisodeStartNumber";
      this.labelEpisodeStartNumber.Size = new System.Drawing.Size(70, 13);
      this.labelEpisodeStartNumber.TabIndex = 2;
      this.labelEpisodeStartNumber.Text = "First Episode:";
      this.toolTip1.SetToolTip(this.labelEpisodeStartNumber, "The episode number to start at for use in the extracted .mp3 files");
      // 
      // toolTip1
      // 
      this.toolTip1.AutoPopDelay = 20000;
      this.toolTip1.InitialDelay = 500;
      this.toolTip1.ReshowDelay = 100;
      // 
      // labelAudioStream
      // 
      this.labelAudioStream.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.labelAudioStream.AutoSize = true;
      this.labelAudioStream.Enabled = false;
      this.labelAudioStream.Location = new System.Drawing.Point(488, 36);
      this.labelAudioStream.Name = "labelAudioStream";
      this.labelAudioStream.Size = new System.Drawing.Size(73, 13);
      this.labelAudioStream.TabIndex = 4;
      this.labelAudioStream.Text = "Audio Stream:";
      this.toolTip1.SetToolTip(this.labelAudioStream, "Some videos files contain multiple audio streams. Please choose one.");
      // 
      // comboBoxStreamAudioFromVideo
      // 
      this.comboBoxStreamAudioFromVideo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.comboBoxStreamAudioFromVideo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboBoxStreamAudioFromVideo.Enabled = false;
      this.comboBoxStreamAudioFromVideo.FormattingEnabled = true;
      this.comboBoxStreamAudioFromVideo.Location = new System.Drawing.Point(491, 52);
      this.comboBoxStreamAudioFromVideo.Name = "comboBoxStreamAudioFromVideo";
      this.comboBoxStreamAudioFromVideo.Size = new System.Drawing.Size(110, 21);
      this.comboBoxStreamAudioFromVideo.TabIndex = 5;
      this.toolTip1.SetToolTip(this.comboBoxStreamAudioFromVideo, "Some videos files contain multiple audio streams. Please choose one.");
      // 
      // numericUpDownEpisodeStartNumber
      // 
      this.numericUpDownEpisodeStartNumber.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.numericUpDownEpisodeStartNumber.Location = new System.Drawing.Point(517, 32);
      this.numericUpDownEpisodeStartNumber.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
      this.numericUpDownEpisodeStartNumber.Name = "numericUpDownEpisodeStartNumber";
      this.numericUpDownEpisodeStartNumber.Size = new System.Drawing.Size(45, 20);
      this.numericUpDownEpisodeStartNumber.TabIndex = 3;
      this.toolTip1.SetToolTip(this.numericUpDownEpisodeStartNumber, "The episode number to start at for use in the extracted .mp3 files");
      this.numericUpDownEpisodeStartNumber.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
      // 
      // groupBoxName
      // 
      this.groupBoxName.Controls.Add(this.labelRequiredName);
      this.groupBoxName.Controls.Add(this.labelName);
      this.groupBoxName.Controls.Add(this.textBoxName);
      this.groupBoxName.Controls.Add(this.labelEpisodeStartNumber);
      this.groupBoxName.Controls.Add(this.numericUpDownEpisodeStartNumber);
      this.groupBoxName.Location = new System.Drawing.Point(12, 390);
      this.groupBoxName.Name = "groupBoxName";
      this.groupBoxName.Size = new System.Drawing.Size(589, 58);
      this.groupBoxName.TabIndex = 10;
      this.groupBoxName.TabStop = false;
      this.groupBoxName.Text = "Naming:";
      // 
      // labelRequiredName
      // 
      this.labelRequiredName.AutoSize = true;
      this.labelRequiredName.ForeColor = System.Drawing.Color.Red;
      this.labelRequiredName.Location = new System.Drawing.Point(44, 16);
      this.labelRequiredName.Name = "labelRequiredName";
      this.labelRequiredName.Size = new System.Drawing.Size(51, 13);
      this.labelRequiredName.TabIndex = 111;
      this.labelRequiredName.Text = "(required)";
      // 
      // errorProvider1
      // 
      this.errorProvider1.ContainerControl = this;
      // 
      // labelRequiredMedia
      // 
      this.labelRequiredMedia.AutoSize = true;
      this.labelRequiredMedia.ForeColor = System.Drawing.Color.Red;
      this.labelRequiredMedia.Location = new System.Drawing.Point(138, 37);
      this.labelRequiredMedia.Name = "labelRequiredMedia";
      this.labelRequiredMedia.Size = new System.Drawing.Size(51, 13);
      this.labelRequiredMedia.TabIndex = 111;
      this.labelRequiredMedia.Text = "(required)";
      // 
      // labelRequiremedOutput
      // 
      this.labelRequiremedOutput.AutoSize = true;
      this.labelRequiremedOutput.ForeColor = System.Drawing.Color.Red;
      this.labelRequiremedOutput.Location = new System.Drawing.Point(252, 77);
      this.labelRequiremedOutput.Name = "labelRequiremedOutput";
      this.labelRequiremedOutput.Size = new System.Drawing.Size(51, 13);
      this.labelRequiremedOutput.TabIndex = 111;
      this.labelRequiremedOutput.Text = "(required)";
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
      this.textBoxHelp.Size = new System.Drawing.Size(589, 21);
      this.textBoxHelp.TabIndex = 114;
      this.textBoxHelp.TabStop = false;
      this.textBoxHelp.Text = "Use this tool to extract, convert and split the audio track from a media file.";
      this.textBoxHelp.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
      // 
      // DialogExtractAudioFromMedia
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.BackColor = System.Drawing.SystemColors.Control;
      this.CancelButton = this.buttonCancel;
      this.ClientSize = new System.Drawing.Size(613, 497);
      this.Controls.Add(this.textBoxHelp);
      this.Controls.Add(this.labelRequiremedOutput);
      this.Controls.Add(this.labelRequiredMedia);
      this.Controls.Add(this.groupBoxName);
      this.Controls.Add(this.labelAudioStream);
      this.Controls.Add(this.comboBoxStreamAudioFromVideo);
      this.Controls.Add(this.labelOutputDir);
      this.Controls.Add(this.buttonOutput);
      this.Controls.Add(this.textBoxOutputDir);
      this.Controls.Add(this.buttonCancel);
      this.Controls.Add(this.buttonExtract);
      this.Controls.Add(this.groupBoxOptions);
      this.Controls.Add(this.labelMediaFile);
      this.Controls.Add(this.buttonMediaFile);
      this.Controls.Add(this.textBoxMediaFile);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "DialogExtractAudioFromMedia";
      this.Text = "Extract Audio from Media Tool";
      this.Load += new System.EventHandler(this.DialogAudioRipper_Load);
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DialogAudioRipper_FormClosing);
      this.groupBoxOptions.ResumeLayout(false);
      this.groupBoxCheckLyrics.ResumeLayout(false);
      this.groupBoxCheckLyrics.PerformLayout();
      this.groupBoxRemoveLinesWithoutCounterpart.ResumeLayout(false);
      this.groupBoxRemoveLinesWithoutCounterpart.PerformLayout();
      this.groupBoxRemoveStyledLines.ResumeLayout(false);
      this.groupBoxRemoveStyledLines.PerformLayout();
      this.groupBoxUseTimingsFrom.ResumeLayout(false);
      this.groupBoxUseTimingsFrom.PerformLayout();
      this.groupBoxCheckTimeShift.ResumeLayout(false);
      this.groupBoxCheckTimeShift.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownTimeShiftSubs2)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownTimeShiftSubs1)).EndInit();
      this.groupBoxCheckSpan.ResumeLayout(false);
      this.groupBoxCheckSpan.PerformLayout();
      this.groupBoxBitrate.ResumeLayout(false);
      this.groupBoxBitrate.PerformLayout();
      this.groupBoxFormat.ResumeLayout(false);
      this.groupBoxFormat.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownEpisodeStartNumber)).EndInit();
      this.groupBoxName.ResumeLayout(false);
      this.groupBoxName.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Label labelMediaFile;
    private System.Windows.Forms.Button buttonMediaFile;
    private System.Windows.Forms.TextBox textBoxMediaFile;
    private System.Windows.Forms.GroupBox groupBoxOptions;
    private System.Windows.Forms.RadioButton radioButtonMultiple;
    private System.Windows.Forms.RadioButton radioButtonSingle;
    private System.Windows.Forms.Button buttonExtract;
    private System.Windows.Forms.Button buttonCancel;
    private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
    private System.Windows.Forms.Button buttonOutput;
    private System.Windows.Forms.TextBox textBoxOutputDir;
    private System.Windows.Forms.Label labelOutputDir;
    private System.Windows.Forms.Label labelName;
    private System.Windows.Forms.TextBox textBoxName;
    private System.Windows.Forms.Label labelEpisodeStartNumber;
    private System.Windows.Forms.ToolTip toolTip1;
    private System.Windows.Forms.ErrorProvider errorProvider1;
    private System.Windows.Forms.MaskedTextBox maskedTextBoxClipLength;
    private System.Windows.Forms.Label labelBitrateUnits;
    private System.Windows.Forms.GroupBox groupBoxFormat;
    private System.Windows.Forms.GroupBox groupBoxBitrate;
    private System.Windows.Forms.ComboBox comboBoxBitrate;
    private System.Windows.Forms.NumericUpDown numericUpDownEpisodeStartNumber;
    private GroupBoxCheck groupBoxCheckSpan;
    private System.Windows.Forms.Label labelSpanStart;
    private System.Windows.Forms.MaskedTextBox maskedTextBoxSpanEnd;
    private System.Windows.Forms.MaskedTextBox maskedTextBoxSpanStart;
    private System.Windows.Forms.Label labelSpanEnd;
    private System.Windows.Forms.Label labelAudioStream;
    private System.Windows.Forms.ComboBox comboBoxStreamAudioFromVideo;
    private System.Windows.Forms.GroupBox groupBoxName;
    private System.Windows.Forms.OpenFileDialog openFileDialog;
    private GroupBoxCheck groupBoxCheckLyrics;
    private System.Windows.Forms.Label labelSubs2File;
    private System.Windows.Forms.Label labelSubs1File;
    private System.Windows.Forms.TextBox textBoxSubs1File;
    private System.Windows.Forms.Button buttonSubs2File;
    private System.Windows.Forms.Button buttonSubs1File;
    private System.Windows.Forms.TextBox textBoxSubs2File;
    private System.Windows.Forms.GroupBox groupBoxUseTimingsFrom;
    private System.Windows.Forms.RadioButton radioButtonTimingSubs1;
    private System.Windows.Forms.RadioButton radioButtonTimingSubs2;
    private GroupBoxCheck groupBoxCheckTimeShift;
    private System.Windows.Forms.Label labelTimeShiftSubs2Units;
    private System.Windows.Forms.NumericUpDown numericUpDownTimeShiftSubs2;
    private System.Windows.Forms.Label labelTimeShiftSubs2;
    private System.Windows.Forms.Label labelTimeShiftSubs1;
    private System.Windows.Forms.NumericUpDown numericUpDownTimeShiftSubs1;
    private System.Windows.Forms.Label labelTimeShiftSubs1Units;
    private System.Windows.Forms.ComboBox comboBoxEncodingSubs1;
    private System.Windows.Forms.Label labelEncodingSubs1;
    private System.Windows.Forms.Label labelEncodingSubs2;
    private System.Windows.Forms.ComboBox comboBoxEncodingSubs2;
    private System.Windows.Forms.Label labelRequiredName;
    private System.Windows.Forms.Label labelRequiremedOutput;
    private System.Windows.Forms.Label labelRequiredMedia;
    private System.Windows.Forms.Label label1RequiredSubs1;
    private System.Windows.Forms.Label labelOptionalSubs2;
    private System.Windows.Forms.GroupBox groupBoxRemoveStyledLines;
    private System.Windows.Forms.CheckBox checkBoxRemoveStyledLinesSubs2;
    private System.Windows.Forms.CheckBox checkBoxRemoveStyledLinesSubs1;
    private System.Windows.Forms.TextBox textBoxHelp;
    private System.Windows.Forms.GroupBox groupBoxRemoveLinesWithoutCounterpart;
    private System.Windows.Forms.CheckBox checkBoxRemoveLinesWithoutCounterpartSubs2;
    private System.Windows.Forms.CheckBox checkBoxRemoveLinesWithoutCounterpartSubs1;
  }
}