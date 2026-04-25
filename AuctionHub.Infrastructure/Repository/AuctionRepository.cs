using AuctionHub.Domain.Entities;
using AuctionHub.Domain.Interfaces.Repositories;
using AuctionHub.Infrastructure.Context;

namespace AuctionHub.Infrastructure.Repository
{
    internal sealed class AuctionRepository(AuctionHubContext context) : BaseRepository<Auction>(context), IAuctionRepository
    {
    }
}
