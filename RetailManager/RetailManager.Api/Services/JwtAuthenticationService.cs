using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RetailManager.Api.Configuration;
using RetailManager.Api.Data.Entities;
using RetailManager.Api.Exceptions;
using RetailManager.Api.Interfaces;
using RetailManager.Api.Models.Requests;
using RetailManager.Api.Models.Responses;
using System.Security.Claims;
using System.Text;

namespace RetailManager.Api.Services
{
    public class JwtAuthenticationService : IAuthService
    {
        private readonly UserManager<RetailManagerAuthUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly JwtSettings _jwtSettings;

        public JwtAuthenticationService(
            UserManager<RetailManagerAuthUser> userManager,
            ITokenService tokenService,
            IOptions<JwtSettings> jwtSettings)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _jwtSettings = jwtSettings.Value;
        }

        public async Task<LoginResponse> LoginAsync(LoginRequest login)
        {
            var user = await _userManager.FindByEmailAsync(login.Username);
            bool validCredentials = await _userManager.CheckPasswordAsync(user, login.Password);

            if (!validCredentials)
            {
                throw new UnauthorizedException();
            }

            var token = await _tokenService.GenerateTokenAsync(login.Username);
            var refreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = refreshToken.RefreshToken;
            user.RefreshTokenExpiry = refreshToken.Expiration;

            await _userManager.UpdateAsync(user);

            return new LoginResponse
            {
                AccessToken = token.AccessToken,
                Username = user.UserName,
                RefreshToken = refreshToken.RefreshToken,
                Expiration = token.Expiration
            };
        }

        public async Task<LoginResponse> RefreshTokenAsync(RefreshTokenRequest request)
        {
            var principal = _tokenService.GetPrincipalFromExpiredToken(request.AccessToken);

            if (principal?.Identity?.Name is null)
            {
                throw new UnauthorizedException();
            }

            var user = await _userManager.FindByNameAsync(principal.Identity.Name);

            if (user is null 
                || user.RefreshToken != request.RefreshToken 
                || user.RefreshTokenExpiry < DateTime.UtcNow)
            {
                throw new UnauthorizedException();
            }

            var token = await _tokenService.GenerateTokenAsync(principal.Identity.Name);

            return new LoginResponse
            {
                AccessToken = token.AccessToken,
                Username = user.UserName,
                RefreshToken = request.RefreshToken,
                Expiration = token.Expiration,
            };
        }

        public async Task LogoutAsync(string? username)
        {
            if (username is null)
            {
                throw new UnauthorizedException();
            }

            var user = await _userManager.FindByNameAsync(username);

            if (user is null)
            {
                throw new UnauthorizedException();
            }

            user.RefreshToken = null;

            await _userManager.UpdateAsync(user);
        }
    }
}
