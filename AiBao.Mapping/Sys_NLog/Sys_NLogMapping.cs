using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text;

namespace AiBao.Mapping
{
    [Serializable]
    public class Sys_NLogMapping
    {
        public Guid Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Application { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime Logged { get; set; }
        [Required]
        [StringLength(50)]
        public string Level { get; set; }
        public string Message { get; set; }
        public string Logger { get; set; }
        public string Callsite { get; set; }
        public string Exception { get; set; }

        #region 

        /// <summary>
        /// 
        /// </summary>
        public string LoggedFormat => Logged.ToString("F");

        #endregion
    }
}
