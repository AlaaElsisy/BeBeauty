using BeBeauty.Models.identity;
using BeBeauty.Models;

namespace BeBeauty.DTOs.OrdersDTos
{
    public class DisplayOrder
    {
        public int OrderId { get; set; }
        public string UserId { get; set; }
        
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }
        public int TotalAmount { get; set; }
        public List<OrderDetailDTO> OrderDetails { get; set; }
    }
}
