namespace Feirapp.Domain.Services.Utils;

public sealed class Result<T>
{
    public bool Success { get; init; }
    public string? Message { get; init; }
    public T? Value { get; init; }

    public static Result<T> Ok(T value, string? message = null) => new()
    {
        Success = true,
        Value = value,
        Message = message
    };
    public static Result<T> Fail(string message) => new()
    {
        Success = false,
        Message = message
    };
}
