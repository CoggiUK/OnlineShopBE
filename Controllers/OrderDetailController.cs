using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineShopBE.Models;
using OnlineShopBE.Services;

namespace OnlineShopBE.Controllers
{
    [Route("api/order-details")]
    [ApiController]

    public class OrderDetailController : Controller
    {
        private readonly OnlineShopContext _context;
        private readonly JwtService _jwtService;

        public OrderDetailController(OnlineShopContext context, JwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        // 1. Lấy danh sách sản phẩm trong đơn hàng	
        [Authorize]
        [HttpGet("/{orderId}")]
        public IActionResult GetOrderDetails(int orderId)
        {
            var orderDetails = _context.OrderDetails.Where(s => s.Id == orderId);
            return Ok(orderDetails);
        }
    }
}
