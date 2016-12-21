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
using System.IO;
using System.Text.RegularExpressions;
using System.Text;

namespace subs2srs
{
  /// <summary>
  /// Used for reading/writing to the settings file.
  /// </summary>
  public class PrefIO
  {
    /// <summary>
    /// Represents a key/value pair of settings
    /// </summary>
    public class SettingsPair
    {
      public string Key { get; set; }
      public string Value { get; set; }

      public SettingsPair(string key, string value)
      {
        this.Key = key;
        this.Value = value;
      }
    }

    /// <summary>
    /// Write a list of settings to the settings file.
    /// </summary>
    public static void writeString(List<SettingsPair> settingsList)
    {
      try
      {
        // Read settings file into memory
        StreamReader reader = new StreamReader(ConstantSettings.SettingsFilename, Encoding.UTF8);
        string contents = reader.ReadToEnd();
        reader.Close();

        // Replace each setting in list
        foreach (SettingsPair pair in settingsList)
        {
          string regex = "^" + pair.Key + @"\s*?=.*$";
          string replacement = pair.Key + " = " + pair.Value;
          contents = Regex.Replace(contents, regex, replacement, RegexOptions.Multiline);
        }

        // Write settings back out to file
        TextWriter writer = new StreamWriter(ConstantSettings.SettingsFilename, false, Encoding.UTF8);
        writer.Write(contents);
        writer.Close();
      }
      catch
      {
        UtilsMsg.showErrMsg("Unable to find the preferences file: '" + ConstantSettings.SettingsFilename + "'\r\nPreferences will not be saved.");
      }
    }


    /// <summary>
    /// Read a string setting.
    /// </summary>
    public static string getString(string key, string def)
    {
      string fileLine = "";
      string value = "";

      try
      {
        StreamReader settingsFile = new StreamReader(ConstantSettings.SettingsFilename, Encoding.UTF8);
     
        // Read each line of the settings file, try to find the key and its value
        while ((fileLine = settingsFile.ReadLine()) != null)
        {
          Match lineMatch = Regex.Match(fileLine,
            @"^(?<Key>" + key + @")\s*=\s*(?<Value>.*)",
            RegexOptions.IgnoreCase | RegexOptions.Compiled);

          if (lineMatch.Success)
          {
            string settingsKey = lineMatch.Groups["Key"].ToString().Trim();

            // Does this line contain the search key?
            if (settingsKey.ToLower() == key.ToLower())
            {
              // Extract the value from the line
              value = convertFromTokens(lineMatch.Groups["Value"].ToString().Trim());
              break;
            }
          }
        }

        settingsFile.Close();

        // If the value is set to "none", blank it
        if (value.ToLower() == "none")
        {
          value = "";
        }
        else if (value == "") // else if the key is not found, use the default
        {
          value = def;
        }
      }
      catch
      {
        value = def;
      }

      return value;
    }


    /// <summary>
    /// Read a boolean setting.
    /// </summary>
    public static bool getBool(string key, bool def)
    {
      string defString = def.ToString();
      string valString = "";
      bool value = false;

      valString = PrefIO.getString(key, defString);

      if (valString.ToLower() == "true")
      {
        value = true;
      }

      return value;
    }


    /// <summary>
    /// Read an integer setting.
    /// </summary>
    public static int getInt(string key, int def)
    {
      string defString = def.ToString();
      string valString = "";
      int value = 0;

      valString = PrefIO.getString(key, defString);

      try
      {
        value = Int32.Parse(valString);
      }
      catch
      {
        value = def;
      }

      return value;
    }


    /// <summary>
    /// Read a float setting.
    /// </summary>
    public static float getFloat(string key, float def)
    {
      string defString = def.ToString();
      string valString = "";
      float value = 0;

      valString = PrefIO.getString(key, defString);

      try
      {
        value = (float)UtilsLang.toDouble(valString);
      }
      catch
      {
        value = def;
      }

      return value;
    }


    /// <summary>
    /// Replace tabs and newline tokens with their with real tabs and newlines.
    /// </summary>
    public static string convertFromTokens(string format)
    {
      string newFormat = format;

      newFormat = newFormat.Replace("${tab}", "\t");
      newFormat = newFormat.Replace("${cr}", "\r");
      newFormat = newFormat.Replace("${lf}", "\n");
    
      return newFormat;
    }


