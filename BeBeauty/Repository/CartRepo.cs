using BeBeauty.Models;
using Microsoft.EntityFrameworkCore;

namespace BeBeauty.Repository
{
    public class CartRepo : GenericRepo<CartItem>
    {
        private readonly ApplicationDbContext _context;
        public CartRepo(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
            _context = applicationDbContext;
        }

        public IEnumerable<CartItem> GetCartByUserId(string userId)
        {
            return _context.CartItems
                .Include(c => c.Product)
                .Where(c => c.UserId == userId)
                .ToList();
        }

        public CartItem GetById(int id)
        {
            return _context.CartItems
                .Include(c => c.Product)
                .FirstOrDefault(c => c.CartItemId == id);
        }

        public Product GetProductById(int productId)
        {
            return _context.Products.Find(productId);
        }

        public void Add(CartItem cartItem)
        {
            
            var existingItem = _context.CartItems
                .FirstOrDefault(c => c.UserId == cartItem.UserId && c.ProductId == cartItem.ProductId);

            if (existingItem != null)
            {
                
                existingItem.Quantity += cartItem.Quantity;
                _context.CartItems.Update(existingItem);
            }
            else
            {
                
                _context.CartItems.Add(cartItem);
            }
        }

        //public void UpdateCartItem(CartItem cartItem)
        //{
        //    _context.CartItems.Update(cartItem);
        //}

        //public void RemoveFromCart(int cartItemId)
        //{
        //    var cartItem = _context.CartItems.Find(cartItemId);
        //    if (cartItem != null)
        //    {
        //        _context.CartItems.Remove(cartItem);
        //    }
        //}

        public void ClearCart(string userId)
        {
            var cartItems = _context.CartItems
                .Where(c => c.UserId == userId)
            .ToList();

            _context.CartItems.RemoveRange(cartItems);
        }

    }
}
