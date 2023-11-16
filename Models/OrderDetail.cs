using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace WebsiteBanCaPhe.Models
{
    public class OrderDetail
    {
        [Key]
        public int OrderDetailId { get; set; }
        [ForeignKey("UserOrder")]
        public int OrderId { get; set; }
        public UserOrder? UserOrder { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public Product Product { get; set; }

        [Required]
        public long Quantity { get; set; }

        [BindNever]
        public long TotalPrice { get; set; }
    }
}
