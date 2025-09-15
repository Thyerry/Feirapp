namespace Feirapp.API.Helpers.Response;

public static class ApiResponseFactory
{
    public static ApiResponse<T> Success<T>(T data, string message = "Request successful")
    {
        return new ApiResponse<T>("success", message, data, null);
    }
    
    public static ApiResponse<T> Failure<T>(string message, List<string>? errors = null)
    {
        return new ApiResponse<T>("failure", message, default, errors ?? []);
    }
}