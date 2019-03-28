using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AiBao.Framework.Pages;
using AiBao.Mapping;
using AiBao.Services;
using AiBao.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AiBao.Web.Pages.Admin.Profile
{

    public class IndexModel : AdminPageModel
    {
        SysUserService _sysUserService;

        public IndexModel(SysUserService sysUserService)
        {
            _sysUserService = sysUserService;
        }


        /// <summary>
        /// 
        /// </summary>
        [BindProperty]
        public Sys_UserMapping CurrentUser { get; set; }
        public ChangePwdModel PwdModel { get; set; }




        public void OnGet()
        {
            CurrentUser = WorkContext.GetUser();
        }



        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IActionResult OnPostUpdate()
        {
            if (!ModelState.IsValid)
            {
                return NotValid();
            }
            CurrentUser.Id = UserId;
            _sysUserService.UpdateUser(CurrentUser, UserId);
            AjaxData.Code = 0;
            AjaxData.Message = "修改成功";
            return Json(AjaxData);
        }




    }
}