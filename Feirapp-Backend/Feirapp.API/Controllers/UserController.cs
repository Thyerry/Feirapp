using Feirapp.API.Helpers;
using Feirapp.API.Helpers.Response;
using Feirapp.Domain.Services.Users.Commands;
using Feirapp.Domain.Services.Users.Interfaces;
using Feirapp.Domain.Services.Users.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Feirapp.API.Controllers;

[ApiController]
[Authorize]
[Route("api/user")]
public class UserController(IUserService userService, IConfiguration config) : Controller
{
    [HttpPost("create-user")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<bool>), 201)]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserCommand command, CancellationToken ct)
    {
        await userService.CreateUserAsync(command, ct);

        return Created(nameof(command), ApiResponseFactory.Success(true));
    }

    [HttpPost("login")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<TokenResponse>), 200)]
    public async Task<IActionResult> Login([FromBody] LoginCommand command, CancellationToken ct)
    {
        var result = await userService.LoginAsync(command, ct);
        var response = new TokenResponse(JwtHelper.GenerateJwtToken(result, config.GetSection("JwtSettings")));
        return Ok(ApiResponseFactory.Success(response));
    }

    [HttpPost]
    public async Task<IActionResult> TestAuthEndpoint()
    {
        await Task.CompletedTask;
        return Ok();
    }
}