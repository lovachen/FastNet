using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AiBao.Mapping
{
    [Serializable]
    public class Sys_ActivityLogCommentMapping
    {
        [Key]
        [StringLength(100)]
        public string EntityName { get; set; }
        [StringLength(150)]
        public string Comment { get; set; }

    }
}
