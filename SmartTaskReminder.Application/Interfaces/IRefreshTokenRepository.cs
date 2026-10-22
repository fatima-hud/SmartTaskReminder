using SmartTaskReminder.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartTaskReminder.Application.Interfaces
{
    public interface IRefreshTokenRepository:IGenericRepositoryAsync<RefreshToken>
    {
        Task<RefreshToken?> GetByTokenAsync(string token);
        Task<List<RefreshToken>> GetAllByUserIdAsync(int userId);
        
    }
}
