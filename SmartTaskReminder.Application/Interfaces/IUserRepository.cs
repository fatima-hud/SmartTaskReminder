using SmartTaskReminder.Core.Entities;

namespace SmartTaskReminder.Application.Interfaces;

public interface IUserRepository : IGenericRepositoryAsync<User>
{
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetByUserNameAsync(string userName);
}
