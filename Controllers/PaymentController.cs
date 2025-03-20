using Microsoft.AspNetCore.Mvc;
using OnlineShopBE.Models;
using OnlineShopBE.Services;

namespace OnlineShopBE.Controllers
{
    [Route("api/payments")]
    [ApiController]
    public class PaymentController : Controller
    {
        private readonly OnlineShopContext _context;
        private readonly JwtService _jwtService;

        public PaymentController(OnlineShopContext context, JwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        // 1. Thanh toán đơn hàng
        [HttpPost("pay")]
        public IActionResult PayOrder([FromBody] Order order)
        {
            if (order == null || order.OrderDetails == null || !order.OrderDetails.Any())
            {
                return BadRequest(new { Message = "Invalid order details." });
            }

            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var customerId = _jwtService.GetCustomerIdFromToken(token);

            order.CustomerId = customerId;
            order.OrderDate = DateTime.UtcNow;
            order.Status = "Paid";

            _context.Orders.Add(order);
            _context.SaveChanges();

            return Ok(new { Message = "Order payment successful.", OrderId = order.Id });
        }

        // 2. Xem lịch sử thanh toán

        [HttpGet("history")]
        public IActionResult GetPaymentHistory()
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var customerId = _jwtService.GetCustomerIdFromToken(token);

            var orders = _context.Orders
                .Where(o => o.CustomerId == customerId)
                .ToList();

            if (orders == null || !orders.Any())
            {
                return NotFound(new { Message = "No payment history found for this customer." });
            }

            return Ok(orders);
        }
    }
}
