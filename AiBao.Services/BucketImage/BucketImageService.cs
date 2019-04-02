using AiBao.Entities;
using AiBao.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AiBao.Services
{
    public class BucketImageService
    {
        private readonly static Object lockObj = new object();
        private ABDbContext _dbContext;

        public BucketImageService(ABDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// 通过sha1获取
        /// </summary>
        /// <param name="sha1"></param>
        /// <returns></returns>
        public BucketImageMapping GetSHA1(string sha1)
        {
            return _dbContext.BucketImage.Select(item => new
            BucketImageMapping()
            {
                SHA1 = sha1,
                VisitUrl = item.VisitUrl,
                IOPath = item.IOPath,
            }).FirstOrDefault(o => o.SHA1 == sha1);
        }


        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="bucketImage"></param>
        public void AddImage(Entities.BucketImage bucketImage)
        {
            lock (lockObj)
            {
                if (!_dbContext.BucketImage.Any(o => o.SHA1 == bucketImage.SHA1))
                {
                    _dbContext.BucketImage.Add(bucketImage);
                    _dbContext.SaveChanges();
                }
            }
        }










    }
}
