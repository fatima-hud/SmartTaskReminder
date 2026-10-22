using Microsoft.EntityFrameworkCore;
using SmartTaskReminder.Application.Interfaces.Dashboard;
using SmartTaskReminder.Core.Enums;
using SmartTaskReminder.Infrastructure.Context;

namespace SmartTaskReminder.Infrastructure.Repositories.Dashboard
{
    public class AdminDashboardRepository : IAdminDashboardRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public AdminDashboardRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

      
        public async Task<int> GetTotalUsersAsync()
        {
            return await _dbContext.Users.Where(u=>u.Role.Name == "User").CountAsync();
        }

        
        public async Task<int> GetActiveUsersLast30DaysAsync(
            DateTime fromDate)
        {
            return await _dbContext.TaskItems
                .Where(t => t.CreatedAt >= fromDate && t.User.Role.Name=="User")
                .Select(t => t.UserId)
                .Distinct()
                .CountAsync();
        }

       
        public async Task<int> GetNewUsersLast30DaysAsync(
            DateTime fromDate)
        {
            return await _dbContext.Users
                .CountAsync(u => u.CreatedAt >= fromDate && u.Role.Name == "User");
        }

 
        public async Task<int> GetTotalTasksAsync()
        {
            return await _dbContext.TaskItems.CountAsync();
        }

        public async Task<int> GetCreatedTasksInPeriodAsync( DateTime startDate,  DateTime endDate)
        {
            var endDateValue = endDate.Date.AddDays(1);
            return await _dbContext.TaskItems
                .CountAsync(t =>
                    t.CreatedAt >= startDate.Date &&
                    t.CreatedAt < endDateValue);
        }

        // المهام المكتملة خلال الفترة المحددة
        public async Task<int> GetCompletedTasksInPeriodAsync( DateTime startDate,DateTime endDate)
        {
            var endDateValue = endDate.Date.AddDays(1);
            return await _dbContext.TaskItems
                .CountAsync(t =>
                    t.Status == TaskItemStatus.Completed &&
                    t.UpdatedAt >= startDate.Date &&
                    t.UpdatedAt <endDateValue);
        }
    }
}