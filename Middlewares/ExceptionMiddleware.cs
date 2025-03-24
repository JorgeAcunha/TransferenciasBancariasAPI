using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace TransferenciasBancarias.Middlewares
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
                _logger.LogError(ex, "Unhandled Exception");

                context.Response.ContentType = "application/json";

                // Loguear más detalle para nosotros (en consola)
                var detailedError = new
                {
                    message = ex.Message,
                    stackTrace = ex.StackTrace
                };
                _logger.LogError("Detailed Error: {Error}", JsonSerializer.Serialize(detailedError));

                if (ex is UnauthorizedAccessException || context.Response.StatusCode == (int)HttpStatusCode.Unauthorized)
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    var result = JsonSerializer.Serialize(new { error = "No autorizado. Token inválido o ausente." });
                    await context.Response.WriteAsync(result);
                }
                else
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    var result = JsonSerializer.Serialize(new { error = "Ha ocurrido un error inesperado." });
                    await context.Response.WriteAsync(result);
                }
            }

        }
    }
}
