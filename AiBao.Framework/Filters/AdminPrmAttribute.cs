using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.AspNetCore.Http;
using cts.web.core.Model;

namespace AiBao.Framework.Filters
{
    public class AdminPrmAttribute: TypeFilterAttribute
    {
        int _platform;
        bool _ignoreFilter;

        public AdminPrmAttribute(int platform = 0, bool ignore = false) : base(typeof(AdminPrmFilter))
        {
            _platform = platform;
            _ignoreFilter = ignore;
            Arguments = new object[] { platform, ignore };
        }

        public bool IgnoreFilter => _ignoreFilter;


        private class AdminPrmFilter : IActionFilter
        {
            int _platform;
            bool _ignoreFilter;
            WorkContext _workContext;

            public AdminPrmFilter(WorkContext workContext,
                int platform,
                bool ignore)
            {
                _workContext = workContext;
                _platform = platform;
                _ignoreFilter = ignore;
            }

            public void OnActionExecuted(ActionExecutedContext context)
            {
            }


            public void OnActionExecuting(ActionExecutingContext context)
            {
                var actionFilter = context.ActionDescriptor.FilterDescriptors.Where(filter => filter.Scope == FilterScope.Action)
                       .Select(filter => filter.Filter).OfType<AdminPrmAttribute>().FirstOrDefault();

                if (actionFilter?.IgnoreFilter ?? _ignoreFilter)
                    return;

                if (context.Filters.Any(filter => filter is AdminPrmFilter))
                {
                   var path = context.HttpContext.Request.Path;
                    if (!_workContext.IsPermit(path))
                    {
                        if (_platform == 0)
                        {
                            if (context.HttpContext.Request.IsAjaxRequest())
                            {
                                context.Result = new JsonResult(new AjaxResult() { Code = 1007, Message = "非法操作，您没有权限" });
                            }
                            else
                            {
                                context.Result = new RedirectResult("/admin/forbidden");
                            }
                        }
                        else
                        {
                            context.Result = new OkObjectResult(new ApiJsonResult() { code = 1007, msg = "非法操作，您没有权限" });
                        }
                    }
                }
            }
        }


    }
}
