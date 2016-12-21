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
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;


namespace subs2srs
{
  /// <summary>
  /// General utilies.
  /// </summary>
  public class UtilsCommon
  {
    /// <summary>
    /// Check if an integer is within it's valid range. If is isn't set to a default.
    /// </summary>
    public static int checkRange(int value, int min, int max, int def)
    {
      int newValue = def;

      if ((value >= min) && (value <= max))
      {
        newValue = value;
      }

      return newValue;
    }


    /// <summary>
    /// If value is in set of valid values, use it. Otherwise, set to the default.
    /// </summary>
    public static T checkRangeInSet<T>(T value, List<T> validValues, T def)
    {
      T outBitrate = def;

      if (validValues.Contains(value))
      {
        outBitrate = value;
      }

      return outBitrate;
    }


    /// <summary>
    /// Get the directory that the executable resides in.
    /// </summary>
    public static string getAppDir(bool addSlash)
    {
      string appDir = Application.ExecutablePath.Substring(
        0, Application.ExecutablePath.LastIndexOf(Path.DirectorySeparatorChar));

      if (addSlash)
      {
        appDir += Path.DirectorySeparatorChar;
      }

      return appDir;
    }

    /// <summary>
    /// Get the multle nearest to the provided value.
    /// </summary>
    public static int getNearestMultiple(int value, int multiple)
    {
      int ret = 0;
      int remainder = value % multiple;

      if (remainder == 0)
      {
        ret = value;
      }
      if (remainder < ((int)multiple / 2))
      {
        ret = value - remainder;
      }
      else
      {
        ret = value + multiple - remainder;
      }

      return ret;
    }

    

    /// <summary>
    ///  Get a list of non-hidden files in a directory that match the given file pattern.
    /// </summary>
    public static string[] getNonHiddenFilesInDir(string dir, string searchPatttern)
    {
      if (Directory.Exists(dir))
      {
        List<string> subsFiles = Directory.GetFiles(dir, searchPatttern, SearchOption.TopDirectoryOnly).ToList();
        subsFiles.Sort();
        
        List<string> unHiddenFiles = new List<string>();

        foreach (string file in subsFiles)
        {
          if ((File.GetAttributes(file) & FileAttributes.Hidden) != FileAttributes.Hidden)
          {
            unHiddenFiles.Add(file);
          }
        }

        return unHiddenFiles.ToArray();
      }
      else
      {
        return new string[0];
      }
    }


    /// <summary>
    ///  Get a list of non-hidden files in a directory.
    /// </summary>
    public static string[] getNonHiddenFilesInDir(string dir)
    {
      return getNonHiddenFilesInDir(dir, "*");
    }


    // File can be the full path to a dir or or it can be a dir + wildcard (D:\temp\*.mp3).
    // Valid Wildcards:
    //   * = Zero or more characters.
    //   ? = Exactly zero or one character. 

    /// <summary>
    /// Get the non-hidden files based on the provided file pattern.
    /// File pattern can be the full path to a dir or it can be a dir + wildcard (D:\temp\*.mp3).
    /// Valid Wildcards:
    ///   * = Zero or more characters.
    ///   ? = Exactly zero or one character. 
    /// </summary>
    static public string[] getNonHiddenFiles(string filePattern)
    {
      List<string> unHiddenFiles = new List<string>();
      string[] nonHiddenFiles = new string[0];

      if (filePattern.Length > 0)
      {
        string dir = Path.GetDirectoryName(filePattern);
        string pattern = filePattern.Substring(dir.Length);

        if (Directory.Exists(dir))
        {
          List<string> allFiles = Directory.GetFiles(Path.GetFullPath(dir), Path.GetFileName(pattern)).ToList();
          allFiles.Sort();

          foreach (string file in allFiles)
          {
            if ((File.GetAttributes(file) & FileAttributes.Hidden) != FileAttributes.Hidden)
            {
              unHiddenFiles.Add(file);
            }
          }

          nonHiddenFiles = unHiddenFiles.ToArray();
        }
      }

      return nonHiddenFiles;
    }


    /// <summary>
    /// Return string containing each element of provided list seperated by semicolons.
    /// </summary>
    static public string makeSemiString(string[] words)
    {
      string outStr = "";

      foreach (string word in words)
      {
        outStr += word.Trim() + ";";
      }

      outStr = outStr.TrimEnd(new char[] { ';' });

      return outStr;
    }


