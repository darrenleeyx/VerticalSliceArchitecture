using Serilog;
using Serilog.Formatting.Compact;

namespace VerticalSliceArchitecture.Common.Registrations;

public static class SerilogRegistration
{
    public static IHostBuilder RegisterSerilog(this IHostBuilder host, IConfiguration configuration)
    {
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .Enrich.FromLogContext()
            .Enrich.WithEnvironmentName()
            .Enrich.WithMachineName()
            .Enrich.WithThreadId()
            .Enrich.WithCorrelationIdHeader()
            .Enrich.WithProperty("Application", nameof(VerticalSliceArchitecture))
            .WriteTo.Console()
            .WriteTo.File(
                formatter: new CompactJsonFormatter(),
                path: ".\\Logs\\Clef\\log-.json",
                rollingInterval: RollingInterval.Day)
            .CreateLogger();

        host.UseSerilog(Log.Logger);

        return host;
    }
}