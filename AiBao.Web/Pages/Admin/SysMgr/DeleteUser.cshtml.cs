using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AiBao.Framework.Pages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AiBao.Services;

namespace AiBao.Web.Pages.Admin.SysMgr
{
    /// <summary>
    /// 
    /// </summary>
    public class DeleteUserModel : AdminPrmPageModel
    {
        SysUserService _sysUserService;
        SysUserJwtService _sysUserJwtService;
        SysRoleService _sysRoleService;

        public DeleteUserModel(SysUserService sysUserService,
            SysUserJwtService sysUserJwtService,
            SysRoleService sysRoleService)
        {
            _sysUserJwtService = sysUserJwtService;
            _sysUserService = sysUserService;
            _sysRoleService = sysRoleService;
        }


        public IActionResult OnGet(Guid id)
        {
            _sysUserService.Delete(id, UserId);
            try
            {
                _sysUserJwtService.CompelOut(id);
                _sysRoleService.SetUserRoles(id, null, UserId);
            }
            catch (Exception)
            {

            }
            AjaxData.Code = 0;
            AjaxData.Message = "删除成功";
            return Json(AjaxData);
        }
    }
}