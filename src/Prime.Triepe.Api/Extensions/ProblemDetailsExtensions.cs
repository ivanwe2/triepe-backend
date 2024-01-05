using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Triepe.Domain.Exceptions;

namespace Triepe.Api.Extensions
{
    public static class ProblemDetailsExtensions
    {
        public static IResult ToProblemDetails(this ProblemDetails problemDetails, ICustomException exception)
        {

        }
    }
}
