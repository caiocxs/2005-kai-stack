using Backend.Application.DTOs;
using Backend.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthenticationController : ControllerBase
{
    private readonly IAuthenticationService _authService;

    public AuthenticationController(IAuthenticationService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        var result = await _authService.LoginAsync(request, cancellationToken);
        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request, CancellationToken cancellationToken)
    {
        var result = await _authService.RegisterAsync(request, cancellationToken);
        if (!result.Success)
            return BadRequest(result);

        return CreatedAtAction(nameof(Login), result);
    }
}
