using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Ambev.DeveloperEvaluation.Common.Logging;

public class RequestLoggingMiddleware
{
  private readonly RequestDelegate _next;
  private readonly ILogger<RequestLoggingMiddleware> _logger;

  public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
  {
    _next = next;
    _logger = logger;
  }

  public async Task InvokeAsync(HttpContext context)
  {
    var sw = Stopwatch.StartNew();
    try
    {
      await _next(context);
    }
    finally
    {
      sw.Stop();
      var elapsed = sw.ElapsedMilliseconds;

      var logMessage = new
      {
        Method = context.Request.Method,
        Path = context.Request.Path,
        QueryString = context.Request.QueryString.ToString(),
        StatusCode = context.Response.StatusCode,
        ElapsedMilliseconds = elapsed,
        User = context.User?.Identity?.Name ?? "anonymous",
        IP = context.Connection.RemoteIpAddress?.ToString(),
        UserAgent = context.Request.Headers["User-Agent"].ToString()
      };

      if (context.Response.StatusCode >= 500)
      {
        _logger.LogError("Request completed with error: {@LogMessage}", logMessage);
      }
      else if (context.Response.StatusCode >= 400)
      {
        _logger.LogWarning("Request completed with warning: {@LogMessage}", logMessage);
      }
      else
      {
        _logger.LogInformation("Request completed successfully: {@LogMessage}", logMessage);
      }
    }
  }
}
