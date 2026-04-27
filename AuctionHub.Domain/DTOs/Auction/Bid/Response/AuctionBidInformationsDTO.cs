using AuctionHub.Domain.Enums.Auction;

namespace AuctionHub.Domain.DTOs.Auction.Bid.Response
{
    public record AuctionBidInformationsDTO(EAuctionStatus Status, decimal StartingPrice);
}
