using System.Net;

namespace WebAppDemo.Api.Middelwares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
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
            HttpStatusCode statusCode = HttpStatusCode.InternalServerError;

            switch (exception)
            {
                case ArgumentException:
                case FormatException:  
                    statusCode = HttpStatusCode.BadRequest;
                    _logger.LogError(exception, "Format or argument error occurred");
                    break;

                case NullReferenceException:
                    _logger.LogError(exception, "Null Refrence exception");
                    break;

                default:
                    _logger.LogError(exception, "Unhandled exception");
                    break;
            }

            var response = new
            {
                StatusCode = (int)statusCode,
                exception.Message
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            await context.Response.WriteAsJsonAsync(response);
        }
    }

}
