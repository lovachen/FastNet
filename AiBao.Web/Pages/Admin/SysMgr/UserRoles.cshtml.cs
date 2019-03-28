using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AiBao.Framework.Pages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AiBao.Mapping;
using AiBao.Services;

namespace AiBao.Web.Pages.Admin.SysMgr
{
    public class UserRolesModel : AdminPrmPageModel
    {

        SysUserService _sysUserService;
        SysRoleService _sysRoleService;


        public UserRolesModel(SysRoleService sysRoleService,
            SysUserService sysUserService)
        {
            _sysUserService = sysUserService;
            _sysRoleService = sysRoleService;
        }

        public Sys_UserMapping SysUser { get; set; }
        public List<Sys_RoleMapping> Roles { get; set; }
        public List<Sys_UserRoleMapping> UserRoles { get; set; }
        [BindProperty]
        public List<Guid> RoleIds { get; set; }

        public void OnGet(Guid id)
        {
            SysUser = _sysUserService.GetUserMapping(id);
            var userRoles = _sysRoleService.GetUserRoles();
            UserRoles = userRoles.Where(o => o.UserId == id).Distinct().ToList();
            Roles = _sysRoleService.GetRoles();
        }

        public IActionResult OnPost(Guid id)
        {
            _sysRoleService.SetUserRoles(id, RoleIds, UserId);
            AjaxData.Code = 0;
            AjaxData.Message = "保存成功";
            return Json(AjaxData);
        }


    }
}