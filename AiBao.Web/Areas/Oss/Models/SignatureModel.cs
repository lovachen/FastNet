using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AiBao.Web.Areas.Models
{
    /// <summary>
    /// 签名对象
    /// </summary>
    public class SignatureModel
    {
        [Required(ErrorMessage = "请输入AccessKeyId")]
        public string AccessKeyId { get; set; }

        [Required(ErrorMessage ="请输入文件的MD5")]
        public string ContentMD5 { get; set; }

        /// <summary>
        /// 表示HTTP 请求的Method，主要有PUT、GET、POST、HEAD、DELETE等。
        /// </summary>
        [Required(ErrorMessage = "请输入VERB")]
        public string VERB { get; set; }
    }
}
