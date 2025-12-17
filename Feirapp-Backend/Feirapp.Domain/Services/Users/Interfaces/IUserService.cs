using Feirapp.Domain.Services.Users.Methods.CreateUser;
using Feirapp.Domain.Services.Users.Methods.Login;
using Feirapp.Domain.Services.Utils;

namespace Feirapp.Domain.Services.Users.Interfaces;

public interface IUserService
{
    Task<Result<bool>> CreateUserAsync(CreateUserCommand command, CancellationToken ct);
    Task<Result<LoginResponse>> LoginAsync(LoginRequest request, CancellationToken ct);
}