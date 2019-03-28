using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AiBao.Entities
{
    public partial class Sys_Role
    {
        public Guid Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        public Guid? Creator { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreationTime { get; set; }
        [StringLength(150)]
        public string Description { get; set; }
    }
}
