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
using System.Diagnostics;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;


namespace subs2srs
{
  /// <summary>
  /// Utilities related to video.
  /// </summary>
  public class UtilsVideo
  {
    /// <summary>
    /// Video codec to use.
    /// </summary>
    public enum VideoCodec
    {
      COPY, // Copy, don't re-encode
      h264,
      MPEG4
    }

    /// <summary>
    /// Audio codec to use.
    /// </summary>
    public enum AudioCodec
    {
      COPY,  // Copy, don't re-encode
      AAC,
      MP3
    }

    /// <summary>
    /// x264 preset to use. libx264-XXX.ffpreset.
    /// </summary>
    public enum Presetx264
    {
      None,      // Don't use a preset
      UltraFast, // The worst quality (fast!)
      SuperFast, // This is probably a good enough defualt. The speed is on par with MPEG4.
      VeryFast, 
      Faster,   
      Fast,     
      Medium,   
      Slow,     
      Slower,   
      VerySlow, 
      Placebo,  // The best quality (slow!!!)
    }

    /// <summary>
    /// x264 profile to use.
    /// </summary>
    public enum Profilex264
    {
      None,     // Don't use use a profile
      IPod320,  // This is for very old iPods
      IPod640,  // This is for newer iPods
      Baseline, 
      Main,
      High,     // ffmpeg defaults to High
    }


    /// <summary>
    /// Format a video codec argument for ffmpeg.
    /// </summary>
    public static string formatVideoCodecArg(VideoCodec videoCodec)
    {
      string videoCodecArg = "";
      string command = "-codec:v ";

      switch (videoCodec)
      {
        case VideoCodec.COPY: videoCodecArg = command + "copy"; break;
        case VideoCodec.h264: videoCodecArg = command + "libx264"; break;
        case VideoCodec.MPEG4: videoCodecArg = command + " mpeg4"; break;
        default: videoCodecArg = ""; break;
      }

      return videoCodecArg;
    }


    /// <summary>
    /// Format an audio codec argument for ffmpeg.
    /// </summary>
    public static string formatAudioCodecArg(AudioCodec audioCodec)
    {
      string audioCodecArg = "";
      string command = "-codec:a ";

      switch (audioCodec)
      {
        case AudioCodec.COPY:
          audioCodecArg = command + "copy";
          break;

        case AudioCodec.AAC: 
          audioCodecArg = command + "aac";
          break;

        case AudioCodec.MP3:
          audioCodecArg = command + "mp3";
          break;

        default: audioCodecArg = ""; 
          break;
      }

      return audioCodecArg;
    }


    /// <summary>
    /// Format a x264 profile argument for ffmpeg.
    /// </summary>
    public static string formatProfileFileArg(Profilex264 preset)
    {
      string presetCommand = "-fpre";
      string profileArg = "";
      string profileFile = "";
      string profilePath = "";

      switch (preset)
      {
        case Profilex264.IPod320: profileFile = "libx264-ipod320.ffpreset"; break;
        case Profilex264.IPod640: profileFile = "libx264-ipod640.ffpreset"; break;
        case Profilex264.Baseline: profileFile = "libx264-baseline.ffpreset"; break;
        case Profilex264.Main: profileFile = "libx264-main.ffpreset"; break;
        case Profilex264.High: profileFile = ""; break;
        case Profilex264.None: profileFile = ""; break;
        default: profileFile = ""; break;
      }

      if (profileFile.Length > 0)
      {
        profilePath = Path.Combine(ConstantSettings.PathFFmpegPresetsFull, profileFile);

        // Example: -fpre "E:\subs2srs\subs2srs\bin\Release\Utils\ffmpeg\presets\libx264-ipod640.ffpreset"
        profileArg = String.Format("{0} \"{1}\"", presetCommand, profilePath);
      }

      return profileArg;
    }


