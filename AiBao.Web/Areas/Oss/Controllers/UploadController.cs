using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AiBao.Web.Areas.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using AiBao.Services;
using cts.web.core.Librs;
using Microsoft.AspNetCore.Hosting;
using AiBao.Core.MediaItem;
using cts.web.core.MediaItem;
using System.DrawingCore;

namespace AiBao.Web.Areas.Oss.Controllers
{ 
    public class UploadController : AreaOssController
    {
        SettingService _settingService;
        BucketService _bucketService;
        BucketImageService _bucketImageService;
        IHostingEnvironment _hostingEnvironment;
        IMediaItemStorage _mediaItemStorage;

        public UploadController(SettingService settingService,
            BucketImageService bucketImageService,
            BucketService bucketService,
            IHostingEnvironment hostingEnvironment,
            IMediaItemStorage mediaItemStorage)
        {
            _mediaItemStorage = mediaItemStorage;
            _hostingEnvironment = hostingEnvironment;
            _bucketService = bucketService;
            _settingService = settingService;
            _bucketImageService = bucketImageService;
        }

        /// <summary>
        /// 获取签名字符串
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        // oss/upload/signature
        [Route("signature")]
        [HttpPost]
        public IActionResult Signature([FromBody]SignatureModel model)
        {
            if (!ModelState.IsValid)
                return NoValid();

            var settings = _settingService.GetMasterSettings();
            if (String.IsNullOrEmpty(settings.OSSAccessKeyId) || String.IsNullOrEmpty(settings.OSSAccessKeyId))
            {
                ApiData.code = 2001;
                ApiData.msg = "暂未开放上传操作";
                return Ok(ApiData);
            }
            if (!settings.OSSAccessKeyId.Equals(model.AccessKeyId, StringComparison.InvariantCultureIgnoreCase))
            {
                ApiData.code = 2001;
                ApiData.msg = "AccessKeyId错误";
                return Ok(ApiData);
            }
            var signatureString = EncryptorHelper.HmacSha1(settings.OSSAccessKeySecret, $"{model.VERB}{model.ContentMD5}");
            ApiData.code = 0;
            ApiData.msg = "获取成功";
            ApiData.data = new { Signature = signatureString };
            return Ok(ApiData);
        }


        /// <summary>
        /// 上传图片
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        //POST /oss/upload/image
        [HttpPost("image")]
        public async Task<IActionResult> UpImage([FromForm]UploadModel model)
        {
            #region 数据验证

            if (!ModelState.IsValid)
                return NoValid();
            if (Request.Form.Files == null || Request.Form.Files.Count == 0 || !Request.Form.Files[0].IsImage())
            {
                ApiData.code = 1006;
                ApiData.msg = "请上传文件";
                return Ok(ApiData);
            }
            var bucket = _bucketService.GetBucketBayName(model.bucket);
            if (bucket == null)
            {
                ApiData.code = 2001;
                ApiData.msg = "bucket错误";
                return Ok(ApiData);
            }
            #endregion

            IFormFile file = Request.Form.Files[0];

            string sha1 = file.GetSHA1();

            var item = _bucketImageService.GetSHA1(sha1);
            if (item != null)
            {
                ApiData.code = 0;
                ApiData.msg = "上传成功";
                ApiData.data = new { url = $"/oss/imagecn{item.VisitUrl}" };
                return Ok(ApiData);
            }

            if (!ValidSignature(model.signature, file.GetMD5(), model.VERB))
            {
                ApiData.code = 1005;
                ApiData.msg = "签名验证失败";
                return Ok(ApiData);
            }

            string fileName = Guid.NewGuid().ToString();//文件名
            string visitUrl = $"/{bucket.Name}/{fileName}";
            Guid id = CombGuid.NewGuid();//entityid
                                         //存储目录
            string path = System.IO.Path.Combine(MediaItemConfig.RootDir, bucket.Name, DateTime.Today.ToString("yyyy-MM-dd"), id.ToString());
            bool compress = false;
            if (bucket.IsCompress)
            {
                if (file.Length > 1024 * 800)
                    compress = true;
            }
            //保存文件并且获取文件的相对存储路径
            var image = file.CreateImagePathFromStream(_mediaItemStorage, path, compress, 70);

            _bucketImageService.AddImage(new Entities.BucketImage()
            {
                Id = id,
                BucketId = bucket.Id,
                CreationTime = DateTime.Now,
                FileName = file.Name,
                IOPath = image.IOPath,
                SHA1 = sha1,
                VisitUrl = visitUrl,
                Width = image.Width,
                Height = image.Height,
                ExtName=image.ExtName
            });
            ApiData.code = 0;
            ApiData.msg = "上传成功";
            ApiData.data = new { url = $"/oss/imagecn{visitUrl}" };
            await Task.FromResult(0);
            return Ok(ApiData);
        }

        /// <summary>
        /// 验证签名
        /// </summary>
        /// <param name="signature"></param>
        /// <param name="contentMD5"></param>
        /// <param name="VERB"></param>
        /// <returns></returns>
        private bool ValidSignature(string signature, string contentMD5, string VERB)
        {
            var settings = _settingService.GetMasterSettings();
            string signatureString = EncryptorHelper.HmacSha1(settings.OSSAccessKeySecret, $"{VERB}{contentMD5}");
            return signatureString.Equals(signature, StringComparison.InvariantCultureIgnoreCase);
        }



    }
}