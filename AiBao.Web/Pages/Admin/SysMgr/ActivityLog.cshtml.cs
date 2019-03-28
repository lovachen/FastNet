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
    /// <summary>
    /// 
    /// </summary>
    public class ActivityLogModel : AdminPrmPageModel
    {
        ActivityLogService _activityLogService;

        public ActivityLogModel(ActivityLogService activityLogService)
        {
            _activityLogService = activityLogService;
        }

        [BindProperty(SupportsGet =true)]
        public ActivityLogSearchArg Arg { get; set; }

        public void OnGet()
        {

        }

        public IActionResult OnGetData()
        {
            var parameters = Request.QueryString.ToTableParms();

            var pageList = _activityLogService.AdminSearch(Arg, parameters);
            var data = pageList.ToAjax();
            return Json(data);
        }


    }
}