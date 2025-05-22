using AutoMapper;
using BeBeauty.DTOs.CartDtO;
using BeBeauty.Models;
using BeBeauty.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BeBeauty.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly CartRepo cartRepo;
        private readonly IMapper mapper;

        public CartController(CartRepo _cartRepo, IMapper _mapper)
        {
            cartRepo = _cartRepo;
            mapper = _mapper;
        }

        [HttpGet("user/{userId}")]
        [Authorize(Roles = "Admin,User")] 
        public IActionResult GetCartByUser(string userId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(userId))
                {
                    return BadRequest("User ID cannot be empty.");
                }

                var cartItems = cartRepo.GetCartByUserId(userId);

                if (cartItems == null || !cartItems.Any())
                {
                    return NotFound("No items found in cart.");
                }

                var cartDtos =  mapper.Map<List<DisplayCartItem>>(cartItems);
                return Ok(cartDtos);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
       [Authorize(Roles = "User")]  
        public IActionResult AddToCart([FromBody] AddCartItem cartItemDto)
        {
            try
            {
                if (cartItemDto == null)
                {
                    return BadRequest("Invalid cart item data.");
                }

                if (!ModelState.IsValid)
                {
                    var errors = ModelState
                        .Where(kvp => kvp.Value.Errors.Any())
                        .ToDictionary(
                            kvp => kvp.Key,
                            kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                        );
                    return BadRequest(new { Errors = errors });
                }

                // Verify product exists and has sufficient stock
                var product =  cartRepo.GetProductById(cartItemDto.ProductId);
                if (product == null)
                {
                    return NotFound("Product not found.");
                }

                var cartItem =  mapper.Map<CartItem>(cartItemDto);
               cartRepo.Add(cartItem);
                cartRepo.Save();

                var result = mapper.Map<DisplayCartItem>(cartItem);
                return CreatedAtAction(nameof(GetCartItem), new { id = cartItem.CartItemId }, result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetCartItem(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("Invalid cart item ID.");
                }

                var cartItem =  cartRepo.GetById(id);
                if (cartItem == null)
                {
                    return NotFound("Cart item not found.");
                }

               
                var currentUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                //if (cartItem.UserId != currentUserId && !User.IsInRole("Admin"))
                //{
                //    Console.WriteLine("User ID: " + currentUserId);
                //    return Forbid();
                //}

                var cartItemDto = mapper.Map<DisplayCartItem>(cartItem);
                return Ok(cartItemDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "User")]
        public IActionResult UpdateCartItem(int id, [FromBody] UpdateCartItem cartItemDto)
        {
            try
            {
                if (id <= 0 || cartItemDto == null)
                {
                    return BadRequest("Invalid data.");
                }

                if (id != cartItemDto.CartItemId)
                {
                    return BadRequest("ID mismatch.");
                }

                if (!ModelState.IsValid)
                {
                    var errors = ModelState
                        .Where(kvp => kvp.Value.Errors.Any())
                        .ToDictionary(
                            kvp => kvp.Key,
                            kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                        );
                    return BadRequest(new { Errors = errors });
                }

                var existingItem =  cartRepo.GetById(id);
                if (existingItem == null)
                {
                    return NotFound("Cart item not found.");
                }

                // Verify user owns this cart item
                var currentUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                //if (existingItem.UserId != currentUserId)
                //{
                //    return Forbid();
                //}

                mapper.Map(cartItemDto, existingItem);
                cartRepo.Update(existingItem);
                 cartRepo.Save();

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult RemoveFromCart(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("Invalid cart item ID.");
                }

                var cartItem =  cartRepo.GetById(id);
                if (cartItem == null)
                {
                    return NotFound("Cart item not found.");
                }

                // Verify user owns this cart item or is admin
                var currentUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                //if (cartItem.UserId != currentUserId && !User.IsInRole("Admin"))
                //{
                //    return Forbid();
                //}

                var cartItemDto =  mapper.Map<DisplayCartItem>(cartItem);
                 cartRepo.Delete(id);
                 cartRepo.Save();

                return Ok(cartItemDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("clear/{userId}")]
        [Authorize(Roles = "User,Admin")]
        public IActionResult ClearCart(string userId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(userId))
                {
                    return BadRequest("User ID cannot be empty.");
                }

               
                var currentUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                //if (userId != currentUserId && !User.IsInRole("Admin"))
                //{
                //    return Forbid();
                //}

                cartRepo.ClearCart(userId);
               cartRepo.Save();

                return Ok("Cart cleared successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

