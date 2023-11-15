using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Security.Principal;

namespace WebsiteBanCaPhe.Models
{
    public class UserOrder
    {
        [Key]
        public int OrderId { get; set; }

        [Required]
        public DateTime OrderDate { get; set; }

        [Required]
        public string? ReceiverName { get; set; }

        [Required]
		[RegularExpression(@"^0[0-9]{9,10}$", ErrorMessage = "Số điện thoại không hợp lệ")]
		public string? PhoneNumber { get; set; }

        [Required]
        public string? Address { get; set; }

        [Required]
        public string? PaymentMethod { get; set; }

        public string? Note { get; set; } = "không có ghi chú";

        public decimal ShippingFee { get; set; } = 0;

        public decimal TotalValue { get; set; } = 0;

        [Required]
        public bool IsDone { get; set; } = false;

        public bool IsPaid { get; set; } = false;

        [ForeignKey("Account")]
        public int AccountId { get; set; }
        public virtual Account? Account { get; set; }
    }
}
