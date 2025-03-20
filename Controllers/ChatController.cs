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

        // 1. Send a message to AI
        [HttpPost("send")]
        public async Task<IActionResult> SendMessage([FromBody] ChatMessage chatMessage)
        {
            if (chatMessage == null || string.IsNullOrEmpty(chatMessage.Message))
            {
                return BadRequest("Message cannot be empty.");
            }

            var customerId = _jwtService.GetCustomerIdFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
            chatMessage.UserId = customerId;

            // Process the message with AI (this is a placeholder, replace with actual AI service call)
            chatMessage.Response = "AI response to: " + chatMessage.Message;
            chatMessage.CreatedAt = DateTime.UtcNow;

            _context.ChatMessages.Add(chatMessage);
            await _context.SaveChangesAsync();

            return Ok(chatMessage);
        }

        // 2. View chat history
        [HttpGet("history")]
        public IActionResult GetChatHistory()
        {
            var customerId = _jwtService.GetCustomerIdFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
            var chatHistory = _context.ChatMessages
                .Where(cm => cm.UserId == customerId)
                .OrderBy(cm => cm.CreatedAt)
                .ToList();

            if (chatHistory == null || !chatHistory.Any())
            {
                return NotFound("No chat history found for the user.");
            }

            return Ok(chatHistory);
        }

        // 3. Suggest existing products
        [HttpGet("suggest-products")]
        public IActionResult SuggestProducts()
        {
            var products = _context.Products
                .OrderBy(p => p.Name)
                .ToList();

            if (products == null || !products.Any())
            {
                return NotFound("No products found.");
            }

            return Ok(products);
        }
    }
}
