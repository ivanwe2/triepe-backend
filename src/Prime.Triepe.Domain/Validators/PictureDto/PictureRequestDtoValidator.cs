using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Triepe.Domain.Dtos.PictureDtos;

namespace Triepe.Domain.Validators.PictureDto
{
    public class PictureRequestDtoValidator : AbstractValidator<PictureRequestDto>
    {
        public PictureRequestDtoValidator()
        {
            RuleFor(x => x.FileExtension)
                .NotEmpty()
                .MaximumLength(10).WithMessage("Extension can not be larger than 10 symbols!");
            RuleFor(x => x.Description)
                .NotEmpty()
                .MaximumLength(50).WithMessage("Description can not be larger than 50 symbols!");
            RuleFor(x => x.Bytes)
                .NotEmpty()
                .WithMessage("Failed to convert file!");
            RuleFor(x => x.Size)
                .InclusiveBetween(0, decimal.MaxValue).WithMessage("Size can not be less than 0!");
            RuleFor(x => x.ProductId)
                .NotEmpty().WithMessage("Invalid Product Id provided!");
        }
    }
}
