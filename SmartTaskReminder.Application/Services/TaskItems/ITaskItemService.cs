using SmartTaskReminder.Application.Dtos.TaskItems.Requests;
using SmartTaskReminder.Application.Dtos.TaskItems.Responses;
using SmartTaskReminder.Application.Paginations;
using SmartTaskReminder.Core.Enums;
using SmartTaskReminder.Core.Results;

namespace SmartTaskReminder.Application.Services.TaskItems
{
    public interface ITaskItemService
    {
        Task<Result<ResponseTaskItemDto>> CreateAsync(int userId, RequestCreateTaskItemDto request);
        Task<Result<ResponseTaskItemDto>> GetByIdAsync(int userId, int taskId);
        Task<Result<PagedResult<ResponseTaskItemDto>>> GetMyTasksAsync(int userId, int pageNumber, int pageSize);
        Task<Result<ResponseTaskItemDto>> UpdateAsync( int userId, int taskId,RequestUpdateTaskItemDto request);
        Task<Result> ChangeStatusAsync( int userId, int taskId,TaskItemStatus status);
        Task<Result> DeleteAsync(int userId,  int taskId);
        Task<Result<PagedResult<ResponseTaskItemDto>>> FilterByDateAsync(int userId,DateTime startDate,DateTime endDate,int pageNumber,int pageSize);
        Task<Result<PagedResult<ResponseTaskItemDto>>> FilterByPriorityAsync(int userId,TaskItemPriority priority, int pageNumber, int pageSize);
        Task<Result<PagedResult<ResponseTaskItemDto>>> FilterByStatusAsync(int userId,TaskItemStatus status, int pageNumber, int pageSize);





    }
}
