using System;
using System.Collections.Generic;
using System.Text;

namespace SmartTaskReminder.Application.Dtos.Auth.Responses
{
    public class ResponseLogInDto
    {
        public int UserId { get; set; }
        public string Username { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string AccessToken { get; set; }=string.Empty;
        public string RefreshTokenString { get; set; } = string.Empty;
        public DateTime ExpireAt { get; set; }
    }
}
