
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartTaskReminder.API.Bases;
using SmartTaskReminder.API.Extensions;
using SmartTaskReminder.Application.Dtos.Users;
using SmartTaskReminder.Application.Services.Users;
using System.Security.Claims;

[ApiController]
[Route("api/[controller]/[action]")]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ILogger<UsersController> _logger;

    public UsersController(IUserService userService,ILogger<UsersController>logger)
    {
        _userService = userService;
        _logger = logger;
    }

    [HttpPut]
    [Authorize]
    public async Task<ApiResult> UpdateMyInfo([FromBody] RequestUpdateUserDto request)

    {
        try
        {
            var userIdValue = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;


            if (!int.TryParse(userIdValue, out var userId))
                return ApiResult.Unauthorized();

            var result = await _userService.UpdateUserInfoAsync(userId, request);
                

            if (!result.IsSuccess)
                return this.ToApiResult(result);

            return ApiResult.Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while updating user information.");
            return ApiResult.InternalServerError("حدث خطأ داخلي");
        }
    }

    [HttpDelete]
    [Authorize]
    public async Task<ApiResult> DeleteMyAccount()
    {
        try
        {
            var userIdValue = User.FindFirst( ClaimTypes.NameIdentifier)?.Value;
               

            if (!int.TryParse(userIdValue, out var userId))
                return ApiResult.Unauthorized();

            var result = await _userService.DeleteAccountAsync(userId);

            if (!result.IsSuccess)
                return this.ToApiResult(result);

            return ApiResult.Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while deleting user account.");
            return ApiResult.InternalServerError("حدث خطأ داخلي");
        }
    }


}
