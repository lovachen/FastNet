using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AiBao.Web.Models
{
     public class ViewLoginModel
    {
        /// <summary>
        /// 
        /// </summary>
        [Required(ErrorMessage ="请输入账号")]
        public string Account { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required(ErrorMessage ="请输入密码")]
        public string Password { get; set; }
    }
}
