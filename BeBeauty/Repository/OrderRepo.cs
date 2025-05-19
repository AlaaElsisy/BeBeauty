using BeBeauty.Models;
using Microsoft.EntityFrameworkCore;

namespace BeBeauty.Repository
{
    public class OrderRepo:GenericRepo<Order>
    {
        private readonly ApplicationDbContext con;
        public OrderRepo(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
            con = applicationDbContext;
        }
        
        public  List<Order>  GetAll()
        {
            

            return con.Orders
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product).ToList();
               
        }
        public Order GetById(int id)
        {
            return con.Orders
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                .FirstOrDefault(o => o.OrderId == id);
        }
        public List<Order> GetOrdersByUserId(string userId)
        {
            return con.Orders
                .Include(o => o.User)
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                .Where(o => o.UserId == userId)
                .ToList();
        }

        public List<Order> GetOrdersByStatus(string status)
        {
            return con.Orders
                .Include(o => o.User)
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                .Where(o => o.Status == status)
                .ToList();
        }
    }
}
