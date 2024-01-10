using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Triepe.Domain.Abstractions.Providers
{
    public interface IValidationProvider
    {
        void TryValidate<TDto>(TDto dto);
        Task TryValidateAsync<TDto>(TDto dto);
    }
}
