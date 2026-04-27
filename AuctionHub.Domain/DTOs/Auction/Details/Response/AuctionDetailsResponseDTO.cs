using AuctionHub.Domain.Enums.Auction;

namespace AuctionHub.Domain.DTOs.Auction.Details.Response
{
    public record AuctionDetailsResponseDTO(
        long Id,
        string Title,
        string Description,
        decimal StartingPrice,
        decimal? CurrentPrice,
        DateTime StartTime,
        DateTime EndTime,
        EAuctionStatus Status,
        string SellerName,
        string? WinnerName);
}
