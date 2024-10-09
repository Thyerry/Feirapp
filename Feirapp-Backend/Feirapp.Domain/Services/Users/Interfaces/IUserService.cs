using Feirapp.Domain.Services.Users.Commands;

namespace Feirapp.Domain.Services.Users.Interfaces;

public interface IUserService
{
    Task CreateUserAsync(CreateUserCommand command, CancellationToken ct);
}