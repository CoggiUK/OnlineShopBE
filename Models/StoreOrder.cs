using System;
using System.Collections.Generic;

namespace OnlineShopBE.Models;

public partial class StoreOrder
{
    public int Id { get; set; }

    public int StoreId { get; set; }

    public int OrderId { get; set; }

    public virtual Order Order { get; set; } = null!;

    public virtual Store Store { get; set; } = null!;
}
