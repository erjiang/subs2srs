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
using System.Text;
using System.Windows.Forms;

namespace subs2srs
{
  /// <summary>
  /// A generic progress dialog.
  /// </summary>
	public partial class DialogProgress : Form
	{
		private bool cancelState = false;
    private int stepsCurrent;
    private int stepsTotal;
    private string stepName;
    private bool detailedProgress;
    private DateTime duration;

		public bool Cancel
		{
      get { return this.cancelState; }
		}

    public int StepsCurrent
    {
      get { return this.stepsCurrent; }
      set 
      { 
        this.stepsCurrent = value;
        this.updateTitles();
      }
    }

    public int StepsTotal
    {
      get { return this.stepsTotal; }
      set { this.stepsTotal = value; }
    }

    public string StepName
    {
      get { return this.stepName; }
      set { this.stepName = value; }
    }

    public bool DetailedProgress
    {
      get { return this.detailedProgress; }
      set
      { 
        this.detailedProgress = value;

        if (this.detailedProgress)
        {
          this.progressBarDetailed.Visible = true;
          this.ClientSize = new Size(this.ClientSize.Width, 145);
        }
        else
        {
          this.labelStatsTimeProcessed.Visible = false;
          this.labelStatsTimeProcessedValue.Visible = false;
          this.labelStatsTimeRemaining.Visible = false;
          this.labelStatsTimeRemainingValue.Visible = false;
          this.labelStatsFps.Visible = false;
          this.labelStatsFpsValue.Visible = false;
          this.labelStatsFrame.Visible = false;
          this.labelStatsFrameValue.Visible = false;

          this.progressBarDetailed.Visible = false;
          this.ClientSize = new Size(this.ClientSize.Width, 110);
        }
      }
    }

    public DateTime Duration
    {
      get { return this.duration; }
      set { this.duration = value; }
    }

    public DataReceivedEventHandler FFmpegOutputHandler
    {
      get { return this.ffmpegOutputHandler; }
    }

    
		public DialogProgress()
		{
			InitializeComponent();
      this.stepsCurrent = 0;
      this.stepsTotal = 0;
      this.stepName = "";
      this.DetailedProgress = false;
      this.updateTitles();
      this.duration = new DateTime();
		}

    private void updateTitles()
    {
      string title = "Progress";

      if ((this.stepsCurrent > 0) && (this.stepsTotal > 0))
      {
        title += String.Format(" - Step {0} of {1}", 
          this.stepsCurrent, this.stepsTotal);

        if (this.stepName.Length > 0)
        {
          title += String.Format(" - {0}", this.stepName);
        }
      }

      this.Text = title;
    }

    public void updateProgress(int progress, string text)
    {
      this.labelDesc.Text = text;

      if (progress < 0)
      {
        this.progressBarMain.Style = ProgressBarStyle.Marquee;
      }
      else
      {
        this.progressBarMain.Style = ProgressBarStyle.Blocks;
        this.progressBarMain.Value = Math.Min(100, progress);
      }
    }

    public void updateProgress(string text)
    {
      this.labelDesc.Text = text;
    }

    public void updateDetailedProgress(int progress, InfoFFmpegProgress ffmpegProgress)
    {
      this.progressBarDetailed.Value = Math.Min(100, progress);
      this.progressBarDetailed.Style = ProgressBarStyle.Marquee;

      if (progress > 0)
      {
        this.progressBarDetailed.Style = ProgressBarStyle.Blocks;
        DateTime timeRemaining = UtilsSubs.getDurationTime(ffmpegProgress.Time, duration);

        this.labelStatsTimeProcessed.Visible = true;
        this.labelStatsTimeProcessedValue.Visible = true;
        this.labelStatsTimeRemaining.Visible = true;
        this.labelStatsTimeRemainingValue.Visible = true;

        if (ffmpegProgress.VideoProgress)
        {
          this.labelStatsFps.Visible = true;
          this.labelStatsFpsValue.Visible = true;
          this.labelStatsFrame.Visible = true;
          this.labelStatsFrameValue.Visible = true;
        }
        else
        {
          this.labelStatsFps.Visible = false;
          this.labelStatsFpsValue.Visible = false;
          this.labelStatsFrame.Visible = false;
          this.labelStatsFrameValue.Visible = false;
        }

        this.labelStatsTimeProcessedValue.Text = string.Format("{0:00}:{1:00}:{2:00}",
          ffmpegProgress.Time.TimeOfDay.Hours,
          ffmpegProgress.Time.TimeOfDay.Minutes,
          ffmpegProgress.Time.TimeOfDay.Seconds);


        this.labelStatsTimeRemainingValue.Text = string.Format("{0:00}:{1:00}:{2:00}",
          timeRemaining.TimeOfDay.Hours,
          timeRemaining.TimeOfDay.Minutes,
          timeRemaining.TimeOfDay.Seconds);

        if (ffmpegProgress.VideoProgress)
        {
          this.labelStatsFpsValue.Text = string.Format("{0}", ffmpegProgress.FPS);
          this.labelStatsFrameValue.Text = string.Format("{0}", ffmpegProgress.Frame);
        }
      }
    }

