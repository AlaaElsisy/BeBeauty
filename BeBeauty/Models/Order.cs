namespace BeBeauty.Models
{
    public class Order
    {

        public int OrderId { get; set; }
        public string UserId { get; set; }
        //public ApplicationUser User { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }
        public decimal TotalAmount { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
