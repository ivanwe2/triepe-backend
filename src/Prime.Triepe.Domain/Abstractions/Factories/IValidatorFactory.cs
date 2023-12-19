using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Triepe.Domain.Abstractions.Factories
{
    public interface IValidatorFactory
    {
        IValidator<T> GetValidator<T>();
    }
}
