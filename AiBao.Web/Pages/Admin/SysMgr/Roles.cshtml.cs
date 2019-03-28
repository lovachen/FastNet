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

    public class RolesModel : AdminPrmPageModel
    {
        SysRoleService _sysRoleService;

        public RolesModel(SysRoleService sysRoleService)
        {
            _sysRoleService = sysRoleService;
        }

        public List<Sys_RoleMapping> Sys_Roles { get; set; } 


        public void OnGet()
        {
            Sys_Roles = _sysRoleService.GetRoles();
        }



    }
}