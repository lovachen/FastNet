using AiBao.Framework.Filters;
using cts.web.core.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AiBao.Framework.Pages
{
    [WebException]
    public abstract class BasePageModel : PageModel
    {
        /// <summary>
        /// ajax请求返回结果
        /// </summary> 
        protected AjaxResult AjaxData = new AjaxResult() { Code = -1, Message = "未知信息" };

        /// <summary>
        /// 返回json结果
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected IActionResult Json(object value)
        {
            return new JsonResult(value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected IActionResult NotValid()
        {
            AjaxData.Code = 1005;
            AjaxData.Message = ModelState.GetErrMsg();
            return new JsonResult(AjaxData);
        }
         
    }
}
