using System;
using System.Threading;
using System.Threading.Tasks;
using Feirapp.Entities.Enums;
using Feirapp.Infrastructure.Configuration;
using Feirapp.Infrastructure.Repository;
using Feirapp.Tests.Fixtures;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Xunit;

namespace Feirapp.Tests.IntegrationTest.Infrastructure;

[Collection("integration-tests-collection")]
public class UserRepositoryTests : IAsyncLifetime
{
    private readonly BaseContext _context;
    private const string ConnectionString = "Host=localhost;Port=5433;Database=feirapp-test-db;Username=feirapp-test-user;Password=feirapp-test-password;Include Error Detail=true;";

    public UserRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<BaseContext>()
            .UseNpgsql(ConnectionString)
            .Options;
        _context = new BaseContext(options);
    }

    public Task InitializeAsync()
    {
        return Task.CompletedTask;
    }

    public async Task DisposeAsync()
    {
        _context.ChangeTracker.Clear();
        await _context.Database.ExecuteSqlRawAsync("DELETE FROM users;");
        await _context.SaveChangesAsync();
    }
    
    #region Insert
    
    [Fact]
    public async Task InsertAsync_ShouldAddUser_WhenValid()
    {
        // Arrange
        var repo = new UserRepository(_context);
        var user = Fake.User(email: "john.doe@example.com", name: "John Doe", status: UserStatus.Active);

        // Act
        await repo.InsertAsync(user, CancellationToken.None);
        await _context.SaveChangesAsync();

        // Assert
        var dbUser = await _context.Users.FindAsync(user.Id);
        dbUser.Should().NotBeNull();
        dbUser!.Email.Should().Be(user.Email);
        dbUser.Name.Should().Be(user.Name);
        dbUser.Status.Should().Be(UserStatus.Active);
    }

    [Fact]
    public async Task InsertAsync_ShouldThrow_WhenEmailIsNull()
    {
        // Arrange
        var repo = new UserRepository(_context);
        var user = Fake.User(email: null);
        user.Email = null!; 

        // Act
        await repo.InsertAsync(user, CancellationToken.None);
        var act = async () => await _context.SaveChangesAsync();

        // Assert
        var ex = await act.Should().ThrowAsync<DbUpdateException>();
        ex.Which.InnerException.Should().BeOfType<PostgresException>()
            .Which.SqlState.Should().Be("23502"); 
    }

    [Fact]
    public async Task InsertAsync_ShouldThrow_WhenEmailExceedsMaxLength()
    {
        // Arrange
        var repo = new UserRepository(_context);
        var longLocal = new string('a', 260);
        var user = Fake.User(email: longLocal + "@example.com");

        // Act
        await repo.InsertAsync(user, CancellationToken.None);
        var act = async () => await _context.SaveChangesAsync();

        // Assert
        var ex = await act.Should().ThrowAsync<DbUpdateException>();
        ex.Which.InnerException.Should().BeOfType<PostgresException>()
            .Which.SqlState.Should().Be("22001");
    }
    
    #endregion Insert
    
    #region GetByEmail 
    
    [Fact]
    public async Task GetByEmailAsync_ShouldReturnUser_WhenExists()
    {
        // Arrange
        var repo = new UserRepository(_context);
        var user = Fake.User(email: "alice@example.com", name: "Alice");
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        // Act
        var result = await repo.GetByEmailAsync(user.Email, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(user.Id);
        result.Email.Should().Be(user.Email);
    }

    [Fact]
    public async Task GetByEmailAsync_ShouldReturnNull_WhenNotExists()
    {
        // Arrange
        var repo = new UserRepository(_context);

        // Act
        var result = await repo.GetByEmailAsync("missing@example.com", CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetByEmailAsync_ShouldBeCaseSensitive_ByDefault()
    {
        // Arrange
        var repo = new UserRepository(_context);
        var user = Fake.User(email: "CaseTest@Example.com");
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        // Act
        var resultLower = await repo.GetByEmailAsync("casetest@example.com", CancellationToken.None);

        // Assert
        resultLower.Should().BeNull();
    }

    [Fact]
    public async Task InsertAsync_ShouldPersistUtcDateTimes()
    {
        // Arrange
        var repo = new UserRepository(_context);
        var created = new DateTime(2021, 5, 1, 13, 45, 0, DateTimeKind.Utc);
        var lastLogin = new DateTime(2022, 6, 2, 10, 30, 0, DateTimeKind.Utc);
        var user = Fake.User(email: "utc@example.com", createdAt: created, lastLogin: lastLogin);

        // Act
        await repo.InsertAsync(user, CancellationToken.None);
        await _context.SaveChangesAsync();
        var dbUser = await _context.Users.FindAsync(user.Id);

        // Assert
        dbUser.Should().NotBeNull();
        dbUser!.CreatedAt.Kind.Should().Be(DateTimeKind.Utc);
        dbUser.LastLogin.Kind.Should().Be(DateTimeKind.Utc);
        dbUser.CreatedAt.Should().Be(created);
        dbUser.LastLogin.Should().Be(lastLogin);
    }
    
    #endregion GetByEmail
}