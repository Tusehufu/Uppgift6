using Microsoft.EntityFrameworkCore;
using InternJohan.Dev.Infrastructure.Models;

namespace InternJohan.Dev.App
{
    public class ApplicationDbContext : DbContext
    {
        // Konstruktor som tar DbContextOptions och skickar det vidare till basklassen
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Definiera DbSet-egenskaper för dina modeller (entiteter)
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<SportEvent> SportEvents { get; set; }

        // Konfigurera modeller och relationer i OnModelCreating
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Definiera rollernas data
            var roles = new List<Role>
    {
        new Role { Id = 1, Name = "User" },
        new Role { Id = 2, Name = "Admin" },
        new Role { Id = 3, Name = "Moderator" }
    };

            // Seed rollerna med HasData
            modelBuilder.Entity<Role>().HasData(roles);

            // Lägg till andra konfigurationer och relationer här om det behövs
        }
    }
}
