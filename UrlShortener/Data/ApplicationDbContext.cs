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
        public DbSet<AboutContent> AboutContents { get; set; }
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

            modelBuilder.Entity<AboutContent>(entity =>
            {
                entity.HasKey(a => a.Id);

                entity.Property(a => a.Content)
                    .IsRequired();

                entity.HasOne(a => a.LastModifiedBy)
                    .WithMany()
                    .HasForeignKey(a => a.LastModifiedById)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
