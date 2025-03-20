using Microsoft.AspNetCore.Mvc;
using OnlineShopBE.Models;
using OnlineShopBE.Services;

namespace OnlineShopBE.Controllers
{
    [Route("api/carts")]
    [ApiController]
    public class CartController : Controller
    {
        private readonly OnlineShopContext _context;
        private readonly JwtService _jwtService;

        public CartController(OnlineShopContext context, JwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }
        // 1. Lấy giỏ hàng
        // 2. Thêm sản phẩm vào giỏ hàng
        // 3. Cập nhật số lượng sản phẩm trong giỏ hàng
        // 4. Xóa sản phẩm khỏi giỏ hàng
    }
}
