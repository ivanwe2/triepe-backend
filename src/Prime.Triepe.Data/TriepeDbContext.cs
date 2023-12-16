using Microsoft.EntityFrameworkCore;

namespace Prime.Triepe.Data
{
    public class TriepeDbContext : DbContext
    {
        public TriepeDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}
