namespace VerticalSliceArchitecture.Common.Registrations;

public static class SwaggerRegistration
{
    public static IApplicationBuilder UseSwaggerPage(this IApplicationBuilder app)
    {
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/openapi/v1.json", "OpenAPI V1");
        });

        return app;
    }
}