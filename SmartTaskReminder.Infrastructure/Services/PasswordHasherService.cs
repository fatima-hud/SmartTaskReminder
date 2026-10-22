using SmartTaskReminder.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using SmartTaskReminder.Application.Interfaces;
using Microsoft.AspNetCore.Identity;
namespace SmartTaskReminder.Infrastructure.Services
{
    public class PasswordHasherService : IPasswordHasherService
    {
        private readonly PasswordHasher<User> _hasher = new();

        public async Task<string> HashPassword(User user, string password)
        {
            return _hasher.HashPassword(user, password);
        }

        public async Task<bool> VerifyPassword(User user, string password)
        {
            var result = _hasher.VerifyHashedPassword(
                user,
                user.PasswordHash,
                password
            );

            return result != PasswordVerificationResult.Failed;
        }

    }
}
