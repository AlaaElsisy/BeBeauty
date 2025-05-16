using System.ComponentModel.DataAnnotations;

namespace BeBeauty.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        [Required]
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public decimal Price { get; set; }
        public decimal SalePrice { get; set; }
        public string Description { get; set; }

        // Foreign key
        public int CategoryId { get; set; }
        public Category Category { get; set; }

    }
}
