using System.Diagnostics;
using System.Text;
using System.Text.Json;
using Serilog;

namespace Feirapp.API.Middlewares;

public class RequestLoggingMiddleware(RequestDelegate next)
{
    public async Task Invoke(HttpContext context)
    {
        var sw = new Stopwatch();
        sw.Start();

        var traceId = Activity.Current?.TraceId.ToString() ?? Guid.NewGuid().ToString();

        context.Items["TraceId"] = traceId;
        var requestBody = await ReadRequestBody(context);
        try
        {
            await next(context);

            sw.Stop();
            
            Log.Information("HTTP Request Completed {@Log}",
                new
                {
                    timestamp = DateTime.UtcNow,
                    level = "Information",
                    message = "Request completed",
                    traceId,
                    request = new
                    {
                        method = context.Request.Method,
                        path = context.Request.Path,
                        query = context.Request.QueryString.ToString(),
                        body = requestBody,
                        ip = context.Connection.RemoteIpAddress?.ToString()
                    },
                    response = new
                    {
                        statusCode = context.Response.StatusCode,
                        elapsedMs = sw.ElapsedMilliseconds
                    },
                    user = GetUser(context)
                }
            );
        }
        catch (Exception ex)
        {
            sw.Stop();
            
            Log.Error(ex, "HTTP Request Error {@Log}",
                new
                {
                    timestamp = DateTime.UtcNow,
                    level = "Error",
                    message = "Unhandled exception",
                    traceId,
                    request = new
                    {
                        method = context.Request.Method,
                        path = context.Request.Path,
                        body = requestBody,
                        query = context.Request.QueryString.ToString()
                    },
                    exception = new
                    {
                        type = ex.GetType().Name,
                        message = ex.Message,
                        stackTrace = ex.StackTrace
                    }
                }
            );

            await ExceptionHandlerMiddleware.HandleExceptionAsync(context, ex);
        }
    }
    
    private static async Task<string> ReadRequestBody(HttpContext context)
    {
        var body = "";

        if (context.Request.Method is "POST" or "PUT" or "PATCH")
        {
            context.Request.EnableBuffering();
            using (var reader = new StreamReader(context.Request.Body, Encoding.UTF8, leaveOpen: true, detectEncodingFromByteOrderMarks: false))
            {
                body = await reader.ReadToEndAsync();
            }
            
            if (body.Length > 10_000)
                body = body[..10_000] + "...(truncated)";

            if (!string.IsNullOrWhiteSpace(body))
            {
                body = Sanitize(body);
            }
            
            context.Request.Body.Position = 0;
        }

        return body;
    }
    
    private static string Sanitize(string body)
    {
        try
        {
            var json = JsonSerializer.Deserialize<Dictionary<string, object>>(body);

            if (json == null)
                return body;

            var sensitiveFields = new[] { "password", "token", "secret", "confirmPassword" };

            foreach (var field in sensitiveFields)
            {
                if (json.ContainsKey(field))
                    json[field] = "***REDACTED***";
            }

            return JsonSerializer.Serialize(json);
        }
        catch
        {
            return body;
        }
    }
    
    private static object? GetUser(HttpContext context)
    {
        if (context.User?.Identity?.IsAuthenticated != true)
            return null;
        
        return new
        {
            id = context.User.FindFirst("id")?.Value,
            email = context.User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress")?.Value,
            name = context.User.FindFirst("name")?.Value
        };
    }
}