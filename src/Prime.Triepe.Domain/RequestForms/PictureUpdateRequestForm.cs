using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Triepe.Domain.RequestForms
{
    public class PictureUpdateRequestForm
    {
        public string FileName { get; set; }
        public IFormFile File { get; set; }
    }
}
