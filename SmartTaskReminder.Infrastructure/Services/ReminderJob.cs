
using Microsoft.Extensions.Logging;
using SmartTaskReminder.Application.Interfaces;

namespace SmartTaskReminder.Infrastructure.Services;

public class ReminderJob : IReminderJob
{
    private readonly ILogger<ReminderJob> _logger;

    public ReminderJob(ILogger<ReminderJob> logger)
    {
        _logger = logger;
    }

    public Task SendReminderAsync(int taskId)
    {
        _logger.LogInformation(
            "Reminder triggered for TaskId: {TaskId} at {Time}",
            taskId,
            DateTime.UtcNow);

        return Task.CompletedTask;
    }
}
