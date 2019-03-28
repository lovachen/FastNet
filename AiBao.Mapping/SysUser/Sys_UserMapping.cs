using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AiBao.Mapping
{
    [Serializable]
    public class Sys_UserMapping
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage ="请输入账号")]
        [StringLength(50)]
        public string Account { get; set; }

        [Required(ErrorMessage ="请输入姓名")]
        [StringLength(10)]
        public string Name { get; set; }

        [StringLength(512)]
        public string Password { get; set; }

        [StringLength(256)]
        public string Salt { get; set; }

        public bool IsAdmin { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime CreationTime { get; set; }

        [StringLength(50)]
        public string LastIpAddress { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? LastActivityTime { get; set; }

        public bool IsDeleted { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? DeletedTime { get; set; }

        public Guid? Creator { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [StringLength(256)]
        [RegularExpression(@"^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$",ErrorMessage ="邮箱地址格式错误")]
        public string Email { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string MobilePhone { get; set; }
    }
}
