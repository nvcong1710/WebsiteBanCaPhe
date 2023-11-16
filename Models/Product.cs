using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebsiteBanCaPhe.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        [Required]
        public string? ProductName { get; set; }

        [Required]
        public string? ProductDescription { get; set; }

        [Required]
        public string? PhotoURL { get; set; }

        [Required]
        public string? Origin { get; set; }

        [Required]
        public string? Branch { get; set; }

        [Required]
        public long Price { get; set; }

        [Required]
        public int Quantity { get; set; }

        public int QuantitySold { get; set; } = 0;

        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public virtual Category? Category { get; set; }
    }
}
