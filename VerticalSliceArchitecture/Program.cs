using Serilog;
using VerticalSliceArchitecture.Common.Registrations;
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

        host.RegisterSerilog(configuration);

        services.AddProblemDetails()
                .AddOpenApi();

        services.RegisterFeatures()
                .RegisterInfrastructure(configuration);
    }

    var app = builder.Build();
    {
        if (app.Environment.IsDevelopment())
        {
            app.Services.Seed();
        }

        app.MapOpenApi();

        app.UseFeatures()
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

