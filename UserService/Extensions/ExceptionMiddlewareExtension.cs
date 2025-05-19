using Entities.ErrorModel;
using Microsoft.AspNetCore.Diagnostics;
using Entities.Exceptions;

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
                        context.Response.StatusCode = contextFeature.Error switch
                        {
                            UnauthorizedException => StatusCodes.Status401Unauthorized,
                            BadRequestException => StatusCodes.Status400BadRequest,
                            ForbiddenException => StatusCodes.Status403Forbidden,
                            NotFoundException => StatusCodes.Status404NotFound,
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
