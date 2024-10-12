namespace RetailManager.Api.Models.Dtos
{
    public class TokenDto
    {
        public string AccessToken { get; set; }
        public DateTime Expiration {  get; set; }
    }
}
