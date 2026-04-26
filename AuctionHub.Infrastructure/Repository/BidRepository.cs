using AuctionHub.Domain.DTOs.Auction.Bid.Request;
using AuctionHub.Domain.DTOs.Auction.Bid.Response;
using AuctionHub.Domain.Entities;
using AuctionHub.Domain.Interfaces.Repositories;
using AuctionHub.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace AuctionHub.Infrastructure.Repository
{
    internal sealed class BidRepository(AuctionHubContext context) : BaseRepository<Bid>(context), IBidRepository
    {
        public async Task<bool> CreateAndOutBidAsync(BidRequestDTO content, CancellationToken cancellationToken = default)
        {
            if (!await CreateAsync(content, cancellationToken)) return false;

            if (content.OutBidId.HasValue && !await OutbidAsync(content.OutBidId.Value, cancellationToken)) return false;

            return true;
        }

        public Task<BidInformationsResponseDTO?> GetBidToOutBidAsync(long auctionId, CancellationToken cancellationToken = default)
        {
            return context.Bids
                .Where(b => b.AuctionId == auctionId)
                .OrderByDescending(b => b.Id)
                .Select(b => new BidInformationsResponseDTO(b.Id, b.Amount))
                .FirstOrDefaultAsync(cancellationToken);
        }

        private async Task<bool> CreateAsync(BidRequestDTO content, CancellationToken cancellationToken = default)
        {
            var bid = Bid.Create(content);

            await context.Bids.AddAsync(bid, cancellationToken);

            return await context.SaveChangesAsync(cancellationToken) > 0;
        }

        private async Task<bool> OutbidAsync(long outBidId, CancellationToken cancellationToken = default)
        {
            var bidToOutBid = await context.Bids.FirstOrDefaultAsync(b => b.Id == outBidId, cancellationToken);

            if(bidToOutBid is null) return false;

            bidToOutBid.Outbid();

            return await context.SaveChangesAsync(cancellationToken) > 0;
        }
    }
}
