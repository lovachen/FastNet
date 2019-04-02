using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AiBao.Framework.Pages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AiBao.Services;
using AiBao.Mapping;

namespace AiBao.Web.Pages.Admin.Oss
{

    public class BucketModel : AdminPrmPageModel
    {
        BucketService _bucketService;

        public BucketModel(BucketService bucketService)
        {
            _bucketService = bucketService;
        }

        public List<BucketMapping> Buckets { get; set; }

        public void OnGet()
        {
            Buckets = _bucketService.AllBuckets();
        }
    }
}