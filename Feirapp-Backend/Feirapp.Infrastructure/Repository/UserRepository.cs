using Feirapp.Domain.Services.Users.Interfaces;
using Feirapp.Entities.Entities;
using Feirapp.Infrastructure.Configuration;
using Microsoft.EntityFrameworkCore;

namespace Feirapp.Infrastructure.Repository;

public class UserRepository(BaseContext context) : IUserRepository
{
    public async Task<User?> GetByEmailAsync(string email, CancellationToken ct)
    {
        return await context.Users.FirstOrDefaultAsync(u => u.Email == email, ct);
    }

    public async Task InsertAsync(User user, CancellationToken ct)
    {
        await context.Users.AddAsync(user, ct);
    }

    public void UpdateAsync(User user, CancellationToken ct)
    {
        context.Users.Update(user);
    }
}