using SmartTaskReminder.Application.Dtos.Users;
using SmartTaskReminder.Application.Interfaces;
using SmartTaskReminder.Core.Enums;
using SmartTaskReminder.Core.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartTaskReminder.Application.Services.Users
{
    public class UserService:IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRefreshTokenRepository _refreshTokenRepository;

        public UserService(IUserRepository userRepository, IRefreshTokenRepository refreshTokenRepository)
        {
            _userRepository = userRepository;
            _refreshTokenRepository = refreshTokenRepository;
        }

        public async Task<Result> UpdateUserInfoAsync(int userId,RequestUpdateUserDto request)
  
        {
            var user = await _userRepository.GetByIdAsync(userId);

            if (user is null)
                return Result.NotFound("User not found.");

            if (string.IsNullOrWhiteSpace(request.FirstName))
                return Result.BadRequest("First name is required.");

            if (string.IsNullOrWhiteSpace(request.LastName))
                return Result.BadRequest("Last name is required.");

            if (!Enum.IsDefined(typeof(Gender), request.Gender))
                return Result.BadRequest("Invalid gender.");

            if (request.BirthDate.Date >= DateTime.UtcNow.Date)
                return Result.BadRequest("Birth date must be in the past.");

            user.FirstName = request.FirstName.Trim();
            user.LastName = request.LastName.Trim();
            user.Gender = request.Gender;
            user.BirthDate = request.BirthDate;
            user.UpdatedAt = DateTime.UtcNow;

            await _userRepository.UpdateAsync(user);

            return Result.Success("User information updated successfully.");
        }

        public async Task<Result> DeleteAccountAsync(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);

            if (user is null || user.IsDeleted)
                return Result.NotFound("User not found.");

            var refreshTokens =  await _refreshTokenRepository.GetAllByUserIdAsync(userId);
              

            foreach (var token in refreshTokens)
            {
                if (!token.IsRevoked)
                {
                    token.IsRevoked = true;
                    await _refreshTokenRepository.UpdateAsync(token);
                }
            }

            user.IsDeleted = true;
            user.DeletedAt = DateTime.UtcNow;
            user.UpdatedAt = DateTime.UtcNow;

            await _userRepository.UpdateAsync(user);

            return Result.Success("Account deleted successfully.");
        }


    }
}
