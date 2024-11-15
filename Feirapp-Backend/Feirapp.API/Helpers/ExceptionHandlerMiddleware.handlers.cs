using Feirapp.API.Helpers.Response;
using FluentValidation;

namespace Feirapp.API.Helpers;

public partial class ExceptionHandlerMiddleware
{
    private static Task HandleValidationException(HttpContext context, ValidationException validationException)
    {
        context.Response.StatusCode = StatusCodes.Status400BadRequest;

        var errors = validationException.Errors.Select(e => e.ErrorMessage).ToList();

        var response = ApiResponseFactory.Failure<object>("Validation error", errors);
        return context.Response.WriteAsJsonAsync(response);
    }

    private static Task HandleInvalidOperationException(HttpContext context, InvalidOperationException invalidOperationException)
    {
        context.Response.StatusCode = StatusCodes.Status400BadRequest;
        
        var response = ApiResponseFactory.Failure<object>(invalidOperationException.Message);

        return context.Response.WriteAsJsonAsync(response);
    }
}