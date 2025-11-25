using Feirapp.API.Helpers;
using Feirapp.API.Helpers.Response;
using Feirapp.Domain.Services.Users.Interfaces;
using Feirapp.Domain.Services.Users.Methods.CreateUser;
using Feirapp.Domain.Services.Users.Methods.Login;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Feirapp.API.Controllers;

[ApiController]
[Authorize]
[Route("api/user")]
public class UserController(IUserService userService, IConfiguration config) : Controller
{
    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserCommand command, CancellationToken ct)
    {
        await userService.CreateUserAsync(command, ct);
        return Ok(ApiResponseFactory.Success(true));
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken ct)
    {
        var result = await userService.LoginAsync(request, ct);
        var response = new TokenResponse(JwtHelper.GenerateJwtToken(result, config.GetSection("JwtSettings")));
        return Ok(ApiResponseFactory.Success(response));
    }

    [HttpPost("test-auth")]
    public async Task<IActionResult> TestAuthEndpoint()
    {
        await Task.CompletedTask;
        return Ok();
    }
}