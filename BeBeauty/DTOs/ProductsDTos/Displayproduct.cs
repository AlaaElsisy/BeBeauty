using BeBeauty.Models;
using System.ComponentModel.DataAnnotations;

namespace BeBeauty.DTOs.ProductsDTos
{
    public class Displayproduct
    {

        public int ProductId { get; set; }
     
        public string Name { get; set; }
       
        public string ImageUrl { get; set; }
         public decimal Price { get; set; }

         public decimal? SalePrice { get; set; }


        public string? Description { get; set; }

         
        public int CategoryId { get; set; }
         
    }
}
