using System;
using System.Collections.Generic;

namespace OnlineShopBE.Models;

public partial class Order
{
    public int Id { get; set; }

    public int CustomerId { get; set; }

    public DateTime? OrderDate { get; set; }

    public string Status { get; set; } = null!;

    public decimal TotalAmount { get; set; }

    public virtual User Customer { get; set; } = null!;

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual ICollection<StoreOrder> StoreOrders { get; set; } = new List<StoreOrder>();
}
