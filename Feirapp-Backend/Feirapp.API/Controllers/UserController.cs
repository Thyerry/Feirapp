using Feirapp.API.Helpers;
using Feirapp.Domain.Services.Users.Commands;
using Feirapp.Domain.Services.Users.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Feirapp.API.Controllers;

[ApiController]
[Route("api/user")]
public class UserController(IUserService userService, IConfiguration config) : Controller
{
    [HttpPost("create-user")]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserCommand command, CancellationToken ct)
    {
        await userService.CreateUserAsync(command, ct);

        return Created();
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginCommand command, CancellationToken ct)
    {
        var result = await userService.LoginAsync(command, ct);
        return Ok(result with
        {
            Token = JwtHelper.GenerateJwtToken(result, config.GetSection("JwtSettings"))
        });
    }
}
