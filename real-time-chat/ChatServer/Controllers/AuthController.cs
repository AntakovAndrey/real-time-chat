using ChatServer.Dto;
using ChatServer.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ChatServer.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : Controller
{
    private readonly IUserService _userService;

    public AuthController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto, CancellationToken cancellationToken)
    {
        try
        {
            var foundUser = await _userService.AuthenticateAsync(loginDto, cancellationToken);
            var token = _userService.GenerateToken(foundUser);
            return Ok(new 
                {
                    token,
                    user = foundUser
                }); 
        }
        catch (InvalidDataException exception)
        {
            return StatusCode(401, "Wrong username or password");
        }
        
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto, CancellationToken cancellationToken)
    {
        try
        {
            await _userService.AddUserAsync(registerDto, cancellationToken);
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal Server Error");
        }
    }
}
