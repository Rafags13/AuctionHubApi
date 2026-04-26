using AuctionHub.Domain.DTOs.Auction.Open.Response;

namespace AuctionHub.Infrastructure.Services.Channel.Auction.Open.Producer
{
    public record OpenAuctionEvent(long Id) : OpenAuctionResponseDTO(Id)
    {
        public OpenAuctionEvent(OpenAuctionResponseDTO content) : this(content.Id) {

        } 
    }
}
