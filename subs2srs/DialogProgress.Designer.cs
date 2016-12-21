namespace subs2srs
{
	partial class DialogProgress
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose( bool disposing )
		{
			if( disposing && ( components != null ) )
			{
				components.Dispose();
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
      this.bnCancel = new System.Windows.Forms.Button();
      this.labelDesc = new System.Windows.Forms.Label();
      this.progressBarMain = new System.Windows.Forms.ProgressBar();
      this.progressBarDetailed = new System.Windows.Forms.ProgressBar();
      this.labelStatsTimeProcessed = new System.Windows.Forms.Label();
      this.labelStatsTimeRemaining = new System.Windows.Forms.Label();
      this.labelStatsFps = new System.Windows.Forms.Label();
      this.labelStatsFrame = new System.Windows.Forms.Label();
      this.labelStatsTimeRemainingValue = new System.Windows.Forms.Label();
      this.labelStatsFrameValue = new System.Windows.Forms.Label();
      this.labelStatsTimeProcessedValue = new System.Windows.Forms.Label();
      this.labelStatsFpsValue = new System.Windows.Forms.Label();
      this.SuspendLayout();
      // 
      // bnCancel
      // 
      this.bnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.bnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.bnCancel.Location = new System.Drawing.Point(332, 106);
      this.bnCancel.Name = "bnCancel";
      this.bnCancel.Size = new System.Drawing.Size(75, 23);
      this.bnCancel.TabIndex = 0;
      this.bnCancel.Text = "Cancel";
      this.bnCancel.UseVisualStyleBackColor = true;
      // 
      // labelDesc
      // 
      this.labelDesc.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.labelDesc.AutoEllipsis = true;
      this.labelDesc.Location = new System.Drawing.Point(12, 19);
      this.labelDesc.Name = "labelDesc";
      this.labelDesc.Size = new System.Drawing.Size(395, 26);
      this.labelDesc.TabIndex = 1;
      this.labelDesc.Text = "Processing...";
      // 
      // progressBarMain
      // 
      this.progressBarMain.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.progressBarMain.Location = new System.Drawing.Point(12, 48);
      this.progressBarMain.Name = "progressBarMain";
      this.progressBarMain.Size = new System.Drawing.Size(395, 17);
      this.progressBarMain.TabIndex = 2;
      // 
      // progressBarDetailed
      // 
      this.progressBarDetailed.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.progressBarDetailed.Location = new System.Drawing.Point(12, 71);
      this.progressBarDetailed.Name = "progressBarDetailed";
      this.progressBarDetailed.Size = new System.Drawing.Size(395, 17);
      this.progressBarDetailed.TabIndex = 3;
      this.progressBarDetailed.Visible = false;
      // 
      // labelStatsTimeProcessed
      // 
      this.labelStatsTimeProcessed.AutoSize = true;
      this.labelStatsTimeProcessed.Location = new System.Drawing.Point(12, 104);
      this.labelStatsTimeProcessed.Name = "labelStatsTimeProcessed";
      this.labelStatsTimeProcessed.Size = new System.Drawing.Size(60, 13);
      this.labelStatsTimeProcessed.TabIndex = 4;
      this.labelStatsTimeProcessed.Text = "Processed:";
      this.labelStatsTimeProcessed.Visible = false;
      // 
      // labelStatsTimeRemaining
      // 
      this.labelStatsTimeRemaining.AutoSize = true;
      this.labelStatsTimeRemaining.Location = new System.Drawing.Point(12, 121);
      this.labelStatsTimeRemaining.Name = "labelStatsTimeRemaining";
      this.labelStatsTimeRemaining.Size = new System.Drawing.Size(60, 13);
      this.labelStatsTimeRemaining.TabIndex = 4;
      this.labelStatsTimeRemaining.Text = "Remaining:";
      this.labelStatsTimeRemaining.Visible = false;
      // 
      // labelStatsFps
      // 
      this.labelStatsFps.AutoSize = true;
      this.labelStatsFps.Location = new System.Drawing.Point(162, 121);
      this.labelStatsFps.Name = "labelStatsFps";
      this.labelStatsFps.Size = new System.Drawing.Size(33, 13);
      this.labelStatsFps.TabIndex = 4;
      this.labelStatsFps.Text = "FPS: ";
      this.labelStatsFps.Visible = false;
      // 
      // labelStatsFrame
      // 
      this.labelStatsFrame.AutoSize = true;
      this.labelStatsFrame.Location = new System.Drawing.Point(162, 104);
      this.labelStatsFrame.Name = "labelStatsFrame";
      this.labelStatsFrame.Size = new System.Drawing.Size(39, 13);
      this.labelStatsFrame.TabIndex = 4;
      this.labelStatsFrame.Text = "Frame:";
      this.labelStatsFrame.Visible = false;
      // 
      // labelStatsTimeRemainingValue
      // 
      this.labelStatsTimeRemainingValue.AutoSize = true;
      this.labelStatsTimeRemainingValue.Location = new System.Drawing.Point(78, 121);
      this.labelStatsTimeRemainingValue.Name = "labelStatsTimeRemainingValue";
      this.labelStatsTimeRemainingValue.Size = new System.Drawing.Size(49, 13);
      this.labelStatsTimeRemainingValue.TabIndex = 4;
      this.labelStatsTimeRemainingValue.Text = "00:06:43";
      this.labelStatsTimeRemainingValue.Visible = false;
      // 
      // labelStatsFrameValue
      // 
      this.labelStatsFrameValue.AutoSize = true;
      this.labelStatsFrameValue.Location = new System.Drawing.Point(207, 104);
      this.labelStatsFrameValue.Name = "labelStatsFrameValue";
      this.labelStatsFrameValue.Size = new System.Drawing.Size(31, 13);
      this.labelStatsFrameValue.TabIndex = 4;
      this.labelStatsFrameValue.Text = "1234";
      this.labelStatsFrameValue.Visible = false;
      // 
      // labelStatsTimeProcessedValue
      // 
      this.labelStatsTimeProcessedValue.AutoSize = true;
      this.labelStatsTimeProcessedValue.Location = new System.Drawing.Point(78, 104);
      this.labelStatsTimeProcessedValue.Name = "labelStatsTimeProcessedValue";
      this.labelStatsTimeProcessedValue.Size = new System.Drawing.Size(49, 13);
      this.labelStatsTimeProcessedValue.TabIndex = 4;
      this.labelStatsTimeProcessedValue.Text = "00:12:34";
      this.labelStatsTimeProcessedValue.Visible = false;
      // 
      // labelStatsFpsValue
      // 
      this.labelStatsFpsValue.AutoSize = true;
      this.labelStatsFpsValue.Location = new System.Drawing.Point(207, 121);
      this.labelStatsFpsValue.Name = "labelStatsFpsValue";
      this.labelStatsFpsValue.Size = new System.Drawing.Size(19, 13);
      this.labelStatsFpsValue.TabIndex = 4;
      this.labelStatsFpsValue.Text = "57";
      this.labelStatsFpsValue.Visible = false;
      // 
      // DialogProgress
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.BackColor = System.Drawing.SystemColors.Control;
      this.CancelButton = this.bnCancel;
      this.ClientSize = new System.Drawing.Size(419, 141);
      this.Controls.Add(this.labelStatsTimeRemaining);
      this.Controls.Add(this.labelStatsFrame);
      this.Controls.Add(this.labelStatsFps);
      this.Controls.Add(this.labelStatsFpsValue);
      this.Controls.Add(this.labelStatsTimeRemainingValue);
      this.Controls.Add(this.labelStatsFrameValue);
      this.Controls.Add(this.labelStatsTimeProcessedValue);
      this.Controls.Add(this.labelStatsTimeProcessed);
      this.Controls.Add(this.progressBarDetailed);
      this.Controls.Add(this.progressBarMain);
      this.Controls.Add(this.labelDesc);
      this.Controls.Add(this.bnCancel);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "DialogProgress";
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "Progress";
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormProgress_FormClosing);
      this.ResumeLayout(false);
      this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button bnCancel;
		public System.Windows.Forms.Label labelDesc;
		public System.Windows.Forms.ProgressBar progressBarMain;
    public System.Windows.Forms.ProgressBar progressBarDetailed;
    private System.Windows.Forms.Label labelStatsTimeProcessed;
    private System.Windows.Forms.Label labelStatsTimeRemaining;
    private System.Windows.Forms.Label labelStatsFps;
    private System.Windows.Forms.Label labelStatsFrame;
    private System.Windows.Forms.Label labelStatsTimeRemainingValue;
    private System.Windows.Forms.Label labelStatsFrameValue;
    private System.Windows.Forms.Label labelStatsTimeProcessedValue;
    private System.Windows.Forms.Label labelStatsFpsValue;
	}
}