using SmartTaskReminder.Application.Services.Auth;
using SmartTaskReminder.Application.Services.TaskItems;
using SmartTaskReminder.Application.Interfaces;
using SmartTaskReminder.Infrastructure.Repositories;
using SmartTaskReminder.Infrastructure.InfrastructureBases;
using SmartTaskReminder.Infrastructure.Services;
using SmartTaskReminder.Application.Services.Dashboard;
using SmartTaskReminder.Application.Interfaces.Dashboard;
using SmartTaskReminder.Infrastructure.Repositories.Dashboard;
using SmartTaskReminder.Core.Entities;
using SmartTaskReminder.Application.Services.Users;
namespace SmartTaskReminder.API.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
           services.AddScoped<IAuthService, AuthService>(); 
           services.AddScoped<ITaskItemService, TaskItemService>();
           services.AddScoped<ITaskItemRepository, TaskItemRepository>();
           services.AddScoped<IUserRepository, UserRepository>();
           services.AddScoped<IUnitOfWork, UnitOfWork>();
           services.AddScoped<IPasswordHasherService, PasswordHasherService>();
           services.AddScoped<IRoleRepository, RoleRepository>();
           services.AddScoped<ITokenService, TokenService>();
           services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            services.AddScoped<IAdminDashboardService, AdminDashboardService>();
            services.AddScoped<IAdminDashboardRepository, AdminDashboardRepository>();
            services.AddScoped<IPasswordHasherService, PasswordHasherService>();
            services.AddScoped<IReminderJob, ReminderJob>();
            services.AddScoped<IReminderScheduler, HangfireReminderScheduler>();
           
            services.AddScoped<IUserService, UserService>();








        }
    }
}
