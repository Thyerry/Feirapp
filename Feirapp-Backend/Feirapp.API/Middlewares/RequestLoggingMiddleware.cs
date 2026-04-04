using System.Diagnostics;
using System.Text;
using System.Text.Json;
using Serilog;
using Serilog.Context;

namespace Feirapp.API.Middlewares;

public class RequestLoggingMiddleware(RequestDelegate next)
{
    public async Task Invoke(HttpContext context)
    {
        var sw = Stopwatch.StartNew();

        var endpoint = context.GetEndpoint() as RouteEndpoint;
        var routePattern =
            endpoint?.RoutePattern?.RawText
            ?? context.Request.Path.Value;

        var requestBody = await ReadRequestBody(context);

        var request = new
        {
            method = context.Request.Method,
            path = context.Request.Path.Value,
            query = context.Request.QueryString.Value,
            ip = context.Connection.RemoteIpAddress?.ToString(),
            body = string.IsNullOrWhiteSpace(requestBody) ? null : requestBody
        };
        
        using (LogContext.PushProperty("traceId", context.TraceIdentifier))
        using (LogContext.PushProperty("user", context.User.FindFirst("id")?.Value))
        using (LogContext.PushProperty("endpoint", $"{context.Request.Method} {routePattern}"))
        {
            try
            {
                await next(context);

                sw.Stop();

                var response = new
                {
                    statusCode = context.Response.StatusCode,
                    elapsedMs = sw.ElapsedMilliseconds
                };

                Log.Information(
                    "Request completed {@request} {@response}",
                    request,
                    response
                );
            }
            catch (Exception ex)
            {
                sw.Stop();

                var response = new
                {
                    statusCode = 500,
                    elapsedMs = sw.ElapsedMilliseconds
                };

                Log.Error(
                    ex,
                    "Request failed {@request} {@response}",
                    request,
                    response
                );

                throw;
            }
        }
    }

    private static async Task<string> ReadRequestBody(HttpContext context)
    {
        var body = "";

        if (context.Request.Method is "POST" or "PUT" or "PATCH")
        {
            context.Request.EnableBuffering();
            using (var reader = new StreamReader(context.Request.Body, Encoding.UTF8, leaveOpen: true,
                       detectEncodingFromByteOrderMarks: false))
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

            var sensitiveFields = new[] { "password", "confirmPassword" , "token", "secret" };

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
}