using be_atoutmajeur.Models.DTOs.Auth;
using be_atoutmajeur.Models.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace be_atoutmajeur.Controllers.Auth;

[ApiController]
[Route("auth/")]
[Authorize]
[Produces("application/json")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    
    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(IApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(IApiResponse), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IApiResponse>> Register([FromBody] RegisterRequestDto dto)
    {
        var result = await _authService.RegisterAsync(dto);
        return Ok(result);
    }

    [HttpPost("login")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(IApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IApiResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(IApiResponse), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IApiResponse>> Login([FromBody] LoginRequestDto dto)
    {
        var result = await _authService.LoginAsync(dto);
        return Ok(result);
    }
}