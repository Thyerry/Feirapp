using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Feirapp.Domain.Services.UnitOfWork;
using Feirapp.Domain.Services.Users.Implementations;
using Feirapp.Domain.Services.Users.Interfaces;
using Feirapp.Domain.Services.Users.Methods.CreateUser;
using Feirapp.Domain.Services.Users.Methods.Login;
using Feirapp.Domain.Services.Utils;
using Feirapp.Entities.Entities;
using Feirapp.Entities.Enums;
using FluentAssertions;
using FluentValidation;
using NSubstitute;
using Xunit;

namespace Feirapp.Tests.UnitTest.Domain;

public class UserServiceTests
{
    private static (UserService sut, IUnitOfWork uow, IUserRepository userRepo) BuildSut()
    {
        var uow = Substitute.For<IUnitOfWork>();
        var userRepo = Substitute.For<IUserRepository>();
        uow.UserRepository.Returns(userRepo);
        uow.SaveChangesAsync(Arg.Any<CancellationToken>()).Returns(1);
        var sut = new UserService(uow);
        return (sut, uow, userRepo);
    }

    #region CreateUserAsync

    [Fact]
    public async Task CreateUserAsync_OnSuccess_InsertsUser_And_SavesChanges()
    {
        // Arrange
        var (sut, uow, userRepo) = BuildSut();
        userRepo
            .GetByEmailAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns((User?)null);
        var command = new CreateUserCommand("John Doe", "john@example.com", "Abcd1234!", "Abcd1234!");

        // Act
        await sut.CreateUserAsync(command, CancellationToken.None);

        // Assert
        await userRepo.Received(1).InsertAsync(Arg.Any<User>(), Arg.Any<CancellationToken>());
        await uow.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task CreateUserAsync_OnSuccess_MapsAndSetsDerivedFieldsCorrectly()
    {
        // Arrange
        var (sut, _, userRepo) = BuildSut();
        userRepo.GetByEmailAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns((User?)null);
        var command = new CreateUserCommand("Jane Smith", "jane@example.com", "Abcd1234!", "Abcd1234!");

        User? captured = null;
        userRepo
            .When(r => r.InsertAsync(Arg.Any<User>(), Arg.Any<CancellationToken>()))
            .Do(ci => captured = ci.Arg<User>());

        // Act
        var before = DateTime.UtcNow;
        await sut.CreateUserAsync(command, CancellationToken.None);
        var after = DateTime.UtcNow;

        // Assert
        captured.Should().NotBeNull();
        captured!.Name.Should().Be(command.Name);
        captured.Email.Should().Be(command.Email);
        captured.Id.Should().NotBe(Guid.Empty);
        captured.Status.Should().Be(UserStatus.Active);
        captured.CreatedAt.Kind.Should().Be(DateTimeKind.Utc);
        captured.CreatedAt.Should().BeOnOrAfter(before).And.BeOnOrBefore(after);
    }

    [Fact]
    public async Task CreateUserAsync_OnSuccess_HashesPassword_And_GeneratesSalt()
    {
        // Arrange
        var (sut, _, userRepo) = BuildSut();
        userRepo.GetByEmailAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns((User?)null);
        const string plainPassword = "Qwerty12!";
        var command = new CreateUserCommand("Alice", "alice@example.com", plainPassword, plainPassword);

        User? captured = null;
        userRepo
            .When(r => r.InsertAsync(Arg.Any<User>(), Arg.Any<CancellationToken>()))
            .Do(ci => captured = ci.Arg<User>());

        // Act
        await sut.CreateUserAsync(command, CancellationToken.None);

        // Assert
        captured.Should().NotBeNull();
        captured!.PasswordSalt.Should().NotBeNullOrWhiteSpace();
        captured.Password.Should().NotBe(plainPassword);
        var expectedHash = PasswordHasher.ComputeHash(plainPassword, captured.PasswordSalt);
        captured.Password.Should().Be(expectedHash);
    }

    [Fact]
    public async Task CreateUserAsync_OnSuccess_Calls_GetByEmail_With_CommandEmail()
    {
        // Arrange
        var (sut, _, userRepo) = BuildSut();
        userRepo.GetByEmailAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns((User?)null);
        var command = new CreateUserCommand("Bob", "bob@example.com", "Abcd1234!", "Abcd1234!");

        // Act
        await sut.CreateUserAsync(command, CancellationToken.None);


        await userRepo.Received(1).GetByEmailAsync(command.Email, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task CreateUserAsync_WhenEmailAlreadyExists_ReturnsFail_And_DoesNotInsert()
    {
        // Arrange
        var (sut, uow, userRepo) = BuildSut();
        userRepo.GetByEmailAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(new User { Id = Guid.NewGuid(), Email = "taken@example.com" });
        var command = new CreateUserCommand("Taken User", "taken@example.com", "Abcd1234!", "Abcd1234!");

        // Act
        var result = await sut.CreateUserAsync(command, CancellationToken.None);

        // Assert
        result.Success.Should().BeFalse();
        result.Message.Should().NotBeNull();
        result.Message!.ToLowerInvariant().Should().Contain("already in use");
        
        await userRepo.DidNotReceive().InsertAsync(Arg.Any<User>(), Arg.Any<CancellationToken>());
        await uow.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Theory]
    [MemberData(nameof(CreateUserCommandData))]
    public async Task CreateUserAsync_WhenNameIsInvalid_ReturnsFail_And_SkipsRepository(CreateUserCommand command)
    {
        // Arrange
        var (sut, uow, userRepo) = BuildSut();

        // Act
        var result = await sut.CreateUserAsync(command, CancellationToken.None);
        
        // Assert
        result.Success.Should().BeFalse();
        result.Message.Should().NotBeNullOrWhiteSpace();

        await userRepo.DidNotReceive().GetByEmailAsync(Arg.Any<string>(), Arg.Any<CancellationToken>());
        await userRepo.DidNotReceive().InsertAsync(Arg.Any<User>(), Arg.Any<CancellationToken>());
        await uow.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    public static IEnumerable<object[]> CreateUserCommandData()
    {
        // Purpose: Validate Name is required.
        yield return [new CreateUserCommand(" ", "x@example.com", "Abcd1234!", "Abcd1234!")];
        
        // Purpose: Validate Email is required.
        yield return [new CreateUserCommand("User", " ", "Abcd1234!", "Abcd1234!")];
        
        // Purpose: Enforce minimum password length of 8.
        yield return [new CreateUserCommand("User", "u@example.com", "Abc12!", "Abc12!")];
        
        // Purpose: Password must contain at least one uppercase letter.
        yield return [new CreateUserCommand("User", "u@example.com", "abcd1234!", "abcd1234!")];
        
        // Purpose: Password must contain at least one lowercase letter.
        yield return [new CreateUserCommand("User", "u@example.com", "ABCD1234!", "ABCD1234!")];
        
        // Purpose: Password must contain at least one digit.
        yield return [new CreateUserCommand("User", "u@example.com", "Abcdefg!", "Abcdefg!")];
        
        // Purpose: Password must contain at least one special character.
        yield return [new CreateUserCommand("User", "u@example.com", "Abcdefg1", "Abcdefg1")];
        
        // Purpose: ConfirmPassword must match Password.
        yield return [new CreateUserCommand("User", "u@example.com", "Abcd1234!", "Xbcd1234!")];
    }
    
    #endregion

    #region LoginAsync

    private static User BuildUserWithPassword(string name, string email, string plainPassword)
    {
        var salt = PasswordHasher.GenerateSalt();
        var hash = PasswordHasher.ComputeHash(plainPassword, salt);
        return new User
        {
            Id = Guid.NewGuid(),
            Name = name,
            Email = email,
            PasswordSalt = salt,
            Password = hash,
            CreatedAt = DateTime.UtcNow,
            Status = UserStatus.Active,
            LastLogin = default
        };
    }

    [Fact]
    public async Task LoginAsync_OnSuccess_ReturnsMappedLoginResponse()
    {
        // Arrange
        var (sut, _, userRepo) = BuildSut();
        var user = BuildUserWithPassword("Alice", "alice@example.com", "Abcd1234!");
        userRepo.GetByEmailAsync(user.Email, Arg.Any<CancellationToken>()).Returns(user);

        // Act
        var result = await sut.LoginAsync(new LoginRequest(user.Email, "Abcd1234!"), CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value!.Id.Should().Be(user.Id.ToString());
        result.Value!.Name.Should().Be(user.Name);
        result.Value!.Email.Should().Be(user.Email);
    }

    [Fact]
    public async Task LoginAsync_OnSuccess_UpdatesLastLogin_And_SavesChangesOnce()
    {
        // Arrange
        var (sut, uow, userRepo) = BuildSut();
        var user = BuildUserWithPassword("Bob", "bob@example.com", "Xyz12345!");
        userRepo.GetByEmailAsync(user.Email, Arg.Any<CancellationToken>()).Returns(user);
        
        // Act
        var before = DateTime.UtcNow;
        await sut.LoginAsync(new LoginRequest(user.Email, "Xyz12345!"), CancellationToken.None);
        var after = DateTime.UtcNow;

        // Assert
        user.LastLogin.Kind.Should().Be(DateTimeKind.Utc);
        user.LastLogin.Should().BeOnOrAfter(before).And.BeOnOrBefore(after);
        await uow.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task LoginAsync_OnSuccess_Calls_GetByEmail_With_RequestEmail()
    {
        // Arrange
        var (sut, _, userRepo) = BuildSut();
        var user = BuildUserWithPassword("Carol", "carol@example.com", "Qwerty12!");
        userRepo.GetByEmailAsync(user.Email, Arg.Any<CancellationToken>()).Returns(user);

        // Act
        await sut.LoginAsync(new LoginRequest(user.Email, "Qwerty12!"), CancellationToken.None);

        // Assert
        await userRepo.Received(1).GetByEmailAsync(user.Email, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task LoginAsync_WhenUserNotFound_ReturnsFail_And_DoesNotSave()
    {
        // Arrange
        var (sut, uow, userRepo) = BuildSut();
        const string email = "missing@example.com";
        userRepo.GetByEmailAsync(email, Arg.Any<CancellationToken>()).Returns((User?)null);

        // Act
        var result = await sut.LoginAsync(new LoginRequest(email, "Whatever1!"), CancellationToken.None);

        // Assert
        result.Success.Should().BeFalse();
        result.Message.Should().NotBeNull();
        result.Message!.ToLowerInvariant().Should().Contain("incorrect");
        await uow.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
        await userRepo.Received(1).GetByEmailAsync(email, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task LoginAsync_WhenPasswordMismatch_ReturnsFail_And_DoesNotSave_Or_UpdateLastLogin()
    {
        // Arrange
        var (sut, uow, userRepo) = BuildSut();
        var user = BuildUserWithPassword("Dave", "dave@example.com", "Correct1!");
        var originalLastLogin = new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        user.LastLogin = originalLastLogin;
        userRepo.GetByEmailAsync(user.Email, Arg.Any<CancellationToken>()).Returns(user);

        // Act
        var result = await sut.LoginAsync(new LoginRequest(user.Email, "Wrong1!"), CancellationToken.None);

        // Assert
        result.Success.Should().BeFalse();
        result.Message.Should().NotBeNull();
        result.Message!.ToLowerInvariant().Should().Contain("incorrect");
        user.LastLogin.Should().Be(originalLastLogin);
        await uow.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
        await userRepo.DidNotReceive().InsertAsync(Arg.Any<User>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task LoginAsync_OnSuccess_DoesNotCallRepositoryInsertOrUpdate()
    {
        // Arrange
        var (sut, uow, userRepo) = BuildSut();
        var user = BuildUserWithPassword("Eve", "eve@example.com", "Secret9!");
        userRepo.GetByEmailAsync(user.Email, Arg.Any<CancellationToken>()).Returns(user);

        // Act
        await sut.LoginAsync(new LoginRequest(user.Email, "Secret9!"), CancellationToken.None);

        // Assert
        await uow.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
        await userRepo.DidNotReceive().InsertAsync(Arg.Any<User>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task LoginAsync_OnFailure_WithEmptyEmail_ReturnsFail_And_DoesNotSave()
    {
        // Arrange
        var (sut, uow, userRepo) = BuildSut();
        const string emptyEmail = " ";
        userRepo.GetByEmailAsync(emptyEmail, Arg.Any<CancellationToken>()).Returns((User?)null);

        // Act
        var result = await sut.LoginAsync(new LoginRequest(emptyEmail, "Anything1!"), CancellationToken.None);

        // Assert
        result.Success.Should().BeFalse();
        result.Message.Should().NotBeNull();
        result.Message!.ToLowerInvariant().Should().Contain("incorrect");
        await uow.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
        await userRepo.Received(1).GetByEmailAsync(emptyEmail, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task LoginAsync_OnFailure_WithWhitespacePassword_ReturnsFail_And_DoesNotSave()
    {
        // Arrange
        var (sut, uow, userRepo) = BuildSut();
        var user = BuildUserWithPassword("Frank", "frank@example.com", "ValidPass1!");
        userRepo.GetByEmailAsync(user.Email, Arg.Any<CancellationToken>()).Returns(user);

        // Act
        var result = await sut.LoginAsync(new LoginRequest(user.Email, " "), CancellationToken.None);

        // Assert
        result.Success.Should().BeFalse();
        result.Message.Should().NotBeNull();
        result.Message!.ToLowerInvariant().Should().Contain("incorrect");
        await uow.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    #endregion
}