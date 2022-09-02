using GlobalErrorHandling.Logger;
using Microsoft.AspNetCore.Diagnostics;
using System.Net;

namespace GlobalErrorHandling.EceptionHandler
{
    public static class ExceptionMiddlewareExtensions
    {
        public static void ConfigureExceptionHandler(this IApplicationBuilder app, ILoggerManager logger)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";

                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {
                        logger.LogError($"Something went wrong: {contextFeature.Error}");
                        await context.Response.WriteAsync(new ErrorDetails
                        {
                            StatusCode = (int)HttpStatusCode.InternalServerError,
                            Message = "Internal Server Error"
                        }.ToString());
                    }
                });
            });
        }
    }
}
