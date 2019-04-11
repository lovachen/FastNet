using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace AiBao.Framework.Skia
{
    /// <summary>
    /// SkiaSharp 工具类
    /// </summary>
    public class SkiaHelper
    {
        private const int QUALITY = 75;

        /// <summary>
        /// 创建缩略图，并且格式化
        /// </summary>
        /// <param name="abPath"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static SKData MakeThumb(string abPath, int width, int height, CroppingMode mode, SKEncodedImageFormat format)
        {
            using (var input = System.IO.File.OpenRead(abPath))
            {
                if (mode == CroppingMode.NONE)
                {
                    return SKImage.FromEncodedData(input).Encode(format, QUALITY);
                }
                using (var inputStream = new SKManagedStream(input))
                {
                    using (var bitmap = SKBitmap.Decode(inputStream))
                    {
                        var cropRect = new CroppingRectangle(bitmap, width, height, mode);

                        SKBitmap croppedBitmap = new SKBitmap((int)cropRect.Dest.Width, (int)cropRect.Dest.Height);

                        using (SKCanvas canvas = new SKCanvas(croppedBitmap))
                        {
                            canvas.DrawBitmap(bitmap, cropRect.Rect, cropRect.Dest);
                        }
                        return SKImage.FromBitmap(croppedBitmap).Encode(format, QUALITY);
                    }
                }
            }
        }

        /// <summary>
        /// 创建缩略图，并且格式化
        /// </summary>
        /// <param name="abPath">图片绝对路径</param>
        /// <param name="cut">指定的剪裁方式</param>
        /// <param name="resize">指定的剪裁大小</param>
        /// <param name="format">格式</param>
        /// <returns></returns>
        public static SKData MakeThumb(string abPath, string cut, string resize, SKEncodedImageFormat format)
        {
            var resolve = GetResolve(cut, resize);
            return MakeThumb(abPath, resolve.Width, resolve.Height, resolve.Mode, format);
        }

        /// <summary>
        /// 创建缩略图，并且格式化
        /// </summary>
        /// <param name="abPath">图片绝对路径</param>
        /// <param name="cut">指定的剪裁方式</param>
        /// <param name="resize">指定的剪裁大小</param>
        /// <param name="extName">文件扩展名</param>
        /// <returns></returns>
        public static SKData MakeThumb(string abPath, string cut, string resize, string extName)
        {
            var resolve = GetResolve(cut, resize);
            return MakeThumb(abPath, resolve.Width, resolve.Height, resolve.Mode, GetImageFormat(extName));
        }

        /// <summary>
        /// 创建缩略图，未格式化
        /// </summary>
        /// <param name="abPath"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        public static SKBitmap MakeThumb(string abPath, int width, int height, CroppingMode mode)
        {
            using (var input = System.IO.File.OpenRead(abPath))
            {
                if (mode == CroppingMode.NONE)
                {
                    return SKBitmap.Decode(input);
                }
                using (var inputStream = new SKManagedStream(input))
                {
                    using (var bitmap = SKBitmap.Decode(inputStream))
                    {
                        var cropRect = new CroppingRectangle(bitmap, width, height, mode);

                        SKBitmap croppedBitmap = new SKBitmap((int)cropRect.Dest.Width, (int)cropRect.Dest.Height);

                        using (SKCanvas canvas = new SKCanvas(croppedBitmap))
                        {
                            canvas.DrawBitmap(bitmap, cropRect.Rect, cropRect.Dest);
                        }
                        return croppedBitmap;
                    }
                }
            }
        }

        /// <summary>
        /// 创建缩略图，未格式化
        /// </summary>
        /// <param name="abPath"></param>
        /// <param name="cut"></param>
        /// <param name="resize"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        public static SKBitmap MakeThumb(string abPath, string cut, string resize)
        {
            var resolve = GetResolve(cut, resize);
            return MakeThumb(abPath, resolve.Width, resolve.Height, resolve.Mode);
        }



        /// <summary>
        /// 获取格式
        /// </summary>
        /// <param name="extName"></param>
        /// <returns></returns>
        public static SKEncodedImageFormat GetImageFormat(string extName)
        {
            SKEncodedImageFormat format = SKEncodedImageFormat.Jpeg;
            switch (extName?.ToLower())
            {
                case ".astc":
                    return SKEncodedImageFormat.Astc;
                case ".bmp":
                    return SKEncodedImageFormat.Bmp;
                case ".dng":
                    return SKEncodedImageFormat.Dng;
                case ".gif":
                    return SKEncodedImageFormat.Gif;
                case ".ico":
                    return SKEncodedImageFormat.Ico;
                case ".ktx":
                    return SKEncodedImageFormat.Ktx;
                case ".pkm":
                    return SKEncodedImageFormat.Pkm;
                case ".png":
                    return SKEncodedImageFormat.Png;
                case ".wbmp":
                    return SKEncodedImageFormat.Wbmp;
                case ".webp":
                    return SKEncodedImageFormat.Webp;
            }
            return format;
        }

        /// <summary>
        /// 获取剪裁方式
        /// </summary>
        /// <param name="cut">指定剪裁 m_fill：裁剪，m_h：指定高，m_w：指定宽</param>
        /// <param name="resize">剪裁大小</param>
        /// <returns></returns>
        public static ResolveArg GetResolve(string cut, string resize)
        {
            return new ResolveArg(cut, resize);
        }

        /// <summary>
        /// 测量文字 获取矩形
        /// </summary>
        /// <param name="text"></param>
        /// <param name="fontName"></param>
        /// <param name="fontSize"></param>
        /// <param name="fontStyle"></param>
        /// <returns></returns>
        public static SKRect MeasureText(string text, string fontName, float fontSize, SKFontStyle fontStyle)
        {
            SKRect rect = new SKRect();

            using (SKTypeface font = SKTypeface.FromFamilyName(fontName, fontStyle))
            {
                using (SKPaint paint = new SKPaint())
                {
                    paint.IsAntialias = true;
                    paint.Typeface = font;
                    paint.TextSize = fontSize;
                    paint.MeasureText(text, ref rect);
                }
            }
            return rect;
        }

        /// <summary>
        ///  测量文字 获取矩形
        /// </summary>
        /// <param name="text"></param>
        /// <param name="paint"></param>
        /// <returns></returns>
        internal static SKRect MeasureText(string text, SKPaint paint)
        {
            SKRect rect = new SKRect();
            paint.MeasureText(text, ref rect);
            return rect;
        }  

    }
}
