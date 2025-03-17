namespace OnlineShopBE.DTOModes.UserModels
{
    public class UpdateUserModel
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string? Role { get; set; }
        public string? NewPassword { get; set; } 
    }
}
