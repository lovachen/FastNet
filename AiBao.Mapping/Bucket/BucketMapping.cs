using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AiBao.Mapping
{
    [Serializable]
    public class BucketMapping
    {
        public Guid Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreationTime { get; set; }
        [StringLength(500)]
        public string Description { get; set; }
        public Guid Creator { get; set; }
        public bool IsCompress { get; set; }
    }
}
