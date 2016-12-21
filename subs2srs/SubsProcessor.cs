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
  /// Manages all subtitle, audio, snapshot, and video processing.
  /// </summary>
  class SubsProcessor
  {
    private DialogProgress dialogProgress = null;
    private DateTime workerStartTime;
    private int currentStep = 0;


    /// <summary>
    /// Start the processing.
    /// </summary>
    public void start()
    {
      start(null);
    }


    /// <summary>
    /// Start the processing in a seperate thread.
    /// If combinedAll is not null, use it rather then generating a new combinedAll.
    /// </summary>
    public void start(List<List<InfoCombined>> combinedAll)
    {
      // Create directory stucture
      try
      {
        createOutputDirStructure();
      }
      catch
      {
        UtilsMsg.showErrMsg("Cannot write to output directory. \nTry checking the directory's permissions.");
        return;
      }

      Logger.Instance.info("SubsProcessor.start");
      Logger.Instance.writeSettingsToLog();

      // Start the worker thread
      try
      {
        WorkerVars workerVars = new WorkerVars(combinedAll, getMediaDir(Settings.Instance.OutputDir, Settings.Instance.DeckName), WorkerVars.SubsProcessingType.Normal);

        // Create a background thread
        BackgroundWorker bw = new BackgroundWorker();
        bw.DoWork += new DoWorkEventHandler(bw_DoWork);
        bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);

        // Create a progress dialog on the UI thread
        dialogProgress = new DialogProgress();
        this.currentStep = 0;
        dialogProgress.StepsTotal = determineNumSteps(combinedAll);

        this.workerStartTime = DateTime.Now;
        bw.RunWorkerAsync(workerVars);

        // Lock up the UI with this modal progress form
        dialogProgress.ShowDialog();
        dialogProgress = null;
      }
      catch(Exception e1)
      {
        UtilsMsg.showErrMsg("Something went wrong before processing could start.\n"+ e1);
        return;
      }
    }


    /// <summary>
    /// Performs the work in the processing thread.
    /// </summary>
    private void bw_DoWork(object sender, DoWorkEventArgs e)
    {
      WorkerVars workerVars = e.Argument as WorkerVars;
      List<List<InfoCombined>> combinedAll = new List<List<InfoCombined>>();
      WorkerSubs subsWorker = new WorkerSubs();
      int totalLines = 0;
      bool needToGenerateCombinedAll = (workerVars.CombinedAll == null);

      // Only generate a combinedAll if one if not provided
      if (needToGenerateCombinedAll)
      {
        // Parse and combine the subtitles
        try
        {
          DialogProgress.nextStepInvoke(dialogProgress, ++currentStep, "Combine subs");

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

        foreach (List<InfoCombined> combArray in workerVars.CombinedAll)
        {
          totalLines += combArray.Count;
        }

        if (totalLines == 0)
        {
          UtilsMsg.showErrMsg("No lines of dialog could be parsed from the subtitle files.\nPlease check that they are valid.");
          e.Cancel = true;
          return;
        }

        // Inactivate lines
        try
        {
          DialogProgress.nextStepInvoke(dialogProgress, ++currentStep, "Inactivate lines");

          combinedAll = subsWorker.inactivateLines(workerVars, dialogProgress);

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
          UtilsMsg.showErrMsg("Something went wrong while setting active lines.\n" + e1);
          e.Cancel = true;
          return;
        }
      }

      // Find context lines
      if ((Settings.Instance.ContextLeadingCount > 0) || (Settings.Instance.ContextTrailingCount > 0))
      {
        try
        {
          DialogProgress.nextStepInvoke(dialogProgress, ++currentStep, "Find context lines");

          combinedAll = subsWorker.markLinesOnlyNeededForContext(workerVars, dialogProgress);

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
          UtilsMsg.showErrMsg("Something went wrong while finding context lines.\n" + e1);
          e.Cancel = true;
          return;
        }
      }

      // Remove Inactive lines (unless they are needed for context)
      try
      {
        DialogProgress.nextStepInvoke(dialogProgress, ++currentStep, "Remove inactive lines");

        combinedAll = subsWorker.removeInactiveLines(workerVars, dialogProgress, true);

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
        UtilsMsg.showErrMsg("Something went wrong while removing inactive lines.\n" + e1);
        e.Cancel = true;
        return;
      }

      totalLines = 0;
      foreach (List<InfoCombined> combArray in workerVars.CombinedAll)
      {
        totalLines += combArray.Count;
      }
      
      if (totalLines == 0)
      {
        UtilsMsg.showErrMsg("No lines will be processed. Please check your settings to make\nsure that you are not mistakenly pruning too many lines.");
        e.Cancel = true;
        return;
      }

      try
      {
        // Move vobsubs from preview dir to .media dir
        if (!needToGenerateCombinedAll)
        {
          if (!subsWorker.copyVobsubsFromPreviewDirToMediaDir(workerVars, dialogProgress))
          {
            e.Cancel = true;
            return;
          }
        }
      }
      catch
      {

      }

      // Generate SRS import file
      try
      {
        DialogProgress.nextStepInvoke(dialogProgress, ++currentStep, "Generate import file");

        WorkerSrs srsWorker = new WorkerSrs();

        if (!srsWorker.genSrs(workerVars, dialogProgress))
        {
          e.Cancel = true;
          return;
        }
      }
      catch(Exception e1)
      {
        UtilsMsg.showErrMsg("Something went wrong while generating the SRS import file.\n" + e1);
        e.Cancel = true;
        return;
      }

      List<List<InfoCombined>> combinedAllWithContext = ObjectCopier.Clone<List<List<InfoCombined>>>(workerVars.CombinedAll);
  
      // Generate audio clips
      try
      {
        if (Settings.Instance.AudioClips.Enabled)
        {
          DialogProgress.nextStepInvoke(dialogProgress, ++currentStep, "Generate audio clips");

          if(((Settings.Instance.ContextLeadingCount > 0) && Settings.Instance.ContextLeadingIncludeAudioClips)
            || ((Settings.Instance.ContextTrailingCount > 0) && Settings.Instance.ContextTrailingIncludeAudioClips))
          {
            workerVars.CombinedAll = combinedAllWithContext;
          }
          else
          {
            workerVars.CombinedAll = subsWorker.removeContextOnlyLines(combinedAllWithContext);
          }

          WorkerAudio audioWorker = new WorkerAudio();

          if (!audioWorker.genAudioClip(workerVars, dialogProgress))
          {
            e.Cancel = true;
            return;
          }
        }
      }
      catch(Exception e1)
      {
        UtilsMsg.showErrMsg("Something went wrong while generating the audio clips.\n" + e1);
        e.Cancel = true;
        return;
      }

      // Generate Snapshots
      try
      {
        if (Settings.Instance.Snapshots.Enabled)
        {
          DialogProgress.nextStepInvoke(dialogProgress, ++currentStep, "Generate snapshots");

          if (((Settings.Instance.ContextLeadingCount > 0) && Settings.Instance.ContextLeadingIncludeSnapshots)
            || ((Settings.Instance.ContextTrailingCount > 0) && Settings.Instance.ContextTrailingIncludeSnapshots))
          {
            workerVars.CombinedAll = combinedAllWithContext;
          }
          else
          {
            workerVars.CombinedAll = subsWorker.removeContextOnlyLines(combinedAllWithContext);
          }

          WorkerSnapshot snapshotWorker = new WorkerSnapshot();

          if (!snapshotWorker.genSnapshots(workerVars, dialogProgress))
          {
            e.Cancel = true;
            return;
          }
        }
      }
      catch(Exception e1)
      {
        UtilsMsg.showErrMsg("Something went wrong while generating snapshots.\n" + e1);
        e.Cancel = true;
        return;
      }

      // Generate video clips
      try
      {
        if (Settings.Instance.VideoClips.Enabled)
        {
          DialogProgress.nextStepInvoke(dialogProgress, ++currentStep, "Generate video clips");

          if (((Settings.Instance.ContextLeadingCount > 0) && Settings.Instance.ContextLeadingIncludeVideoClips)
            || ((Settings.Instance.ContextTrailingCount > 0) && Settings.Instance.ContextTrailingIncludeVideoClips))
          {
            workerVars.CombinedAll = combinedAllWithContext;
          }
          else
          {
            workerVars.CombinedAll = subsWorker.removeContextOnlyLines(combinedAllWithContext);
          }

          WorkerVideo videoWorker = new WorkerVideo();

          if (!videoWorker.genVideoClip(workerVars, dialogProgress))
          {
            e.Cancel = true;
            return;
          }
        }
      }
      catch(Exception e1)
      {
        UtilsMsg.showErrMsg("Something went wrong while generating the video clips.\n" + e1);
        e.Cancel = true;
        return;
      }

      e.Result = workerVars;
    }


    /// <summary>
    /// Gets called when all subtitle processing is finished (or cancelled).
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

      TimeSpan workerTotalTime = DateTime.Now - this.workerStartTime;
      WorkerVars workerVars = e.Result as WorkerVars;
      string srsFormat = getSrsFormatList();
      string endMessage = String.Format("Processing completed in {0:0.00} minutes.\n\n{1}",       
        workerTotalTime.TotalMinutes, srsFormat);
      UtilsMsg.showInfoMsg(endMessage);
    }


    /// <summary>
    ///  Create the needed directory stucture in the output directory.
    /// </summary>
    private void createOutputDirStructure()
    {
      Directory.CreateDirectory(getMediaDir(Settings.Instance.OutputDir, Settings.Instance.DeckName));
    }


    /// <summary>
    /// Get the full path of the media directory.
    /// </summary>
    private string getMediaDir(string outDir, string deckName)
    {
      return string.Format(@"{0}{1}{2}.media", 
                           outDir,                       // {0}
                           Path.DirectorySeparatorChar,  // {1}
                           deckName);                    // {2}
    }


    /// <summary>
    /// Get the numbered list of the SRS import file format.
    /// </summary>
    private string getSrsFormatList()
    {
      string srsFormat = "Format of the Anki import file: \n";
      int listNum = 1;

      if (ConstantSettings.SrsTagFormat != "")
      {
        srsFormat += "\n" + listNum.ToString() + ") Tag";
        listNum++;
      }

      if (ConstantSettings.SrsSequenceMarkerFormat != "")
      {
        srsFormat += "\n" + listNum.ToString() + ") Sequence Marker";
        listNum++;
      }

      if (Settings.Instance.AudioClips.Enabled)
      {
        srsFormat += "\n" + listNum.ToString() + ") Audio clip";
        listNum++;
      }

      if (Settings.Instance.Snapshots.Enabled)
      {
        srsFormat += "\n" + listNum.ToString() + ") Snapshot";
        listNum++;
      }

      if (Settings.Instance.VideoClips.Enabled)
      {
        srsFormat += "\n" + listNum.ToString() + ") Video clip";
        listNum++;
      }

      srsFormat += "\n" + listNum.ToString() + ") Line from Subs1";
      listNum++;

      if (Settings.Instance.Subs[1].FilePattern != "")
      {
        srsFormat += "\n" + listNum.ToString() + ") Line from Subs2";
      }

      if (Settings.Instance.ContextLeadingCount > 0)
      {
        string plural = "s";

        if (Settings.Instance.ContextLeadingCount == 1)
        {
          plural = "";
        }

        srsFormat += "\n+ " + Settings.Instance.ContextLeadingCount + " leading line" + plural;
      }

      if (Settings.Instance.ContextTrailingCount > 0)
      {
        string plural = "s";

        if (Settings.Instance.ContextTrailingCount == 1)
        {
          plural = "";
        }

        srsFormat += "\n+ " + Settings.Instance.ContextTrailingCount + " trailing line" + plural;
      }

      return srsFormat;
    }


    /// <summary>
    /// Determine the number of steps that will be performed (audio step, video step, etc.).
    /// </summary>
    private int determineNumSteps(List<List<InfoCombined>> combinedAll)
    {
      int numSteps = 0;

      if (combinedAll == null)
      {
        numSteps = 4;
      }
      else
      {
        numSteps = 2;
      }

      if ((Settings.Instance.ContextLeadingCount > 0) || (Settings.Instance.ContextTrailingCount > 0))
      {
        numSteps++;
      }

      if (Settings.Instance.AudioClips.Enabled)
      {
        numSteps++;
      }

      if (Settings.Instance.Snapshots.Enabled)
      {
        numSteps++;
      }

      if (Settings.Instance.VideoClips.Enabled)
      {
        numSteps++;
      }

      return numSteps;
    }


  }
}
