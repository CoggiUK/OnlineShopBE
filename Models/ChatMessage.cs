using System;
using System.Collections.Generic;

namespace OnlineShopBE.Models;

public partial class ChatMessage
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string Message { get; set; } = null!;

    public string Response { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public virtual User User { get; set; } = null!;
}
