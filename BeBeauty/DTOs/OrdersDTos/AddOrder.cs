using System.ComponentModel.DataAnnotations;

namespace BeBeauty.DTOs.OrdersDTos
{
    public class AddOrder
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        public string Status { get; set; }

        [Required]
        [ Range(1, 999999, ErrorMessage = "Total amount must be greater than or equal to 1.")]
        public decimal TotalAmount { get; set; }

        public List<OrderDetailDTO> OrderDetails { get; set; }
    }
}
