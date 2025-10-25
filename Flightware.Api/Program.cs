using System.Net.Http.Headers;
using System.Reflection;
using Common.Core.Abstract;
using FastEndpoints;
using FastEndpoints.Swagger;
using Flightware.Api.Configuration;
using Flightware.Application.Pipeline;
using Flightware.Domain.Abstract;
using Flightware.Domain.Models;
using Flightware.Infrastructure;
using Flightware.ServiceDefaults;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Winton.Extensions.Configuration.Consul;
using Winton.Extensions.Configuration.Consul.Parsers;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddConsul("Archive/", options =>
{
    options.ConsulConfigurationOptions = o =>
    {
        o.Address = new Uri(Environment.GetEnvironmentVariable("CONSUL_ADDRESS")!);
    };
    options.ReloadOnChange = true;
});


builder.Services.AddOptions<List<MaterialType>>()
    .Configure<IConfiguration>((options, configuration) =>
    {
        options.Clear();
        var section = configuration.GetSection("MaterialTypes");
        options.AddRange(from materialSection in section.GetChildren()
            let name = materialSection.Key
            let orderParams = materialSection
                                  .GetSection("OrderParameters")
                                  .GetSection("OrderParameters")
                                  .Get<List<OrderParameter>>()
                              ?? throw new Exception()
            select new MaterialType { Name = name, OrderParameters = orderParams });
    });

var m = builder.Services.BuildServiceProvider().GetService<IOptionsMonitor<List<MaterialType>>>();
while (true)
{
    Console.WriteLine(nameof(RazorComponentsEndpointConventionBuilder));
    Console.WriteLine(m?.CurrentValue[0].OrderParameters.Count);
    await Task.Delay(TimeSpan.FromSeconds(1));
}

// builder.AddServiceDefaults();
// builder.Services.AddMediator(options =>
// {
//     options.ServiceLifetime = ServiceLifetime.Scoped;
//     options.PipelineBehaviors = [typeof(ValidationPipelineBehavior<,>)];
// });
// builder.Services.AddValidatorsFromAssemblyContaining(typeof(ValidationPipelineBehavior<,>));
// builder.Services.Scan(selector => selector
//     .FromAssemblyDependencies(Assembly.GetExecutingAssembly())
//     .AddClasses(c => c.AssignableToAny(
//         typeof(IUserRepository),
//         typeof(IUnitOfWork)))
//     .AsImplementedInterfaces()
//     .WithScopedLifetime());
// builder.Services.AddDbContext<ApplicationDbContext>(options =>
// {
//     options.UseNpgsql(builder.Configuration.GetConnectionString("flightware"), builderOptions =>
//     {
//         builderOptions.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName);
//     });
// });
// builder.Services.AddCors();
// builder.Services.AddAuthentication();
// builder.Services.AddAuthorization();
// builder.Services.AddFastEndpoints();
// builder.Services.SwaggerDocument(options => options.ShortSchemaNames = true);
//
// var app = builder.Build();
//
// app.MapServiceDefaults();
// app.UseCors();
// app.UseAuthentication();
// app.UseAuthorization();
// app.MapFastEndpoints(config =>
// {
//     config.Endpoints.RoutePrefix = "api";
//     config.Errors.UseProblemDetails();
// });
//
// if (app.Environment.IsDevelopment())
// {
//     app.UseOpenApi();
//     app.UseSwaggerUI(options => options.DocumentTitle = "Flightware API");
//     using var scope = app.Services.CreateScope();
//     var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
//     await dbContext.Database.MigrateAsync();
// }
//
// await app.RunAsync();
