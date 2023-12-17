using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Triepe.Domain.Pagination;

namespace Triepe.Data.Extensions
{
    public static class QueryableExtensions
    {
        public static async Task<PaginatedResult<T>> PaginateAsync<T>(this IQueryable<T> collection, int pageNumber, int pageSize, bool isSortingEmpty = true)
        {
            if (pageNumber < 0 || pageSize < 0)
            {
                throw new ValidationException("Page and size should not be negative values!");
            }

            var totalElements = await collection.CountAsync();
            var skip = pageNumber * pageSize;

            if (totalElements == 0 || totalElements < skip)
            {
                return PaginatedResult<T>.EmptyResult(totalElements, pageNumber, isSortingEmpty);
            }

            var result = await collection.Skip(skip).Take(pageSize).ToListAsync();

            return new PaginatedResult<T>(result, totalElements, pageNumber, pageSize, isSortingEmpty);
        }

    }
}
