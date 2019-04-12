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
        private SettingService _settingService;

        public ImageCnController(BucketImageService bucketImageService,
            IHostingEnvironment hostingEnvironment,
            SettingService settingService)
        {
            _settingService = settingService;
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
        /// 不剪裁+水印
        /// </summary>
        /// <param name="bucket"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        [Route("{bucket}/{name}/l_logo")]
        public IActionResult GetAppendMark(string bucket, string name, string l_logo)
        {
            string visitUrl = $"/{bucket}/{name}";
            var item = _bucketImageService.GetByVisitUrl(visitUrl);
            if (item == null) return NotFound();

            string abPath = Path.Combine(_hostingEnvironment.ContentRootPath, item.IOPath);
            if (!System.IO.File.Exists(abPath))
                return NotFound();

            string marPaht = Path.Combine(_hostingEnvironment.ContentRootPath, _settingService.GetMasterSettings().MarkPath);
            using (var img = SkiaHelper.MakeThumb(abPath, null, null, item.ExtName, marPaht))
            {
                return File(img.ToArray(), $"image/{item.ExtName?.Substring(1)}");
            }
        }

        /// <summary>
        /// 剪裁无水印
        /// </summary>
        /// <param name="bucket"></param>
        /// <param name="name"></param>
        /// <param name="cut">剪裁方式。m_fill,m_w,m_h</param>
        /// <param name="resize">剪裁大小，w_100,h_100,w_100xh_100</param>
        /// <param name="l_logo"></param>
        /// <returns></returns>
        [Route("{bucket}/{name}!{cut},{resize}")]
        public IActionResult GetCut(string bucket, string name, string cut, string resize)
        {
            string visitUrl = $"/{bucket}/{name}";
            var item = _bucketImageService.GetByVisitUrl(visitUrl);
            if (item == null) return NotFound();

            string abPath = Path.Combine(_hostingEnvironment.ContentRootPath, item.IOPath);
            if (!System.IO.File.Exists(abPath))
                return NotFound();

            using (var img = SkiaHelper.MakeThumb(abPath, cut, resize, item.ExtName, null))
            {
                return File(img.ToArray(), $"image/{item.ExtName?.Substring(1)}");
            }
        }

        /// <summary>
        /// 剪裁+水印
        /// </summary>
        /// <param name="bucket"></param>
        /// <param name="name"></param>
        /// <param name="cut"></param>
        /// <param name="resize"></param>
        /// <param name="l_logo"></param>
        /// <returns></returns>
        [Route("{bucket}/{name}!{cut},{resize}/l_logo")]
        public IActionResult GetCutAppendMark(string bucket, string name, string cut, string resize)
        {
            string visitUrl = $"/{bucket}/{name}";
            var item = _bucketImageService.GetByVisitUrl(visitUrl);
            if (item == null) return NotFound();

            string abPath = Path.Combine(_hostingEnvironment.ContentRootPath, item.IOPath);
            if (!System.IO.File.Exists(abPath))
                return NotFound();
            string marPaht = Path.Combine(_hostingEnvironment.ContentRootPath, _settingService.GetMasterSettings().MarkPath);
            using (var img = SkiaHelper.MakeThumb(abPath, cut, resize, item.ExtName, marPaht))
            {
                return File(img.ToArray(), $"image/{item.ExtName?.Substring(1)}");
            }
        }

    }




}