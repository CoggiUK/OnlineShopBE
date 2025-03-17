using System;
using System.Collections.Generic;

namespace OnlineShopBE.Models;

public partial class Store
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public int OwnerId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual User Owner { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();

    public virtual ICollection<StoreOrder> StoreOrders { get; set; } = new List<StoreOrder>();
}
