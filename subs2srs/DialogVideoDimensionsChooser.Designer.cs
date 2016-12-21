namespace subs2srs
{
  partial class DialogVideoDimensionsChooser
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
      this.buttonCancel = new System.Windows.Forms.Button();
      this.buttonOK = new System.Windows.Forms.Button();
      this.numericUpDownPercent = new System.Windows.Forms.NumericUpDown();
      this.labelPercentUnits = new System.Windows.Forms.Label();
      this.numericUpDownOrigWidth = new System.Windows.Forms.NumericUpDown();
      this.numericUpDownOrigHeight = new System.Windows.Forms.NumericUpDown();
      this.textBoxNewWidth = new System.Windows.Forms.TextBox();
      this.textBoxNewHeight = new System.Windows.Forms.TextBox();
      this.labelOrigWidthUnits = new System.Windows.Forms.Label();
      this.labelOrigHeightUnits = new System.Windows.Forms.Label();
      this.labelVideoWidth = new System.Windows.Forms.Label();
      this.labelVideoHeight = new System.Windows.Forms.Label();
      this.labelNewWidth = new System.Windows.Forms.Label();
      this.labelNewHeight = new System.Windows.Forms.Label();
      this.labelNewHeightUnits = new System.Windows.Forms.Label();
      this.labelNewWidthUnits = new System.Windows.Forms.Label();
      this.labelOf = new System.Windows.Forms.Label();
      this.labelIs = new System.Windows.Forms.Label();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPercent)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownOrigWidth)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownOrigHeight)).BeginInit();
      this.SuspendLayout();
      // 
      // buttonCancel
      // 
      this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonCancel.CausesValidation = false;
      this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.buttonCancel.Location = new System.Drawing.Point(379, 83);
      this.buttonCancel.Name = "buttonCancel";
      this.buttonCancel.Size = new System.Drawing.Size(77, 23);
      this.buttonCancel.TabIndex = 3;
      this.buttonCancel.Text = "Cancel";
      this.buttonCancel.UseVisualStyleBackColor = true;
      // 
      // buttonOK
      // 
      this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.buttonOK.Location = new System.Drawing.Point(296, 83);
      this.buttonOK.Name = "buttonOK";
      this.buttonOK.Size = new System.Drawing.Size(77, 23);
      this.buttonOK.TabIndex = 2;
      this.buttonOK.Text = "OK";
      this.buttonOK.UseVisualStyleBackColor = true;
      // 
      // numericUpDownPercent
      // 
      this.numericUpDownPercent.Increment = new decimal(new int[] {
            5,
            0,
            0,
            0});
      this.numericUpDownPercent.Location = new System.Drawing.Point(12, 31);
      this.numericUpDownPercent.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            0});
      this.numericUpDownPercent.Name = "numericUpDownPercent";
      this.numericUpDownPercent.Size = new System.Drawing.Size(48, 20);
      this.numericUpDownPercent.TabIndex = 4;
      this.numericUpDownPercent.Value = new decimal(new int[] {
            35,
            0,
            0,
            0});
      this.numericUpDownPercent.ValueChanged += new System.EventHandler(this.recomuputeDimesions);
      // 
      // labelPercentUnits
      // 
      this.labelPercentUnits.AutoSize = true;
      this.labelPercentUnits.Location = new System.Drawing.Point(63, 34);
      this.labelPercentUnits.Name = "labelPercentUnits";
      this.labelPercentUnits.Size = new System.Drawing.Size(15, 13);
      this.labelPercentUnits.TabIndex = 5;
      this.labelPercentUnits.Text = "%";
      // 
      // numericUpDownOrigWidth
      // 
      this.numericUpDownOrigWidth.Location = new System.Drawing.Point(203, 19);
      this.numericUpDownOrigWidth.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
      this.numericUpDownOrigWidth.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            0});
      this.numericUpDownOrigWidth.Name = "numericUpDownOrigWidth";
      this.numericUpDownOrigWidth.Size = new System.Drawing.Size(48, 20);
      this.numericUpDownOrigWidth.TabIndex = 6;
      this.numericUpDownOrigWidth.Value = new decimal(new int[] {
            720,
            0,
            0,
            0});
      this.numericUpDownOrigWidth.ValueChanged += new System.EventHandler(this.recomuputeDimesions);
      // 
      // numericUpDownOrigHeight
      // 
      this.numericUpDownOrigHeight.Location = new System.Drawing.Point(203, 45);
      this.numericUpDownOrigHeight.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
      this.numericUpDownOrigHeight.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            0});
      this.numericUpDownOrigHeight.Name = "numericUpDownOrigHeight";
      this.numericUpDownOrigHeight.Size = new System.Drawing.Size(48, 20);
      this.numericUpDownOrigHeight.TabIndex = 7;
      this.numericUpDownOrigHeight.Value = new decimal(new int[] {
            480,
            0,
            0,
            0});
      this.numericUpDownOrigHeight.ValueChanged += new System.EventHandler(this.recomuputeDimesions);
      // 
      // textBoxNewWidth
      // 
      this.textBoxNewWidth.Location = new System.Drawing.Point(389, 19);
      this.textBoxNewWidth.Name = "textBoxNewWidth";
      this.textBoxNewWidth.ReadOnly = true;
      this.textBoxNewWidth.Size = new System.Drawing.Size(48, 20);
      this.textBoxNewWidth.TabIndex = 8;
      // 
      // textBoxNewHeight
      // 
      this.textBoxNewHeight.Location = new System.Drawing.Point(389, 45);
      this.textBoxNewHeight.Name = "textBoxNewHeight";
      this.textBoxNewHeight.ReadOnly = true;
      this.textBoxNewHeight.Size = new System.Drawing.Size(48, 20);
      this.textBoxNewHeight.TabIndex = 9;
      // 
      // labelOrigWidthUnits
      // 
      this.labelOrigWidthUnits.AutoSize = true;
      this.labelOrigWidthUnits.Location = new System.Drawing.Point(254, 21);
      this.labelOrigWidthUnits.Name = "labelOrigWidthUnits";
      this.labelOrigWidthUnits.Size = new System.Drawing.Size(18, 13);
      this.labelOrigWidthUnits.TabIndex = 11;
      this.labelOrigWidthUnits.Text = "px";
      // 
      // labelOrigHeightUnits
      // 
      this.labelOrigHeightUnits.AutoSize = true;
      this.labelOrigHeightUnits.Location = new System.Drawing.Point(254, 47);
      this.labelOrigHeightUnits.Name = "labelOrigHeightUnits";
      this.labelOrigHeightUnits.Size = new System.Drawing.Size(18, 13);
      this.labelOrigHeightUnits.TabIndex = 12;
      this.labelOrigHeightUnits.Text = "px";
      // 
      // labelVideoWidth
      // 
      this.labelVideoWidth.AutoSize = true;
      this.labelVideoWidth.Location = new System.Drawing.Point(132, 21);
      this.labelVideoWidth.Name = "labelVideoWidth";
      this.labelVideoWidth.Size = new System.Drawing.Size(68, 13);
      this.labelVideoWidth.TabIndex = 13;
      this.labelVideoWidth.Text = "Video Width:";
      // 
      // labelVideoHeight
      // 
      this.labelVideoHeight.AutoSize = true;
      this.labelVideoHeight.Location = new System.Drawing.Point(129, 47);
      this.labelVideoHeight.Name = "labelVideoHeight";
      this.labelVideoHeight.Size = new System.Drawing.Size(71, 13);
      this.labelVideoHeight.TabIndex = 14;
      this.labelVideoHeight.Text = "Video Height:";
      // 
      // labelNewWidth
      // 
      this.labelNewWidth.AutoSize = true;
      this.labelNewWidth.Location = new System.Drawing.Point(323, 21);
      this.labelNewWidth.Name = "labelNewWidth";
      this.labelNewWidth.Size = new System.Drawing.Size(63, 13);
      this.labelNewWidth.TabIndex = 16;
      this.labelNewWidth.Text = "New Width:";
      // 
      // labelNewHeight
      // 
      this.labelNewHeight.AutoSize = true;
      this.labelNewHeight.Location = new System.Drawing.Point(320, 47);
      this.labelNewHeight.Name = "labelNewHeight";
      this.labelNewHeight.Size = new System.Drawing.Size(66, 13);
      this.labelNewHeight.TabIndex = 17;
      this.labelNewHeight.Text = "New Height:";
      // 
      // labelNewHeightUnits
      // 
      this.labelNewHeightUnits.AutoSize = true;
      this.labelNewHeightUnits.Location = new System.Drawing.Point(440, 47);
      this.labelNewHeightUnits.Name = "labelNewHeightUnits";
      this.labelNewHeightUnits.Size = new System.Drawing.Size(18, 13);
      this.labelNewHeightUnits.TabIndex = 19;
      this.labelNewHeightUnits.Text = "px";
      // 
      // labelNewWidthUnits
      // 
      this.labelNewWidthUnits.AutoSize = true;
      this.labelNewWidthUnits.Location = new System.Drawing.Point(440, 21);
      this.labelNewWidthUnits.Name = "labelNewWidthUnits";
      this.labelNewWidthUnits.Size = new System.Drawing.Size(18, 13);
      this.labelNewWidthUnits.TabIndex = 18;
      this.labelNewWidthUnits.Text = "px";
      // 
      // labelOf
      // 
      this.labelOf.AutoSize = true;
      this.labelOf.Location = new System.Drawing.Point(96, 34);
      this.labelOf.Name = "labelOf";
      this.labelOf.Size = new System.Drawing.Size(16, 13);
      this.labelOf.TabIndex = 20;
      this.labelOf.Text = "of";
      // 
      // labelIs
      // 
      this.labelIs.AutoSize = true;
      this.labelIs.Location = new System.Drawing.Point(290, 33);
      this.labelIs.Name = "labelIs";
      this.labelIs.Size = new System.Drawing.Size(14, 13);
      this.labelIs.TabIndex = 21;
      this.labelIs.Text = "is";
      // 
      // DialogVideoDimensionsChooser
      // 
      this.AcceptButton = this.buttonOK;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.buttonCancel;
      this.ClientSize = new System.Drawing.Size(468, 118);
      this.Controls.Add(this.labelIs);
      this.Controls.Add(this.labelOf);
      this.Controls.Add(this.labelNewHeightUnits);
      this.Controls.Add(this.labelNewWidthUnits);
      this.Controls.Add(this.labelNewHeight);
      this.Controls.Add(this.labelNewWidth);
      this.Controls.Add(this.labelVideoHeight);
      this.Controls.Add(this.labelVideoWidth);
      this.Controls.Add(this.labelOrigHeightUnits);
      this.Controls.Add(this.labelOrigWidthUnits);
      this.Controls.Add(this.textBoxNewHeight);
      this.Controls.Add(this.textBoxNewWidth);
      this.Controls.Add(this.numericUpDownOrigHeight);
      this.Controls.Add(this.numericUpDownOrigWidth);
      this.Controls.Add(this.labelPercentUnits);
      this.Controls.Add(this.numericUpDownPercent);
      this.Controls.Add(this.buttonCancel);
      this.Controls.Add(this.buttonOK);
      this.MaximizeBox = false;
      this.MaximumSize = new System.Drawing.Size(476, 152);
      this.MinimizeBox = false;
      this.MinimumSize = new System.Drawing.Size(476, 152);
      this.Name = "DialogVideoDimensionsChooser";
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
      this.Text = "Video Clip Dimensions Chooser";
      this.Load += new System.EventHandler(this.recomuputeDimesions);
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPercent)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownOrigWidth)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownOrigHeight)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Button buttonCancel;
    private System.Windows.Forms.Button buttonOK;
    private System.Windows.Forms.NumericUpDown numericUpDownPercent;
    private System.Windows.Forms.Label labelPercentUnits;
    private System.Windows.Forms.NumericUpDown numericUpDownOrigWidth;
    private System.Windows.Forms.NumericUpDown numericUpDownOrigHeight;
    private System.Windows.Forms.TextBox textBoxNewWidth;
    private System.Windows.Forms.TextBox textBoxNewHeight;
    private System.Windows.Forms.Label labelOrigWidthUnits;
    private System.Windows.Forms.Label labelOrigHeightUnits;
    private System.Windows.Forms.Label labelVideoWidth;
    private System.Windows.Forms.Label labelVideoHeight;
    private System.Windows.Forms.Label labelNewWidth;
    private System.Windows.Forms.Label labelNewHeight;
    private System.Windows.Forms.Label labelNewHeightUnits;
    private System.Windows.Forms.Label labelNewWidthUnits;
    private System.Windows.Forms.Label labelOf;
    private System.Windows.Forms.Label labelIs;
  }
}