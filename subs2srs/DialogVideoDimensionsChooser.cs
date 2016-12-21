using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace subs2srs
{
  /// <summary>
  /// The Video/Snapshot dimensions dialog
  /// </summary>
  public partial class DialogVideoDimensionsChooser : Form
  {
    public enum VideoDimesionsChoooserType
    {
      Snapshot = 0,
      Video = 1
    }


    private VideoDimesionsChoooserType chooserType;
    ImageSize newRes = new ImageSize(720, 480);
    private bool isLoaded = false;


    public VideoDimesionsChoooserType ChooserType
    {
      get { return ChooserType; }
      set { ChooserType = value; }
    }


    public bool IsLoaded
    {
      get { return isLoaded; }
      set { isLoaded = value; }
    }


    public ImageSize VideoDimensions
    {
      get 
      { 
        return new ImageSize((int)numericUpDownOrigWidth.Value, (int)numericUpDownOrigHeight.Value);
      }

      set
      {
        numericUpDownOrigWidth.Value = (decimal)value.Width;
        numericUpDownOrigHeight.Value = (decimal)value.Height;
        computeNewDimesions();
      }
    }


    public ImageSize NewDimensions
    {
      get { return new ImageSize(newRes.Width, newRes.Height); }
    }


    public DialogVideoDimensionsChooser(VideoDimesionsChoooserType chooserType)
    {
      InitializeComponent();

      this.chooserType = chooserType;

      if (chooserType == VideoDimesionsChoooserType.Video)
      {
        this.Text = "Video Clip Dimensions Chooser";
      }
      else
      {
        this.Text = "Snapshot Dimensions Chooser";
      }
    }

 
    private void computeNewDimesions()
    {
      int widthIncrement = 2;
      int heightIncrement = 2;

      newRes.Width = (int)(((float)this.numericUpDownPercent.Value / 100.0) * (int)numericUpDownOrigWidth.Value);
      newRes.Height = (int)(((float)this.numericUpDownPercent.Value / 100.0) * (int)numericUpDownOrigHeight.Value);

      if (chooserType == VideoDimesionsChoooserType.Video)
      {
        widthIncrement = 16;
      }

      newRes.Width = UtilsCommon.getNearestMultiple(newRes.Width, widthIncrement);
      newRes.Height = UtilsCommon.getNearestMultiple(newRes.Height, heightIncrement);

      if (newRes.Width < 16)
      {
        newRes.Width = 16;
      }
      else if (newRes.Width > 2048)
      {
        newRes.Width = 2048;
      }

      if (newRes.Height < 16)
      {
        newRes.Height = 16;
      }
      else if(newRes.Height > 2048)
      {
        newRes.Height = 2048;
      }

      this.textBoxNewWidth.Text = newRes.Width.ToString();
      this.textBoxNewHeight.Text = newRes.Height.ToString();
    }


    private void recomuputeDimesions(object sender, EventArgs e)
    {
      isLoaded = true;
      computeNewDimesions();
    }



  }
}
