using AiBao.Entities;
using AiBao.Mapping;
using AspNetCore.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Newtonsoft.Json;
using cts.web.core;

namespace AiBao.Services
{
    public class BucketService : BaseService
    {
        private const string MODEL_KEY = "ab.services.bucket.all";
        private readonly static Object lockObj = new object();

        private ICacheManager _cacheManager;
        private ABDbContext _dbContext;
        IMapper _mapper;
        ActivityLogService _activityLogService;

        public BucketService(ICacheManager cacheManager,
            ABDbContext dbContext,
            ActivityLogService activityLogService,
            IMapper mapper)
        {
            _activityLogService = activityLogService;
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
                    CreationTime = item.CreationTime,
                    IsCompress = item.IsCompress
                }).ToList();
            });
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="bucket"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public (bool Status, string Message) UpdateBucket(BucketMapping bucket, Guid userId)
        {
            var item = _dbContext.Bucket.Find(bucket.Id);
            if (item == null) return Fail("数据不存在");
            string oldLog = JsonConvert.SerializeObject(item);
            item.Description = bucket.Description;
            item.IsCompress = bucket.IsCompress;
            string newLog = JsonConvert.SerializeObject(item);
            _dbContext.SaveChanges();
            _activityLogService.UpdatedEntity<Entities.Bucket>(item.Id, oldLog, newLog, userId);
            _cacheManager.Remove(MODEL_KEY);
            return Success("修改成功");
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public (bool Status, string Message) AddBucket(Bucket item)
        {
            lock (lockObj)
            {
                if (!_dbContext.Bucket.Any(o => o.Name == item.Name))
                {
                    string newLog = JsonConvert.SerializeObject(item);
                    _dbContext.Bucket.Add(item);
                    _dbContext.SaveChanges();
                    _activityLogService.InsertedEntity<Entities.Bucket>(item.Id, null, newLog, item.Creator);
                    _cacheManager.Remove(MODEL_KEY);
                    return Success("添加成功");
                }
                return Fail("名称已存在");
            }
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
        /// 删除，已经使用的无法删除
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public (bool Status, String Message) Delete(Guid id, Guid userId)
        {
            if (_dbContext.BucketImage.Any(o => o.BucketId == id))
                return Fail("已使用，不能删除");
            _dbContext.Database.ExecuteSqlCommand($"DELETE FROM [Bucket] WHERE [Id]={id}");
            _cacheManager.Remove(MODEL_KEY);
            return Success("删除成功");
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
