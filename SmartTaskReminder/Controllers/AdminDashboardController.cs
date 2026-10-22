using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartTaskReminder.API.Bases;
using SmartTaskReminder.API.Extensions;
using SmartTaskReminder.Application.Dtos.Dashboard;
using SmartTaskReminder.Application.Services.Dashboard;

namespace SmartTaskReminder.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminDashboardController : ControllerBase
    {
        private readonly IAdminDashboardService _adminDashboardService;
        private readonly ILogger<AdminDashboardController> _logger;

        public AdminDashboardController(IAdminDashboardService adminDashboardService,ILogger<AdminDashboardController>logger)
        {
            _adminDashboardService = adminDashboardService;
            _logger = logger;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ApiResult<ResponseAdminDashboardDto>> GetDashboard([FromQuery] DateTime startDate,[FromQuery] DateTime endDate)
  
        {
            try
            {
                var result = await _adminDashboardService.GetDashboardAsync(startDate,endDate);

                if (!result.IsSuccess)
                    return this.ToApiResult(result);

                return ApiResult<ResponseAdminDashboardDto>.Ok(result.Value);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting admin dashboard.");
                return ApiResult<ResponseAdminDashboardDto>
                    .InternalServerError("حدث خطأ داخلي");
            }
        }
    }
}
