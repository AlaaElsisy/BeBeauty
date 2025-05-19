using System.ComponentModel.DataAnnotations;

namespace BeBeauty.DTOs.CatecoryDTos
{
    public class DisplayCategory
    {
        public int CategoryId { get; set; }
        [Required(ErrorMessage = "Category name is required.")]
        
        [StringLength(100, ErrorMessage = "Category name cannot exceed 100 characters.")]
        public string Name { get; set; }
    }
}
