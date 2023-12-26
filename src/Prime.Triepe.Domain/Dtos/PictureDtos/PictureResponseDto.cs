using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Triepe.Domain.Dtos.PictureDtos
{
    public class PictureResponseDto : BaseDto
    {
        public string FileName { get; set; }

        public string FilePath { get; set; }

        public string Content { get; set; }

    }
}
