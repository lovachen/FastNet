using AiBao.Framework.Filters;
using cts.web.core.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace AiBao.Framework.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    [WebException(1)]
    public abstract class ApiBaseController: ControllerBase
    {
        public ApiJsonResult ApiData = new ApiJsonResult() { code = -1, msg = "未知信息" };


        /// <summary>
        /// 验证未通过
        /// </summary>
        /// <returns></returns>
        protected IActionResult NoValid()
        {
            ApiData.code = 1005;
            ApiData.msg = ModelState.GetErrMsg();
            return Ok(ApiData);
        }



    }
}
