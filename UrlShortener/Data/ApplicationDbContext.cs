using Microsoft.EntityFrameworkCore;
using UrlShortener.Models.Entities;

namespace UrlShortener.Data
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<ShortUrl> ShortUrls { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Login)
                .IsUnique();
            modelBuilder.Entity<ShortUrl>()
                .HasIndex(su => su.ShortCode)
                .IsUnique();
            modelBuilder.Entity<User>()
                .HasMany(u => u.CreatedUrls)
                .WithOne(su => su.CreatedBy)
                .HasForeignKey(su => su.CreatedById);
        }
    }
}
