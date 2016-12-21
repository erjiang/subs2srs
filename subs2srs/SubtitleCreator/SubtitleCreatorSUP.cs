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

// This file was taken from the Subtitle Creator project and modified to fit the needs of subs2srs.
// http://sourceforge.net/projects/subtitlecreator/
//

//
// See http://dvd.sourceforge.net/dvdinfo/index.html for hacked-together DVD subtitle specs.
//

using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.ComponentModel;
using System.Windows.Forms;
using System.Collections.Generic;

namespace SubtitleCreator
{



  /// <summary>
  /// Summary description for SUP.
  /// </summary>
  public class SUP
  {
    // cb4960, v26.5: Hackish. Wrote following 3 constants to support larger Blu-ray subtitles (as opposed to DVD sizes).
    const int MAX_WIDTH = 1920; // Original value was 720
    const int MAX_HEIGHT_NTSC = 1080; // Original value was 480
    const int MAX_HEIGHT_PAL = 1080; // Original value was 576

    const byte TRANSPARENT = 0;

    public Color[] Colors = new Color[16];
    public Color backgndColor, foregndColor, outlineColor, antialiasColor;
    public bool backgndTransparency, foregndTransparency, outlineTransparency, antialiasTransparency;
    public int[] colorIndex = { 0, 1, 2, 3 };
    public int ShowEverySUP = 2;
    private Color[] preferedSubtitleColors = new Color[4];

    public int VideoSizeX, VideoSizeY;
    public bool IsPAL = true;

    public void SetPAL()
    {
      IsPAL = true;

      VideoSizeX = MAX_WIDTH;
      VideoSizeY = MAX_HEIGHT_PAL;
    }

    public void SetNTSC()
    {
      IsPAL = false;

      VideoSizeX = MAX_WIDTH;
      VideoSizeY = MAX_HEIGHT_NTSC;
    }


    // Class containing all necessary data for each SUP in the SUP file
    private class SUPdata
    {
      // Pointers
      public long nextSUP;
      public long startControlSeq;
      public long startBitmapInterleaved1;
      public long startBitmapInterleaved2;

      // Characteristic data
      public byte[] ColorSet;
      public byte[] newColorSet;
      public byte[] Transparency;
      public byte[] ChgColCon;
      // HD Extension
      public int[] HDColorSet; // Y Cr Cb
      public int[] HDnewColorSet;
      public byte[] HDTransparency;

      public Color[] customColors;
      public bool[] customTransparency;

      // VobSub
      public string SUPlanguage; // This parameter is significant for VobSub subtitles only

      public DateTime PackageStartTime;
      public DateTime PackageEndTime;

      private Bitmap _SubtitlePicture;
      private Rectangle _SubtitleBorders;		// denotes the area within the bitmap that contains text
      public Rectangle BitmapPos;				// denotes the postion of the bitmap on the screen

      public SUPdata()
      {
        ColorSet = new byte[4];
        Transparency = new byte[4];
        BitmapPos = new Rectangle(0, 0, 0, 0);
        HDColorSet = new int[256];
        HDTransparency = new byte[256];

        PackageStartTime = new DateTime(0);
        PackageEndTime = new DateTime(0);

        _SubtitleBorders = new Rectangle(0, 0, 0, 0);
      }

      public void Dispose()
      {
        if (_SubtitlePicture != null)
          _SubtitlePicture.Dispose();
        _SubtitlePicture = null;
      }

      public void SetSubtitlePicture(ref Bitmap bmp)
      {
        if (_SubtitleBorders.Width > 0 && _SubtitleBorders.Height > 0)
        {
          RectangleF subtitleArea = new RectangleF(_SubtitleBorders.X, _SubtitleBorders.Y,
            _SubtitleBorders.Width, _SubtitleBorders.Height);

          _SubtitlePicture = bmp.Clone(subtitleArea, PixelFormat.Format24bppRgb);
        }

        _SubtitleBorders.X = 0;
        _SubtitleBorders.Y = 0;
      }

      /// <summary>
      /// Get the bitmap as it is stored (24bpp, no transparency)
      /// </summary>
      public Bitmap SubtitlePicture
      {
        get
        {
          if (_SubtitlePicture == null)
          {
            // Create the bitmap
            _SubtitlePicture = new Bitmap(
              BitmapPos.Width, BitmapPos.Height, PixelFormat.Format24bppRgb);
          }

          return _SubtitlePicture;
        }
      }

      const byte TRANSPARENT = 0;
      public int GetTransparent()
      {
        // Get the transparent color
        for (int i = 0; i < Transparency.Length; i++)
          if (Transparency[i] == TRANSPARENT)
            return i;
        return -1;
      }

      public Rectangle SubtitleBorders
      {
        get
        {
          return _SubtitleBorders;
        }
      }

      public Rectangle GetSubtitleRectangle()
      {
        return new Rectangle(0, 0, _SubtitlePicture.Width, _SubtitlePicture.Height);
      }

      /// <summary>
      /// Determines the size of the subtitle, which can be used as the sourceRectangle,
      /// so only the useful information is copied to the destination.
      /// </summary>
      /// <param name="Xmin">Left most point of the subtitle border</param>
      /// <param name="Xmax">Right most point of the subtitle border</param>
      /// <param name="Ymin">Top most point of the subtitle border</param>
      /// <param name="Ymax">Bottom most point of the subtitle border</param>
      public void SetSubtitleBorders(int Xmin, int Xmax, int Ymin, int Ymax)
      {
        if ((Xmax - Xmin + 1) % 2 == 1)
        {
          // Width is odd: only correct it if we've shrunken the bitmap
          if (Xmax - Xmin >= BitmapPos.Width)
          {
            if (Xmin > 0)
              Xmin--;
            else
              Xmax++;
            //else
            //	Xmin++;
          }
        }
        _SubtitleBorders.Width = Xmax - Xmin + 1;

        if ((Ymax - Ymin + 1) % 2 == 1)
        {
          // Height is odd
          if (Ymin > 0)
            Ymin--;
          else
            Ymin++;
        }
        _SubtitleBorders.Height = Ymax - Ymin + 1;

        //todo
        //_SubtitleBorders.X = Math.Min(Xmin, DVDinfo.Instance.VideoSizeX -_SubtitleBorders.Width);
        //_SubtitleBorders.Y = Math.Min(Ymin, DVDinfo.Instance.VideoSizeY -_SubtitleBorders.Height);
        _SubtitleBorders.X = Math.Min(Xmin, MAX_WIDTH - _SubtitleBorders.Width);
        _SubtitleBorders.Y = Math.Min(Ymin, MAX_HEIGHT_PAL - _SubtitleBorders.Height);

        // Adjust the bitmap position, now that the bitmap has become smaller
        BitmapPos.X += _SubtitleBorders.X;
        BitmapPos.Y += _SubtitleBorders.Y;
        BitmapPos.Width = _SubtitleBorders.Width;
        BitmapPos.Height = _SubtitleBorders.Height;
      }
    }

    private static ArrayList SUPList;
    //private IEnumerator mySUPEnumerator;
    //private int NoOfSUPs=0;
    private Bitmap _bmpSUP;
    private Bitmap _HDbmpSUP;
    private static Color[] subColors = new Color[16];

    private static SUP instance = new SUP();

    private SUP()
    {
      // Initialize the bmpSUP to the maximum size possible
      // I will use it to write new sups, and copy the relevant portion to SUPdata.
      _bmpSUP = new Bitmap(MAX_WIDTH, MAX_HEIGHT_PAL, PixelFormat.Format4bppIndexed);
      SUPList = new ArrayList(2500);

      subColors[0] = Color.FromArgb(253, 240, 222);
      subColors[1] = Color.FromArgb(255, 230, 127);
      subColors[2] = Color.FromArgb(251, 105, 38);
      subColors[3] = Color.FromArgb(253, 0, 0);
      subColors[4] = Color.FromArgb(252, 240, 228);
      subColors[5] = Color.FromArgb(254, 249, 248);
      subColors[6] = Color.FromArgb(254, 200, 0);
      subColors[7] = Color.FromArgb(180, 0, 0);
      subColors[8] = Color.FromArgb(189, 160, 0);
      subColors[9] = Color.FromArgb(237, 199, 0);
      subColors[10] = Color.FromArgb(98, 80, 0);
      subColors[11] = Color.FromArgb(137, 99, 0);
      subColors[12] = Color.FromArgb(29, 29, 29);
      subColors[13] = Color.FromArgb(126, 126, 126);
      subColors[14] = Color.FromArgb(189, 189, 189);
      subColors[15] = Color.FromArgb(253, 253, 253);
    }

    // cb4960: This was added to fix the messed up timings after clicking the regenerate preview button
    public static void reset()
    {
      instance = new SUP();
    }

    public static SUP Instance
    {
      get
      {
        return instance;
      }
    }



    public DateTime GetStartTime(int Index)
    {
      SUPdata mySUPdata = (SUPdata)SUPList[Index];
      return mySUPdata.PackageStartTime;
    }

    public int GetStartTimeMsec(int Index)
    {
      SUPdata data = (SUPdata)SUPList[Index];
      return ((data.PackageStartTime.Hour * 3600 + data.PackageStartTime.Minute * 60 + data.PackageStartTime.Second) * 1000 + data.PackageStartTime.Millisecond);
    }

    /// <summary>
    /// Get the colors of the SUP
    /// </summary>
    /// <param name="Index"></param>
    /// <returns></returns>
    public byte[] GetColors(int Index)
    {
      SUPdata data = (SUPdata)SUPList[Index];
      return data.newColorSet;
    }

    /// <summary>
    /// Set the colors of the current subtitle to the currently selected set of colors
    /// </summary>
    /// <param name="Index"></param>
    /// <param name="colors"></param>
    public void SetColors(int Index, byte[] colors)
    {
      SUPdata data = (SUPdata)SUPList[Index];
      data.newColorSet = colors;
      SUPList[Index] = data;
    }

    /// <summary>
    /// Get the transparency values of the SUP
    /// </summary>
    /// <param name="Index"></param>
    /// <returns></returns>
    public byte[] GetTransparency(int Index)
    {
      SUPdata data = (SUPdata)SUPList[Index];
      return data.Transparency;
    }

    /// <summary>
    /// Get the transparent color
    /// </summary>
    /// <param name="Index"></param>
    /// <returns></returns>
    public byte GetTransparentColor(int Index)
    {
      SUPdata data = (SUPdata)SUPList[Index];
      return data.ColorSet[data.GetTransparent()];
    }

    /// <summary>
    /// Set the transparency of the SUP
    /// </summary>
    /// <param name="Index"></param>
    /// <param name="transparency"></param>
    public void SetTransparency(int Index, byte[] transparency)
    {
      SUPdata data = (SUPdata)SUPList[Index];
      data.Transparency = transparency;
      SUPList[Index] = data;
    }

    public int GetEndTimeMsec(int Index)
    {
      SUPdata data = (SUPdata)SUPList[Index];
      return ((data.PackageEndTime.Hour * 3600 + data.PackageEndTime.Minute * 60 + data.PackageEndTime.Second) * 1000 + data.PackageEndTime.Millisecond);
    }

    public void SetEndTime(int Index, DateTime dt)
    {
      SUPdata data = (SUPdata)SUPList[Index];
      data.PackageEndTime = dt;
      data.PackageEndTime.AddMilliseconds(-1);
    }

    public DateTime GetEndTime(int Index)
    {
      SUPdata mySUPdata = (SUPdata)SUPList[Index];
      return mySUPdata.PackageEndTime;
    }

    /// <summary>
    /// Get position of subtitle on the screen
    /// </summary>
    /// <param name="Index"></param>
    /// <returns></returns>
    public Rectangle GetBitmapPosition(int Index)
    {
      SUPdata mySUPdata = (SUPdata)SUPList[Index];
      return mySUPdata.BitmapPos;
    }

    public void SetBitmapPosition(int Index, Rectangle rect)
    {
      SUPdata mySUPdata = (SUPdata)SUPList[Index];
      mySUPdata.BitmapPos = rect;
      SUPList[Index] = mySUPdata;
    }

    public void RemoveSUP(int Index)
    {
      SUPList.RemoveAt(Index);
    }

    public void ClearSUP()
    {
      if (SUPList.Count != 0)
      {
        // Delete all bitmaps
        for (int j = 0; j < SUPList.Count; j++)
          ((SUPdata)SUPList[j]).SubtitlePicture.Dispose();
        SUPList.Clear();
        //NoOfSUPs = 0;
      }
    }

    static public string ReturnStrVobSubID()
    {
      SUPdata mySUPdata = (SUPdata)SUPList[0];
      return mySUPdata.SUPlanguage;
    }


