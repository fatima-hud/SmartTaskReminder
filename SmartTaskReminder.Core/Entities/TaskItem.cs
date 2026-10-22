using SmartTaskReminder.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartTaskReminder.Core.Entities
{
    public class TaskItem:BaseEntity
    {
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        public TaskItemPriority Priority { get; set; } = TaskItemPriority.Medium;

        public TaskItemStatus Status { get; set; } = TaskItemStatus.Pending;

        public DateTime? DueDate { get; set; }

        public int? ReminderBeforeMinutes { get; set; }
        public string? ReminderJobId { get; set; }
        public int UserId { get; set; }
        public User User { get; set; } = null!;
    }
}
