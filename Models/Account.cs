using System.ComponentModel.DataAnnotations;

namespace WebsiteBanCaPhe.Models
{
    public class Account
    {
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
    }
}
