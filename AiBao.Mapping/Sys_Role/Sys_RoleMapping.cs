using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AiBao.Mapping
{
    [Serializable]
    //[Bind(new string[] { "Id"})]
    public class Sys_RoleMapping
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage ="请输入角色名称")]
        [StringLength(50,ErrorMessage ="名称不能操过50个字符")]
        public string Name { get; set; }

        public Guid? Creator { get; set; }


        [Column(TypeName = "datetime")]
        public DateTime CreationTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [StringLength(150)]
        public string Description { get; set; }
    }
}
