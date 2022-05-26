using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rato4ka_back.Models
{
    [Table("ScoreCat")]
    public class ScoreCategory
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("compType")]
        public string ComponentType { get; set; }
        [Column("createdAt")]
        public DateTime CreatedAt { get; set; }
        [Column("pictureURL")]
        public string PictureUrl { get; set; }
        [Column("createdBy")]
        public int CreatedBy { get; set; }
        [ForeignKey("CreatedBy")]
        public User User { get; set; }
        [Column("contentId")]
        public int ContentId { get; set; }
        [ForeignKey("ContentId")]
        public Content Content { get; set; }
        [Column("seasonId")]
        public int SeasonId { get; set; }
        [ForeignKey("SeasonId")]
        public Season Season { get; set; }
        [Column("episodeId")]
        public int EpisodeId { get; set; }
        [ForeignKey("EpisodeId")]
        public Episode Episode { get; set; }
    }
}