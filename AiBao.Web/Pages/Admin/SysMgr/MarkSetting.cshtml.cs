using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AiBao.Framework.Pages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AiBao.Services;
using Microsoft.AspNetCore.Http;
using AiBao.Services.Setting;

namespace AiBao.Web.Pages.Admin.SysMgr
{
    public class MarkSettingModel : AdminPrmPageModel
    {
        MarkLogoService _markLogoService;

        public MarkSettingModel(MarkLogoService markLogoService)
        {
            _markLogoService = markLogoService;
        }




        public IActionResult OnPost(IFormFile file)
        {
            if (file == null || !file.IsImage())
            {
                AjaxData.Message = "请选择图片文件";
                return Json(AjaxData);
            }
            using (var stream = file.OpenReadStream())
            {
                _markLogoService.SaveStream(stream);
                AjaxData.Message = "保存成功";
                AjaxData.Code = 0;
                return Json(AjaxData);
            }
        }

        /// <summary>
        /// 保存图片不能在时显示的底图
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public IActionResult OnPostBaseMap(IFormFile file)
        {
            if (file == null || !file.IsImage())
            {
                AjaxData.Message = "请选择图片文件";
                return Json(AjaxData);
            }
            using (var stream = file.OpenReadStream())
            {
                _markLogoService.Save404Stream(stream);
                AjaxData.Message = "保存成功";
                AjaxData.Code = 0;
                return Json(AjaxData);
            }
        }




    }
}