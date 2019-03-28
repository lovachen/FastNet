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
using Microsoft.AspNetCore.Http;

namespace AiBao.Web.Pages.Admin.SysMgr
{
    public class NLogModel : AdminPrmPageModel
    {
        SysNLogService _sysNLogService;

        public NLogModel(SysNLogService sysNLogService)
        {
            _sysNLogService = sysNLogService;
        }

        [BindProperty]
        public NLogSearchArg Arg { get; set; }

        public void OnGet()
        {

        }

        public IActionResult OnGetData()
        {
            var parameters = Request.QueryString.ToTableParms();
            var pageList = _sysNLogService.AdminSearch(Arg, parameters);
            return Json(pageList.ToAjax());
        }


    }
}