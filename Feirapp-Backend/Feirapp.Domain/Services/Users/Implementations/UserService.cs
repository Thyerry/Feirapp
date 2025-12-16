using Feirapp.Domain.Mappers;
using Feirapp.Domain.Services.UnitOfWork;
using Feirapp.Domain.Services.Users.Interfaces;
using Feirapp.Domain.Services.Users.Methods.CreateUser;
using Feirapp.Domain.Services.Users.Methods.Login;
using Feirapp.Domain.Services.Utils;
using Feirapp.Entities.Enums;
using FluentValidation;

namespace Feirapp.Domain.Services.Users.Implementations;

public class UserService(IUnitOfWork uow) : IUserService
{
    public async Task<Result<bool>> CreateUserAsync(CreateUserCommand command, CancellationToken ct)
    {
        try
        {
            command.Validate();
        }
        catch (ValidationException ex)
        {
            var message = ex.Errors != null && ex.Errors.Any()
                ? string.Join(" | ", ex.Errors.Select(e => e.ErrorMessage))
                : ex.Message;
            return Result<bool>.Fail(message);
        }

        if (await uow.UserRepository.GetByEmailAsync(command.Email, ct) != null)
            return Result<bool>.Fail("The user email is already in use.");

        var user = command.ToEntity();
        
        user.Id = GuidGenerator.Generate();
        user.PasswordSalt = PasswordHasher.GenerateSalt();
        user.Password = PasswordHasher.ComputeHash(user.Password, user.PasswordSalt);
        user.CreatedAt = DateTime.UtcNow;
        user.Status = UserStatus.Active;
        
        await uow.UserRepository.InsertAsync(user, ct);
        await uow.SaveChangesAsync(ct);
        return Result<bool>.Ok(true, "User created successfully.");
    }

    public async Task<Result<LoginResponse>> LoginAsync(LoginRequest request, CancellationToken ct)
    {
        var user = await uow.UserRepository.GetByEmailAsync(request.Email, ct);
        if (user == null)
            return Result<LoginResponse>.Fail("The user email or password is incorrect.");

        var passwordHash = PasswordHasher.ComputeHash(request.Password, user.PasswordSalt);
        if (user.Password != passwordHash)
            return Result<LoginResponse>.Fail("The user email or password is incorrect.");

        user.LastLogin = DateTime.UtcNow;
        await uow.SaveChangesAsync(ct);
        return Result<LoginResponse>.Ok(user.ToLoginResponse());
    }
}