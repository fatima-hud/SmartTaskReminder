using SmartTaskReminder.Application.Dtos.TaskItems.Requests;
using SmartTaskReminder.Application.Dtos.TaskItems.Responses;
using SmartTaskReminder.Application.Interfaces;
using SmartTaskReminder.Application.Paginations;
using SmartTaskReminder.Core.Entities;
using SmartTaskReminder.Core.Enums;
using SmartTaskReminder.Core.Results;
namespace SmartTaskReminder.Application.Services.TaskItems;

public class TaskItemService : ITaskItemService
{
    private readonly ITaskItemRepository _taskRepository;
   
    private readonly IReminderScheduler _reminderScheduler;
    public TaskItemService(ITaskItemRepository taskRepository, IReminderScheduler reminderScheduler)
    {
        _taskRepository = taskRepository;
       
        _reminderScheduler = reminderScheduler;
    }

    public async Task<Result<ResponseTaskItemDto>> CreateAsync(int userId, RequestCreateTaskItemDto request)
    {
        if (string.IsNullOrWhiteSpace(request.Title))
        {
            return Result<ResponseTaskItemDto>.BadRequest("Task title is required");
        }
        if (request.Title.Length > 200)
        {
            return Result<ResponseTaskItemDto>.BadRequest("Task title cannot exceed 200 characters");
        }

        if (request.ReminderBeforeMinutes < 0)
        {
            return Result<ResponseTaskItemDto>.BadRequest("Reminder minutes cannot be negative");
        }

        if (request.DueDate.HasValue && request.DueDate.Value <= DateTime.UtcNow)
        {
            return Result<ResponseTaskItemDto>.BadRequest("Due date must be in the future");
        }
        if (request.ReminderBeforeMinutes.HasValue && !request.DueDate.HasValue)
        {
            return Result<ResponseTaskItemDto>.BadRequest("A reminder cannot be set without a due date.");

        }
        if (request.DueDate.HasValue && request.ReminderBeforeMinutes.HasValue
            && request.DueDate.Value.AddMinutes(-request.ReminderBeforeMinutes.Value) <= DateTime.UtcNow)
        {
            return Result<ResponseTaskItemDto>.BadRequest("Reminder time must be in the future.");

        }

        var task = new TaskItem
        {
            Title = request.Title.Trim(),
            Description = request.Description,
            Priority = request.Priority,
            Status = TaskItemStatus.Pending,
            DueDate = request.DueDate,
            ReminderBeforeMinutes = request.ReminderBeforeMinutes,
            UserId = userId,
            CreatedAt = DateTime.UtcNow,
            IsDeleted = false
        };

        await _taskRepository.AddAsync(task);


        if (task.DueDate.HasValue && task.ReminderBeforeMinutes.HasValue)

        {
            var reminderTime = task.DueDate.Value.AddMinutes(
                -task.ReminderBeforeMinutes.Value);

            var delay = reminderTime - DateTime.UtcNow;
            if (delay > TimeSpan.Zero)
            {
                task.ReminderJobId = _reminderScheduler.ScheduleReminder(task.Id, delay);
            }

            await _taskRepository.SaveChangesAsync();
        }
        return Result<ResponseTaskItemDto>.Success(MapToDto(task));
    }

    public async Task<Result<ResponseTaskItemDto>> GetByIdAsync(int userId, int taskId)
    {
        var task = await _taskRepository.GetByIdAsync(taskId);

        if (task is null || task.UserId != userId)
        {
            return Result<ResponseTaskItemDto>.NotFound("Task not found");
        }

        return Result<ResponseTaskItemDto>.Success(MapToDto(task));
    }

    public async Task<Result<PagedResult<ResponseTaskItemDto>>> GetMyTasksAsync(int userId, int pageNumber, int pageSize)
    {
        if (pageNumber < 1 || pageSize < 1 || pageSize > 100)
        {
            return Result<PagedResult<ResponseTaskItemDto>>.BadRequest("Invalid pagination parameters");

        }
        var result = await _taskRepository.GetAllTasksByUserAsync(userId, pageNumber, pageSize);

        var items = result.Items.Select(MapToDto).ToList();

        var response = new PagedResult<ResponseTaskItemDto>(items, result.MetaData);

        return Result<PagedResult<ResponseTaskItemDto>>.Success(response);
    }


