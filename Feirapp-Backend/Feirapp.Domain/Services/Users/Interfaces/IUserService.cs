using Feirapp.Domain.Services.Users.Commands;
using Feirapp.Domain.Services.Users.Responses;

namespace Feirapp.Domain.Services.Users.Interfaces;

public interface IUserService
{
    Task CreateUserAsync(CreateUserCommand command, CancellationToken ct);
    Task<LoginResponse> LoginAsync(LoginCommand command, CancellationToken ct);
}