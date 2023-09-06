#nullable enable
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rato4ka_back.Models
{
    [Table("Episodes")]
    public class Episode: Base
    {
        [Column("name")]
        public string? Name { get; set; }
        [Column("season_id")]
        public int SeasonId { get; set; }
        [ForeignKey("SeasonId")]
        public Season? Season { get; set; }
        [Column("episode_number")]
        public int EpisodeNumber { get; set; }
        [Column("image")]
        public Byte[]? Image { get; set; }
        [Column("duration")]
        public int Duration { get; set; }
        [Column("release_date")]
        public DateTime? ReleaseDate { get; set; }
        [Column("desc")]
        public string? Description { get; set; }
    }
}