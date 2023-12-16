using Prime.Triepe.Domain.Dtos;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Prime.Triepe.Domain.Abstractions.Repositories
{
    public interface IBaseRepository
    {
        Task<TDto> GetByIdAsync<TDto>(Guid id);
        Task<PaginatedResult<TDto>> GetAllAsync<TDto>(
            int pageNumber, int pageSize, Expression<Func<TDto, bool>> filter = null);
        Task<TDto> CreateAsync<TDto>(TDto dto);
        Task UpdateAsync<TDto>(TDto dto);
        Task DeleteAsync(Guid id);
    }
}
