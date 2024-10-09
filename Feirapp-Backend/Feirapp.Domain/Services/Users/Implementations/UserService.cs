using Feirapp.Domain.Mappers;
using Feirapp.Domain.Services.Users.Commands;
using Feirapp.Domain.Services.Users.Interfaces;

namespace Feirapp.Domain.Services.Users.Implementations;

public class UserService(IUserRepository userRepository) : IUserService
{
    public async Task CreateUserAsync(CreateUserCommand command, CancellationToken ct)
    {
        try
        {
            await userRepository.InsertAsync(command.ToEntity(), ct);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}