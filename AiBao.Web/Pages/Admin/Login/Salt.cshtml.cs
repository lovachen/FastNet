using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AiBao.Framework.Pages;
using AiBao.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AiBao.Web.Pages.Admin.Login
{
    public class SaltModel : BasePageModel
    {
        SysUserService _sysUserService;

        public SaltModel(SysUserService sysUserService)
        {
            _sysUserService = sysUserService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public IActionResult OnGet(string account)
        {
            var item = _sysUserService.GetSalt(account);
            AjaxData.Code = 0;
            AjaxData.Result = new { Salt = item.Salt ?? "", R = item.R ?? "" };
            AjaxData.Message = "获取成功";
            return Json(AjaxData);
        }
    }
}