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
        public string? PhoneNumber { get; set; }

        [Required]
        public string? Address { get; set; }

        [Required]
        public string? PaymentMethod { get; set; }

        public string? Note { get; set; }

        [Required]
        public decimal ShippingFee { get; set; }

        [Required]
        public decimal TotalValue { get; set; }

        
        [Required]
        public bool IsDone { get; set; }

        [ForeignKey("Account")]
        public int AccountId { get; set; }
        public virtual Account? Account { get; set; }
    }
}
