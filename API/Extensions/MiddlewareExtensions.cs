using System.Net;
using AspNetCoreRateLimit;
using Microsoft.AspNetCore.Diagnostics;
using Service.SharedDTOs;

namespace API.Extensions
{
    public static class MiddlewareExtensions
    {
        public static void AddMidddlewares(this WebApplication app)
        {
            app.UseExceptionHandler(appBuilder =>
            {
                appBuilder.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";

                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();

                    if (contextFeature != null)
                    {
                        var exception = contextFeature.Error;

                        var responseModel = ResponseModelDto<NoContent>.Fail(exception.Message, HttpStatusCode.InternalServerError);

                        await context.Response.WriteAsJsonAsync(responseModel);
                    }
                });
            });

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseIpRateLimiting();
            app.UseHttpsRedirection();

            app.MapControllers();
        }
    }
}