    /// <summary>
    /// Trim spaces from words in provided list.
    /// </summary>
    static public string[] removeExtraSpaces(string[] words)
    {
      for (int i = 0; i < words.Length; i++)
      {
        words[i] = words[i].Trim();
      }

      return words;
    }


    /// <summary>
    /// Call an exe and pass it the provided arguments. Blocking.
    /// </summary>
    static private bool callExe(string exe, string args, bool useShellExecute, bool createNoWindow)
    {
      Process process = new Process();
      bool status = false;

      try
      {
        process.StartInfo.FileName = exe;
        process.StartInfo.Arguments = args;
        process.StartInfo.UseShellExecute = useShellExecute;
        process.StartInfo.CreateNoWindow = createNoWindow;
        process.Start();

        process.WaitForExit(); // Blocking

        status = true;
      }
      catch
      {
        try
        {
          process.Kill();
        }
        catch
        {
          // Dont care
        }
      }

      return status;
    }

    /// <summary>
    /// Call an exe with the provided arguments and returns the standard out text. Blocking.
    /// </summary>
    static private string callExeAndGetStdout(string exe, string args, bool useShellExecute, bool createNoWindow)
    {
      Process process = new Process();
      string stdOutText = "Error.";

      try
      {
        process.StartInfo.FileName = exe;
        process.StartInfo.Arguments = args;
        process.StartInfo.UseShellExecute = useShellExecute;
        process.StartInfo.CreateNoWindow = createNoWindow;
        process.StartInfo.RedirectStandardOutput = true;
        process.Start();

        stdOutText = process.StandardOutput.ReadToEnd();
      }
      catch
      {
        try
        {
          process.Kill();
          stdOutText = "Error.";
        }
        catch
        {
          // Dont care
        }
      }

      return stdOutText;
    }


    /// <summary>
    /// Call an exe with the provided arguments.
    /// </summary>
    static public void startProcess(string relExePath, string fullExePath, string args,
      bool useShellExecute, bool createNoWindow)
    {
      Process process = new Process();
      bool status = true;

      // Try several different ways of calling ffmpeg because of the dreaded
      // "System.ComponentModel.Win32Exception: The system cannot find the drive specified" exception

      // Try relative path from subs2srs.exe
      status = callExe(relExePath, args, useShellExecute, createNoWindow);

      // Try absolute path to exe
      if (!status)
      {
        status = callExe(fullExePath, args, useShellExecute, createNoWindow);
      }

      // Try setting PATH to include the absolute path of exe
      if (!status)
      {
        string oldPath = Environment.GetEnvironmentVariable("Path");
        string dir = Path.GetDirectoryName(fullExePath);

        if (!oldPath.Contains(dir))
        {
          string newPath = oldPath + ";" + dir;
          Environment.SetEnvironmentVariable("Path", newPath);
        }

        status = callExe(Path.GetFileName(fullExePath), args, useShellExecute, createNoWindow);
      }
    }


    /// <summary>
    /// Call an exe with the provided arguments. Don't open a DOS window.
    /// </summary>
    static public void startProcess(string relExePath, string fullExePath, string args)
    {
      startProcess(relExePath, fullExePath, args, false, true);
    }


    /// <summary>
    /// Call an exe with the provided arguments. Don't open a DOS window.
    /// Get the standard output.
    /// </summary>
    static public string startProcessAndGetStdout(string relExePath, string fullExePath, string args)
    {
      Process process = new Process();
      string stdOutText = "Error.";

      // Try several different ways of calling because of the dreaded
      // "System.ComponentModel.Win32Exception: The system cannot find the drive specified" exception

      // Try relative path from subs2srs.exe
      stdOutText = callExeAndGetStdout(relExePath, args, false, true);

      // Try absolute path to exe
      if (stdOutText == "Error.")
      {
        stdOutText = callExeAndGetStdout(fullExePath, args, false, true);
      }

      // Try setting PATH to include the absolute path of exe
      if (stdOutText == "Error.")
      {
        string oldPath = Environment.GetEnvironmentVariable("Path");
        string dir = Path.GetDirectoryName(fullExePath);

        if (!oldPath.Contains(dir))
        {
          string newPath = oldPath + ";" + dir;
          Environment.SetEnvironmentVariable("Path", newPath);
        }

        stdOutText = callExeAndGetStdout(Path.GetFileName(fullExePath), args, false, true);
      }

      return stdOutText;
    }


