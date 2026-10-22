using SmartTaskReminder.Application.Dtos.Dashboard;
using System;
using System.Collections.Generic;
using System.Text;
using SmartTaskReminder.Core.Results;
namespace SmartTaskReminder.Application.Services.Dashboard
{
    public interface IAdminDashboardService
    {
        Task<Result<ResponseAdminDashboardDto>> GetDashboardAsync(DateTime startDate,DateTime endDate);
            
            
    }
}
