using Serilog;
using VerticalSliceArchitecture;
using VerticalSliceArchitecture.Features;
using VerticalSliceArchitecture.Infrastructure;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);
    {
        var host = builder.Host;
        var services = builder.Services;
        var configuration = builder.Configuration;

        host.UseSerilog();

        services
            .AddProblemDetails()
            .AddOpenApi()
            .AddFeatures()
            .AddInfrastructure(configuration);
    }

    var app = builder.Build();
    {
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
}
catch (Exception ex)
{
    Log.Logger.Error(ex, "An error occurred during application startup.");
    Environment.Exit(1);
}
finally
{
    Log.CloseAndFlush();
}

