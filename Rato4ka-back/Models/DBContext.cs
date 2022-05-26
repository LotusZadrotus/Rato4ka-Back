using System.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Npgsql.TypeHandlers.DateTimeHandlers;

namespace Rato4ka_back.Models
{
    public class DBContext : DbContext
    {
        private readonly IConfiguration _config;
        public DbSet<User> Users { get; set; }
        public DbSet<Content> Contents { get; set; }
        public DbSet<Cred> Credentials { get; set; }
        public DbSet<Links> Links { get; set; }
        public DBContext(IConfiguration config)
        {
            _config = config;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(_config.GetConnectionString("DefaultConnection"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // modelBuilder.Entity<Content>()
            //     .Property(e => e.CreatedAt)
            //     .HasConversion(v=> v.ToString(), v=> new DateHandler(v))
        }
    }
}