    /// <summary>
    /// Read all the settings in the settings file and store globally.
    /// </summary>
    public static void read()
    {
      ConstantSettings.MainWindowWidth = getInt("main_window_width", PrefDefaults.MainWindowWidth);
      ConstantSettings.MainWindowHeight = getInt("main_window_height", PrefDefaults.MainWindowHeight);

      ConstantSettings.DefaultEnableAudioClipGeneration = getBool("default_enable_audio_clip_generation", PrefDefaults.DefaultEnableAudioClipGeneration);
      ConstantSettings.DefaultEnableSnapshotsGeneration = getBool("default_enable_snapshots_generation", PrefDefaults.DefaultEnableSnapshotsGeneration);
      ConstantSettings.DefaultEnableVideoClipsGeneration = getBool("default_enable_video_clips_generation", PrefDefaults.DefaultEnableVideoClipsGeneration);

      ConstantSettings.VideoPlayer = getString("video_player", PrefDefaults.VideoPlayer);
      ConstantSettings.VideoPlayerArgs = getString("video_player_args", PrefDefaults.VideoPlayerArgs);

      ConstantSettings.ReencodeBeforeSplittingAudio = getBool("reencode_before_splitting_audio", PrefDefaults.ReencodeBeforeSplittingAudio);
      ConstantSettings.EnableLogging = getBool("enable_logging", PrefDefaults.EnableLogging);
      ConstantSettings.AudioNormalizeArgs = getString("audio_normalize_args", PrefDefaults.AudioNormalizeArgs);
      ConstantSettings.LongClipWarningSeconds = getInt("long_clip_warning_seconds", PrefDefaults.LongClipWarningSeconds);

      ConstantSettings.DefaultAudioClipBitrate = getInt("default_audio_clip_bitrate", PrefDefaults.DefaultAudioClipBitrate);
      ConstantSettings.DefaultAudioNormalize = getBool("default_audio_normalize", PrefDefaults.DefaultAudioNormalize);

      ConstantSettings.DefaultVideoClipVideoBitrate = getInt("default_video_clip_video_bitrate", PrefDefaults.DefaultVideoClipVideoBitrate);
      ConstantSettings.DefaultVideoClipAudioBitrate = getInt("default_video_clip_audio_bitrate", PrefDefaults.DefaultVideoClipAudioBitrate);
      ConstantSettings.DefaultIphoneSupport = getBool("default_ipod_support", PrefDefaults.DefaultIphoneSupport);

      ConstantSettings.DefaultEncodingSubs1 = getString("default_encoding_subs1", PrefDefaults.DefaultEncodingSubs1);
      ConstantSettings.DefaultEncodingSubs2 = getString("default_encoding_subs2", PrefDefaults.DefaultEncodingSubs2);

      ConstantSettings.DefaultContextNumLeading = getInt("default_context_num_leading", PrefDefaults.DefaultContextNumLeading);
      ConstantSettings.DefaultContextNumTrailing = getInt("default_context_num_trailing", PrefDefaults.DefaultContextNumTrailing);

      ConstantSettings.DefaultContextLeadingRange = getInt("default_context_leading_range", PrefDefaults.DefaultContextLeadingRange);
      ConstantSettings.DefaultContextTrailingRange = getInt("default_context_trailing_range", PrefDefaults.DefaultContextTrailingRange);

      ConstantSettings.DefaultRemoveStyledLinesSubs1 = getBool("default_remove_styled_lines_subs1", PrefDefaults.DefaultRemoveStyledLinesSubs1);
      ConstantSettings.DefaultRemoveStyledLinesSubs2 = getBool("default_remove_styled_lines_subs2", PrefDefaults.DefaultRemoveStyledLinesSubs1);

      ConstantSettings.DefaultRemoveNoCounterpartSubs1 = getBool("default_remove_no_counterpart_subs1", PrefDefaults.DefaultRemoveNoCounterpartSubs1);
      ConstantSettings.DefaultRemoveNoCounterpartSubs2 = getBool("default_remove_no_counterpart_subs2", PrefDefaults.DefaultRemoveNoCounterpartSubs2);

      ConstantSettings.DefaultIncludeTextSubs1 = getString("default_included_text_subs1", PrefDefaults.DefaultIncludeTextSubs1);
      ConstantSettings.DefaultIncludeTextSubs2 = getString("default_included_text_subs2", PrefDefaults.DefaultIncludeTextSubs2);

      ConstantSettings.DefaultExcludeTextSubs1 = getString("default_excluded_text_subs1", PrefDefaults.DefaultExcludeTextSubs1);
      ConstantSettings.DefaultExcludeTextSubs2 = getString("default_excluded_text_subs2", PrefDefaults.DefaultExcludeTextSubs2);

      ConstantSettings.DefaultExcludeDuplicateLinesSubs1 = getBool("default_exclude_duplicate_lines_subs1", PrefDefaults.DefaultExcludeDuplicateLinesSubs1);
      ConstantSettings.DefaultExcludeDuplicateLinesSubs2 = getBool("default_exclude_duplicate_lines_subs2", PrefDefaults.DefaultExcludeDuplicateLinesSubs2);

      ConstantSettings.DefaultExcludeLinesFewerThanCharsSubs1 = getBool("default_exclude_lines_with_fewer_than_n_chars_subs1", PrefDefaults.DefaultExcludeLinesFewerThanCharsSubs1);
      ConstantSettings.DefaultExcludeLinesFewerThanCharsSubs2 = getBool("default_exclude_lines_with_fewer_than_n_chars_subs2", PrefDefaults.DefaultExcludeLinesFewerThanCharsSubs2);
      ConstantSettings.DefaultExcludeLinesFewerThanCharsNumSubs1 = getInt("default_exclude_lines_with_fewer_than_n_chars_num_subs1", PrefDefaults.DefaultExcludeLinesFewerThanCharsNumSubs1);
      ConstantSettings.DefaultExcludeLinesFewerThanCharsNumSubs2 = getInt("default_exclude_lines_with_fewer_than_n_chars_num_subs2", PrefDefaults.DefaultExcludeLinesFewerThanCharsNumSubs2);

      ConstantSettings.DefaultExcludeLinesShorterThanMsSubs1 = getBool("default_exclude_lines_shorter_than_n_ms_subs1", PrefDefaults.DefaultExcludeLinesShorterThanMsSubs1);
      ConstantSettings.DefaultExcludeLinesShorterThanMsSubs2 = getBool("default_exclude_lines_shorter_than_n_ms_subs2", PrefDefaults.DefaultExcludeLinesShorterThanMsSubs2);
      ConstantSettings.DefaultExcludeLinesShorterThanMsNumSubs1 = getInt("default_exclude_lines_shorter_than_n_ms_num_subs1", PrefDefaults.DefaultExcludeLinesShorterThanMsNumSubs1);
      ConstantSettings.DefaultExcludeLinesShorterThanMsNumSubs2 = getInt("default_exclude_lines_shorter_than_n_ms_num_subs2", PrefDefaults.DefaultExcludeLinesShorterThanMsNumSubs2);

      ConstantSettings.DefaultExcludeLinesLongerThanMsSubs1 = getBool("default_exclude_lines_longer_than_n_ms_subs1", PrefDefaults.DefaultExcludeLinesLongerThanMsSubs1);
      ConstantSettings.DefaultExcludeLinesLongerThanMsSubs2 = getBool("default_exclude_lines_longer_than_n_ms_subs2", PrefDefaults.DefaultExcludeLinesLongerThanMsSubs2);
      ConstantSettings.DefaultExcludeLinesLongerThanMsNumSubs1 = getInt("default_exclude_lines_longer_than_n_ms_num_subs1", PrefDefaults.DefaultExcludeLinesLongerThanMsNumSubs1);
      ConstantSettings.DefaultExcludeLinesLongerThanMsNumSubs2 = getInt("default_exclude_lines_longer_than_n_ms_num_subs2", PrefDefaults.DefaultExcludeLinesLongerThanMsNumSubs2);

      ConstantSettings.DefaultJoinSentencesSubs1 = getBool("default_join_sentences_subs1", PrefDefaults.DefaultJoinSentencesSubs1);
      ConstantSettings.DefaultJoinSentencesSubs2 = getBool("default_join_sentences_subs2", PrefDefaults.DefaultJoinSentencesSubs2);
      ConstantSettings.DefaultJoinSentencesCharListSubs1 = getString("default_join_sentences_char_list_subs1", PrefDefaults.DefaultJoinSentencesCharListSubs1);
      ConstantSettings.DefaultJoinSentencesCharListSubs2 = getString("default_join_sentences_char_list_subs2", PrefDefaults.DefaultJoinSentencesCharListSubs2);

      ConstantSettings.DefaultFileBrowserStartDir = getString("default_file_browser_start_dir", PrefDefaults.DefaultFileBrowserStartDir);

      ConstantSettings.SrsFilenameFormat = getString("srs_filename_format", PrefDefaults.SrsFilenameFormat);

      ConstantSettings.SrsDelimiter = getString("srs_delimiter", PrefDefaults.SrsDelimiter);

      ConstantSettings.SrsTagFormat = getString("srs_tag_format", PrefDefaults.SrsTagFormat);
      ConstantSettings.SrsSequenceMarkerFormat = getString("srs_sequence_marker_format", PrefDefaults.SrsSequenceMarkerFormat);

      ConstantSettings.SrsAudioFilenamePrefix = getString("srs_audio_filename_prefix", PrefDefaults.SrsAudioFilenamePrefix);
      ConstantSettings.AudioFilenameFormat = getString("audio_filename_format", PrefDefaults.AudioFilenameFormat);
      ConstantSettings.AudioId3Artist = getString("audio_id3_artist", PrefDefaults.AudioId3Artist);
      ConstantSettings.AudioId3Album = getString("audio_id3_album", PrefDefaults.AudioId3Album);
      ConstantSettings.AudioId3Title = getString("audio_id3_title", PrefDefaults.AudioId3Title);
      ConstantSettings.AudioId3Genre = getString("audio_id3_genre", PrefDefaults.AudioId3Genre);
      ConstantSettings.AudioId3Lyrics = getString("audio_id3_lyrics", PrefDefaults.AudioId3Lyrics);
      ConstantSettings.SrsAudioFilenameSuffix = getString("srs_audio_filename_suffix", PrefDefaults.SrsAudioFilenameSuffix);

      ConstantSettings.SrsSnapshotFilenamePrefix = getString("srs_snapshot_filename_prefix", PrefDefaults.SrsSnapshotFilenamePrefix);
      ConstantSettings.SnapshotFilenameFormat = getString("snapshot_filename_format", PrefDefaults.SnapshotFilenameFormat);
      ConstantSettings.SrsSnapshotFilenameSuffix = getString("srs_snapshot_filename_suffix", PrefDefaults.SrsSnapshotFilenameSuffix);

      ConstantSettings.SrsVideoFilenamePrefix = getString("srs_video_filename_prefix", PrefDefaults.SrsVideoFilenamePrefix);
      ConstantSettings.VideoFilenameFormat = getString("video_filename_format", PrefDefaults.VideoFilenameFormat);
      ConstantSettings.SrsVideoFilenameSuffix = getString("srs_video_filename_suffix", PrefDefaults.SrsVideoFilenameSuffix);

      ConstantSettings.SrsSubs1Format = getString("srs_subs1_format", PrefDefaults.SrsSubs1Format);
      ConstantSettings.SrsSubs2Format = getString("srs_subs2_format", PrefDefaults.SrsSubs2Format);

      ConstantSettings.ExtractMediaAudioFilenameFormat = getString("extract_media_audio_filename_format", PrefDefaults.ExtractMediaAudioFilenameFormat);
      ConstantSettings.ExtractMediaLyricsSubs1Format = getString("extract_media_lyrics_subs1_format", PrefDefaults.ExtractMediaLyricsSubs1Format);
      ConstantSettings.ExtractMediaLyricsSubs2Format = getString("extract_media_lyrics_subs2_format", PrefDefaults.ExtractMediaLyricsSubs2Format);

      ConstantSettings.DuelingSubtitleFilenameFormat = getString("dueling_subtitle_filename_format", PrefDefaults.DuelingSubtitleFilenameFormat);
      ConstantSettings.DuelingQuickRefFilenameFormat = getString("dueling_quick_ref_filename_format", PrefDefaults.DuelingQuickRefFilenameFormat);
      ConstantSettings.DuelingQuickRefSubs1Format = getString("dueling_quick_ref_subs1_format", PrefDefaults.DuelingQuickRefSubs1Format);
      ConstantSettings.DuelingQuickRefSubs2Format = getString("dueling_quick_ref_subs2_format", PrefDefaults.DuelingQuickRefSubs2Format);

      ConstantSettings.SrsVobsubFilenamePrefix = getString("srs_vobsub_filename_prefix", PrefDefaults.SrsVobsubFilenamePrefix);

      // NOTE: vobsub filename format is not in the settings file
      ConstantSettings.VobsubFilenameFormat = getString("vobsub_filename_format", PrefDefaults.VideoFilenameFormat);
      ConstantSettings.SrsVobsubFilenameSuffix = getString("srs_vobsub_filename_suffix", PrefDefaults.SrsVobsubFilenameSuffix);

      


    }





  }
}