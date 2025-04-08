using LangLearningAPI.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;

namespace LangLearningAPI.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(
            RequestDelegate next,
            ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
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

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            _logger.LogError(exception, "An error occurred");

            context.Response.ContentType = "application/json";

            var response = new
            {
                Status = (int)HttpStatusCode.InternalServerError,
                Title = "Internal Server Error",
                Detail = exception.Message,
                ErrorCode = "UNKNOWN_ERROR",
                Errors = (Dictionary<string, string[]>?)null
            };

            if (exception is ApiException apiException)
            {
                context.Response.StatusCode = apiException.StatusCode;
                response = new
                {
                    Status = apiException.StatusCode,
                    Title = apiException.Message,
                    Detail = apiException.Message,
                    ErrorCode = apiException.ErrorCode,
                    Errors = (Dictionary<string, string[]>?)null
                };
            }
            else if (exception is ValidationException validationException)
            {
                context.Response.StatusCode = validationException.StatusCode;
                response = new
                {
                    Status = validationException.StatusCode,
                    Title = validationException.Message,
                    Detail = validationException.Message,
                    ErrorCode = validationException.ErrorCode,
                    Errors = validationException.Errors
                };
            }

            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}