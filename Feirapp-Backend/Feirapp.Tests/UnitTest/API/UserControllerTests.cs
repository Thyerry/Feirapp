using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Feirapp.API.Controllers;
using Feirapp.API.Helpers.Response;
using Feirapp.Domain.Services.Users.Interfaces;
using Feirapp.Domain.Services.Users.Methods.CreateUser;
using Feirapp.Domain.Services.Users.Methods.Login;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NSubstitute;
using Xunit;

namespace Feirapp.Tests.UnitTest.API;

public class UserControllerTests
{
    private static IConfiguration BuildJwtConfig()
    {
        var inMemorySettings = new Dictionary<string, string?>
        {
            {"JwtSettings:SecretKey", "ThisIsASecretKeyForTests_1234567890"},
            {"JwtSettings:Issuer", "Feirapp.Tests"},
            {"JwtSettings:Audience", "Feirapp.Tests.Audience"},
            {"JwtSettings:TokenLifetime", "00:30:00"}
        };

        return new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings!)
            .Build();
    }
    
    #region CreateUser

    [Fact]
    public async Task CreateUser_OnSuccess_ReturnsApiResponseTrue()
    {
        // Arrange
        var userService = Substitute.For<IUserService>();
        var config = Substitute.For<IConfiguration>();
        var command = new CreateUserCommand("Test User", "test@example.com", "password", "password");
        var sut = new UserController(userService, config);
        
        // Act
        var result = await sut.CreateUser(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<OkObjectResult>();
        var objectResult = result as OkObjectResult;
        objectResult?.StatusCode.Should().Be(200);
        objectResult?.Value.Should().BeOfType<ApiResponse<bool>>();
        var apiResponse = objectResult?.Value as ApiResponse<bool>;
        apiResponse?.Data.Should().BeTrue(); 
        await userService.Received(1).CreateUserAsync(command, Arg.Any<CancellationToken>());
    }

    #endregion CreateUser
    
    #region Login

    [Fact]
    public async Task Login_OnSuccess_ReturnsApiResponseWithToken()
    {
        // Arrange
        var userService = Substitute.For<IUserService>();
        var config = BuildJwtConfig();
        var request = new LoginRequest("test@example.com", "password");
        var loginResponse = new LoginResponse("user-id-1", "Test User", request.Email);
        userService.LoginAsync(request, Arg.Any<CancellationToken>()).Returns(loginResponse);
        var sut = new UserController(userService, config);

        // Act
        var result = await sut.Login(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<OkObjectResult>();
        var objectResult = result as OkObjectResult;
        objectResult?.StatusCode.Should().Be(200);
        objectResult?.Value.Should().BeOfType<ApiResponse<TokenResponse>>();
        var apiResponse = objectResult?.Value as ApiResponse<TokenResponse>;
        apiResponse?.Data.Should().NotBeNull(); 
        apiResponse?.Data?.Token.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task Login_OnSuccess_InvokesUserServiceWithRequest()
    {
        // Arrange
        var userService = Substitute.For<IUserService>();
        var config = BuildJwtConfig();
        var request = new LoginRequest("john.doe@example.com", "strongPassword!");
        userService.LoginAsync(request, Arg.Any<CancellationToken>())
            .Returns(new LoginResponse("user-id-2", "John Doe", request.Email));
        var sut = new UserController(userService, config);

        // Act
        await sut.Login(request, CancellationToken.None);

        // Assert
        await userService.Received(1).LoginAsync(request, Arg.Any<CancellationToken>());
    }
    
    #endregion Login
}