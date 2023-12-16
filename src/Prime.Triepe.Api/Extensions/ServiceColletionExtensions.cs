﻿using AutoMapper.Extensions.ExpressionMapping;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Prime.Triepe.Data;

namespace Prime.Triepe.Api.Extenions
{
    public static class ServiceColletionExtensions
    {
        public static IServiceCollection AddTriepeDbContext(
            this IServiceCollection services, string connString) 
            => services.AddDbContext<TriepeDbContext>(
                options => options.UseSqlServer(connString));

        public static IServiceCollection AddTriepeAutomapper(this IServiceCollection services) 
            => services.AddAutoMapper(mc =>
                {
                    mc.AddExpressionMapping();
                    mc.AddProfile(new Data.Mapper.MapperProfile());
                });

        public static IServiceCollection AddTriepeSwagger(this IServiceCollection services)
            => services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Triepe API", Version = "v1" });
            });
    }
}
