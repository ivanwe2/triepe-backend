using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System;
using Triepe.Domain.Exceptions;
using Triepe.Api.Extensions;

namespace Triepe.Api.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        private const string PATH_NOT_FOUND_MESSAGE = "Request path was not found!";
        public ExceptionHandlingMiddleware(
            RequestDelegate next, 
            ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);

                if (httpContext.Response.StatusCode == 404)
                {
                    _logger.LogError(PATH_NOT_FOUND_MESSAGE);

                    await HandleExceptionAsync(httpContext, HttpStatusCode.NotFound, PATH_NOT_FOUND_MESSAGE);
                }
            }
            catch (CustomException ex)
            {
                _logger.LogError($"Exception occured: {ex}");

                await HandleExceptionAsync(httpContext, ex.StatusCode, ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception occured: {ex}");

                await HandleExceptionAsync(httpContext, HttpStatusCode.InternalServerError, "");
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
            }.CustomJsonSerialization());
        }
    }
}
