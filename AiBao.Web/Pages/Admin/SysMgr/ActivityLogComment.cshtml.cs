using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AiBao.Framework.Pages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AiBao.Services;
using AiBao.Mapping;
using AiBao.Entities;

namespace AiBao.Web.Pages.Admin.SysMgr
{
    public class ActivityLogCommentModel : AdminPrmPageModel
    {
        SysActivityLogCommentService _sysActivityLogCommentService;

        public ActivityLogCommentModel(SysActivityLogCommentService sysActivityLogCommentService)
        {
            _sysActivityLogCommentService = sysActivityLogCommentService;
        }

        /// <summary>
        /// 
        /// </summary>
        public List<Sys_ActivityLogCommentMapping> ActivityLogComments { get; set; }


        public void OnGet()
        {
            ActivityLogComments = _sysActivityLogCommentService.GetActivityLogComments();
        }



    }
}