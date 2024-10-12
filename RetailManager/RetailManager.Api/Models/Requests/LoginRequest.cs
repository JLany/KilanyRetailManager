namespace RetailManager.Api.Models.Requests
{
    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Grant_type { get; set; }
    }
}