    /// <summary>
    /// This function will convert a color from the VobSub color space to the MPEG one
    /// Normally, DVD use the MPEG space. However vobsub uses another color space so for
    /// compatibility reasons, we use this conversion function when reading .idx files
    /// so that the read palette is in the correct color space.
    /// The value returned by this function is the converted value.
    /// </summary>
    /// <param name="JpegColor">The color to be converted</param>
    static Color YCCVSColor2Mpeg(Color JpegColor)
    {
      byte Red = JpegColor.R;
      byte Green = JpegColor.G;
      byte Blue = JpegColor.B;


      // Inverted by hand from VobSubFile.cpp
      // At http://cvs.sourceforge.net/viewcvs.py/guliverkli/guliverkli/src/subtitles/VobSubFile.cpp
      // Y =    0.1283 R + 0.5205 G + 0.21 B
      // Cb = - 0,0843 R - 0,3422 G + 0,4266 B
      // Cr =   0,6066 R - 0,4322 G - 0,1744 B


      double Y = Math.Round((0.1283 * Red) + (0.5205 * Green) + (0.21 * Blue) + 16);
      double V = Math.Round((0.6066 * Red) - (0.4322 * Green) - (0.1744 * Blue) + 128);
      double U = Math.Round(-(0.0843 * Red) - (0.3422 * Green) + (0.4266 * Blue) + 128);

      int Y_ = ((int)Y) - 16;					// Y
      int Cr = ((int)V) - 128;					// Cr
      int Cb = ((int)U) - 128;					// Cb

      // Apply SubtitleCreator default formula
      int OutRed = (int)Math.Min(Math.Max(Math.Round(1.1644F * Y_ + 1.596F * Cr), 0), 255); // R
      int OutGreen = (int)Math.Min(Math.Max(Math.Round(1.1644F * Y_ - 0.813F * Cr - 0.391F * Cb), 0), 255); // G
      int OutBlue = (int)Math.Min(Math.Max(Math.Round(1.1644F * Y_ + 2.018F * Cb), 0), 255); // B

      return Color.FromArgb(JpegColor.A, OutRed, OutGreen, OutBlue);
    }


    struct idxdata
    {
      public int PTSidx;
      public long filepos;
    }

    string tempSUPfilename = "";

    /// <summary>
    /// Reads VobSub information.
    /// From idx file, get palette, timestamps, delays.
    /// From sub file, get bitmaps, duration and timestamps if no idx.
    /// Write the results in a .sup file.
    /// </summary>
    /// <param name="Filename">filename (.sub)</param>
    /// <param name="ticksPerMSec">number of ticks per ms</param>
    /// <param name="fs">reference to the already open file stream (.sub)</param>
    /// <param name="SUPfilename">Name of the SUP file to create from the Sub/Idx file</param>
    /// <param name="RetrievePalette">If true, palette will be retrieved from idx file</param>
    /// <param name="StreamIndex">If -1, retrieve the first stream, else retrieve the stream whose index is given here (0x20 to 0x3f)</param>
    /// <param name="Language">string holding the language information from the VobSub idx file</param>
    public bool ConvertSubIdxToSup(string Filename, float ticksPerMSec, ref FileStream fs, string SUPfilename, bool RetrievePalette, int StreamIndex)
    {
      // Try to get extra information if idx file available
      // List of information retrieved:
      // - palette (according to input parameter: RetrievePalette)
      // - timestamps
      // - delays
      string idxfilename = Path.GetDirectoryName(Filename) + Path.DirectorySeparatorChar + Path.GetFileNameWithoutExtension(Filename) + ".idx";
      bool idx_exists = false;
      List<idxdata> List_idxdata = new List<idxdata>();

      // This variable will reflect the active stream
      // Will be updated according to the id: line
      int ActiveStream = -1;

      if (File.Exists(idxfilename))
      {
        int PTS_offset = 0;

        TextReader IdxTr;
        try
        {
          IdxTr = new StreamReader(idxfilename);
          string Line;
          string[] palette = new string[20];

          Line = IdxTr.ReadLine();
          idx_exists = true;
          while (Line != null)
          {
            Line = IdxTr.ReadLine();

            if ((!Line.StartsWith("#")) && (Line.Contains("id:") && (StreamIndex != -1)))
            {
              Line = Line.Substring(Line.IndexOf("index:") + 6);
              //ActiveStream=Int32.Parse(Line)+0x20;
              ActiveStream = Int32.Parse(Line);
              // Set StreamIndex to first stream found if -1
              if (StreamIndex == -1) StreamIndex = ActiveStream;
            }

            if ((!Line.StartsWith("#")) && (Line.Contains("size:")))
            {
              if (Line.Contains("720x480"))
              {
                SetNTSC();
              }
              else
              {
                SetPAL();
              }
            }

            if ((!Line.StartsWith("#")) && (Line.Contains("palette:")) && (RetrievePalette))
            {
              Line = Line.Remove(0, 8);
              palette = Line.Split(',');
              for (int n = 0; n < 16; n++)
              {
                // Update our palette according to idx palette
                uint fullcolor = UInt32.Parse(palette[n], System.Globalization.NumberStyles.HexNumber);

                // Copy colors to settings
                //mySettings.Colors[n]= YCCVSColor2Mpeg(Color.FromArgb((int)((fullcolor & 0xff0000)>>16),(int)((fullcolor & 0x00ff00)>>8),(int)(fullcolor & 0x0000ff)));
                subColors[n] = YCCVSColor2Mpeg(Color.FromArgb((int)((fullcolor & 0xff0000) >> 16), (int)((fullcolor & 0x00ff00) >> 8), (int)(fullcolor & 0x0000ff)));
              }
            }

            // Don't want to support custom colors TAG because it is not easily transposable to SUP files
            // It is done for Media Players

            if ((ActiveStream == StreamIndex) && (!Line.StartsWith("#")) && (Line.Contains("timestamp: ")) && (idx_exists))
            {
              Line = Line.Remove(0, 11);
              string[] PTS_idx_splitted = Line.Split(',');

              // First retrieve PTS from idx file
              string[] PTS_idx_timings = PTS_idx_splitted[0].Split(':');
              if (PTS_idx_timings[0].Contains("-"))
                idx_exists = false; // Negative timing is invalid: Don't use .idx timings
              else
              {
                idxdata temp_idxdata;
                int PTS_idx = (int)(ticksPerMSec * (Convert.ToInt32(PTS_idx_timings[3]) + 1000 * (Convert.ToInt32(PTS_idx_timings[2]) + 60 * (Convert.ToInt32(PTS_idx_timings[1]) + 60 * Convert.ToInt32(PTS_idx_timings[0])))));
                //Debug.WriteLine("PTS_idx_splitted[0]="+PTS_idx_splitted[0]+" ticksPerMSec="+ticksPerMSec);
                temp_idxdata.PTSidx = PTS_idx + PTS_offset;

                // Then retrieve filepos from idx file
                string[] PTS_idx_filepos = PTS_idx_splitted[1].Split(':');
                temp_idxdata.filepos = UInt32.Parse(PTS_idx_filepos[1], System.Globalization.NumberStyles.HexNumber);
                List_idxdata.Add(temp_idxdata);
              }
            }
            /*
            #	 delay: [sign]hh:mm:ss:ms
            #
            # Where:
            #	 [sign]: +, - (optional)
            #	 hh: hours (0 <= hh)
            #	 mm/ss: minutes/seconds (0 <= mm/ss <= 59)
            #	 ms: milliseconds (0 <= ms <= 999)
            #
            #	 Note: You can't position a sub before the previous with a negative value.
             */
            if ((!Line.StartsWith("#")) && (Line.Contains("delay")))
            {
              string[] Line_splitted = Line.Split(':');
              string[] Hours_splitted;
              int delay_idx;

              if (Line_splitted[1].Contains("+"))
              {
                Hours_splitted = Line_splitted[1].Split('+');
                delay_idx = (int)(ticksPerMSec * (Convert.ToInt32(Line_splitted[4]) + 1000 * (Convert.ToInt32(Line_splitted[3]) + 60 * (Convert.ToInt32(Line_splitted[2]) + 60 * Convert.ToInt32(Hours_splitted[1])))));
              }
              else if (Line_splitted[1].Contains("-"))
              {
                Hours_splitted = Line_splitted[1].Split('-');
                delay_idx = (int)(-ticksPerMSec * (Convert.ToInt32(Line_splitted[4]) + 1000 * (Convert.ToInt32(Line_splitted[3]) + 60 * (Convert.ToInt32(Line_splitted[2]) + 60 * Convert.ToInt32(Hours_splitted[1])))));
              }
              else
                delay_idx = (int)(ticksPerMSec * (Convert.ToInt32(Line_splitted[4]) + 1000 * (Convert.ToInt32(Line_splitted[3]) + 60 * (Convert.ToInt32(Line_splitted[2]) + 60 * Convert.ToInt32(Line_splitted[1])))));

              // Update accumulated delay
              PTS_offset += delay_idx;
            }
          }
          IdxTr.Close();
        }
        catch (System.Exception)
        {
          // We get here only if there is no idx file (we don't care!)
        }
      }

      // We will now generate a temporary SUP file to open normally
      // If idx exists, we will loop through the filepos'es to read all subpics
      // This should also work with interlaced subs!

      int i;
      byte[] b = new byte[4];

      // This is a ".SUB" file
      // First convert the .SUB file to .SUP
      // Use the filename passed as a parameter for that
      FileStream fsSUP = new FileStream(SUPfilename, FileMode.Create, FileAccess.Write);

      //MemWriter mw = new MemWriter(20000); // Max size of 1 sup bitmap 20 Kbytes
      // 20k wasn't big enough for the "Mimi o Sumaseba" jap stream, so increased to 100k
      MemWriter mw = new MemWriter(100000); // Max size of 1 sup bitmap 100 Kbytes
      BinaryWriter w = new BinaryWriter(fsSUP);

      long offset_in_sub_file;

      int CurrentVobSubId;
      int PES_header_length;

      if (idx_exists)
      {
        foreach (idxdata my_idxdata in List_idxdata)
        {
          // Yield to other windows processes
          //System.Windows.Forms.Application.DoEvents();

          offset_in_sub_file = my_idxdata.filepos;

          // Go to next subpic
          fs.Position = offset_in_sub_file;

          if (((i = fs.ReadByte()) == 0) && ((i = fs.ReadByte()) == 0) && ((i = fs.ReadByte()) == 1) && ((i = fs.ReadByte()) == 0xba))
          {
            // Memorize address of current subpic
            long currentsubposition = fs.Position - 4;

            // Found new header 0x00 0x00 0x01 0xba
            SUPdata data = new SUPdata();

            mw.WriteByte((byte)'S');
            mw.WriteByte((byte)'P');

            // Skip 14 more bytes
            fs.Seek(14, SeekOrigin.Current);

            fs.Read(b, 0, 2);  // Read 2 bytes (b18 and b19)
            long data_block_size = (b[0] << 8) + b[1];  // Data block size (less than 2028 - 2028 if full block)

            fs.Read(b, 0, 3);
            PES_header_length = b[2];

            // Skip needed bytes
            if (PES_header_length != 0) fs.Seek(PES_header_length, SeekOrigin.Current);

            // Read VobSub ID
            fs.Read(b, 0, 1);
            CurrentVobSubId = b[0];

            int PTS = my_idxdata.PTSidx;
            //Debug.WriteLine("PTS="+PTS);

            // Write start time
            mw.WriteByte((byte)(PTS & 0x000000ff));
            mw.WriteByte((byte)((PTS & 0x0000ff00) >> 8));
            mw.WriteByte((byte)((PTS & 0x00ff0000) >> 16));
            mw.WriteByte((byte)((PTS & 0xff000000) >> 24));

            // Write four empty bytes
            mw.WriteInt32(0);

            // Determine the number of chunks for the current sup
            // Read byte data_block_size of each next sequence
            // until it is less than 2028 or out of file.
            // Then check that corresponding b[21] is 0.
            int chunk_number = 1;
            int to_skip = 0;

            // Search only if first chunk is full
            // We want to know how many chunks we have
            // This block is compatible with interlaced and non interlaced
            while (data_block_size == (0x800 - 20))
            {
              if ((offset_in_sub_file + (18 + 0x800 * (chunk_number + to_skip))) < (fs.Length))
              {
                fs.Position = offset_in_sub_file + (18 + 0x800 * (chunk_number + to_skip));

                fs.Read(b, 0, 2);
                data_block_size = (b[0] << 8) + b[1];  // Data block size (less than 2028 - 2028 if full block)
                // test that b[21] is O, else this is the next SUP

                fs.Read(b, 0, 3);
                PES_header_length = b[2];

                // Skip needed bytes
                if (PES_header_length != 0) fs.Seek(PES_header_length, SeekOrigin.Current);

                // Read VobSub ID
                fs.Read(b, 0, 1);
                if (CurrentVobSubId == b[0])
                {
                  fs.Position = offset_in_sub_file + (18 + 0x800 * chunk_number);
                  fs.Read(b, 0, 2);

                  fs.Read(b, 0, 2);
                  chunk_number++;
                }
                else
                {
                  to_skip++;
                  data_block_size = 0x800 - 20; // To continue looping
                }
              }
              else break;
            }

            // Now we have the SUP info interleaved with the headers
            for (i = 1; i <= (chunk_number + to_skip); i++)
            {
              // Put pointer to correct position
              fs.Position = offset_in_sub_file + (18 + 0x800 * (i - 1));
              fs.Read(b, 0, 2);  // Read b18 and b19
              data_block_size = (b[0] << 8) + b[1];
              fs.Read(b, 0, 3);
              PES_header_length = b[2];

              // Skip needed bytes
              if (PES_header_length != 0) fs.Seek(PES_header_length, SeekOrigin.Current);

              // Read VobSub ID
              fs.Read(b, 0, 1);

              if (CurrentVobSubId == b[0])
              {
                if (i == 1)
                {
                  fs.Position = 24 + PES_header_length + offset_in_sub_file;
                  data_block_size = data_block_size - (4 + PES_header_length);
                }
                else
                {
                  fs.Position = (24 + 0x800 * (i - 1)) + offset_in_sub_file + PES_header_length;
                  data_block_size = data_block_size - 4;
                }

                for (int j = 0; j < data_block_size; j++)
                {
                  mw.WriteByte((byte)fs.ReadByte());
                }
              }
            }


            offset_in_sub_file += chunk_number * 0x800;
          }

          // Write from the beginning until current position
          long EndPosition = mw.GetPosition();
          mw.GotoBegin();

          // Write whole memory stream to file
          w.Write(mw.GetBuf(), 0, (int)EndPosition);

        }
        fsSUP.Close();
      }
      else
      // This part is for when there is no associated .idx file
      {
        bool writetoSUP = false;
        offset_in_sub_file = 0;

        while (offset_in_sub_file < fs.Length)
        {
          //					System.Windows.Forms.Application.DoEvents();
          fs.Position = offset_in_sub_file;

          if (((i = fs.ReadByte()) == 0) && ((i = fs.ReadByte()) == 0) && ((i = fs.ReadByte()) == 1) && ((i = fs.ReadByte()) == 0xba))
          {
            // Memorize address of current subpic
            long currentsubposition = fs.Position - 4;

            // Found new header 0x00 0x00 0x01 0xba
            SUPdata data = new SUPdata();

            mw.WriteByte((byte)'S');
            mw.WriteByte((byte)'P');

            // Skip 14 more bytes
            fs.Seek(14, SeekOrigin.Current);

            fs.Read(b, 0, 2);  // Read 2 bytes (b18 and b19)
            long data_block_size = (b[0] << 8) + b[1];  // Data block size (less than 2028 - 2028 if full block)

            fs.Read(b, 0, 3);  // Skip 3 bytes
            PES_header_length = b[2];

            // Read start time: PTS = Presentation Time Stamp
            fs.Read(b, 0, 2);  // Read 2 bytes (b23 and b24)
            int TS3 = ((b[0] & 0x0e) << 4) | ((b[1] & 0xf8) >> 3);
            int TS2 = ((b[1] & 0x07) << 5);
            fs.Read(b, 0, 2);
            TS2 |= ((b[0] & 0xf8) >> 3);
            int TS1 = ((b[0] & 0x06) << 5) | ((b[1] & 0xfc) >> 2);
            int TS0 = ((b[1] & 0x03) << 6);
            fs.Read(b, 0, 1);
            TS0 |= ((b[0] & 0xfc) >> 2);
            int PTS = TS0 + (TS1 << 8) + (TS2 << 16) + (TS3 << 24);

            // FIX : PTS is 33 bits !!!
            //
            PTS = (PTS << 1) + (b[0] & 0x01);

            if (PES_header_length > 5) fs.Seek((PES_header_length - 5), SeekOrigin.Current);
            fs.Read(b, 0, 1);
            CurrentVobSubId = b[0];

            // Now we will generate this SUB if and only if the streamid is correct
            if (StreamIndex == -1) StreamIndex = CurrentVobSubId;
            if (StreamIndex == CurrentVobSubId)
            {
              writetoSUP = true;

            }
            else
            {
              writetoSUP = false;
            }

            // Write start time
            mw.WriteByte((byte)(PTS & 0x000000ff));
            mw.WriteByte((byte)((PTS & 0x0000ff00) >> 8));
            mw.WriteByte((byte)((PTS & 0x00ff0000) >> 16));
            mw.WriteByte((byte)((PTS & 0xff000000) >> 24));

            // Write four empty bytes
            mw.WriteInt32(0);

            // Determine the number of chunks for the current sup
            // Read byte data_block_size of each next sequence
            // until it is less than 2028 or out of file.
            // Then check that corresponding b[21] is 0.
            int chunk_number = 1;
            int to_skip = 0;

            // Search only if first chunk is full
            while (data_block_size == (0x800 - 20))
            {
              if ((offset_in_sub_file + (18 + 0x800 * (chunk_number + to_skip))) < (fs.Length))
              {
                fs.Position = offset_in_sub_file + (18 + 0x800 * (chunk_number + to_skip));

                fs.Read(b, 0, 2);
                data_block_size = (b[0] << 8) + b[1];  // Data block size (less than 2028 - 2028 if full block)
                // test that b[21] is O, else this is the next SUP

                fs.Read(b, 0, 3);
                PES_header_length = b[2];

                // Skip needed bytes
                if (PES_header_length != 0) fs.Seek(PES_header_length, SeekOrigin.Current);

                // Read VobSub ID
                fs.Read(b, 0, 1);
                if (CurrentVobSubId == b[0])
                {
                  fs.Position = offset_in_sub_file + (18 + 0x800 * chunk_number);
                  fs.Read(b, 0, 2);

                  fs.Read(b, 0, 2);
                  chunk_number++;
                }
                else
                {
                  to_skip++;
                  data_block_size = 0x800 - 20; // To continue looping
                }
              }
              else break;
            }

            // Now we have the SUP info interleaved with the headers
            for (i = 1; i <= (chunk_number + to_skip); i++)
            {
              // Put pointer to correct position
              fs.Position = offset_in_sub_file + (18 + 0x800 * (i - 1));
              fs.Read(b, 0, 2);  // Read b18 and b19
              data_block_size = (b[0] << 8) + b[1];
              fs.Read(b, 0, 3);
              PES_header_length = b[2];

              // Skip needed bytes
              if (PES_header_length != 0) fs.Seek(PES_header_length, SeekOrigin.Current);

              // Read VobSub ID
              fs.Read(b, 0, 1);

              if (CurrentVobSubId == b[0])
              {
                if (i == 1)
                {
                  fs.Position = 24 + PES_header_length + offset_in_sub_file;
                  data_block_size = data_block_size - (4 + PES_header_length);
                }
                else
                {
                  fs.Position = (24 + 0x800 * (i - 1)) + offset_in_sub_file + PES_header_length;
                  data_block_size = data_block_size - 4;
                }

                for (int j = 0; j < data_block_size; j++)
                {
                  mw.WriteByte((byte)fs.ReadByte());
                }
              }
            }

            offset_in_sub_file += chunk_number * 0x800;
          }

          if (offset_in_sub_file == 0)
          {
            fsSUP.Close();
            fs.Close();
            return false; // This file is not a binary .sub file --> return false
          }


          // Write from the beginning until current position
          long EndPosition = mw.GetPosition();
          mw.GotoBegin();

          if (writetoSUP)
          {
            // Write whole memory stream to file
            w.Write(mw.GetBuf(), 0, (int)EndPosition);
          }
        }
      }

      fsSUP.Close();
      fs.Close();

      return true;
    }

   
    /* Legend from SubRip sources Unit08.pas
        $00: begin DCSQTArr.Forced := True; Result.Command := SupCmdOn; end; //FSTA_DSP - Forced start display
        $01: Result.Command := SupCmdOn; //STA_DSP - start display
        $02: Result.Command := SupCmdOff; //STP_DSP - stop display
        $03: Result.Color := Read2Byte; //SET_COLOR - set colors: 0x03wxyz: w-empahsis 2 color, x-emphasis 1 color, y-pattern color, z-background color
        $04: Result.Contrast := Read2Byte; //SET_CONTR - set contrast (transparency): 0x04wxyz w-emphasis 2 transparency and so on
        $05: Result.PicCoord := ReadCoor; //SET_DAREA - set display area: 0x05xxxXXXyyyYYY, xxx - start X coordinate, XXX - end X coordinate, yyy - start Y coordinate, YYY - end Y coordinate
        $06: Result.LineOff := ReadLinesOf; //SET_DSPXA - set display pixel area: 0x06xxxxyyyy, xxxx - start addr. of PXD Top Field, yyyy - start addr. of PXD Bottom Field
        $07: ReadChn_ColCon; //CHN_COLCON - change color/contrast, 0x07wwww ....... 0fffffff: where wwww is the length including wwww and termination code 0x0fffffff
        $FF: Break; //CMD_END - command sequence terminator, End of current Command Header
		 */


