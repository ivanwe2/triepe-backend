using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Prime.Triepe.Api.Extenions;
using Serilog;
using Serilog.Events;
using System.Collections.Generic;
using System.Reflection;
using Triepe.Api.AutofacModules;
using FluentValidation;

//Log.Logger = new LoggerConfiguration()
//    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
//    .Enrich.FromLogContext()
//    .WriteTo.Console()
//    .CreateBootstrapLogger();

var builder = WebApplication.CreateBuilder(args);
builder.Host
    .UseServiceProviderFactory(new AutofacServiceProviderFactory())
    .ConfigureContainer<ContainerBuilder>(autofacBuilder =>
    {
        autofacBuilder.RegisterModule<ServicesModule>();
        autofacBuilder.RegisterModule<RepositoriesModule>();
        autofacBuilder.RegisterModule<FactoriesModule>();
        autofacBuilder.RegisterModule<ProvidersModule>();
    });
    //.UseSerilog((context, services, configuration) =>
    //    configuration
    //      .ReadFrom.Configuration(context.Configuration)
    //      .ReadFrom.Services(services));

builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddValidatorsFromAssembly(Assembly.Load("Triepe.Domain"));

builder.Services.AddControllers();
//builder.Services.ConfigureCustomModelStateResponseFactory();

//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme,
//                    options => builder.Configuration.Bind("JwtSettings", options));

builder.Services.AddEndpointsApiExplorer()
                .AddTriepeSwagger()
                .AddTriepeAutomapper()
                .AddTriepeDbContext(builder.Configuration.GetConnectionString("Default"))
                .AddCors()
                .AddLogging()
                .AddHttpContextAccessor();

//builder.Services.AddSwaggerGen(c => { c.EnableAnnotations(); });

var app = builder.Build();

//app.ConfigureCustomExceptionMiddleware();

app.UseAuthentication();
app.UseAuthorization();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger()
       .UseSwaggerUI(c =>
       {
           c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
       });
}

app.MapControllers();
app.MapSwagger();
app.UpdateDatabase();

app.Run();
