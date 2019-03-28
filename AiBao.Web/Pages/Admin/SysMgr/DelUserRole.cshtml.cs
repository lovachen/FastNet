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
    public class DelUserRoleModel : AdminPrmPageModel
    {
        SysRoleService _sysRoleService;

        public DelUserRoleModel(SysRoleService sysRoleService)
        {
            _sysRoleService = sysRoleService;
        }


        public IActionResult OnGet(Guid id,Guid roleId)
        {
            _sysRoleService.DeleteUserRole(id, roleId, UserId);
            AjaxData.Code = 0;
            AjaxData.Message = "删除成功";
            return Json(AjaxData);
        }
    }
}