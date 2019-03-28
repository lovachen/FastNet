using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AiBao.Framework.Pages;
using AiBao.Services;
using AiBao.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using cts.web.core.Jwt;
using AiBao.Framework.Security;

namespace AiBao.Web.Pages.Admin
{
    public class LoginModel : BasePageModel
    {
        SysUserService _sysUserService;
        SysUserAuthentication _sysUserAuthentication;

        public LoginModel(SysUserService sysUserService,
            SysUserAuthentication sysUserAuthentication)
        {
            _sysUserAuthentication = sysUserAuthentication;
            _sysUserService = sysUserService;
        }


        [BindProperty]
        public ViewLoginModel Login { get; set; }


        public void OnGet()
        {

        }


        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return NotValid();
            }
            var res = _sysUserService.ValidateUser(Login.Account, Login.Password, 0);
            AjaxData.Message = res.Message;
            AjaxData.Code = res.Status ? 0 : 2001;
            if (res.Status)
            {
                _sysUserAuthentication.SignIn(res.Jwt.Jti, res.User, res.Jwt.Expiration);
                AjaxData.Result = new { Url = "/admin/home" };
            }
            return Json(AjaxData);
        }

    }
}