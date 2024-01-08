using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Triepe.Domain.Exceptions
{
    public class NotFoundException : CustomException
    {
        public NotFoundException(string message)
            : base(message)
        {
            StatusCode = System.Net.HttpStatusCode.NotFound;
        }
    }
}
