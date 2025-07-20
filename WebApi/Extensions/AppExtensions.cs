using Microsoft.AspNetCore.Builder;

using WebApi.Middlewares;

namespace WebApi.Extensions;

public static class AppExtensions
{
    public static void UseSwaggerExtension(this IApplicationBuilder app)
    {
        _ = app.UseSwagger();
        _ = app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "CleanArchitecture.WebApi");
        });
    }
    public static void UseErrorHandlingMiddleware(this IApplicationBuilder app)
    {
        _ = app.UseMiddleware<ErrorHandlerMiddleware>();
    }
}
