using System.ComponentModel.DataAnnotations.Schema;

namespace Rato4ka_back.Models
{
    [Table("Contents_categories")]
    public class ContentsCategories
    {
        [Column("contents_id")]
        public string ContentsId { get; set; }
        [Column("categories_id")]
        public string CategoriesID { get; set; }
    }
}
