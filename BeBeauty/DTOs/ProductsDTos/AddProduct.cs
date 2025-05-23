using System.ComponentModel.DataAnnotations;

namespace BeBeauty.DTOs.ProductsDTos
{
    public class AddProduct
    {


        [Required(ErrorMessage = "Product name is required.")]
        [StringLength(100, ErrorMessage = "Product name cannot exceed 100 characters.")]
        public string Name { get; set; }
        [Required]
        //public string ImageUrl { get; set; }
        [Range(1, 999999, ErrorMessage = "Price must be greater than or equal to 1.")]
        public decimal Price { get; set; }

        [Range(1, 999999, ErrorMessage = "Sale Price must be less than or equal to 1.")]
        public decimal? SalePrice { get; set; }


        public string? Description { get; set; }

        [Required]
        public int CategoryId { get; set; }
        public IFormFile ImageFile { get; set; }
    }
}
