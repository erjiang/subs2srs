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
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace subs2srs
{
  /// <summary>
  /// Responsible for processing subtitles in the worker thread.
  /// </summary>
  class WorkerSubs
  {
    /// <summary>
    /// Pair lines from Subs1 to Subs2.
    /// 
    /// Subs2srs takes a two-pass approach when it combines Subs1 and Subs2. The first pass matches a line from 
    /// Subs1 to the closest line in Subs2. Closest here meaning the difference in start times. This will work fine
    /// assuming that both Subs1 and Subs2 have the exact same number of lines and that lines are similarly timed.
    /// Naturally, this is almost never the case. The result being that some lines will be skipped and others repeated.
    /// The second pass adds back in any lines that were skipped by combining them with others lines and doesn't allow 
    /// the same line to be repeated. 
    /// 
    /// This is used by the Main Dialog, Preview, Dueling Subtitles tools, and Extract Audio from Media Tool.
    /// </summary>
    public List<List<InfoCombined>> combineAllSubs(WorkerVars workerVars, DialogProgress dialogProgress)
    {
      List<List<InfoCombined>> combinedAll = new List<List<InfoCombined>>();
      int totalEpisodes = Settings.Instance.Subs[0].Files.Length;

      // For each episode
      for (int epIdx = 0; epIdx < totalEpisodes; epIdx++)
      {
        SubsParser subs1Parser = null;
        SubsParser subs2Parser = null;
        List<InfoLine> subs1LineInfos = null;
        List<InfoLine> subs2LineInfos = null;
        string curSub1File = Settings.Instance.Subs[0].Files[epIdx];
        string curSub2File = "";
        bool subs1ContainsVobsubs = UtilsSubs.filePatternContainsVobsubs(Settings.Instance.Subs[0].FilePattern);
        bool subs2ContainsVobsubs = UtilsSubs.filePatternContainsVobsubs(Settings.Instance.Subs[1].FilePattern);

        string progressText = String.Format("Processing subtitles for episode {0} of {1}",
                                            epIdx + 1, totalEpisodes);

        if (subs1ContainsVobsubs || subs2ContainsVobsubs)
        {
          progressText += "\nNote: Vobsubs (.idx/.sub) can take a few moments to process.";
        }

        int progress = Convert.ToInt32((epIdx + 1) * (100.0 / totalEpisodes));

        DialogProgress.updateProgressInvoke(dialogProgress, progress, progressText);

        int streamSubs1 = 0;
        int streamSubs2 = 0;

        // In the subtitles are in Vobsub format, get the stream to use 
        if (subs1ContainsVobsubs)
        {
          streamSubs1 = Convert.ToInt32(Settings.Instance.Subs[0].VobsubStream.Num);
        }

        if (subs2ContainsVobsubs)
        {
          streamSubs2 = Convert.ToInt32(Settings.Instance.Subs[1].VobsubStream.Num);
        }

        // Parse Subs1
        subs1Parser = UtilsSubs.getSubtitleParserType(workerVars, curSub1File, streamSubs1, 
          epIdx + Settings.Instance.EpisodeStartNumber, 1, Encoding.GetEncoding(Settings.Instance.Subs[0].Encoding));

        Logger.Instance.writeFileToLog(curSub1File, Encoding.GetEncoding(Settings.Instance.Subs[0].Encoding));

        subs1LineInfos = subs1Parser.parse();

        if (subs1LineInfos == null)
        {
          return null;
        }

        subs1LineInfos = removeIncorrectlyTimedLines(subs1LineInfos);

        if (Settings.Instance.Subs[0].JoinSentencesEnabled && !subs1ContainsVobsubs)
        {
          subs1LineInfos = combinePartialLinesIntoSentence(subs1LineInfos, Settings.Instance.Subs[0].JoinSentencesCharList);
        }

        // Apply Subs1 time shift
        if (Settings.Instance.TimeShiftEnabled)
        {
          foreach (InfoLine line in subs1LineInfos)
          {
            line.StartTime = UtilsSubs.shiftTiming(line.StartTime, Settings.Instance.Subs[0].TimeShift);
            line.EndTime = UtilsSubs.shiftTiming(line.EndTime, Settings.Instance.Subs[0].TimeShift);
          }
        }

        // Parse Subs2 (if needed)
        if (Settings.Instance.Subs[1].Files.Length != 0)
        {
          curSub2File = Settings.Instance.Subs[1].Files[epIdx];
          subs2Parser = UtilsSubs.getSubtitleParserType(workerVars, curSub2File, streamSubs2, 
            epIdx + Settings.Instance.EpisodeStartNumber, 2, Encoding.GetEncoding(Settings.Instance.Subs[1].Encoding));

          Logger.Instance.writeFileToLog(curSub2File, Encoding.GetEncoding(Settings.Instance.Subs[1].Encoding));

          subs2LineInfos = subs2Parser.parse();

          if (subs2LineInfos == null)
          {
            return null;
          }

          subs2LineInfos = removeIncorrectlyTimedLines(subs2LineInfos);

          if (Settings.Instance.Subs[1].JoinSentencesEnabled && !subs2ContainsVobsubs)
          {
            subs2LineInfos = combinePartialLinesIntoSentence(subs2LineInfos, Settings.Instance.Subs[1].JoinSentencesCharList);
          }

          // Apply Subs2 time shift
          if (Settings.Instance.TimeShiftEnabled)
          {
            foreach (InfoLine line in subs2LineInfos)
            {
              line.StartTime = UtilsSubs.shiftTiming(line.StartTime, Settings.Instance.Subs[1].TimeShift);
              line.EndTime = UtilsSubs.shiftTiming(line.EndTime, Settings.Instance.Subs[1].TimeShift);
            }
          }
        }
        else
        {
          subs2LineInfos = subs1LineInfos;
        }

        // Pass 1: Match each Subs1 to the closest Subs2
        List<InfoCombined> combinedSubs = this.pass1CombineSubs(subs1LineInfos, subs2LineInfos);

        if (dialogProgress.Cancel)
        {
          return null;
        }


        // Pass 2: Fix mismatches present after Pass 1.
        if (Settings.Instance.Subs[1].Files.Length != 0)
        {
          combinedSubs = this.pass2FixMismatches(combinedSubs, subs2LineInfos);

          if (dialogProgress.Cancel)
          {
            return null;
          }
        }

        // Adjust the timings based on user preference
        foreach (InfoCombined comb in combinedSubs)
        {
          // Use Subs2 timings?
          if (Settings.Instance.Subs[1].TimingsEnabled)
          {
            // Important Note:
            // The rest of the software uses subs1 for timing, so just cram subs2 timings into subs1
            comb.Subs1.StartTime = comb.Subs2.StartTime;
            comb.Subs1.EndTime = comb.Subs2.EndTime;
          }
        }

        if (dialogProgress.Cancel)
        {
          return null;
        }

        // Add this episode's paired lines
        combinedAll.Add(combinedSubs);
      }

      return combinedAll;
    }


    /// <summary>
    /// Given a line from Subs1, get the closest matching line from a list of Subs2 lines based on timestamp.
    /// May return null if Subs1 RemoveNoCounterpart is set and there is no obvious Subs2 counterpart.
    /// </summary>
    private InfoCombined getCombinedOverlap(InfoLine subs1LineInfo, List<InfoLine> subs2LineInfos)
    {
      InfoLine bestMatch = new InfoLine();
      double bestOverlap = -999999999999999999.0;
      int stopCount = 0;

      // Compare the given Subs1 against each of the Subs2 lines
      foreach (InfoLine info in subs2LineInfos)
      {
        // How well did these lines overlap (1.0 is the max)
        double overlap = UtilsSubs.getOverlap(subs1LineInfo.StartTime, subs1LineInfo.EndTime, info.StartTime, info.EndTime);

        // Did the current pair of lines overlap more than any pair thus far?
        if (overlap >= bestOverlap)
        {
          bestMatch.Actor = info.Actor;
          bestMatch.EndTime = info.EndTime;
          bestMatch.StartTime = info.StartTime;
          bestMatch.Text = info.Text;

          bestOverlap = overlap;

          stopCount = 0; // Reset
        }
        else
        {
          stopCount++;

          // Stop trying to find matches after a while
          if (stopCount == 20)
          {
            break;
          }
        }
      }

      // If no overlap, don't use provided Subs1 line
      if (Settings.Instance.Subs[0].RemoveNoCounterpart
        && (bestOverlap < 0.0))
      {
        return null;
      }

      return new InfoCombined(subs1LineInfo, bestMatch);
    }


    /// <summary>
    /// Subtitle matching Pass 1. Match each line in Subs1 to the closest line is Subs2 (based on timestamp).
    /// </summary>
    private List<InfoCombined> pass1CombineSubs(List<InfoLine> subs1LineList, List<InfoLine> subs2LineList)
    {
      List<InfoCombined> combinedSubs = new List<InfoCombined>();

      // Pass 1: Pair Subs1 and Subs2 based on timestamp
      foreach (InfoLine info in subs1LineList)
      {
        InfoCombined combInfo = this.getCombinedOverlap(info, subs2LineList);

        // If a match was found
        if (combInfo != null)
        {
          combinedSubs.Add(combInfo);
        }
      }

      return combinedSubs;
    }


    /// <summary>
    /// Subtitle matching Pass 2. Fix repeated and skipped lines caused by Pass 1.
    /// </summary>
    private List<InfoCombined> pass2FixMismatches(List<InfoCombined> combList, List<InfoLine> subs2LineList)
    {
      combList = this.combineConsecutiveRepeats(combList);
      combList = this.combineSkipped(subs2LineList, combList);

      return combList;
    }


    /// <summary>
    /// Get a list of consecutive Subs2 repeats.
    /// </summary>
    private List<InfoCombined> findConsecutiveRepeats(List<InfoCombined> combList)
    {
      List<InfoCombined> subs2Repeats = new List<InfoCombined>();
      InfoCombined prevComb = new InfoCombined(new InfoLine(), new InfoLine());

      //TextWriter writer = new StreamWriter(@"repeats.txt", false, Encoding.UTF8); // For debug

      foreach (InfoCombined comb in combList)
      {
        if (comb.Subs2.Text == prevComb.Subs2.Text
          && comb.Subs2.StartTime == prevComb.Subs2.StartTime)
        {
          subs2Repeats.Add(comb);
          //writer.WriteLine("******** REPEATED LINE: ********: " + comb.Subs1.Text + "  " + comb.Subs2.Text);
        }
        else
        {
          //writer.WriteLine(comb.Subs1.Text + "  " + comb.Subs2.Text);
          prevComb = comb;
        }
      }

      //writer.Close();

      return subs2Repeats;
    }


    /// <summary>
    /// When Subs1 and Subs2 have a different number of lines or the timings are mismatched, 
    /// then pairs can contain repeated Subs2 text. This routine will combine the repeats into
    /// a single line.
    /// 
    /// Example:
    /// 
    /// Subs1_A --> Subs2_A    \
    /// Subs1_B --> Subs2_A     | These 3 lines are repeated.
    /// Subs1_C --> Subs2_A    /
    /// Subs1_D --> Subs2_B
    /// Subs1_E --> Subs2_C
    /// 
    /// Transform the above into:
    /// 
    /// Subs1_A Subs1_B Subs1_C --> Subs2_A   ; Combined into a single line
    /// Subs1_D                 --> Subs2_B
    /// Subs1_E                 --> Subs2_C
    /// 
    /// </summary>
    private List<InfoCombined> combineConsecutiveRepeats(List<InfoCombined> combList)
    {
      List<InfoCombined> repeatList = this.findConsecutiveRepeats(combList);

      foreach (InfoCombined repeat in repeatList)
      {
        // Remove the repeat from the comb list
        combList.Remove(repeat);

        foreach (InfoCombined comb in combList)
        {
          if (comb.Subs2.StartTime == repeat.Subs2.StartTime && 
            comb.Subs2.Text == repeat.Subs2.Text)
          {
            // Add the repeat's Subs1 text to the original Subs1 (Subs1_A in the example)
            comb.Subs1.Text += " " + repeat.Subs1.Text;

            // Expand the end time of original Subs1 (Subs1_A in the example)
            comb.Subs1.EndTime = repeat.Subs1.EndTime;

            break;
          }
        }
      }

      return combList;
    }


    /// <summary>
    /// Get list of skipped Subs2 lines after the first pass of subtitle pairing.
    /// </summary>
    private List<InfoLine> findSkippedLines(List<InfoLine> subs2LineList, List<InfoCombined> combList)
    {
      List<InfoLine> skippedList = new List<InfoLine>();
      //TextWriter writer = new StreamWriter(@"skipped.txt", false, Encoding.UTF8); // For debug
      //InfoCombined foundComb = new InfoCombined(new InfoLine(), new InfoLine()); // For debug

      foreach (InfoLine line in subs2LineList) 
      {
        bool found = false;

        foreach (InfoCombined comb in combList)
        {
          if (line.Text == comb.Subs2.Text
            && line.StartTime == comb.Subs2.StartTime)
          {
            //foundComb = comb;
            found = true;
            break;
          }
        }

        if (!found)
        {
          skippedList.Add(line);
          //writer.WriteLine("******** SKIPPED LINE: ********: " + line.Text);
        }
        else
        {
          //writer.WriteLine(foundComb.Subs1.Text + "  " + line.Text);
        }
      }

     // writer.Close();

      return skippedList;
    }


    /// <summary>
    /// When Subs1 and Subs2 have different number of lines or the timings are mismatched, 
    /// then lines from Subs2 can be skipped. This routine will combine the skipped lines with 
    /// their closest match based on timestamp.
    /// 
    /// Example:
    /// 
    /// Subs1_A --> Subs2_A
    /// Subs1_B --> Subs2_C   ; Subs2_B was skipped!
    /// Subs1_C --> Subs2_D
    /// Subs1_D --> Subs2_E
    /// 
    /// Transform the above into:
    /// 
    /// Subs1_A --> Subs2_A Subs2_B
    /// Subs1_B --> Subs2_C
    /// Subs1_C --> Subs2_D
    /// Subs1_D --> Subs2_E
    /// 
    /// or:
    /// 
    /// Subs1_A --> Subs2_A 
    /// Subs1_B --> Subs2_B Subs2_C 
    /// Subs1_C --> Subs2_D
    /// Subs1_D --> Subs2_E
    /// 
    /// </summary>
    private List<InfoCombined> combineSkipped(List<InfoLine> subs2LineList, List<InfoCombined> combList)
    {
      // Get list of skipped lines
      List<InfoLine> skippedList = this.findSkippedLines(subs2LineList, combList);

      Dictionary<InfoCombined, List<InfoLine>> skipDic = new Dictionary<InfoCombined, List<InfoLine>>();

      // Associate each skipped line with the closest non-skipped line in the Subs1 list
      foreach (InfoLine skip in skippedList)
      {
        InfoCombined bestMatch = new InfoCombined(new InfoLine(), new InfoLine());
        double bestOverlap = -999999999999999999.0;

        foreach (InfoCombined comb in combList)
        {
          double overlap = UtilsSubs.getOverlap(skip.StartTime, skip.EndTime, comb.Subs1.StartTime, comb.Subs1.EndTime);

          if (overlap >= bestOverlap)
          {
            bestMatch = comb;
            bestOverlap = overlap;
          }
        }

        // If user wants to, toss out Subs2 with no obvious Subs1 counterpart
        if (Settings.Instance.Subs[1].RemoveNoCounterpart
          && (bestOverlap < 0.0))
        {
          continue;
        }
        else
        {
          // Because multiple skipped lines can match the same line from Sub1, 
          // add to dictionary so that they can be sorted lateer
          if (skipDic.ContainsKey(bestMatch))
          {
            skipDic[bestMatch].Add(skip);
          }
          else
          {
            // Add Subs1 line to dictionary as the key
            skipDic.Add(bestMatch, new List<InfoLine>());

            // Add the best matched line's Subs2 line
            skipDic[bestMatch].Add(bestMatch.Subs2);

            // Add the original skipped line
            skipDic[bestMatch].Add(skip);
          }
        }
      }

      // Sort the matched lines (for the case where multiple skipped lines match the same Subs1 line)
      foreach (List<InfoLine> infoList in skipDic.Values)
      {
        infoList.Sort();
      }

      // Now change text and time info from the skipped lines into the comb list
      foreach (InfoCombined comb in skipDic.Keys)
      {
        string subs2Text = "";

        // Combine the text
        foreach (InfoLine infoList in skipDic[comb])
        {
          subs2Text += infoList.Text + " ";
        }

        comb.Subs2.Text = subs2Text.Trim();
       
        comb.Subs2.StartTime = skipDic[comb][0].StartTime;

        int numInfoElements = skipDic[comb].Count - 1;
        comb.Subs2.EndTime = skipDic[comb][numInfoElements].EndTime;
      }

      return combList;
    }


    /// <summary>
    /// In the provided list of lines, remove any lines where Start Time >= End Time.
    /// </summary>
    private List<InfoLine> removeIncorrectlyTimedLines(List<InfoLine> infoLines)
    {
      List<InfoLine> infoLinesGood = new List<InfoLine>();

      foreach (InfoLine infoLine in infoLines)
      {
        if (infoLine.StartTime.TimeOfDay.TotalMilliseconds < infoLine.EndTime.TimeOfDay.TotalMilliseconds)
        {
          infoLinesGood.Add(infoLine);
        }
      }

      return infoLinesGood;
    }


    /// <summary>
    /// Combine lines if the last character of the line indicates that it should be joined with the next line.
    /// Example:
    /// 
    ///   LineA,
    ///   LineB.
    ///   
    ///   If comma is the character that indicates a partial line, then the above lines would be combined as:
    ///   
    ///   LineA, LineB.
    ///   
    ///  continuationChars is a string containing all of the characters that can indicate a partial line.
    ///  Example: ",、 →"
    /// 
    /// </summary>
    private List<InfoLine> combinePartialLinesIntoSentence(List<InfoLine> infoLines, string continuationChars)
    {
      if ((continuationChars.Length == 0) || (infoLines.Count == 0))
      {
        return infoLines;
      }

      List<InfoLine> outInfoLines = new List<InfoLine>();

      for (int lineIdx = 0; lineIdx < infoLines.Count; lineIdx++)
      {
        InfoLine curLine = infoLines[lineIdx];  

        if(curLine.Text.Length == 0)
        {
          continue;
        }

        string lastCharInLine = curLine.Text[curLine.Text.Length - 1].ToString();

        // If this is a partial line of a sentence, combine
        if (continuationChars.Contains(lastCharInLine))
        {
          InfoLine combinedLines;
          int endIdx = findEndLineOfCurrentSentence(infoLines, lineIdx, continuationChars, out combinedLines);
          outInfoLines.Add(combinedLines);
          lineIdx = endIdx;
        }
        else
        {
          outInfoLines.Add(curLine);
        }
      }

      return outInfoLines;
    }


    /// <summary>
    /// Given the index of the first line of a partial sentence, 
    ///   1) Find the index of the last line of the partial sentence.
    ///   2) Combine all lines between the first and last lines.
    /// </summary>
    private int findEndLineOfCurrentSentence(List<InfoLine> infoLines, int startLineIdx, 
      string continuationChars, out InfoLine combinedLines)
    {
      combinedLines = infoLines[startLineIdx];
      int lineIdx = startLineIdx + 1;

      // If given the last line, we can't go any further
      if (lineIdx >= infoLines.Count)
      {
        return startLineIdx;
      }
      
      // Find the end line
      for (; lineIdx < infoLines.Count; lineIdx++)
      {
        InfoLine curLine = infoLines[lineIdx];  

        if (curLine.Text.Length == 0)
        {
          continue;
        }

        string lastCharInLine = curLine.Text[curLine.Text.Length - 1].ToString();

        combinedLines.Text += " " + curLine.Text;
        combinedLines.EndTime = curLine.EndTime;

        // If we have found the end line of the current sentence
        if (!continuationChars.Contains(lastCharInLine))
        {
          break;
        }
      }

      return lineIdx;
    }

    
    /// <summary>
    /// In the provided list of combined lines, remove any lines where Subs1 Start Time >= Subs1 End Time.
    /// </summary>
    private List<InfoCombined> removeIncorrectlyTimedLines(List<InfoCombined> combs)
    {
      List<InfoCombined> goodCombs = new List<InfoCombined>();

      foreach (InfoCombined comb in combs)
      {
        if (comb.Subs1.StartTime.TimeOfDay.TotalMilliseconds < comb.Subs1.EndTime.TimeOfDay.TotalMilliseconds)
        {
          goodCombs.Add(comb);
        }
      }

      return goodCombs;
    }



    /// <summary>
    /// Inactivate lines from episodes based on include/exclude lists, duplicate lines, etc.
    /// Does not remove lines, only modifies their active flag.
    /// </summary>
    public List<List<InfoCombined>> inactivateLines(WorkerVars workerVars, DialogProgress dialogProgress)
    {
      int totalLines = 0;
      int progessCount = 0;

      foreach (List<InfoCombined> combArray in workerVars.CombinedAll)
      {
        totalLines += combArray.Count;
      }

      bool checkIncludesExludes = (Settings.Instance.Subs[0].IncludedWords.Length != 0) || (Settings.Instance.Subs[1].IncludedWords.Length != 0) ||
                                  (Settings.Instance.Subs[0].ExcludedWords.Length != 0) || (Settings.Instance.Subs[1].ExcludedWords.Length != 0);

      Hashtable allLinesSoFarSubs1 = new Hashtable();
      Hashtable allLinesSoFarSubs2 = new Hashtable();
      bool removeDupsSubs1 = Settings.Instance.Subs[0].ExcludeDuplicateLinesEnabled;
      bool removeDupsSubs2 = Settings.Instance.Subs[1].ExcludeDuplicateLinesEnabled && (Settings.Instance.Subs[1].FilePattern != "");
      bool checkDups = removeDupsSubs1 || removeDupsSubs2;

      // For each episode
      foreach (List<InfoCombined> combArray in workerVars.CombinedAll)
      {
        List<InfoCombined> compArrayRemoved = new List<InfoCombined>();

        // For each line in episode
        foreach (InfoCombined comb in combArray)
        {
          bool isExcluded = false;
          progessCount++;

          // Don't process if subtitles are vobsub
          bool isSubs1Vobsub = UtilsSubs.filePatternContainsVobsubs(Settings.Instance.Subs[0].FilePattern);
          bool isSubs2Vobsub = UtilsSubs.filePatternContainsVobsubs(Settings.Instance.Subs[1].FilePattern);

          // Remove lines not in the span
          if (Settings.Instance.SpanEnabled && !isExcluded)
          {
            DateTime spanStart = Settings.Instance.SpanStart;
            DateTime spanEnd = Settings.Instance.SpanEnd;

            if ((comb.Subs1.StartTime < spanStart) || (comb.Subs1.StartTime > spanEnd))
            {
              isExcluded = true;
            }
          }


          bool passedIgnoreShorterThanTime = true;

          // Make sure that line is at least the min amount of milliseconds as specified by user
          if (Settings.Instance.Subs[0].ExcludeShorterThanTimeEnabled && Settings.Instance.Subs[0].TimingsEnabled)
          {
            passedIgnoreShorterThanTime = (Math.Abs((int)comb.Subs1.EndTime.TimeOfDay.TotalMilliseconds - (int)comb.Subs1.StartTime.TimeOfDay.TotalMilliseconds) >= Settings.Instance.Subs[0].ExcludeShorterThanTime);
          }
          else if (Settings.Instance.Subs[1].ExcludeShorterThanTimeEnabled && Settings.Instance.Subs[1].TimingsEnabled)
          {
            // Note: using Subs1 here is not a mistake because Subs2 timings will be placed into Subs1 in combineSubs().
            passedIgnoreShorterThanTime = (Math.Abs((int)comb.Subs1.EndTime.TimeOfDay.TotalMilliseconds - (int)comb.Subs1.StartTime.TimeOfDay.TotalMilliseconds) >= Settings.Instance.Subs[1].ExcludeShorterThanTime);
          }

          if (!passedIgnoreShorterThanTime && !isExcluded)
          {
            isExcluded = true;
          }


          bool passedIgnoreLongerThanTime = true;

          // Make sure that line is at least the min amount of milliseconds as specified by user
          if (Settings.Instance.Subs[0].ExcludeLongerThanTimeEnabled && Settings.Instance.Subs[0].TimingsEnabled)
          {
            passedIgnoreLongerThanTime = (Math.Abs((int)comb.Subs1.EndTime.TimeOfDay.TotalMilliseconds - (int)comb.Subs1.StartTime.TimeOfDay.TotalMilliseconds) <= Settings.Instance.Subs[0].ExcludeLongerThanTime);
          }
          else if (Settings.Instance.Subs[1].ExcludeLongerThanTimeEnabled && Settings.Instance.Subs[1].TimingsEnabled)
          {
            // Note: using Subs1 here is not a mistake because Subs2 timings will be placed into Subs1 in combineSubs().
            passedIgnoreLongerThanTime = (Math.Abs((int)comb.Subs1.EndTime.TimeOfDay.TotalMilliseconds - (int)comb.Subs1.StartTime.TimeOfDay.TotalMilliseconds) <= Settings.Instance.Subs[1].ExcludeLongerThanTime);
          }

          if (!passedIgnoreLongerThanTime && !isExcluded)
          {
            isExcluded = true;
          }

          
          // Make sure that line is at least the min character length as specified by user
          bool passedIgnoreFewerTestSubs1 = 
            (isSubs1Vobsub || 
            (!Settings.Instance.Subs[0].ExcludeFewerEnabled) || 
            (Settings.Instance.Subs[0].ExcludeFewerEnabled && comb.Subs1.Text.Length >= Settings.Instance.Subs[0].ExcludeFewerCount));

          bool passedIgnoreFewerTestSubs2 = (isSubs2Vobsub || 
            (!Settings.Instance.Subs[1].ExcludeFewerEnabled) || 
            (Settings.Instance.Subs[1].ExcludeFewerEnabled && comb.Subs2.Text.Length >= Settings.Instance.Subs[1].ExcludeFewerCount));

          if ((!passedIgnoreFewerTestSubs1 || !passedIgnoreFewerTestSubs2) && !isExcluded)
          {
            isExcluded = true;
          }


          // Remove based on actors
          if (Settings.Instance.ActorList.Count > 0 && !isExcluded)
          {
            bool actorFound = false;
            foreach (string actor in Settings.Instance.ActorList)
            {   
              if (Settings.Instance.Subs[0].ActorsEnabled)
              {
                if(comb.Subs1.Actor.Trim().ToLower() == actor.Trim().ToLower())
                {
                  actorFound = true;
                  break;
                }
              }
              else
              {
                if(comb.Subs2.Actor.Trim().ToLower() == actor.Trim().ToLower())
                {
                  actorFound = true;
                  break;
                }
              }
            }
            isExcluded = !actorFound;
          }


          // Remove based on include/exclude lists
          if (checkIncludesExludes && !isExcluded)
          {
            if (!isSubs1Vobsub && Settings.Instance.Subs[0].ExcludedWords.Length != 0)
            {
              foreach (string word in Settings.Instance.Subs[0].ExcludedWords)
              {
                if (comb.Subs1.Text.ToLower().Contains(word.ToLower()))
                {
                  isExcluded = true;
                }
              }
            }

            if (!isSubs2Vobsub && Settings.Instance.Subs[1].ExcludedWords.Length != 0 && !isExcluded)
            {
              foreach (string word in Settings.Instance.Subs[1].ExcludedWords)
              {
                if (comb.Subs2.Text.ToLower().Contains(word.ToLower()))
                {
                  isExcluded = true;
                }
              }
            }

            if (!isSubs1Vobsub && Settings.Instance.Subs[0].IncludedWords.Length != 0 && !isExcluded)
            {
              bool wordFound = false;
              foreach (string word in Settings.Instance.Subs[0].IncludedWords)
              {
                if (comb.Subs1.Text.ToLower().Contains(word.ToLower()))
                {
                  wordFound = true;
                  break;
                }
              }

              isExcluded = !wordFound;
            }

            if (!isSubs2Vobsub && Settings.Instance.Subs[1].IncludedWords.Length != 0 && !isExcluded)
            {
              bool wordFound = false;
              foreach (string word in Settings.Instance.Subs[1].IncludedWords)
              {
                if (comb.Subs2.Text.ToLower().Contains(word.ToLower()))
                {
                  wordFound = true;
                  break;
                }
              }

              isExcluded = !wordFound;
            }
          }


          // Remove Duplicates
          if (checkDups && !isExcluded)
          {
            if (!isSubs1Vobsub && removeDupsSubs1)
            {
              if (allLinesSoFarSubs1.Contains(comb.Subs1.Text.ToLower()))
              {
                isExcluded = true;
              }
              else
              {
                allLinesSoFarSubs1.Add(comb.Subs1.Text.ToLower(), comb);
              }
            }

            if (!isSubs2Vobsub && removeDupsSubs2)
            {
              if (allLinesSoFarSubs2.Contains(comb.Subs2.Text.ToLower()))
              {
                isExcluded = true;
              }
              else
              {
                allLinesSoFarSubs2.Add(comb.Subs2.Text.ToLower(), comb);
              }
            }
          }


          // Remove lines without a kanji
          if (Settings.Instance.LangaugeSpecific.KanjiLinesOnly && !isExcluded)
          {
            if ((!isSubs1Vobsub && !UtilsLang.containsIdeograph(comb.Subs1.Text)) && (!isSubs2Vobsub && !UtilsLang.containsIdeograph(comb.Subs2.Text)))
            {
              isExcluded = true;
            }
          }

          // Unset the active flag
          if (isExcluded)
          {
            comb.Active = false;
          }

          string progressText = String.Format("Setting active lines based on user specified rules: {0}%",
                                              Convert.ToInt32(progessCount * (100.0 / totalLines)));

          int progress = Convert.ToInt32(progessCount * (100.0 / totalLines));

          DialogProgress.updateProgressInvoke(dialogProgress, progress, progressText);

          if (dialogProgress.Cancel)
          {
            return null;
          }
        }
      }

      return workerVars.CombinedAll;
    }


    /// <summary>
    /// Set the onlyNeededForContext flag for lines that are inactive but needed for context purposes.
    /// Call inactivateLines() before calling this routine.
    /// </summary>
    public List<List<InfoCombined>> markLinesOnlyNeededForContext(WorkerVars workerVars, DialogProgress dialogProgress)
    {
      int totalLines = 0;
      int progessCount = 0;

      foreach (List<InfoCombined> combArray in workerVars.CombinedAll)
      {
        totalLines += combArray.Count;
      }

      // For each episode
      for(int episodeIdx = 0; episodeIdx < workerVars.CombinedAll.Count; episodeIdx++)
      {
        List<InfoCombined> combArray = workerVars.CombinedAll[episodeIdx];

        // For each line in episode
        for(int lineIdx = 0; lineIdx < combArray.Count; lineIdx++)
        {
          InfoCombined comb = combArray[lineIdx];
          progessCount++;

          if (comb.Active)
          {
            // Leading
            for (int i = Math.Max(0, lineIdx - Settings.Instance.ContextLeadingCount); i < lineIdx; i++)
            {
              if (!combArray[i].Active)
              {
                combArray[i].OnlyNeededForContext = true;
              }
            }

            // Trailing
            for (int i = Math.Min(combArray.Count - 1, lineIdx + 1); i < Math.Min(combArray.Count - 1, lineIdx + Settings.Instance.ContextTrailingCount + 1); i++)
            {
              if (!combArray[i].Active)
              {
                combArray[i].OnlyNeededForContext = true;
              }
            }
          }

          string progressText = String.Format("Find lines needed for context: {0}%",
                                              Convert.ToInt32(progessCount * (100.0 / totalLines)));

          int progress = Convert.ToInt32(progessCount * (100.0 / totalLines));

          DialogProgress.updateProgressInvoke(dialogProgress, progress, progressText);

          if (dialogProgress.Cancel)
          {
            return null;
          }
        }
      }

      return workerVars.CombinedAll;
    }


    /// <summary>
    /// Get a list with the inactive lines removed from each episode.
    /// </summary>
    public List<List<InfoCombined>> removeInactiveLines(WorkerVars workerVars, DialogProgress dialogProgress, bool dontRemoveContextLines)
    {
      int totalLines = 0;
      int progessCount = 0;
      List<List<InfoCombined>> activeLines = new List<List<InfoCombined>>();

      bool subs1ContainsVobsubs = UtilsSubs.filePatternContainsVobsubs(Settings.Instance.Subs[0].FilePattern);
      bool subs2ContainsVobsubs = UtilsSubs.filePatternContainsVobsubs(Settings.Instance.Subs[1].FilePattern);

      foreach (List<InfoCombined> combArray in workerVars.CombinedAll)
      {
        totalLines += combArray.Count;
      }

      // For each episode
      foreach (List<InfoCombined> combArray in workerVars.CombinedAll)
      {
        List<InfoCombined> currentEpisodeActiveLines = new List<InfoCombined>();

        // For each line in episode
        foreach (InfoCombined comb in combArray)
        {
          progessCount++;

          if (comb.Active || (dontRemoveContextLines && comb.OnlyNeededForContext))
          {
            currentEpisodeActiveLines.Add(comb);
          }
          else
          {
            if (subs1ContainsVobsubs)
            {
              try
              {
                // Multiple vobsub images can be shown in a single line, so extract each image and delete it
                List<string> vobsubImages = UtilsSubs.extractVobsubFilesFromText(comb.Subs1.Text);

                foreach (string vobsubImage in vobsubImages)
                {
                  File.Delete(workerVars.MediaDir + Path.DirectorySeparatorChar + vobsubImage);
                }
              }
              catch
              {
                // Don't care
              }
            }

            if (subs2ContainsVobsubs)
            {
              try
              {
                // Multiple vobsub image can be shown in a single line, so extract each image and delete it
                List<string> vobsubImages = UtilsSubs.extractVobsubFilesFromText(comb.Subs2.Text);

                foreach (string vobsubImage in vobsubImages)
                {
                  File.Delete(workerVars.MediaDir + Path.DirectorySeparatorChar + vobsubImage);
                }
              }
              catch
              {
                // Don't care
              }

            }
          }

          string progressText = String.Format("Remove inactive lines: {0}%",
                                              Convert.ToInt32(progessCount * (100.0 / totalLines)));

          int progress = Convert.ToInt32(progessCount * (100.0 / totalLines));

          DialogProgress.updateProgressInvoke(dialogProgress, progress, progressText);

          if (dialogProgress.Cancel)
          {
            return null;
          }
        }

        activeLines.Add(currentEpisodeActiveLines);
      }

      return activeLines;
    }


    /// <summary>
    /// Remove the context-only lines.
    /// </summary>
    public List<List<InfoCombined>> removeContextOnlyLines(List<List<InfoCombined>> combinedAll)
    {
      List<List<InfoCombined>> activeLines = new List<List<InfoCombined>>();

      // For each episode
      foreach (List<InfoCombined> combArray in combinedAll)
      {
        List<InfoCombined> currentEpisodeActiveLines = new List<InfoCombined>();

        // For each line in episode
        foreach (InfoCombined comb in combArray)
        {
          if (comb.Active)
          {
            currentEpisodeActiveLines.Add(comb);
          }
        }

        activeLines.Add(currentEpisodeActiveLines);
      }

      return activeLines;
    }


    /// <summary>
    /// Copy the Vobsub image files from the temporary preview directory to the media directory.
    /// </summary>
    public bool copyVobsubsFromPreviewDirToMediaDir(WorkerVars workerVars, DialogProgress dialogProgress)
    {
      int totalLines = 0;
      int progessCount = 0;

      bool subs1ContainsVobsubs = UtilsSubs.filePatternContainsVobsubs(Settings.Instance.Subs[0].FilePattern);
      bool subs2ContainsVobsubs = UtilsSubs.filePatternContainsVobsubs(Settings.Instance.Subs[1].FilePattern);
      string tempPreviewDir = Path.GetTempPath() + ConstantSettings.TempPreviewDirName;

      foreach (List<InfoCombined> combArray in workerVars.CombinedAll)
      {
        totalLines += combArray.Count;
      }

      // For each episode
      foreach (List<InfoCombined> combArray in workerVars.CombinedAll)
      {
        // For each line in episode
        foreach (InfoCombined comb in combArray)
        {
          progessCount++;

          if (subs1ContainsVobsubs)
          {
            try
            {
              // Multiple vobsub image can be shown in a single line, so copy each of them
              List<string> vobsubImages = UtilsSubs.extractVobsubFilesFromText(comb.Subs1.Text);

              foreach (string vobsubImage in vobsubImages)
              {
                File.Copy(tempPreviewDir + Path.DirectorySeparatorChar + vobsubImage,
                           workerVars.MediaDir + Path.DirectorySeparatorChar + vobsubImage);
              }
            }
            catch
            {
              // Don't care
            }
          }

          if (subs2ContainsVobsubs)
          {
            try
            {
              // Multiple vobsub image can be shown in a single line, so copy each of them
              List<string> vobsubImages = UtilsSubs.extractVobsubFilesFromText(comb.Subs2.Text);

              foreach (string vobsubImage in vobsubImages)
              {
                File.Copy(tempPreviewDir + Path.DirectorySeparatorChar + vobsubImage,
                           workerVars.MediaDir + Path.DirectorySeparatorChar + vobsubImage);
              }
            }
            catch
            {
              // Don't care
            }
          }   

          string progressText = String.Format("Copying vobsubs to .media directory: {0}%",
                                              Convert.ToInt32(progessCount * (100.0 / totalLines)));

          int progress = Convert.ToInt32(progessCount * (100.0 / totalLines));

          DialogProgress.updateProgressInvoke(dialogProgress, progress, progressText);

          if (dialogProgress.Cancel)
          {
            return false;
          }
        }
      }

      return true;
    }

   
  }
}
