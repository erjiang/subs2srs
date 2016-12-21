namespace subs2srs
{
  partial class DialogPreviewSnapshot
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DialogPreviewSnapshot));
      this.pictureBoxPreview = new System.Windows.Forms.PictureBox();
      ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPreview)).BeginInit();
      this.SuspendLayout();
      // 
      // pictureBoxPreview
      // 
      this.pictureBoxPreview.Dock = System.Windows.Forms.DockStyle.Fill;
      this.pictureBoxPreview.Location = new System.Drawing.Point(0, 0);
      this.pictureBoxPreview.Name = "pictureBoxPreview";
      this.pictureBoxPreview.Size = new System.Drawing.Size(292, 266);
      this.pictureBoxPreview.TabIndex = 0;
      this.pictureBoxPreview.TabStop = false;
      this.pictureBoxPreview.Click += new System.EventHandler(this.pictureBoxPreview_Click);
      // 
      // DialogPreviewSnapshot
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(292, 266);
      this.Controls.Add(this.pictureBoxPreview);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "DialogPreviewSnapshot";
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.Text = "Snapshot Preview";
      ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPreview)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.PictureBox pictureBoxPreview;
  }
}