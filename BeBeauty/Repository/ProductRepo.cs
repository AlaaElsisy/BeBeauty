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
    }
     
}
