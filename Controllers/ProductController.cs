using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineShopBE.DTOModes.ProductModels;
using OnlineShopBE.Models;
using OnlineShopBE.Services;

namespace OnlineShopBE.Controllers
{
    [Route("api/product")]
    [ApiController]

    public class ProductController : Controller
    {
        private readonly OnlineShopContext _context;
        private readonly JwtService _jwtService;

        public ProductController(OnlineShopContext context, JwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }
        // 1. Lấy danh sách sản phẩm
        [HttpGet()]
        public async Task<IActionResult> GetAllProducts()
        {
            var listProduct = _context.Products.Include(s => s.Store).Select( s => new ResponeProductModel{
                Name = s.Name,
                Description = s.Description,
                Price = s.Price,
                Stock = s.Stock,
                Category = s.Category,
                StoreName = s.Store.Name
            }).ToList();
            return Ok(listProduct);
        }

        // 2. Xem chi tiết sản phẩm	
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var ProductDetail = _context.Products.Include(s => s.Store).Where(s => s.Id == id).Select(s => new ResponeProductModel
            {
                Name = s.Name,
                Description = s.Description,
                Price = s.Price,
                Stock = s.Stock,
                Category = s.Category,
                StoreName = s.Store.Name
            }).ToList();
            return Ok(ProductDetail);
        }

        // 3. Lọc sản phẩm theo danh mục	
        [HttpGet("category/{categoryName}")]
        public async Task<IActionResult> GetByCategory(string categoryName)
        {
            var ProductDetail = _context.Products.Include(s => s.Store).Where(s => s.Category == categoryName).Select(s => new ResponeProductModel
            {
                Name = s.Name,
                Description = s.Description,
                Price = s.Price,
                Stock = s.Stock,
                Category = s.Category,
                StoreName = s.Store.Name
            }).ToList();
            return Ok(ProductDetail);
        }

        // 4. Thêm sản phẩm mới	
        [Authorize(Roles = "ShopOwner")]
        [HttpPost("create")]
        public async Task<IActionResult> AddProduct(CreateProductModel createProduct )
        {
            return Ok();
        }

        // 5. Cập nhật sản phẩm	
        [Authorize(Roles = "ShopOwner")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct()
        {
            return Ok();
        }

        // 6. Xóa sản phẩm	
        [Authorize(Roles = "ShopOwner")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct()
        {
            return Ok();
        }

    }
}
