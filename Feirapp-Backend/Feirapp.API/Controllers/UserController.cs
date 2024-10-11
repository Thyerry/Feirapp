using Feirapp.API.Helpers;
using Feirapp.Domain.Services.Users.Commands;
using Feirapp.Domain.Services.Users.Interfaces;
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
    public async Task<IActionResult> CreateUser([FromBody] CreateUserCommand command, CancellationToken ct)
    {
        await userService.CreateUserAsync(command, ct);

        return Created();
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginCommand command, CancellationToken ct)
    {
        var result = await userService.LoginAsync(command, ct);
        return Ok(new { Token = JwtHelper.GenerateJwtToken(result, config.GetSection("JwtSettings")) });
    }

    [HttpPost]
    public async Task<IActionResult> TestAuthEndpoint()
    {
        await Task.CompletedTask;
        return Ok();
    }
}