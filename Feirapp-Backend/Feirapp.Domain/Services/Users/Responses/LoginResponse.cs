namespace Feirapp.Domain.Services.Users.Responses;

public record LoginResponse(string Id, string Name, string Email, string? Token = null);