    /// <summary>
    /// Call ffmpeg with provided arguments. Blocking.
    /// </summary>
    static public void startFFmpeg(string ffmpegAudioProgArgs, bool useShellExecute, bool createNoWindow)
    {
      startProcess(ConstantSettings.PathFFmpegExe, ConstantSettings.PathFFmpegFullExe, ffmpegAudioProgArgs,
        useShellExecute, createNoWindow);
    }


    /// <summary>
    /// Call ffmpeg with provided arguments and update the progress dialog.
    /// No windows will popup. If Cancel is pressed in the progress dialog, the process will be killed.
    /// </summary>
    static public void startFFmpegProgress(string ffmpegAudioProgArgs, DialogProgress dialogProgress)
    {
      Process ffmpegProcess = new Process();
      bool tryAgain = true;
      bool useShellExecute = false;
      bool createNoWindow = true;

      // Try several different ways of calling ffmpeg because of the dreaded
      // "System.ComponentModel.Win32Exception: The system cannot find the drive specified" exception

      // Try relative path from subs2srs.exe
      try
      {
        ffmpegProcess.StartInfo.FileName = ConstantSettings.PathFFmpegExe;
        ffmpegProcess.StartInfo.Arguments = ffmpegAudioProgArgs;
        ffmpegProcess.StartInfo.UseShellExecute = useShellExecute;
        ffmpegProcess.StartInfo.CreateNoWindow = createNoWindow;
        ffmpegProcess.StartInfo.RedirectStandardError = true;
        ffmpegProcess.ErrorDataReceived += new DataReceivedEventHandler(DialogProgress.getFFmpegOutputHandler(dialogProgress));
        ffmpegProcess.Start();
        ffmpegProcess.BeginErrorReadLine();

        // Loop until process has exited
        while (!ffmpegProcess.HasExited)
        {
          Thread.Sleep(100);

          // If the Cancel button was pressed
          if(DialogProgress.getCancelInvoke(dialogProgress))
          {
            try
            {
              ffmpegProcess.Kill();
            }
            catch
            {
              // Don't care
            }

            break;
          }
        }

        tryAgain = false;
      }
      catch
      {
        try
        {
          ffmpegProcess.Kill();
        }
        catch
        {
          // Dont care
        }

        tryAgain = true;
      }

      // Try absolute path to ffmpeg.exe
      if (tryAgain)
      {
        ffmpegProcess = new Process();

        try
        {
          ffmpegProcess.StartInfo.FileName = ConstantSettings.PathFFmpegFullExe;
          ffmpegProcess.StartInfo.Arguments = ffmpegAudioProgArgs;
          ffmpegProcess.StartInfo.UseShellExecute = useShellExecute;
          ffmpegProcess.StartInfo.CreateNoWindow = createNoWindow;
          ffmpegProcess.StartInfo.RedirectStandardError = true;
          ffmpegProcess.ErrorDataReceived += new DataReceivedEventHandler(DialogProgress.getFFmpegOutputHandler(dialogProgress));
          ffmpegProcess.Start();
          ffmpegProcess.BeginErrorReadLine();

          // Loop until process has exited
          while (!ffmpegProcess.HasExited)
          {
            Thread.Sleep(100);

            // If the Cancel button was pressed
            if (DialogProgress.getCancelInvoke(dialogProgress))
            {
              try
              {
                ffmpegProcess.Kill();
              }
              catch
              {
                // Don't care
              }

              break;
            }
          }

          tryAgain = false;
        }
        catch (Exception)
        {
          try
          {
            ffmpegProcess.Kill();
          }
          catch
          {
            // Dont care
          }

          tryAgain = true;
        }
      }

      // Try setting PATH to include the absolute path of ffmpeg.exe
      if (tryAgain)
      {
        try
        {
          string oldPath = Environment.GetEnvironmentVariable("Path");
          string ffmpegDir = Path.GetDirectoryName(ConstantSettings.PathFFmpegFullExe);

          if (!oldPath.Contains(ffmpegDir))
          {
            string newPath = oldPath + ";" + ffmpegDir;
            Environment.SetEnvironmentVariable("Path", newPath);
          }

          ffmpegProcess = new Process();

          ffmpegProcess.StartInfo.FileName = ConstantSettings.ExeFFmpeg;
          ffmpegProcess.StartInfo.Arguments = ffmpegAudioProgArgs;
          ffmpegProcess.StartInfo.UseShellExecute = useShellExecute;
          ffmpegProcess.StartInfo.CreateNoWindow = createNoWindow;
          ffmpegProcess.StartInfo.RedirectStandardError = true;
          ffmpegProcess.ErrorDataReceived += new DataReceivedEventHandler(DialogProgress.getFFmpegOutputHandler(dialogProgress));
          ffmpegProcess.Start();
          ffmpegProcess.BeginErrorReadLine();

          // Loop until process has exited
          while (!ffmpegProcess.HasExited)
          {
            Thread.Sleep(100);

            // If the Cancel button was pressed
            if (DialogProgress.getCancelInvoke(dialogProgress))
            {
              try
              {
                ffmpegProcess.Kill();
              }
              catch
              {
                // Don't care
              }

              break;
            }
          }
        }
        catch
        {
          // Don't care
        }
      }
    }


