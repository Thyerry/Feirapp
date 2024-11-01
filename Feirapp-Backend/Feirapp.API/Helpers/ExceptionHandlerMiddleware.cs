using FluentValidation;
using System.Net;
using System.Text.Json;
using Feirapp.API.Helpers.Response;

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
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        var response = new ApiResponse<object>
        (
            Status: "error",
            Message: "An unexpected error ocurred",
            Data: default,
            Errors: [exception.Message]
        );

        return context.Response.WriteAsJsonAsync(response);
    }
    
    private string Process(Exception exception, HttpContext context)
    {
        var response = context.Response;
        response.ContentType = "application/json";
        switch (exception)
        {
            case ValidationException e:
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                var errorObj = new
                {
                    Message = "There was some validation errors",
                    Errors = e.Errors.Select(x => new
                    {
                        x.PropertyName,
                        x.ErrorMessage,
                        x.AttemptedValue,
                    })
                };

                return JsonSerializer.Serialize(errorObj);

            case InvalidOperationException e:
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                return JsonSerializer.Serialize(new
                {
                    message = e.Message,
                });

            default:
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return JsonSerializer.Serialize(new
                {
                    message = exception.Message,
                    innerException = exception.InnerException?.Message,
                });
        }
    }
}