    /// <summary>
    /// Format a x264 preset argument for ffmpeg.
    /// Note: The options for -profile can be found here:
    /// http://www.linuxcertif.com/man/1/x264/
    /// </summary>
    public static string formatPresetFileArg(Presetx264 preset)
    {
      string preseteArg = "";
      string presetText = "";

      switch (preset)
      {
        case Presetx264.UltraFast: presetText = "ultrafast"; break;
        case Presetx264.SuperFast: presetText = "superfast"; break;
        case Presetx264.VeryFast: presetText = "veryfast"; break;
        case Presetx264.Faster: presetText = "faster"; break;
        case Presetx264.Fast: presetText = "fast"; break;
        case Presetx264.Medium: presetText = "medium"; break;
        case Presetx264.Slow: presetText = "slow"; break;
        case Presetx264.Slower: presetText = "slower"; break;
        case Presetx264.VerySlow: presetText = "veryslow"; break;
        case Presetx264.Placebo: presetText = "placebo"; break;
        case Presetx264.None: presetText = ""; break;
        default: presetText = ""; break;
      }

      if (presetText.Length > 0)
      {
        // Example: -preset superfast
        preseteArg = String.Format("-preset {0}", presetText);
      }

      return preseteArg;
    }


    /// <summary>
    /// Format a keyframe option. These are needed because x264 can only cut 
    /// on a keyframe and the default keyframe is interval is HUGE!
    /// </summary>
    public static string formatKeyframeOptionsArg(VideoCodec videoCodec)
    {
      string keyframeOptionsArg = "";

      switch (videoCodec)
      {
        case VideoCodec.h264: keyframeOptionsArg = "-g 6 -keyint_min 6"; break;
        default: keyframeOptionsArg = ""; break;
      }

      return keyframeOptionsArg;
    }


    /// <summary>
    /// Format a video map argument for ffmpeg.
    /// </summary>
    public static string formatVideoMapArg()
    {
      string videoMapArg = "-map 0:0";

      return videoMapArg;
    }


    /// <summary>
    /// Format a audio map argument for ffmpeg.
    /// </summary>
    public static string formatAudioMapArg(string audioStream)
    {
      string audioMapArg = "";

      if ((audioStream.Length > 0) && (audioStream != "-"))
      {
        // Example: -map 0:1
        audioMapArg = "-map " + audioStream;
      }

      return audioMapArg;
    }


    /// <summary>
    /// Format an audio bitrate argument for ffmpeg.
    /// </summary>
    public static string formatAudioBitrateArg(int bitrate)
    {
      string bitrateArg = String.Format("-b:a {0}k", bitrate);

      return bitrateArg;
    }


    /// <summary>
    /// Format a start time for ffmpeg.
    /// </summary>
    public static string formatStartTimeArg(DateTime startTime)
    {
      // Example: -ss 00:00:07.920
      string startTimeArg = String.Format("-ss {0:00.}:{1:00.}:{2:00.}.{3:000.}",
                                  (int)startTime.TimeOfDay.TotalHours,      // {0}
                                  (int)startTime.TimeOfDay.Minutes,         // {1}
                                  (int)startTime.TimeOfDay.Seconds,         // {2}
                                  (int)startTime.TimeOfDay.Milliseconds);   // {3}

      return startTimeArg;
    }


    /// <summary>
    /// Format a duration argument for ffmpeg.
    /// </summary>
    public static string formatDurationArg(DateTime startTime, DateTime endTime)
    {
      DateTime diffTime = UtilsSubs.getDurationTime(startTime, endTime);

      // Example: -ss 00:00:07.920 -t 00:24:35.120
      string durationArg = String.Format("-t {0:00.}:{1:00.}:{2:00.}.{3:000.}",
                                 (int)diffTime.TimeOfDay.TotalHours,       // {0}
                                 (int)diffTime.TimeOfDay.Minutes,          // {1}
                                 (int)diffTime.TimeOfDay.Seconds,          // {2}
                                 (int)diffTime.TimeOfDay.Milliseconds);    // {3}

      return durationArg;

    }


