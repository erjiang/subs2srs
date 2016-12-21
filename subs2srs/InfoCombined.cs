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
using System.Linq;
using System.Text;

namespace subs2srs
{
  /// <summary>
  /// Represents paired lines: Subs1 and it's corresponding Sub2.
  /// </summary>
  [Serializable]
  public class InfoCombined
  {
    private InfoLine subs1;
    private InfoLine subs2;
    private bool active;
    private bool onlyNeededForContext;


    public InfoLine Subs1 
    {
      get { return subs1; } 
    }

    public InfoLine Subs2
    {
      get { return subs2; } 
    }

    /// <summary>
    /// Is the line active? (That is, will it be processed?)
    /// </summary>
    public bool Active
    {
      get { return active; }
      set { active = value; }
    }

    /// <summary>
    /// Is the line only needed for context information?
    /// If true, Active is false for this line.
    /// </summary>
    public bool OnlyNeededForContext
    {
      get { return this.onlyNeededForContext; }
      set { this.onlyNeededForContext = value; }
    }


    public InfoCombined(InfoLine subs1, InfoLine subs2)
    {
      this.subs1 = subs1;
      this.subs2 = subs2;
      this.active = true;
      this.onlyNeededForContext = false;
    }

    public InfoCombined(InfoLine subs1, InfoLine subs2, bool active)
    {
      this.subs1 = subs1;
      this.subs2 = subs2;
      this.active = active;
      this.onlyNeededForContext = false;
    }


    public override string ToString()
    {
      return String.Format("{0}, {1}, {2}, {3}",
        this.active, this.onlyNeededForContext, this.subs1.StartTime, this.subs1.EndTime);
    }


  }
}
