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

    public class DeleteRoleModel : AdminPrmPageModel
    {

        SysRoleService _sysRoleService;

        public DeleteRoleModel(SysRoleService sysRoleService)
        {
            _sysRoleService = sysRoleService;
        }


        public IActionResult OnGet(Guid id)
        {
            var res = _sysRoleService.DeleteRole(id, UserId);
            AjaxData.Message = res.Message;
            AjaxData.Code = res.Status ? 0 : 2001;
            return Json(AjaxData);
        }
    }
}