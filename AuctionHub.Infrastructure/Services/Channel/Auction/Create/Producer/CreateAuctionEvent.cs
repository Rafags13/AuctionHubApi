using AuctionHub.Domain.DTOs.Auction.Create.Request;

namespace AuctionHub.Infrastructure.Services.Channel.Auction.Create.Producer
{
    public record CreateAuctionEvent(RequestCreateAuctionDTO @Event) :
        RequestCreateAuctionDTO(@Event.Title, @Event.Description, @Event.StartingPrice, @Event.StartTime, @Event.SellerId);
}
