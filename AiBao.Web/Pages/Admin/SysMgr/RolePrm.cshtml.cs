using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AiBao.Framework.Pages;
using AiBao.Mapping;
using AiBao.Services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AiBao.Web.Pages.Admin.SysMgr
{
    /// <summary>
    /// 角色权限设置
    /// </summary>
    public class RolePrmModel : AdminPrmPageModel
    {
        SysRoleService _sysRoleService;
        SysCategoryService _sysCategoryService;
        IMapper _mapper;

        public RolePrmModel(SysRoleService sysRoleService,
            IMapper mapper,
            SysCategoryService sysCategoryService)
        {
            _sysCategoryService = sysCategoryService;
            _mapper = mapper;
            _sysRoleService = sysRoleService;
        }

        /// <summary>
        /// 
        /// </summary>
        public List<Sys_CategoryMapping> Sys_Categories { get; set; }
        public Sys_RoleMapping Role { get; set; }
        public List<Sys_PermissionMapping> Permissions { get; set; }

        [BindProperty]
        public List<Guid> CategoryIds { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public void OnGet(Guid id)
        {
            Role = _sysRoleService.GetRoleMapping(id);
            var categories = _sysCategoryService.GetAllCache();
            Sys_Categories = categories.Where(o => o.Target == "0").ToList();
            Permissions = _sysRoleService.GetRolePermissons().Where(o=>o.RoleId==id).ToList();
        }


        public IActionResult OnPost(Guid Id)
        {
            var res = _sysRoleService.SetRolePermission(Id, CategoryIds, UserId);
            AjaxData.Message = res.Message;
            AjaxData.Code = res.Status ? 0 : 2001;
            return Json(AjaxData);
        }





    }
}