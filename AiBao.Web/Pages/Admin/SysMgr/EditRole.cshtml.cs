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
    public class EditRoleModel : AdminPrmPageModel
    {

        SysRoleService _sysRoleService;
        IMapper _mapper;

        public EditRoleModel(SysRoleService sysRoleService,
            IMapper mapper)
        {
            _mapper = mapper;
            _sysRoleService = sysRoleService;
        }

        /// <summary>
        /// 
        /// </summary>
        [BindProperty]
        public Sys_RoleMapping Role { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        public void OnGet(Guid? id = null)
        {
            if (id.HasValue)
                Role = _sysRoleService.GetRoleMapping(id.Value);
        }


        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
                return NotValid();
            (bool Status, string Message) res;
            if (Role.Id != Guid.Empty)
            {
                res = _sysRoleService.UpdateRole(Role, UserId);
            }
            else
            {
                var item = _mapper.Map<Entities.Sys_Role>(Role);
                item.Id = CombGuid.NewGuid();
                item.CreationTime = DateTime.Now;
                item.Creator = UserId;
                res = _sysRoleService.AddRole(item);
            }
            AjaxData.Message = res.Message;
            AjaxData.Code = res.Status ? 0 : 2001;
            return Json(AjaxData);
        }
         








    }
}