using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.VisualBasic;
namespace Rato4ka_back.Models{
    public class Base{
        [Key]
        [Column("id")]
        public int Id{ get; set; }
    }
}