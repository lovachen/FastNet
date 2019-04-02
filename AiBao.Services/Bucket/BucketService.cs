using AiBao.Entities;
using AiBao.Mapping;
using AspNetCore.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AiBao.Services
{
    public class BucketService
    {
        private const string MODEL_KEY = "ab.services.bucket.all";

        private ICacheManager _cacheManager;
        private ABDbContext _dbContext;

        public BucketService(ICacheManager cacheManager,
            ABDbContext dbContext)
        {
            _dbContext = dbContext;
            _cacheManager = cacheManager;
        }

        /// <summary>
        /// 获取所有的bucket并缓存
        /// </summary>
        /// <returns></returns>
        public List<BucketMapping> AllBuckets()
        {
            return _cacheManager.Get<List<BucketMapping>>(MODEL_KEY, () =>
            {
                return _dbContext.Bucket.Select(item => new BucketMapping()
                {
                    Id = item.Id,
                    Name = item.Name,
                    Description = item.Description,
                    CreationTime = item.CreationTime
                }).ToList();
            });
        }

        /// <summary>
        /// 从缓存中通过名称获取 忽略大小写
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public BucketMapping GetBucketBayName(string name)
        {
            var list = AllBuckets();
            return list.FirstOrDefault(o => o.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
        }









    }
}
