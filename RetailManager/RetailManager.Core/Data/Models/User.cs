using System;
using System.ComponentModel.DataAnnotations;

namespace RetailManager.Core.Data.Models
{
    public class User
    {
        public string Id { get; set; }
        [Required]
        [MinLength(2)]
        [MaxLength(100)]
        public string FirstName { get; set; }
        [Required]
        [MinLength(2)]
        [MaxLength(100)]
        public string LastName { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string EmailAddress { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
