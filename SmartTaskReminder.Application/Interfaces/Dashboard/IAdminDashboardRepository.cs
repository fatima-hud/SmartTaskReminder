namespace SmartTaskReminder.Application.Interfaces.Dashboard
{
    public interface IAdminDashboardRepository
    {
        Task<int> GetTotalUsersAsync();

        Task<int> GetActiveUsersLast30DaysAsync(DateTime fromDate);

        Task<int> GetNewUsersLast30DaysAsync(DateTime fromDate);

        Task<int> GetTotalTasksAsync();

        Task<int> GetCreatedTasksInPeriodAsync( DateTime startDate, DateTime endDate);

        Task<int> GetCompletedTasksInPeriodAsync( DateTime startDate, DateTime endDate);
           
           
    }
}