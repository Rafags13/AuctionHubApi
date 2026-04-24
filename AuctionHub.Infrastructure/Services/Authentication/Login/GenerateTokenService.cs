using AuctionHub.Domain.Constants.Authentication.Login;
using AuctionHub.Domain.DTOs.User.Common;
using AuctionHub.Domain.DTOs.User.Request.Login;
using AuctionHub.Domain.Interfaces.Services.Authentication.Login;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace AuctionHub.Infrastructure.Services.Authentication.Login
{
    internal sealed class GenerateTokenService : IGenerateTokenService
    {
        public RefreshTokenDTO GenerateRefreshToken()
        {
            var bytes = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(bytes);

            var refreshToken = Convert.ToBase64String(bytes);
            var expiratesAt = DateTime.UtcNow.AddDays(7);

            return new RefreshTokenDTO(refreshToken, expiratesAt);
        }

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
                expires: DateTime.UtcNow.AddMinutes(15),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
