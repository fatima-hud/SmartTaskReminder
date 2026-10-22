using System;
using System.Collections.Generic;
using System.Text;

namespace SmartTaskReminder.Core.Entities
{
    public class RefreshToken:BaseEntity
    {
       
        public string Token { get; set; } = string.Empty;
        public int UserId { get; set; }
        public User User { get; set; } = null!;
        public DateTime ExpiresAt { get; set; }
        public bool IsExpired => DateTime.UtcNow >= ExpiresAt;
        public bool IsRevoked { get; set; }
        public void Revoke()
        {
            IsRevoked = true;
        }
        public bool IsActive()
        {
            return !IsRevoked && !IsExpired;
        }
    }
}
