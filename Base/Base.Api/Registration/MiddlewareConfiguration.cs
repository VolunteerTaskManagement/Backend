using Base.Application.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Scalar.AspNetCore;

namespace Base.Api.Registration
{
    public static class MiddlewareConfiguration
    {
        public static IApplicationBuilder UseCustomMiddlewares(this WebApplication app, WebApplicationBuilder builder)
        {
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.MapScalarApiReference(options =>
                {
                    options.Servers =
                    [
                        new ScalarServer("https://localhost:7121/")
                    ];
                });
            }


            app.UseExceptionHandler(
                new ExceptionHandlerOptions()
                {
                    AllowStatusCode404Response = true,
                    ExceptionHandlingPath = "/error"
                }
            );

            app.UseHsts();
            app.UseHttpsRedirection();

            app.UseErrorHandlerMiddleware();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.DefaultModelsExpandDepth(-1);
            });

            app.UseCors();

            app.UseMiddleware<JwtCookieMiddleware>();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            return app;
        }
    }
}
