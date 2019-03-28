using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AiBao.Entities
{
    public partial class Sys_ActivityLog
    {
        public Guid Id { get; set; }
        [StringLength(50)]
        public string Method { get; set; }
        [StringLength(100)]
        public string EntityName { get; set; }
        [StringLength(64)]
        public string PrimaryKey { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreationTime { get; set; }
        public Guid? Creator { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        [StringLength(150)]
        public string Comment { get; set; }
    }
}
