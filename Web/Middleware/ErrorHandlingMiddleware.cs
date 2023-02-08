using CliveBot.Application.Errors;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text.Json;

namespace CliveBot.Web.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex, _logger);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex, ILogger<ErrorHandlingMiddleware> logger)
        {
            RestException? restException;
            switch (ex)
            {
                case RestException re:
                    restException = re;
                    context.Response.StatusCode = (int)re.Code;
                    break;
                default:
                    string error = string.IsNullOrWhiteSpace(ex.Message) ? "Unknown Error" : ex.Message;
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    restException = new RestException(HttpStatusCode.InternalServerError, error);
                    break;
            }

            if (restException != null)
            {
                await context.Response.WriteAsJsonAsync(new { restException.Code, restException.Message });
            } else
            {
                await context.Response.WriteAsJsonAsync(new { Code = HttpStatusCode.InternalServerError, Message = "Unknown Error" });
            }
        }

        public static async Task HandleExceptionAsync(HttpContext context)
        {
            var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();

            RestException? restException;
            Exception? e = exceptionHandlerPathFeature?.Error;
            switch (e)
            {
                case RestException re:
                    restException = re;
                    context.Response.StatusCode = (int)re.Code;
                    break;
                default:
                    string error = string.IsNullOrWhiteSpace(e?.Message) ? "Unknown Error" : e.Message;
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    restException = new RestException(HttpStatusCode.InternalServerError, error);
                    break;
            }

            if (restException != null)
            {
                await context.Response.WriteAsJsonAsync(new { restException.Code, restException.Message });
            }
            else
            {
                await context.Response.WriteAsJsonAsync(new { Code = HttpStatusCode.InternalServerError, Message = "Unknown Error" });
            }
        }
    }
}
