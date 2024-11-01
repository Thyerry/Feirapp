using FluentValidation;
using System.Net;
using Feirapp.API.Helpers.Response;

namespace Feirapp.API.Helpers;

public partial class ExceptionHandlerMiddleware
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

        switch (exception)
        {
            case ValidationException validationException:
                return HandleValidationException(context, validationException);
            case InvalidOperationException invalidOperationException:
                return HandleInvalidOperationException(context, invalidOperationException);
        }

        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        var genericResponse = new ApiResponse<object>
        (
            Status: "error",
            Message: "An unexpected error ocurred",
            Data: default,
            Errors: [exception.Message]
        );

        return context.Response.WriteAsJsonAsync(genericResponse);
    }
}