    public struct StreamsData
    {
      public byte VobSubId;
      public string Language;
    }

    List<StreamsData> MyStreamsData = new List<StreamsData>();

    /// <summary>
    /// Retrieves the number of languages from a VobSub (sub) file
    /// Will decode the bitmap and read the VobIds
    /// <param name="SubFileName">Name of the SUB file from which to retrieve the number of languages</param>
    /// <returns>An integer.</returns>
    /// </summary>
    int RetrieveLanguageTableFromSub(string SubFileName)
    {
      StreamsData TempStreamsData;
      int LanguageStreamNumber = 0;
      FileStream fs;
      fs = new FileStream(SubFileName, FileMode.Open, FileAccess.Read);

      int i, CurrentVobSubId;
      bool[] id_found = new bool[256];
      for (int n = 0; n < 256; n++) id_found[n] = false;
      byte[] b = new byte[4];

      // This is a ".SUB" file
      long offset_in_sub_file = 0;

      // Start scanning
      while (offset_in_sub_file < fs.Length)
      {
        //				System.Windows.Forms.Application.DoEvents();

        fs.Position = offset_in_sub_file;

        if (((i = fs.ReadByte()) == 0) && ((i = fs.ReadByte()) == 0) && ((i = fs.ReadByte()) == 1) && ((i = fs.ReadByte()) == 0xba))
        {

          // Memorize address of current subpic
          long currentsubposition = fs.Position - 4;

          // Found new header 0x00 0x00 0x01 0xba
          // Skip 14 bytes
          fs.Seek(14, SeekOrigin.Current);
          fs.Read(b, 0, 2);  // Read 2 bytes (b18 and b19)
          long data_block_size = (b[0] << 8) + b[1];  // Data block size (less than 2028 - 2028 if full block)
          fs.Seek(8, SeekOrigin.Current);
          fs.Read(b, 0, 1);

          CurrentVobSubId = b[0];

          // Check if Id already seen
          if ((CurrentVobSubId >= 0x20) && (CurrentVobSubId < 0x40))
          {
            if (!id_found[CurrentVobSubId])
            {
              id_found[CurrentVobSubId] = true;
              TempStreamsData.VobSubId = (byte)CurrentVobSubId;
              TempStreamsData.Language = "--";
              LanguageStreamNumber++;
              MyStreamsData.Add(TempStreamsData);
            }
          }

          // Determine the number of chunks for the current sup
          // Read byte data_block_size of each next sequence
          // until it is less than 2028 or out of file.
          // Then check that corresponding b[21] is 0.
          int chunk_number = 1;

          // Search only if first chunk is full
          while (data_block_size == (0x800 - 20))
          {
            if ((offset_in_sub_file + (18 + 0x800 * chunk_number)) < (fs.Length))
            {
              fs.Position = offset_in_sub_file + (18 + 0x800 * chunk_number);
              fs.Read(b, 0, 2);
              data_block_size = (b[0] << 8) + b[1];  // Data block size (less than 2028 - 2028 if full block)
              // test that b[21] is O, else this is the next SUP
              fs.Read(b, 0, 2);
              if (b[1] != 0) data_block_size = 0;
              else chunk_number++;
            }
            else break;
          }
          // Now we have the SUP info interleaved with the headers
          for (i = 1; i <= chunk_number; i++)
          {
            // Retrieve current data chunk size
            fs.Position = offset_in_sub_file + (18 + 0x800 * (i - 1));
            fs.Read(b, 0, 2);  // Read b18 and b19
            data_block_size = (b[0] << 8) + b[1];
            fs.Read(b, 0, 3);
            int PES_header_length = b[2];

            if (i == 1)
            {
              fs.Position = 24 + PES_header_length + offset_in_sub_file;
              data_block_size = data_block_size - (4 + PES_header_length);
            }
            else
            {
              fs.Position = (24 + 0x800 * (i - 1)) + offset_in_sub_file + PES_header_length;
              data_block_size = data_block_size - 4;
            }
            // Go ahead
            fs.Seek(data_block_size, SeekOrigin.Current);
          }

          offset_in_sub_file += chunk_number * 0x800;
        }

        if (offset_in_sub_file == 0)
        {
          fs.Close();
          return -1; // This file is not a binary .sub file --> return false
        }
      }
      fs.Close();
      return LanguageStreamNumber;
    }

