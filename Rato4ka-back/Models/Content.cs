#nullable enable
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.VisualBasic;

namespace Rato4ka_back.Models
{
    [Table("Contents")]
    public class Content: Base
    {
        [Column(name:"name")]
        public string? Name { get; set; }
        [Column("tagsIds",TypeName = "json")]
        public string?[] TagsIds { get; set; }
        [Column("creatorId")]
        public int CreatorId { get; set; }
        [ForeignKey("CreatorId")]
        public User? User { get; set; }
        [Column("createdAt")]    
        public DateTime? CreatedAt { get; set; }
        [Column("releaseDate")]
        public DateTime? ReleaseDate { get; set; }
        [Column("description")]
        public string? Desc { get; set; }
        [Column("image")]
        public byte[]? Image { get; set; }
        [Column("imageURL")]
        public string? imgURL { get; set; }
    }
}