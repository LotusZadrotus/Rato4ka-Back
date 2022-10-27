using System.ComponentModel.DataAnnotations.Schema;

namespace Rato4ka_back.Models
{
    public class Links: Base
    {
        [Column("link")]
        public string Link { get; set; }
    }
}