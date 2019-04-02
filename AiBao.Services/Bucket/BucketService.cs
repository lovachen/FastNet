using AiBao.Entities;
using AiBao.Mapping;
using AspNetCore.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace AiBao.Services
{
    public class BucketService
    {
        private const string MODEL_KEY = "ab.services.bucket.all";

        private ICacheManager _cacheManager;
        private ABDbContext _dbContext;
        IMapper _mapper;

        public BucketService(ICacheManager cacheManager,
            ABDbContext dbContext,
            IMapper mapper)
        {
            _mapper = mapper;
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
        /// 
        /// </summary>
        /// <param name="bucket"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public (bool Status, string Message) UpdateBucket(BucketMapping bucket, Guid userId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public (bool Status, string Message) AddBucket(Bucket item)
        {
            throw new NotImplementedException();
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


        /// <summary>
        /// 获取详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public BucketMapping GetBucketMapping(Guid id)
        {
            var item = _dbContext.Bucket.AsNoTracking().FirstOrDefault(o => o.Id == id);
            if (item == null) return null;
            return _mapper.Map<BucketMapping>(item);
        }






    }
}
