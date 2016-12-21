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
  /// The Subtitle Style dialog.
  /// </summary>
  public partial class DialogSubtitleStyle : Form
  {
    private InfoStyle style;

    public InfoStyle Style 
    {
      get
      {
        return style; 
      }
      set 
      { 
        style = value;
        updateGUI();
      } 
    }


    public string Title
    {
      get
      {
        return this.Text;
      }
      set
      {
        this.Text = value;
      }
    }


    private List<StyleEncoding> styleEncodings = new List<StyleEncoding>();

    public DialogSubtitleStyle()
    {
      InitializeComponent();

      comboBoxMiscEncoding.Items.AddRange(StyleEncoding.getDefaultList().ToArray());
      comboBoxMiscEncoding.SelectedIndex = 1;

      style = new InfoStyle();
      updateGUI();
    }

    private void buttonFont_Click(object sender, EventArgs e)
    {
      fontDialog.FontMustExist = true;

      this.fontDialog.ShowDialog();

      updateFontPreview();
    }


    private void panelColor_Click(object sender, EventArgs e)
    {
      colorDialog.Color = ((Panel)sender).BackColor;

      if (this.colorDialog.ShowDialog() == DialogResult.OK)
      {
        ((Panel)sender).BackColor = colorDialog.Color;
      }
    }

    private void updateFontPreview()
    {
      this.textBoxFont.Text = fontDialog.Font.Name + ", " + fontDialog.Font.Size + ", " + fontDialog.Font.Style;
    }

    private void updateSettings()
    {
      style.Font = this.fontDialog.Font;
      style.ColorPrimary = this.panelColorPrimary.BackColor;
      style.ColorSecondary = this.panelColorSecondary.BackColor;
      style.ColorOutline = this.panelColorOutline.BackColor;
      style.ColorShadow = this.panelColorShadow.BackColor;
      style.OpacityPrimary = (int)this.numericUpDownOpacityPrimary.Value;
      style.OpacitySecondary = (int)this.numericUpDownOpacitySecondary.Value;
      style.OpacityOutline = (int)this.numericUpDownOpacityOutline.Value;
      style.OpacityShadow = (int)this.numericUpDownOpacityShadow.Value;
      style.Outline = (int)this.numericUpDownOutline.Value;
      style.Shadow = (int)this.numericUpDownShadow.Value;
      style.OpaqueBox = this.checkBoxOpaqueBox.Checked;

      if (this.radioButtonAlignment1.Checked)
      {
        style.Alignment = 1;
      }
      else if (this.radioButtonAlignment2.Checked)
      {
        style.Alignment = 2;
      }
      else if (this.radioButtonAlignment3.Checked)
      {
        style.Alignment = 3;
      }
      else if (this.radioButtonAlignment4.Checked)
      {
        style.Alignment = 4;
      }
      else if (this.radioButtonAlignment5.Checked)
      {
        style.Alignment = 5;
      }
      else if (this.radioButtonAlignment6.Checked)
      {
        style.Alignment = 6;
      }
      else if (this.radioButtonAlignment7.Checked)
      {
        style.Alignment = 7;
      }
      else if (this.radioButtonAlignment8.Checked)
      {
        style.Alignment = 8;
      }
      else if (this.radioButtonAlignment9.Checked)
      {
        style.Alignment = 9;
      }

      style.MarginLeft = (int)this.numericUpDownMarginsLeft.Value;
      style.MarginRight = (int)this.numericUpDownMarginsRight.Value;
      style.MarginVertical = (int)this.numericUpDownMarginsVertical.Value;
      style.ScaleX = (int)this.numericUpDownMiscScaleX.Value;
      style.ScaleY = (int)this.numericUpDownMiscScaleY.Value;
      style.Rotation = (int)this.numericUpDownMiscRotation.Value;
      style.Spacing = (int)this.numericUpDownMiscSpacing.Value;
      style.Encoding = StyleEncoding.getDefaultList()[this.comboBoxMiscEncoding.SelectedIndex];
    }


    private void updateGUI()
    {
      this.fontDialog.Font = style.Font;
      this.panelColorPrimary.BackColor = style.ColorPrimary;
      this.panelColorSecondary.BackColor = style.ColorSecondary;
      this.panelColorOutline.BackColor = style.ColorOutline;
      this.panelColorShadow.BackColor = style.ColorShadow;
      this.numericUpDownOpacityPrimary.Value = (decimal)style.OpacityPrimary;
      this.numericUpDownOpacitySecondary.Value = (decimal)style.OpacitySecondary;
      this.numericUpDownOpacityOutline.Value = (decimal)style.OpacityOutline;
      this.numericUpDownOpacityShadow.Value = (decimal)style.OpacityShadow;
      this.numericUpDownOutline.Value = (decimal)style.Outline;
      this.numericUpDownShadow.Value = (decimal)style.Shadow;
      this.checkBoxOpaqueBox.Checked = style.OpaqueBox;

      this.radioButtonAlignment1.Checked = false;
      this.radioButtonAlignment2.Checked = false;
      this.radioButtonAlignment3.Checked = false;
      this.radioButtonAlignment4.Checked = false;
      this.radioButtonAlignment5.Checked = false;
      this.radioButtonAlignment6.Checked = false;
      this.radioButtonAlignment7.Checked = false;
      this.radioButtonAlignment8.Checked = false;
      this.radioButtonAlignment9.Checked = false;

      switch (style.Alignment)
      {
        case 1: this.radioButtonAlignment1.Checked = true; break;
        case 2: this.radioButtonAlignment2.Checked = true; break;
        case 3: this.radioButtonAlignment3.Checked = true; break;
        case 4: this.radioButtonAlignment4.Checked = true; break;
        case 5: this.radioButtonAlignment5.Checked = true; break;
        case 6: this.radioButtonAlignment6.Checked = true; break;
        case 7: this.radioButtonAlignment7.Checked = true; break;
        case 8: this.radioButtonAlignment8.Checked = true; break;
        case 9: this.radioButtonAlignment9.Checked = true; break;
      }

      this.numericUpDownMarginsLeft.Value = (decimal)style.MarginLeft;
      this.numericUpDownMarginsRight.Value = (decimal)style.MarginRight;
      this.numericUpDownMarginsVertical.Value = (decimal)style.MarginVertical;
      this.numericUpDownMiscScaleX.Value = (decimal)style.ScaleX;
      this.numericUpDownMiscScaleY.Value = (decimal)style.ScaleY;
      this.numericUpDownMiscRotation.Value = (decimal)style.Rotation;
      this.numericUpDownMiscSpacing.Value = (decimal)style.Spacing;

      for (int i = 0; i < StyleEncoding.getDefaultList().Count; i++)
      {
        if (comboBoxMiscEncoding.Items[i].ToString() == style.Encoding.ToString())
        {
          comboBoxMiscEncoding.SelectedIndex = i;
          break;
        }
      }

      updateFontPreview();
    }

    private void buttonOK_Click(object sender, EventArgs e)
    {
      updateSettings();
    }

 
 


  }
}
