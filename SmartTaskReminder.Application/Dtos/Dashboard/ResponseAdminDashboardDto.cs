using System;
using System.Collections.Generic;
using System.Text;

namespace SmartTaskReminder.Application.Dtos.Dashboard
{
    public class ResponseAdminDashboardDto
    {
        public int TotalUsers { get; set; }

        public int ActiveUsersLast30Days { get; set; }

        public int NewUsersLast30Days { get; set; }

        public int TotalTasks { get; set; }

        public int CreatedTasksInPeriod { get; set; }

        public int CompletedTasksInPeriod { get; set; }
    }
}
