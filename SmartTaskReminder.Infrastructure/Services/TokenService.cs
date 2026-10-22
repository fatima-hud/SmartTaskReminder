using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SmartTaskReminder.Application.Dtos.Auth.Responses;
using SmartTaskReminder.Application.Services.Auth;
using SmartTaskReminder.Core.Entities;
using SmartTaskReminder.Core.Helping;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace SmartTaskReminder.Infrastructure.Services
{
    public class TokenService : ITokenService
    {
        private readonly JwtSettings _jwtSettings;

        public TokenService(IOptions<JwtSettings> jwtSettings)
        {
            _jwtSettings = jwtSettings.Value;
        }

        public JwtSecurityToken GenerateJwtToken(User user, string role)
        {
            var claims = new List<Claim>
    {
        new(ClaimTypes.Name, user.UserName
            ?? throw new ArgumentException("Username is required.")),
        new(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new(ClaimTypes.Role, role)
    };

            var secretKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));

            var credentials = new SigningCredentials(
                secretKey,
                SecurityAlgorithms.HmacSha256);

            return new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(
                    _jwtSettings.AccessTokenExpireMinute),
                signingCredentials: credentials);
        }
        public RefreshToken GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            RandomNumberGenerator.Fill(randomNumber);

            return new RefreshToken
            {
                Token = Convert.ToBase64String(randomNumber),
                ExpiresAt = DateTime.UtcNow.AddDays(
                    _jwtSettings.RefreshTokenExpireDay),
                CreatedAt = DateTime.UtcNow
            };
        }
    }
}
