using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rato4ka_back.Models
{
    [Table("Cred")]
    public class Cred
    {
        [Key]
        [Column("login")]
        public string Login { get; set; }
        [Column("key")]
        public string Key { get; set; }
        [Column("email")]
        public bool IsEmail { get; set; }
        [Column("expDate")]
        public DateTime ExpDate { get; set; }
    }
}