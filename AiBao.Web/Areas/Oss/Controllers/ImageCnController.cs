using System;
using System.Collections.Generic;
using System.DrawingCore;
using System.DrawingCore.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AiBao.Core.MediaItem;
using AiBao.Framework.Skia;
using AiBao.Services;
using AiBao.Services.Setting;
using cts.web.core.Librs;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using SkiaSharp;

namespace AiBao.Web.Areas.Oss.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [ResponseCache(Location = ResponseCacheLocation.Any, Duration = 86400)]
    public class ImageCnController : AreaOssController
    {
        BucketImageService _bucketImageService;
        IHostingEnvironment _hostingEnvironment;
        MarkLogoService _markLogoService;

        public ImageCnController(BucketImageService bucketImageService,
            MarkLogoService markLogoService,
            IHostingEnvironment hostingEnvironment)
        {
            _markLogoService = markLogoService;
            _bucketImageService = bucketImageService;
            _hostingEnvironment = hostingEnvironment;
        }

        /// <summary>
        /// 查看原图
        /// </summary>
        /// <param name="bucket"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        [Route("{bucket}/{name}")]
        public IActionResult Get(string bucket, string name)
        {
            string visitUrl = $"/{bucket}/{name}";

            var item = _bucketImageService.GetByVisitUrl(visitUrl);
            if (item == null) return NotFile();

            string abPath = Path.Combine(_hostingEnvironment.ContentRootPath, item.IOPath);
            if (!System.IO.File.Exists(abPath))
            {
                return NotFile();
            }

            using (FileStream fs = new FileStream(abPath, FileMode.Open))
            {
                byte[] bt = new byte[fs.Length];
                fs.Read(bt, 0, bt.Length);
                return File(bt, $"image/{item.ExtName?.Substring(1)}");
            }
        }

        /// <summary>
        /// 剪裁、水印
        /// </summary>
        /// <param name="bucket"></param>
        /// <param name="name"></param>
        /// <param name="query">参数，以'逗号分隔',例如:m_fill,w_200xh_300,l_logo</param>
        /// <returns></returns>
        [Route("{bucket}/{name}!{query}")]
        public IActionResult GetAppendMark(string bucket, string name, string query)
        {
            string visitUrl = $"/{bucket}/{name}";
            var item = _bucketImageService.GetByVisitUrl(visitUrl);
            if (item == null) return NotFile();

            string abPath = Path.Combine(_hostingEnvironment.ContentRootPath, item.IOPath);
            if (!System.IO.File.Exists(abPath))
                return NotFile();

            string cut = null, resize = null;
            Stream stream = null;

            if (!String.IsNullOrEmpty(query))
            {
                var arr = query.Split(',');
                foreach (var q in arr)
                {
                    string temp = q.ToLower();
                    if (temp.StartsWith("m_"))
                        cut = temp;
                    if (temp.StartsWith("w_") || temp.StartsWith("h_"))
                        resize = temp;
                    if (temp.Equals("l_logo"))
                    {
                        stream = _markLogoService.GetStream();
                    }
                }
            }

            using (var img = SkiaHelper.MakeThumb(abPath, cut, resize, item.ExtName, stream))
            {
                return File(img.ToArray(), $"image/{item.ExtName?.Substring(1)}");
            }
        }

        /// <summary>
        /// 没有文件时返回
        /// </summary>
        /// <returns></returns>
        private IActionResult NotFile()
        {
            var bt = _markLogoService.Get404Stream();
            if (bt == null)
                return NotFound();
            return File(bt, "image/png");
        }
    }




}