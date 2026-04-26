using AuctionHub.Domain.DTOs.Auction.Ending.Response;

namespace AuctionHub.Infrastructure.Services.Channel.Auction.Ending.Producer
{
    public record EndAuctionEvent(long Id, long? LastBidderId) : EndingAuctionResponseDTO(Id, LastBidderId)
    {
        public EndAuctionEvent(EndingAuctionResponseDTO content) : this(content.Id, content.LastBidderId)
        {

        }
    }
}
