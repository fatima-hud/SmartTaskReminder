using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using SmartTaskReminder.Application.Dtos.Auth.Requests;
using SmartTaskReminder.Application.Dtos.Auth.Responses;
using SmartTaskReminder.Application.Dtos.Auths.Requests;
using SmartTaskReminder.Application.Interfaces;
using SmartTaskReminder.Core.Entities;
using SmartTaskReminder.Core.Helping;
using SmartTaskReminder.Core.Results;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Options;
namespace SmartTaskReminder.Application.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasherService _passwordHasher;

        private readonly ITokenService _tokenService;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private JwtSettings _jwtSettings { get; }
        private readonly ILogger<AuthService> _logger;
        public AuthService(IUserRepository userRepository, IRoleRepository roleRepository, IUnitOfWork unitOfWork, IPasswordHasherService passwordHasherService, ITokenService tokenService, IRefreshTokenRepository refreshTokenRepository, IOptions<JwtSettings> jwtSettings, ILogger<AuthService> logger)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _unitOfWork = unitOfWork;
            _passwordHasher = passwordHasherService;

            _tokenService = tokenService;
            _refreshTokenRepository = refreshTokenRepository;
            _jwtSettings = jwtSettings.Value;
            _logger = logger;
        }

        public async Task<Result<ResponseLogInDto>> LoginInAsync(RequestLoginDto request)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email);
            if (user is null)
            {
                return Result<ResponseLogInDto>.UnAuthorized("email or password is invalid");
            }
            var passwordResult = await _passwordHasher.VerifyPassword(user, request.Password);

            if (!passwordResult)
            {
                return Result<ResponseLogInDto>.UnAuthorized("email or password is invalid");

            }

            var role = await _roleRepository.GetRoleByUserAsync(user.Id);
            if (role is null)
            {
                return Result<ResponseLogInDto>.UnAuthorized("user role is not found");
            }
            var jwt = await GetJWTAndRerfreshToken(user, role.Name);
            var ret = new ResponseLogInDto
            {
                UserId = user.Id,
                Username = user.UserName,
                FullName = $"{user.FirstName} {user.LastName}",
                Email = user.Email,
                AccessToken = jwt.AccessToken,
                RefreshTokenString = jwt.RefreshToken,
                ExpireAt = jwt.ExpireAt
            };

            return Result<ResponseLogInDto>.Success(ret);
        }

        public async Task<Result> RegisterAsync(RequestRegisterDto request)
        {
            var existUserName = await _userRepository.GetByUserNameAsync(request.UserName);

            if (existUserName != null)
            {
                return Result.BadRequest("userName is already taken");
            }


            var existUser = await _userRepository.GetByEmailAsync(request.Email);

            if (existUser != null)
            {
                return Result.BadRequest("email is already taken");
            }

            var isValidEmail = await CheckFormEmailAsync(request.Email);

            if (!isValidEmail)
            {
                return Result.BadRequest("The email address must contain a valid domain name and symbol:@");
            }
            var today = DateTime.UtcNow.Date;
            var birthDate = request.BirthDate.Date;
            if (birthDate >= today)
            {
                return Result.BadRequest("Birth date must be in the past.");
            }
            var checkPasswordResult = await CheckValidPassword(request.Password);
            if (!checkPasswordResult.IsSuccess)
            {
                return Result.BadRequest(checkPasswordResult.Message);
            }

            var defaultRoleId = await _roleRepository.GetRoleIdByNameAsync("User");

            if (!defaultRoleId.HasValue)
            {
                return Result.NotFound("default role is not found");
            }
            var user = new User
            {
                UserName = request.UserName,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Gender = request.Gender,
                RoleId = defaultRoleId.Value,
                BirthDate = request.BirthDate

            };

            user.PasswordHash = await _passwordHasher.HashPassword(user, request.Password);



            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();
            return Result.Success();


        }
        public async Task<bool> CheckFormEmailAsync(string email)
        {

            string pattern = @"^[a-zA-Z0-9._%+-]+@gmail\.com$";

            Regex regex = new Regex(pattern);
            return regex.IsMatch(email);
        }

        public async Task<JwtAuthResult> GetJWTAndRerfreshToken(User user, string roleName)
        {
            var jwt = _tokenService.GenerateJwtToken(user, roleName);

            var accessToken = new JwtSecurityTokenHandler().WriteToken(jwt);


            var refreshToken = _tokenService.GenerateRefreshToken();

            var userRefreshToken = new RefreshToken
            {
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = refreshToken.ExpiresAt,
                IsRevoked = false,
                Token = refreshToken.Token,
                UserId = user.Id
            };

            await _refreshTokenRepository.AddAsync(userRefreshToken);

            var response = new JwtAuthResult
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken.Token,
                ExpireAt = refreshToken.ExpiresAt
            };

            return response;
        }


        public async Task<Result> CheckValidPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password) || password.Length < 8)
            {
                return Result.BadRequest("Password must be at least 8 characters long.");

            }
            if (!Regex.IsMatch(password, @"[A-Z]"))
            {
                return Result.BadRequest("Password must contain at least one uppercase letter.");
            }

            if (!Regex.IsMatch(password, @"[a-z]"))
            {
                return Result.BadRequest("Password must contain at least one lowercase letter.");
            }


            if (!Regex.IsMatch(password, @"[0-9]"))
            {
                return Result.BadRequest("Password must contain at least one number.");
            }


            if (!Regex.IsMatch(password, @"[^a-zA-Z0-9]"))
            {
                return Result.BadRequest("Password must contain at least one special character.");
            }

            return Result.Success();

        }

        public async Task<Result> LogoutAsync(int userId, RequestLogOutDto logOutDto)
        {
            var token = await _refreshTokenRepository.GetByTokenAsync(logOutDto.RefreshToken);
            if (token is null)
            {
                return Result.Failure("Refresh token not found.");
            }
            if (token.UserId != userId)
            {
                return Result.UnAuthorized("RefreshToken does not belong to the current user.");
            }
            if (token.IsExpired)
            {
                return Result.BadRequest("Refresh token has expired");
            }
            if (token.IsRevoked)
            {
                return Result.BadRequest("Refresh token was already revoked");
            }
            token.IsRevoked = true;
            await _refreshTokenRepository.UpdateAsync(token);
            return Result.Success("تم تسجيل الخروج بنجاح");
        }



        public async Task<Result<JwtAuthResult>> RefreshAccessTokenAsync(
            RequestRefreshAccessTokenDto request)
        {
            try
            {
                var principal = GetPrincipalFromExpiredToken(request.AccessToken);

                var userIdValue = principal.FindFirst(
                    ClaimTypes.NameIdentifier)?.Value;

                if (!int.TryParse(userIdValue, out var userId))
                    return Result<JwtAuthResult>.BadRequest(
                        "Invalid Access Token.");

                var user = await _userRepository.GetByIdAsync(userId);

                if (user == null || user.IsDeleted)
                    return Result<JwtAuthResult>.NotFound(
                        "User not found.");

                var validationResult = await ValidateRefreshTokenAsync(
                    request.RefreshToken,
                    userId);

                if (!validationResult.IsSuccess)
                    return Result<JwtAuthResult>.Failure(
                        validationResult.Message,
                        validationResult.Errors);

                var oldRefreshToken =
                    await _refreshTokenRepository.GetByTokenAsync(
                        request.RefreshToken);

                if (oldRefreshToken == null)
                    return Result<JwtAuthResult>.BadRequest(
                        "Invalid Refresh Token.");

                var role = await _roleRepository.GetRoleByUserAsync(userId);

                if (role == null)
                    return Result<JwtAuthResult>.Failure(
                        "User role not found.");

                await _refreshTokenRepository.DeleteAsync(oldRefreshToken);

                var result = await GetJWTAndRerfreshToken(user, role.Name);

                return Result<JwtAuthResult>.Success(result);
            }
            catch (SecurityTokenException ex)
            {
                _logger.LogWarning(
                    ex, "Invalid token received during token refresh.");

                return Result<JwtAuthResult>.BadRequest(
                    "Invalid or expired token.");
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex, "An error occurred while refreshing the token.");

                return Result<JwtAuthResult>.Failure(
                    "An internal error occurred.");
            }
        }


        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = _jwtSettings.validateIssuer,
                ValidateAudience = _jwtSettings.validateAudience,
                ValidateIssuerSigningKey = _jwtSettings.validateIssuerSigningKey,

                ValidIssuer = _jwtSettings.Issuer,
                ValidAudience = _jwtSettings.Audience,

                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(_jwtSettings.SecretKey)),

                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var principal = tokenHandler.ValidateToken(
                token,
                tokenValidationParameters,
                out var securityToken);

            if (securityToken is not JwtSecurityToken jwtSecurityToken ||
                !string.Equals(
                    jwtSecurityToken.Header.Alg,
                    SecurityAlgorithms.HmacSha256,
                    StringComparison.Ordinal))
            {
                throw new SecurityTokenException("Invalid token.");
            }

            return principal;
        }

        private async Task<Result> ValidateRefreshTokenAsync(
            string refreshToken,
            int userId)
        {
            var storedToken =
                await _refreshTokenRepository.GetByTokenAsync(refreshToken);

            if (storedToken == null)
                return Result.Failure("Invalid refresh token.");

            if (storedToken.UserId != userId)
                return Result.Failure("User mismatch.");

            if (storedToken.IsRevoked)
                return Result.Failure("Refresh token revoked.");

            if (storedToken.ExpiresAt <= DateTime.UtcNow)
                return Result.Failure("Refresh token expired.");

            return Result.Success();
        }


    }
}
