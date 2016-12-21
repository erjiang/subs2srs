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
using System.Drawing;
using System.Linq;
using System.Text;

namespace subs2srs
{

  /// <summary>
  /// Represents a .ass subtitle style.
  /// </summary>
  public class InfoStyle
  {
    private Font font = new Font(new FontFamily("Arial"), 20, FontStyle.Regular);

    private Color colorPrimary = System.Drawing.Color.White;
    private Color colorSecondary = System.Drawing.Color.Red;
    private Color colorOutline = System.Drawing.Color.Black;
    private Color colorShadow = System.Drawing.Color.Black;
    private int opacityPrimary = 0;
    private int opacitySecondary = 0;
    private int opacityOutline = 0;
    private int opacityShadow = 0;

    private int outline = 2;
    private int shadow = 2;
    private bool opaqueBox = false;

    private int alignment = 2;

    private int marginLeft = 10;
    private int marginRight = 10;
    private int marginVertical = 10;

    private int scaleX = 100;
    private int scaleY = 100;
    private int rotation = 0;
    private int spacing = 0;
    private StyleEncoding encoding = new StyleEncoding(1, "Default");


    public Font Font
    {
      get { return font; }
      set { font = value; }
    }

    public Color ColorPrimary
    {
      get { return colorPrimary; }
      set { colorPrimary = value; }
    }

    public Color ColorSecondary
    {
      get { return colorSecondary; }
      set { colorSecondary = value; }
    }

    public Color ColorOutline
    {
      get { return colorOutline; }
      set { colorOutline = value; }
    }

    public Color ColorShadow
    {
      get { return colorShadow; }
      set { colorShadow = value; }
    }

    public int OpacityPrimary
    {
      get { return opacityPrimary; }
      set { opacityPrimary = value; }
    }

    public int OpacitySecondary
    {
      get { return opacitySecondary; }
      set { opacitySecondary = value; }
    }

    public int OpacityOutline
    {
      get { return opacityOutline; }
      set { opacityOutline = value; }
    }

    public int OpacityShadow
    {
      get { return opacityShadow; }
      set { opacityShadow = value; }
    }

    public int Outline
    {
      get { return outline; }
      set { outline = value; }
    }

    public int Shadow
    {
      get { return shadow; }
      set { shadow = value; }
    }

    public bool OpaqueBox
    {
      get { return opaqueBox; }
      set { opaqueBox = value; }
    }

    public int Alignment
    {
      get { return alignment; }
      set { alignment = value; }
    }

    public int MarginLeft
    {
      get { return marginLeft; }
      set { marginLeft = value; }
    }

    public int MarginRight
    {
      get { return marginRight; }
      set { marginRight = value; }
    }

    public int MarginVertical
    {
      get { return marginVertical; }
      set { marginVertical = value; }
    }

    public int ScaleX
    {
      get { return scaleX; }
      set { scaleX = value; }
    }

    public int ScaleY
    {
      get { return scaleY; }
      set { scaleY = value; }
    }

    public int Rotation
    {
      get { return rotation; }
      set { rotation = value; }
    }

    public int Spacing
    {
      get { return spacing; }
      set { spacing = value; }
    }

    public StyleEncoding Encoding
    {
      get { return encoding; }
      set { encoding = value; }
    }


    public InfoStyle()
    {

    }
  }


  public class StyleEncoding
  {
    private int num;
    private string text;

    public int Num
    {
      get { return num; }
      set { num = value; }
    }

    public string Text
    {
      get { return text; }
      set { text = value; }
    }

    public StyleEncoding(int num, string text)
    {
      this.num = num;
      this.text = text;
    }

    public static List<StyleEncoding> getDefaultList()
    {
      List<StyleEncoding> defaultList = new List<StyleEncoding>();

      defaultList.Add(new StyleEncoding(0, "ANSI"));
      defaultList.Add(new StyleEncoding(1, "Default"));
      defaultList.Add(new StyleEncoding(2, "Symbol"));
      defaultList.Add(new StyleEncoding(77, "Mac"));
      defaultList.Add(new StyleEncoding(128, "Shift_JIS"));
      defaultList.Add(new StyleEncoding(129, "Hangeul"));
      defaultList.Add(new StyleEncoding(130, "Johab"));
      defaultList.Add(new StyleEncoding(134, "GB2312"));
      defaultList.Add(new StyleEncoding(136, "Chinese BIG5"));
      defaultList.Add(new StyleEncoding(161, "Greek"));
      defaultList.Add(new StyleEncoding(162, "Turkish"));
      defaultList.Add(new StyleEncoding(163, "Vietnamese"));
      defaultList.Add(new StyleEncoding(177, "Hebrew"));
      defaultList.Add(new StyleEncoding(178, "Arabic"));
      defaultList.Add(new StyleEncoding(186, "Baltic"));
      defaultList.Add(new StyleEncoding(204, "Russian"));
      defaultList.Add(new StyleEncoding(222, "Thai"));
      defaultList.Add(new StyleEncoding(238, "East European"));
      defaultList.Add(new StyleEncoding(255, "OEM"));

      return defaultList;
    }

    public override string ToString()
    {
      return num.ToString() + " - " + text;
    }
  }
}
