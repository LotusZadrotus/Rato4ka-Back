#nullable enable
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rato4ka_back.Models
{
    [Table("Episodes")]
    public class Episode
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("episodeNumber")]
        public int EpisodeNumber { get; set; }
        [Column("name")]
        public string? Name { get; set; }
        [Column("description")]
        public string? Description { get; set; }
        [Column("isMovie")]
        public bool IsMovie { get; set; }
        [Column("duration")]
        public int Duration { get; set; }
        [Column("releaseDate")]
        public DateTime? ReleaseDate { get; set; }
        [Column("seasonId")]
        public int SeasonId { get; set; }
        [ForeignKey("SeasonId")]
        public Season? Season { get; set; }
    }
}