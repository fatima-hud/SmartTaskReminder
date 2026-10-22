using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartTaskReminder.API.Bases;
using SmartTaskReminder.API.Extensions;
using SmartTaskReminder.Application.Dtos.Auth.Requests;
using SmartTaskReminder.Application.Dtos.Auth.Responses;
using SmartTaskReminder.Application.Dtos.Auths.Requests;
using SmartTaskReminder.Application.Interfaces;
using SmartTaskReminder.Application.Services.Auth;
using System.Security.Claims;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(
        IAuthService authService,
        ILogger<AuthController> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<ApiResult> Register([FromBody] RequestRegisterDto request)
        
    {
        try
        {
            var result = await _authService.RegisterAsync(request);

            if (!result.IsSuccess)
                return this.ToApiResult(result);

            return ApiResult.Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while registering a user.");
            return ApiResult.InternalServerError("حدث خطأ داخلي");
        }
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ApiResult<ResponseLogInDto>> Login( [FromBody] RequestLoginDto request)
       
    {
        try
        {
            var result = await _authService.LoginInAsync(request);

            if (!result.IsSuccess)
                return this.ToApiResult(result);

            return ApiResult<ResponseLogInDto>.Ok(result.Value);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while logging in.");
            return ApiResult<ResponseLogInDto>
                .InternalServerError("حدث خطأ داخلي");
        }
    }

    [HttpPost("logout")]
    [Authorize]
    public async Task<ApiResult> Logout([FromBody] RequestLogOutDto request)
        
    {
        try
        {
            var userIdValue = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                

            if (!int.TryParse(userIdValue, out var userId))
                return ApiResult.Unauthorized();

            var result = await _authService.LogoutAsync(userId, request);

            if (!result.IsSuccess)
                return this.ToApiResult(result);

            return ApiResult.Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while logging out.");
            return ApiResult.InternalServerError("حدث خطأ داخلي");
        }
    }


    [HttpPost("refresh-token")]
    [AllowAnonymous]
    public async Task<ApiResult<JwtAuthResult>> RefreshAccessToken(
        [FromBody] RequestRefreshAccessTokenDto request)
    {
        try
        {
            var result = await _authService.RefreshAccessTokenAsync(request);

            if (!result.IsSuccess)
                return this.ToApiResult(result);

            return ApiResult<JwtAuthResult>.Ok(result.Value);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "An error occurred while refreshing the access token.");

            return ApiResult<JwtAuthResult>.InternalServerError(
                "An internal error occurred.");
        }
    }

}