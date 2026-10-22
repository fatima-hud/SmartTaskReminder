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
    public class RoleRepository :GenericRepositoryAsync<Role>, IRoleRepository
    {
        public RoleRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<Role?> GetRoleByUserAsync(int userId)
        {
            return await _dbContext.Roles
                .Where(r => r.Users.Any(u => u.Id == userId))
                .FirstOrDefaultAsync();
        }

        public async Task<int?> GetRoleIdByNameAsync(string roleName)
        {
            return await _dbContext.Roles
                .Where(r => r.Name == roleName)
                .Select(r => (int?)r.Id)
                .FirstOrDefaultAsync();
        }
        
    }
}
