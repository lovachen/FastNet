using cts.web.core.Menu;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AiBao.Services
{
    public class InstallService 
    {
        private IHostingEnvironment _environment;
        private SysCategoryService _sysCategoryService;
        private SettingService _settingService;

        public InstallService(IHostingEnvironment environment,
            SysCategoryService sysCategoryService,
            SettingService settingService)
        {
            _sysCategoryService = sysCategoryService;
            _environment = environment;
            _settingService = settingService;
        }


        /// <summary>
        /// 接口初始化
        /// </summary>
        public void Install()
        {
            var xmlSiteMap = new XmlSiteMap();
            xmlSiteMap.LoadFrom(Path.Combine(_environment.ContentRootPath, "sitemap.xml"));
            List<Entities.Sys_Category> sysApis = new List<Entities.Sys_Category>();
            xmlSiteMap.SiteMapNodes.ForEach(item =>
            {
                sysApis.Add(new Entities.Sys_Category()
                {
                    Id=CombGuid.NewGuid(),
                    Name = item.Name,
                    RouteTemplate = item.RouteTemplate ?? "",
                    Code = item.Code,
                    FatherCode = item.FatherCode,
                    UID = item.UID,
                    Target = item.Target ?? "",
                    IsMenu = item.IsMenu ?? "",
                    Sort = item.Sort,
                    Action = item.Action ?? "",
                    Controller = item.Controller ?? "",
                    IconClass = item.IconClass ?? "",
                    RouteName = item.RouteName ?? ""
                });
            });
            _sysCategoryService.Init(sysApis);
            _settingService.Init();
        }


    }
}
