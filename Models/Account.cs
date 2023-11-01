using System.ComponentModel.DataAnnotations;

namespace WebsiteBanCaPhe.Models
{
    public class Account
    {
        public Account()
        {
        }

        [Key]
        public int AccountId { get; set; }

        [Required]
        public string? PhoneNumber { get; set; }

        [Required]
        public string? Password { get; set; }

        [Required]
        public string? FullName { get; set; }

        [Required]
        public string? Gender { get; set; }

        public Account(int accountId, string? phoneNumber, string? password, string? fullName, string? gender)
        {
            AccountId = accountId;
            PhoneNumber = phoneNumber;
            Password = password;
            FullName = fullName;
            Gender = gender;
        }
    }
}
