using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AiBao.Framework.Pages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AiBao.Services;
using AiBao.Mapping;
using Microsoft.AspNetCore.Http;

namespace AiBao.Web.Pages.Admin.Oss
{

    public class BucketImageModel : AdminPrmPageModel
    {
        BucketImageService _bucketImageService;

        public BucketImageModel(BucketImageService bucketImageService)
        {
            _bucketImageService = bucketImageService;
        }

        [BindProperty]
        public BucketImageSearchArg Arg { get; set; }

        public void OnGet()
        {

        }

        public IActionResult OnGetData()
        {
            var parms = Request.QueryString.ToTableParms();
            var pageList = _bucketImageService.AdminSearch(Arg, parms);
            
            return Json(pageList.ToAjax());
        }

    }
}