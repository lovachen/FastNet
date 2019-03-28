using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AiBao.Framework.Pages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AiBao.Mapping;
using AiBao.Services;
using AiBao.Framework;


namespace AiBao.Web.Pages.Admin.SysMgr
{
    public class ActivityLogCommentInitModel : AdminPrmPageModel
    {
        SysActivityLogCommentService _sysActivityLogCommentService;

        public ActivityLogCommentInitModel(SysActivityLogCommentService sysActivityLogCommentService)
        {
            _sysActivityLogCommentService = sysActivityLogCommentService;
        }

        public IActionResult OnGet()
        {
            _sysActivityLogCommentService.Initialize();
            AjaxData.Code = 0;
            AjaxData.Message = "初始化成功";
            return Json(AjaxData);
        }
    }
}