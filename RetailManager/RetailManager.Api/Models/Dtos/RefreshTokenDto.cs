namespace RetailManager.Api.Models.Dtos
{
    public class RefreshTokenDto
    {
        public string RefreshToken { get; set; }
        public DateTime Expiration { get; set; }
    }
}