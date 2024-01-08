using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Triepe.Api.Extensions
{
    public static class ProblemDetailsExtensions
    {
        public static string CustomJsonSerialization(this ProblemDetails problemDetails)
            => JsonSerializer.Serialize(problemDetails, new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
    }
}
