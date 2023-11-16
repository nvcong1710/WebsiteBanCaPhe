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
		[RegularExpression(@"^0[0-9]{9,10}$", ErrorMessage = "Số điện thoại không hợp lệ")]
		public string? PhoneNumber { get; set; }

        [Required]
		[RegularExpression(@"^.{6,}$", ErrorMessage = "Mật khẩu chứa ít nhất 6 kí tự")]
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
