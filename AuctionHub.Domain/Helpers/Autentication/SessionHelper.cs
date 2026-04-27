using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace AuctionHub.Domain.Helpers.Autentication
{
    public static class SessionHelper
    {
        public static long? GetUserId(HttpContext? httpContext)
        {
            if (httpContext?.User.Identity?.IsAuthenticated == false) return null;

            var userIdClaim = httpContext?.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            if (userIdClaim == null || !long.TryParse(userIdClaim.Value, out var userId)) return null;
            
            return userId;
        }
    }
}
