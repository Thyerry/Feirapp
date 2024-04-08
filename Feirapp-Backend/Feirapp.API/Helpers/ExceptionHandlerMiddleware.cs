using FluentValidation;
using System.Net;
using System.Text.Json;

namespace Feirapp.API.Helpers;

public class ExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception exception)
        {
            var response = context.Response; 
            response.ContentType = "application/json";

            string result;
            switch (exception)
            {
                case ValidationException e:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    result = JsonSerializer.Serialize(e.Errors);
                    break;

                default:
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    result = JsonSerializer.Serialize(new { message = "We're having some technical issues right now. Try again later." });
                    break;
            }

            await response.WriteAsync(result);
        }
    }
}