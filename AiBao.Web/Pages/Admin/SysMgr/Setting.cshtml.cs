using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AiBao.Framework.Pages;
using AiBao.Services;
using AiBao.Mapping;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;


namespace AiBao.Web.Pages.Admin.SysMgr
{
    public class SettingModel : AdminPrmPageModel
    {

        SettingService _settingService;

        public SettingModel(SettingService settingService)
        {
            _settingService = settingService;
        }

        public SiteSettings SiteSettings { get; set; }

        public void OnGet()
        {
           SiteSettings = _settingService.GetMasterSettings();
        }



    }
}