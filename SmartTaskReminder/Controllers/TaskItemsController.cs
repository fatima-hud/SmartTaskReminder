using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartTaskReminder.API.Bases;
using SmartTaskReminder.API.Extensions;
using SmartTaskReminder.Application.Dtos.TaskItems.Requests;
using SmartTaskReminder.Application.Dtos.TaskItems.Responses;
using SmartTaskReminder.Application.Paginations;
using SmartTaskReminder.Application.Services.TaskItems;
using SmartTaskReminder.Core.Enums;
using System.Security.Claims;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TaskItemsController : ControllerBase
{
    private readonly ITaskItemService _taskItemService;
    private readonly ILogger<TaskItemsController> _logger;

    public TaskItemsController(
        ITaskItemService taskItemService,
        ILogger<TaskItemsController> logger)
    {
        _taskItemService = taskItemService;
        _logger = logger;
    }

    private bool TryGetUserId(out int userId)
    {
        userId = 0;

        var userIdValue = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        return int.TryParse(userIdValue, out userId);
    }
   
    [HttpPost]
    public async Task<ApiResult<ResponseTaskItemDto>> Create([FromBody] RequestCreateTaskItemDto request)
        
    {
        try
        {
            if (!TryGetUserId(out var userId))
                return ApiResult<ResponseTaskItemDto>.Unauthorized();

            var result = await _taskItemService.CreateAsync(userId, request);

            if (!result.IsSuccess)
                return this.ToApiResult(result);

            return ApiResult<ResponseTaskItemDto>.Ok(result.Value);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while creating a task.");
            return ApiResult<ResponseTaskItemDto>
                .InternalServerError("حدث خطأ داخلي");
        }
    }

    [HttpGet("{taskId}")]
    public async Task<ApiResult<ResponseTaskItemDto>> GetById(int taskId)
    {
        try
        {
            if (!TryGetUserId(out var userId))
                return ApiResult<ResponseTaskItemDto>.Unauthorized();

            var result = await _taskItemService.GetByIdAsync(userId, taskId);

            if (!result.IsSuccess)
                return this.ToApiResult(result);

            return ApiResult<ResponseTaskItemDto>.Ok(result.Value);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while getting task {TaskId}.", taskId);
            return ApiResult<ResponseTaskItemDto>
                .InternalServerError("حدث خطأ داخلي");
        }
    }

    [HttpGet]
    public async Task<ApiResult<PagedResult<ResponseTaskItemDto>>> GetMyTasks( [FromQuery] int pageNumber = 1,[FromQuery] int pageSize = 10)
     
    {
        try
        {
            if (!TryGetUserId(out var userId))
                return ApiResult<PagedResult<ResponseTaskItemDto>>.Unauthorized();

            var result = await _taskItemService.GetMyTasksAsync(userId, pageNumber, pageSize);
                

            if (!result.IsSuccess)
                return this.ToApiResult(result);

            return ApiResult<PagedResult<ResponseTaskItemDto>>.Ok(result.Value);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while getting user tasks.");
            return ApiResult<PagedResult<ResponseTaskItemDto>>
                .InternalServerError("حدث خطأ داخلي");
        }
    }

    [HttpPut("{taskId}")]
    public async Task<ApiResult<ResponseTaskItemDto>> Update(int taskId, [FromBody] RequestUpdateTaskItemDto request)
 
    {
        try
        {
            if (!TryGetUserId(out var userId))
                return ApiResult<ResponseTaskItemDto>.Unauthorized();

            var result = await _taskItemService.UpdateAsync(
                userId, taskId, request);

            if (!result.IsSuccess)
                return this.ToApiResult(result);

            return ApiResult<ResponseTaskItemDto>.Ok(result.Value);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while updating task {TaskId}.", taskId);
            return ApiResult<ResponseTaskItemDto>
                .InternalServerError("حدث خطأ داخلي");
        }
    }

    [HttpPatch("{taskId}/status")]
    public async Task<ApiResult> ChangeStatus( int taskId,[FromQuery] TaskItemStatus status)
       
        
    {
        try
        {
            if (!TryGetUserId(out var userId))
                return ApiResult.Unauthorized();

            var result = await _taskItemService.ChangeStatusAsync(
                userId, taskId, status);

            if (!result.IsSuccess)
                return this.ToApiResult(result);

            return ApiResult.Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while changing status of task {TaskId}.", taskId);
            return ApiResult.InternalServerError("حدث خطأ داخلي");
        }
    }

    [HttpDelete("{taskId}")]
    public async Task<ApiResult> Delete(int taskId)
    {
        try
        {
            if (!TryGetUserId(out var userId))
                return ApiResult.Unauthorized();

            var result = await _taskItemService.DeleteAsync(userId, taskId);

            if (!result.IsSuccess)
                return this.ToApiResult(result);

            return ApiResult.Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while deleting task {TaskId}.", taskId);
            return ApiResult.InternalServerError("حدث خطأ داخلي");
        }
    }

   
    [HttpGet("filter/date")]
    public async Task<ApiResult<PagedResult<ResponseTaskItemDto>>> FilterByDate(
        [FromQuery] DateTime startDate,
        [FromQuery] DateTime endDate,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        try
        {
            if (!TryGetUserId(out var userId))
                return ApiResult<PagedResult<ResponseTaskItemDto>>.Unauthorized();

            var result = await _taskItemService.FilterByDateAsync(
                userId, startDate, endDate, pageNumber, pageSize);

            if (!result.IsSuccess)
                return this.ToApiResult(result);

            return ApiResult<PagedResult<ResponseTaskItemDto>>.Ok(result.Value);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while filtering tasks by date.");
            return ApiResult<PagedResult<ResponseTaskItemDto>>
                .InternalServerError("حدث خطأ داخلي");
        }
    }

    [HttpGet("filter/priority")]
    public async Task<ApiResult<PagedResult<ResponseTaskItemDto>>> FilterByPriority(
        [FromQuery] TaskItemPriority priority,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        try
        {
            if (!TryGetUserId(out var userId))
                return ApiResult<PagedResult<ResponseTaskItemDto>>.Unauthorized();

            var result = await _taskItemService.FilterByPriorityAsync(
                userId, priority, pageNumber, pageSize);

            if (!result.IsSuccess)
                return this.ToApiResult(result);

            return ApiResult<PagedResult<ResponseTaskItemDto>>.Ok(result.Value);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while filtering tasks by priority.");
            return ApiResult<PagedResult<ResponseTaskItemDto>>
                .InternalServerError("حدث خطأ داخلي");
        }
    }

    [HttpGet("filter/status")]
    public async Task<ApiResult<PagedResult<ResponseTaskItemDto>>> FilterByStatus(
        [FromQuery] TaskItemStatus status,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        try
        {
            if (!TryGetUserId(out var userId))
                return ApiResult<PagedResult<ResponseTaskItemDto>>.Unauthorized();

            var result = await _taskItemService.FilterByStatusAsync(
                userId, status, pageNumber, pageSize);

            if (!result.IsSuccess)
                return this.ToApiResult(result);

            return ApiResult<PagedResult<ResponseTaskItemDto>>.Ok(result.Value);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while filtering tasks by priority.");
            return ApiResult<PagedResult<ResponseTaskItemDto>>
                .InternalServerError("حدث خطأ داخلي");
        }
    }
}