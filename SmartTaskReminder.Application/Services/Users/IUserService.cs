using SmartTaskReminder.Application.Dtos.Users;
using SmartTaskReminder.Core.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartTaskReminder.Application.Services.Users
{
    public interface IUserService
    {
        Task<Result> UpdateUserInfoAsync(int userId,RequestUpdateUserDto request);
        Task<Result> DeleteAccountAsync(int userId);
    }
}
