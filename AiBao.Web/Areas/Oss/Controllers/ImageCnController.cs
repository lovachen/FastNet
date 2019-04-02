using System;
using System.Collections.Generic;
using System.DrawingCore;
using System.DrawingCore.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AiBao.Core.MediaItem;
using AiBao.Services;
using cts.web.core.Librs;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace AiBao.Web.Areas.Oss.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class ImageCnController : AreaOssController
    {
        BucketImageService _bucketImageService;
        IHostingEnvironment _hostingEnvironment;

        public ImageCnController(BucketImageService bucketImageService,
            IHostingEnvironment hostingEnvironment)
        {
            _bucketImageService = bucketImageService;
            _hostingEnvironment = hostingEnvironment;
        }

        [Route("{bucket}/{name}")]
        public IActionResult Get(string bucket, string name)
        {
            string visitUrl = $"/{bucket}/{name}";

            var item = _bucketImageService.GetByVisitUrl(visitUrl);
            if (item == null) return NotFound();

            string abPath = Path.Combine(_hostingEnvironment.ContentRootPath, item.IOPath);
            if (!System.IO.File.Exists(abPath))
                return NotFound();

            using (FileStream fs = new FileStream(abPath, FileMode.Open))
            {
                byte[] bt = new byte[fs.Length];
                fs.Read(bt, 0, bt.Length);
                return File(bt, $"image/{item.ExtName?.Substring(1)}");
            }
        }

        /// <summary>
        /// 获取图片，并且剪裁
        /// </summary>
        /// <param name="bucket"></param>
        /// <param name="name"></param>
        /// <param name="cut">剪裁方式。m_fill,m_w,m_h</param>
        /// <param name="resize">剪裁大小，w_100,h_100,w_100xh_100</param>
        /// <param name="l_logo"></param>
        /// <returns></returns>
        [Route("{bucket}/{name}!{cut},{resize}")]
        public IActionResult Get(string bucket, string name, string cut, string resize)
        {
            string visitUrl = $"/{bucket}/{name}";
            var item = _bucketImageService.GetByVisitUrl(visitUrl);
            if (item == null) return NotFound();

            string abPath = Path.Combine(_hostingEnvironment.ContentRootPath, item.IOPath);
            if (!System.IO.File.Exists(abPath))
                return NotFound();

            using (var img = CutImage(abPath, cut, resize))
            {
                ImageFormat format = ImageFormat.Jpeg;
                string contentType = "image/jpeg";
                switch (item.ExtName?.ToLower())
                {
                    case ".jpg":
                    case ".jpeg":
                        contentType = "image/jpeg";
                        break;
                    case ".png":
                        format = ImageFormat.Png;
                        contentType = "image/png";
                        break;
                    case ".gif":
                        format = ImageFormat.Gif;
                        contentType = "image/gif";
                        break;
                }
                using (MemoryStream ms = new MemoryStream())
                {
                    img.Save(ms, format);
                    return File(ms.ToArray(), contentType);
                }
            }
        }


        #region 私有方法

        /// <summary>
        /// 剪裁图片
        /// </summary>
        /// <param name="abPath"></param>
        /// <param name="cut"></param>
        /// <param name="resize"></param>
        /// <returns></returns>
        private Image CutImage(string abPath, string cut, string resize)
        {
            Image image = Image.FromFile(abPath);
            if (resize.StartsWith("w_") && resize.StartsWith("h_"))
            {
                return image;
            }
            ThumbnailProportion pro = ThumbnailProportion.WIDTH;
            int width = 0, height = 0;
            string w, h;
            if (resize.IndexOf('x') > -1)
            {
                var arr = resize.Split("x");
                for (int i = 0; i < arr.Length; i++)
                {
                    if (arr[i].StartsWith("w_"))
                    {
                        w = arr[i].Replace("w_", "");
                        int.TryParse(w, out width);
                    }
                    else
                    {
                        h = arr[i].Replace("h_", "");
                        int.TryParse(h, out height);
                    }
                } 
            }
            else if (resize.StartsWith("h_"))
            {
                h = resize.Replace("h_", "");
                int.TryParse(h, out height);
            }
            else if (resize.StartsWith("w_"))
            {
                w = resize.Replace("w_", "");
                int.TryParse(w, out width);
            }
            switch (cut)
            {
                case "m_fill":
                    pro = ThumbnailProportion.CUT;
                    if (width <= 0 || height <= 0)
                        return image;
                    break;
                case "m_fixed":
                    pro = ThumbnailProportion.WIDTH_HEIHT;
                    if (width <= 0 || height <= 0)
                        return image;
                    break;
                case "m_h":
                    pro = ThumbnailProportion.HEIHT;
                    if (height <= 0)
                        return image;
                    break;
                case "m_w":
                    pro = ThumbnailProportion.WIDTH;
                    if (width <= 0)
                        return image; break;
                default:
                    return image;
            }
            var thumb = ImageHelper.CreateThumb(image, pro, width, height);
            image.Dispose();
            return thumb;
        }

        #endregion







    }
}