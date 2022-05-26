#nullable enable
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rato4ka_back.Models
{
    [Table("Seasons")]
    public class Season
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("name")]
        public string? Name { get; set; }
        [Column("seasonNumber")]
        public int? SeasonNumber { get; set; }
        [Column("numberOfEpisodes")]
        public int? NumberOfEpisodes { get; set; }
        [Column("description")]
        public string? Description { get; set; }
        [Column("releaseDate")]
        public DateTime? ReleaseDate { get; set; }
        [Column("contentId")]
        public int ContentId { get; set; }
        [ForeignKey("ContentId")]
        public Content Content { get; set; }
        
    }
}