    /// <summary>
    /// Format a start time and duration argument for ffmpeg.
    /// </summary>
    public static string formatStartTimeAndDurationArg(DateTime startTime, DateTime endTime)
    {
      string startTimeArg = UtilsVideo.formatStartTimeArg(startTime);
      string durationArg = UtilsVideo.formatDurationArg(startTime, endTime);

      DateTime diffTime = UtilsSubs.getDurationTime(startTime, endTime);

      // Example: -ss 00:00:07.920 -t 00:24:35.120
      string timeArg = String.Format("{0} {1}",
                              startTimeArg, // {0}
                              durationArg); // {1}

      return timeArg;
    }


    /// <summary>
    ///  Format a video size (scale) argument for ffmpeg.
    /// </summary>
    public static string formatVideoSizeArg(string inFile, ImageSize size, ImageCrop crop, int width_multiple, int height_multiple)
    {
      string videoSizeArg = "";
      ImageSize outSize = size;

      outSize.Width = UtilsCommon.getNearestMultiple(outSize.Width, width_multiple);
      outSize.Height = UtilsCommon.getNearestMultiple(outSize.Height, height_multiple);

      videoSizeArg = String.Format("scale={0}:{1}",
                                   outSize.Width,    // {0}
                                   outSize.Height);  // {1}

      return videoSizeArg;
    }


    /// <summary>
    ///  Format a video crop argument for ffmpeg.
    /// </summary>
    public static string formatCropArg(string file, ImageSize size, ImageCrop crop)
    {
      ImageSize outSize = UtilsVideo.getResolutionToUseWithCrop(file, size, crop);

      string cropArg = String.Format("crop={0}:{1}:{2}:{3}",
                                      outSize.Width - crop.Left - crop.Right,  // {0}
                                      outSize.Height - crop.Top - crop.Bottom, // {1}
                                      crop.Left,                               // {2}
                                      crop.Top);                               // {3}

      return cropArg;
    }


    /// <summary>
    /// When using a crop, get the resolution to pass to ffmpeg in order to preserve aspect ratio.
    /// </summary>
    public static ImageSize getResolutionToUseWithCrop(string file, ImageSize desiredSize, ImageCrop crop)
    {
      ImageSize outSize;
      int totalCropWidth = crop.Left + crop.Right;
      int totalCropHeight = crop.Top + crop.Bottom;

      if (totalCropWidth == 0 && totalCropHeight == 0)
      {
         outSize = desiredSize;
      }
      else
      {
        ImageSize origSize = UtilsVideo.getVideoResolution(file);

        // output_resolution = desired_resolution + (total_crop - (desired_resolution/orig_resolution) * total_crop))
        outSize = new ImageSize();
        outSize.Width = desiredSize.Width + (totalCropWidth - (int)((desiredSize.Width / (float)origSize.Width) * totalCropWidth));
        outSize.Height = desiredSize.Height + (totalCropHeight - (int)((desiredSize.Height / (float)origSize.Height) * totalCropHeight));
      }
 
      return outSize;
    }


