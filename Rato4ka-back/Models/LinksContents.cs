using System.ComponentModel.DataAnnotations.Schema;

namespace Rato4ka_back.Models
{
    [Table("LinksContent")]
    public class LinksContents
    {
        [Column("link")]
        public string Link { get; set; }
    }
}
