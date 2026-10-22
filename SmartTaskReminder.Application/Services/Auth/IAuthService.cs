using SmartTaskReminder.Application.Dtos.Auth.Requests;
using SmartTaskReminder.Application.Dtos.Auth.Responses;
using SmartTaskReminder.Application.Dtos.Auths.Requests;
using SmartTaskReminder.Core.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartTaskReminder.Application.Services.Auth
{
    public interface IAuthService
    {
        Task<Result> RegisterAsync(RequestRegisterDto request);
        Task<Result<ResponseLogInDto>> LoginInAsync(RequestLoginDto request);
        Task<Result> LogoutAsync(int userId, RequestLogOutDto request);
        Task<Result<JwtAuthResult>> RefreshAccessTokenAsync(RequestRefreshAccessTokenDto request);
            




    }
}
