using FluentValidation;
using VerticalSliceArchitecture.Common.Endpoints;

namespace VerticalSliceArchitecture.Features;

public static class FeatureRegistration
{
    public static IServiceCollection RegisterFeatures(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(IFeatureMarker).Assembly);

        services.AddEndpoints();

        return services;
    }

    public static IApplicationBuilder UseFeatures(this WebApplication app)
    {
        app.MapEndpoints();

        return app;
    }
}
