using AuctionHub.Domain.Constants.User.Login;
using AuctionHub.Domain.DTOs.User.Request.Login;
using AuctionHub.Domain.Interfaces.Services.User.Login;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuctionHub.Infrastructure.Services.User.Login
{
    internal sealed class GenerateTokenService : IGenerateTokenService
    {
        public string GenerateToken(RequestGenerateTokenDTO content)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AuthenticationJwtConstants.SECRET_KEY));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim("id", content.Id.ToString()),
                new Claim("name", content.Name),
                new Claim(ClaimTypes.Role, content.Role.ToString())
            };

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
