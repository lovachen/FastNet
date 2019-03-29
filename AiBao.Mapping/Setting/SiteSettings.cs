using System;
using System.Collections.Generic;
using System.Text;

namespace AiBao.Mapping
{
    public class SiteSettings
    {

        /// <summary>
        /// 
        /// </summary>
        public string SiteName { get; set; } = "AiBao管理中心";

        /// <summary>
        /// 邮件发送host
        /// </summary>
        public string EmailHost { get; set; } = "smtp.qq.com";

        /// <summary>
        /// 邮件端口
        /// </summary>
        public string EmailPort { get; set; } = "587";

        /// <summary>
        /// 邮件账号
        /// </summary>
        public string EmailAccount { get; set; }

        /// <summary>
        /// 邮件地址
        /// </summary>
        public string EmailPassword { get; set; }
    }
}
