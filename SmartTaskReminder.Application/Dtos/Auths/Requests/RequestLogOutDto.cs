using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SmartTaskReminder.Application.Dtos.Auth.Requests
{
    public class RequestLogOutDto
    {
        [Required]
        public string RefreshToken { get; set; } = string.Empty;
    }
}
