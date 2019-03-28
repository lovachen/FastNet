using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text;

namespace AiBao.Mapping
{
    [Serializable]
    [DataContract]
    public class Sys_UserLoginMapping
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }

        [StringLength(50)]
        [DataMember]
        public string IpAddress { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? LoginTime { get; set; }

        public bool Status { get; set; }


        #region 扩展

        /// <summary>
        /// 状态格式
        /// </summary>
        [DataMember]
        public string StatusDecs => Status ? "成功" : "失败";

        /// <summary>
        /// 日期格式
        /// </summary>
        [DataMember]
        public string LoginTimeForamt => LoginTime?.ToString("F") ?? "";

        #endregion
    }
}
