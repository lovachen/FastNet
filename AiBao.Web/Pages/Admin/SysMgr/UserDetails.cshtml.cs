using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AiBao.Framework.Pages;
using AiBao.Mapping;
using AiBao.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AiBao.Web.Pages.Admin.SysMgr
{
    /// <summary>
    /// 
    /// </summary>
    public class UserDetailsModel : AdminPrmPageModel
    {

        SysUserService _sysUserService;
        SysRoleService _sysRoleService;
        SysUserLoginService _sysUserLoginService;
        ActivityLogService _activityLogService;
        SysUserJwtService _sysUserJwtService;

        public UserDetailsModel(SysRoleService sysRoleService,
            SysUserService sysUserService,
            SysUserJwtService sysUserJwtService,
            SysUserLoginService sysUserLoginService,
            ActivityLogService activityLogService)
        {
            _activityLogService = activityLogService;
            _sysUserLoginService = sysUserLoginService;
            _sysUserService = sysUserService;
            _sysRoleService = sysRoleService;
            _sysUserJwtService = sysUserJwtService;
        }
        public Sys_UserMapping SysUser { get; set; }
        public List<Sys_RoleMapping> Roles { get; set; }

        public void OnGet(Guid id)
        {
            SysUser = _sysUserService.GetUserMapping(id);
            Roles = _sysRoleService.GetUserRoles(id);
        }


        /// <summary>
        /// 获取最新的登陆记录
        /// </summary>
        /// <param name="id"></param>
        public IActionResult OnGetLoginLog(Guid id, int draw)
        {
            var list = _sysUserLoginService.GetLastUserLogins(id);
            DatatableModel<Sys_UserLoginMapping> data = new DatatableModel<Sys_UserLoginMapping>(list);
            data.draw = draw;
            return Json(data);
        }

        /// <summary>
        /// 获取操作的最新记录
        /// </summary>
        /// <param name="id"></param>
        public IActionResult OnGetActivityLog(Guid id, int draw)
        {
            var list = _activityLogService.GetLastUserActivityLogs(id);
            DatatableModel<Sys_ActivityLogMapping> data = new DatatableModel<Sys_ActivityLogMapping>(list);
            data.draw = draw;
            return Json(data);
        }


        /// <summary>
        /// 获取登陆的JwtToken
        /// </summary>
        /// <param name="id"></param>
        public IActionResult OnGetJwtToken(Guid id, int draw)
        {
            var list = _sysUserJwtService.GetLastUserJwt(id);
            DatatableModel<Sys_UserJwtMapping> data = new DatatableModel<Sys_UserJwtMapping>(list);
            data.draw = draw;
            return Json(data);
        }








    }
}