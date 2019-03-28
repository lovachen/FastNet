using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AiBao.Framework.Pages;
using AiBao.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AiBao.Web.Pages.Admin.SysMgr
{
    public class EditSettingModel : AdminPrmPageModel
    {

        SettingService _settingService;

        public EditSettingModel(SettingService settingService)
        {
            _settingService = settingService;
        }


        public IActionResult OnGet(string pk, string value)
        {
            _settingService.SaveSetting(pk, value);
            AjaxData.Code = 0;
            AjaxData.Message = "保存成功";
            return Json(AjaxData);
        }
    }
}