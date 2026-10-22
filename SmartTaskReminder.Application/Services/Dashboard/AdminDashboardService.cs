using SmartTaskReminder.Application.Dtos.Dashboard;
using SmartTaskReminder.Application.Interfaces.Dashboard;
using SmartTaskReminder.Core.Results;
namespace SmartTaskReminder.Application.Services.Dashboard
{
    public class AdminDashboardService : IAdminDashboardService
    {
        private readonly IAdminDashboardRepository _repository;

        public AdminDashboardService(
            IAdminDashboardRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result<ResponseAdminDashboardDto>> GetDashboardAsync( DateTime startDate,DateTime endDate)
            
        {
            if (startDate > endDate)
            {
                return Result<ResponseAdminDashboardDto>.BadRequest("start date should be less than end date");
            }

            var fromDate = DateTime.UtcNow.Date.AddDays(-29);

            var dashboard = new ResponseAdminDashboardDto
            {
                TotalUsers =  await _repository.GetTotalUsersAsync(),
                  

                ActiveUsersLast30Days =   await _repository.GetActiveUsersLast30DaysAsync(fromDate),
                 
                        

                NewUsersLast30Days =await _repository.GetNewUsersLast30DaysAsync(fromDate),
                        
                    

                TotalTasks =await _repository.GetTotalTasksAsync(),
                    

                CreatedTasksInPeriod = await _repository.GetCreatedTasksInPeriodAsync(startDate, endDate),
                   
                        

                CompletedTasksInPeriod =  await _repository.GetCompletedTasksInPeriodAsync(startDate, endDate)
                  
                        
            };

            return Result<ResponseAdminDashboardDto>.Success(dashboard);
        }
    }
}