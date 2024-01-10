using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Triepe.Domain.Exceptions
{
    public class NoExistingValidatorException : CustomException
    {
        public NoExistingValidatorException(Type dtoType) 
            : base($"Requested validator for type: {dtoType}, was not found!") 
        {
            StatusCode = HttpStatusCode.InternalServerError;
        }
    }
}
