using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TodoAppIdentityMvc.Models.Entities;

namespace TodoAppIdentityMvc.Core.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Todo> Todos { get; set; }

        public DbSet<Archive> Archives { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // AppUser → Todos (One-to-Many)
            modelBuilder.Entity<Todo>()
                .HasOne(t => t.AppUser)
                .WithMany(u => u.Todos)
                .HasForeignKey(t => t.AppUserId)
                .OnDelete(DeleteBehavior.Restrict); // Avoid cascading delete conflicts

            // AppUser → Archives (One-to-Many)
            modelBuilder.Entity<Archive>()
                .HasOne(a => a.AppUser)
                .WithMany(u => u.Archives)
                .HasForeignKey(a => a.AppUserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Todo → Archive (One-to-One or One-to-Many)
            modelBuilder.Entity<Archive>()
                .HasOne(a => a.Todo)
                .WithOne(t => t.Archive) // Or .WithMany() if one Todo can have multiple archives (rare)
                .HasForeignKey<Archive>(a => a.TodoId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete
        }

    }
}
