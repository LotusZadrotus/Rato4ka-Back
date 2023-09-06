using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rato4ka_back.Models
{
    [Table("Creditionals")]
    public class Cred
    {
        [Key]
        [Column("login")]
        public string Login { get; set; }
        [Column("key")]
        public string Key { get; set; }
        [Column("email")]
        public bool IsEmail { get; set; }
        [Column("exp_date")]
        public DateTime ExpDate { get; set; }
    }
}