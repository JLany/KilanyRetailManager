namespace RetailManager.Api.Utils.Jwt
{
    public interface ITokenGenerator
    {
        Task<object?> GenerateToken(string username);
    }
}
