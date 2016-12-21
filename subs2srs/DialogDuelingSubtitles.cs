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
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace subs2srs
{
  /// <summary>
  /// The Dueling subtitles dialog.
  /// </summary>
  public partial class DialogDuelingSubtitles : Form
  {
    // The global settings prior to entering this dialog. Restored when dialog is exited.
    private SaveSettings oldSettings = new SaveSettings();

    private InfoStyle styleSubs1 = new InfoStyle();
    private InfoStyle styleSubs2 = new InfoStyle();

    DialogSubtitleStyle styleDialogSubs = new DialogSubtitleStyle();

    private DialogProgress dialogProgress = null;
    private string lastDirPath = "";
    private int invalidCount = 0;

    private int duelPattern;
    private bool isSubs1First = true;
    private bool quickReference = false;

    public string Subs1FilePattern
    {
      set
      {
        this.textBoxSubs1File.Text = value;
      }
    }

    public string Subs2FilePattern
    {
      set
      {
        this.textBoxSubs2File.Text = value;
      }
    }

    public string OutputDir
    {
      set
      {
        this.textBoxOutputDir.Text = value;
      }
    }

    public string DeckName
    {
      set
      {
        this.textBoxName.Text = value;
      }
    }

    public int EpisodeStartNumber
    {
      set
      {
        this.numericUpDownEpisodeStartNumber.Value = (decimal)value;
      }
    }

    public bool UseSubs1Timings
    {
      set
      {
        this.radioButtonTimingSubs1.Checked = value;
        this.radioButtonTimingSubs2.Checked = !this.radioButtonTimingSubs1.Checked;
      }
    }

    public bool UseTimeShift
    {
      set
      {
        this.groupBoxTimeShift.Checked = value;
      }
    }

    public int TimeShiftSubs1
    {
      set
      {
        this.numericUpDownTimeShiftSubs1.Value = (decimal)value;
      }
    }

    public int TimeShiftSubs2
    {
      set
      {
        this.numericUpDownTimeShiftSubs2.Value = (decimal)value;
      }
    }

    public string EncodingSubs1
    {
      set
      {
        this.comboBoxEncodingSubs1.Text = value;
      }
    }


    public string EncodingSubs2
    {
      set
      {
        this.comboBoxEncodingSubs2.Text = value;
      }
    }


    public string FileBrowserStartDir
    {
      get
      {
        string dir = "";

        if (Directory.Exists(this.lastDirPath))
        {
          dir = lastDirPath;
        }

        return dir;
      }

      set
      {
        this.lastDirPath = value;
      }
    }


    public DialogDuelingSubtitles()
    {
      InitializeComponent();

      this.comboBoxPriority.SelectedIndex = 0;

      this.initEncodingsDropdown();
    }


    private void DialogDuelingSubtitles_Load(object sender, EventArgs e)
    {
      this.ActiveControl = this.textBoxSubs1File;

      // Save the global settings
      SaveSettings curSettings = new SaveSettings();
      curSettings.gatherData();
      this.oldSettings = ObjectCopier.Clone<SaveSettings>(curSettings);
 
      // Fill out some controls with global settings data
      this.checkBoxRemoveLinesWithoutCounterpartSubs1.Checked = Settings.Instance.Subs[0].RemoveNoCounterpart;
      this.checkBoxRemoveLinesWithoutCounterpartSubs2.Checked = Settings.Instance.Subs[1].RemoveNoCounterpart;
      this.checkBoxRemoveStyledLinesSubs1.Checked = Settings.Instance.Subs[0].RemoveStyledLines;
      this.checkBoxRemoveStyledLinesSubs2.Checked = Settings.Instance.Subs[1].RemoveStyledLines;
    }


    private void initEncodingsDropdown()
    {
      foreach (InfoEncoding encoding in InfoEncoding.getEncodings())
      {
        this.comboBoxEncodingSubs1.Items.Add(encoding.LongName);
        this.comboBoxEncodingSubs2.Items.Add(encoding.LongName);
      }
    }


    private void buttonSubs1File_Click(object sender, EventArgs e)
    {
      textBoxSubs1File.Text = showFileDialog(textBoxSubs1File.Text.Trim(),
        "All Subtitle Files (*.ass;*.ssa;*.srt;*.mkv)|*.ass;*.ssa;*.srt;*.mkv|" +
        "Advanced Substation Alpha (*.ass;*.ssa)|*.ass;*.ssa|" +
        "Matroska (*.mkv)|*.mkv|" +
        "SubRip (*.srt)|*.srt", 1);

    }

    private void buttonSubs2File_Click(object sender, EventArgs e)
    {
      textBoxSubs2File.Text = showFileDialog(textBoxSubs2File.Text.Trim(),
        "All Subtitle Files (*.ass;*.ssa;*.srt;*.mkv)|*.ass;*.ssa;*.srt;*.mkv|" +
        "Advanced Substation Alpha (*.ass;*.ssa)|*.ass;*.ssa|" +
        "Matroska (*.mkv)|*.mkv|" +
        "SubRip (*.srt)|*.srt", 1);
    }

    private void buttonOutputDir_Click(object sender, EventArgs e)
    {
      textBoxOutputDir.Text = showFolderDialog(textBoxOutputDir.Text);
    }


    /// <summary>
    /// Open folder dialog (starting at the last folder) and get the selected folder.
    /// </summary>
    private string showFolderDialog(string currentPath)
    {
      string ret = currentPath;

      if (Directory.Exists(currentPath))
      {
        this.folderBrowserDialog.SelectedPath = currentPath;
      }
      else if (lastDirPath != "")
      {
        this.folderBrowserDialog.SelectedPath = lastDirPath;
      }

      if (this.folderBrowserDialog.ShowDialog() == DialogResult.OK)
      {
        ret = this.folderBrowserDialog.SelectedPath;
        lastDirPath = this.folderBrowserDialog.SelectedPath;
      }

      return ret;
    }


    /// <summary>
    /// Open folder dialog (starting at the current file) and get the selected file.
    /// </summary>
    private string showFileDialog(string currentFilePattern, string filter, int filterIndex)
    {
      string selectedFile = currentFilePattern;
      string curDir = "";

      this.openFileDialog.FileName = "";

      this.openFileDialog.Filter = filter;

      openFileDialog.FilterIndex = filterIndex;

      try
      {
        curDir = (Path.GetDirectoryName(currentFilePattern));
      }
      catch
      {
        curDir = "";
      }

      if (Directory.Exists(curDir))
      {
        this.openFileDialog.InitialDirectory = curDir;
      }
      else if (lastDirPath != "")
      {
        this.openFileDialog.InitialDirectory = lastDirPath;
      }

      if (this.openFileDialog.ShowDialog() == DialogResult.OK)
      {
        selectedFile = this.openFileDialog.FileName;
        lastDirPath = Path.GetDirectoryName(selectedFile);
      }

      return selectedFile;
    }


    private void buttonStyleSubs1_Click(object sender, EventArgs e)
    {
      styleDialogSubs.Style = styleSubs1;
      styleDialogSubs.Title = "Subs1 Style";
      styleDialogSubs.ShowDialog();
      styleSubs1 = styleDialogSubs.Style;
    }

    private void button2StyleSubs2_Click(object sender, EventArgs e)
    {
      styleDialogSubs.Style = styleSubs2;
      styleDialogSubs.Title = "Subs2 Style";
      styleDialogSubs.ShowDialog();
      styleSubs2 = styleDialogSubs.Style;
    }


    /// <summary>
    /// Start the dueling subtitle creation thread.
    /// </summary>
    private void buttonCreate_Click(object sender, EventArgs e)
    {
      errorProvider1.Clear();

      if (validateForm())
      {
        updateSettings();

        Logger.Instance.info("DuelingSubtitles: GO!");
        Logger.Instance.writeSettingsToLog();

        // Start the worker thread
        try
        {
          WorkerVars workerVars = new WorkerVars(null, Settings.Instance.OutputDir, WorkerVars.SubsProcessingType.Dueling);

          // Create a background thread
          BackgroundWorker bw = new BackgroundWorker();
          bw.DoWork += new DoWorkEventHandler(bw_DoWork);
          bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);

          // Create a progress dialog on the UI thread
          dialogProgress = new DialogProgress();

          bw.RunWorkerAsync(workerVars);

          // Lock up the UI with this modal progress form
          dialogProgress.ShowDialog();
          dialogProgress = null;
        }
        catch (Exception e1)
        {
          UtilsMsg.showErrMsg("Something went wrong before processing could start.\n" + e1);
          return;
        }
      }
    }


    private void DialogDuelingSubtitles_FormClosing(object sender, FormClosingEventArgs e)
    {
      // Restore the global settings to the state prior to loading this dialog
      Settings.Instance.loadSettings(this.oldSettings);
    }

    // Determine if file is of a supported subtitle format
    private bool isSupportedSubtitleFormat(string file)
    {
      string ext = file.Substring(file.LastIndexOf(".")).ToLower();
      bool ret = false;

      if ((ext == ".srt") || (ext == ".ass") || (ext == ".ssa"))
      {
        ret = true;
      }

      return ret;
    }


    /// <summary>
    /// Validate GUI. Inform user if they entered invalid data.
    /// </summary>
    private bool validateForm()
    {
      bool status = false;
      invalidCount = 0;

      this.ValidateChildren();

      if (invalidCount == 0)
      {
        status = true;
      }
      else if (invalidCount == 1)
      {
        UtilsMsg.showErrMsg("Please correct the error on this form."
          + "\r\n\r\nHover the mouse over the red error bubble"
          + "\r\nto determine the nature of the error.");
      }
      else
      {
        UtilsMsg.showErrMsg(String.Format("Please correct the {0} errors on this form.", invalidCount)
          + "\r\n\r\nHover the mouse over the red error bubbles"
           + "\r\nto determine the nature of the errors.");
      }

      return status;
    }


    private void textBoxName_Validating(object sender, CancelEventArgs e)
    {
      string error = null;
      string name = ((TextBox)sender).Text.Trim();

      if (name == "")
      {
        error = "Must consist of 1 or more characters";
        invalidCount++;
      }
      else if (name.Contains("\\") || name.Contains("/") || name.Contains(":") ||
               name.Contains("*") || name.Contains("?") || name.Contains("\"") ||
               name.Contains("<") || name.Contains(">") || name.Contains("|"))
      {
        error = "Must not contain any of the following characters:\n \\ / : * ? \" < > |";
        invalidCount++;
      }

      errorProvider1.SetError((Control)sender, error);
    }


    private void textBoxSubsOutputDir_Validating(object sender, CancelEventArgs e)
    {
      string error = null;
      string dir = ((TextBox)sender).Text.Trim();

      if (!Directory.Exists(dir))
      {
        error = "Directory does not exist";
        invalidCount++;
      }

      errorProvider1.SetError((Control)sender, error);
    }


    private void textBoxSubs1File_Validating(object sender, CancelEventArgs e)
    {
      string error = null;
      string filePattern = ((TextBox)sender).Text.Trim();
      string[] files = UtilsCommon.getNonHiddenFiles(filePattern);

      if ((error == null) && (UtilsSubs.getNumSubsFiles(filePattern) == 0))
      {
        error = "Please provide a valid subtitle file. \nOnly .srt, .ass and .ssa are allowed.";
        invalidCount++;
      }

      if ((error == null))
      {
        foreach (string f in files)
        {
          if (!isSupportedSubtitleFormat(f))
          {
            error = "Please provide a valid subtitle file.\nOnly .srt, .ass, and .ssa are allowed.";
            invalidCount++;
            break;
          }
        }
      }

      errorProvider1.SetError((Control)sender, error);
    }


    private void textBoxSubs2File_Validating(object sender, CancelEventArgs e)
    {
      string error = null;
      string filePattern = ((TextBox)sender).Text.Trim();
      string[] files = UtilsCommon.getNonHiddenFiles(filePattern);

      if ((error == null) && (UtilsSubs.getNumSubsFiles(filePattern) == 0))
      {
        error = "Please provide a valid subtitle file. \nOnly .srt, .ass and .ssa are allowed.";
        invalidCount++;
      }

      if ((error == null))
      {
        foreach (string f in files)
        {
          if (!isSupportedSubtitleFormat(f))
          {
            error = "Please provide a valid subtitle file. \nOnly .srt, .ass and .ssa are allowed.";
            invalidCount++;
            break;
          }
        }
      }

      if ((error == null) && (UtilsSubs.getNumSubsFiles(filePattern) != UtilsSubs.getNumSubsFiles(textBoxSubs1File.Text.Trim())))
      {
        error = "The number of files here must match\nthe number of subtitle files in Subs1.";
        invalidCount++;
      }  

      errorProvider1.SetError((Control)sender, error);
    }


    private void updateSettings()
    {
      try
      {
        Settings.Instance.Subs[0].FilePattern = textBoxSubs1File.Text.Trim();
        Settings.Instance.Subs[0].TimingsEnabled = radioButtonTimingSubs1.Checked;
        Settings.Instance.Subs[0].TimeShift = (int)numericUpDownTimeShiftSubs1.Value;
        Settings.Instance.Subs[0].Files = UtilsSubs.getSubsFiles(Settings.Instance.Subs[0].FilePattern).ToArray();
        Settings.Instance.Subs[0].Encoding = InfoEncoding.longToShort(this.comboBoxEncodingSubs1.Text);
        Settings.Instance.Subs[0].RemoveNoCounterpart = this.checkBoxRemoveLinesWithoutCounterpartSubs1.Checked;
        Settings.Instance.Subs[0].RemoveStyledLines = this.checkBoxRemoveStyledLinesSubs1.Checked;

        Settings.Instance.Subs[1].FilePattern = textBoxSubs2File.Text.Trim();
        Settings.Instance.Subs[1].TimingsEnabled = radioButtonTimingSubs2.Checked;
        Settings.Instance.Subs[1].TimeShift = (int)numericUpDownTimeShiftSubs2.Value;
        Settings.Instance.Subs[1].Files = UtilsSubs.getSubsFiles(Settings.Instance.Subs[1].FilePattern).ToArray();
        Settings.Instance.Subs[1].Encoding = InfoEncoding.longToShort(this.comboBoxEncodingSubs2.Text);
        Settings.Instance.Subs[1].RemoveNoCounterpart = this.checkBoxRemoveLinesWithoutCounterpartSubs2.Checked;
        Settings.Instance.Subs[1].RemoveStyledLines = this.checkBoxRemoveStyledLinesSubs2.Checked;

        Settings.Instance.OutputDir = textBoxOutputDir.Text.Trim();

        Settings.Instance.TimeShiftEnabled = groupBoxTimeShift.Checked;

        Settings.Instance.DeckName = textBoxName.Text.Trim();
        Settings.Instance.EpisodeStartNumber = (int)numericUpDownEpisodeStartNumber.Value;

        duelPattern = (int)this.numericUpDownPattern.Value;

        isSubs1First = (this.comboBoxPriority.SelectedIndex == 0);

        quickReference = this.checkBoxQuickReference.Checked;
      }
      catch (Exception e1)
      {
        UtilsMsg.showErrMsg("Something went wrong while gathering interface data:\n" + e1);
        return;
      }
    }


    /// <summary>
    /// Called when the Dueling Subtitles creation thread completes.
    /// </summary>
    private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
      if (dialogProgress != null)
      {
        dialogProgress.Hide();
        dialogProgress = null;
      }

      if (e.Error != null)
      {
        UtilsMsg.showErrMsg(e.Error.Message);
        return;
      }

      if (e.Cancelled)
      {
        MessageBox.Show("Action cancelled.", UtilsAssembly.Title, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return;
      }

      WorkerVars workerVars = e.Result as WorkerVars;
      UtilsMsg.showInfoMsg("Dueling subtitles have been created successfully.");
    }


    /// <summary>
    /// Perform the work in the Dueling Subtitles creation thread.
    /// </summary>
    private void bw_DoWork(object sender, DoWorkEventArgs e)
    {
      WorkerVars workerVars = e.Argument as WorkerVars;
      List<List<InfoCombined>> combinedAll = new List<List<InfoCombined>>();
      WorkerSubs subsWorker = new WorkerSubs();

      // Parse and combine the subtitles
      try
      {
        combinedAll = subsWorker.combineAllSubs(workerVars, dialogProgress);

        if (combinedAll != null)
        {
          workerVars.CombinedAll = combinedAll;
        }
        else
        {
          e.Cancel = true;
          return;
        }
      }
      catch (Exception e1)
      {
        UtilsMsg.showErrMsg("Something went wrong before processing could start.\n" + e1);
        e.Cancel = true;
        return;
      }


      // Create the .ass file
      try
      {
        if (!createDuelingSubtitles(workerVars, dialogProgress))
        {
          e.Cancel = true;
          return;
        }
      }
      catch (Exception e1)
      {
        UtilsMsg.showErrMsg("Something went wrong when generating the Dueling Subtitles.\n" + e1);
        e.Cancel = true;
        return;
      }

      // Create the quick reference file
      if (quickReference)
      {
        try
        {
          if (!createQuickReference(workerVars, dialogProgress))
          {
            e.Cancel = true;
            return;
          }
        }
        catch (Exception e1)
        {
          UtilsMsg.showErrMsg("Something went wrong when generating the quick reference file.\n" + e1);
          e.Cancel = true;
          return;
        }
      }
    }


    /// <summary>
    /// Create the Dueling Subtitles .ass file.
    /// </summary>
    private bool createDuelingSubtitles(WorkerVars workerVars, DialogProgress dialogProgress)
    {
      int totalLines = 0;
      int progessCount = 0;
      int totalEpisodes = workerVars.CombinedAll.Count;
      int episodeIdx = -1;
      int lineIdx = -1;
      DateTime lastTime = new DateTime();

      foreach (List<InfoCombined> combArray in workerVars.CombinedAll)
      {
        totalLines += combArray.Count;

        // Get the last time
        foreach (InfoCombined info in combArray)
        {
          if (info.Subs1.EndTime.TimeOfDay.TotalMilliseconds > lastTime.TimeOfDay.TotalMilliseconds)
          {
            lastTime = new DateTime();
            lastTime = lastTime.AddMilliseconds(info.Subs1.EndTime.TimeOfDay.TotalMilliseconds);
          }
        }
      }

      UtilsName name = new UtilsName(Settings.Instance.DeckName, totalEpisodes,
        totalLines, lastTime, 0, 0);

      // For each episode
      foreach (List<InfoCombined> combArray in workerVars.CombinedAll)
      {
        episodeIdx++;
        lineIdx = -1;

        string nameStr = name.createName(ConstantSettings.DuelingSubtitleFilenameFormat, Settings.Instance.EpisodeStartNumber + episodeIdx,
          0, new DateTime(), new DateTime(), "", "");

        // Create filename
        string subtitleFilename = String.Format("{0}{1}{2}",
                                                Settings.Instance.OutputDir,
                                                Path.DirectorySeparatorChar,
                                                nameStr);
        TextWriter subtitleWriter = new StreamWriter(subtitleFilename, false, Encoding.UTF8);

        /* 
           Example .ass file:
           ; Generated with subs2srs - http://sourceforge.net/projects/subs2srs/

           [Script Info]
           Title:Some_Title_001
           ScriptType:v4.00+
           Collisions:Normal
           Timer:100.0000
                 
           [V4+ Styles]
           Format: Name, Fontname, Fontsize, PrimaryColour, SecondaryColour, OutlineColour, BackColour, Bold, Italic, Underline, StrikeOut, ScaleX, ScaleY, Spacing, Angle, BorderStyle, Outline, Shadow, Alignment, MarginL, MarginR, MarginV, Encoding
           Style: Subs1,Meiryo,18,&H00F9EDFF,&H0000FFFF,&H3C2B0E17,&HA0000000,0,0,0,0,84,100,0,0,1,2,1,2,10,10,15,1
           Style: Subs2,Meiryo,18,&H00F9EDFF,&H0000FFFF,&H3C2B0E17,&HA0000000,0,0,0,0,84,100,0,0,1,2,1,2,10,10,15,1
                 
           [Events]
           Format: Layer, Start, End, Style, Actor, MarginL, MarginR, MarginV, Effect, Text

           Dialogue: 0,0:00:31.14,0:00:36.16,Subs1,NTP,0000,0000,0000,,This is the first line from Subs1
           Dialogue: 0,0:00:31.14,0:00:36.16,Subs2,NTP,0000,0000,0000,,This is corresponding line from Subs2

           etc.
        */

        // [Script Info]
        string scriptInfoText = formatScriptInfoText(Settings.Instance.EpisodeStartNumber + episodeIdx);
        subtitleWriter.WriteLine(scriptInfoText);

        // [V4+ Styles]
        string stylesText = formatSytlesText();
        subtitleWriter.WriteLine(stylesText);

        // [Events] (header)
        string eventsHeaderText = formatEventHeaderText();
        subtitleWriter.WriteLine(eventsHeaderText);

        // For each line in episode, [Events] Dialog: ...
        foreach (InfoCombined comb in combArray)
        {
          progessCount++;
          lineIdx++;

          string dialogLine = "";

          dialogLine = formatDialogPair(workerVars.CombinedAll, episodeIdx, lineIdx);

          // Write line to file
          subtitleWriter.WriteLine(dialogLine);

          string progressText = string.Format("Generating Subtitle file: line {0} of {1}",
                                              progessCount.ToString(),
                                              totalLines.ToString());

          int progress = Convert.ToInt32(progessCount * (100.0 / totalLines));

          DialogProgress.updateProgressInvoke(dialogProgress, progress, progressText);

          if (dialogProgress.Cancel)
          {
            subtitleWriter.Close();
            return false;
          }
        }

        subtitleWriter.Close();
      }

      return true;
    }


    /// <summary>
    /// Format the .ass header comment.
    /// </summary>
    private string formatScriptInfoText(int episode)
    {
      /*
        ; Generated with subs2srs - http://sourceforge.net/projects/subs2srs/
        
        [Script Info]
        Title:Some_Title_001
        ScriptType:v4.00+
        Collisions:Normal
        Timer:100.0000
      */

      string scriptInfo = "";

      scriptInfo += String.Format("; Generated with {0} - http://sourceforge.net/projects/{0}/\n\n",
                                  UtilsAssembly.Title);
      scriptInfo += "[Script Info]\n";
      scriptInfo += String.Format("Title:{0}_{1:000.}\n",
                                          Settings.Instance.DeckName,
                                          episode);
      scriptInfo += "ScriptType:v4.00+\n";
      scriptInfo += "Collisions:Normal\n";
      scriptInfo += "Timer:100.0000\n";

      return scriptInfo;
    }
 

    /// <summary>
    /// Format the style of the .ass dialog text.
    /// </summary>
    private string formatSytlesText()
    {
      string styles = "";

      styles += "[V4+ Styles]\n";
      styles += "Format: Name, Fontname, Fontsize, PrimaryColour, SecondaryColour, OutlineColour, BackColour, Bold, Italic, Underline, StrikeOut, ScaleX, ScaleY, Spacing, Angle, BorderStyle, Outline, Shadow, Alignment, MarginL, MarginR, MarginV, Encoding\n";
      styles += formatSingleStyle("Subs1", styleSubs1);
      styles += formatSingleStyle("Subs2", styleSubs2);

      return styles;
    }


    /// <summary>
    /// Format a single style (Subs1 or Subs2) for the .ass dialog text.
    /// </summary>
    private string formatSingleStyle(string name, InfoStyle style)
    {
      int italic = 0;
      int bold = 0;
      int underline = 0;
      int strikeOut = 0;
      int bolderStyle = 1;

      if (style.Font.Italic)
      {
        italic = -1;
      }

      if (style.Font.Bold)
      {
        bold = -1;
      }

      if (style.Font.Underline)
      {
        underline = -1;
      }

      if (style.Font.Strikeout)
      {
        strikeOut = -1;
      }

      if (style.OpaqueBox)
      {
        bolderStyle = 3;
      }

      // Example:
      // Style: Subs1,Meiryo,18,&H00F9EDFF,&H0000FFFF,&H3C2B0E17,&HA0000000,0,0,0,0,84,100,0,0,1,2,1,2,10,10,15,1
      string styleStr = String.Format("Style: {0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18},{19},{20},{21},{22}\n",
                            name,                 // {0} Name
                            style.Font.Name,      // {1} Fontname
                            style.Font.Size,      // {2} Fontsize
                            UtilsSubs.formatAssColor(style.ColorPrimary, style.OpacityPrimary),     // {3} PrimaryColour
                            UtilsSubs.formatAssColor(style.ColorSecondary, style.OpacitySecondary), // {4} SecondaryColour
                            UtilsSubs.formatAssColor(style.ColorOutline, style.OpacityOutline),     // {5} OutlineColour
                            UtilsSubs.formatAssColor(style.ColorShadow, style.OpacityShadow),       // {6} BackColour
                            bold,                 // {7} Bold
                            italic,               // {8} Italic
                            underline,            // {9} Underline
                            strikeOut,            // {10} StrikeOut
                            style.ScaleX,         // {11} ScaleX
                            style.ScaleY,         // {12} ScaleY
                            style.Spacing,        // {13} Spacing
                            style.Rotation,       // {14} Angle
                            bolderStyle,          // {15} BorderStyle
                            style.Outline,        // {16} Outline
                            style.Shadow,         // {17} Shadow
                            style.Alignment,      // {18} Alignment
                            style.MarginLeft,     // {19} MarginL
                            style.MarginRight,    // {20} MarginR
                            style.MarginVertical, // {21} MarginV
                            style.Encoding.Num);  // {22} Encoding

      return styleStr;
    }


    /// <summary>
    /// Format the .ass dialog format.
    /// </summary>
    private string formatEventHeaderText()
    {
      string eventsHeader = "";


      eventsHeader += "[Events]\n";
      eventsHeader += "Format: Layer, Start, End, Style, Actor, MarginL, MarginR, MarginV, Effect, Text";

      return eventsHeader;
    }


    /// <summary>
    /// Format a single line of .ass dialog text.
    /// </summary>
    private string formatDialogSingle(bool isSubs1, InfoCombined comb)
    {
      string name = "";
      string text = "";
      string single = "";

      if (isSubs1)
      {
        name = "Subs1";
        text = comb.Subs1.Text;
      }
      else
      {
        name = "Subs2";
        text = comb.Subs2.Text;
      }

      // Example:
      // Dialogue: 0,0:00:31.14,0:00:36.16,Subs1,NA,0000,0000,0000,,This is the first line from Subs1
      single = String.Format("Dialogue: 0,{0},{1},{2},NA,0000,0000,0000,,{3}",
                              UtilsSubs.formatAssTime(comb.Subs1.StartTime),
                              UtilsSubs.formatAssTime(comb.Subs1.EndTime),
                              name,
                              text);

      return single;
    }

    /// <summary>
    /// Format a pair of .ass dialog text lines.
    /// </summary>
    private string formatDialogPair(List<List<InfoCombined>> combinedAll, int episodeIndex, int lineIndex)
    {
      List<InfoCombined> combArray = combinedAll[episodeIndex];
      InfoCombined comb = combArray[lineIndex];
      string pair = "";

      // Draw Subs1 or Subs2 first based on the priority selected by the user
      if (isSubs1First)
      {
        pair += formatDialogSingle(true, comb);

        if (lineIndex % duelPattern == 0)
        {
          pair += "\n";
          pair += formatDialogSingle(false, comb);
        }
      }
      else
      {
        if (lineIndex % duelPattern == 0)
        {
          pair += formatDialogSingle(false, comb);
          pair += "\n";
        }

        pair += formatDialogSingle(true, comb);
      }

      return pair;
    }


    /// <summary>
    /// Create the quick reference text file.
    /// </summary>
    private bool createQuickReference(WorkerVars workerVars, DialogProgress dialogProgress)
    {
      int totalLines = 0;
      int progessCount = 0;
      int episodeIdx = -1;
      int totalEpisodes = workerVars.CombinedAll.Count;
      int lineIdx = -1;
      DateTime lastTime = new DateTime();

      foreach (List<InfoCombined> combArray in workerVars.CombinedAll)
      {
        totalLines += combArray.Count;

        // Get the last time
        foreach (InfoCombined info in combArray)
        {
          if (info.Subs1.EndTime.TimeOfDay.TotalMilliseconds > lastTime.TimeOfDay.TotalMilliseconds)
          {
            lastTime = new DateTime();
            lastTime = lastTime.AddMilliseconds(info.Subs1.EndTime.TimeOfDay.TotalMilliseconds);
          }
        }
      }

      UtilsName name = new UtilsName(Settings.Instance.DeckName, totalEpisodes,
        totalLines, lastTime, 0, 0);

      // For each episode
      foreach (List<InfoCombined> combArray in workerVars.CombinedAll)
      {
        episodeIdx++;
        lineIdx = -1;

        string nameStr = name.createName(ConstantSettings.DuelingQuickRefFilenameFormat, Settings.Instance.EpisodeStartNumber + episodeIdx,
          0, new DateTime(), new DateTime(), "", "");

        // Create filename
        string subtitleFilename = String.Format("{0}{1}{2}",
                                            Settings.Instance.OutputDir,
                                            Path.DirectorySeparatorChar,
                                            nameStr);
        TextWriter subtitleWriter = new StreamWriter(subtitleFilename, false, Encoding.UTF8);

        /* 
           [0:00:31.14]  This is the first line from Subs1
           [0:00:31.14]  This is corresponding line from Subs2

           [0:00:34.35]  This is the second line from Subs1
           [0:00:34.35]  This is corresponding line from Subs2
        */

        // For each line in episode
        foreach (InfoCombined comb in combArray)
        {
          progessCount++;
          lineIdx++;

          string dialogLine = formatQuickReferenceDialogPair(comb, name, Settings.Instance.EpisodeStartNumber + episodeIdx, progessCount);

          // Write line to file
          subtitleWriter.WriteLine(dialogLine);


          string progressText = string.Format("Generating quick reference file: line {0} of {1}",
                                              progessCount.ToString(),
                                              totalLines.ToString());

          int progress = Convert.ToInt32(progessCount * (100.0 / totalLines));

          DialogProgress.updateProgressInvoke(dialogProgress, progress, progressText);

          if (dialogProgress.Cancel)
          {
            subtitleWriter.Close();
            return false;
          }
        }

        subtitleWriter.Close();
      }

      return true;
    }

    /// <summary>
    /// Format a pair of dialog for the quick reference text file.
    /// </summary>
    private string formatQuickReferenceDialogPair(InfoCombined comb, UtilsName name, int episode, int sequenceNumber)
    {
      string pair = "";

      string subs1Text = comb.Subs1.Text;
      string subs2Text = comb.Subs2.Text;

      string nameStr = name.createName(ConstantSettings.DuelingQuickRefSubs1Format, episode,
        sequenceNumber, comb.Subs1.StartTime, comb.Subs1.StartTime, subs1Text, subs2Text);

      pair += String.Format("{0}",
                            nameStr);

      if (this.textBoxSubs2File.Text.Trim().Length > 0)
      {
        if (ConstantSettings.DuelingQuickRefSubs2Format != "")
        {
          nameStr = name.createName(ConstantSettings.DuelingQuickRefSubs2Format, episode,
            sequenceNumber, comb.Subs1.StartTime, comb.Subs1.StartTime, subs1Text, subs2Text);

          pair += "\n";
          pair += String.Format("{0}",
                                nameStr);
        }
      }

      return pair;
    }


    private void textBoxSubs1File_TextChanged(object sender, EventArgs e)
    {
      this.updateSubs(1);
    }


    private void textBoxSubs2File_TextChanged(object sender, EventArgs e)
    {
      this.updateSubs(2);
    }


    /// <summary>
    /// Update the information related to Subs1 or Subs2.
    /// </summary>
    private void updateSubs(int subsNum)
    {
      TextBox textbox;
      string file;

      // Get items that depend on whether it's subs1 or subs2
      if (subsNum == 1)
      {
        textbox = this.textBoxSubs1File;
        file = this.textBoxSubs1File.Text.Trim();
      }
      else
      {
        textbox = this.textBoxSubs2File;
        file = this.textBoxSubs2File.Text.Trim();
      }

      // If input file is an MKV, show the MKV track select dialog
      if (Path.GetExtension(file) == ".mkv")
      {
        List<MkvTrack> allSubTrackList = UtilsMkv.getSubtitleTrackList(file);
        List<MkvTrack> subTrackList = new List<MkvTrack>();

        // Remove VOBSUB tracks
        foreach (MkvTrack subTrack in allSubTrackList)
        {
          if (subTrack.Extension != "sub")
          {
            subTrackList.Add(subTrack);
          }
        }

        if (subTrackList.Count == 0)
        {
          UtilsMsg.showInfoMsg("This .mkv file does not contain any subtitle tracks.");
          textbox.Text = "";
        }
        else
        {
          DialogSelectMkvTrack mkvDlg = new DialogSelectMkvTrack(file, subsNum, subTrackList);
          DialogResult result = mkvDlg.ShowDialog();

          if (result == DialogResult.OK)
          {
            textbox.Text = mkvDlg.ExtractedFile;
          }
          else
          {
            textbox.Text = "";
          }
        }

        return; // Since textbox.Text was changed, this routine will be called again
      }
    }









  }
}
