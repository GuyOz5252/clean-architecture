using System.Reflection;
using FastEndpoints;
using FastEndpoints.Swagger;
using ChatServer.Api.Middlewares;
using ChatServer.Application.Pipeline;
using ChatServer.Domain.Abstract;
using ChatServer.Infrastructure;
using ChatServer.ServiceDefaults;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

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
    options.UseNpgsql(builder.Configuration.GetConnectionString("chat-server-db"), builderOptions =>
    {
        builderOptions.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName);
    });
});
builder.Services.AddCors();
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<ProblemDetailsExceptionHandler>();
builder.Services.AddAuthentication();
builder.Services.AddAuthorization();
builder.Services.AddFastEndpoints();
builder.Services.SwaggerDocument(options => options.ShortSchemaNames = true);

var app = builder.Build();

app.MapServiceDefaults();
app.UseExceptionHandler();
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
