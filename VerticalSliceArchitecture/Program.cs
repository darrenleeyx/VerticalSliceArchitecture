using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Serilog;
using VerticalSliceArchitecture.Common.Abstractions;
using VerticalSliceArchitecture.Common.Abstractions.Repositories;
using VerticalSliceArchitecture.Common.Endpoints;
using VerticalSliceArchitecture.Common.Pipelines;
using VerticalSliceArchitecture.Infrastructure;
using VerticalSliceArchitecture.Infrastructure.Users;

var builder = WebApplication.CreateBuilder(args);
var host = builder.Host;
var services = builder.Services;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

try
{
    host.UseSerilog();

    services.AddProblemDetails();
    services.AddOpenApi();
    services.AddEndpoints();
    services.AddValidatorsFromAssembly(typeof(Program).Assembly);

    services.AddDbContext<AppDbContext>(options =>
    {
        // Just for testing purposes
        options.UseInMemoryDatabase("AppDb");
    });

    services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<AppDbContext>());
    services.AddScoped<IReadOnlyUserRepository, UserRepository>();
    services.AddScoped<IReadWriteUserRepository, UserRepository>();

    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        app.Services.Seed();
    }

    app.MapOpenApi();

    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "OpenAPI V1");
    });

    app.MapEndpoints();
    app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

    app.UseHttpsRedirection();

    app.Run();
}
catch (Exception ex)
{
    Log.Logger.Error(ex, "An error occurred during application startup.");
}
finally
{
    Log.CloseAndFlush();
}

