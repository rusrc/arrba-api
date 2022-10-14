using System;
using System.Net;
using System.Threading.Tasks;
using Arrba.Exceptions;
using Arrba.Services.Logger;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Arrba.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogService _logService;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogService logService)
        {
            this.next = next;
            _logService = logService;
        }

        public async Task Invoke(HttpContext context /* other dependencies */)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                _logService.Error(ex.Message, ex);
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var code = HttpStatusCode.InternalServerError; // 500 if unexpected

            if (exception is BusinessLogicException)
            {
                code = HttpStatusCode.BadRequest;
            }

            var result = JsonConvert.SerializeObject(new { exception.Message });
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;

            return context.Response.WriteAsync(result);
        }
    }
}
