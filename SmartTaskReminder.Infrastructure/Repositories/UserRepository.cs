using SmartTaskReminder.Infrastructure.InfrastructureBases;
using System;
using System.Collections.Generic;
using System.Text;
using SmartTaskReminder.Core.Entities;
using SmartTaskReminder.Application.Interfaces;
using SmartTaskReminder.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
namespace SmartTaskReminder.Infrastructure.Repositories
{
    public class UserRepository : GenericRepositoryAsync<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _dbContext.Users
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User?> GetByUserNameAsync(string userName)
        {
            return await _dbContext.Users
                .FirstOrDefaultAsync(u => u.UserName == userName);
        }
    }
}
