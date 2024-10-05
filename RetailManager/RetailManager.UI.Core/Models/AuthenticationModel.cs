namespace RetailManager.UI.Core.Models
{
    public class AuthenticationModel
    {
        public string Access_Token { get; set; }
        public string UserName { get; set; }
        public bool IsAuthenticated
            => !string.IsNullOrWhiteSpace(Access_Token);
        public string Error { get; set; }
        public string Error_Description { get; set; }
    }
}
