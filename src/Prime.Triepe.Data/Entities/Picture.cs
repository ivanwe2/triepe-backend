using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Triepe.Data.Entities.Base;

namespace Triepe.Data.Entities
{
    public class Picture : BaseEntity
    {
        public byte[] Bytes { get; set; }
        public string Description { get; set; }
        public string FileExtension { get; set; }
        public decimal Size { get; set; }
        public Guid ProductId { get; set; }

        public Product Product { get; set; }
    }
}
