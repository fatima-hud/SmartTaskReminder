using SmartTaskReminder.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SmartTaskReminder.Application.Dtos.Auth.Requests
{
    public class RequestRegisterDto
    {
        [Required]
        public string UserName { get; set; } = string.Empty;
        [Required]
        public string FirstName { get; set; } = string.Empty;
        [Required]
        public string LastName { get; set; } = string.Empty;
        [Required]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
        [Required]
        public Gender Gender { get; set; }
        [Required]
        public DateTime BirthDate { get; set; }

    }
}
