using SmartTaskReminder.Core.Entities;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace SmartTaskReminder.Application.Services.Auth
{
    public interface ITokenService
    {
        public JwtSecurityToken GenerateJwtToken(User user, string role);
        public RefreshToken GenerateRefreshToken();
    }
}
