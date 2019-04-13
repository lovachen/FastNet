using AspNetCore.Cache;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AiBao.Services.Setting
{
    public class MarkLogoService
    {
        private readonly static object lockObj = new object();

        private const string FILE_NAME = "mark_logo.png";
        private const string FILE_NAME_404 = "img_404_orrer.png";
        private const string MARK_LOGO = "ab.services.marklogo";
        private const string IMG_404="ab.services.404.img";
        private ICacheManager _cacheManager;
        private IHostingEnvironment _env;

        public MarkLogoService(ICacheManager cacheManager,
            IHostingEnvironment env)
        {
            _env = env;
            _cacheManager = cacheManager;
        }

        /// <summary>
        /// 获取水印的图片，并且缓存
        /// </summary>
        /// <returns></returns>
        public Stream GetStream()
        {
            var byteArr = _cacheManager.Get<byte[]>(MARK_LOGO, () =>
             {
                 string path = Path.Combine(_env.WebRootPath, FILE_NAME);
                 if (File.Exists(path))
                 {
                     using (var stream = File.OpenRead(path))
                     {
                         byte[] bt = new byte[stream.Length];
                         stream.Read(bt, 0, bt.Length);
                         return bt;
                     }
                 }
                 return null;
             });
            if (byteArr != null)
                return new MemoryStream(byteArr);
            return null;
        }

        /// <summary>
        /// 存储水印图片
        /// </summary>
        /// <param name="stream"></param>
        public void SaveStream(Stream stream)
        {
            lock (lockObj)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    using (var fs = File.OpenWrite(Path.Combine(_env.WebRootPath, FILE_NAME)))
                    {
                        stream.CopyTo(ms);
                        ms.WriteTo(fs);
                        fs.Flush();
                        fs.Close();
                        _cacheManager.Remove(MARK_LOGO);
                    }
                }
            }
        }

        /// <summary>
        /// 移除缓存
        /// </summary>
        public void RemoveCache()
        {
            _cacheManager.Remove(MARK_LOGO);
        }

        /// <summary>
        /// 图片不存在是默认返回的图片
        /// </summary>
        /// <returns></returns>
        public byte[] Get404Stream()
        {
            return _cacheManager.Get<byte[]>(IMG_404, () =>
            {
                string path = Path.Combine(_env.ContentRootPath, FILE_NAME_404);
                if (File.Exists(path))
                {
                    using (var stream = File.OpenRead(path))
                    {
                        byte[] bt = new byte[stream.Length];
                        stream.Read(bt, 0, bt.Length);
                        return bt;
                    }
                }
                return null;
            }); 
        }

        /// <summary>
        /// 保存图片加载失败时候的底图
        /// </summary>
        /// <param name="stream"></param>
        public void Save404Stream(Stream stream)
        {
            lock (lockObj)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    using (var fs = File.OpenWrite(Path.Combine(_env.ContentRootPath, FILE_NAME_404)))
                    {
                        stream.CopyTo(ms);
                        ms.WriteTo(fs);
                        fs.Flush();
                        fs.Close();
                        _cacheManager.Remove(MARK_LOGO);
                    }
                }
            }
        }
    }
}
