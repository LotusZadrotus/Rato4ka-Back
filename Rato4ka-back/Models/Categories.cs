using System.ComponentModel.DataAnnotations.Schema;

namespace Rato4ka_back.Models
{
    public class Categories: Base
    {
        [Column("desc")]
        public string Description { get; set; }
    }
}
