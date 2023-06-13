using FluentValidation;
using Newtonsoft.Json;
using System.Net;

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

            var result = "";
            switch (exception)
            {
                case ValidationException e:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    result = JsonConvert.SerializeObject(e.Errors);
                    break;

                default:
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    result = JsonConvert.SerializeObject(new { message = exception.Message });
                    break;
            }
            await response.WriteAsync(result);
        }
    }
}