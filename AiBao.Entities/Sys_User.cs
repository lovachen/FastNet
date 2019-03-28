using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AiBao.Entities
{
    public partial class Sys_User
    {
        public Guid Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Account { get; set; }
        [Required]
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
        [StringLength(256)]
        public string Email { get; set; }
        [StringLength(50)]
        public string MobilePhone { get; set; }
    }
}
