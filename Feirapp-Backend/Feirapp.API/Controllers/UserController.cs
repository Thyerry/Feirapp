using Feirapp.Domain.Services.Users.Commands;
using Feirapp.Domain.Services.Users.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Feirapp.API.Controllers;

[ApiController]
[Route("api/user")]
public class UserController(IUserService userService) : Controller
{
    [HttpPost("create-user")]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserCommand command, CancellationToken ct)
    {
        await userService.CreateUserAsync(command, ct);

        return Created();
    }
}