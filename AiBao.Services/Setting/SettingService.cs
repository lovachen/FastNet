using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Caching.Memory;
using System.Linq;
using AspNetCore.Cache;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using AiBao.Entities;
using cts.web.core;
using AiBao.Mapping;
using Newtonsoft.Json;
using System.IO;

namespace AiBao.Services
{
    public class SettingService : BaseService
    {
        private const string MODEL_KEY = "ab.services.settings";

        private ICacheManager _cacheManager;
        private ABDbContext _dbContext;

        public SettingService(ICacheManager cacheManager,
            ABDbContext dbContext)
        {
            _dbContext = dbContext;
            _cacheManager = cacheManager;
        }

        /// <summary>
        /// 获取
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> GetSiteSettings()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            var siteList = _dbContext.Sys_Setting.ToList();
            siteList.ForEach(item =>
            {
                dic.TryAdd(item.Name, String.IsNullOrEmpty(item.Value) ? "" : item.Value.Trim());
            });
            return dic;
        }

        /// <summary>
        /// 保存基数设置的值
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public (bool Status, string Message) SaveSetting(string name, string value)
        {
            var item = _dbContext.Sys_Setting.FirstOrDefault(o => o.Name == name);
            if (item == null)
            {
                _dbContext.Sys_Setting.Add(new Entities.Sys_Setting()
                {
                    Id = Guid.NewGuid(),
                    Name = name,
                    Value = value
                });
            }
            else
            {
                item.Value = value;
            }
            _dbContext.SaveChanges();
            _cacheManager.Remove(MODEL_KEY);
            return Success("保存成功");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public SiteSettings GetMasterSettings()
        {
            return _cacheManager.Get<SiteSettings>(MODEL_KEY, () =>
            {
                var jsonStr = JsonConvert.SerializeObject(GetSiteSettings());
                return JsonConvert.DeserializeObject<SiteSettings>(jsonStr);
            });
        }
 
        /// <summary>
        /// 初始化配置参数
        /// </summary>
        public void Init()
        {
            SiteSettings settings = new SiteSettings();
            List<Entities.Sys_Setting> listSettings = new List<Sys_Setting>();
            var propertyInfos = settings.GetType().GetProperties();
            if (propertyInfos.Length > 0)
            {
                foreach (PropertyInfo info in propertyInfos)
                {
                    if (info.CanWrite)
                    {
                        listSettings.Add(new Sys_Setting()
                        {
                            Name = info.Name,
                            Value = info.GetValue(settings)?.ToString() ?? ""
                        });
                    }
                }
            }
            if (listSettings.Any())
            {
                System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
                stopwatch.Start();
                var list = _dbContext.Sys_Setting.ToList();
                stopwatch.Stop();
                foreach (var item in list)
                {
                    if (!listSettings.Any(o => o.Name == item.Name))
                    {
                        _dbContext.Sys_Setting.Remove(item);
                    }
                }
                listSettings.ForEach(item =>
                {
                    if (!list.Any(o => o.Name == item.Name))
                    {
                        item.Id = CombGuid.NewGuid();
                        _dbContext.Sys_Setting.Add(item);
                    }
                });
                _dbContext.SaveChanges();
            }
            else
            {
                _dbContext.Database.ExecuteSqlCommand("DELETE FROM [Sys_Setting]");
            }
        }
    }
}
