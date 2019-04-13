using SkiaSharp;
using System;
using System.Collections.Generic;
using System.IO;
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
        /// <param name="mode"></param>
        /// <param name="format"></param>
        /// <param name="logoPath">水印地址，不为空时添加水印</param>
        /// <returns></returns>
        public static SKData MakeThumb(string abPath, int width, int height, CroppingMode mode, SKEncodedImageFormat format, string logoPath)
        {
            using (var input = File.OpenRead(abPath))
            {
                SKBitmap bitmap = null;
                SKCanvas canvas = null;
                if (mode == CroppingMode.NONE)
                {
                    bitmap = SKBitmap.Decode(input);
                    canvas = new SKCanvas(bitmap);
                }
                else
                {
                    var source = SKBitmap.Decode(input);
                    var cropRect = new CroppingRectangle(source, width, height, mode);
                    bitmap = new SKBitmap((int)cropRect.Dest.Width, (int)cropRect.Dest.Height);
                    canvas = new SKCanvas(bitmap);
                    canvas.DrawBitmap(source, cropRect.Rect, cropRect.Dest);
                }
                if (!String.IsNullOrEmpty(logoPath))
                {
                    DrawMark(canvas, bitmap.Width, bitmap.Height, logoPath);
                }
                SKData data = SKImage.FromBitmap(bitmap).Encode(format, QUALITY);
                canvas.Dispose();
                bitmap.Dispose();
                return data;
            }
        }

        /// <summary>
        /// 创建缩略图，并且格式化
        /// </summary>
        /// <param name="abPath">图片绝对路径</param>
        /// <param name="cut">指定的剪裁方式</param>
        /// <param name="resize">指定的剪裁大小</param>
        /// <param name="format">格式</param>
        /// <param name="logoPath">水印地址，不为空时添加水印</param>
        /// <returns></returns>
        public static SKData MakeThumb(string abPath, string cut, string resize, SKEncodedImageFormat format, string logoPath)
        {
            var resolve = GetResolve(cut, resize);
            return MakeThumb(abPath, resolve.Width, resolve.Height, resolve.Mode, format, logoPath);
        }

        /// <summary>
        /// 创建缩略图，并且格式化
        /// </summary>
        /// <param name="abPath">图片绝对路径</param>
        /// <param name="cut">指定的剪裁方式</param>
        /// <param name="resize">指定的剪裁大小</param>
        /// <param name="extName">文件扩展名</param>
        /// <param name="logoPath">水印地址，不为空时添加水印</param>
        /// <returns></returns>
        public static SKData MakeThumb(string abPath, string cut, string resize, string extName, string logoPath)
        {
            var resolve = GetResolve(cut, resize);
            return MakeThumb(abPath, resolve.Width, resolve.Height, resolve.Mode, GetImageFormat(extName), logoPath);
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

        /// <summary>
        /// 添加水印
        /// </summary>
        /// <param name="height"></param>
        /// <param name="width"></param>
        /// <param name="canvas"></param>
        /// <param name="logoPath"></param>
        internal static void DrawMark(SKCanvas canvas, int width, int height, string logoPath)
        {
            if (File.Exists(logoPath))
            {
                using (var stream = File.OpenRead(logoPath))
                {
                    using (var bitMap = SKBitmap.Decode(stream))
                    {
                        int x = width / bitMap.Width + 2;
                        int y = height / bitMap.Height + 2;
                        canvas.RotateDegrees(-20, width / 2, height / 2);
                        for (int i = -2; i < x; i++)
                        {
                            for (int k = -3; k < y; k++)
                            {
                                canvas.DrawBitmap(bitMap, new SKPoint(bitMap.Width * i * 2f, bitMap.Height * k * 2f));
                            }
                        }
                    }
                }
            }
        }




    }
}
