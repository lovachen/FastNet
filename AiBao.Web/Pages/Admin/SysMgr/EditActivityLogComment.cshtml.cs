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

    public class EditActivityLogCommentModel : AdminPrmPageModel
    {

        SysActivityLogCommentService _sysActivityLogCommentService;

        public EditActivityLogCommentModel(SysActivityLogCommentService sysActivityLogCommentService)
        {
            _sysActivityLogCommentService = sysActivityLogCommentService;
        }




        public IActionResult OnGet(string pk,string value)
        {
            _sysActivityLogCommentService.Update(pk, value);
            AjaxData.Code = 0;
            AjaxData.Message = "保存成功";
            return Json(AjaxData);
        }
    }
}