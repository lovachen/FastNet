using AiBao.Services;
using cts.web.core.Mail;
using cts.web.core.Model;
using Microsoft.AspNetCore.Http;
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
        int _platform;
        string _pageDir;
        /// <summary>
        /// 
        /// </summary>
        public WebExceptionAttribute(int platform = 0, string pageDir = "/admin") : base(typeof(ApiExceptionFilter))
        {
            _platform = platform;
            _pageDir = pageDir;
            Arguments = new object[] { platform , pageDir };
        }

        private class ApiExceptionFilter : IExceptionFilter
        {
            private ILogger<ApiExceptionFilter> _logger;
            private IMailProvide _mailProvide;
            private SettingService _settingService;
            int _platform;
            string _pageDir;

            public ApiExceptionFilter(ILogger<ApiExceptionFilter> logger,
                IMailProvide mailProvide,
                SettingService settingService,
                int platform = 0,
                string pageDir = "/admin")
            {
                _logger = logger;
                _mailProvide = mailProvide;
                _settingService = settingService;
                _platform = platform;
            _pageDir = pageDir;
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
                    var settings = _settingService.GetMasterSettings();
                    if (!String.IsNullOrEmpty(settings.ErrorToMailAddress))
                    {
                        string[] mails = mails = new string[] { settings.ErrorToMailAddress };
                        if (settings.ErrorToMailAddress.Contains(";"))
                        {
                            mails = settings.ErrorToMailAddress.Split(";");
                        }
                        var config = new MailConfig() { Account = settings.EmailAccount, Host = settings.EmailHost, Password = settings.EmailPassword };
                        if (int.TryParse(settings.EmailPort, out int _port))
                        {
                            config.Port = _port;
                            _mailProvide.Smtp(config, mails, $"{settings.SiteName}系统错误提醒", ex.Message + Environment.NewLine + ex.StackTrace);
                        }
                    }
                }
                catch (Exception)
                {

                }
                switch (_platform)
                {
                    case 0:
                        if (context.HttpContext.Request.IsAjaxRequest())
                        {
                            context.Result = new JsonResult(new AjaxResult() { Code = 1001, Message = "系统错误，请稍后重试" });
                        }
                        else
                        {
                            context.Result = new RedirectToPageResult($"{_pageDir}/error");
                        }
                        break;
                    case 1:
                        context.Result = new OkObjectResult(new ApiJsonResult() { code = 1001, msg = "系统错误，请稍后重试" });
                        break;
                }

            }



        }
    }
}
