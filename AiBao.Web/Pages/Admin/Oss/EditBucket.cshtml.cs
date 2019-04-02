using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AiBao.Framework.Pages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AiBao.Mapping;
using AiBao.Services;
using AutoMapper;

namespace AiBao.Web.Pages.Admin.Oss
{
    public class EditBucketModel : AdminPrmPageModel
    {
        BucketService _bucketService;
        IMapper _mapper;

        public EditBucketModel(BucketService bucketService,
            IMapper mapper)
        {
            _bucketService = bucketService;
            _mapper = mapper;
        }

        [BindProperty]
        public BucketMapping Bucket { get; set; }


        public void OnGet(Guid? id)
        {
            if (id.HasValue)
                Bucket = _bucketService.GetBucketMapping(id.Value);
        }


        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
                return NotValid();
            (bool Status, string Message) res;
            if (Bucket.Id != Guid.Empty)
            {
                res = _bucketService.UpdateBucket(Bucket, UserId);
            }
            else
            {
                var item = _mapper.Map<Entities.Bucket>(Bucket);
                item.Id = CombGuid.NewGuid();
                item.CreationTime = DateTime.Now;
                item.Creator = UserId;
                res = _bucketService.AddBucket(item);
            }
            AjaxData.Message = res.Message;
            AjaxData.Code = res.Status ? 0 : 2001;
            return Json(AjaxData);
        }

    }
}