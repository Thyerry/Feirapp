namespace Feirapp.API.Helpers.Response;

public record ApiResponse<T>(string Status, string Message, T? Data, List<string>? Errors);
