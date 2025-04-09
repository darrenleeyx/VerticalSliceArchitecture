using VerticalSliceArchitecture.Common.Pipelines;

namespace VerticalSliceArchitecture;

public static class MiddlewareRegistration
{
    public static IApplicationBuilder UseMiddlewares(this IApplicationBuilder app)
    {
        app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

        return app;
    }
}
