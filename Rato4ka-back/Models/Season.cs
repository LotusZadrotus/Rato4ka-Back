#nullable enable
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rato4ka_back.Models
{
    [Table("Seasons")]
    public class Season: Base
    {
        [Column("name")]
        public string? Name { get; set; }
        [Column("content_id")]
        public int ContentId { get; set; }
        [ForeignKey("ContentId")]
        [Column("creator_id")]
        public int CreatorId { get; set; }
        [ForeignKey("ContentId")]
        [Column("release_date")]
        public DateTime? ReleaseDate { get; set; }
        [Column("image")]
        public Byte[]? Image { get; set; }
        [Column("desc")]
        public string? Description { get; set; }
        [Column("season_number")]
        public int? SeasonNumber { get; set; }
        [Column("is_movie")]
        public bool IsMovie { get; set; } = false;
        [Column("duration")]
        public int? Duration { get; set; }
        
        public Contents Content { get; set; }
        
    }
}