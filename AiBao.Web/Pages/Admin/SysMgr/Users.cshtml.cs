using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AiBao.Services;
using cts.web.core.Librs;
using AiBao.Framework.Pages;
using AiBao.Mapping;
using Microsoft.AspNetCore.Http;

namespace AiBao.Web.Pages.Admin.SysMgr
{
    /// <summary>
    /// 系统用户列表
    /// </summary>
    public class UsersModel : AdminPrmPageModel
    {
        SysUserService _sysUserService;


        public UsersModel(SysUserService sysUserService)
        {
            _sysUserService = sysUserService;
        }

        [BindProperty(SupportsGet = true)]
        public SysUserSearchArg Arg { get; set; }


        public void OnGet()
        {

        }

        public IActionResult OnGetData()
        {
            var parms = Request.QueryString.ToTableParms();
            var pageList = _sysUserService.AdminSearch(Arg, parms);
            var data = pageList.ToAjax();
            return Json(data);
        }




    }
}