using SmartTaskReminder.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartTaskReminder.Application.Dtos.TaskItems.Responses
{
    public class ResponseTaskItemDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public TaskItemPriority Priority { get; set; }
        public TaskItemStatus Status { get; set; }
        public DateTime? DueDate { get; set; }
        public int? ReminderBeforeMinutes { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