    public async Task<Result<ResponseTaskItemDto>> UpdateAsync(
        int userId,
        int taskId,
        RequestUpdateTaskItemDto request)
    {
        var task = await _taskRepository.GetByIdAsync(taskId);

        if (task is null || task.UserId != userId)
            return Result<ResponseTaskItemDto>.NotFound("Task not found");

        if (string.IsNullOrWhiteSpace(request.Title))
            return Result<ResponseTaskItemDto>.BadRequest("Task title is required");

        if (request.Title.Trim().Length > 200)
            return Result<ResponseTaskItemDto>.BadRequest(
                "Task title cannot exceed 200 characters");

        if (request.ReminderBeforeMinutes.HasValue &&
            request.ReminderBeforeMinutes.Value < 0)
            return Result<ResponseTaskItemDto>.BadRequest(
                "Reminder minutes cannot be negative");

        if (request.DueDate.HasValue &&
            request.DueDate.Value <= DateTime.UtcNow)
            return Result<ResponseTaskItemDto>.BadRequest(
                "Due date must be in the future");

        if (request.ReminderBeforeMinutes.HasValue &&
            !request.DueDate.HasValue)
            return Result<ResponseTaskItemDto>.BadRequest(
                "A reminder cannot be set without a due date.");

        // Validate reminder time before changing the task.
        if (request.DueDate.HasValue &&
            request.ReminderBeforeMinutes.HasValue &&
            request.DueDate.Value.AddMinutes(
                -request.ReminderBeforeMinutes.Value) <= DateTime.UtcNow)
        {
            return Result<ResponseTaskItemDto>.BadRequest(
                "Reminder time must be in the future.");
        }

        // Cancel the previous reminder.
        if (!string.IsNullOrWhiteSpace(task.ReminderJobId))
        {
            _reminderScheduler.CancelReminder(task.ReminderJobId);
            task.ReminderJobId = null;
        }

        task.Title = request.Title.Trim();
        task.Description = request.Description;
        task.Priority = request.Priority;
        task.DueDate = request.DueDate;
        task.ReminderBeforeMinutes = request.ReminderBeforeMinutes;
        task.UpdatedAt = DateTime.UtcNow;

        await _taskRepository.UpdateAsync(task);

        // Schedule a new reminder when configured.
        if (task.DueDate.HasValue &&
            task.ReminderBeforeMinutes.HasValue)
        {
            var reminderTime = task.DueDate.Value.AddMinutes(
                -task.ReminderBeforeMinutes.Value);

            var delay = reminderTime - DateTime.UtcNow;

            task.ReminderJobId = _reminderScheduler.ScheduleReminder(
                task.Id,
                delay);

            await _taskRepository.UpdateAsync(task);
        }

        return Result<ResponseTaskItemDto>.Success(MapToDto(task));
    }


    public async Task<Result> DeleteAsync(int userId, int taskId)
    {
        var task = await _taskRepository.GetByIdAsync(taskId);

        if (task is null || task.UserId != userId)
            return Result.NotFound("Task not found");
        if (!string.IsNullOrWhiteSpace(task.ReminderJobId))
        {
            _reminderScheduler.CancelReminder(task.ReminderJobId);
            task.ReminderJobId = null;
        }
        task.IsDeleted = true;
        task.DeletedAt = DateTime.UtcNow;
        task.UpdatedAt = DateTime.UtcNow;

        await _taskRepository.UpdateAsync(task);


        return Result.Success();
    }

    public async Task<Result> ChangeStatusAsync(int userId, int taskId, TaskItemStatus status)
    {
        var task = await _taskRepository.GetByIdAsync(taskId);

        if (task is null || task.UserId != userId)
            return Result.NotFound("Task not found");

        if (!Enum.IsDefined(typeof(TaskItemStatus), status))
            return Result.BadRequest("Invalid task status");

        task.Status = status;
        task.UpdatedAt = DateTime.UtcNow;

        await _taskRepository.UpdateAsync(task);
        return Result.Success();
    }


    private static ResponseTaskItemDto MapToDto(TaskItem task)
    {
        return new ResponseTaskItemDto
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            Priority = task.Priority,
            Status = task.Status,
            DueDate = task.DueDate,
            ReminderBeforeMinutes = task.ReminderBeforeMinutes,
            CreatedAt = task.CreatedAt
        };
    }
    public async Task<Result<PagedResult<ResponseTaskItemDto>>> FilterByDateAsync(int userId, DateTime startDate, DateTime endDate, int pageNumber, int pageSize)
    {
        if (pageNumber < 1 || pageSize < 1 || pageSize > 100)
        {
            return Result<PagedResult<ResponseTaskItemDto>>.BadRequest("Invalid pagination parameters");

        }

        if (startDate > endDate)
        {
            return Result<PagedResult<ResponseTaskItemDto>>.BadRequest("Start date must be before or equal to end date");

        }

        var result = await _taskRepository.FilterByDateAsync(userId, startDate, endDate, pageNumber, pageSize);


        var items = result.Items.Select(MapToDto).ToList();

        var response = new PagedResult<ResponseTaskItemDto>(items, result.MetaData);


        return Result<PagedResult<ResponseTaskItemDto>>.Success(response);
    }

    public async Task<Result<PagedResult<ResponseTaskItemDto>>> FilterByPriorityAsync(int userId, TaskItemPriority priority, int pageNumber, int pageSize)

    {
        if (pageNumber < 1 || pageSize < 1 || pageSize > 100)
        {
            return Result<PagedResult<ResponseTaskItemDto>>.BadRequest("Invalid pagination parameters");

        }

        if (!Enum.IsDefined(typeof(TaskItemPriority), priority))
        {
            return Result<PagedResult<ResponseTaskItemDto>>.BadRequest("Invalid task priority");

        }

        var result = await _taskRepository.FilterByPriorityAsync(userId, priority, pageNumber, pageSize);


        var items = result.Items.Select(MapToDto).ToList();

        var response = new PagedResult<ResponseTaskItemDto>(items, result.MetaData);


        return Result<PagedResult<ResponseTaskItemDto>>.Success(response);
    }

    public async Task<Result<PagedResult<ResponseTaskItemDto>>> FilterByStatusAsync(int userId, TaskItemStatus status, int pageNumber, int pageSize)
    {
        var result = await _taskRepository.FilterByStatusAsync(userId, status, pageNumber, pageSize);

        var items = result.Items.Select(MapToDto).ToList();

        var response = new PagedResult<ResponseTaskItemDto>(items, result.MetaData);

        return Result<PagedResult<ResponseTaskItemDto>>.Success(response);
    }
}