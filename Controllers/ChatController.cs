using Microsoft.AspNetCore.Mvc;
using OnlineShopBE.Models;
using OnlineShopBE.Services;

namespace OnlineShopBE.Controllers
{
    [Route("api/chat")]
    [ApiController]
    public class ChatController : Controller
    {
        private readonly OnlineShopContext _context;
        private readonly JwtService _jwtService;

        public ChatController(OnlineShopContext context, JwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }
        // 1. Gửi tin nhắn đến AI
        // 2. Xóa tin nhắn
    }
}
