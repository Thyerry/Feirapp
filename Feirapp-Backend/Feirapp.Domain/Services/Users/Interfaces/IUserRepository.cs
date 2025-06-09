using Feirapp.Entities.Entities;

namespace Feirapp.Domain.Services.Users.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email, CancellationToken ct);
    Task InsertAsync(User user, CancellationToken ct);
    void UpdateAsync(User user, CancellationToken ct);
}