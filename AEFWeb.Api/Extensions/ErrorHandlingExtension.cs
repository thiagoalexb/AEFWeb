using AEFWeb.Api.Middlewares;
using Microsoft.AspNetCore.Builder;

namespace AEFWeb.Api.Extensions
{
    public static class ErrorHandlingExtension
    {
        public static IApplicationBuilder UseErrorHandlingMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ErrorHandlingMiddleware>();
        }
    }
}
