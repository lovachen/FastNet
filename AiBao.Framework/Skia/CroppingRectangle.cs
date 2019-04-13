using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace AiBao.Framework.Skia
{
    /// <summary>
    /// 剪裁矩形
    /// </summary>
    internal class CroppingRectangle
    {
        public CroppingRectangle(SKBitmap shourceBitmap, int width, int height, CroppingMode croppingMode)
        {
            SKRect dest = new SKRect(0, 0, width, height);
            SKRect rect = new SKRect(0, 0, shourceBitmap.Width, shourceBitmap.Height);
            switch (croppingMode)
            {
                case CroppingMode.RATIO_WIDTH:
                    dest.Bottom = rect.Bottom * width / rect.Right;
                    break;
                case CroppingMode.RATIO_HEIGHT:
                    dest.Right = rect.Right * height / rect.Bottom;
                    break;
                case CroppingMode.CUT:
                    SKRect maxRect = new SKRect(0, 0, shourceBitmap.Width, shourceBitmap.Height);

                    float oRight = maxRect.Right, oBottom = maxRect.Bottom;
                    //纵向比计算 
                    if (oRight / oBottom > (float)width / height)
                    {
                        rect.Top = 0;
                        float w = rect.Bottom * width / height;
                        rect.Left = (rect.Width - w) / 2;
                        rect.Right = rect.Left + w;
                    }
                    else
                    {
                        rect.Left = 0;
                        float h = rect.Right * height / width;
                        rect.Top = (rect.Bottom - h) / 2;
                        rect.Bottom = rect.Top + h;
                    }
                    break;
            }
            Rect = rect;
            Dest = dest;
        }

        /// <summary>
        /// 源矩形
        /// </summary>
        public SKRect Rect { get; }

        /// <summary>
        /// 目标矩形
        /// </summary>
        public SKRect Dest { get; }
    }

}
