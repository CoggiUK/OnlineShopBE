using Microsoft.IdentityModel.Tokens;
using OnlineShopBE.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace OnlineShopBE.Services
{
    public class JwtService
    {
        private readonly IConfiguration _configuration;

        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateJwtToken(User user, IList<string> roles)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user), "User cannot be null");

            if (string.IsNullOrEmpty(user.Username))
                throw new ArgumentNullException(nameof(user.Username), "Username cannot be null or empty");

            if (roles == null || roles.Count == 0 || roles.Any(string.IsNullOrEmpty))
                throw new ArgumentNullException(nameof(roles), "Roles cannot be null or empty");

            var claims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.UniqueName, user.Username),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email ?? ""), 
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var jwtKey = _configuration["JwtSettings:SecretKey"];
            if (string.IsNullOrEmpty(jwtKey))
                throw new ArgumentNullException(nameof(jwtKey), "JWT Key cannot be null or empty. Check appsettings.json.");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(60),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public int GetCustomerIdFromToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadToken(token) as JwtSecurityToken;

            if (jwtToken == null)
                throw new SecurityTokenException("Invalid token");

            var customerIdClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier);

            if (customerIdClaim == null)
                throw new SecurityTokenException("CustomerId claim not found");

            return int.Parse(customerIdClaim.Value);
        }
    }
}
