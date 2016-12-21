namespace subs2srs
{
  partial class DialogPref
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DialogPref));
      this.propertyGridPref = new System.Windows.Forms.PropertyGrid();
      this.buttonCancel = new System.Windows.Forms.Button();
      this.buttonOK = new System.Windows.Forms.Button();
      this.panelBottom = new System.Windows.Forms.Panel();
      this.buttonTokenList = new System.Windows.Forms.Button();
      this.buttonResultItem = new System.Windows.Forms.Button();
      this.buttonResetAllDefaults = new System.Windows.Forms.Button();
      this.panelBottom.SuspendLayout();
      this.SuspendLayout();
      // 
      // propertyGridPref
      // 
      this.propertyGridPref.Dock = System.Windows.Forms.DockStyle.Fill;
      this.propertyGridPref.Location = new System.Drawing.Point(0, 0);
      this.propertyGridPref.Name = "propertyGridPref";
      this.propertyGridPref.Size = new System.Drawing.Size(682, 403);
      this.propertyGridPref.TabIndex = 0;
      this.propertyGridPref.ToolbarVisible = false;
      // 
      // buttonCancel
      // 
      this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.buttonCancel.Location = new System.Drawing.Point(595, 3);
      this.buttonCancel.Name = "buttonCancel";
      this.buttonCancel.Size = new System.Drawing.Size(75, 23);
      this.buttonCancel.TabIndex = 1;
      this.buttonCancel.Text = "&Cancel";
      this.buttonCancel.UseVisualStyleBackColor = true;
      // 
      // buttonOK
      // 
      this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonOK.Location = new System.Drawing.Point(514, 3);
      this.buttonOK.Name = "buttonOK";
      this.buttonOK.Size = new System.Drawing.Size(75, 23);
      this.buttonOK.TabIndex = 2;
      this.buttonOK.Text = "&OK";
      this.buttonOK.UseVisualStyleBackColor = true;
      this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
      // 
      // panelBottom
      // 
      this.panelBottom.Controls.Add(this.buttonTokenList);
      this.panelBottom.Controls.Add(this.buttonResultItem);
      this.panelBottom.Controls.Add(this.buttonResetAllDefaults);
      this.panelBottom.Controls.Add(this.buttonOK);
      this.panelBottom.Controls.Add(this.buttonCancel);
      this.panelBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
      this.panelBottom.Location = new System.Drawing.Point(0, 403);
      this.panelBottom.Name = "panelBottom";
      this.panelBottom.Size = new System.Drawing.Size(682, 33);
      this.panelBottom.TabIndex = 3;
      // 
      // buttonTokenList
      // 
      this.buttonTokenList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.buttonTokenList.Location = new System.Drawing.Point(303, 3);
      this.buttonTokenList.Name = "buttonTokenList";
      this.buttonTokenList.Size = new System.Drawing.Size(104, 23);
      this.buttonTokenList.TabIndex = 2;
      this.buttonTokenList.Text = "&Token List...";
      this.buttonTokenList.UseVisualStyleBackColor = true;
      this.buttonTokenList.Click += new System.EventHandler(this.buttonTokenList_Click);
      // 
      // buttonResultItem
      // 
      this.buttonResultItem.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.buttonResultItem.Location = new System.Drawing.Point(93, 3);
      this.buttonResultItem.Name = "buttonResultItem";
      this.buttonResultItem.Size = new System.Drawing.Size(114, 23);
      this.buttonResultItem.TabIndex = 2;
      this.buttonResultItem.Text = "Reset &Selected Item";
      this.buttonResultItem.UseVisualStyleBackColor = true;
      this.buttonResultItem.Click += new System.EventHandler(this.buttonResultItem_Click);
      // 
      // buttonResetAllDefaults
      // 
      this.buttonResetAllDefaults.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.buttonResetAllDefaults.ForeColor = System.Drawing.Color.DarkRed;
      this.buttonResetAllDefaults.Location = new System.Drawing.Point(12, 3);
      this.buttonResetAllDefaults.Name = "buttonResetAllDefaults";
      this.buttonResetAllDefaults.Size = new System.Drawing.Size(75, 23);
      this.buttonResetAllDefaults.TabIndex = 2;
      this.buttonResetAllDefaults.Text = "&Reset All";
      this.buttonResetAllDefaults.UseVisualStyleBackColor = true;
      this.buttonResetAllDefaults.Click += new System.EventHandler(this.buttonResetAllDefaults_Click);
      // 
      // DialogPref
      // 
      this.AcceptButton = this.buttonOK;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.buttonCancel;
      this.ClientSize = new System.Drawing.Size(682, 436);
      this.Controls.Add(this.propertyGridPref);
      this.Controls.Add(this.panelBottom);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.Name = "DialogPref";
      this.Text = "Preferences";
      this.Load += new System.EventHandler(this.DialogPref_Load);
      this.panelBottom.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.PropertyGrid propertyGridPref;
    private System.Windows.Forms.Button buttonCancel;
    private System.Windows.Forms.Button buttonOK;
    private System.Windows.Forms.Panel panelBottom;
    private System.Windows.Forms.Button buttonResetAllDefaults;
    private System.Windows.Forms.Button buttonResultItem;
    private System.Windows.Forms.Button buttonTokenList;
  }
}