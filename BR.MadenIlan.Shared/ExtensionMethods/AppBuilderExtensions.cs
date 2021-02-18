
using System.Threading.Tasks;

using BR.MadenIlan.Shared.Models;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;

using Newtonsoft.Json;

namespace BR.MadenIlan.Shared.ExtensionMethods
{
    public static class AppBuilderExtensions
    {
        public static void UseCustomExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(config =>
            {
                config.Run(async context =>
                {
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    context.Response.ContentType = "application/json";
                    IExceptionHandlerPathFeature error = context.Features.Get<IExceptionHandlerPathFeature>();
                    if (error != null)
                    {
                        System.Exception ex = error.Error;
                        ErrorDto dto = new ErrorDto
                        {
                            StatusCode = StatusCodes.Status500InternalServerError,
                            Path = error.Path
                        };
                        dto.Errors.Add(ex.Message);
                        if (ex is CustomException)
                            dto.IsShow = true;
                        await context.Response.WriteAsync(JsonConvert.SerializeObject(dto));
                    }

                });
            });
        }

        public static void UseDelayRequestDevelopment(this IApplicationBuilder app)
        {
            app.Use(async (context, next) =>
            {
                await Task.Delay(1000);
                await next.Invoke();
            });
        }
    }
}
