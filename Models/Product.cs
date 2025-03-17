using System;
using System.Collections.Generic;

namespace OnlineShopBE.Models;

public partial class Product
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public decimal Price { get; set; }

    public int Stock { get; set; }

    public string Category { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public int StoreId { get; set; }

    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual Store Store { get; set; } = null!;
}
