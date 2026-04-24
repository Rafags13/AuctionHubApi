using AuctionHub.Domain.Enums.User;
using System.Text.Json.Serialization;

namespace AuctionHub.Domain.DTOs.Authentication.Register.Request
{
    public record RequestRegisterUserDTO(string Name, string Email, string Password, [property: JsonIgnore] ERole Role);

    public record RequestCreateBidderDTO(
        string Name,
        string Email,
        string Password
    ) : RequestRegisterUserDTO(
        Name,
        Email,
        Password,
        ERole.BIDDER
    );

    public record RequestCreateSellerDTO(
        string Name,
        string Email,
        string Password
    ) : RequestRegisterUserDTO(
        Name,
        Email,
        Password,
        ERole.SELLER
    );
}
