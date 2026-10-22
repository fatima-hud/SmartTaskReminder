using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SmartTaskReminder.Application.Dtos.Auth.Requests
{
    public class RequestForgetPasswordDto
    {
        [Required, EmailAddress]
        public string Email { get; set; }=string.Empty;
    }
}
