using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;

namespace subs2srs
{
  /// <summary>
  /// Singleton logger.
  /// </summary>
  public class Logger
  {
    private static Mutex logFileMutex = new Mutex();
    private StringBuilder builder = new StringBuilder(1000);
    private string logFile = "log.txt";
    private bool initalized = false;
    private static readonly Logger instance = new Logger();


    // Singleton instance
    public static Logger Instance
    {
      get
      {
        return instance;
      }
    }


    private Logger()
    {
      if (!ConstantSettings.EnableLogging)
      {
        return;
      }

      DateTime now = DateTime.Now;

      logFile = String.Format("{0}log-{1:0000}{2:00}{3:00}-{4:00}{5:00}{6:00}.txt",
        ConstantSettings.LogDir,
        now.Year,
        now.Month,
        now.Day,
        now.Hour,
        now.Minute,
        now.Second);

      try
      {
        // Write blank file
        TextWriter writer = new StreamWriter(logFile, false, Encoding.UTF8);
        writer.Close();
        initalized = true;

        
        info("subs2srs version: " + UtilsAssembly.Version);

        // Windows version. 
        // Example: Microsoft Windows NT 6.1.7601 Service Pack 1
        // ------------------------------------------------------------------------------------------------------------------------------------------+
        // |           |   Windows    |   Windows    |   Windows    |Windows NT| Windows | Windows | Windows | Windows | Windows | Windows | Windows |
        // |           |     95       |      98      |     Me       |    4.0   |  2000   |   XP    |  2003   |  Vista  |  2008   |    7    | 2008 R2 |
        // +-----------------------------------------------------------------------------------------------------------------------------------------+
        // |PlatformID | Win32Windows | Win32Windows | Win32Windows | Win32NT  | Win32NT | Win32NT | Win32NT | Win32NT | Win32NT | Win32NT | Win32NT |
        // +-----------------------------------------------------------------------------------------------------------------------------------------+
        // |Major      |              |              |              |          |         |         |         |         |         |         |         |
        // | version   |      4       |      4       |      4       |    4     |    5    |    5    |    5    |    6    |    6    |    6    |    6    |
        // +-----------------------------------------------------------------------------------------------------------------------------------------+
        // |Minor      |              |              |              |          |         |         |         |         |         |         |         |
        // | version   |      0       |     10       |     90       |    0     |    0    |    1    |    2    |    0    |    0    |    1    |    1    |
        // +-----------------------------------------------------------------------------------------------------------------------------------------+
        info(System.Environment.OSVersion.VersionString);

        // Delete all except the 10 newest logs in the log directory
        DirectoryInfo folder = new DirectoryInfo(ConstantSettings.LogDir);
        List<FileInfo> files = new List<FileInfo>(folder.GetFiles());

        if (files.Count >= ConstantSettings.MaxLogFiles)
        {
          files.Sort(compareFileInfo);

          for (int i = 0; i < files.Count - ConstantSettings.MaxLogFiles; i++)
          {
            if (files[i].FullName.Contains("log-"))
            {
              File.Delete(files[i].FullName);
            }
          }
        }
      }
      catch
      {
        // Don't care
      }
    }


    /// <summary>
    /// Compare FileInfo based on LastWriteTime.
    /// </summary>
    private static int compareFileInfo(FileInfo x, FileInfo y)
    {
      return x.LastWriteTime.CompareTo(y.LastWriteTime);
    }


    /// <summary>
    /// Creeate a timestamp for the log.
    /// </summary>
    private string createTimestamp()
    {
      DateTime now = DateTime.Now;

      string time = String.Format("{0:00}:{1:00}:{2:00}.{3:000}: ", 
        now.Hour, now.Minute, now.Second, now.Millisecond);

      return time;
    }


    /// <summary>
    /// Write the log information so far to file.
    /// </summary>
    public void flush()
    {
      if (!ConstantSettings.EnableLogging || !initalized)
      {
        return;
      }

      try
      {
        logFileMutex.WaitOne();

        TextWriter writer = new StreamWriter(logFile, true, Encoding.UTF8);
        writer.WriteLine(builder.ToString());
        writer.Close();

        logFileMutex.ReleaseMutex();

        builder = new StringBuilder(1000);
      }
      catch
      {
        // Don't care
      }
    }


    /// <summary>
    /// Append line to the log.
    /// </summary>
    public void append(string text)
    {
      builder.Append(createTimestamp());
      builder.AppendLine(text);
    }

    /// <summary>
    /// Append informational line to the log.
    /// </summary>
    public void info(string text)
    {
      this.append("   " + text);
    }


