
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RetailManager.Api.Configuration;
using RetailManager.Api.Data.Context;
using RetailManager.Api.Data.Entities;
using RetailManager.Api.Interfaces;
using RetailManager.Api.Models.Dtos;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace RetailManager.Api.Services
{
    public class JwtTokenService : ITokenService
    {
        private readonly UserManager<RetailManagerAuthUser> _userManager;
        private readonly RetailManagerAuthContext _context;
        private readonly IUserRoleService _userRoles;
        private readonly JwtSettings _jwtSettings;

        public JwtTokenService(
            UserManager<RetailManagerAuthUser> userManager,
            RetailManagerAuthContext context,
            IOptions<JwtSettings> jwtSettings,
            IUserRoleService userRoles)
        {
            _userManager = userManager;
            _context = context;
            _userRoles = userRoles;
            _jwtSettings = jwtSettings.Value;
        }

        public async Task<TokenDto> GenerateTokenAsync(string username)
        {
            var user = await _userManager.FindByEmailAsync(username);
            // No check for null here because we checked credentials already
            // before trying to generate a token.

            var userRoles = _userRoles.GetUserRoles(user.Id);

            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, username),
                new(ClaimTypes.NameIdentifier, user.Id),
                new(JwtRegisteredClaimNames.Nbf, new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds().ToString()),
                new(JwtRegisteredClaimNames.Exp, new DateTimeOffset(DateTime.Now.AddMinutes(_jwtSettings.AccessTokenExpirationMinutes)).ToUnixTimeSeconds().ToString())
            };

            foreach (var role in userRoles)
            {
                claims.Add(new(ClaimTypes.Role, role.RoleName));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpirationMinutes),
                signingCredentials: creds);


            return new TokenDto
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = token.ValidTo
            };
        }

        public RefreshTokenDto GenerateRefreshToken()
        {
            var randomNumber = new byte[32];

            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return new RefreshTokenDto
            {
                RefreshToken = Convert.ToBase64String(randomNumber),
                Expiration = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationDays)
            };
        }

        public ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
        {
            var secretKey = Encoding.UTF8.GetBytes(_jwtSettings.SecretKey);

            var validation = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _jwtSettings.Issuer,
                ValidAudience = _jwtSettings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(secretKey),
                ValidateLifetime = false,
            };

            return new JwtSecurityTokenHandler().ValidateToken(token, validation, out _);
        }
    }
}
