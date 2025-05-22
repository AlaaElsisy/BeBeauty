using System;
using BeBeauty.Models;

namespace BeBeauty.Repository
{
    public class ProductRepo:GenericRepo<Product> 
    {
        private readonly   ApplicationDbContext con;
        public ProductRepo(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
            con = applicationDbContext;
        }

        public List<Product> GetProductsByCategoryId(int categoryId)
        {
            return con.Products.Where(p => p.CategoryId == categoryId).ToList();
        }
        public List<Product> GetProductsByName(string productName)
        {
           
            return con.Products
               .Where(p => p.Name.ToLower().Contains(productName.ToLower()))
               .ToList();
        }

        public List<Product> GetProductsByDescription(string description)
        {
            return con.Products
                          .Where(p => p.Description.Contains(description))
                          .ToList();
        }

    }

}
