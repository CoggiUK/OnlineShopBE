using Microsoft.AspNetCore.Mvc;
using OnlineShopBE.Models;
using OnlineShopBE.Services;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace OnlineShopBE.Controllers
{
    [Route("api/carts")]
    [ApiController]
    [Authorize]
    public class CartController : Controller
    {
        private readonly OnlineShopContext _context;
        private readonly JwtService _jwtService;

        public CartController(OnlineShopContext context, JwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        // 1. Get the list of products in the cart
        [HttpGet]
        public async Task<IActionResult> GetCartItems()
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var customerId = _jwtService.GetCustomerIdFromToken(token);
            var cartItems = _context.Carts
                .Where(c => c.CustomerId == customerId)
                .Select(c => new
                {
                    c.ProductId,
                    c.Quantity,
                    c.Product.Name,
                    c.Product.Price
                })
                .ToList();

            return Ok(cartItems);
        }

        // 2. Add a product to the cart
        [HttpPost]
        public async Task<IActionResult> AddToCart([FromBody] Cart cart)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var customerId = _jwtService.GetCustomerIdFromToken(token);
            cart.CustomerId = customerId;

            var existingCartItem = _context.Carts
                .FirstOrDefault(c => c.CustomerId == cart.CustomerId && c.ProductId == cart.ProductId);

            if (existingCartItem != null)
            {
                existingCartItem.Quantity += cart.Quantity;
            }
            else
            {
                _context.Carts.Add(cart);
            }

            await _context.SaveChangesAsync();
            return Ok(cart);
        }

        // 3. Update the quantity of a product in the cart
        [HttpPut("{productId}")]
        public async Task<IActionResult> UpdateCartItem(int productId, [FromBody] int quantity)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var customerId = _jwtService.GetCustomerIdFromToken(token);
            var cartItem = _context.Carts
                .FirstOrDefault(c => c.CustomerId == customerId && c.ProductId == productId);

            if (cartItem == null)
            {
                return NotFound();
            }

            cartItem.Quantity = quantity;
            await _context.SaveChangesAsync();
            return Ok(cartItem);
        }

        // 4. Remove a product from the cart
        [HttpDelete("{productId}")]
        public async Task<IActionResult> RemoveFromCart(int productId)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var customerId = _jwtService.GetCustomerIdFromToken(token);
            var cartItem = _context.Carts
                .FirstOrDefault(c => c.CustomerId == customerId && c.ProductId == productId);

            if (cartItem == null)
            {
                return NotFound();
            }

            _context.Carts.Remove(cartItem);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
