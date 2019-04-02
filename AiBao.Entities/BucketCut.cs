using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AiBao.Entities
{
    public partial class BucketCut
    {
        public Guid Id { get; set; }
        public Guid BucketId { get; set; }
        [Required]
        [StringLength(150)]
        public string Value { get; set; }
        public Guid Creator { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreationTime { get; set; }
    }
}
