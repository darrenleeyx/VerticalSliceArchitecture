using Serilog;
using VerticalSliceArchitecture;
using VerticalSliceArchitecture.Features;
using VerticalSliceArchitecture.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
var host = builder.Host;
var services = builder.Services;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

try
{
    host.UseSerilog();

    services
        .AddProblemDetails()
        .AddOpenApi()
        .AddFeatures()
        .AddInfrastructure();

    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        app.Services.Seed();
    }

    app.MapOpenApi();

    app
        .UseFeatures()
        .UseSwaggerPage()
        .UseMiddlewares();

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

