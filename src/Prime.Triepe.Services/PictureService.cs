using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Triepe.Domain.Abstractions.Providers;
using Triepe.Domain.Abstractions.Repositories;
using Triepe.Domain.Abstractions.Services;
using Triepe.Domain.Dtos.PictureDtos;
using Triepe.Domain.Exceptions;
using Triepe.Domain.Pagination;
using Triepe.Domain.RequestForms;

namespace Triepe.Services
{
    public class PictureService : IPictureService
    {
        private readonly IPictureRepository _repository;
        private readonly IProductRepository _productRepository;
        private readonly IValidationProvider _validationProvider;

        public PictureService(IPictureRepository repository, 
            IValidationProvider validationProvider,
            IProductRepository productRepository)
        {
            _repository = repository;
            _validationProvider = validationProvider;
            _productRepository = productRepository;
        }

        public async Task<PictureResponseDto> CreateAsync(PictureCreateRequestForm requestForm)
        {
            await EnsureExistingProduct(requestForm.ProductId);

            var dto = new PictureRequestDto()
            {
                Size = requestForm.File.Length,
                Description = requestForm.FileName,
                ProductId = requestForm.ProductId,
                FileExtension = requestForm.File.ContentType,
                Bytes = ConvertIFormFileToByteArray(requestForm.File)
            };

            _validationProvider.TryValidate(dto);

            return await _repository.CreateAsync<PictureRequestDto, PictureResponseDto>(dto);
        }

        public async Task DeleteAsync(Guid id)
        {
            EnsureValidId(id);

            await _repository.DeleteAsync(id);
        }

        public async Task<PaginatedResult<PictureResponseDto>> GetPaginatedResultAsync(PagingInfo pagingInfo,
            Expression<Func<PictureResponseDto, bool>> filter = null)
        {
            return await _repository.GetPageAsync(pagingInfo.Page, pagingInfo.Size, filter);
        }

        public async Task<PictureResponseDto> GetByIdAsync(Guid id)
        {
            EnsureValidId(id);

            return await _repository.GetByIdAsync<PictureResponseDto>(id);
        }

        public async Task UpdateAsync(Guid id, PictureUpdateRequestForm requestForm)
        {
            EnsureValidId(id);

            var dto = new PictureRequestDto()
            {
                Size = requestForm.File.Length,
                Description = requestForm.FileName,
                FileExtension = requestForm.File.ContentType,
                Bytes = ConvertIFormFileToByteArray(requestForm.File)
            };

            _validationProvider.TryValidate(dto);

            await _repository.UpdateAsync(id, dto);
        }

        private void EnsureValidId(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentNullException(nameof(id));
        }

        private async Task EnsureExistingProduct(Guid productId)
        {
            if(!await _productRepository.HasAnyAsync(productId))
            {
                throw new Domain.Exceptions.NotFoundException("Product was not found!");
            }
        }

        private byte[] ConvertIFormFileToByteArray(IFormFile file)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                file.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }
    }
}
