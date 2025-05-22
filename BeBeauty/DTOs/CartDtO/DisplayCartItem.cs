using BeBeauty.DTOs.ProductsDTos;

namespace BeBeauty.DTOs.CartDtO
{
    public class DisplayCartItem
    {
        public int CartItemId { get; set; }
        public string UserId { get; set; }
        public int ProductId { get; set; }
        public Displayproduct Product { get; set; } // Reusing your DisplayProduct DTO
        public int Quantity { get; set; }
    }
}
