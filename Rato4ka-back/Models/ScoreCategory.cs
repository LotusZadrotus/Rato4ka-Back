using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rato4ka_back.Models
{
    [Table("Score_categories")]
    public class ScoreCategory: Base
    {
        [Column("name")]
        public string Name { get; set; }
        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
        [Column("creator_id")]
        public int CreatorId { get; set; }
        [ForeignKey("CreatorId")]
        public User User { get; set; }
        [Column("content_id")]
        public int ContentId { get; set; }
        [ForeignKey("ContentId")]
        public Contents Content { get; set; }
        [Column("season_id")]
        public int SeasonId { get; set; }
        [ForeignKey("SeasonId")]
        public Season Season { get; set; }
        [Column("episode_id")]
        public int EpisodeId { get; set; }
        [ForeignKey("EpisodeId")]
        public Episode Episode { get; set; }
        [Column("image")]
        public byte?[] Image { get; set; }
        [Column("desc")]
        public string Description { get; set; }
    }
}