namespace subs2srs
{
  partial class DialogAbout
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DialogAbout));
      this.logoPictureBox = new System.Windows.Forms.PictureBox();
      this.textBoxDescription = new System.Windows.Forms.TextBox();
      this.okButton = new System.Windows.Forms.Button();
      this.linkLabelContact = new System.Windows.Forms.LinkLabel();
      this.linkLabelWebsite = new System.Windows.Forms.LinkLabel();
      this.labelWebsite = new System.Windows.Forms.Label();
      this.labelContact = new System.Windows.Forms.Label();
      this.labelProjectName = new System.Windows.Forms.Label();
      this.labelVersionHeader = new System.Windows.Forms.Label();
      this.labelAuthorHeader = new System.Windows.Forms.Label();
      this.labelAuthor = new System.Windows.Forms.Label();
      this.labelVersion = new System.Windows.Forms.Label();
      ((System.ComponentModel.ISupportInitialize)(this.logoPictureBox)).BeginInit();
      this.SuspendLayout();
      // 
      // logoPictureBox
      // 
      this.logoPictureBox.BackColor = System.Drawing.Color.White;
      this.logoPictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.logoPictureBox.Image = global::subs2srs.Properties.Resources.About;
      this.logoPictureBox.Location = new System.Drawing.Point(12, 12);
      this.logoPictureBox.Name = "logoPictureBox";
      this.logoPictureBox.Size = new System.Drawing.Size(131, 259);
      this.logoPictureBox.TabIndex = 12;
      this.logoPictureBox.TabStop = false;
      // 
      // textBoxDescription
      // 
      this.textBoxDescription.BackColor = System.Drawing.SystemColors.Control;
      this.textBoxDescription.Location = new System.Drawing.Point(155, 137);
      this.textBoxDescription.Margin = new System.Windows.Forms.Padding(6, 3, 3, 3);
      this.textBoxDescription.Multiline = true;
      this.textBoxDescription.Name = "textBoxDescription";
      this.textBoxDescription.ReadOnly = true;
      this.textBoxDescription.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
      this.textBoxDescription.Size = new System.Drawing.Size(271, 134);
      this.textBoxDescription.TabIndex = 2;
      this.textBoxDescription.TabStop = false;
      this.textBoxDescription.Text = resources.GetString("textBoxDescription.Text");
      // 
      // okButton
      // 
      this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.okButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.okButton.Location = new System.Drawing.Point(353, 281);
      this.okButton.Name = "okButton";
      this.okButton.Size = new System.Drawing.Size(75, 22);
      this.okButton.TabIndex = 3;
      this.okButton.Text = "&OK";
      // 
      // linkLabelContact
      // 
      this.linkLabelContact.AutoSize = true;
      this.linkLabelContact.Location = new System.Drawing.Point(205, 81);
      this.linkLabelContact.Name = "linkLabelContact";
      this.linkLabelContact.Size = new System.Drawing.Size(101, 13);
      this.linkLabelContact.TabIndex = 0;
      this.linkLabelContact.TabStop = true;
      this.linkLabelContact.Text = "cb4960@gmail.com";
      this.linkLabelContact.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelContact_LinkClicked);
      // 
      // linkLabelWebsite
      // 
      this.linkLabelWebsite.AutoSize = true;
      this.linkLabelWebsite.Location = new System.Drawing.Point(205, 104);
      this.linkLabelWebsite.Name = "linkLabelWebsite";
      this.linkLabelWebsite.Size = new System.Drawing.Size(43, 13);
      this.linkLabelWebsite.TabIndex = 1;
      this.linkLabelWebsite.TabStop = true;
      this.linkLabelWebsite.Text = "website";
      this.linkLabelWebsite.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelWebsite_LinkClicked);
      // 
      // labelWebsite
      // 
      this.labelWebsite.AutoSize = true;
      this.labelWebsite.Location = new System.Drawing.Point(152, 104);
      this.labelWebsite.Name = "labelWebsite";
      this.labelWebsite.Size = new System.Drawing.Size(49, 13);
      this.labelWebsite.TabIndex = 29;
      this.labelWebsite.Text = "Website:";
      // 
      // labelContact
      // 
      this.labelContact.AutoSize = true;
      this.labelContact.Location = new System.Drawing.Point(152, 81);
      this.labelContact.Name = "labelContact";
      this.labelContact.Size = new System.Drawing.Size(47, 13);
      this.labelContact.TabIndex = 30;
      this.labelContact.Text = "Contact:";
      // 
      // labelProjectName
      // 
      this.labelProjectName.AutoSize = true;
      this.labelProjectName.Location = new System.Drawing.Point(152, 12);
      this.labelProjectName.Name = "labelProjectName";
      this.labelProjectName.Size = new System.Drawing.Size(68, 13);
      this.labelProjectName.TabIndex = 31;
      this.labelProjectName.Text = "ProjectName";
      // 
      // labelVersionHeader
      // 
      this.labelVersionHeader.AutoSize = true;
      this.labelVersionHeader.Location = new System.Drawing.Point(152, 35);
      this.labelVersionHeader.Name = "labelVersionHeader";
      this.labelVersionHeader.Size = new System.Drawing.Size(45, 13);
      this.labelVersionHeader.TabIndex = 32;
      this.labelVersionHeader.Text = "Version:";
      // 
      // labelAuthorHeader
      // 
      this.labelAuthorHeader.AutoSize = true;
      this.labelAuthorHeader.Location = new System.Drawing.Point(152, 58);
      this.labelAuthorHeader.Name = "labelAuthorHeader";
      this.labelAuthorHeader.Size = new System.Drawing.Size(41, 13);
      this.labelAuthorHeader.TabIndex = 33;
      this.labelAuthorHeader.Text = "Author:";
      // 
      // labelAuthor
      // 
      this.labelAuthor.AutoSize = true;
      this.labelAuthor.Location = new System.Drawing.Point(205, 58);
      this.labelAuthor.Name = "labelAuthor";
      this.labelAuthor.Size = new System.Drawing.Size(37, 13);
      this.labelAuthor.TabIndex = 34;
      this.labelAuthor.Text = "author";
      // 
      // labelVersion
      // 
      this.labelVersion.AutoSize = true;
      this.labelVersion.Location = new System.Drawing.Point(205, 35);
      this.labelVersion.Name = "labelVersion";
      this.labelVersion.Size = new System.Drawing.Size(41, 13);
      this.labelVersion.TabIndex = 35;
      this.labelVersion.Text = "version";
      // 
      // DialogAbout
      // 
      this.AcceptButton = this.okButton;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.BackColor = System.Drawing.SystemColors.Control;
      this.ClientSize = new System.Drawing.Size(440, 315);
      this.Controls.Add(this.labelVersion);
      this.Controls.Add(this.labelAuthor);
      this.Controls.Add(this.labelAuthorHeader);
      this.Controls.Add(this.labelVersionHeader);
      this.Controls.Add(this.labelProjectName);
      this.Controls.Add(this.labelContact);
      this.Controls.Add(this.labelWebsite);
      this.Controls.Add(this.linkLabelWebsite);
      this.Controls.Add(this.linkLabelContact);
      this.Controls.Add(this.logoPictureBox);
      this.Controls.Add(this.okButton);
      this.Controls.Add(this.textBoxDescription);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "DialogAbout";
      this.Padding = new System.Windows.Forms.Padding(9);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "About subs2srs";
      ((System.ComponentModel.ISupportInitialize)(this.logoPictureBox)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.PictureBox logoPictureBox;
    private System.Windows.Forms.TextBox textBoxDescription;
    private System.Windows.Forms.Button okButton;
    private System.Windows.Forms.LinkLabel linkLabelContact;
    private System.Windows.Forms.LinkLabel linkLabelWebsite;
    private System.Windows.Forms.Label labelWebsite;
    private System.Windows.Forms.Label labelContact;
    private System.Windows.Forms.Label labelProjectName;
    private System.Windows.Forms.Label labelVersionHeader;
    private System.Windows.Forms.Label labelAuthorHeader;
    private System.Windows.Forms.Label labelAuthor;
    private System.Windows.Forms.Label labelVersion;
  }
}
