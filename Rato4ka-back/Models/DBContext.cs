using System.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Rato4ka_back.Models
{
    public class DBContext : DbContext
    {
        private readonly IConfiguration _config;
        private readonly IWebHostEnvironment _env;
        private const string DEFAULT = "DefaultConnection";
        private const string PROD = "ProdConnection";
        public DbSet<User> Users { get; set; }
        public DbSet<Contents> Contents { get; set; }
        public DbSet<Cred> Credentials { get; set; }
        public DbSet<LinksUsers> Links { get; set; }
        public DBContext(IConfiguration config, IWebHostEnvironment env)
        {
            _config = config;
            _env = env;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var con_str = _env.IsDevelopment() ? _config.GetConnectionString(DEFAULT) : _config.GetConnectionString(PROD); 
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