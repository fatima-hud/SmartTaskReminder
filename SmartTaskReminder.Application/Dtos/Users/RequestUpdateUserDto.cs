using SmartTaskReminder.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartTaskReminder.Application.Dtos.Users
{
    public class RequestUpdateUserDto
    {
        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public Gender Gender { get; set; }

        public DateTime BirthDate { get; set; }
    }
}
