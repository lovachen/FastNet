using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AiBao.Entities
{
    public partial class Sys_Permission
    {
        public Guid Id { get; set; }
        public Guid RoleId { get; set; }
        public Guid CategoryId { get; set; }
    }
}