    /// <summary>
    /// Convert the input video using the specified options.
    /// 
    /// Note:
    /// h.264 and .mp4 have timing/cutting issues. h.264 only cuts on the last keyframe, 
    /// which could be several seconds before the time that you actually want to cut.
    /// 
    /// When cutting an .mp4 (even with MPEG4 video and MP3 audio), the cut will take place  
    /// ~0.5 seconds before it should.
    /// 
    /// (Is this still true?)
    /// </summary>
    public static void convertVideo(string inFile, string audioStream, DateTime startTime, DateTime endTime,
      ImageSize size, ImageCrop crop, int bitrateVideo, int bitrateAudio, VideoCodec videoCodec, AudioCodec audioCodec,
      Profilex264 profile, Presetx264 preset, string outFile, DialogProgress dialogProgress)
    {
      string videoMapArg = UtilsVideo.formatVideoMapArg();
      string audioMapArg = UtilsVideo.formatAudioMapArg(audioStream);

      string videoCodecArg = UtilsVideo.formatVideoCodecArg(videoCodec);

      string presetArg = UtilsVideo.formatPresetFileArg(preset);
      string keyframeOptionsArg = UtilsVideo.formatKeyframeOptionsArg(videoCodec);
      string profileArg = UtilsVideo.formatProfileFileArg(profile);

      string videoSizeArg = UtilsVideo.formatVideoSizeArg(inFile, size, crop, 16, 2);
      string videoBitrateArg = String.Format("-b:v {0}k", bitrateVideo);

      string audioCodecArg = UtilsVideo.formatAudioCodecArg(audioCodec);
      string audioBitrateArg = UtilsVideo.formatAudioBitrateArg(bitrateAudio);

      string timeArg = UtilsVideo.formatStartTimeAndDurationArg(startTime, endTime);

      string cropArg = UtilsVideo.formatCropArg(inFile, size, crop);

      string threadsArg = "-threads 0";

      string ffmpegConvertArgs = "";

      // Good ffmpeg resource: http://howto-pages.org/ffmpeg/
      // 0:0 is assumed to be the video stream
      // Audio stream: 0:n where n is the number of the audio stream (usually 1)
      //
      // Example format: 
      // -y -i "G:\Temp\input.mkv" -ac 2 -map 0:v:0 -map 0:a:0 -codec:v libx264 -preset superfast -g 6 -keyint_min 6 
      // -fpre "E:\subs2srs\subs2srs\bin\Release\Utils\ffmpeg\presets\libx264-ipod640.ffpreset" 
      // -b:v 800k -codec:a aac -b:a 128k -ss 00:03:32.420 -t 00:02:03.650 -vf "scale 352:202, crop=352:202:0:0" -threads 0
      // "C:\Documents and Settings\cb4960\Local Settings\Temp\~subs2srs_temp.mp4"
      ffmpegConvertArgs = String.Format("-y -i \"{0}\" -ac 2 {1} {2} {3} {4} {5} {6} {7} {8} {9}" +
                                        " {10} -vf \"{11}, {12}\" {13} \"{14}\" ",
                                        // Input video file name                    
                                        inFile,                   // {0}

                                        // Mapping
                                        videoMapArg,              // {1}
                                        audioMapArg,              // {2}

                                        // Video
                                        videoCodecArg,            // {3}
                                        presetArg,                // {4}
                                        keyframeOptionsArg,       // {5}
                                        profileArg,               // {6}
                                        videoBitrateArg,          // {7}

                                        // Audio
                                        audioCodecArg,            // {8}
                                        audioBitrateArg,          // {9}

                                        // Time span
                                        timeArg,                  // {10}

                                        // Filters
                                        videoSizeArg,             // {11}
                                        cropArg,                  // {12}

                                        // Threads
                                        threadsArg,               // {13}

                                        // Output video file name
                                        outFile);                 // {14}


      if (dialogProgress == null)
      {
        UtilsCommon.startFFmpeg(ffmpegConvertArgs, true, true);
      }
      else
      {
        UtilsCommon.startFFmpegProgress(ffmpegConvertArgs, dialogProgress);
      }
    }


