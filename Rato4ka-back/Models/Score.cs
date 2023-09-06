using System.ComponentModel.DataAnnotations.Schema;

namespace Rato4ka_back.Models
{
    [Table("Scores")]
    public class Score
    {
        [Column("user_id")]
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }
        [Column("score")]
        public int ScoreValue { get; set; }
        [Column("score_cat_id")]
        public int ScoreCatId { get; set; }
        [ForeignKey("ScoreCatId")]
        public ScoreCategory ScoreCategory { get; set; }
    }
}