using System;
using System.Collections.Generic;
using System.Text;

namespace SmartTaskReminder.Application.Interfaces
{
    public interface IReminderJob
    {
        Task SendReminderAsync(int taskId);
        
    }
}
