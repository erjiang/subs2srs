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
  /// The Advanced Subtitle Options dialog.
  /// </summary>
  public partial class DialogAdvancedSubtitleOptions : Form
  {

    private string subs1FilePattern = "";
    private string subs2FilePattern = "";
    private string subs1Encoding = "";
    private string subs2Encoding = "";

    public string Subs1FilePattern
    {
      set
      {
        subs1FilePattern = value;
      }
    }

    public string Subs2FilePattern
    {
      set
      {
        subs2FilePattern = value;
      }
    }

    public string Subs1Encoding
    {
      set
      {
        subs1Encoding = value;
      }
    }

    public string Subs2Encoding
    {
      set
      {
        subs2Encoding = value;
      }
    }


    public DialogAdvancedSubtitleOptions()
    {
      InitializeComponent();
    }


    private void buttonIncludeFromFileSubs1_Click(object sender, EventArgs e)
    {
      string fileText = getIncludeExludeTextFromFile();

      if (fileText != null)
      {
        textBoxSubs1IncludedWords.Text = fileText;
      }
    }


    private void buttonExcludeFromFileSubs1_Click(object sender, EventArgs e)
    {
      string fileText = getIncludeExludeTextFromFile();

      if (fileText != null)
      {
        textBoxSubs1ExcludedWords.Text = fileText;
      }
    }


    private void buttonIncludeFromFileSubs2_Click(object sender, EventArgs e)
    {
      string fileText = getIncludeExludeTextFromFile();

      if (fileText != null)
      {
        textBoxSubs2IncludedWords.Text = fileText;
      }
    }


    private void buttonExcludeFromFileSubs2_Click(object sender, EventArgs e)
    {
      string fileText = getIncludeExludeTextFromFile();

      if (fileText != null)
      {
        textBoxSubs2ExcludedWords.Text = fileText;
      }
    }


    /// <summary>
    /// Open a file meant to populate an exclude or include only field.
    /// </summary>
    private string getIncludeExludeTextFromFile()
    {
      string outStr = "";
      OpenFileDialog openDialog = new OpenFileDialog();
      StreamReader fileStream;

      openDialog.Filter = "Text files (*.txt)|*.txt";
      openDialog.FilterIndex = 1;
      openDialog.RestoreDirectory = true;
      openDialog.Multiselect = false;

      if (openDialog.ShowDialog() == DialogResult.OK)
      {
        try
        {
          if ((fileStream = new StreamReader(openDialog.OpenFile())) != null)
          {
            string fileText = fileStream.ReadToEnd().Trim();
            string[] words = fileText.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

            outStr = UtilsCommon.makeSemiString(words);

            fileStream.Close();
          }
        }
        catch
        {
          UtilsMsg.showErrMsg("Could not open file.");
          return null;
        }
      }

      return outStr;
    }


    /// <summary>
    /// Determine if this is a subitle file that contains an actor field.
    /// </summary>
    public static bool isActorSupportedSubtitleFormat(string file)
    {
      string ext = file.Substring(file.LastIndexOf(".")).ToLower();
      bool ret = false;

      if ((ext == ".ass") || (ext == ".ssa"))
      {
        ret = true;
      }

      return ret;
    }


    /// <summary>
    /// Parse the actors from the subtitle file (if possible) and populate the actors list.
    /// </summary>
    private void buttonActorCheck_Click(object sender, EventArgs e)
    {
      string[] subs1Files = null;
      string[] subs2Files = null;
     
      listBoxActors.Items.Clear();

      if (radioButtonSubs1Actor.Checked)
      {
        if (subs1FilePattern.Length == 0)
        {
          UtilsMsg.showErrMsg("Can't check - Subs1 file isn't valid.");
          return;
        }
        else
        {
          subs1Files = UtilsCommon.getNonHiddenFiles(subs1FilePattern);

          if (subs1Files.Length > 0)
          {
            foreach (string f in subs1Files)
            {
              if (!isActorSupportedSubtitleFormat(f))
              {
                UtilsMsg.showErrMsg("Can't check - Incorrect subtitle format found in Subs1 (only .ass/.ssa allowed).");
                return;
              }
            }
          }
          else
          {
            UtilsMsg.showErrMsg("Can't check - No .ass/ssa files were found in Subs1.");
            return;
          }
        }
      }
      else
      {
        if (subs2FilePattern.Length == 0)
        {
          UtilsMsg.showErrMsg("Can't check - Subs2 file isn't valid.");
          return;
        }
        else
        {
          subs2Files = UtilsCommon.getNonHiddenFiles(subs2FilePattern);

          if (subs2Files.Length > 0)
          {

            foreach (string f in subs2Files)
            {
              if (!isActorSupportedSubtitleFormat(f))
              {
                UtilsMsg.showErrMsg("Can't check - Incorrect subtitle format found in Subs2 (only .ass/.ssa allowed).");
                return;
              }
            }
          }
          else
          {
            UtilsMsg.showErrMsg("Can't check - No .ass/ssa files were found in Subs2.");
            return;
          }
        }  
      }

      string[] fileList = null;
      Encoding fileEncoding;

      int subsNum = 1;

      if (radioButtonSubs1Actor.Checked)
      {
        subsNum = 1;
        fileList = subs1Files;
        fileEncoding = Encoding.GetEncoding(InfoEncoding.longToShort(this.subs1Encoding));
      }
      else
      {
        subsNum = 2;
        fileList = subs2Files;
        fileEncoding = Encoding.GetEncoding(InfoEncoding.longToShort(this.subs2Encoding));
      }

      List<string> actorList = new List<string>();

      // Get list of actors from all episodes
      foreach (string file in fileList)
      {
        SubsParser subsParser = new SubsParserASS(null, file, fileEncoding, subsNum);
        List<InfoLine> subsLineInfos = subsParser.parse();

        foreach (InfoLine info in subsLineInfos)
        {
          string actor = info.Actor.Trim();
          if(!actorList.Contains(actor))
          {
            actorList.Add(actor);
          }
        }
      }

      foreach(string actor in actorList)
      {
        string addActor = actor;
        listBoxActors.Items.Add(addActor);
      }

      for(int i = 0; i < listBoxActors.Items.Count; i++)
      {
        listBoxActors.SetSelected(i, true);
      }
    }


    private void buttonActorAll_Click(object sender, EventArgs e)
    {
      for (int i = 0; i < listBoxActors.Items.Count; i++)
      {
        listBoxActors.SetSelected(i, true);
      }
    }


    private void buttonActorNone_Click(object sender, EventArgs e)
    {
      for (int i = 0; i < listBoxActors.Items.Count; i++)
      {
        listBoxActors.SetSelected(i, false);
      }
    }


    private void buttonActorInvert_Click(object sender, EventArgs e)
    {
      for (int i = 0; i < listBoxActors.Items.Count; i++)
      {
        listBoxActors.SetSelected(i, !listBoxActors.GetSelected(i));
      }
    }


    private void DialogAdvancedOptionsSubs_Load(object sender, EventArgs e)
    {
      errorProvider1.Clear();
      updateGUI();
    }


    private void panelColor_Click(object sender, EventArgs e)
    {
      colorDialog1.Color = ((Panel)sender).BackColor;

      if (this.colorDialog1.ShowDialog() == DialogResult.OK)
      {
        ((Panel)sender).BackColor = colorDialog1.Color;
      }
    }


    private void buttonResetColors_Click(object sender, EventArgs e)
    {
      SaveSettings defaultSettings = new SaveSettings();

      panelColorBackground.BackColor = defaultSettings.vobSubColors.Colors[0];
      panelColorText.BackColor = defaultSettings.vobSubColors.Colors[1];
      panelColorOutline.BackColor = defaultSettings.vobSubColors.Colors[2];
      panelColorAntialias.BackColor = defaultSettings.vobSubColors.Colors[3];

      checkBoxColorBackground.Checked = defaultSettings.vobSubColors.TransparencyEnabled[0];
      checkBoxColorText.Checked = defaultSettings.vobSubColors.TransparencyEnabled[1];
      checkBoxColorOutline.Checked = defaultSettings.vobSubColors.TransparencyEnabled[2];
      checkBoxColorAntialias.Checked = defaultSettings.vobSubColors.TransparencyEnabled[3];
    }


    private void checkBoxSubs1ExcludeFewer_CheckedChanged(object sender, EventArgs e)
    {
      numericUpDownSubs1ExcludeFewer.Enabled = checkBoxSubs1ExcludeFewer.Checked;
    }

    private void checkBoxSubs2ExcludeFewer_CheckedChanged(object sender, EventArgs e)
    {
      numericUpDownSubs2ExcludeFewer.Enabled = checkBoxSubs2ExcludeFewer.Checked;
    }

    private void checkBoxSubs1ExcludeShorterThanTime_CheckedChanged(object sender, EventArgs e)
    {
      numericUpDownSubs1ExcludeShorterThanTime.Enabled = checkBoxSubs1ExcludeShorterThanTime.Checked;
    }

    private void checkBoxSubs2ExcludeShorterThanTime_CheckedChanged(object sender, EventArgs e)
    {
      numericUpDownSubs2ExcludeShorterThanTime.Enabled = checkBoxSubs2ExcludeShorterThanTime.Checked;
    }

    private void checkBoxSubs1ExcludeLongerThanTime_CheckedChanged(object sender, EventArgs e)
    {
      numericUpDownSubs1ExcludeLongerThanTime.Enabled = checkBoxSubs1ExcludeLongerThanTime.Checked;
    }

    private void checkBoxSubs2ExcludeLongerThanTime_CheckedChanged(object sender, EventArgs e)
    {
      numericUpDownSubs2ExcludeLongerThanTime.Enabled = checkBoxSubs2ExcludeLongerThanTime.Checked;
    }

    private void checkBoxSubs1JoinSentences_CheckedChanged(object sender, EventArgs e)
    {
      textBoxSubs1JoinSentenceChars.Enabled = checkBoxSubs1JoinSentences.Checked;
    }

    private void checkBoxSubs2JoinSentences_CheckedChanged(object sender, EventArgs e)
    {
      textBoxSubs2JoinSentenceChars.Enabled = checkBoxSubs2JoinSentences.Checked;
    }

    private void radioButtonActor_CheckedChanged(object sender, EventArgs e)
    {
      listBoxActors.Items.Clear();
    }

    public void clearActorListbox()
    {
      listBoxActors.Items.Clear();
    }

    private void buttonOK_Click(object sender, EventArgs e)
    {
      updateSettings();
    }
  
    /// <summary>
    /// Update the global settings based on GUI.
    /// </summary>
    private void updateSettings()
    {
      Settings.Instance.Subs[0].IncludedWords = UtilsCommon.removeExtraSpaces(textBoxSubs1IncludedWords.Text.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries));
      Settings.Instance.Subs[0].ExcludedWords = UtilsCommon.removeExtraSpaces(textBoxSubs1ExcludedWords.Text.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries));
      Settings.Instance.Subs[0].RemoveNoCounterpart = checkBoxSubs1RemovedNoCounterpart.Checked;
      Settings.Instance.Subs[0].RemoveStyledLines = checkBoxSubs1RemoveStyledLines.Checked;
      Settings.Instance.Subs[0].ExcludeDuplicateLinesEnabled = checkBoxSubs1ExcludeDuplicateLines.Checked;
      Settings.Instance.Subs[0].ExcludeFewerEnabled = checkBoxSubs1ExcludeFewer.Checked;
      Settings.Instance.Subs[0].ExcludeFewerCount = (int)numericUpDownSubs1ExcludeFewer.Value;
      Settings.Instance.Subs[0].ExcludeShorterThanTimeEnabled = checkBoxSubs1ExcludeShorterThanTime.Checked;
      Settings.Instance.Subs[0].ExcludeShorterThanTime = (int)numericUpDownSubs1ExcludeShorterThanTime.Value;
      Settings.Instance.Subs[0].ExcludeLongerThanTimeEnabled = checkBoxSubs1ExcludeLongerThanTime.Checked;
      Settings.Instance.Subs[0].ExcludeLongerThanTime = (int)numericUpDownSubs1ExcludeLongerThanTime.Value;
      Settings.Instance.Subs[0].JoinSentencesEnabled = checkBoxSubs1JoinSentences.Checked;
      Settings.Instance.Subs[0].JoinSentencesCharList = textBoxSubs1JoinSentenceChars.Text.Trim();
      Settings.Instance.Subs[0].ActorsEnabled = radioButtonSubs1Actor.Checked;

      Settings.Instance.Subs[1].IncludedWords = UtilsCommon.removeExtraSpaces(textBoxSubs2IncludedWords.Text.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries));
      Settings.Instance.Subs[1].ExcludedWords = UtilsCommon.removeExtraSpaces(textBoxSubs2ExcludedWords.Text.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries));
      Settings.Instance.Subs[1].RemoveNoCounterpart = checkBoxSubs2RemoveNoCounterpart.Checked;
      Settings.Instance.Subs[1].RemoveStyledLines = checkBoxSubs2RemoveStyledLines.Checked;
      Settings.Instance.Subs[1].ExcludeDuplicateLinesEnabled = checkBoxSubs2ExcludeDuplicateLines.Checked;
      Settings.Instance.Subs[1].ExcludeFewerEnabled = checkBoxSubs2ExcludeFewer.Checked;
      Settings.Instance.Subs[1].ExcludeFewerCount = (int)numericUpDownSubs2ExcludeFewer.Value;
      Settings.Instance.Subs[1].ExcludeShorterThanTimeEnabled = checkBoxSubs2ExcludeShorterThanTime.Checked;
      Settings.Instance.Subs[1].ExcludeShorterThanTime = (int)numericUpDownSubs2ExcludeShorterThanTime.Value;
      Settings.Instance.Subs[1].ExcludeLongerThanTimeEnabled = checkBoxSubs2ExcludeLongerThanTime.Checked;
      Settings.Instance.Subs[1].ExcludeLongerThanTime = (int)numericUpDownSubs2ExcludeLongerThanTime.Value;
      Settings.Instance.Subs[1].JoinSentencesEnabled = checkBoxSubs2JoinSentences.Checked;
      Settings.Instance.Subs[1].JoinSentencesCharList = textBoxSubs2JoinSentenceChars.Text.Trim();
      Settings.Instance.Subs[1].ActorsEnabled = radioButtonSubs2Actor.Checked;

      Settings.Instance.ContextLeadingCount = (int)numericUpDownContextLeading.Value;
      Settings.Instance.ContextTrailingCount = (int)numericUpDownContextTrailing.Value;
      Settings.Instance.ContextLeadingIncludeAudioClips = checkBoxLeadingIncludeAudioClips.Checked;
      Settings.Instance.ContextLeadingIncludeSnapshots = checkBoxLeadingIncludeSnapshots.Checked;
      Settings.Instance.ContextLeadingIncludeVideoClips = checkBoxLeadingIncludeVideoClips.Checked;
      Settings.Instance.ContextLeadingRange = (int)numericUpDownLeadingRange.Value;

      Settings.Instance.ContextTrailingIncludeAudioClips = checkBoxTrailingIncludeAudioClips.Checked;
      Settings.Instance.ContextTrailingIncludeSnapshots = checkBoxTrailingIncludeSnapshots.Checked;
      Settings.Instance.ContextTrailingIncludeVideoClips = checkBoxTrailingIncludeVideoClips.Checked;
      Settings.Instance.ContextTrailingRange = (int)numericUpDownTrailingRange.Value;

      Settings.Instance.ActorList.Clear();

      for (int i = 0; i < listBoxActors.Items.Count; i++)
      {
        if (listBoxActors.GetSelected(i))
        {
          Settings.Instance.ActorList.Add((string)listBoxActors.Items[i]);
        }
      }

      Settings.Instance.LangaugeSpecific.KanjiLinesOnly = checkBoxJapKanjiOnly.Checked;

      Settings.Instance.VobSubColors.Enabled = groupBoxCheckVobsubColors.Checked;

      Settings.Instance.VobSubColors.Colors[0] = panelColorBackground.BackColor;
      Settings.Instance.VobSubColors.Colors[1] = panelColorText.BackColor;
      Settings.Instance.VobSubColors.Colors[2] = panelColorOutline.BackColor;
      Settings.Instance.VobSubColors.Colors[3] = panelColorAntialias.BackColor;

      Settings.Instance.VobSubColors.TransparencyEnabled[0] = checkBoxColorBackground.Checked;
      Settings.Instance.VobSubColors.TransparencyEnabled[1] = checkBoxColorText.Checked;
      Settings.Instance.VobSubColors.TransparencyEnabled[2] = checkBoxColorOutline.Checked;
      Settings.Instance.VobSubColors.TransparencyEnabled[3] = checkBoxColorAntialias.Checked;
    }


    /// <summary>
    /// Update GUI based on global settings.
    /// </summary>
    private void updateGUI()
    {
      try
      {
        textBoxSubs1IncludedWords.Text = UtilsCommon.makeSemiString(Settings.Instance.Subs[0].IncludedWords);
        textBoxSubs1ExcludedWords.Text = UtilsCommon.makeSemiString(Settings.Instance.Subs[0].ExcludedWords);
        checkBoxSubs1RemoveStyledLines.Checked = Settings.Instance.Subs[0].RemoveStyledLines;
        checkBoxSubs1RemovedNoCounterpart.Checked = Settings.Instance.Subs[0].RemoveNoCounterpart;
        checkBoxSubs1ExcludeDuplicateLines.Checked = Settings.Instance.Subs[0].ExcludeDuplicateLinesEnabled;
        checkBoxSubs1ExcludeFewer.Checked = Settings.Instance.Subs[0].ExcludeFewerEnabled;
        numericUpDownSubs1ExcludeFewer.Value = (decimal)Settings.Instance.Subs[0].ExcludeFewerCount;
        checkBoxSubs1ExcludeShorterThanTime.Checked = Settings.Instance.Subs[0].ExcludeShorterThanTimeEnabled;
        numericUpDownSubs1ExcludeShorterThanTime.Value = (decimal)Settings.Instance.Subs[0].ExcludeShorterThanTime;
        checkBoxSubs1ExcludeLongerThanTime.Checked = Settings.Instance.Subs[0].ExcludeLongerThanTimeEnabled;
        numericUpDownSubs1ExcludeLongerThanTime.Value = (decimal)Settings.Instance.Subs[0].ExcludeLongerThanTime;
        checkBoxSubs1JoinSentences.Checked = Settings.Instance.Subs[0].JoinSentencesEnabled;
        textBoxSubs1JoinSentenceChars.Text = Settings.Instance.Subs[0].JoinSentencesCharList;
        radioButtonSubs1Actor.Checked = Settings.Instance.Subs[0].ActorsEnabled;

        textBoxSubs2IncludedWords.Text = UtilsCommon.makeSemiString(Settings.Instance.Subs[1].IncludedWords);
        textBoxSubs2ExcludedWords.Text = UtilsCommon.makeSemiString(Settings.Instance.Subs[1].ExcludedWords);
        checkBoxSubs2RemoveNoCounterpart.Checked = Settings.Instance.Subs[1].RemoveNoCounterpart;
        checkBoxSubs2RemoveStyledLines.Checked = Settings.Instance.Subs[1].RemoveStyledLines;
        checkBoxSubs2ExcludeDuplicateLines.Checked = Settings.Instance.Subs[1].ExcludeDuplicateLinesEnabled;
        checkBoxSubs2ExcludeFewer.Checked = Settings.Instance.Subs[1].ExcludeFewerEnabled;
        numericUpDownSubs2ExcludeFewer.Value = (decimal)Settings.Instance.Subs[1].ExcludeFewerCount;
        checkBoxSubs2ExcludeShorterThanTime.Checked = Settings.Instance.Subs[1].ExcludeShorterThanTimeEnabled;
        numericUpDownSubs2ExcludeShorterThanTime.Value = Settings.Instance.Subs[1].ExcludeShorterThanTime;
        checkBoxSubs2ExcludeLongerThanTime.Checked = Settings.Instance.Subs[1].ExcludeLongerThanTimeEnabled;
        numericUpDownSubs2ExcludeLongerThanTime.Value = Settings.Instance.Subs[1].ExcludeLongerThanTime;
        checkBoxSubs2JoinSentences.Checked = Settings.Instance.Subs[1].JoinSentencesEnabled;
        textBoxSubs2JoinSentenceChars.Text = Settings.Instance.Subs[1].JoinSentencesCharList;
        radioButtonSubs2Actor.Checked = Settings.Instance.Subs[1].ActorsEnabled;

        numericUpDownContextLeading.Value = (decimal)Settings.Instance.ContextLeadingCount;
        checkBoxLeadingIncludeAudioClips.Checked = Settings.Instance.ContextLeadingIncludeAudioClips;
        checkBoxLeadingIncludeSnapshots.Checked = Settings.Instance.ContextLeadingIncludeSnapshots;
        checkBoxLeadingIncludeVideoClips.Checked = Settings.Instance.ContextLeadingIncludeVideoClips;
        numericUpDownLeadingRange.Value = (decimal)Settings.Instance.ContextLeadingRange;

        numericUpDownContextTrailing.Value = (decimal)Settings.Instance.ContextTrailingCount;
        checkBoxTrailingIncludeAudioClips.Checked = Settings.Instance.ContextTrailingIncludeAudioClips;
        checkBoxTrailingIncludeSnapshots.Checked = Settings.Instance.ContextTrailingIncludeSnapshots;
        checkBoxTrailingIncludeVideoClips.Checked = Settings.Instance.ContextTrailingIncludeVideoClips;
        numericUpDownTrailingRange.Value = (decimal)Settings.Instance.ContextTrailingRange;

        checkBoxJapKanjiOnly.Checked = Settings.Instance.LangaugeSpecific.KanjiLinesOnly;

        groupBoxCheckVobsubColors.Checked = Settings.Instance.VobSubColors.Enabled;
        panelColorBackground.BackColor = Settings.Instance.VobSubColors.Colors[0];
        panelColorText.BackColor = Settings.Instance.VobSubColors.Colors[1];
        panelColorOutline.BackColor = Settings.Instance.VobSubColors.Colors[2];
        panelColorAntialias.BackColor = Settings.Instance.VobSubColors.Colors[3];

        checkBoxColorBackground.Checked = Settings.Instance.VobSubColors.TransparencyEnabled[0];
        checkBoxColorText.Checked = Settings.Instance.VobSubColors.TransparencyEnabled[1];
        checkBoxColorOutline.Checked = Settings.Instance.VobSubColors.TransparencyEnabled[2];
        checkBoxColorAntialias.Checked = Settings.Instance.VobSubColors.TransparencyEnabled[3];
      }
      catch(Exception e1)
      {
        UtilsMsg.showErrMsg("Could not completely fill in all settings for this dialog.\n" 
          + "Did you load a file saved from a previous version?\n\n" + e1);
      }
    }




  
  
  





  }
}
