using System.ComponentModel.DataAnnotations.Schema;

namespace Rato4ka_back.Models
{
    [Table("Scores")]
    public class Score
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("userId")]
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }
        [Column("score")]
        public int ScoreValue { get; set; }
        [Column("scoreCatId")]
        public int ScoreCatId { get; set; }
        [ForeignKey("ScoreCatId")]
        public ScoreCategory ScoreCategory { get; set; }
    }
}