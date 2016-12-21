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
  /// Utilities related to snapshots.
  /// </summary>
  public class UtilsSnapshot
  {
    /// <summary>
    /// Take a snapshot of a video at the provided time.
    /// </summary>
    public static void takeSnapshotFromVideo(string inFile, DateTime snapTime, ImageSize size, ImageCrop crop, string outFile)
    {
      string startTimeArg = UtilsVideo.formatStartTimeArg(snapTime);
      string videoSizeArg = UtilsVideo.formatVideoSizeArg(inFile, size, crop, 2, 2);
      string cropArg = UtilsVideo.formatCropArg(inFile, size, crop);

      string ffmpegSnapshotProgArgs = "";

      // Example format: 
      // -y -an -ss 00:03:33.370 -i "G:\Temp\input.mkv" -s 358x202 -f image2 -vf crop=358:202:0:0 -vframes 1 "output.jpg"
      ffmpegSnapshotProgArgs = String.Format("-y -an {0} -i \"{1}\" -f image2 -vf \"{2}, {3}\" -vframes 1 \"{4}\"",
                                            // Time to take snapshot at
                                            startTimeArg,  // {0}

                                            // Filename                            
                                            inFile,        // {1}

                                            // Filters
                                            videoSizeArg,  // {2}
                                            cropArg,       // {3}
        
                                            // Output name        
                                            outFile);      // {4}

      UtilsCommon.startFFmpeg(ffmpegSnapshotProgArgs, false, true);
    }


    /// <summary>
    /// Get image from file in a way that the file is not locked by Windows afterwards.
    /// </summary>
    /// <param name="filename">The image file</param>
    public static Image getImageFromFile(string filename)
    {
      FileStream imageStream = File.Open(filename, FileMode.Open, FileAccess.Read);
      Byte[] imageBytes = new Byte[imageStream.Length];

      imageStream.Read(imageBytes, 0, Convert.ToInt32(imageStream.Length));
      imageStream.Close();

      MemoryStream imageMemStream = new MemoryStream(imageBytes);
      Image image = Image.FromStream(imageMemStream);
      imageMemStream.Dispose();

      return image;
    }


    /// <summary>
    /// Fit an image to a size while correctly preserving the aspect ratio.
    /// </summary>
    public static Image fitImageToSize(Image image, Size sizeToFit)
    {
      // Check if we really need to resize
      if ((image.Width < sizeToFit.Width) && (image.Height < sizeToFit.Height))
      {
        return image;
      }

      float ratioH = sizeToFit.Height / (float)image.Height;
      float ratioW = sizeToFit.Width / (float)image.Width;

      int newWidth = 1;
      int newHeight = 1;

      if (ratioH < ratioW)
      {
        newWidth = (int)(ratioH * image.Width);
        newHeight = sizeToFit.Height;
      }
      else
      {
        newWidth = sizeToFit.Width;
        newHeight = (int)(ratioW * image.Height);
      }

      image = resizeImage(image, new Size(newWidth, newHeight));

      return image;
    }


    /// <summary>
    /// Resize an image to the desired size.
    /// Taken from: http://www.switchonthecode.com/tutorials/csharp-tutorial-image-editing-saving-cropping-and-resizing
    /// </summary>
    public static Image resizeImage(Image imgToResize, Size size)
    {
      int sourceWidth = imgToResize.Width;
      int sourceHeight = imgToResize.Height;

      float nPercent = 0;
      float nPercentW = 0;
      float nPercentH = 0;

      nPercentW = ((float)size.Width / (float)sourceWidth);
      nPercentH = ((float)size.Height / (float)sourceHeight);

      if (nPercentH < nPercentW)
      {
        nPercent = nPercentH;
      }
      else
      {
        nPercent = nPercentW;
      }

      int destWidth = (int)(sourceWidth * nPercent);
      int destHeight = (int)(sourceHeight * nPercent);

      Bitmap b = new Bitmap(destWidth, destHeight);
      Graphics g = Graphics.FromImage((Image)b);
      g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

      g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
      g.Dispose();

      return (Image)b;
    }



    /// <summary>
    /// Concatenate a series of images vertically and left justified.
    /// </summary>
    public static Image concatImages(List<Image> images)
    {
      int maxWidth = 0;
      int totalHeight = 0;

      // Get totals
      foreach (Image image in images)
      {
        if (image.Width > maxWidth)
        {
          maxWidth = image.Width;
        }

        totalHeight += image.Height;
      }

      // Create a graphics object of the desired size
      Bitmap bitmap = new Bitmap(maxWidth, totalHeight);
      Graphics g = Graphics.FromImage(bitmap);

      int curY = 0;

      // Paste each image to the graphics object's image
      foreach (Image image in images)
      {
        g.DrawImage(image, 0, curY);
        curY += image.Height;
      }

      return (Image)bitmap;
    }


  }
}
