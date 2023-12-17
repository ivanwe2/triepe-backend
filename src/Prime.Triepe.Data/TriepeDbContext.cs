using Microsoft.EntityFrameworkCore;
using Triepe.Data.Entities;

namespace Triepe.Data
{
    public class TriepeDbContext : DbContext
    {
        public TriepeDbContext(DbContextOptions<TriepeDbContext> options) : base(options)
        {
        }
        public DbSet<Picture> Photos { get; set; }
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Picture>()
                .HasOne(p => p.Product)
                .WithMany()
                .HasForeignKey(p => p.ProductId);

            modelBuilder.Entity<Product>()
                .HasMany(p => p.Pictures)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);
        }
    }
}
