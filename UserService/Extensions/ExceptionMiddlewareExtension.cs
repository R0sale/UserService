using Entities.ErrorModel;
using Microsoft.AspNetCore.Diagnostics;

namespace UserService.Extensions
{
    public static class ExceptionMiddlewareExtension
    {
        public static void ConfigureExceptionHandler(this WebApplication app)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.ContentType = "application/json";
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();

                    if (contextFeature != null)
                    {
                        context.Response.StatusCode = contextFeature switch
                        {
                            _ => StatusCodes.Status500InternalServerError
                        };

                        await context.Response.WriteAsync(
                            new ErrorDetails()
                            {
                                StatusCode = context.Response.StatusCode,
                                Message = contextFeature.Error.Message
                            }.ToString());
                    }
                });
            });
        }
    }
}
