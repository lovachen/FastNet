using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AiBao.Entities
{
    public partial class Sys_ActivityLogComment
    {
        [Key]
        [StringLength(100)]
        public string EntityName { get; set; }
        [StringLength(150)]
        public string Comment { get; set; }
    }
}
