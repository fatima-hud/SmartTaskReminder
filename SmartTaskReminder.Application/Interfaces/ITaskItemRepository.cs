using SmartTaskReminder.Application.Dtos.TaskItems.Responses;
using SmartTaskReminder.Application.Paginations;
using SmartTaskReminder.Core.Entities;
using SmartTaskReminder.Core.Enums;
namespace SmartTaskReminder.Application.Interfaces
{
    public interface ITaskItemRepository : IGenericRepositoryAsync<TaskItem>
    {
        public Task<PagedResult<TaskItem>> GetAllTasksByUserAsync(int userId,int pageNumber,int pageSize);
        Task<PagedResult<TaskItem>> FilterByDateAsync(int userId,DateTime startDate,  DateTime endDate ,int pageNumber,int pageSize);
        Task<PagedResult<TaskItem>> FilterByPriorityAsync(int userId,TaskItemPriority priority ,int pageNumber,int pageSize);
        Task<PagedResult<TaskItem>> FilterByStatusAsync(int userId, TaskItemStatus status, int pageNumber, int pageSize);


    }
}
