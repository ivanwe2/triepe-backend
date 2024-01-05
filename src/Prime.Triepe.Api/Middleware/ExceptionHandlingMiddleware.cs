using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System;
using Triepe.Domain.Exceptions;

namespace Triepe.Api.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(
            RequestDelegate next, 
            ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            HttpStatusCode statusCode = HttpStatusCode.InternalServerError;
            string detail = "";

            try
            {
                await _next(httpContext);

                if (httpContext.Response.StatusCode == 404)
                {
                    statusCode = (HttpStatusCode)httpContext.Response.StatusCode;
                    detail = "Request path was not found";
                    await HandleExceptionAsync(httpContext, statusCode, detail);
                }
            }
            catch (Exception ex)
            {
                switch (ex)
                {
                    case ArgumentNullException:
                        statusCode = HttpStatusCode.BadRequest;
                        detail = ex.Message;
                        break;
                    case ValidationException:
                        statusCode = HttpStatusCode.BadRequest;
                        detail = ex.Message;
                        break;
                    case NotFoundException:
                        statusCode = HttpStatusCode.NotFound;
                        detail = ex.Message;
                        break;
                    case NoExistingValidatorException:
                        statusCode = HttpStatusCode.BadRequest;
                        detail = ex.Message;
                        break;
                    default:
                        statusCode = HttpStatusCode.InternalServerError;
                        detail = "Internal Server Error.";
                        break;
                }

                _logger.LogError($"Exception occured: {ex}");

                await HandleExceptionAsync(httpContext, statusCode, detail);
            }
        }
        private async Task HandleExceptionAsync(HttpContext context, HttpStatusCode code, string detail)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;

            await context.Response.WriteAsync(new ProblemDetails()
            {
                Type = "about:blank",
                Title = code.ToString(),
                Status = context.Response.StatusCode,
                Detail = detail,
                Instance = context.Request.Path
            }.ToString());
        }
    }
}
