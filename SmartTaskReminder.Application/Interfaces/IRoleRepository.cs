using System;
using System.Collections.Generic;
using System.Text;
using SmartTaskReminder.Core.Entities;
namespace SmartTaskReminder.Application.Interfaces
{
    public interface IRoleRepository:IGenericRepositoryAsync<Role>
    {
        Task<int?>GetRoleIdByNameAsync(string roleName);
        Task<Role?> GetRoleByUserAsync(int userId);
    }
}
