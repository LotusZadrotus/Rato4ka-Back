#nullable enable
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.VisualBasic;

namespace Rato4ka_back.Models
{
    [Table("Contents")]
    public class Contents: Base
    {
        [Column(name:"name")]
        public string? Name { get; set; }
        [Column("creator_id")]
        public int CreatorId { get; set; }
        [ForeignKey("CreatorId")]
        public User? User { get; set; }
        [Column("created_at")]    
        public DateTime? CreatedAt { get; set; }
        [Column("release_date")]
        public DateTime? ReleaseDate { get; set; }
        [Column("desc")]
        public string? Decription { get; set; }
        [Column("image")]
        public byte[]? Image { get; set; }
    }
}