    /// <summary>
    /// Extract a video clip from a longer video clip without re-encoding.
    /// </summary>
    public static void cutVideo(string inFile, DateTime startTime, DateTime endTime, string outFile)
    {
      string startTimeArg = UtilsVideo.formatStartTimeArg(startTime);
      string durationArg = UtilsVideo.formatDurationArg(startTime, endTime);
      string timeArg = formatStartTimeAndDurationArg(startTime, endTime);

      string ffmpegCutArgs = "";

      // Example format:
      // -y -i -ss 00:00:00.000 -t "input.avi" -t 00:00:01.900 -c copy "output.avi"
      // Note: The order of the arguments is strange, but unless -ss comes before -i,
      //       the video will SOMETIMES fail to copy and the output will consist
      //       of only the audio. Lots of experimentation involved.
      ffmpegCutArgs = String.Format("-y {0} -i \"{1}\" {2} -c copy \"{3}\"",
                                    // Start time
                                    startTimeArg,             // {0}

                                    // Input video file name
                                    inFile,                   // {1}

                                    // Duration
                                    durationArg,              // {2}

                                    // Output video file name 
                                    outFile);                 // {3}

      UtilsCommon.startFFmpeg(ffmpegCutArgs, false, true);
    }



    /// <summary>
    /// Return the ffmpeg information string for a media file.
    /// </summary>
    public static string getVideoInfoStr(string file)
    {
      string procArgs = "";

      // Example format: -i "example.mkv"
      procArgs = String.Format("-i \"{0}\"", file);

      string output = UtilsCommon.getFFmpegText(procArgs);

      return output;
    }


    /// <summary>
    /// Extract the video resolution from the ffmpeg information string.
    /// </summary>
    public static ImageSize getVideoResolution(string file)
    {
      string videoInfo = "";

      videoInfo = getVideoInfoStr(file);

      /* EXAMPLE OUTPUT:
        $ ./ffmpeg -i test.mkv
        FFmpeg version SVN-r16573, Copyright (c) 2000-2009 Fabrice Bellard, et al.
          configuration: --extra-cflags=-fno-common --enable-memalign-hack --enable-pthreads ...
          libavutil     49.12. 0 / 49.12. 0
          libavcodec    52.10. 0 / 52.10. 0
          libavformat   52.23. 1 / 52.23. 1
          libavdevice   52. 1. 0 / 52. 1. 0
          libswscale     0. 6. 1 /  0. 6. 1
          built on Jan 13 2009 03:17:03, gcc: 4.2.4
        Input #0, matroska, from 'test.mkv':
          Duration: 00:24:09.96, start: 0.000000, bitrate: N/A
            Stream #0.0(eng): Video: h264, yuv420p, 1024x576, 23.98 tb(r)           <--------------- Here
            Stream #0.1(jpn): Audio: aac, 48000 Hz, stereo, s16
            Stream #0.2(eng): Subtitle: 0x0000
            Stream #0.3(eng): Subtitle: 0x0000
            Stream #0.4: Subtitle: 0x0000
            Stream #0.5: Attachment: 0x0000
            Stream #0.6: Attachment: 0x0000
            Stream #0.7: Attachment: 0x0000
        At least one output file must be specified
      */

      Match lineMatch = Regex.Match(videoInfo,
        @"^.*Video: .*?,.*?, (?<ResH>\d*?)x(?<ResV>\d*)", RegexOptions.Compiled | RegexOptions.Multiline);

      if (!lineMatch.Success)
      {
        throw new Exception("Could not determine video resolution.");
      }

      ImageSize size = new ImageSize();

      try
      {
        size.Width = Int32.Parse(lineMatch.Groups["ResH"].ToString().Trim());
        size.Height = Int32.Parse(lineMatch.Groups["ResV"].ToString().Trim());
      }
      catch (Exception e1)
      {
        throw new Exception("Could not determine video resolution - Invalid video resolution format.\n" + e1);
      }

      return size;
    }


