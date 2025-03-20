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
        // 2. Xem lịch sử thanh toán

    }
}
