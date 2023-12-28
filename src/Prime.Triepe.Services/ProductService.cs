using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Triepe.Domain.Abstractions.Providers;
using Triepe.Domain.Abstractions.Repositories;
using Triepe.Domain.Abstractions.Services;
using Triepe.Domain.Dtos.ProductDtos;
using Triepe.Domain.Pagination;

namespace Triepe.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;
        private readonly IValidationProvider _validationProvider;

        public ProductService(IProductRepository repository, IValidationProvider validationProvider)
        {
            _repository = repository;
            _validationProvider = validationProvider;
        }
        
        public async Task<ProductResponseDto> CreateAsync(ProductRequestDto productDto)
        {
            _validationProvider.TryValidate(productDto);

            return await _repository.CreateAsync<ProductRequestDto, ProductResponseDto>(productDto);
        }

        public async Task DeleteAsync(Guid id)
        {
            EnsureValidId(id);

            await _repository.DeleteAsync(id);
        }

        public async Task<PaginatedResult<ProductResponseDto>> GetPaginatedResultAsync(PagingInfo pagingInfo,
            Expression<Func<ProductResponseDto, bool>> filter = null)
        {
            return await _repository.GetPageAsync(pagingInfo.Page, pagingInfo.Size, filter);
        }

        public async Task<ProductResponseDto> GetByIdAsync(Guid id)
        {
            EnsureValidId(id);

            return await _repository.GetByIdAsync<ProductResponseDto>(id);
        }

        public async Task UpdateAsync(Guid id, ProductRequestDto productDto)
        {
            EnsureValidId(id);

            _validationProvider.TryValidate(productDto);

            await _repository.UpdateAsync(id, productDto);
        }

        private void EnsureValidId(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentNullException(nameof(id));
        }
    }
}
