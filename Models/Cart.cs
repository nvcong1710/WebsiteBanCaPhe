using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Security.Principal;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace WebsiteBanCaPhe.Models
{
    public class Cart
    {
        [Key]
        public int CartId { get; set; }
		
        [BindNever]
		public decimal TotalValue { get; set; }

        [ForeignKey("Account")]
        public int AccountId { get; set; }
        public Account? Account { get; set; }
    }
}
