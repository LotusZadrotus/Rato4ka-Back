using System.ComponentModel.DataAnnotations.Schema;

namespace Rato4ka_back.Models
{
    [Table("Links_season")]
    public class LinksSeasons
    {
        [Column("link")]
        public string Link { get; set; }
    }
}
