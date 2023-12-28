using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Triepe.Domain.Dtos.ProductDtos;

namespace Triepe.Domain.Validators
{
    public class ProductRequestDtoValidator : AbstractValidator<ProductRequestDto>
    {
        public ProductRequestDtoValidator()
        {
            RuleFor(x => x.Name).NotEmpty()
                .MaximumLength(50).WithMessage("Name can not be larger than 50 symbols!");
            RuleFor(x => x.Price).NotNull()
                .InclusiveBetween(1,10000).WithMessage("Price must be larger than 0 and smaller than 10 000!");
            RuleFor(x => x.Quantity)
                .InclusiveBetween(0, int.MaxValue).WithMessage("Quantity can not be less than 0!");
        }
    }
}
