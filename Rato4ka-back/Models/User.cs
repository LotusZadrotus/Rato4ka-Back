#nullable enable
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rato4ka_back.Models
{
    [Table("Users")]
    public class User: Base
    {
        [Column("avatar")]
        public byte[]? Avatar { get; set; }
        [Column("discord_id")]
        public string? DiscordId { get; set; }
        [Column("password")]
        public string? Password { get; set; }
        [Column("email")]
        [EmailAddress]
        public string? Email { get; set; }
        [Column("is_admin")]
        public bool IsAdmin { get; set; }
        [Column("name")]
        public string? Name { get; set; }
        [Column("login")]
        public string? Login { get; set; }
        [Column("salt")]
        public string? Salt { get; set; }
        [Column("is_email_confirmed")]
        public bool Confirmed { get; set; }
        [Column("desc")]
        public string? Description { get; set; }

        public User(int id, bool isAdmin, string name)
        {
            Id = id;
            Name = name;
            IsAdmin = isAdmin;
        }
        public User(){}

        public override string ToString()
        {
            return $"User:\n id:{Id}\n name:{Name}\n login:{Login}\n avatar:{Avatar}\n password:{Password}\n email:{Email}\n isAdmin:{IsAdmin}\n";
        }
    }
}