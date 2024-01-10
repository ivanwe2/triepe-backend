using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Triepe.Domain.Dtos.PictureDtos;
using Triepe.Domain.Pagination;
using Triepe.Domain.RequestForms;

namespace Triepe.Domain.Abstractions.Services
{
    public interface IPictureService
    {
        Task<PictureResponseDto> CreateAsync(PictureCreateRequestForm requestForm);
        Task DeleteAsync(Guid id);
        Task<PictureResponseDto> GetByIdAsync(Guid id);
        Task<PaginatedResult<PictureResponseDto>> GetPaginatedResultAsync(PagingInfo pagingInfo,
            Expression<Func<PictureResponseDto, bool>> filter = null);
        Task UpdateAsync(Guid id, PictureUpdateRequestForm requestForm);
    }
}
