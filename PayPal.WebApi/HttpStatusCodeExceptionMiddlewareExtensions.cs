using Microsoft.AspNetCore.Builder;

namespace PayPal.WebApi
{
    public static class HttpStatusCodeExceptionMiddlewareExtensions
    {

        // Extension method used to add the middleware to the HTTP request pipeline.
        public static IApplicationBuilder UseHttpStatusCodeExceptionMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<HttpStatusCodeExceptionMiddleware>();
        }
    }
}