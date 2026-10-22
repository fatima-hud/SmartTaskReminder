using System;
using System.Collections.Generic;
using System.Text;

namespace SmartTaskReminder.Application.Dtos.Auth.Responses
{
    public class RefreshTokenResult
    {
        public string RefreshTokenString { get; set; } = string.Empty;
        public DateTime ExpireAt { get; set; }
    }
}
