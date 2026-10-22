using System;
using System.Collections.Generic;
using System.Text;

namespace SmartTaskReminder.Application.Interfaces
{
    public interface IReminderScheduler
    {
        string ScheduleReminder(int taskId, TimeSpan delay);

        bool CancelReminder(string jobId);
    }
}
