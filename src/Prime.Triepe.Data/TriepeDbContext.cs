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
            modelBuilder.Entity<Product>()
                .Navigation(p => p.Pictures);

            //modelBuilder.Entity<Picture>()
            //    .HasOne(p => p.Product)
            //    .WithMany()
            //    .HasForeignKey(p => p.ProductId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
