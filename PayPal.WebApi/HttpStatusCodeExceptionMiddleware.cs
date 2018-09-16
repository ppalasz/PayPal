using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PayPal.Business;
using PayPal.WebApi;

namespace PayPal.WebApi
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class HttpStatusCodeExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<HttpStatusCodeExceptionMiddleware> _logger;

        public HttpStatusCodeExceptionMiddleware(RequestDelegate next,ILoggerFactory loggerFactory)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _logger = loggerFactory?.CreateLogger<HttpStatusCodeExceptionMiddleware>() ?? throw new ArgumentNullException(nameof(loggerFactory));
        }

        public async Task Invoke(HttpContext context /* other dependencies */)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            int code = (int)HttpStatusCode.InternalServerError; // 500 if unexpected
            ApiException ex =new ApiException()
            {
                ErrorCode = code,
                Message = exception.Message 
            };

            if (exception is HttpStatusCodeException)
            {
                ex.ErrorCode = ((HttpStatusCodeException)exception).StatusCode;
            }
            
            var result = JsonConvert.SerializeObject(ex);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = ex.ErrorCode;

            return context.Response.WriteAsync(result);
        }

    }
}

