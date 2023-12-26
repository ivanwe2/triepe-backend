using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Triepe.Domain.RequestForms
{
    public class PictureCreateRequestForm
    {
        public string FileName { get; set; }
        public IFormFile File { get; set; }
        public Guid ProductId { get; set; }
    }
}
