
using Hangfire;
using SmartTaskReminder.Application.Interfaces;

namespace SmartTaskReminder.Infrastructure.Services;

public class HangfireReminderScheduler : IReminderScheduler
{
    private readonly IBackgroundJobClient _backgroundJobClient;

    public HangfireReminderScheduler(
        IBackgroundJobClient backgroundJobClient)
    {
        _backgroundJobClient = backgroundJobClient;
    }

    public string ScheduleReminder(int taskId, TimeSpan delay)
    {
        return _backgroundJobClient.Schedule<IReminderJob>(
            job => job.SendReminderAsync(taskId),
            delay);
    }

    public bool CancelReminder(string jobId)
    {
        return _backgroundJobClient.Delete(jobId);
    }
}
