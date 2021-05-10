using System;
using System.Threading.Tasks;
using DiffChecker.Errors;
using DiffChecker.Middleware.Interfaces;
using DiffChecker.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace DiffChecker.Middleware
{
    public class ExceptionHandlerMiddleware : IExceptionHandlerMiddleware
    {
        private readonly ILogger<ExceptionHandlerMiddleware> _logger;

        public ExceptionHandlerMiddleware(ILogger<ExceptionHandlerMiddleware> logger)
        {
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unexpected error: {ex}");
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            var errorResponse = HandleException(exception);

            context.Response.StatusCode = errorResponse.StatusCode;

            return context.Response.WriteAsync(JsonConvert.SerializeObject(errorResponse));
        }

        private static ErrorResponse HandleException(Exception exception)
        {
            if (exception is IDiffServiceException)
                return (exception as IDiffServiceException).ToErrorResponse();

            return new ErrorResponse
            {
                StatusCode = 500,
                Message = exception.Message
            };
        }
    }
}