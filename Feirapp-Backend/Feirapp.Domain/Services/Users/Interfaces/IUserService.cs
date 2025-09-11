using Feirapp.Domain.Services.Users.Methods.CreateUser;
using Feirapp.Domain.Services.Users.Methods.Login;

namespace Feirapp.Domain.Services.Users.Interfaces;

public interface IUserService
{
    Task CreateUserAsync(CreateUserCommand command, CancellationToken ct);
    Task<LoginResponse> LoginAsync(LoginRequest request, CancellationToken ct);
}