using AuctionHub.Domain.DTOs.Auction.Bid.Request;
using AuctionHub.Domain.DTOs.Auction.Bid.Response;
using AuctionHub.Domain.Entities;
using AuctionHub.Domain.Enums.Auction;
using AuctionHub.Domain.Interfaces.Repositories;
using AuctionHub.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace AuctionHub.Infrastructure.Repository
{
    internal sealed class BidRepository(AuctionHubContext context) : BaseRepository<Bid>(context), IBidRepository
    {
        public async Task<bool> CancelAsync(long id, CancellationToken cancellationToken = default)
        {
            var bid = await FirstOrDefaultAsync(b => b.Id == id, cancellationToken);

            if (bid is null) return false;

            bid.Cancel();

            return await context.SaveChangesAsync(cancellationToken) > 0;
        }

        public async Task<long?> CreateAsync(BidRequestDTO content, CancellationToken cancellationToken = default)
        {
            var bid = Bid.Create(content);

            await context.Bids.AddAsync(bid, cancellationToken);

            await context.SaveChangesAsync(cancellationToken);

            return bid?.Id;
        }

        private IQueryable<Bid> GetLastBid(long auctionId)
        {
            return GetAll(b => b.AuctionId == auctionId && b.Status == EBidStatus.VALID)
                .OrderByDescending(b => b.Id);
        }

        public Task<decimal?> GetLastBidAmountAsync(long auctionId, CancellationToken cancellationToken = default)
        {
            return GetLastBid(auctionId)
                .Select(b => (decimal?)b.Amount)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public Task<long?> GetOutBidIdAsync(long auctionId, CancellationToken cancellationToken = default)
        {
            return GetLastBid(auctionId)
                .Select(b => (long?)b.Id)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<bool> OutbidAsync(long outBidId, CancellationToken cancellationToken = default)
        {
            var bidToOutBid = await context.Bids.FirstOrDefaultAsync(b => b.Id == outBidId, cancellationToken);

            if(bidToOutBid is null) return false;

            bidToOutBid.Outbid();

            return await context.SaveChangesAsync(cancellationToken) > 0;
        }
    }
}
