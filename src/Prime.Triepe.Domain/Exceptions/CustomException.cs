using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Triepe.Domain.Exceptions
{
    public interface ICustomException
    {
        public HttpStatusCode StatusCode { get; }
        public string Message { get; }

    }
}
