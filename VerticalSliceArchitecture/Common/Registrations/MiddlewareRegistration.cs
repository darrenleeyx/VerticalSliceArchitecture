using VerticalSliceArchitecture.Common.Pipelines;

namespace VerticalSliceArchitecture.Common.Registrations;

public static class MiddlewareRegistration
{
    public static IApplicationBuilder UseMiddlewares(this IApplicationBuilder app)
    {
        app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

        return app;
    }
}
