using System.Reflection;
using Common.Core.Abstract;
using FastEndpoints;
using FastEndpoints.Swagger;
using Flightware.Application.Pipeline;
using Flightware.Domain.Abstract;
using Flightware.Domain.Models;
using Flightware.Infrastructure;
using Flightware.ServiceDefaults;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Winton.Extensions.Configuration.Consul;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddConsul("Archive/MaterialTypes", options =>
{
    options.ConsulConfigurationOptions = o =>
    {
        o.Address = new Uri(Environment.GetEnvironmentVariable("CONSUL_ADDRESS")!);
    };
    options.ReloadOnChange = true;
});

builder.Services.AddOptions<List<MaterialType>>()
    .Bind(builder.Configuration.GetSection("MaterialTypes"));

builder.AddServiceDefaults();
builder.Services.AddMediator(options =>
{
    options.ServiceLifetime = ServiceLifetime.Scoped;
    options.PipelineBehaviors = [typeof(ValidationPipelineBehavior<,>)];
});
builder.Services.AddValidatorsFromAssemblyContaining(typeof(ValidationPipelineBehavior<,>));
builder.Services.Scan(selector => selector
    .FromAssemblyDependencies(Assembly.GetExecutingAssembly())
    .AddClasses(c => c.AssignableToAny(
        typeof(IUserRepository),
        typeof(IUnitOfWork)))
    .AsImplementedInterfaces()
    .WithScopedLifetime());
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("flightware"), builderOptions =>
    {
        builderOptions.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName);
    });
});
builder.Services.AddCors();
builder.Services.AddAuthentication();
builder.Services.AddAuthorization();
builder.Services.AddFastEndpoints();
builder.Services.SwaggerDocument(options => options.ShortSchemaNames = true);

var app = builder.Build();

app.MapServiceDefaults();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.MapFastEndpoints(config =>
{
    config.Endpoints.RoutePrefix = "api";
    config.Errors.UseProblemDetails();
});

if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUI(options => options.DocumentTitle = "Flightware API");
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await dbContext.Database.MigrateAsync();
}

await app.RunAsync();