    /// <summary>
    /// Append error line to the log.
    /// </summary>
    public void error(string text)
    {
      this.append("** " + text);
    }


    /// <summary>
    /// Append warning line to the log.
    /// </summary>
    public void warning(string text)
    {
      this.append("~~ " + text);
    }


    /// <summary>
    /// Add new section.
    /// </summary>
    public void startSection(string text)
    {
      string sectionText = ">> [ " + text + " ]";

      this.append(sectionText);
    }


    /// <summary>
    /// End section.
    /// </summary>
    public void endSection(string text)
    {
      string sectionText = "<< [ " + text + " ]";

      this.append(sectionText);
    }


    /// <summary>
    /// Print the name and value of a variable to the log.
    /// </summary>
    public void var<T>(T item) where T : class 
    {
      this.append(String.Format("   {0} ", item));
    }


    /// <summary>
    /// Dump the most important settings to the log.
    /// </summary>
    public void writeSettingsToLog()
    {
      if (!ConstantSettings.EnableLogging || !initalized)
      {
        return;
      }

      startSection("Settings");

      for (int i = 0; i < 2; i++)
      {
        info("Subs " + i + ":");
        var(new { Settings.Instance.Subs[i].ActorsEnabled });
        var(new { Settings.Instance.Subs[i].Encoding });
       
        string excludedWords = UtilsCommon.makeSemiString(Settings.Instance.Subs[i].ExcludedWords);
        var(new { excludedWords });

        var(new { Settings.Instance.Subs[i].RemoveNoCounterpart });
        var(new { Settings.Instance.Subs[i].RemoveStyledLines });
        var(new { Settings.Instance.Subs[i].ExcludeDuplicateLinesEnabled });
        var(new { Settings.Instance.Subs[i].ExcludeFewerCount });
        var(new { Settings.Instance.Subs[i].ExcludeFewerEnabled });
        var(new { Settings.Instance.Subs[i].ExcludeLongerThanTime });
        var(new { Settings.Instance.Subs[i].ExcludeLongerThanTimeEnabled });
        var(new { Settings.Instance.Subs[i].ExcludeShorterThanTime });
        var(new { Settings.Instance.Subs[i].ExcludeShorterThanTimeEnabled });
        var(new { Settings.Instance.Subs[i].FilePattern });

        foreach (string file in Settings.Instance.Subs[i].Files)
        {
          Logger.Instance.var(new { file });
        }

        string includedWords = UtilsCommon.makeSemiString(Settings.Instance.Subs[i].IncludedWords);
        var(new { includedWords });

        var(new { Settings.Instance.Subs[i].TimeShift });
        var(new { Settings.Instance.Subs[i].TimingsEnabled });
        var(new { Settings.Instance.Subs[i].VobsubStream });

      }

      var(new { Settings.Instance.VideoClips.Enabled });
      var(new { Settings.Instance.VideoClips.FilePattern });

      foreach (string file in Settings.Instance.VideoClips.Files)
      {
        var(new { file });
      }

      info("Audio Clips:");
      var(new { Settings.Instance.AudioClips.Enabled });
      var(new { Settings.Instance.AudioClips.filePattern });

      foreach (string file in Settings.Instance.AudioClips.Files)
      {
        var(new { file });
      }

      var(new { Settings.Instance.AudioClips.Bitrate });
      var(new { Settings.Instance.AudioClips.PadEnabled });
      var(new { Settings.Instance.AudioClips.PadStart });
      var(new { Settings.Instance.AudioClips.PadEnd });
      var(new { Settings.Instance.AudioClips.UseAudioFromVideo });
      var(new { Settings.Instance.AudioClips.UseExistingAudio });
      var(new { Settings.Instance.AudioClips.Normalize });

      info("Snapshots:");
      var(new { Settings.Instance.Snapshots.Enabled });
      var(new { Settings.Instance.Snapshots.Crop.Bottom });
      var(new { Settings.Instance.Snapshots.Size.Width });
      var(new { Settings.Instance.Snapshots.Size.Height });

      info("Video Clips:");
      var(new { Settings.Instance.VideoClips.AudioStream });
      var(new { Settings.Instance.VideoClips.BitrateAudio });
      var(new { Settings.Instance.VideoClips.BitrateVideo });
      var(new { Settings.Instance.VideoClips.Crop.Bottom });
      var(new { Settings.Instance.VideoClips.IPodSupport });
      var(new { Settings.Instance.VideoClips.PadEnabled });
      var(new { Settings.Instance.VideoClips.PadStart });
      var(new { Settings.Instance.VideoClips.PadEnd });
      var(new { Settings.Instance.VideoClips.Size.Width });
      var(new { Settings.Instance.VideoClips.Size.Height });

      info("Other:");
      var(new { Settings.Instance.ContextLeadingCount });
      var(new { Settings.Instance.ContextLeadingIncludeAudioClips });
      var(new { Settings.Instance.ContextLeadingIncludeSnapshots });
      var(new { Settings.Instance.ContextLeadingIncludeVideoClips });
      var(new { Settings.Instance.ContextLeadingRange });

      var(new { Settings.Instance.ContextTrailingCount });
      var(new { Settings.Instance.ContextTrailingIncludeAudioClips });
      var(new { Settings.Instance.ContextTrailingIncludeSnapshots });
      var(new { Settings.Instance.ContextTrailingIncludeVideoClips });
      var(new { Settings.Instance.ContextTrailingRange });

      var(new { Settings.Instance.DeckName });
      var(new { Settings.Instance.EpisodeStartNumber });
      var(new { Settings.Instance.LangaugeSpecific.KanjiLinesOnly });
      var(new { Settings.Instance.OutputDir });
      var(new { Settings.Instance.TimeShiftEnabled });
      var(new { Settings.Instance.SpanEnabled });
      var(new { Settings.Instance.SpanEnd });
      var(new { Settings.Instance.SpanStart });
      var(new { Settings.Instance.VobSubColors.Enabled });

      string vobSubColors = Settings.Instance.VobSubColors.Colors[0].ToString();
      var(new { vobSubColors });

      endSection("Settings");

      startSection("Preferences");

      var(new { ConstantSettings.SaveExt });
      var(new { HelpFile = ConstantSettings.HelpPage });
      var(new { ConstantSettings.ExeFFmpeg });
      var(new { ConstantSettings.PathFFmpegExe });
      var(new { ConstantSettings.PathFFmpegFullExe });
      var(new { ConstantSettings.PathFFmpegPresetsFull });
      var(new { ConstantSettings.TempImageFilename });
      var(new { ConstantSettings.TempVideoFilename });
      var(new { ConstantSettings.TempAudioFilename });
      var(new { ConstantSettings.TempAudioPreviewFilename });
      var(new { ConstantSettings.TempPreviewDirName });
      var(new { ConstantSettings.TempMkvExtractSubs1Filename });
      var(new { ConstantSettings.TempMkvExtractSubs2Filename });
      var(new { ConstantSettings.NormalizeAudioExe });
      var(new { ConstantSettings.PathNormalizeAudioExeRel });
      var(new { ConstantSettings.PathNormalizeAudioExeFull });
      var(new { ConstantSettings.PathSubsReTimerFull });
      var(new { ConstantSettings.PathMkvDirRel });
      var(new { ConstantSettings.PathMkvDirFull });
      var(new { ConstantSettings.PathMkvInfoExeRel });
      var(new { ConstantSettings.PathMkvInfoExeFull });
      var(new { ConstantSettings.PathMkvExtractExeRel });
      var(new { ConstantSettings.PathMkvExtractExeFull });

      string tempDirectory = Path.GetTempPath();
      var(new { tempDirectory });

      var(new { ConstantSettings.SettingsFilename });
      var(new { ConstantSettings.MainWindowWidth });
      var(new { ConstantSettings.MainWindowHeight });
      var(new { ConstantSettings.DefaultEnableAudioClipGeneration });
      var(new { ConstantSettings.DefaultEnableSnapshotsGeneration });
      var(new { ConstantSettings.DefaultEnableVideoClipsGeneration });
      var(new { ConstantSettings.VideoPlayer });
      var(new { ConstantSettings.VideoPlayerArgs });
      var(new { ConstantSettings.ReencodeBeforeSplittingAudio });
      var(new { ConstantSettings.EnableLogging });
      var(new { ConstantSettings.AudioNormalizeArgs });
      var(new { ConstantSettings.LongClipWarningSeconds });
      var(new { ConstantSettings.DefaultAudioClipBitrate });
      var(new { ConstantSettings.DefaultAudioNormalize });
      var(new { ConstantSettings.DefaultVideoClipVideoBitrate });
      var(new { ConstantSettings.DefaultVideoClipAudioBitrate });
      var(new { DefaultIphoneSupport = ConstantSettings.DefaultIphoneSupport });
      var(new { ConstantSettings.DefaultEncodingSubs1 });
      var(new { ConstantSettings.DefaultEncodingSubs2 });
      var(new { ConstantSettings.DefaultContextNumLeading });
      var(new { ConstantSettings.DefaultContextNumTrailing });
      var(new { ConstantSettings.DefaultFileBrowserStartDir });
      var(new { ConstantSettings.DefaultRemoveStyledLinesSubs1 });
      var(new { ConstantSettings.DefaultRemoveStyledLinesSubs2 });
      var(new { ConstantSettings.DefaultRemoveNoCounterpartSubs1 });
      var(new { ConstantSettings.DefaultRemoveNoCounterpartSubs2 });
      var(new { ConstantSettings.DefaultIncludeTextSubs1 });
      var(new { ConstantSettings.DefaultIncludeTextSubs2 });
      var(new { ConstantSettings.DefaultExcludeTextSubs1 });
      var(new { ConstantSettings.DefaultExcludeTextSubs2 });
      var(new { ConstantSettings.DefaultExcludeDuplicateLinesSubs1 });
      var(new { ConstantSettings.DefaultExcludeDuplicateLinesSubs2 });
      var(new { ConstantSettings.DefaultExcludeLinesFewerThanCharsSubs1 });
      var(new { ConstantSettings.DefaultExcludeLinesFewerThanCharsSubs2 });
      var(new { ConstantSettings.DefaultExcludeLinesFewerThanCharsNumSubs1 });
      var(new { ConstantSettings.DefaultExcludeLinesFewerThanCharsNumSubs2 });
      var(new { ConstantSettings.DefaultExcludeLinesShorterThanMsSubs1 });
      var(new { ConstantSettings.DefaultExcludeLinesShorterThanMsSubs2 });
      var(new { ConstantSettings.DefaultExcludeLinesShorterThanMsNumSubs1 });
      var(new { ConstantSettings.DefaultExcludeLinesShorterThanMsNumSubs2 });
      var(new { ConstantSettings.DefaultExcludeLinesLongerThanMsSubs1 });
      var(new { ConstantSettings.DefaultExcludeLinesLongerThanMsSubs2 });
      var(new { ConstantSettings.DefaultExcludeLinesLongerThanMsNumSubs1 });
      var(new { ConstantSettings.DefaultExcludeLinesLongerThanMsNumSubs2 });
      var(new { ConstantSettings.SrsFilenameFormat });
      var(new { ConstantSettings.SrsDelimiter });
      var(new { ConstantSettings.SrsTagFormat });
      var(new { ConstantSettings.SrsSequenceMarkerFormat });
      var(new { ConstantSettings.SrsAudioFilenamePrefix });
      var(new { ConstantSettings.SrsAudioFilenameSuffix });
      var(new { ConstantSettings.SrsSnapshotFilenamePrefix });
      var(new { ConstantSettings.SrsSnapshotFilenameSuffix });
      var(new { ConstantSettings.SrsVideoFilenamePrefix });
      var(new { ConstantSettings.SrsVideoFilenameSuffix });
      var(new { ConstantSettings.SrsSubs1Format });
      var(new { ConstantSettings.SrsSubs2Format });
      var(new { ConstantSettings.SrsVobsubFilenamePrefix });
      var(new { ConstantSettings.SrsVobsubFilenameSuffix });
      var(new { ConstantSettings.AudioFilenameFormat });
      var(new { ConstantSettings.SnapshotFilenameFormat });
      var(new { ConstantSettings.VideoFilenameFormat });
      var(new { ConstantSettings.VobsubFilenameFormat });
      var(new { ConstantSettings.AudioId3Artist });
      var(new { ConstantSettings.AudioId3Album });
      var(new { ConstantSettings.AudioId3Title });
      var(new { ConstantSettings.AudioId3Genre });
      var(new { ConstantSettings.AudioId3Lyrics });
      var(new { ConstantSettings.ExtractMediaAudioFilenameFormat });
      var(new { ConstantSettings.ExtractMediaLyricsSubs1Format });
      var(new { ConstantSettings.ExtractMediaLyricsSubs2Format });
      var(new { ConstantSettings.DuelingSubtitleFilenameFormat });
      var(new { ConstantSettings.DuelingQuickRefFilenameFormat });
      var(new { ConstantSettings.DuelingQuickRefSubs1Format });
      var(new { ConstantSettings.DuelingQuickRefSubs2Format });

      endSection("Preferences");

      flush();
    }


    /// <summary>
    /// Write an entire file to the log.
    /// </summary>
    public void writeFileToLog(string file, Encoding encoding)
    {
      if (!ConstantSettings.EnableLogging || !initalized)
      {
        return;
      }

      try
      {
        startSection("File: " + file);
        TextReader reader = new StreamReader(file, encoding);
        string text = reader.ReadToEnd();
        info(text);
        reader.Close();
        endSection("File: " + file);
        flush();
      }
      catch
      {
        // Don't care
      }
    }





  }
}
