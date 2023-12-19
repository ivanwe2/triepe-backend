using FluentValidation;
using FluentValidation.Results;
using System.Linq;
using System.Threading.Tasks;
using Triepe.Domain.Abstractions.Providers;
using Triepe.Domain.Exceptions;
using IValidatorFactory = Triepe.Domain.Abstractions.Factories.IValidatorFactory;
using ValidationException = Triepe.Domain.Exceptions.ValidationException;

namespace Triepe.Domain.Providers
{
    public class ValidationProvider : IValidationProvider
    {
        private readonly IValidatorFactory _validatorFactory;
        public ValidationProvider(IValidatorFactory validatorFactory)
        {
            _validatorFactory = validatorFactory;
        }
        public void TryValidate<TDto>(TDto dto)
        {
            var result = TryGetValidator<TDto>().Validate(dto);

            ThrowIfInvalid(result);
        }

        public async Task TryValidateAsync<TDto>(TDto dto)
        {
            var result = await TryGetValidator<TDto>().ValidateAsync(dto);

            ThrowIfInvalid(result);
        }

        private void ThrowIfInvalid(ValidationResult result)
        {
            if (!result.IsValid)
            {
                var message = string.Join(";  ", result.Errors.Select(x => x.ErrorMessage));
                throw new ValidationException(message);
            }
        }

        private IValidator<TDto> TryGetValidator<TDto>()
        {
            var dtoTypeValidator = _validatorFactory.GetValidator<TDto>();
            if (dtoTypeValidator is null)
            {
                throw new NoExistingValidatorException(typeof(TDto));
            }
            return dtoTypeValidator;
        }

    }
}
