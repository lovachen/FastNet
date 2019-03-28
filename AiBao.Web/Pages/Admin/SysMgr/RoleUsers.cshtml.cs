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
using AutoMapper;

namespace AiBao.Web.Pages.Admin.SysMgr
{
    /// <summary>
    /// 查看角色详情
    /// </summary>
    public class RoleUsersModel : AdminPrmPageModel
    {

        SysRoleService _sysRoleService;
        SysUserService _sysUserService;

        public RoleUsersModel(SysRoleService sysRoleService,
            SysUserService sysUserService)
        {
            _sysUserService = sysUserService;
            _sysRoleService = sysRoleService;
        }


        public Sys_RoleMapping Role { get; set; }
        public List<Sys_UserMapping> Sys_Users { get; set; }

        public void OnGet(Guid id)
        {
            Role = _sysRoleService.GetRoleMapping(id);
            Sys_Users = _sysUserService.GetRoleUsers(id);
        }
    }
}