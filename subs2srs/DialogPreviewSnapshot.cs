//  Copyright (C) 2009-2016 Christopher Brochtrup
//
//  This file is part of subs2srs.
//
//  subs2srs is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  subs2srs is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with subs2srs.  If not, see <http://www.gnu.org/licenses/>.
//
//////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace subs2srs
{
  /// <summary>
  /// The Preview snapshot dialog.
  /// </summary>
  public partial class DialogPreviewSnapshot : Form
  {
    private Image snapshot;

    /// <summary>
    /// The snapshot that will be displayed (full-size) within this dialog.
    /// </summary>
    public Image Snapshot 
    {
      get 
      { 
        return snapshot; 
      }
      set 
      { 
        snapshot = value;

        this.ClientSize = new Size(snapshot.Width, snapshot.Height);

        this.pictureBoxPreview.Image = snapshot;
      } 
    }


    public DialogPreviewSnapshot()
    {
      InitializeComponent();
    }

    private void pictureBoxPreview_Click(object sender, EventArgs e)
    {
      this.Close();
    }













  }
}
