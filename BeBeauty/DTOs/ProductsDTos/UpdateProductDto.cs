namespace BeBeauty.DTOs.ProductsDTos
{
    public class UpdateProductDto
    {
        public int ProductId { get; set; }  
        public string Name { get; set; }
        public decimal Price { get; set; }
        public decimal SalePrice { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }

 
        public IFormFile? ImageFile { get; set; }
    }
}
