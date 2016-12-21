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
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace subs2srs
{
  /// <summary>
  /// Contains popup message helper routines.
  /// </summary>
  class UtilsMsg
  {
    /// <summary>
    /// Show an error messsage.
    /// </summary>
    public static void showErrMsg(string msg)
    {
      MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

      Logger.Instance.error(msg);
    }


    /// <summary>
    /// Show an information message.
    /// </summary>
    public static void showInfoMsg(string msg)
    {
      MessageBox.Show(msg, UtilsAssembly.Title, MessageBoxButtons.OK, MessageBoxIcon.Information);

      Logger.Instance.info(msg);
    }

    /// <summary>
    /// Show a Yes/No message.
    /// </summary>
    public static bool showConfirm(string msg)
    {
      DialogResult result = MessageBox.Show(msg, "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

      return (result == DialogResult.Yes);
    }
  }
}
