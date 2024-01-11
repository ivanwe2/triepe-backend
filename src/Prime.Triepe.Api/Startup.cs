using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Prime.Triepe.Api.Extenions;
using Serilog;
using System.Reflection;
using Triepe.Api.AutofacModules;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

builder.Host
    .UseServiceProviderFactory(new AutofacServiceProviderFactory())
    .ConfigureContainer<ContainerBuilder>(autofacBuilder =>
    {
        autofacBuilder.RegisterModule<ServicesModule>();
        autofacBuilder.RegisterModule<RepositoriesModule>();
        autofacBuilder.RegisterModule<FactoriesModule>();
        autofacBuilder.RegisterModule<ProvidersModule>();
    })
    .UseSerilog((context,services, configuration) =>
        configuration
            .ReadFrom.Configuration(context.Configuration)
            .ReadFrom.Services(services));

builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddValidatorsFromAssembly(Assembly.Load("Triepe.Domain"));

builder.Services.AddControllers()
    .AddMvcOptions(options => options.SuppressAsyncSuffixInActionNames = false);

builder.Services.AddEndpointsApiExplorer()
                .AddTriepeSwagger()
                .AddTriepeAutomapper()
                .AddTriepeDbContext(builder.Configuration.GetConnectionString("Default"))
                .AddCors()
                .AddLogging()
                .AddHttpContextAccessor()
                .AddProblemDetails();

var app = builder.Build();

app.UseCustomExceptionMiddleware();

app.UseAuthentication();
app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI();

if (app.Environment.IsDevelopment())
{
    app.UseSerilogRequestLogging();
}

app.MapControllers();
app.MapSwagger();
app.UpdateDatabase();

app.Run();
