using AuctionHub.Domain.Entities;
using AuctionHub.Domain.Interfaces.Repositories;
using AuctionHub.Infrastructure.Context;

namespace AuctionHub.Infrastructure.Repository
{
    internal sealed class BidRepository(AuctionHubContext context) : BaseRepository<Bid>(context), IBidRepository
    {
    }
}
