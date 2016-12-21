namespace subs2srs
{
  partial class DialogDuelingSubtitles
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DialogDuelingSubtitles));
      this.buttonCancel = new System.Windows.Forms.Button();
      this.buttonCreate = new System.Windows.Forms.Button();
      this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
      this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
      this.buttonOutputDir = new System.Windows.Forms.Button();
      this.buttonSubs2File = new System.Windows.Forms.Button();
      this.buttonSubs1File = new System.Windows.Forms.Button();
      this.groupBoxUseTimingsFrom = new System.Windows.Forms.GroupBox();
      this.radioButtonTimingSubs1 = new System.Windows.Forms.RadioButton();
      this.radioButtonTimingSubs2 = new System.Windows.Forms.RadioButton();
      this.numericUpDownPattern = new System.Windows.Forms.NumericUpDown();
      this.comboBoxPriority = new System.Windows.Forms.ComboBox();
      this.checkBoxQuickReference = new System.Windows.Forms.CheckBox();
      this.labelPriority = new System.Windows.Forms.Label();
      this.button2StyleSubs2 = new System.Windows.Forms.Button();
      this.buttonStyleSubs1 = new System.Windows.Forms.Button();
      this.labelOutputDir = new System.Windows.Forms.Label();
      this.labelSubs2File = new System.Windows.Forms.Label();
      this.textBoxOutputDir = new System.Windows.Forms.TextBox();
      this.labelSubs1File = new System.Windows.Forms.Label();
      this.textBoxSubs2File = new System.Windows.Forms.TextBox();
      this.textBoxSubs1File = new System.Windows.Forms.TextBox();
      this.labelPattern1 = new System.Windows.Forms.Label();
      this.labelPattern2 = new System.Windows.Forms.Label();
      this.groupBoxName = new System.Windows.Forms.GroupBox();
      this.labelRequiredName = new System.Windows.Forms.Label();
      this.labelName = new System.Windows.Forms.Label();
      this.textBoxName = new System.Windows.Forms.TextBox();
      this.labelEpisodeStartNumber = new System.Windows.Forms.Label();
      this.numericUpDownEpisodeStartNumber = new System.Windows.Forms.NumericUpDown();
      this.labelEncodingSubs1 = new System.Windows.Forms.Label();
      this.labelEndodingSubs2 = new System.Windows.Forms.Label();
      this.groupBoxRemoveStyledLines = new System.Windows.Forms.GroupBox();
      this.checkBoxRemoveStyledLinesSubs2 = new System.Windows.Forms.CheckBox();
      this.checkBoxRemoveStyledLinesSubs1 = new System.Windows.Forms.CheckBox();
      this.groupBoxRemoveLinesWithoutCounterpart = new System.Windows.Forms.GroupBox();
      this.checkBoxRemoveLinesWithoutCounterpartSubs2 = new System.Windows.Forms.CheckBox();
      this.checkBoxRemoveLinesWithoutCounterpartSubs1 = new System.Windows.Forms.CheckBox();
      this.groupBoxTimeShift = new subs2srs.GroupBoxCheck();
      this.labelTimeShiftSubs2Units = new System.Windows.Forms.Label();
      this.numericUpDownTimeShiftSubs2 = new System.Windows.Forms.NumericUpDown();
      this.labelTimeShiftSubs2 = new System.Windows.Forms.Label();
      this.labelTimeShiftSubs1 = new System.Windows.Forms.Label();
      this.numericUpDownTimeShiftSubs1 = new System.Windows.Forms.NumericUpDown();
      this.labelTimeShiftSubs1Units = new System.Windows.Forms.Label();
      this.groupBoxOptionsSubs1 = new System.Windows.Forms.GroupBox();
      this.groupBoxSubtitleOptions = new System.Windows.Forms.GroupBox();
      this.groupOtherOptions = new System.Windows.Forms.GroupBox();
      this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
      this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
      this.comboBoxEncodingSubs1 = new System.Windows.Forms.ComboBox();
      this.comboBoxEncodingSubs2 = new System.Windows.Forms.ComboBox();
      this.labelRequiredSubs1 = new System.Windows.Forms.Label();
      this.labelRequiredSubs2 = new System.Windows.Forms.Label();
      this.labelRequiredOutput = new System.Windows.Forms.Label();
      this.textBoxHelp = new System.Windows.Forms.TextBox();
      ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
      this.groupBoxUseTimingsFrom.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPattern)).BeginInit();
      this.groupBoxName.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownEpisodeStartNumber)).BeginInit();
      this.groupBoxRemoveStyledLines.SuspendLayout();
      this.groupBoxRemoveLinesWithoutCounterpart.SuspendLayout();
      this.groupBoxTimeShift.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownTimeShiftSubs2)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownTimeShiftSubs1)).BeginInit();
      this.groupBoxOptionsSubs1.SuspendLayout();
      this.groupBoxSubtitleOptions.SuspendLayout();
      this.groupOtherOptions.SuspendLayout();
      this.SuspendLayout();
      // 
      // buttonCancel
      // 
      this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.buttonCancel.Location = new System.Drawing.Point(507, 423);
      this.buttonCancel.Name = "buttonCancel";
      this.buttonCancel.Size = new System.Drawing.Size(75, 23);
      this.buttonCancel.TabIndex = 13;
      this.buttonCancel.Text = "&Cancel";
      this.buttonCancel.UseVisualStyleBackColor = true;
      // 
      // buttonCreate
      // 
      this.buttonCreate.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.buttonCreate.ForeColor = System.Drawing.Color.DarkGreen;
      this.buttonCreate.Location = new System.Drawing.Point(217, 416);
      this.buttonCreate.Name = "buttonCreate";
      this.buttonCreate.Size = new System.Drawing.Size(160, 32);
      this.buttonCreate.TabIndex = 12;
      this.buttonCreate.Text = "Create &Dueling Subtitles!";
      this.buttonCreate.UseVisualStyleBackColor = true;
      this.buttonCreate.Click += new System.EventHandler(this.buttonCreate_Click);
      // 
      // errorProvider1
      // 
      this.errorProvider1.ContainerControl = this;
      // 
      // toolTip1
      // 
      this.toolTip1.AutoPopDelay = 20000;
      this.toolTip1.InitialDelay = 500;
      this.toolTip1.ReshowDelay = 100;
      // 
      // buttonOutputDir
      // 
      this.buttonOutputDir.Location = new System.Drawing.Point(12, 145);
      this.buttonOutputDir.Name = "buttonOutputDir";
      this.buttonOutputDir.Size = new System.Drawing.Size(56, 22);
      this.buttonOutputDir.TabIndex = 6;
      this.buttonOutputDir.Text = "&Output...";
      this.toolTip1.SetToolTip(this.buttonOutputDir, "Select an output directory");
      this.buttonOutputDir.UseVisualStyleBackColor = true;
      this.buttonOutputDir.Click += new System.EventHandler(this.buttonOutputDir_Click);
      // 
      // buttonSubs2File
      // 
      this.buttonSubs2File.Location = new System.Drawing.Point(12, 105);
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
      this.buttonSubs1File.Location = new System.Drawing.Point(12, 65);
      this.buttonSubs1File.Name = "buttonSubs1File";
      this.buttonSubs1File.Size = new System.Drawing.Size(56, 22);
      this.buttonSubs1File.TabIndex = 0;
      this.buttonSubs1File.Text = "&Subs1...";
      this.toolTip1.SetToolTip(this.buttonSubs1File, "Select a subtitle file");
      this.buttonSubs1File.UseVisualStyleBackColor = true;
      this.buttonSubs1File.Click += new System.EventHandler(this.buttonSubs1File_Click);
      // 
      // groupBoxUseTimingsFrom
      // 
      this.groupBoxUseTimingsFrom.Controls.Add(this.radioButtonTimingSubs1);
      this.groupBoxUseTimingsFrom.Controls.Add(this.radioButtonTimingSubs2);
      this.groupBoxUseTimingsFrom.Location = new System.Drawing.Point(7, 16);
      this.groupBoxUseTimingsFrom.Name = "groupBoxUseTimingsFrom";
      this.groupBoxUseTimingsFrom.Size = new System.Drawing.Size(110, 65);
      this.groupBoxUseTimingsFrom.TabIndex = 0;
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
      // numericUpDownPattern
      // 
      this.numericUpDownPattern.Location = new System.Drawing.Point(161, 23);
      this.numericUpDownPattern.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
      this.numericUpDownPattern.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
      this.numericUpDownPattern.Name = "numericUpDownPattern";
      this.numericUpDownPattern.Size = new System.Drawing.Size(45, 20);
      this.numericUpDownPattern.TabIndex = 1;
      this.toolTip1.SetToolTip(this.numericUpDownPattern, resources.GetString("numericUpDownPattern.ToolTip"));
      this.numericUpDownPattern.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
      // 
      // comboBoxPriority
      // 
      this.comboBoxPriority.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboBoxPriority.FormattingEnabled = true;
      this.comboBoxPriority.Items.AddRange(new object[] {
            "Subs1",
            "Subs2"});
      this.comboBoxPriority.Location = new System.Drawing.Point(119, 51);
      this.comboBoxPriority.Name = "comboBoxPriority";
      this.comboBoxPriority.Size = new System.Drawing.Size(56, 21);
      this.comboBoxPriority.TabIndex = 2;
      this.toolTip1.SetToolTip(this.comboBoxPriority, resources.GetString("comboBoxPriority.ToolTip"));
      // 
      // checkBoxQuickReference
      // 
      this.checkBoxQuickReference.AutoSize = true;
      this.checkBoxQuickReference.Location = new System.Drawing.Point(12, 53);
      this.checkBoxQuickReference.Name = "checkBoxQuickReference";
      this.checkBoxQuickReference.Size = new System.Drawing.Size(201, 17);
      this.checkBoxQuickReference.TabIndex = 2;
      this.checkBoxQuickReference.Text = "Also generate quick reference .txt file";
      this.toolTip1.SetToolTip(this.checkBoxQuickReference, "In addition to the subtitle file, also generate a quick \r\nreference .txt file wit" +
              "h just the Subs1 and Subs2 text.\r\nCan be useful for following the dialog and loo" +
              "king up \r\nunknown words. ");
      this.checkBoxQuickReference.UseVisualStyleBackColor = true;
      // 
      // labelPriority
      // 
      this.labelPriority.AutoSize = true;
      this.labelPriority.Location = new System.Drawing.Point(27, 54);
      this.labelPriority.Name = "labelPriority";
      this.labelPriority.Size = new System.Drawing.Size(89, 13);
      this.labelPriority.TabIndex = 64;
      this.labelPriority.Text = "Alignment priority:";
      this.toolTip1.SetToolTip(this.labelPriority, resources.GetString("labelPriority.ToolTip"));
      // 
      // button2StyleSubs2
      // 
      this.button2StyleSubs2.Location = new System.Drawing.Point(107, 19);
      this.button2StyleSubs2.Name = "button2StyleSubs2";
      this.button2StyleSubs2.Size = new System.Drawing.Size(94, 23);
      this.button2StyleSubs2.TabIndex = 1;
      this.button2StyleSubs2.Text = "&Subs2 St&yle...";
      this.toolTip1.SetToolTip(this.button2StyleSubs2, "Choose a style to use for the Subs2 portion of the dueling subtitles");
      this.button2StyleSubs2.UseVisualStyleBackColor = true;
      this.button2StyleSubs2.Click += new System.EventHandler(this.button2StyleSubs2_Click);
      // 
      // buttonStyleSubs1
      // 
      this.buttonStyleSubs1.Location = new System.Drawing.Point(7, 19);
      this.buttonStyleSubs1.Name = "buttonStyleSubs1";
      this.buttonStyleSubs1.Size = new System.Drawing.Size(94, 23);
      this.buttonStyleSubs1.TabIndex = 0;
      this.buttonStyleSubs1.Text = "Subs1 S&tyle...";
      this.toolTip1.SetToolTip(this.buttonStyleSubs1, "Choose a style to use for the Subs1 portion of the dueling subtitles");
      this.buttonStyleSubs1.UseVisualStyleBackColor = true;
      this.buttonStyleSubs1.Click += new System.EventHandler(this.buttonStyleSubs1_Click);
      // 
      // labelOutputDir
      // 
      this.labelOutputDir.AutoSize = true;
      this.labelOutputDir.Location = new System.Drawing.Point(9, 130);
      this.labelOutputDir.Name = "labelOutputDir";
      this.labelOutputDir.Size = new System.Drawing.Size(241, 13);
      this.labelOutputDir.TabIndex = 24;
      this.labelOutputDir.Text = "Directory where the generated files will be placed:";
      this.toolTip1.SetToolTip(this.labelOutputDir, "The dueling .ass subtitles will be placed into this directory");
      // 
      // labelSubs2File
      // 
      this.labelSubs2File.AutoSize = true;
      this.labelSubs2File.Location = new System.Drawing.Point(9, 90);
      this.labelSubs2File.Name = "labelSubs2File";
      this.labelSubs2File.Size = new System.Drawing.Size(264, 13);
      this.labelSubs2File.TabIndex = 22;
      this.labelSubs2File.Text = "The corresponding subtitle file in your native language:";
      this.toolTip1.SetToolTip(this.labelSubs2File, resources.GetString("labelSubs2File.ToolTip"));
      // 
      // textBoxOutputDir
      // 
      this.textBoxOutputDir.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.textBoxOutputDir.Location = new System.Drawing.Point(74, 146);
      this.textBoxOutputDir.Name = "textBoxOutputDir";
      this.textBoxOutputDir.Size = new System.Drawing.Size(359, 20);
      this.textBoxOutputDir.TabIndex = 7;
      this.toolTip1.SetToolTip(this.textBoxOutputDir, "The dueling .ass subtitles will be placed into this directory");
      this.textBoxOutputDir.Validating += new System.ComponentModel.CancelEventHandler(this.textBoxSubsOutputDir_Validating);
      // 
      // labelSubs1File
      // 
      this.labelSubs1File.AutoSize = true;
      this.labelSubs1File.Location = new System.Drawing.Point(9, 50);
      this.labelSubs1File.Name = "labelSubs1File";
      this.labelSubs1File.Size = new System.Drawing.Size(172, 13);
      this.labelSubs1File.TabIndex = 23;
      this.labelSubs1File.Text = "Subtitle file in your target language:";
      this.toolTip1.SetToolTip(this.labelSubs1File, resources.GetString("labelSubs1File.ToolTip"));
      // 
      // textBoxSubs2File
      // 
      this.textBoxSubs2File.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.textBoxSubs2File.Location = new System.Drawing.Point(74, 106);
      this.textBoxSubs2File.Name = "textBoxSubs2File";
      this.textBoxSubs2File.Size = new System.Drawing.Size(359, 20);
      this.textBoxSubs2File.TabIndex = 4;
      this.toolTip1.SetToolTip(this.textBoxSubs2File, resources.GetString("textBoxSubs2File.ToolTip"));
      this.textBoxSubs2File.TextChanged += new System.EventHandler(this.textBoxSubs2File_TextChanged);
      this.textBoxSubs2File.Validating += new System.ComponentModel.CancelEventHandler(this.textBoxSubs2File_Validating);
      // 
      // textBoxSubs1File
      // 
      this.textBoxSubs1File.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.textBoxSubs1File.BackColor = System.Drawing.SystemColors.Window;
      this.textBoxSubs1File.Location = new System.Drawing.Point(74, 66);
      this.textBoxSubs1File.Name = "textBoxSubs1File";
      this.textBoxSubs1File.Size = new System.Drawing.Size(359, 20);
      this.textBoxSubs1File.TabIndex = 1;
      this.toolTip1.SetToolTip(this.textBoxSubs1File, resources.GetString("textBoxSubs1File.ToolTip"));
      this.textBoxSubs1File.TextChanged += new System.EventHandler(this.textBoxSubs1File_TextChanged);
      this.textBoxSubs1File.Validating += new System.ComponentModel.CancelEventHandler(this.textBoxSubs1File_Validating);
      // 
      // labelPattern1
      // 
      this.labelPattern1.AutoSize = true;
      this.labelPattern1.Location = new System.Drawing.Point(9, 25);
      this.labelPattern1.Name = "labelPattern1";
      this.labelPattern1.Size = new System.Drawing.Size(149, 13);
      this.labelPattern1.TabIndex = 0;
      this.labelPattern1.Text = "Create a dueling subtitle every";
      this.toolTip1.SetToolTip(this.labelPattern1, resources.GetString("labelPattern1.ToolTip"));
      // 
      // labelPattern2
      // 
      this.labelPattern2.AutoSize = true;
      this.labelPattern2.Location = new System.Drawing.Point(209, 25);
      this.labelPattern2.Name = "labelPattern2";
      this.labelPattern2.Size = new System.Drawing.Size(34, 13);
      this.labelPattern2.TabIndex = 64;
      this.labelPattern2.Text = "line(s)";
      this.toolTip1.SetToolTip(this.labelPattern2, resources.GetString("labelPattern2.ToolTip"));
      // 
      // groupBoxName
      // 
      this.groupBoxName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.groupBoxName.Controls.Add(this.labelRequiredName);
      this.groupBoxName.Controls.Add(this.labelName);
      this.groupBoxName.Controls.Add(this.textBoxName);
      this.groupBoxName.Controls.Add(this.labelEpisodeStartNumber);
      this.groupBoxName.Controls.Add(this.numericUpDownEpisodeStartNumber);
      this.groupBoxName.Location = new System.Drawing.Point(12, 351);
      this.groupBoxName.Name = "groupBoxName";
      this.groupBoxName.Size = new System.Drawing.Size(570, 58);
      this.groupBoxName.TabIndex = 11;
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
      this.labelRequiredName.TabIndex = 112;
      this.labelRequiredName.Text = "(required)";
      // 
      // labelName
      // 
      this.labelName.AutoSize = true;
      this.labelName.Location = new System.Drawing.Point(4, 16);
      this.labelName.Name = "labelName";
      this.labelName.Size = new System.Drawing.Size(38, 13);
      this.labelName.TabIndex = 0;
      this.labelName.Text = "Name:";
      this.toolTip1.SetToolTip(this.labelName, "Name to be used in the filenames");
      // 
      // textBoxName
      // 
      this.textBoxName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.textBoxName.Location = new System.Drawing.Point(7, 32);
      this.textBoxName.Name = "textBoxName";
      this.textBoxName.Size = new System.Drawing.Size(467, 20);
      this.textBoxName.TabIndex = 1;
      this.toolTip1.SetToolTip(this.textBoxName, "Name to be used in the filenames");
      this.textBoxName.Validating += new System.ComponentModel.CancelEventHandler(this.textBoxName_Validating);
      // 
      // labelEpisodeStartNumber
      // 
      this.labelEpisodeStartNumber.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.labelEpisodeStartNumber.AutoSize = true;
      this.labelEpisodeStartNumber.Location = new System.Drawing.Point(495, 16);
      this.labelEpisodeStartNumber.Name = "labelEpisodeStartNumber";
      this.labelEpisodeStartNumber.Size = new System.Drawing.Size(70, 13);
      this.labelEpisodeStartNumber.TabIndex = 2;
      this.labelEpisodeStartNumber.Text = "First Episode:";
      this.toolTip1.SetToolTip(this.labelEpisodeStartNumber, "The episode number to start at for use in the filenames");
      // 
      // numericUpDownEpisodeStartNumber
      // 
      this.numericUpDownEpisodeStartNumber.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.numericUpDownEpisodeStartNumber.Location = new System.Drawing.Point(498, 32);
      this.numericUpDownEpisodeStartNumber.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
      this.numericUpDownEpisodeStartNumber.Name = "numericUpDownEpisodeStartNumber";
      this.numericUpDownEpisodeStartNumber.Size = new System.Drawing.Size(45, 20);
      this.numericUpDownEpisodeStartNumber.TabIndex = 3;
      this.toolTip1.SetToolTip(this.numericUpDownEpisodeStartNumber, "The episode number to start at for use in the filenames");
      this.numericUpDownEpisodeStartNumber.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
      // 
      // labelEncodingSubs1
      // 
      this.labelEncodingSubs1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.labelEncodingSubs1.AutoSize = true;
      this.labelEncodingSubs1.Location = new System.Drawing.Point(448, 49);
      this.labelEncodingSubs1.Name = "labelEncodingSubs1";
      this.labelEncodingSubs1.Size = new System.Drawing.Size(88, 13);
      this.labelEncodingSubs1.TabIndex = 37;
      this.labelEncodingSubs1.Text = "Subs1 Encoding:";
      this.toolTip1.SetToolTip(this.labelEncodingSubs1, "Encoding to use for Subs1");
      // 
      // labelEndodingSubs2
      // 
      this.labelEndodingSubs2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.labelEndodingSubs2.AutoSize = true;
      this.labelEndodingSubs2.Location = new System.Drawing.Point(448, 90);
      this.labelEndodingSubs2.Name = "labelEndodingSubs2";
      this.labelEndodingSubs2.Size = new System.Drawing.Size(88, 13);
      this.labelEndodingSubs2.TabIndex = 38;
      this.labelEndodingSubs2.Text = "Subs2 Encoding:";
      this.toolTip1.SetToolTip(this.labelEndodingSubs2, "Encoding to use for Subs2");
      // 
      // groupBoxRemoveStyledLines
      // 
      this.groupBoxRemoveStyledLines.Controls.Add(this.checkBoxRemoveStyledLinesSubs2);
      this.groupBoxRemoveStyledLines.Controls.Add(this.checkBoxRemoveStyledLinesSubs1);
      this.groupBoxRemoveStyledLines.Location = new System.Drawing.Point(435, 16);
      this.groupBoxRemoveStyledLines.Name = "groupBoxRemoveStyledLines";
      this.groupBoxRemoveStyledLines.Size = new System.Drawing.Size(126, 65);
      this.groupBoxRemoveStyledLines.TabIndex = 3;
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
      // groupBoxRemoveLinesWithoutCounterpart
      // 
      this.groupBoxRemoveLinesWithoutCounterpart.Controls.Add(this.checkBoxRemoveLinesWithoutCounterpartSubs2);
      this.groupBoxRemoveLinesWithoutCounterpart.Controls.Add(this.checkBoxRemoveLinesWithoutCounterpartSubs1);
      this.groupBoxRemoveLinesWithoutCounterpart.Location = new System.Drawing.Point(257, 16);
      this.groupBoxRemoveLinesWithoutCounterpart.Name = "groupBoxRemoveLinesWithoutCounterpart";
      this.groupBoxRemoveLinesWithoutCounterpart.Size = new System.Drawing.Size(172, 65);
      this.groupBoxRemoveLinesWithoutCounterpart.TabIndex = 3;
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
      // groupBoxTimeShift
      // 
      this.groupBoxTimeShift.Checked = false;
      this.groupBoxTimeShift.Controls.Add(this.labelTimeShiftSubs2Units);
      this.groupBoxTimeShift.Controls.Add(this.numericUpDownTimeShiftSubs2);
      this.groupBoxTimeShift.Controls.Add(this.labelTimeShiftSubs2);
      this.groupBoxTimeShift.Controls.Add(this.labelTimeShiftSubs1);
      this.groupBoxTimeShift.Controls.Add(this.numericUpDownTimeShiftSubs1);
      this.groupBoxTimeShift.Controls.Add(this.labelTimeShiftSubs1Units);
      this.groupBoxTimeShift.Location = new System.Drawing.Point(123, 16);
      this.groupBoxTimeShift.Name = "groupBoxTimeShift";
      this.groupBoxTimeShift.Padding = new System.Windows.Forms.Padding(0, 3, 0, 0);
      this.groupBoxTimeShift.Size = new System.Drawing.Size(128, 65);
      this.groupBoxTimeShift.TabIndex = 1;
      this.groupBoxTimeShift.TabStop = false;
      this.groupBoxTimeShift.Text = "Time Shift:";
      this.toolTip1.SetToolTip(this.groupBoxTimeShift, resources.GetString("groupBoxTimeShift.ToolTip"));
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
      // groupBoxOptionsSubs1
      // 
      this.groupBoxOptionsSubs1.Controls.Add(this.comboBoxPriority);
      this.groupBoxOptionsSubs1.Controls.Add(this.labelPriority);
      this.groupBoxOptionsSubs1.Controls.Add(this.button2StyleSubs2);
      this.groupBoxOptionsSubs1.Controls.Add(this.buttonStyleSubs1);
      this.groupBoxOptionsSubs1.Location = new System.Drawing.Point(12, 265);
      this.groupBoxOptionsSubs1.Name = "groupBoxOptionsSubs1";
      this.groupBoxOptionsSubs1.Size = new System.Drawing.Size(209, 80);
      this.groupBoxOptionsSubs1.TabIndex = 9;
      this.groupBoxOptionsSubs1.TabStop = false;
      this.groupBoxOptionsSubs1.Text = "Text Styles:";
      // 
      // groupBoxSubtitleOptions
      // 
      this.groupBoxSubtitleOptions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.groupBoxSubtitleOptions.Controls.Add(this.groupBoxRemoveLinesWithoutCounterpart);
      this.groupBoxSubtitleOptions.Controls.Add(this.groupBoxRemoveStyledLines);
      this.groupBoxSubtitleOptions.Controls.Add(this.groupBoxUseTimingsFrom);
      this.groupBoxSubtitleOptions.Controls.Add(this.groupBoxTimeShift);
      this.groupBoxSubtitleOptions.Location = new System.Drawing.Point(12, 173);
      this.groupBoxSubtitleOptions.Name = "groupBoxSubtitleOptions";
      this.groupBoxSubtitleOptions.Size = new System.Drawing.Size(570, 86);
      this.groupBoxSubtitleOptions.TabIndex = 8;
      this.groupBoxSubtitleOptions.TabStop = false;
      this.groupBoxSubtitleOptions.Text = "Subtitle Options:";
      // 
      // groupOtherOptions
      // 
      this.groupOtherOptions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.groupOtherOptions.Controls.Add(this.checkBoxQuickReference);
      this.groupOtherOptions.Controls.Add(this.labelPattern1);
      this.groupOtherOptions.Controls.Add(this.labelPattern2);
      this.groupOtherOptions.Controls.Add(this.numericUpDownPattern);
      this.groupOtherOptions.Location = new System.Drawing.Point(227, 265);
      this.groupOtherOptions.Name = "groupOtherOptions";
      this.groupOtherOptions.Size = new System.Drawing.Size(355, 80);
      this.groupOtherOptions.TabIndex = 10;
      this.groupOtherOptions.TabStop = false;
      this.groupOtherOptions.Text = "Dueling Options:";
      // 
      // comboBoxEncodingSubs1
      // 
      this.comboBoxEncodingSubs1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.comboBoxEncodingSubs1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboBoxEncodingSubs1.DropDownWidth = 250;
      this.comboBoxEncodingSubs1.FormattingEnabled = true;
      this.comboBoxEncodingSubs1.Location = new System.Drawing.Point(451, 65);
      this.comboBoxEncodingSubs1.Name = "comboBoxEncodingSubs1";
      this.comboBoxEncodingSubs1.Size = new System.Drawing.Size(131, 21);
      this.comboBoxEncodingSubs1.TabIndex = 2;
      // 
      // comboBoxEncodingSubs2
      // 
      this.comboBoxEncodingSubs2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.comboBoxEncodingSubs2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboBoxEncodingSubs2.DropDownWidth = 250;
      this.comboBoxEncodingSubs2.FormattingEnabled = true;
      this.comboBoxEncodingSubs2.Location = new System.Drawing.Point(451, 106);
      this.comboBoxEncodingSubs2.Name = "comboBoxEncodingSubs2";
      this.comboBoxEncodingSubs2.Size = new System.Drawing.Size(131, 21);
      this.comboBoxEncodingSubs2.TabIndex = 5;
      // 
      // labelRequiredSubs1
      // 
      this.labelRequiredSubs1.AutoSize = true;
      this.labelRequiredSubs1.ForeColor = System.Drawing.Color.Red;
      this.labelRequiredSubs1.Location = new System.Drawing.Point(183, 50);
      this.labelRequiredSubs1.Name = "labelRequiredSubs1";
      this.labelRequiredSubs1.Size = new System.Drawing.Size(51, 13);
      this.labelRequiredSubs1.TabIndex = 111;
      this.labelRequiredSubs1.Text = "(required)";
      // 
      // labelRequiredSubs2
      // 
      this.labelRequiredSubs2.AutoSize = true;
      this.labelRequiredSubs2.ForeColor = System.Drawing.Color.Red;
      this.labelRequiredSubs2.Location = new System.Drawing.Point(275, 90);
      this.labelRequiredSubs2.Name = "labelRequiredSubs2";
      this.labelRequiredSubs2.Size = new System.Drawing.Size(51, 13);
      this.labelRequiredSubs2.TabIndex = 112;
      this.labelRequiredSubs2.Text = "(required)";
      // 
      // labelRequiredOutput
      // 
      this.labelRequiredOutput.AutoSize = true;
      this.labelRequiredOutput.ForeColor = System.Drawing.Color.Red;
      this.labelRequiredOutput.Location = new System.Drawing.Point(252, 130);
      this.labelRequiredOutput.Name = "labelRequiredOutput";
      this.labelRequiredOutput.Size = new System.Drawing.Size(51, 13);
      this.labelRequiredOutput.TabIndex = 112;
      this.labelRequiredOutput.Text = "(required)";
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
      this.textBoxHelp.Size = new System.Drawing.Size(570, 31);
      this.textBoxHelp.TabIndex = 113;
      this.textBoxHelp.TabStop = false;
      this.textBoxHelp.Text = "Create a subtitle file that will simultaneously display a line from Subs1 and its" +
          " corresponding line from Subs2.\r\nNote: This tool only supports text-based subtit" +
          "les (.ass/.ssa/.srt).";
      this.textBoxHelp.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
      // 
      // DialogDuelingSubtitles
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.buttonCancel;
      this.ClientSize = new System.Drawing.Size(594, 462);
      this.Controls.Add(this.textBoxHelp);
      this.Controls.Add(this.labelRequiredOutput);
      this.Controls.Add(this.labelRequiredSubs2);
      this.Controls.Add(this.labelRequiredSubs1);
      this.Controls.Add(this.comboBoxEncodingSubs1);
      this.Controls.Add(this.labelEncodingSubs1);
      this.Controls.Add(this.labelEndodingSubs2);
      this.Controls.Add(this.comboBoxEncodingSubs2);
      this.Controls.Add(this.groupBoxName);
      this.Controls.Add(this.groupOtherOptions);
      this.Controls.Add(this.groupBoxSubtitleOptions);
      this.Controls.Add(this.labelOutputDir);
      this.Controls.Add(this.buttonOutputDir);
      this.Controls.Add(this.labelSubs2File);
      this.Controls.Add(this.textBoxOutputDir);
      this.Controls.Add(this.labelSubs1File);
      this.Controls.Add(this.buttonSubs2File);
      this.Controls.Add(this.textBoxSubs2File);
      this.Controls.Add(this.buttonSubs1File);
      this.Controls.Add(this.textBoxSubs1File);
      this.Controls.Add(this.groupBoxOptionsSubs1);
      this.Controls.Add(this.buttonCreate);
      this.Controls.Add(this.buttonCancel);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.MaximizeBox = false;
      this.MaximumSize = new System.Drawing.Size(600, 490);
      this.MinimizeBox = false;
      this.MinimumSize = new System.Drawing.Size(600, 490);
      this.Name = "DialogDuelingSubtitles";
      this.Text = "Dueling Subtitles Tool";
      this.Load += new System.EventHandler(this.DialogDuelingSubtitles_Load);
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DialogDuelingSubtitles_FormClosing);
      ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
      this.groupBoxUseTimingsFrom.ResumeLayout(false);
      this.groupBoxUseTimingsFrom.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPattern)).EndInit();
      this.groupBoxName.ResumeLayout(false);
      this.groupBoxName.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownEpisodeStartNumber)).EndInit();
      this.groupBoxRemoveStyledLines.ResumeLayout(false);
      this.groupBoxRemoveStyledLines.PerformLayout();
      this.groupBoxRemoveLinesWithoutCounterpart.ResumeLayout(false);
      this.groupBoxRemoveLinesWithoutCounterpart.PerformLayout();
      this.groupBoxTimeShift.ResumeLayout(false);
      this.groupBoxTimeShift.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownTimeShiftSubs2)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownTimeShiftSubs1)).EndInit();
      this.groupBoxOptionsSubs1.ResumeLayout(false);
      this.groupBoxOptionsSubs1.PerformLayout();
      this.groupBoxSubtitleOptions.ResumeLayout(false);
      this.groupOtherOptions.ResumeLayout(false);
      this.groupOtherOptions.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Button buttonCancel;
    private System.Windows.Forms.Button buttonCreate;
    private System.Windows.Forms.ErrorProvider errorProvider1;
    private System.Windows.Forms.ToolTip toolTip1;
    private System.Windows.Forms.GroupBox groupBoxOptionsSubs1;
    private System.Windows.Forms.Label labelOutputDir;
    private System.Windows.Forms.Button buttonOutputDir;
    private System.Windows.Forms.Label labelSubs2File;
    private System.Windows.Forms.TextBox textBoxOutputDir;
    private System.Windows.Forms.Label labelSubs1File;
    private System.Windows.Forms.Button buttonSubs2File;
    private System.Windows.Forms.TextBox textBoxSubs2File;
    private System.Windows.Forms.Button buttonSubs1File;
    private System.Windows.Forms.TextBox textBoxSubs1File;
    private GroupBoxCheck groupBoxTimeShift;
    private System.Windows.Forms.Label labelTimeShiftSubs2Units;
    private System.Windows.Forms.NumericUpDown numericUpDownTimeShiftSubs2;
    private System.Windows.Forms.Label labelTimeShiftSubs2;
    private System.Windows.Forms.Label labelTimeShiftSubs1;
    private System.Windows.Forms.NumericUpDown numericUpDownTimeShiftSubs1;
    private System.Windows.Forms.Label labelTimeShiftSubs1Units;
    private System.Windows.Forms.GroupBox groupBoxUseTimingsFrom;
    private System.Windows.Forms.RadioButton radioButtonTimingSubs1;
    private System.Windows.Forms.RadioButton radioButtonTimingSubs2;
    private System.Windows.Forms.GroupBox groupBoxSubtitleOptions;
    private System.Windows.Forms.NumericUpDown numericUpDownPattern;
    private System.Windows.Forms.Button button2StyleSubs2;
    private System.Windows.Forms.Button buttonStyleSubs1;
    private System.Windows.Forms.GroupBox groupOtherOptions;
    private System.Windows.Forms.Label labelPattern1;
    private System.Windows.Forms.Label labelPattern2;
    private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
    private System.Windows.Forms.ComboBox comboBoxPriority;
    private System.Windows.Forms.Label labelPriority;
    private System.Windows.Forms.CheckBox checkBoxQuickReference;
    private System.Windows.Forms.OpenFileDialog openFileDialog;
    private System.Windows.Forms.GroupBox groupBoxName;
    private System.Windows.Forms.Label labelName;
    private System.Windows.Forms.TextBox textBoxName;
    private System.Windows.Forms.Label labelEpisodeStartNumber;
    private System.Windows.Forms.NumericUpDown numericUpDownEpisodeStartNumber;
    private System.Windows.Forms.ComboBox comboBoxEncodingSubs1;
    private System.Windows.Forms.Label labelEncodingSubs1;
    private System.Windows.Forms.Label labelEndodingSubs2;
    private System.Windows.Forms.ComboBox comboBoxEncodingSubs2;
    private System.Windows.Forms.Label labelRequiredOutput;
    private System.Windows.Forms.Label labelRequiredSubs2;
    private System.Windows.Forms.Label labelRequiredSubs1;
    private System.Windows.Forms.Label labelRequiredName;
    private System.Windows.Forms.GroupBox groupBoxRemoveStyledLines;
    private System.Windows.Forms.CheckBox checkBoxRemoveStyledLinesSubs2;
    private System.Windows.Forms.CheckBox checkBoxRemoveStyledLinesSubs1;
    private System.Windows.Forms.TextBox textBoxHelp;
    private System.Windows.Forms.GroupBox groupBoxRemoveLinesWithoutCounterpart;
    private System.Windows.Forms.CheckBox checkBoxRemoveLinesWithoutCounterpartSubs2;
    private System.Windows.Forms.CheckBox checkBoxRemoveLinesWithoutCounterpartSubs1;

  }
}