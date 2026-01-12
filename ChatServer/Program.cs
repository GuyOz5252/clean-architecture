using System.Reflection;
using System.Text;
using ChatServer.Domain.Abstract;
using ChatServer.Infrastructure.Authentication;
using FastEndpoints;
using FastEndpoints.Swagger;
using ChatServer.Api.Middlewares;
using ChatServer.Application.Pipeline;
using ChatServer.Infrastructure;
using ChatServer.ServiceDefaults;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NSwag;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddMediator(options =>
{
    options.ServiceLifetime = ServiceLifetime.Scoped;
    options.PipelineBehaviors = [typeof(ValidationPipelineBehavior<,>)];
});
builder.Services.AddValidatorsFromAssemblyContaining(typeof(ValidationPipelineBehavior<,>));

builder.Services.Configure<JwtConfiguration>(builder.Configuration.GetSection("JwtConfiguration"));
var jwtConfig = builder.Configuration.GetSection("JwtConfiguration").Get<JwtConfiguration>();

builder.Services.AddScoped<ITokenGenerator, JwtTokenGenerator>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasherService>();
builder.Services.AddScoped(typeof(PasswordHasher<>));

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

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtConfig?.Issuer,
        ValidAudience = jwtConfig?.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig?.Secret ?? string.Empty))
    };
});

builder.Services.AddAuthorization();
builder.Services.AddFastEndpoints();

builder.Services.SwaggerDocument(options =>
{
    options.ShortSchemaNames = true;
    options.DocumentSettings = s =>
    {
        s.DocumentName = "v1";
        s.Title = "ChatServer API";
        s.Version = "v1";
    };
});

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
    app.UseSwaggerUI(options => options.DocumentTitle = "ChatServer API");
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await dbContext.Database.MigrateAsync();
}

await app.RunAsync();
