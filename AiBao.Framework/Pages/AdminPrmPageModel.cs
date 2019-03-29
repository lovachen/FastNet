using AiBao.Framework.Filters;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace AiBao.Framework.Pages
{
    /// <summary>
    /// 权限验证基类
    /// </summary>
    public abstract class AdminPrmPageModel:AdminPageModel
    {

        /// <summary>
        /// 添加权限验证
        /// </summary>
        /// <param name="context"></param>
        public override void OnPageHandlerExecuting(PageHandlerExecutingContext context)
        {
            base.OnPageHandlerExecuting(context);

            if (!WorkContext.IsPermit(context.HttpContext.Request.Path))
            {
                if (context.HttpContext.Request.IsAjaxRequest())
                {
                    AjaxData.Code = 1007;
                    AjaxData.Message = "没有权限";
                    context.Result = Json(AjaxData);
                }
                else
                {
                    context.Result = Redirect("/admin/forbidden");
                }
            }

        }

    }
}