    /// <summary>
    /// Extract the video length from the ffmpeg information string.
    /// </summary>
    public static DateTime getVideoLength(string file)
    {
      DateTime videoLength = new DateTime();
      string videoInfo = "";

      videoInfo = getVideoInfoStr(file);

      /* EXAMPLE OUTPUT:
        $ ./ffmpeg -i test.mkv
        FFmpeg version SVN-r16573, Copyright (c) 2000-2009 Fabrice Bellard, et al.
          configuration: --extra-cflags=-fno-common --enable-memalign-hack --enable-pthreads ...
          libavutil     49.12. 0 / 49.12. 0
          libavcodec    52.10. 0 / 52.10. 0
          libavformat   52.23. 1 / 52.23. 1
          libavdevice   52. 1. 0 / 52. 1. 0
          libswscale     0. 6. 1 /  0. 6. 1
          built on Jan 13 2009 03:17:03, gcc: 4.2.4
        Input #0, matroska, from 'test.mkv':
          Duration: 00:24:09.96, start: 0.000000, bitrate: N/A                      <--------------- Here
            Stream #0.0(eng): Video: h264, yuv420p, 1024x576, 23.98 tb(r)
            Stream #0.1(jpn): Audio: aac, 48000 Hz, stereo, s16
            Stream #0.2(eng): Subtitle: 0x0000
            Stream #0.3(eng): Subtitle: 0x0000
            Stream #0.4: Subtitle: 0x0000
            Stream #0.5: Attachment: 0x0000
            Stream #0.6: Attachment: 0x0000
            Stream #0.7: Attachment: 0x0000
        At least one output file must be specified
      */

      Match lineMatch = Regex.Match(videoInfo,
        "^.*Duration: (?<VideoLength>.*?),.*", RegexOptions.Compiled | RegexOptions.Multiline);

      if (!lineMatch.Success)
      {
        throw new Exception("Could not determine video duration.");
      }

      string rawVideoLength = lineMatch.Groups["VideoLength"].ToString().Trim();

      Match lineMatchVideoLength = Regex.Match(rawVideoLength,
        @"^(?<VideoHr>.*?):(?<VideoMin>.*?):(?<VideoSec>.*?)\.(?<VideoHsec>.*?)$", RegexOptions.Compiled);

      try
      {
        videoLength = videoLength.AddHours(Int32.Parse(lineMatchVideoLength.Groups["VideoHr"].ToString().Trim()));
        videoLength = videoLength.AddMinutes(Int32.Parse(lineMatchVideoLength.Groups["VideoMin"].ToString().Trim()));
        videoLength = videoLength.AddSeconds(Int32.Parse(lineMatchVideoLength.Groups["VideoSec"].ToString().Trim()));
        videoLength = videoLength.AddMilliseconds(Int32.Parse(lineMatchVideoLength.Groups["VideoHsec"].ToString().Trim()) * 10);
      }
      catch (Exception e1)
      {
        throw new Exception("Could not determine video duration - Invalid time format.\n" + e1);
      }

      return videoLength;
    }



    /// <summary>
    /// Get the available audio streams from a video file
    /// </summary>
    /// <param name="file">The video file</param>
    public static List<InfoStream> getAvailableAudioStreams(string file)
    {
      string videoInfo = "";
      List<InfoStream> streamInfos = new List<InfoStream>();

      videoInfo = getVideoInfoStr(file);

      MatchCollection lineMatches = Regex.Matches(videoInfo,
        @".*Stream #(?<AudioStreamNum>\d\:\d)\(?(?<AudioStreamLang>\w*)\)?: Audio: (?<AudioStreamType>.*)",
        RegexOptions.Compiled);
  
      if (lineMatches.Count != 0)
      {
        int count = 0;
        foreach (Match match in lineMatches)
        {
          string streamNum = match.Groups["AudioStreamNum"].ToString().Trim();
          streamNum = streamNum.Replace(".", ":");

          string streamLang = UtilsLang.LangThreeLetter2Full(match.Groups["AudioStreamLang"].ToString().Trim());
          string streamType = match.Groups["AudioStreamType"].ToString().Trim();

          streamInfos.Add(new InfoStream(streamNum, count.ToString(), streamLang, streamType));

          count++;
        }
      }

      return streamInfos;
    }










  }
}
