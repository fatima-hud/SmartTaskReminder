using System;
using System.Collections.Generic;
using System.Text;

namespace SmartTaskReminder.Application.Dtos.Auth.Responses
{
    public class JwtAuthResult
    {
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime ExpireAt { get; set; }
    }
}
