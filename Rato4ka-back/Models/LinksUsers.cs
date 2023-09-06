using System.ComponentModel.DataAnnotations.Schema;

namespace Rato4ka_back.Models
{
    [Table("Links_user")]
    public class LinksUsers: Base
    {
        [Column("link")]
        public string Link { get; set; }
    }
}