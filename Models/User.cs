using System;
using System.Collections.Generic;

namespace OnlineShopBE.Models;

public partial class User
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string Role { get; set; } = null!;

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();

    public virtual ICollection<ChatMessage> ChatMessages { get; set; } = new List<ChatMessage>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<Store> Stores { get; set; } = new List<Store>();
}
