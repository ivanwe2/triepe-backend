using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Triepe.Domain.Pagination;

namespace Triepe.Domain.Abstractions.Repositories
{
    public interface IBaseRepository
    {
        Task<TOutput> GetByIdAsync<TOutput>(Guid id);
        Task<PaginatedResult<TOutput>> GetPageAsync<TOutput>(
           int pageNumber, int pageSize, Expression<Func<TOutput, bool>> filter = null);
        Task<TOutput> CreateAsync<TInput, TOutput>(TInput dto);
        Task UpdateAsync<TInput>(Guid id, TInput dto);
        Task DeleteAsync(Guid id);
        Task<bool> HasAnyAsync(Guid id);
    }

}
