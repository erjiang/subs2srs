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
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace subs2srs
{
  public partial class GroupBoxCheck : GroupBox
  {
    private bool firstTime = true;


    // Text Property
    [
      Browsable(true),
      EditorBrowsable(EditorBrowsableState.Always),
      Description("The text to display next to the checkbox."),
      Category("Appearance")
    ]
    public override string Text
    {
      get
      {
        return this.checkBox1.Text;
      }

      set
      {
        this.checkBox1.Text = value;
        Invalidate();
      }
    }

    // Checked Property
    [
      Browsable(true),
      EditorBrowsable(EditorBrowsableState.Always),
      Description("Checked state."),
      Category("Appearance")
    ]
    public bool Checked
    {
      get
      {
        return this.checkBox1.Checked;
      }

      set
      {
        this.checkBox1.Checked = value;
      }
    }


    public GroupBoxCheck()
    {
      InitializeComponent();
    }


    /// <summary>
    /// Enable or Disable all child controls.
    /// </summary>
    private void setEnableForAllChildControls(bool state)
    {
      foreach (Control ctrl in this.Controls)
      {
        if (ctrl != this.checkBox1)
        {
          ctrl.Enabled = state;
        }
      }
    }


    private void checkBox1_CheckedChanged(object sender, EventArgs e)
    {
      setEnableForAllChildControls(this.checkBox1.Checked);
    }


    private void GroupBoxCheck_Paint(object sender, PaintEventArgs e)
    {
      if(firstTime)
      {
        setEnableForAllChildControls(this.checkBox1.Checked);
        firstTime = false;
      }
    }




  }
}
