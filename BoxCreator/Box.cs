﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace BoxCreator
{
  /// <summary>
  /// Class represents box. Holds functionality for creating box with or without cover.
  /// </summary>
  public class Box
  {
    /// <summary>
    /// Gap between wall and border of canvas
    /// </summary>
    public const int EdgeGap = 8;
    /// <summary>
    /// Gap between box walls 
    /// </summary>
    public const int WallGap = 10;
    /// <summary>
    /// Gap between box and cover (if box is created with cover)
    /// </summary>
    public const int CoverGap = 15;

    /// <summary>
    /// Initializes a new instance of the <see cref="Box"/> class.
    /// </summary>
    public Box()
    {
      UpWallColor = Colors.Yellow;
      BackWallColor = Colors.Orange;
      FrontWallColor = Colors.Beige;
      BottomWallColor = Colors.Navy;
      LeftWallColor = Colors.Orchid;
      RightWallColor = Colors.Olive;
      CoverWallColor = Colors.Yellow;
      FrontCoverWallColor = Colors.Yellow;
      BackCoverWallColor = Colors.Yellow;
      LeftCoverWallColor = Colors.Yellow;
      RightCoverWallColor = Colors.Yellow;
    }

    /// <summary>
    /// Rebuilds the specified main canvas.
    /// </summary>
    /// <param name="canvasToDisplayBox">The canvas on which walls of box will be displayed.</param>
    /// <param name="realWidth">Width of the real box.</param>
    /// <param name="realLength">Length of the real.</param>
    /// <param name="realHeight">Height of the real.</param>
    /// <param name="realCoverHeight">Height of the real cover.</param>
    /// <param name="mainCanvasHeight">Height of the main canvas.</param>
    /// <param name="mainCanvasWidth">Width of the main canvas.</param>
    public void Rebuild(Canvas canvasToDisplayBox, int realWidth, int realLength, int realHeight, int realCoverHeight, int mainCanvasHeight, int mainCanvasWidth)
    {
      BoxCanvas = canvasToDisplayBox;
      BoxCanvas.Children.Clear();

      RealWidth = realWidth;
      RealLength = realLength;
      RealHeight = realHeight;
      RealCoverHeight = realCoverHeight;
      MainCanvasHeight = mainCanvasHeight;
      MainCanvasWidth = mainCanvasWidth;

      Scale = GetScale(realWidth, realLength, realHeight, realCoverHeight); //TODO: mainCanvasHeigth, mainCanvasWidth

      UpWall = new Wall(GetSizeToDisplay(RealLength), GetSizeToDisplay(RealWidth), UpWallColor, WallType.Up);
      BackWall = new Wall(GetSizeToDisplay(RealLength), GetSizeToDisplay(RealHeight), BackWallColor, WallType.Back);
      BottomWall = new Wall(GetSizeToDisplay(RealLength), GetSizeToDisplay(RealWidth), BottomWallColor, WallType.Bottom);
      FrontWall = new Wall(GetSizeToDisplay(RealLength), GetSizeToDisplay(RealHeight), FrontWallColor, WallType.Front);
      LeftWall = new Wall(GetSizeToDisplay(RealHeight), GetSizeToDisplay(RealWidth), LeftWallColor, WallType.Left);
      RightWall = new Wall(GetSizeToDisplay(RealHeight), GetSizeToDisplay(RealWidth), RightWallColor, WallType.Right);

      CoverWall = new Wall(GetSizeToDisplay(RealHeight), GetSizeToDisplay(RealWidth), CoverWallColor, WallType.Cover);
      BackCoverWall = new Wall(GetSizeToDisplay(RealHeight), GetSizeToDisplay(RealWidth), BackCoverWallColor, WallType.BackCover);
      FrontCoverWall = new Wall(GetSizeToDisplay(RealHeight), GetSizeToDisplay(RealWidth), FrontCoverWallColor, WallType.FrontCover);
      LeftCoverWall = new Wall(GetSizeToDisplay(RealHeight), GetSizeToDisplay(RealWidth), LeftCoverWallColor, WallType.LeftCover);
      RightCoverWall = new Wall(GetSizeToDisplay(RealHeight), GetSizeToDisplay(RealWidth), RightCoverWallColor, WallType.RightCover);

      //calculation for displaying on the center the canvas
      int lengthToDisplay = (int)(2 * EdgeGap + 2 * WallGap + GetSizeToDisplay(2 * RealHeight + RealLength));
      int widthToDisplay = (int)(2 * EdgeGap + 3 * WallGap + GetSizeToDisplay(2 * RealHeight + 2 * RealWidth));
      if (IsWithCover)
      {//with coverHeight
        widthToDisplay = (int)(4 * WallGap + 2 * EdgeGap + CoverGap + GetSizeToDisplay(2 * RealHeight + 2 * RealWidth));
     }

      LeftShift = (mainCanvasHeight - lengthToDisplay) / 2;
      TopShift = (mainCanvasWidth - widthToDisplay) / 2;
    }

    /// <summary>
    /// Gets or sets the left shift to display box on the center of the canvas.
    /// </summary>
    /// <value>
    /// The left shift.
    /// </value>
    private int LeftShift { get; set; }

    /// <summary>
    /// Gets or sets the top shift to display box on the center of the canvas.
    /// </summary>
    /// <value>
    /// The top shift.
    /// </value>
    private int TopShift { get; set; }

    public MouseButtonEventHandler WallMouseButtonEventHandler { get; set; }

    /// <summary>
    /// Shows the box on the BoxCanvas.
    /// </summary>
    public void Show()
    {      
      if (IsWithCover)
      {
        AddWall(UpWall, TopShift + EdgeGap, LeftShift + EdgeGap + WallGap + GetSizeToDisplay(RealHeight));

      }
      else
      {
        AddWall(UpWall, TopShift + EdgeGap, LeftShift + EdgeGap + WallGap + GetSizeToDisplay(RealHeight));
        AddWall(BackWall, TopShift + EdgeGap + WallGap + GetSizeToDisplay(RealWidth), LeftShift + EdgeGap + WallGap + GetSizeToDisplay(RealHeight));
        AddWall(BottomWall, TopShift + EdgeGap + 2 * WallGap + GetSizeToDisplay(RealWidth + RealHeight), LeftShift + EdgeGap + WallGap + GetSizeToDisplay(RealHeight));
        AddWall(FrontWall, TopShift + EdgeGap + 3 * WallGap + GetSizeToDisplay(2 * RealWidth + RealHeight), LeftShift + EdgeGap + WallGap + GetSizeToDisplay(RealHeight));
        AddWall(LeftWall, TopShift + EdgeGap + 2 * WallGap + GetSizeToDisplay(RealWidth + RealHeight), LeftShift + EdgeGap);
        AddWall(RightWall, TopShift + EdgeGap + 2 * WallGap + GetSizeToDisplay(RealWidth + RealHeight), LeftShift + EdgeGap + 2*WallGap + GetSizeToDisplay(RealHeight + RealLength));
      }
    }

    private void AddWall(Wall upWall, int top, int left)
    {
      BoxCanvas.Children.Add(upWall.WallCanvas);
      Canvas.SetLeft(upWall.WallCanvas, left);
      Canvas.SetTop(upWall.WallCanvas, top);
      upWall.WallCanvas.MouseLeftButtonDown += WallMouseButtonEventHandler;
     
    }

    /// <summary>
    /// Gets the size to display for given real dimention.
    /// </summary>
    /// <param name="realDimention">The real dimention.</param>
    /// <returns>Size to diplay.</returns>
    private int GetSizeToDisplay(int realDimention)
    {
      return (int)(Scale * realDimention);
    }

    /// <summary>
    /// Gets the main canvas, where box will be display.
    /// </summary>
    public Canvas BoxCanvas { get; private set; }

    /// <summary>
    /// Gets the width of the real box.
    /// </summary>
    /// <value>
    /// The width of the real box.
    /// </value>
    public int RealWidth { get; private set; }

    /// <summary>
    /// Gets the length of the real.
    /// </summary>
    /// <value>
    /// The length of the real.
    /// </value>
    public int RealLength { get; private set; }

    /// <summary>
    /// Gets the height of the real.
    /// </summary>
    /// <value>
    /// The height of the real.
    /// </value>
    public int RealHeight { get; private set; }

    /// <summary>
    /// Gets the height of the real cover.
    /// </summary>
    /// <value>
    /// The height of the real cover.
    /// </value>
    public int RealCoverHeight { get; private set; }

    /// <summary>
    /// Gets the height of the main canvas.
    /// </summary>
    /// <value>
    /// The height of the main canvas.
    /// </value>
    public int MainCanvasHeight { get; private set; }

    /// <summary>
    /// Gets the width of the main canvas.
    /// </summary>
    /// <value>
    /// The width of the main canvas.
    /// </value>
    public int MainCanvasWidth { get; private set; }

    /// <summary>
    /// Gets the scale used to display the box on main canvas.
    /// </summary>
    public double Scale { get; private set; }

    public Wall UpWall { get; private set; }
    public Wall BackWall { get; private set; }
    public Wall BottomWall { get; private set; }
    public Wall FrontWall { get; private set; }
    public Wall LeftWall { get; private set; }
    public Wall RightWall { get; private set; }

    public bool IsWithCover { get { return RealCoverHeight > 0; } }

    public Wall CoverWall { get; private set; }
    public Wall BackCoverWall { get; private set; }
    public Wall FrontCoverWall { get; private set; }
    public Wall LeftCoverWall { get; private set; }
    public Wall RightCoverWall { get; private set; }

    public Color UpWallColor { get; set; }
    public Color BackWallColor { get; set; }
    public Color BottomWallColor { get; set; }
    public Color FrontWallColor { get; set; }
    public Color LeftWallColor { get; set; }
    public Color RightWallColor { get; set; }

    public Color CoverWallColor { get; set; }
    public Color BackCoverWallColor { get; set; }
    public Color FrontCoverWallColor { get; set; }
    public Color LeftCoverWallColor { get; set; }
    public Color RightCoverWallColor { get; set; }

    /// <summary>
    /// Gets the scale which should be used to display the walls.
    /// </summary>
    /// <param name="width">The width of box.</param>
    /// <param name="length">The lenght of box.</param>
    /// <param name="height">The height of box.</param>
    /// <param name="coverHeight">Height of the cover (if cover used > 0; ohterwise 0).</param>
    /// <param name="tableSize">Size of the table where box will be shown.</param>
    /// <returns>
    /// Scale calculated.
    /// </returns>
    /// <remarks>
    /// To calculte is used <see cref="EdgeGap"/>, <see cref="WallGap"/> and <see cref="CoverGap"/>.
    /// </remarks>
    private double GetScale(int width, int length, int height, int coverHeight, int tableSize = 550)
    {
      //scale which will be return
      double scale = 1;
      //gaps which apear on length
      int gapsLength = 2 * EdgeGap + 2 * WallGap;
      //gaps which apear on width
      int gapsWidth = 3 * WallGap + 2 * EdgeGap;
      //length of expanded box (only box) 
      int boxExpandedLength = 2 * height + length;
      //width of expanded box (only box) 
      int boxExpandedWidth = 2 * height + 2 * width;

      if (coverHeight > 0)
      {//with coverHeight
        boxExpandedWidth = 2 * height + 2 * width + 2 * coverHeight;
        gapsWidth = 4 * WallGap + 2 * EdgeGap + CoverGap;
      }

      //scale should be used only for box not for gap!!!
      if (gapsLength + boxExpandedLength > gapsWidth + boxExpandedWidth)
      {
        if (gapsLength + boxExpandedLength > tableSize)
        {
          scale = (double)(boxExpandedLength) / (tableSize - gapsLength);
        }
        else
        {
          scale = (double)(tableSize - gapsLength) / (boxExpandedLength);
        }

      }
      else
      {
        if (gapsWidth + boxExpandedWidth > tableSize - gapsWidth)
        {
          scale = (double)(boxExpandedWidth) / (tableSize - gapsWidth);
        }
        else
        {
          scale = (double)(tableSize - gapsWidth) / (boxExpandedWidth);
        }

      }

      return scale;
    }
  }
}