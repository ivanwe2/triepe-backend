using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Triepe.Api.Middleware;
using Triepe.Data;

namespace Prime.Triepe.Api.Extenions
{
    public static class AppBuilderExtensions
    {
        public static void UpdateDatabase(this IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope();
            var context = serviceScope.ServiceProvider.GetRequiredService<TriepeDbContext>();
            context.Database.Migrate();
        }

        public static void UseCustomExceptionMiddleware(this IApplicationBuilder app)
            => app.UseMiddleware<ExceptionHandlingMiddleware>();
    }
}
