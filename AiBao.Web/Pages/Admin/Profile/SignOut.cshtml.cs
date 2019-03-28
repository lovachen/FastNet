using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AiBao.Framework.Pages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AiBao.Web.Pages.Admin.Profile
{
    /// <summary>
    /// 退出系统
    /// </summary>
    public class SignOutModel : AdminPageModel
    {


        public IActionResult OnGet()
        {
            WorkContext.SignOut(0);
            return RedirectToPage("/Admin/Login");
        }
    }
}