namespace OnlineShopBE.DTOModes.ProductModels
{
    public class CreateProductModel
    {
        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public decimal Price { get; set; }

        public int Stock { get; set; }

        public string Category { get; set; } = null!;

        public DateTime? CreatedAt { get; set; }

        public int StoreId { get; set; }

    }
}
