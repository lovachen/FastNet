using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AiBao.Web.Areas.Models
{
    public class UploadModel
    {
        /// <summary>
        /// 
        /// </summary>
        [Required(ErrorMessage = "请输入AccessKeyId")]
        public string AccessKeyId { get; set; }
        
        /// <summary>
        /// 桶名称
        /// </summary>
        [Required(ErrorMessage = "请输入bucket")]
        public string bucket { get; set; }

        /// <summary>
        /// 授权验证字符串
        /// </summary>
        [Required(ErrorMessage = "授权验证字符串")]
        public string signature { get; set; }

        /// <summary>
        /// 表示HTTP 请求的Method，主要有PUT、GET、POST、HEAD、DELETE等。
        /// </summary>
        [Required(ErrorMessage = "请输入VERB")]
        public string VERB { get; set; }
    }
}
