using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AiBao.Framework.Pages;
using AiBao.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AiBao.Mapping;

namespace AiBao.Web.Pages.Admin.SysMgr
{
    public class NLogDtlModel : AdminPrmPageModel
    {
        SysNLogService _sysNLogService;

        public NLogDtlModel(SysNLogService sysNLogService)
        {
            _sysNLogService = sysNLogService;
        }

        public Sys_NLogMapping SysNLog { get; set; }

        public void OnGet(Guid id)
        {
            SysNLog = _sysNLogService.GetNlog(id);
        }
    }
}