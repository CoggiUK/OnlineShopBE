using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineShopBE.DTOModes.UserModels;
using OnlineShopBE.Models;
using OnlineShopBE.Services;
using System.Security.Cryptography;
using System.Text;

namespace OnlineShopBE.Controllers
{
    [Route("api/auth")]
    [ApiController]

    public class AuthenticationController : Controller
    {
        private readonly OnlineShopContext _context;
        private readonly JwtService _jwtService;

        public AuthenticationController(OnlineShopContext context, JwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }
        // 1. Đăng ký tài khoản
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            if (await _context.Users.AnyAsync(u => u.Username == model.Username))
                return BadRequest(new { message = "Username already exists" });

            var hashedPassword = HashPassword(model.Password);
            var user = new User
            {
                Username = model.Username,
                Email = model.Email,
                PasswordHash = hashedPassword,
                Role = model.Role ?? "Customer"
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new { message = "User registered successfully" });
        }

        // 2. Đăng nhập và nhận JWT Token
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == model.Username);
            if (user == null || !VerifyPassword(model.Password, user.PasswordHash))
                return Unauthorized(new { message = "Invalid username or password" });

            var roles = new List<string> { user.Role };
            var token = _jwtService.GenerateJwtToken(user, roles);

            return Ok(new
            {
                userId = user.Id,
                username = user.Username,
                email = user.Email,
                role = user.Role,
                token = token,
                tokenExpiration = DateTime.UtcNow.AddMinutes(60)
            });
        }
        // Mã hóa mật khẩu SHA-256
        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }

        // Xác minh mật khẩu
        private bool VerifyPassword(string password, string storedHash)
        {
            return HashPassword(password) == storedHash;
        }


    }
}