    public ArrayList LangStreamsData
    {
      get
      {
        ArrayList langs = new ArrayList();

        foreach (StreamsData data in MyStreamsData)
        {
          langs.Add(data);
        }

        return langs;
      }
    }

    /// <summary>
    /// Retrieves a language table from an idx file
    /// If no Idx available will try to get it from the sub file
    /// <param name="IdxFileName">Name of the IDX file from which to retrieve the language table</param>
    /// <returns>A string[] holding the languages in the order found in the idx file. -1 if the idx file doesn't exist.</returns>
    /// </summary>
    public string[] RetrieveLanguageTableFromIdxOrSub(string IdxFileName)
    {
      StreamsData TempStreamsData;

      // First clear list if it already exists
      MyStreamsData.Clear();

      string TempString = "";
      string[] LanguageTable = null;
      string Line, Index;
      TextReader IdxTr;
      try
      {
        if (File.Exists(IdxFileName))
        {
          IdxTr = new StreamReader(IdxFileName);

          Line = IdxTr.ReadLine();
          while (IdxTr.Peek() >= 0)
          {
            Line = IdxTr.ReadLine();
            if ((!Line.StartsWith("#")) && (Line.Contains("id:")))
            {
              if (TempString != "") TempString += "§";
              TempString += Line;
            }

          }
          IdxTr.Close();

          LanguageTable = TempString.Split('§');
          int Nb_lang = LanguageTable.GetLength(0);
          for (int n = 0; n < Nb_lang; n++)
          {
            //Clean the string to keep only the language string and
            //sort the language table supposing it can be in a non growing order
            Line = LanguageTable[n];
            Index = Line;
            Index = Line.Substring(Line.IndexOf("index:") + 6);
            Line = Line.Substring(0, Line.IndexOf(","));
            Line = Line.Substring(Line.IndexOf("id:") + 3);
            Line = Line.TrimStart();
            Line = Line.TrimEnd();
            //TempStreamsData.VobSubId=(byte) (Int32.Parse(Index)+0x20);
            TempStreamsData.VobSubId = (byte)(Int32.Parse(Index));
            TempStreamsData.Language = Line;
            MyStreamsData.Add(TempStreamsData);
            string FullLanguageName = subs2srs.UtilsLang.LangTwoLetter2Full(Line);

            if (FullLanguageName != "")
            {
              //Line = Line + " (" + FullLanguageName + ")";
              Line = FullLanguageName;
            }
            else
            {
              //Line = Line + " (Not detected)";
              Line = Line + "???";
            }

            LanguageTable[n] = Line;
          }
        }
        else
        {
          int x = RetrieveLanguageTableFromSub(Path.ChangeExtension(IdxFileName, "sub"));
          if (x == -1) return null;
          for (int t = 0; t < x; t++)
          {
            if (TempString == "") TempString += "-- (Not detected)";
            else TempString += "§-- (Not detected)";
          }
          LanguageTable = TempString.Split('§');
        }
      }
      catch (Exception e)
      {
        MessageBox.Show(e.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
      return LanguageTable;
    }


    public string Filename;
    public System.Windows.Forms.ProgressBar myProgressBar;
    //public ReturnResultCallback myCallback;
    //public bool Abort = false;

    /* mpucoder
     * Using 90090 for NTSC is to convert to non-drop timecode, which is the recommended
     * timecode for authoring. Dividing by 90000 will give you the drop-frame time. I know you
     * guys like to use milliseconds instead of frames, but pts values are always frame
     * aligned. It is simpler to divide the adjusted pts (the one found in a sup) by the frame
     * duration (3600 for PAL, 3003 for NTSC) to ge the frame number. From there you get
     * hh:mm:ss:ff, or, if you wish, hh:mm:ss.mmm
     * 
     * The duration times of spu's also should be first rounded up and then adjusted to a
     * frame point or you may accumulate arithmetic errors and drop a frame in the next
     * authoring. Use the last DCSQ_STM (the delay value) in this formula:
     * duration_in_frames = ((DCSQ_STM * 1024) + 1023) / frame_duration
     * From there you can compute the end time by adding duration_in_frames to the start (in frames)
     * 
     * Why round up? Because this is the formula used to derive STM values:
     * DCSQ_STM = (frames * frame_duration) >> 10
     * So this value is almost always less than the intended delay when multiplied again
     * by 1024 (or shifted left 10)
     * 
     * EV: I need the duration in msec, so I need to multiply by 1000 and divide the
     * duration_in_frames by 25 (PAL) or 29.97 (NTSC).
     */
    public void ReadSUP(int stream, Color[] customColors, bool[] customTransparency)
    {
      bool PackageEndTimeNotSpecified = true;		// Some SUPs don't specify the end display time (so use the new start time to set the previous end time)
      FileStream fs;

      string TempFilename;

      if (Path.GetExtension(Filename).ToLower() == ".idx")
        TempFilename = Path.ChangeExtension(Filename, ".sub");
      else
        TempFilename = Filename;

      try
      {
        fs = new FileStream(TempFilename, FileMode.Open, FileAccess.Read);
      }
      catch (System.Exception e1)
      {
        System.Windows.Forms.MessageBox.Show(e1.Message, "Error",
                                             System.Windows.Forms.MessageBoxButtons.OK,
                                             System.Windows.Forms.MessageBoxIcon.Error);
        return;
      }

      float ticksPerMSec = 90.000F;
      if (!IsPAL) ticksPerMSec = 90.090F;

      // The LanguageTable will be filled only for VobSub files
      string[] LanguageTable = null;
      string mylanguage = null;

      if (Path.GetExtension(TempFilename).ToLower() == ".sub")
      {
        // First read the streams from the idx file and build a language
        // table with them. They can be either --, --, --, ... or en, fr, fr, de, ...
        LanguageTable = RetrieveLanguageTableFromIdxOrSub(Path.ChangeExtension(Filename, ".idx"));

        if (LanguageTable == null)
        {
          MessageBox.Show("This is not a vobsub file", "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
          return;
        }

        //// Now if we have retrieved a table, ask the user to choose the stream he wants to open
        //int table_length=LanguageTable.GetLength(0);
        //if (table_length>1)
        //  SUPPreviewForm.ShowChoose(LanguageTable);

        //if (table_length==1) SUPPreviewForm.StreamIndex=0;

        //if (SUPPreviewForm.StreamIndex!=-1)
        //{
        //  mylanguage=MyStreamsData[SUPPreviewForm.StreamIndex].Language;
        //  SUPPreviewForm.StreamIndex=MyStreamsData[SUPPreviewForm.StreamIndex].VobSubId;
        //}
        //else
        mylanguage = "--";


        //int streamIndex = SUPPreviewForm.StreamIndex;
        int streamIndex = stream;

        // If vobsub file, use a temporary file in the user's temporary directory for creating a SUP
        tempSUPfilename = Path.GetTempPath() + "~" + Path.GetFileName(TempFilename) + ".sup";
        if (!ConvertSubIdxToSup(TempFilename, ticksPerMSec, ref fs, tempSUPfilename, true, streamIndex))
        {
          MessageBox.Show("This is not a vobsub file", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
          return;
        }

        // This is for the next part (i.e. SUP reading part, just below...)
        fs = new FileStream(tempSUPfilename, FileMode.Open, FileAccess.Read);
      }

      // Test if HD-SUP
      // Read 12 first bytes of the SUP file
      // Check that two first bytes are 'SP'
      // Check if b11 and b12 are '0' -> This is a HD SUP
      long TempPos = fs.Position;
      byte[] TempArray = new byte[12];
      fs.Read(TempArray, 0, 12);
      if ((TempArray[0] == 'S') && (TempArray[1] == 'P') && (TempArray[10] == 0) && (TempArray[11] == 0))
      {
        // This is a HD-SUP
        fs.Position = TempPos;
        ReadHDSUP(ref fs);
      }
      else
      {
        fs.Position = TempPos;
        if (SUPList.Count == 0)
          // Only create when needed
          SUPList = new ArrayList(2500);
        else
          ClearSUP();

        int i;
        byte[] b = new byte[4];

        // Keep track of time
        DateTime CurrentTime = new DateTime(0);

        // Start scanning
        while ((i = fs.ReadByte()) != -1)
        {
          //System.Windows.Forms.Application.DoEvents();

          //// Check whether we need to abort
          //if (worker.CancellationPending) {
          //  //Abort = false;
          //  fs.Position = fs.Length;
          //}

          if (i == (int)'S' && (i = fs.ReadByte()) == (int)'P')
          {
            // Show progress
            //worker.ReportProgress((int)((fs.Position * 100) / fs.Length));

            // Found new header SP
            SUPdata data = new SUPdata();

            // If VobSub file, fill the SUPlanguage field
            if (LanguageTable != null)
            {
              if (mylanguage == "--") data.SUPlanguage = LanguageTable[0];
              else data.SUPlanguage = mylanguage;
            }
            else data.SUPlanguage = "";

            // Read start time: PTS = Presentation Time Stamp
            fs.Read(b, 0, 4);
            int PTS = Swap4Bytes(b);

            //Debug.WriteLine("PTS="+PTS);

            //data.PackageStartTime = data.PackageStartTime.AddMilliseconds(Math.Round(PTS/ticksPerMSec));
            data.PackageStartTime = data.PackageStartTime.AddMilliseconds(Math.Round(PTS / 90.0F));

            // Read four empty bytes
            fs.Read(b, 0, 4);

            // Save SUP reference position
            long refPos = fs.Position;
            // Pointer to next package
            fs.Read(b, 0, 2);
            data.nextSUP = refPos + (long)((int)b[0] << 8 | (int)b[1]);
            // Pointer to start control sequence
            fs.Read(b, 0, 2);
            data.startControlSeq = refPos + (long)((int)b[0] << 8 | (int)b[1]);

            // Jump to control sequence
            fs.Position = data.startControlSeq;

            long NextControl = 0;
            // Read control code
            while (fs.Position < data.nextSUP)
            {
              // Read time
              fs.Read(b, 0, 2);
              // DCSQ_STM
              int TimeOfControl = ((int)b[0] << 8 | (int)b[1]);
              // Read offset to next control sequence
              fs.Read(b, 0, 2);
              long tmp = refPos + ((int)b[0] << 8 | (int)b[1]);
              // Are we dealing with the last control seq (next control equals previous next control)
              if (NextControl != tmp && tmp > fs.Position)
                NextControl = tmp;
              else
                NextControl = data.nextSUP;

              while (fs.Position < NextControl && fs.Position < data.nextSUP)
              {
                i = fs.ReadByte();
                switch (i)
                {
                  case 1:
                    // Start displaying
                    //data.PackageStartTime = data.PackageStartTime.AddMilliseconds(TimeOfControl/90D);
                    if (TimeOfControl > 0)
                      data.PackageStartTime = data.PackageStartTime.AddMilliseconds(((TimeOfControl << 10) + 1023) / ticksPerMSec);
                    if (PackageEndTimeNotSpecified && GetNoOfSubtitles() > 0)
                      this.SetEndTime(GetNoOfSubtitles() - 1, data.PackageStartTime.AddMilliseconds(-1));
                    PackageEndTimeNotSpecified = true;
                    break;
                  case 2:
                    PackageEndTimeNotSpecified = false;
                    // Stop displaying
                    data.PackageEndTime = data.PackageEndTime.AddTicks(data.PackageStartTime.Ticks - data.PackageEndTime.Ticks);
                    data.PackageEndTime = data.PackageEndTime.AddMilliseconds(((TimeOfControl << 10) + 1023) / ticksPerMSec);
                    break;
                  case 3:
                    // Color mapping
                    fs.Read(b, 0, 2);
                    data.ColorSet[3] = (byte)((int)b[0] >> 4);
                    data.ColorSet[2] = (byte)((int)b[0] & 0xF);
                    data.ColorSet[1] = (byte)((int)b[1] >> 4);
                    data.ColorSet[0] = (byte)((int)b[1] & 0xF);
                    data.newColorSet = data.ColorSet;
                    break;
                  case 4:
                    // Transparency
                    fs.Read(b, 0, 2);
                    if (b[0] == 0 && b[1] == 0) break;		// Added for some sups that seem to have some codes resetting all colors
                    data.Transparency[3] = (byte)((b[0] & 0xF0) >> 4);	//(((int) b[0] & 0xF0) == 0);
                    data.Transparency[2] = (byte)(b[0] & 0x0F); //(((int) b[0] & 0x0F) == 0);
                    data.Transparency[1] = (byte)((b[1] & 0xF0) >> 4);	//(((int) b[1] & 0xF0) == 0);
                    data.Transparency[0] = (byte)(b[1] & 0x0F); //(((int) b[1] & 0x0F) == 0);
                    break;
                  case 5:
                    // Subtitle position (and size) xx.xX.XX.yy.yY.YY
                    // Subtitle size is width W=(XXX-xxx+1) and height H=(YYY-yyy+1)
                    fs.Read(b, 0, 3);
                    data.BitmapPos.X = (int)b[0] << 4 | ((int)b[1] & 0xF0) >> 4;
                    data.BitmapPos.Width = (((int)b[1] & 0xF) << 8 | (int)b[2]) -
                      data.BitmapPos.X + 1;
                    fs.Read(b, 0, 3);
                    data.BitmapPos.Y = (int)b[0] << 4 | ((int)b[1] & 0xF0) >> 4;
                    data.BitmapPos.Height = (((int)b[1] & 0xF) << 8 | (int)b[2]) -
                      data.BitmapPos.Y + 1;
                    // Check the values found here
                    if (data.BitmapPos.Width < 0 || data.BitmapPos.Width > MAX_WIDTH ||
                        data.BitmapPos.Height < 0 || data.BitmapPos.Height > MAX_HEIGHT_PAL)
                    {
                      // Something went wrong: reset them and jump to next SP
                      data.BitmapPos.Width = 0;
                      data.BitmapPos.Height = 0;
                      fs.Position = data.nextSUP;
                    }
                    break;
                  case 6:
                    // Start of first and second RLE bitmap line (interleaved)
                    // 11.11.22.22
                    fs.Read(b, 0, 4);
                    data.startBitmapInterleaved1 = refPos +
                      ((uint)b[0] << 8 | (uint)b[1]);
                    data.startBitmapInterleaved2 = refPos +
                      ((uint)b[2] << 8 | (uint)b[3]);
                    break;
                  case 7:
                    // Change color and contrast - don't know what to do with it, so skip
                    fs.Read(b, 0, 2);
                    int length = ((int)b[0] << 8 | (int)b[1]) - 2;
                    data.ChgColCon = new byte[length];
                    fs.Read(data.ChgColCon, 0, length);
                    //fs.Position += length;
                    break;
                  case 0xFF:
                    // End/escape sequence - don't do anything
                    break;
                  default:
                    // If unknown, skip
                    break;
                }
              }
            }

            data.customColors = customColors;
            data.customTransparency = customTransparency;

            Convert2Bitmap(fs, data);
            // Save in PNG (Portable Networks Graphics) format -> more compressed than bitmap
            //string myFile = Path.GetDirectoryName(TempFilename) + "\\SUPBitmap" + NoOfSUPs + ".png";
            //Settings mySettings = Settings.Instance;
            //if (Math.IEEERemainder(NoOfSUPs+1, mySettings.ShowEverySUP) == 0 || NoOfSUPs==0)
            //data.SubtitlePicture.Save(myFile, ImageFormat.Png);

            // Go to the next SUP
            fs.Position = data.nextSUP;

            if (data.PackageStartTime >= CurrentTime)
            {
              CurrentTime = data.PackageStartTime;
              if (data.PackageEndTime < data.PackageStartTime)
                data.PackageEndTime = data.PackageStartTime.AddMilliseconds(100);
              SUPList.Add(data);
            }
            else
              data.Dispose();
            //if (NoOfSUPs > 589)
            //	NoOfSUPs+=0;
          }
        }
      }
      fs.Close();
      if (Path.GetExtension(TempFilename).ToLower() == ".sub")
        File.Delete(tempSUPfilename);
      //worker.ReportProgress(100);
      //if (e != null)
      //  e.Result = "Successfully read binary subtitle file!";
    }

    Color Black, White, Grey, Transp;

    public void ReadHDSUP(ref FileStream fs)
    {
      float ticksPerMSec = 90.000F;
      if (!IsPAL) ticksPerMSec = 90.090F;
      bool PackageEndTimeNotSpecified = true;		// Some SUPs don't specify the end display time (so use the new start time to set the previous end time)

      //bool SetPalette=true;
      bool SetPalette = false;

      if (SUPList.Count == 0)
        // Only create when needed
        SUPList = new ArrayList(2500);
      else
        ClearSUP();

      int i;
      byte[] b = new byte[4];

      // Keep track of time
      DateTime CurrentTime = new DateTime(0);

      // Start scanning
      while ((i = fs.ReadByte()) != -1)
      {
        //				System.Windows.Forms.Application.DoEvents();

        // Check whether we need to abort
        //if (worker.CancellationPending) {
        //  //Abort = false;
        //  fs.Position = fs.Length;
        //}


        long mypos = fs.Position;

        if (i == (int)'S' && (i = fs.ReadByte()) == (int)'P')
        {
          // Show progress
          //worker.ReportProgress((int)((fs.Position * 100) / fs.Length));

          // Found new header SP
          SUPdata data = new SUPdata();
          data.SUPlanguage = "";

          // Read start time: PTS = Presentation Time Stamp
          fs.Read(b, 0, 4);
          int PTS = Swap4Bytes(b);

          //data.PackageStartTime = data.PackageStartTime.AddMilliseconds(Math.Round(PTS/ticksPerMSec));
          data.PackageStartTime = data.PackageStartTime.AddMilliseconds(Math.Round(PTS / 90.0F));

          // Read four empty bytes
          fs.Read(b, 0, 4);

          // Save SUP reference position
          long refPos = fs.Position;
          // Pointer to next package
          fs.Read(b, 0, 2);	// As it is a HD-SUP they will be 0000
          // Now read 4 bytes (SPU_SZ)
          fs.Read(b, 0, 4);
          data.nextSUP = refPos + (long)((int)b[0] << 24 | (int)b[1] << 16 | (int)b[2] << 8 | (int)b[3]);
          // Pointer to start control sequence
          fs.Read(b, 0, 4);
          data.startControlSeq = refPos + (long)((int)b[0] << 24 | (int)b[1] << 16 | (int)b[2] << 8 | (int)b[3]);

          // Jump to control sequence
          fs.Position = data.startControlSeq;

          long NextControl = 0;
          // Read control code
          while (fs.Position < data.nextSUP)
          {
            // Read time
            fs.Read(b, 0, 2);
            // DCSQ_STM
            int TimeOfControl = ((int)b[0] << 8 | (int)b[1]);
            // Read offset to next control sequence
            fs.Read(b, 0, 4);
            long tmp = refPos + ((int)b[0] << 24 | (int)b[1] << 16 | (int)b[2] << 8 | (int)b[3]);						// Are we dealing with the last control seq (next control equals previous next control)
            if (NextControl != tmp && tmp > fs.Position)
              NextControl = tmp;
            else
              NextControl = data.nextSUP;

            while (fs.Position < NextControl && fs.Position < data.nextSUP)
            {
              mypos = fs.Position;
              i = fs.ReadByte();
              switch (i)
              {
                case 1:
                  // Start displaying
                  //data.PackageStartTime = data.PackageStartTime.AddMilliseconds(TimeOfControl/90D);
                  if (TimeOfControl > 0)
                    data.PackageStartTime = data.PackageStartTime.AddMilliseconds(((TimeOfControl << 10) + 1023) / ticksPerMSec);
                  if (PackageEndTimeNotSpecified && GetNoOfSubtitles() > 0)
                    this.SetEndTime(GetNoOfSubtitles() - 1, data.PackageStartTime.AddMilliseconds(-1));
                  PackageEndTimeNotSpecified = true;
                  break;
                case 2:
                  PackageEndTimeNotSpecified = false;
                  // Stop displaying
                  data.PackageEndTime = data.PackageEndTime.AddTicks(data.PackageStartTime.Ticks - data.PackageEndTime.Ticks);
                  data.PackageEndTime = data.PackageEndTime.AddMilliseconds(((TimeOfControl << 10) + 1023) / ticksPerMSec);
                  break;
                case 3:
                  // Color mapping
                  fs.Read(b, 0, 2);
                  data.ColorSet[3] = (byte)((int)b[0] >> 4);
                  data.ColorSet[2] = (byte)((int)b[0] & 0xF);
                  data.ColorSet[1] = (byte)((int)b[1] >> 4);
                  data.ColorSet[0] = (byte)((int)b[1] & 0xF);
                  data.newColorSet = data.ColorSet;
                  break;
                case 0x83:
                  // HD Color mapping
                  for (int ind = 0; ind < 256; ind++)
                  {
                    // Read Y, Cr, Cb
                    int Y = fs.ReadByte();
                    int Cr = fs.ReadByte();
                    int Cb = fs.ReadByte();

                    // Convert them to RGB
                    Y = Y - 16;
                    Cr = Cr - 128;
                    Cb = Cb - 128;

                    int RGB =
                    (((int)Math.Min(Math.Max(Math.Round(1.1644F * Y + 1.596F * Cr), 0), 255)) << 16) +
                    (((int)Math.Min(Math.Max(Math.Round(1.1644F * Y - 0.813F * Cr - 0.391F * Cb), 0), 255)) << 8) +
                    (int)Math.Min(Math.Max(Math.Round(1.1644F * Y + 2.018F * Cb), 0), 255);

                    data.HDColorSet[ind] = RGB;
                  }
                  data.HDnewColorSet = data.HDColorSet;
                  break;
                case 4:
                  // Transparency
                  fs.Read(b, 0, 2);
                  if (b[0] == 0 && b[1] == 0) break;		// Added for some sups that seem to have some codes resetting all colors
                  data.Transparency[3] = (byte)((b[0] & 0xF0) >> 4);	//(((int) b[0] & 0xF0) == 0);
                  data.Transparency[2] = (byte)(b[0] & 0x0F); //(((int) b[0] & 0x0F) == 0);
                  data.Transparency[1] = (byte)((b[1] & 0xF0) >> 4);	//(((int) b[1] & 0xF0) == 0);
                  data.Transparency[0] = (byte)(b[1] & 0x0F); //(((int) b[1] & 0x0F) == 0);
                  break;
                case 0x84:
                  // HD Transparency
                  for (int ind = 0; ind < 256; ind++)
                  {
                    data.HDTransparency[ind] = (byte)fs.ReadByte();
                  }
                  break;
                case 5:
                  // Subtitle position (and size) xx.xX.XX.yy.yY.YY
                  // Subtitle size is width W=(XXX-xxx+1) and height H=(YYY-yyy+1)
                  fs.Read(b, 0, 3);
                  data.BitmapPos.X = (int)b[0] << 4 | ((int)b[1] & 0xF0) >> 4;
                  data.BitmapPos.Width = (((int)b[1] & 0xF) << 8 | (int)b[2]) -
                    data.BitmapPos.X + 1;
                  fs.Read(b, 0, 3);
                  data.BitmapPos.Y = (int)b[0] << 4 | ((int)b[1] & 0xF0) >> 4;
                  data.BitmapPos.Height = (((int)b[1] & 0xF) << 8 | (int)b[2]) -
                    data.BitmapPos.Y + 1;
                  // Check the values found here
                  if (data.BitmapPos.Width < 0 || data.BitmapPos.Width > MAX_WIDTH ||
                      data.BitmapPos.Height < 0 || data.BitmapPos.Height > MAX_HEIGHT_PAL)
                  {
                    // Something went wrong: reset them and jump to next SP
                    data.BitmapPos.Width = 0;
                    data.BitmapPos.Height = 0;
                    fs.Position = data.nextSUP;
                  }
                  break;
                // Exactly the same as 0x05
                case 0x85:
                  // Subtitle position (and size) xx.xX.XX.yy.yY.YY
                  // Subtitle size is width W=(XXX-xxx+1) and height H=(YYY-yyy+1)
                  fs.Read(b, 0, 3);
                  data.BitmapPos.X = (int)b[0] << 4 | ((int)b[1] & 0xF0) >> 4;
                  data.BitmapPos.Width = (((int)b[1] & 0xF) << 8 | (int)b[2]) -
                    data.BitmapPos.X + 1;
                  fs.Read(b, 0, 3);
                  data.BitmapPos.Y = (int)b[0] << 4 | ((int)b[1] & 0xF0) >> 4;
                  data.BitmapPos.Height = (((int)b[1] & 0xF) << 8 | (int)b[2]) -
                    data.BitmapPos.Y + 1;
                  // Check the values found here
                  if (data.BitmapPos.Width < 0 || data.BitmapPos.Width > 1920 ||
                      data.BitmapPos.Height < 0 || data.BitmapPos.Height > 1080)
                  {
                    // Something went wrong: reset them and jump to next SP
                    data.BitmapPos.Width = 0;
                    data.BitmapPos.Height = 0;
                    fs.Position = data.nextSUP;
                  }
                  break;
                case 6:
                  // Start of first and second RLE bitmap line (interleaved)
                  // 11.11.22.22
                  fs.Read(b, 0, 4);
                  data.startBitmapInterleaved1 = refPos +
                    ((uint)b[0] << 8 | (uint)b[1]);
                  data.startBitmapInterleaved2 = refPos +
                    ((uint)b[2] << 8 | (uint)b[3]);
                  break;
                case 0x86:
                  // Start of first and second RLE bitmap line (interleaved)
                  // 11.11.22.22
                  fs.Read(b, 0, 4);
                  data.startBitmapInterleaved1 = refPos +
                    ((uint)b[0] << 24 | (uint)b[1] << 16 | (uint)b[2] << 8 | (uint)b[3]);
                  fs.Read(b, 0, 4);
                  data.startBitmapInterleaved2 = refPos +
                    ((uint)b[0] << 24 | (uint)b[1] << 16 | (uint)b[2] << 8 | (uint)b[3]);
                  break;
                case 7:
                  // Change color and contrast - don't know what to do with it, so skip
                  fs.Read(b, 0, 2);
                  int length = ((int)b[0] << 8 | (int)b[1]) - 2;
                  data.ChgColCon = new byte[length];
                  fs.Read(data.ChgColCon, 0, length);
                  //fs.Position += length;
                  break;
                case 0x87:
                  // HD Change color and contrast - don't know what to do with it, so skip
                  // First get size
                  int extfs = (fs.ReadByte() << 8) | fs.ReadByte();
                  // Then skip it
                  fs.Position = fs.Position + extfs;
                  break;
                case 0xFF:
                  // End/escape sequence - don't do anything
                  break;
                default:
                  // If unknown, skip
                  break;
              }
            }
          }

          if (SetPalette)
          {
            SetPalette = false;

            // If first SUP, set the colors
            // Set transparent color
            Transp = Color.FromArgb(0, 0, 0, 0);

            // Retrieve the darkest color from the HD-SUP palette
            Black = GetDarkestColor(data.HDColorSet);

            // Retrieve the brightest color from the HD-SUP palette
            White = GetBrightestColor(data.HDColorSet);

            // Compute a middle-grey color
            Grey = Color.FromArgb((White.R + Black.R) / 2, (White.G + Black.G) / 2, (White.B + Black.B) / 2);

            // Force the palette
            Colors[0] = Transp;
            Colors[1] = Black;
            Colors[2] = Grey;
            Colors[3] = White;

            colorIndex[0] = 0;
            colorIndex[1] = 1;
            colorIndex[2] = 2;
            colorIndex[3] = 3;

            // Assign and set
            backgndColor = Colors[0];
            foregndColor = Colors[3];
            outlineColor = Colors[1];
            antialiasColor = Colors[2];
          }

          if (data.PackageStartTime >= CurrentTime) ConvertHDSUP2Bitmap(fs, data);
          // Save in PNG (Portable Networks Graphics) format -> more compressed than bitmap
          //					string myFile = "C:\\temp\\SUPBitmap" + NoOfSUPs + ".png";

          // Fill the SD color table
          data.ColorSet[3] = 3;
          data.ColorSet[2] = 2;
          data.ColorSet[1] = 1;
          data.ColorSet[0] = 0;

          // Fill the transparency table
          data.Transparency[3] = 0xf;
          data.Transparency[2] = 0xf;
          data.Transparency[1] = 0xf;
          data.Transparency[0] = 0x0;

          data.newColorSet = data.ColorSet;

          // Go to the next SUP
          fs.Position = data.nextSUP;

          if (data.PackageStartTime >= CurrentTime)
          {
            CurrentTime = data.PackageStartTime;
            if (data.PackageEndTime < data.PackageStartTime)
              data.PackageEndTime = data.PackageStartTime.AddMilliseconds(100);
            SUPList.Add(data);
          }
          else
            data.Dispose();
          //if (NoOfSUPs > 589)
          //	NoOfSUPs+=0;
        }

        // To be fixed lated
        //			SUPPreviewForm.SetIFOColours();				
      }
    }

    #region Example how to invert a bitmap
    /*
			public static bool Invert(Bitmap b)
			{
				// GDI+ still lies to us - the return format is BGR, NOT RGB.
				BitmapData bmData = b.LockBits(new Rectangle(0, 0, b.Width, b.Height),
					ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
				int stride = bmData.Stride;
				System.IntPtr Scan0 = bmData.Scan0;
				unsafe
				{
					byte * p = (byte *)(void *)Scan0;
					int nOffset = stride - b.Width*3;
					int nWidth = b.Width * 3;
					for(int y=0;y < b.Height;++y)
					{
						for(int x=0; x < nWidth; ++x )
						{
							p[0] = (byte)(255-p[0]);
							++p;
						}
						p += nOffset;
					}
				}
			}
			b.UnlockBits(bmData);
		 */
    #endregion

    /// <summary>
    /// Get the darkest color from the HD-SUP palette
    /// </summary>
    private Color GetDarkestColor(int[] HDColor)
    {
      // Retrieve the darkest color from the HD-DVD 8 bit palette
      Color tempcolor, darkestcolor;
      darkestcolor = Color.FromArgb(255, 255, 255);
      for (int palind = 1; palind < 256; palind++)
      {
        int RGB = HDColor[palind];
        byte red = (byte)((RGB & 0xff0000) >> 16);
        byte grn = (byte)((RGB & 0xff00) >> 8);
        byte blu = (byte)(RGB & 0xff);
        tempcolor = Color.FromArgb(red, grn, blu);
        if (tempcolor.GetBrightness() < darkestcolor.GetBrightness())
          darkestcolor = tempcolor;
      }
      return darkestcolor;
    }

    /// <summary>
    /// Get the brightest color from the HD-SUP palette
    /// </summary>
    private Color GetBrightestColor(int[] HDColor)
    {
      // Retrieve the darkest color from the HD-DVD 8 bit palette
      Color tempcolor, brightestcolor;
      brightestcolor = Color.FromArgb(0, 0, 0);
      for (int palind = 1; palind < 256; palind++)
      {
        int RGB = HDColor[palind];
        byte red = (byte)((RGB & 0xff0000) >> 16);
        byte grn = (byte)((RGB & 0xff00) >> 8);
        byte blu = (byte)(RGB & 0xff);
        tempcolor = Color.FromArgb(red, grn, blu);
        if (tempcolor.GetBrightness() > brightestcolor.GetBrightness())
          brightestcolor = tempcolor;
      }
      return brightestcolor;
    }


    /// <summary>
    /// Convert the HD-SUP's RLE stream to a bitmap
    /// </summary>
    /// <param name="fs">FileStream fs</param>
    /// <param name="data">SUPdata data</param>
    private void ConvertHDSUP2Bitmap(FileStream fs, SUPdata data)
    {
      int[] Translate = new int[4];
      Translate[0] = 1;
      Translate[1] = 2;
      Translate[2] = 5;
      Translate[3] = 10;
      int ShowEveryLine = Translate[ShowEverySUP];

      // Show every xth line, show the first 5, and show the last 4kb of subtitles
      if ((SUPList.Count + 1) % ShowEveryLine == 0 || SUPList.Count == 0 ||
          SUPList.Count < 5 ||
          fs.Position > fs.Length - 9096)
      {
        // In order to get the subtitle borders (that part containing text), first
        // check which color is transparent (hopefully, only the background), and
        // furthermore copy the bitmaps position and size as an outer limit.
        int Xmin = data.BitmapPos.Width;
        int Xmax = 0;
        int Ymin = data.BitmapPos.Height;
        int Ymax = 0;

        _HDbmpSUP = new Bitmap(1920, 1080, PixelFormat.Format32bppArgb);
        BitmapData HDbmData = _HDbmpSUP.LockBits(new Rectangle(0, 0, _HDbmpSUP.Width, _HDbmpSUP.Height),
                                    ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);

        int stride = HDbmData.Stride;
        System.IntPtr Scan0 = HDbmData.Scan0;
        unsafe
        {
          byte* p = (byte*)(void*)Scan0;
          byte* p0 = p;

          int x, y, RunLength, color;
          bool EOL = false;
          long EOData;
          for (int j = 0; j < 2; j++)
          {
            if (j == 0)
            {
              // Go to interleaved 1 position
              fs.Position = data.startBitmapInterleaved1;
              EOData = data.startBitmapInterleaved2;
              y = 0;
            }
            else
            {
              // Go to interleaved 2 position
              fs.Position = data.startBitmapInterleaved2;
              EOData = data.startControlSeq;
              p = p0 + stride;
              y = 1;
            }
            x = RunLength = color = 0;

            /*						HD-RLE decoding
                        (P=Color Index, R=RLE)
						
                        1 pixel - 2 bits
                        00PP
						
                        2-9 pixels – 2 bits
                        10PP0RRR
						
                        10-136 pixels – 2 bits
                        10PP1RRRRRRR
						
                        Full Line – 2 bits
                        10PP10000000
						
                        1 pixel – 8 bits
                        01PPPPPPPP
						
                        2-9 pixels – 8 bits
                        11PPPPPPPP0RRR
						
                        10-136 pixels – 8 bits
                        11PPPPPPPP1RRRRRRR
						
                        To end of line – 8 bits
                        11PPPPPPPP10000000
            */

            while (y < data.BitmapPos.Height && fs.Position < EOData)
            {

              // Read data and set in bitmap
              RunLength = Read2Bits(fs);

              // If d0 is 0 --> 2 bit pattern (4 first colors)
              // If d0 is 1 --> 8 bit pattern (252 last colors)
              if ((RunLength & 0x01) == 0)
              {
                // 2 bit RLE - Get 2 bit color
                color = Read2Bits(fs);
              }
              else
              {
                // 8 bit RLE - Get 8 bit color
                color = Read2Bits(fs);
                color = (color << 2) + Read2Bits(fs);
                color = (color << 2) + Read2Bits(fs);
                color = (color << 2) + Read2Bits(fs);
              }

              RunLength = RunLength & 0x02;
              if ((RunLength) == 0)
              {
                // No RLE compression
                RunLength = 1;
              }
              else
              {
                // RLE compression
                RunLength = Read2Bits(fs);

                // Test MSB value
                if ((RunLength & 0x02) == 0)
                {
                  // 2 to 9 pixels
                  RunLength = (RunLength << 2) + Read2Bits(fs);
                  RunLength += 2;
                }
                else
                {
                  // more than 9 pixels
                  RunLength &= 1;
                  RunLength = (RunLength << 2) + Read2Bits(fs);
                  RunLength = (RunLength << 2) + Read2Bits(fs);
                  RunLength = (RunLength << 2) + Read2Bits(fs);

                  if (RunLength == 0)
                  {
                    RunLength = data.BitmapPos.Width - x;
                    EOL = true;
                  }
                  else RunLength += 9;
                }
              }

              // Determine the subtitle borders (just in case, somebody makes
              // them really big, and I only want to display the subtitles)
              // Compute the outline of the subtitle
              if (!(data.HDTransparency[color] == TRANSPARENT))// && y < data.BitmapPos.Height)
              {
                if (Xmin > x)
                  Xmin = x;
                if (Xmax < x + RunLength && x + RunLength <= data.BitmapPos.Width)
                  Xmax = x + RunLength;			// -1 CHECK THIS!!!!
                if (Ymin > y)
                  Ymin = y;
                if (Ymax < y && y < data.BitmapPos.Height)
                  Ymax = y;
              }

              // Convert color using color mapping
              int xe = x + RunLength;
              int RGB = data.HDColorSet[color];
              byte red = (byte)((RGB & 0xff0000) >> 16);
              byte grn = (byte)((RGB & 0xff00) >> 8);
              byte blu = (byte)(RGB & 0xff);
              byte alp = (byte)(255 - data.HDTransparency[color]);

              // Implement pel's transparency fix (don't know why but it works)
              if (color == 0) alp = 0;
              else alp = 255;
              for (; x < xe; x++)
              {
                // Check if we are not outside our allowed drawing area:
                // although this should not happen, I include it because I don't
                // know the exact sup format, and it happened sometimes.
                // Write in BGRAlpha!!
                *p++ = blu;
                *p++ = grn;
                *p++ = red;
                *p++ = alp;
              }

              if (EOL || x >= data.BitmapPos.Width)
              {
                EOL = false;
                x = 0;					// Reset
                y += 2;					// Keep track of vertical pos. in bmp
                p = p0 + y * stride;		// Go to next line
                Read2Bits(true);		// Flush
              }
            }
          }
        }
        _HDbmpSUP.UnlockBits(HDbmData);

        Bitmap SDbmpSUP = new Bitmap(_bmpSUP.Width, _bmpSUP.Height, _HDbmpSUP.PixelFormat);

        // Resize according to set video mode (PAL or NTSC)
        //				SDbmpSUP = (Bitmap) ImageResize(_HDbmpSUP,_bmpSUP.Width,_bmpSUP.Height,System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor);
        SDbmpSUP = (Bitmap)ImageResize(_HDbmpSUP, VideoSizeX, Instance.VideoSizeY, System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor);

        // Second convert it to 4 bit/indexed
        BitmapData SD32data = SDbmpSUP.LockBits(new Rectangle(0, 0, SDbmpSUP.Width, SDbmpSUP.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
        BitmapData SD4data = _bmpSUP.LockBits(new Rectangle(0, 0, _bmpSUP.Width, _bmpSUP.Height), ImageLockMode.WriteOnly, PixelFormat.Format4bppIndexed);

        ColorPalette pal = _bmpSUP.Palette;

        byte index_transp = 0;
        byte index_black = 1;
        byte index_grey = 2;
        byte index_white = 3;

        // Transparent
        pal.Entries[index_transp] = Transp;
        pal.Entries[index_black] = Black;
        pal.Entries[index_grey] = Grey;
        pal.Entries[index_white] = White;
        _bmpSUP.Palette = pal; // The crucial statement

        System.IntPtr ReadScan0 = SD32data.Scan0;
        System.IntPtr WriteScan0 = SD4data.Scan0;

        unsafe
        {
          byte* pread = (byte*)(void*)ReadScan0;
          byte* pwrite = (byte*)(void*)WriteScan0;
          byte red, grn, blu, alp, tmpcolor;
          tmpcolor = 0;
          Color readcolor;

          // Like black and white for example.
          int sublastline = ((data.BitmapPos.Height * _bmpSUP.Height / _HDbmpSUP.Height) + 3);
          for (int line = 0; line < sublastline; line++)
          {
            for (int pixel = 0; pixel < SDbmpSUP.Width; pixel++)
            {
              blu = *pread++;
              grn = *pread++;
              red = *pread++;
              alp = *pread++;

              readcolor = Color.FromArgb(red, grn, blu);
              float readcolbrightness = readcolor.GetBrightness();

              if ((pixel & 1) == 0)
              {
                if (alp < 128) tmpcolor = (byte)(index_transp << 4);
                else if (readcolbrightness < 0.2F) tmpcolor = (byte)(index_black << 4);
                else if (readcolbrightness > 0.7F) tmpcolor = (byte)(index_white << 4);
                else tmpcolor = (byte)(index_grey << 4);
              }
              else
              {
                if (alp < 128) tmpcolor |= index_transp;
                else if (readcolbrightness < 0.2F) tmpcolor |= index_black;
                else if (readcolbrightness > 0.5F) tmpcolor |= index_white;
                else tmpcolor |= index_grey;

                *pwrite++ = tmpcolor;
              }
            }
          }
        }

        // Here we have a 4bpp indexed bitmap using 4 colors: black, grey, white and transparent
        SDbmpSUP.UnlockBits(SD32data);
        _bmpSUP.UnlockBits(SD4data);

        // Resize data parameters according to resized SD subtitle
        //				float Xfactor = (float) _bmpSUP.Width/ (float)_HDbmpSUP.Width;
        float Xfactor = (float)VideoSizeX / (float)_HDbmpSUP.Width;
        //				float Yfactor = (float) _bmpSUP.Height/ (float)_HDbmpSUP.Height;
        float Yfactor = (float)VideoSizeY / (float)_HDbmpSUP.Height;
        data.BitmapPos.Width = (int)(data.BitmapPos.Width * Xfactor) + 1;
        data.BitmapPos.Height = (int)(data.BitmapPos.Height * Yfactor) + 1;
        data.BitmapPos.X = (int)(data.BitmapPos.X * Xfactor) - 1;
        data.BitmapPos.Y = (int)(data.BitmapPos.Y * Yfactor) - 1;

        if (Xmin >= 0 && Xmax > 1 && Ymin >= 0 && Ymax > 1)
        {
          //if (Xmin < 10 && Xmax > data.BitmapPos.Width - 10 && Ymin < 10 && Ymax > data.BitmapPos.Height- 10)
          // Keep the original size if it is almost the same
          //	data.SetSubtitleBorders(0,data.BitmapPos.Width-1,0,data.BitmapPos.Height-1);
          //else
          data.SetSubtitleBorders(Xmin,
                                  Math.Min(data.BitmapPos.Width - 1, Xmax - 1),
                                  Ymin,
                                  Math.Min(data.BitmapPos.Height - 1, Ymax));
        }
        else
          data.SetSubtitleBorders(0, 1, 0, 1);

        data.SetSubtitlePicture(ref _bmpSUP);
      }
      else
      {
        data.SetSubtitleBorders(0, 1, 0, 1);
        data.SetSubtitlePicture(ref _bmpSUP);
      }
    }

    /// <summary>
    /// Resize an Image
    /// </summary>
    /// <param name="img">The Image to resize</param>
    /// <param name="destWidth">The final width of the resized image</param>
    /// <param name="destHeight">The final height of the resized image</param>
    /// <param name="Intermode">The interpolation mode (System.Drawing.Drawing2D.InterpolationMode)</param>
    Image ImageResize(Image img, int destWidth, int destHeight, System.Drawing.Drawing2D.InterpolationMode Intermode)
    {
      int sourceWidth = img.Width;
      int sourceHeight = img.Height;

      Bitmap bm = new Bitmap(destWidth, destHeight, img.PixelFormat);
      Graphics gr = Graphics.FromImage(bm);

      gr.InterpolationMode = Intermode; // This one is much faster and quite the same
      //gr.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;

      gr.DrawImage(img, new Rectangle(0, 0, destWidth, destHeight), new Rectangle(0, 0, sourceWidth, sourceHeight), GraphicsUnit.Pixel);
      gr.Dispose();

      return bm;
    }

    /// <summary>
    /// Convert the SUP's RLE stream to a bitmap
    /// </summary>
    /// <param name="fs">FileStream fs</param>
    /// <param name="data">SUPdata data</param>
    private void Convert2Bitmap(FileStream fs, SUPdata data)
    {
      int[] Translate = new int[4];
      Translate[0] = 1;
      Translate[1] = 2;
      Translate[2] = 5;
      Translate[3] = 10;
      int ShowEveryLine = 1;//Translate[mySettings.ShowEverySUP];

      // Show every xth line, show the first 5, and show the last 4kb of subtitles
      if ((SUPList.Count + 1) % ShowEveryLine == 0 || SUPList.Count == 0 ||
          SUPList.Count < 5 ||
          fs.Position > fs.Length - 9096)
      {
        // In order to get the subtitle borders (that part containing text), first
        // check which color is transparent (hopefully, only the background), and
        // furthermore copy the bitmaps position and size as an outer limit.
        int Xmin = data.BitmapPos.Width;
        int Xmax = 0;
        int Ymin = data.BitmapPos.Height;
        int Ymax = 0;

        BitmapData bmData;
        if (data.ChgColCon == null)
        {
          if (_bmpSUP.PixelFormat != PixelFormat.Format4bppIndexed)
          {
            _bmpSUP.Dispose();
            _bmpSUP = new Bitmap(MAX_WIDTH, MAX_HEIGHT_PAL, PixelFormat.Format4bppIndexed);
          }
          bmData = _bmpSUP.LockBits(new Rectangle(0, 0, _bmpSUP.Width, _bmpSUP.Height),
                                    ImageLockMode.WriteOnly, PixelFormat.Format4bppIndexed);
        }
        else
        {
          if (_bmpSUP.PixelFormat != PixelFormat.Format8bppIndexed)
          {
            _bmpSUP.Dispose();
            _bmpSUP = new Bitmap(MAX_WIDTH, MAX_HEIGHT_PAL, PixelFormat.Format8bppIndexed);
          }
          bmData = _bmpSUP.LockBits(new Rectangle(0, 0, _bmpSUP.Width, _bmpSUP.Height),
                                    ImageLockMode.WriteOnly, PixelFormat.Format8bppIndexed);
        }

        #region Setting the color palette for indexed pictures
        /* http://www.charlespetzold.com/pwcs/PaletteChange.html
				ColorPalette pal = img.Palette;
				for (int i=0; i<4; i++)
					pal.Entries[i] = clr;
				img.Palette = pal; // The crucial statement
				
				// Make sure that in my case, the color palette only contains 4 entries, for each color index
				// in the original bitmap. This allows me to use the same IFO color twice, for example when the
				// original colors reference the same color, once transparent, and once opaque.
				 */
        #endregion
        ColorPalette pal = _bmpSUP.Palette;

        if (data.customColors != null)
        {
          for (int i = 0; i < 4; i++)
          {
            // Get color
            if (data.customTransparency[i] == true)
            {
              // Transparent, alpha = 0, color specified in ARGB
              // Set the background color here
              //pal.Entries[i] = Color.FromArgb(0, mySettings.Colors[Index].R, mySettings.Colors[Index].G, mySettings.Colors[Index].B);
              pal.Entries[i] = Color.FromArgb(0, data.customColors[i]);
            }
            else
            {
              // Opaque, alpha = 255
              // Set the foreground colors here
              //pal.Entries[i] = Color.FromArgb(255, mySettings.Colors[Index].R, mySettings.Colors[Index].G, mySettings.Colors[Index].B);
              pal.Entries[i] = Color.FromArgb(255, data.customColors[i]);
            }
          }
        }
        else
        {

          for (int i = 0; i < 4; i++)
          {
            // Get color
            int Index = data.ColorSet[i];
            if (data.Transparency[i] == TRANSPARENT)
            {
              // Transparent, alpha = 0, color specified in ARGB
              // Set the background color here
              //pal.Entries[i] = Color.FromArgb(0, mySettings.Colors[Index].R, mySettings.Colors[Index].G, mySettings.Colors[Index].B);
              pal.Entries[i] = Color.FromArgb(0, subColors[Index].R, subColors[Index].G, subColors[Index].B);
            }
            else
            {
              // Opaque, alpha = 255
              // Set the foreground colors here
              //pal.Entries[i] = Color.FromArgb(255, mySettings.Colors[Index].R, mySettings.Colors[Index].G, mySettings.Colors[Index].B);
              pal.Entries[i] = Color.FromArgb(255, subColors[Index].R, subColors[Index].G, subColors[Index].B);
            }
          }
        }
        _bmpSUP.Palette = pal; // The crucial statement

        int stride = bmData.Stride;
        System.IntPtr Scan0 = bmData.Scan0;
        unsafe
        {
          byte* p = (byte*)(void*)Scan0;
          byte* p0 = p;
          //byte * pEnd = p0 + bmData.Height * bmData.Width * 3;
          //int nOffset = stride - bmpSUP.Width*3;
          //int nWidth = bmpSUP.Width * 3;

          int x, y, RunLength, color, NoOfZeros;
          bool EOL = false;
          long EOData;
          for (int j = 0; j < 2; j++)
          {
            if (j == 0)
            {
              // Go to interleaved 1 position
              fs.Position = data.startBitmapInterleaved1;
              EOData = data.startBitmapInterleaved2;
              y = 0;
            }
            else
            {
              // Go to interleaved 2 position
              fs.Position = data.startBitmapInterleaved2;
              EOData = data.startControlSeq;
              p = p0 + stride;
              y = 1;
            }
            x = RunLength = color = 0;

            while (y < data.BitmapPos.Height && fs.Position < EOData)
            {
              // Read data and set in bitmap
              RunLength = Read2Bits(fs);
              if (RunLength > 0)
                color = Read2Bits(fs);
              else
              {
                // Runlength > 4
                NoOfZeros = 1;
                // Determine no of bits of Runlength
                while (((RunLength = Read2Bits(fs)) == 0) && (NoOfZeros < 7)) NoOfZeros++;
                // Read runlength - is 7 really the maximum length???
                if (NoOfZeros == 7)
                {
                  EOL = true;
                  color = RunLength;
                  RunLength = data.BitmapPos.Width - x;	// - 1 REMOVED Fill to end of line with color
                }
                else
                {
                  while (NoOfZeros-- > 0)
                    RunLength = (RunLength << 2) | Read2Bits(fs);
                  color = Read2Bits(fs);
                }
              }

              // Determine the subtitle borders (just in case, somebody makes
              // them really big, and I only want to display the subtitles)
              // Compute the outline of the subtitle
              if (!(data.Transparency[color] == TRANSPARENT))// && y < data.BitmapPos.Height)
              {
                if (Xmin > x)
                  Xmin = x;
                if (Xmax < x + RunLength && x + RunLength <= data.BitmapPos.Width)
                  Xmax = x + RunLength;			// -1 CHECK THIS!!!!
                if (Ymin > y)
                  Ymin = y;
                if (Ymax < y && y < data.BitmapPos.Height)
                  Ymax = y;
              }

              #region Example how to use Format4bppIndexed
              /* Source: http://www.bobpowell.net/lockingbits.htm
							 * Accessing individual pixels in a 4 bit per pixel image is handled in a similar manner.
							 * The upper and lower nibble of the byte must be dealt with separately and changing the
							 * contents of the odd X pixels should not effect the even X pixels. The code below shows
							 * how to perform this in C#.
							BitmapData bmd=bm.LockBits(new Rectangle(0, 0, 10, 10),
								System.Drawing.Imaging.ImageLockMode.ReadOnly, bm.PixelFormat);
							int offset = (y * bmd.Stride) + (x >> 1);
							byte currentByte = ((byte *)bmd.Scan0)[offset];
							if((x&1) == 1)
							{
								currentByte &= 0xF0;
								currentByte |= (byte)(colorIndex & 0x0F);
							}
							else
							{
								currentByte &= 0x0F;
								currentByte |= (byte)(colorIndex << 4);
							}
							((byte *)bmd.Scan0)[offset]=currentByte;
							 */
              #endregion

              // Convert color using color mapping: As the PixelFormat is 4bpp indexed,
              // each byte contains two pixels!
              int xe = x + RunLength;
              for (; x < xe; x++)
              {
                if (data.ChgColCon == null)
                {
                  // 4bpp
                  // Check if we are not outside our allowed drawing area:
                  // although this should not happen, I include it because I don't
                  // know the exact sup format, and it happened sometimes.
                  // Write in BGR!!
                  if ((x & 1) == 0)		// Even
                    p[0] = (byte)(color << 4);
                  //p[0]  = (byte) (data.ColorSet[color]<<4);
                  else
                  {
                    // Odd pixels
                    p[0] |= (byte)color;
                    //p[0] |= (byte) data.ColorSet[color];
                    p++;
                  }
                }
                else
                {
                  // 8bpp
                  p[0] = (byte)color; p++;
                }
              }

              if (EOL || x >= data.BitmapPos.Width)
              {
                EOL = false;
                x = 0;					// Reset
                y += 2;					// Keep track of vertical pos. in bmp
                p = p0 + y * stride;		// Go to next line
                Read2Bits(true);		// Flush
              }
            }
          }
          // At this point, the picture has been created and is fine. However,
          // there might be a CHG_COLCON specified, which means I would have to
          // change this before I unlock the bitmap again.
          // NOTE: What if I need to change the palette (esp. tranaparency)
          if (data.ChgColCon != null)
            ChangeColCon(Scan0, stride, data, Xmax);
        }
        _bmpSUP.UnlockBits(bmData);

        // Copy the relevant part to data, but first check its validity
        //if (Xmin >= 0 && Xmin < _bmpSUP.Width-1 && Xmax > 1 && Xmax < _bmpSUP.Width &&
        //	Ymin >= 0 && Ymin < _bmpSUP.Height-1&& Ymax > 1 && Ymax < _bmpSUP.Height)
        if (Xmin >= 0 && Xmax > 1 && Ymin >= 0 && Ymax > 1)
        {
          //if (Xmin < 10 && Xmax > data.BitmapPos.Width - 10 && Ymin < 10 && Ymax > data.BitmapPos.Height- 10)
          // Keep the original size if it is almost the same
          //	data.SetSubtitleBorders(0,data.BitmapPos.Width-1,0,data.BitmapPos.Height-1);
          //else
          data.SetSubtitleBorders(Xmin,
                                  Math.Min(data.BitmapPos.Width - 1, Xmax - 1),
                                  Ymin,
                                  Math.Min(data.BitmapPos.Height - 1, Ymax));
        }
        else
          data.SetSubtitleBorders(0, 1, 0, 1);

        data.SetSubtitlePicture(ref _bmpSUP);
        //_bmpSUP.Save("/home/cb4960/temp/blah.png", ImageFormat.Png);
      }
      else
      {
        data.SetSubtitleBorders(0, 1, 0, 1);
        data.SetSubtitlePicture(ref _bmpSUP);
      }
    }

    /// <summary>
    /// An advanced feature of a subpicture is to change color and contrast for a
    /// certain region of the subpicture. In order to visualize it, I use the following
    /// approach:
    /// - Decode subpicture as normal
    /// - Replace bytes with the new values using a hash table
    /// </summary>
    /// <param name="Scan0"></param>
    /// <param name="stride"></param>
    /// <param name="data"></param>
    private void ChangeColCon(IntPtr Scan0, int stride, SUPdata data, int Xmax)
    {

      #region mpucoder's example
      /*
				MPUCoder's DVD contains the following regional information:
				07 00 01 10 00 96 11 dd 00 00 62 42 ff f0 0f ff ff ff ff

				Decode:
				07         			Start CHG_COLCON
				00 10               2 byte value: total value of the parameter area, including the size word
				00 96 11 dd         4 byte value: 0s ss nt tt -->
            	          			sss (starting top line)= 0x096 = 150
                	      			n number of PX_CTLI to follow  =   1
                    	  			ttt (end line)         = 0x1dd = 477
				00 00 62 42 ff f0   6 bytes, starting column and new color and contrast values
    	                  			starting column  = 00 00
        	              			new color values = 62 42
            	          			contrast values  = ff f0
				0f ff ff ff         2 byte: end of parameter area
				ff					End of subpicture (does not belong to the 0x07 code)
			 */
      #endregion

      ColorPalette pal = _bmpSUP.Palette;

      int startLine = Math.Max(0, (int)((data.ChgColCon[0] << 8) | data.ChgColCon[1]) - data.BitmapPos.Y);
      int nPxCTLI = (int)(data.ChgColCon[2] >> 4);
      int endLine = (int)((((int)(data.ChgColCon[2] & 0x0F)) << 8) | data.ChgColCon[3]) - data.BitmapPos.Y;

      // Process each region within the vertical area between startLine and endLine
      byte[] colorSet = new byte[4];
      byte[] transparency = new byte[4];
      int ptr = 4;
      for (int k = 0; k < nPxCTLI; k++)
      {
        // Pointer to data.ChgColCon
        int startColumn = Math.Max(0, (int)((data.ChgColCon[ptr++] << 8) | data.ChgColCon[ptr++]) - data.BitmapPos.X);
        colorSet[3] = (byte)(data.ChgColCon[ptr] >> 4);
        colorSet[2] = (byte)(data.ChgColCon[ptr++] & 0x0F);
        colorSet[1] = (byte)(data.ChgColCon[ptr] >> 4);
        colorSet[0] = (byte)(data.ChgColCon[ptr++] & 0x0F);
        // Set transparency to either 255 or 0
        transparency[3] = (byte)((data.ChgColCon[ptr] & 0xF0) > 0 ? 255 : 0);
        transparency[2] = (byte)((data.ChgColCon[ptr++] & 0x0F) > 0 ? 255 : 0);
        transparency[1] = (byte)((data.ChgColCon[ptr] & 0xF0) > 0 ? 255 : 0);
        transparency[0] = (byte)((data.ChgColCon[ptr++] & 0x0F) > 0 ? 255 : 0);
        int endColumn;
        if (k + 1 < nPxCTLI)
          endColumn = (int)((data.ChgColCon[ptr] << 8) | data.ChgColCon[ptr + 1]) - data.BitmapPos.X;
        else
          endColumn = Xmax;

        // Add colours to the palette
        for (int i = 0; i < 4; i++)
        {
          int Index = colorSet[i];
          pal.Entries[(k + 1) * 4 + i] = Color.FromArgb(transparency[i],
                                                  Colors[Index].R, Colors[Index].G, Colors[Index].B);
        }

        // TOsDO: Add 4 to the region, i.e. use the previously defined color set
        // see mpucoder's explanation.
        byte addValue = (byte)((k + 1) * 4);
        unsafe
        {
          byte* p0 = (byte*)(void*)Scan0;
          byte* p;
          for (int y = startLine; y < endLine; y++)
          {
            p = p0 + y * stride;
            for (int x = startColumn; x < endColumn; x++)
            {
              p[x] += addValue;
            }
          }
        }
        // TODO: For Karaoke, often the region will be a few letters, and there will
        // be two regions defined, one with the alternate colours, and one with the
        // original ones. In order to reduce the processing, we can check whether the
        // second region uses the same colours as the original, so we can skip it.
      }
      _bmpSUP.Palette = pal;
    }

    private int val;
    private byte pos = 4;
    /// <summary>
    /// Read 2 bits from the input stream
    /// </summary>
    /// <param name="fs">Input FileStream</param>
    /// <returns></returns>
    private int Read2Bits(FileStream fs)
    {
      int shift;

      if (pos >= 3)
      {
        pos = 0;
        val = fs.ReadByte();
      }
      else
        pos++;
      shift = 2 * (3 - pos);
      return (val & 0x3 << shift) >> shift;
    }

    /// <summary>
    ///  After an end of line, flush the buffer, and begin with a new byte
    /// </summary>
    /// <param name="Flush">If true, reset the buffer and next time, read a new byte.</param>
    /// <returns></returns>
    private void Read2Bits(bool Flush)
    {
      if (Flush)
        pos = 4;
    }

    public int GetNoOfSubtitles()
    {
      return SUPList.Count;
    }

    private int Swap4Bytes(byte[] b)
    {
      return ((int)b[3] << 24 | (int)b[2] << 16 | (int)b[1] << 8 | (int)b[0]);
    }

    public Bitmap GetBitmap(int Index)
    {
      if (SUPList == null || Index >= SUPList.Count)
        return new Bitmap(1, 1);

      SUPdata data = (SUPdata)SUPList[Index];
      return data.SubtitlePicture;
    }

    /// <summary>
    /// Get the absolute position of a subtitle on the screen
    /// </summary>
    /// <param name="Index"></param>
    /// <returns></returns>
    public Rectangle GetSubtitleBorders(int Index)
    {
      SUPdata data = (SUPdata)SUPList[Index];
      return data.SubtitleBorders;
    }

    /// <summary>
    /// Determine the width and height of the subtitle
    /// </summary>
    /// <param name="Index"></param>
    /// <returns></returns>
    public Rectangle GetSubtitleRectangle(int Index)
    {
      SUPdata data = (SUPdata)SUPList[Index];
      return data.GetSubtitleRectangle();
    }

    /// <summary>
    /// Shift the subtitles with a certain value up or down
    /// </summary>
    public void ShiftSubtitles(int VerticalSUPOffset)
    {
      for (int i = 0; i < SUPList.Count; i++)
      {
        // Add vertical offset
        Rectangle rect = GetBitmapPosition(i);
        rect.Y += VerticalSUPOffset;
        // Make sure that it's not outside the image (bottom)
        rect.Y = Math.Min(rect.Y, VideoSizeY - rect.Height);
        // If the position becomes negative (e.g. when a subtitle is near the top, don't change the position)
        if (rect.Y > 0)
          SetBitmapPosition(i, rect);
      }
    }

    public void ShiftTime(TimeSpan ts)
    {
      for (int i = 0; i < SUPList.Count; i++)
      {
        SUPdata mySUPdata = (SUPdata)SUPList[i];
        mySUPdata.PackageStartTime += ts;
        mySUPdata.PackageEndTime += ts;
      }
    }

  }
}
