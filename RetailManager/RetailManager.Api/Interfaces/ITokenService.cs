using RetailManager.Api.Models.Dtos;
using System.Security.Claims;

namespace RetailManager.Api.Interfaces
{
    public interface ITokenService
    {
        RefreshTokenDto GenerateRefreshToken();
        Task<TokenDto> GenerateTokenAsync(string username);
        ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
    }
}
