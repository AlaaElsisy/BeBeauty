using System.ComponentModel.DataAnnotations;

namespace BeBeauty.DTOs.ProductsDTos
{
    public class AddProduct
    {

    
        [Required]
        public string Name { get; set; }
        [Required]
        public string ImageUrl { get; set; }
        [Range(1, 999999, ErrorMessage = "Price must be greater than or equal to 1.")]
        public decimal Price { get; set; }

        [Range(1, 999999, ErrorMessage = "Sale Price must be less than or equal to 1.")]
        public decimal? SalePrice { get; set; }


        public string? Description { get; set; }

        [Required]
        public int CategoryId { get; set; }
    }
}
