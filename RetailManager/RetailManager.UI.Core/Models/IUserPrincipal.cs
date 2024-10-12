using System;

namespace RetailManager.UI.Core.Models
{
    public interface IUserPrincipal
    {
        DateTime CreatedDate { get; set; }
        string EmailAddress { get; set; }
        string FirstName { get; set; }
        string Id { get; set; }
        string LastName { get; set; }
        string Token { get; set; }
        DateTime Expiration {  get; set; }
        string RefreshToken {  get; set; }
    }
}