    /// <summary>
    /// Call ffmpeg with the provided arguments. Return the ffmpeg console text.
    /// </summary>
    static public string getFFmpegText(string ffmpegAudioProgArgs)
    {
      Process ffmpegProcess = new Process();
      bool tryAgain = true;
      bool useShellExecute = false;
      bool createNoWindow = true;
      string output = "";

      // Try several different ways of calling ffmpeg because of the dreaded
      // "System.ComponentModel.Win32Exception: The system cannot find the drive specified" exception

      // Try relative path from subs2srs.exe
      try
      {
        ffmpegProcess.StartInfo.FileName = ConstantSettings.PathFFmpegExe;
        ffmpegProcess.StartInfo.Arguments = ffmpegAudioProgArgs;
        ffmpegProcess.StartInfo.UseShellExecute = useShellExecute;
        ffmpegProcess.StartInfo.RedirectStandardError = true;
        ffmpegProcess.StartInfo.CreateNoWindow = createNoWindow;
        ffmpegProcess.Start();

        output = ffmpegProcess.StandardError.ReadToEnd();

        tryAgain = false;
      }
      catch
      {
        try
        {
          ffmpegProcess.Kill();
        }
        catch
        {
          // Dont care
        }

        tryAgain = true;
      }

      // Try absolute path to ffmpeg.exe
      if (tryAgain)
      {
        ffmpegProcess = new Process();

        try
        {
          ffmpegProcess.StartInfo.FileName = ConstantSettings.PathFFmpegFullExe;
          ffmpegProcess.StartInfo.Arguments = ffmpegAudioProgArgs;
          ffmpegProcess.StartInfo.UseShellExecute = useShellExecute;
          ffmpegProcess.StartInfo.RedirectStandardError = true;
          ffmpegProcess.StartInfo.CreateNoWindow = createNoWindow;
          ffmpegProcess.Start();

          output = ffmpegProcess.StandardError.ReadToEnd();

          tryAgain = false;
        }
        catch (Exception)
        {
          try
          {
            ffmpegProcess.Kill();
          }
          catch
          {
            // Dont care
          }

          tryAgain = true;
        }
      }

      // Try setting PATH to include the absolute path of ffmpeg.exe
      if (tryAgain)
      {
        string oldPath = Environment.GetEnvironmentVariable("Path");
        string ffmpegDir = Path.GetDirectoryName(ConstantSettings.PathFFmpegFullExe);

        if (!oldPath.Contains(ffmpegDir))
        {
          string newPath = oldPath + ";" + ffmpegDir;
          Environment.SetEnvironmentVariable("Path", newPath);
        }

        ffmpegProcess = new Process();

        ffmpegProcess.StartInfo.FileName = ConstantSettings.ExeFFmpeg;
        ffmpegProcess.StartInfo.Arguments = ffmpegAudioProgArgs;
        ffmpegProcess.StartInfo.UseShellExecute = useShellExecute;
        ffmpegProcess.StartInfo.RedirectStandardError = true;
        ffmpegProcess.StartInfo.CreateNoWindow = createNoWindow;
        ffmpegProcess.Start();

        output = ffmpegProcess.StandardError.ReadToEnd();
      }

      return output;
    }







  }
}
