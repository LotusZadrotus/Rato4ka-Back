using System.ComponentModel.DataAnnotations.Schema;

namespace Rato4ka_back.Models
{
    public class Links
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("link")]
        public string Link { get; set; }
    }
}