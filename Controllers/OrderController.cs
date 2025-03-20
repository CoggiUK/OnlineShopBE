using Microsoft.AspNetCore.Mvc;
using OnlineShopBE.Models;
using OnlineShopBE.Services;

namespace OnlineShopBE.Controllers
{
    [Route("api/orders")]
    [ApiController]

    public class OrderController : Controller
    {
        private readonly OnlineShopContext _context;
        private readonly JwtService _jwtService;

        public OrderController(OnlineShopContext context, JwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }
        // 1. Lấy danh sách đơn hàng
        // 2. Xem chi tiết đơn hàng
        // 3. Tạo đơn hàng
        // 4. Cập nhật trạng thái đơn hàng
        // 5. Hủy đơn hàng
    }
}
