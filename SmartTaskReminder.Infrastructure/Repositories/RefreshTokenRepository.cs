using Microsoft.EntityFrameworkCore;
using SmartTaskReminder.Application.Interfaces;
using SmartTaskReminder.Core.Entities;
using SmartTaskReminder.Infrastructure.Context;
using SmartTaskReminder.Infrastructure.InfrastructureBases;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartTaskReminder.Infrastructure.Repositories
{
    public class RefreshTokenRepository : GenericRepositoryAsync<RefreshToken>, IRefreshTokenRepository
    {
        public RefreshTokenRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
          
         }

        
            public async Task<List<RefreshToken>> GetAllByUserIdAsync(int userId)
        {
            return await _dbContext.RefreshTokens
                .Where(token => token.UserId == userId)
                .ToListAsync();
        }
        

        public async Task<RefreshToken?> GetByTokenAsync(string token)
        {
            return await _dbContext.RefreshTokens
               
                .FirstOrDefaultAsync(rt => rt.Token == token);
        }
        

    }
}
