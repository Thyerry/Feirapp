using Feirapp.Domain.Mappers;
using Feirapp.Domain.Services.Users.Commands;
using Feirapp.Domain.Services.Users.Interfaces;
using Feirapp.Domain.Services.Utils;
using FluentValidation;
using FluentValidation.Results;

namespace Feirapp.Domain.Services.Users.Implementations;

public class UserService(IUserRepository userRepository) : IUserService
{
    public async Task CreateUserAsync(CreateUserCommand command, CancellationToken ct)
    {
        command.Validate();

        if (await userRepository.GetByEmailAsync(command.Email, ct) != null)
        {
            throw new ValidationException([
                new ValidationFailure(nameof(command.Email), "The user name cannot be null or empty.", command.Email)
            ]);
        }

        var user = command.ToEntity();
        user.PasswordSalt = PasswordHasher.GenerateSalt();
        user.Password = PasswordHasher.ComputeHash(user.Password, user.PasswordSalt);
        await userRepository.InsertAsync(user, ct);
    }
}