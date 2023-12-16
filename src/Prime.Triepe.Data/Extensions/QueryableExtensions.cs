using Microsoft.EntityFrameworkCore;
using Prime.Triepe.Domain.Dtos;
using System.Linq;
using System.Threading.Tasks;

namespace Prime.Triepe.Data.Extensions
{
    public static class QueryableExtensions
    {
        public static async Task<PaginatedResult<T>> PaginateAsync<T>(this IQueryable<T> collection, int pageNumber, int pageSize)
        {
            var count = await collection.CountAsync();
            var skip = (pageNumber - 1) * pageSize;
            if (count == 0 || count < skip)
            {
                return PaginatedResult<T>.Empty();
            }

            var result = await collection.Skip(skip).Take(pageSize).ToListAsync();

            return new PaginatedResult<T>(result, count);
        }
    }
}
