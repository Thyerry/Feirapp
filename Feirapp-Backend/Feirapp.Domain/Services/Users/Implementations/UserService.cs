using Feirapp.Domain.Mappers;
using Feirapp.Domain.Services.UnitOfWork;
using Feirapp.Domain.Services.Users.Interfaces;
using Feirapp.Domain.Services.Users.Methods.CreateUser;
using Feirapp.Domain.Services.Users.Methods.Login;
using Feirapp.Domain.Services.Utils;
using Feirapp.Entities.Enums;
using FluentValidation;
using FluentValidation.Results;

namespace Feirapp.Domain.Services.Users.Implementations;

public class UserService(IUnitOfWork uow) : IUserService
{
    public async Task CreateUserAsync(CreateUserCommand command, CancellationToken ct)
    {
        command.Validate();

        if (await uow.UserRepository.GetByEmailAsync(command.Email, ct) != null)
        {
            throw new ValidationException([
                new ValidationFailure(nameof(command.Email), "The user name cannot be null or empty.", command.Email)
            ]);
        }

        var user = command.ToEntity();
        
        user.PasswordSalt = PasswordHasher.GenerateSalt();
        user.Password = PasswordHasher.ComputeHash(user.Password, user.PasswordSalt);
        user.CreatedAt = DateTime.UtcNow;
        user.Status = UserStatus.Active;
        
        await uow.UserRepository.InsertAsync(user, ct);
        await uow.SaveChangesAsync(ct);
    }

    public async Task<LoginResponse> LoginAsync(LoginRequest request, CancellationToken ct)
    {
        var user = await uow.UserRepository.GetByEmailAsync(request.Email, ct);
        if (user == null)
        {
            throw new ValidationException([
                new ValidationFailure("Email or Password", "The user email or password is incorrect.")
            ]);
        }

        var passwordHash = PasswordHasher.ComputeHash(request.Password, user.PasswordSalt);
        if (user.Password != passwordHash)
        {
            throw new ValidationException([
                new ValidationFailure("Email or Password", "The user email or password is incorrect.")
            ]);
        }

        user.LastLogin = DateTime.UtcNow;
        uow.UserRepository.UpdateAsync(user, ct);

        return user.ToLoginResponse();
    }
}