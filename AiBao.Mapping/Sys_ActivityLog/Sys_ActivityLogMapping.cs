using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text;

namespace AiBao.Mapping
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    [DataContract]
    public class Sys_ActivityLogMapping
    {
        [DataMember]
        public Guid Id { get; set; }

        [DataMember]
        [StringLength(50)]
        public string Method { get; set; }

        [DataMember]
        [StringLength(100)]
        public string EntityName { get; set; }

        [DataMember]
        [StringLength(64)]
        public string PrimaryKey { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime CreationTime { get; set; }
        public Guid? Creator { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string OldValue { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string NewValue { get; set; }
         


        #region 

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string CreationTimeForamt => CreationTime.ToString("F");

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string UserName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string UserAccount { get; set; }

        [DataMember]
        public string Comment { get; set; }

        #endregion

    }
}