		private void FormProgress_FormClosing(object sender, FormClosingEventArgs e)
		{
      this.cancelState = true;
			e.Cancel = true;
		}


    public static void updateProgressInvoke(DialogProgress dialogProgress, int progress, string text)
    {
      // Wait for thread to become avaiable
      if (dialogProgress.IsHandleCreated)
      { 
        dialogProgress.Invoke((MethodInvoker)delegate()
        {
          dialogProgress.updateProgress(progress, text);
        });
      }
    }


    public static void updateProgressInvoke(DialogProgress dialogProgress, string text)
    {
      // Wait for thread to become avaiable
      if (dialogProgress.IsHandleCreated)
      {
        dialogProgress.Invoke((MethodInvoker)delegate()
        {
          dialogProgress.updateProgress(text);
        });
      }
    }


    public static void updateDetailedProgressInvoke(DialogProgress dialogProgress, int progress, InfoFFmpegProgress ffmpegProgress)
    {
      // Wait for thread to become avaiable
      if (dialogProgress.IsHandleCreated)
      {
        dialogProgress.Invoke((MethodInvoker)delegate()
        {
          dialogProgress.updateDetailedProgress(progress, ffmpegProgress);
        });
      }
    }


    public static void nextStepInvoke(DialogProgress dialogProgress, int step, string stepName)
    {
      // Wait for thread to become avaiable
      if (dialogProgress.IsHandleCreated)
      {
        dialogProgress.Invoke((MethodInvoker)delegate()
        {
          dialogProgress.StepName = stepName;
          dialogProgress.StepsCurrent = step;
        });
      }
    }


    public static void enableDetailInvoke(DialogProgress dialogProgress, bool enabled)
    {
      // Wait for thread to become avaiable
      if (dialogProgress.IsHandleCreated)
      {
        dialogProgress.Invoke((MethodInvoker)delegate()
        {
          dialogProgress.DetailedProgress = enabled;
        });
      }
    }


    public static bool getCancelInvoke(DialogProgress dialogProgress)
    {
      bool cancelState = false;

      // Wait for thread to become avaiable
      if (dialogProgress.IsHandleCreated)
      {
        dialogProgress.Invoke((MethodInvoker)delegate()
        {
          cancelState = dialogProgress.Cancel;
        });
      }

      return cancelState;
    }

    public static void setDuration(DialogProgress dialogProgress, DateTime duration)
    {
      // Wait for thread to become avaiable
      if (dialogProgress.IsHandleCreated)
      {
        dialogProgress.Invoke((MethodInvoker)delegate()
        {
          dialogProgress.Duration = duration;
        });
      }
    }

    public static DataReceivedEventHandler getFFmpegOutputHandler(DialogProgress dialogProgress)
    {
      DataReceivedEventHandler outHandler = null;

      // Wait for thread to become avaiable
      if (dialogProgress.IsHandleCreated)
      {
        dialogProgress.Invoke((MethodInvoker)delegate()
        {
          outHandler = dialogProgress.FFmpegOutputHandler;
        });
      }

      return outHandler;
    }

    public void ffmpegOutputHandler(object sendingProcess, DataReceivedEventArgs outLine)
    {
      if (!String.IsNullOrEmpty(outLine.Data))
      {
        InfoFFmpegProgress ffmpegProgress = new InfoFFmpegProgress();

        bool parseSuccess = ffmpegProgress.parseFFmpegProgress(outLine.Data);

        if (parseSuccess)
        {
          int progress = (int)((ffmpegProgress.Time.TimeOfDay.TotalMilliseconds / Math.Max(1, Duration.TimeOfDay.TotalMilliseconds)) * 100);

          DialogProgress.updateDetailedProgressInvoke(this, progress, ffmpegProgress);

          // Debug
          //TextWriter writer = new StreamWriter("ffmpeg_output.txt", true, Encoding.UTF8);
          //writer.WriteLine(outLine.Data);
          //writer.WriteLine(String.Format("Progress: {0:00}", progress));
          //writer.Close();
        }
      }
    }






	}
}