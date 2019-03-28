using AiBao.Entities;
using AiBao.Mapping;
using AspNetCore.Cache;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AiBao.Services
{
    public class SysCategoryService
    {
        const string MODEL_ALL = "ab.sys.category.all";

        private ABDbContext _dbContext;
        private ICacheManager _cacheManager;
        private IMapper _mapper;

        public SysCategoryService(ICacheManager cacheManager,
            ABDbContext dbContext,
            IMapper mapper)
        {
            _mapper = mapper;
            _dbContext = dbContext;
            _cacheManager = cacheManager;
        }

        /// <summary>
        /// 获取全部并缓存
        /// </summary>
        /// <returns></returns>
        public List<Sys_CategoryMapping> GetAllCache()
        {
            return _cacheManager.Get<List<Sys_CategoryMapping>>(MODEL_ALL, () =>
            {
                return _dbContext.Sys_Category.ToList()
                .Select(item => _mapper.Map<Sys_CategoryMapping>(item)).ToList();
            });
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="sysCategories"></param>
        public void Init(List<Entities.Sys_Category> sysCategories)
        {
            using (var tarns = _dbContext.Database.BeginTransaction())
            { 
                var oldList = _dbContext.Sys_Category.ToList();
                oldList.ForEach(del =>
                {
                    var item = sysCategories.FirstOrDefault(o => o.UID == del.UID);
                    if (item == null)
                    {
                        _dbContext.Database.ExecuteSqlCommand($"DELETE FROM [Sys_Permission] WHERE [CategoryId]={del.Id}");
                        _dbContext.Sys_Category.Remove(del);
                    }
                });
                sysCategories.ForEach(entity =>
                {
                    var item = oldList.FirstOrDefault(o => o.UID == entity.UID);
                    if (item == null)
                    {
                        _dbContext.Sys_Category.Add(entity);
                    }
                    else
                    {
                        item.RouteTemplate = entity.RouteTemplate ?? "";
                        item.Name = entity.Name;
                        item.Code = entity.Code;
                        item.FatherCode = entity.FatherCode;
                        item.Target = entity.Target ?? "0";
                        item.Sort = entity.Sort;
                        item.IsMenu = entity.IsMenu ?? "0";
                        item.Controller = entity.Controller ?? "";
                        item.Action = entity.Action ?? "";
                        item.RouteName = entity.RouteName ?? "";
                        item.IconClass = entity.IconClass ?? "";
                    }
                });
                _dbContext.SaveChanges();
                tarns.Commit();
            }
        }
    }
}
