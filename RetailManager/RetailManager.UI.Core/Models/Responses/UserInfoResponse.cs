using System;
using System.ComponentModel.DataAnnotations;

namespace RetailManager.UI.Core.Models.Responses
{
    public class UserInfoResponse
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [DataType(DataType.EmailAddress)]
        public string EmailAddress { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
