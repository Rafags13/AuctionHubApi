namespace AuctionHub.Domain.DTOs.Auction.UpdatePrice
{
    public record RequestUpdateAuctionCurrentPriceDTO(long AuctionId, decimal NewPrice);
}
