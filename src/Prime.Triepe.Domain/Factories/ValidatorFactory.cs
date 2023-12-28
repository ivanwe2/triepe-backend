using FluentValidation;
using System;
using IValidatorFactory = Triepe.Domain.Abstractions.Factories.IValidatorFactory;

namespace Triepe.Domain.Factories
{
    public class ValidatorFactory : IValidatorFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public ValidatorFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IValidator<T> GetValidator<T>()
        {
            return (IValidator<T>)_serviceProvider.GetService(typeof(IValidator<T>));
        }
    }

}
