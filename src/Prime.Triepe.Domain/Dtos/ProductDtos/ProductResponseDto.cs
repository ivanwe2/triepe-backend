using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Triepe.Domain.Dtos.PictureDtos;

namespace Triepe.Domain.Dtos.ProductDtos
{
    public class ProductResponseDto : BaseDto
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public List<PictureResponseDto> Pictures { get; set; }
    }
}
