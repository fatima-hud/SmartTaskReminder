using SmartTaskReminder.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartTaskReminder.Application.Interfaces
{
    public interface IPasswordHasherService
    {
        Task<string> HashPassword(User user, string password);

        Task<bool> VerifyPassword(User user, string password);
    }
}
