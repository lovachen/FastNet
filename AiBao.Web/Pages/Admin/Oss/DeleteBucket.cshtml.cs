using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AiBao.Framework.Pages;
using AiBao.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AiBao.Web.Pages.Admin.Oss
{

    /// <summary>
    /// 删除
    /// </summary>
    public class DeleteBucketModel : AdminPrmPageModel
    {
        BucketService _bucketService;

        public DeleteBucketModel(BucketService bucketService)
        {
            _bucketService = bucketService;
        }


        public IActionResult OnGet(Guid id)
        {
           var res = _bucketService.Delete(id, UserId);
            AjaxData.Code = res.Status ? 0 : 2001;
            AjaxData.Message = res.Message;
            return Json(AjaxData);
        }
    }
}