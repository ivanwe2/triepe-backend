using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Triepe.Domain.Dtos.ProductDtos;
using Triepe.Domain.Pagination;

namespace Triepe.Domain.Abstractions.Services
{
    public interface IProductService
    {
        Task<ProductResponseDto> CreateAsync(ProductRequestDto productDto);
        Task DeleteAsync(Guid id);
        Task<ProductResponseDto> GetByIdAsync(Guid id);
        Task<PaginatedResult<ProductResponseDto>> GetPaginatedResultAsync(PagingInfo pagingInfo,
            Expression<Func<ProductResponseDto, bool>> filter = null);
        Task UpdateAsync(Guid id, ProductRequestDto productDto);
    }
}
