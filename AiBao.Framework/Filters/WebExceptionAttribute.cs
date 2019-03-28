using cts.web.core.Mail;
using cts.web.core.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace AiBao.Framework.Filters
{
    public class WebExceptionAttribute : TypeFilterAttribute
    {
        /// <summary>
        /// 
        /// </summary>
        public WebExceptionAttribute() : base(typeof(ApiExceptionFilter))
        {
            Arguments = new object[] { };
        }

        private class ApiExceptionFilter : IExceptionFilter
        {
            private ILogger<ApiExceptionFilter> _logger;
            private IMailSender _mailSender;
            //private ISettingService _settingService;

            public ApiExceptionFilter(ILogger<ApiExceptionFilter> logger,
                IMailSender mailSender
                //ISettingService settingService
                )
            {
                _logger = logger;
                _mailSender = mailSender;
                //_settingService = settingService;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="context"></param>
            public void OnException(ExceptionContext context)
            {
                try
                {
                    var ex = context.Exception;
                    _logger.LogError(context.Exception, ex.Message ?? "");
                    //发送邮件提醒
                    //var settings = _settingService.GetMasterSettings();
                    //if (!String.IsNullOrEmpty(settings.ErrorToMailAddress))
                    //{
                    //    string[] mails = mails = new string[] { settings.ErrorToMailAddress };
                    //    if (settings.ErrorToMailAddress.Contains(";"))
                    //    {
                    //        mails = settings.ErrorToMailAddress.Split(";");
                    //    }
                    //    //_mailSender.Smtp(mails, "BB.Api接口错误提醒", ex.Message + Environment.NewLine + ex.StackTrace);
                    //}
                }
                catch (Exception)
                {

                }
                context.Result = new OkObjectResult(new ApiJsonResult() { code = 1001, msg = "系统错误，请稍后重试" });
            }



        }
    }
}
