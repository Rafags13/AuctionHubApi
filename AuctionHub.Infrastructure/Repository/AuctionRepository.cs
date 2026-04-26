using AuctionHub.Domain.DTOs.Auction.Create.Request;
using AuctionHub.Domain.Entities;
using AuctionHub.Domain.Interfaces.Repositories;
using AuctionHub.Infrastructure.Context;

namespace AuctionHub.Infrastructure.Repository
{
    internal sealed class AuctionRepository(AuctionHubContext context) : BaseRepository<Auction>(context), IAuctionRepository
    {
        public async Task<bool> CreateAsync(RequestCreateAuctionDTO content, CancellationToken cancellationToken = default)
        {
            var auction = Auction.Create(content);

            await context.Auctions.AddAsync(auction, cancellationToken);

            return await context.SaveChangesAsync(cancellationToken) > 0;
        }
    }
}
