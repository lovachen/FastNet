using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AiBao.Entities
{
    public partial class Sys_UserLogin
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        [StringLength(50)]
        public string IpAddress { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? LoginTime { get; set; }
        public bool Status { get; set; }
    }
}
