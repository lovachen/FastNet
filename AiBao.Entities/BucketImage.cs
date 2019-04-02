using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AiBao.Entities
{
    public partial class BucketImage
    {
        public Guid Id { get; set; }
        public Guid BucketId { get; set; }
        [Required]
        [StringLength(150)]
        public string FileName { get; set; }
        [Required]
        [StringLength(256)]
        public string VisitUrl { get; set; }
        [Required]
        [StringLength(500)]
        public string IOPath { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        [Required]
        [StringLength(256)]
        public string SHA1 { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreationTime { get; set; }
        [StringLength(10)]
        public string ExtName { get; set; }
    }
}
