using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AiBao.Framework.Pages;
using AiBao.Mapping;
using AiBao.Services;
using AutoMapper;
using cts.web.core.Librs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AiBao.Web.Pages.Admin.SysMgr
{
    public class EditUserModel : AdminPrmPageModel
    {
        SysUserService _sysUserService;
        SysRoleService _sysRoleService;
        IMapper _mapper;


        public EditUserModel(SysRoleService sysRoleService, 
            IMapper mapper,
            SysUserService sysUserService)
        {
            _sysUserService = sysUserService;
            _sysRoleService = sysRoleService;
            _mapper = mapper;
        }

        [BindProperty]
        public Sys_UserMapping SysUser { get; set; }
        public List<Sys_RoleMapping> Roles { get; set; }
        public List<Sys_UserRoleMapping> UserRoles { get; set; }
        [BindProperty]
        public List<Guid> RoleIds { get; set; }

        public void OnGet(Guid? id = null)
        {
            if (id.HasValue)
            {
                SysUser = _sysUserService.GetUserMapping(id.Value);
                var userRoles = _sysRoleService.GetUserRoles();
                UserRoles = userRoles.Where(o => o.UserId == id.Value).Distinct().ToList();
            }
            Roles = _sysRoleService.GetRoles();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
                return NotValid();
            (bool Status, string Message) res;
            var item = _mapper.Map<Entities.Sys_User>(SysUser);

            if (SysUser.Id != Guid.Empty)
            {
                res = _sysUserService.UpdateUser(SysUser, UserId);
            }
            else
            {
                item.Account = item.Account.TrimSpace();
                item.Id = CombGuid.NewGuid();
                item.CreationTime = DateTime.Now;
                item.Creator = UserId;
                item.Salt = EncryptorHelper.CreateSaltKey();
                item.Password = (EncryptorHelper.GetMD5(item.Account + item.Salt));
                res = _sysUserService.AddUser(item);
            } 
            AjaxData.Message = res.Message;
            AjaxData.Code = res.Status ? 0 : 2001;
            if (res.Status)
            {
                _sysRoleService.SetUserRoles(item.Id, RoleIds, UserId);
            }
            return Json(AjaxData);
        }


    }
}