using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Serilog;

namespace Ambev.DeveloperEvaluation.Common.Logging;

public class ExceptionLoggingMiddleware
{
  private readonly RequestDelegate _next;
  private readonly ILogger _logger;

  public ExceptionLoggingMiddleware(RequestDelegate next, ILogger logger)
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
      _logger.Error(ex, "An unhandled exception has occurred");
      await HandleExceptionAsync(context, ex);
    }
  }

  private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
  {
    context.Response.ContentType = "application/json";
    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

    var response = new
    {
      error = new
      {
        message = "An error occurred while processing your request",
        details = exception.Message,
        type = exception.GetType().Name
      }
    };

    await context.Response.WriteAsync(JsonSerializer.Serialize(response));
  }
}
