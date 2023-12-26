using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Triepe.Data.Entities;
using Triepe.Domain.Dtos.PictureDtos;
using Triepe.Domain.Dtos.ProductDtos;

namespace Triepe.Data.Mapper
{
    public class PictureProfile : Profile
    {
        public PictureProfile() {
            CreateMap<PictureRequestDto, Picture>();
            CreateMap<Picture, PictureResponseDto>()
                .ForMember(dest => dest.Content, opt => opt.MapFrom(src => Convert.ToBase64String(src.Bytes)));
        }

    }
}
