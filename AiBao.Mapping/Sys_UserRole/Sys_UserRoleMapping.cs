using System;
using System.Collections.Generic;
using System.Text;

namespace AiBao.Mapping
{
    [Serializable]
    public class Sys_UserRoleMapping
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid RoleId { get; set; }
    }
}
