using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AiBao.Web.Models
{
    public class ChangePwdModel
    {
        [Required(ErrorMessage ="请输入原密码")]
        public string OldPwd { get; set; }

        [Required(ErrorMessage ="请输入新密码")]
        public string NewPwd { get; set; }

        [Compare("NewPwd", ErrorMessage ="新密码不一致")]
        public string ConfirmPwd { get; set; }
    }
}
