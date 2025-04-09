using Microsoft.EntityFrameworkCore;
using VerticalSliceArchitecture.Common.Abstractions;

namespace VerticalSliceArchitecture.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddPersistence();

        return services;
    }

    private static IServiceCollection AddPersistence(this IServiceCollection services)
    {
        services.AddDbContext<AppDbContext>(options =>
        {
            // Just for testing purposes
            options.UseInMemoryDatabase("AppDb");
        });

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<AppDbContext>());

        services.Scan(scan => scan
            .FromAssemblyOf<IInfrastructureMarker>()
            .AddClasses(classes => classes
                .Where(type => type.Name.EndsWith("Repository")))
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        return services;
    }
}
