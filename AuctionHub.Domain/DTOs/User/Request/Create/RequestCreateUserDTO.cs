using AuctionHub.Domain.Enums.User;
using System.Text.Json.Serialization;

namespace AuctionHub.Domain.DTOs.User.Request.Create
{
    public record RequestCreateUserDTO(string Name, string Email, string Password, [property: JsonIgnore] ERole Role);

    public record RequestCreateBidderDTO(
        string Name,
        string Email,
        string Password
    ) : RequestCreateUserDTO(
        Name,
        Email,
        Password,
        ERole.BIDDER
    );

    public record RequestCreateSellerDTO(
        string Name,
        string Email,
        string Password
    ) : RequestCreateUserDTO(
        Name,
        Email,
        Password,
        ERole.SELLER
    );
}
