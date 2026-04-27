using AuctionHub.Domain.Enums.User;

namespace AuctionHub.Domain.DTOs.User.Profile.Response
{
    public record UserProfileDTO(string Name, string Email, ERole Role, EUserStatus Status);
}
