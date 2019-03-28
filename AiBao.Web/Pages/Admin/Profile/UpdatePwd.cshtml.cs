using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AiBao.Framework.Pages;
using AiBao.Services;
using AiBao.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AiBao.Web.Pages.Admin.Profile
{
    public class UpdatePwdModel : AdminPageModel
    {

        SysUserService _sysUserService;

        public UpdatePwdModel(SysUserService sysUserService)
        {
            _sysUserService = sysUserService;
        }

        [BindProperty]
        public ChangePwdModel PwdModel { get; set; }


        /// <summary>
        /// 修改密码
        /// </summary>
        /// <returns></returns>
        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return NotValid();
            }
            var res = _sysUserService.UpdatePwd(UserId, PwdModel.OldPwd, PwdModel.NewPwd, UserId);
            AjaxData.Code = res.Status? 0:2001; 
            AjaxData.Message = res.Message;
            return Json(AjaxData);
        }

    }
}