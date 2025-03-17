using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineShopBE.DTOModes.UserModels;
using OnlineShopBE.Models;
using OnlineShopBE.Services;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace OnlineShopBE.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly OnlineShopContext _context;
        private readonly JwtService _jwtService;

        public UsersController(OnlineShopContext context, JwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }


        // 1. Lấy thông tin cá nhân của user hiện tại (dựa trên JWT)
        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> GetProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); 

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { message = "Invalid token or user ID not found" });
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id.ToString() == userId);
            if (user == null) return NotFound(new { message = "User not found" });

            return Ok(new { user.Username, user.Email, Role = user.Role });
        }

        // 2. Lấy danh sách tất cả người dùng (chỉ Admin)
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _context.Users
                .Select(u => new { u.Id, u.Username, u.Email, Role = u.Role })
                .ToListAsync();

            return Ok(users);
        }

        // 3. Lấy thông tin chi tiết của một người dùng (chỉ Admin)
        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return NotFound(new { message = "User not found" });

            return Ok(new { user.Id, user.Username, user.Email, Role = user.Role });
        }

        // 4. Cập nhật thông tin người dùng
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserModel model)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return NotFound(new { message = "User not found" });

            // Chỉ admin hoặc chính user đó mới được phép cập nhật
            var requestingUser = User.FindFirstValue(ClaimTypes.Name);
            var requestingRole = User.FindFirstValue(ClaimTypes.Role);

            if (requestingRole != "Admin" && user.Username != requestingUser)
                return Forbid();

            user.Email = model.Email ?? user.Email;
            if (!string.IsNullOrEmpty(model.NewPassword))
                user.PasswordHash = HashPassword(model.NewPassword);

            await _context.SaveChangesAsync();
            return Ok(new { message = "User updated successfully" });
        }

        // 5. Xóa người dùng (chỉ Admin)
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return NotFound(new { message = "User not found" });

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return Ok(new { message = "User deleted successfully" });
        }

        // Mã hóa mật khẩu SHA-256
        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }

    }
}
