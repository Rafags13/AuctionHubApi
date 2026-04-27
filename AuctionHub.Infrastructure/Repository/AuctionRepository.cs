using AuctionHub.Domain.DTOs.Auction.Bid.Response;
using AuctionHub.Domain.DTOs.Auction.Create.Request;
using AuctionHub.Domain.DTOs.Auction.Details.Response;
using AuctionHub.Domain.DTOs.Auction.Ending.Response;
using AuctionHub.Domain.DTOs.Auction.Open.Response;
using AuctionHub.Domain.DTOs.Auction.UpdatePrice;
using AuctionHub.Domain.Entities;
using AuctionHub.Domain.Enums.Auction;
using AuctionHub.Domain.Interfaces.Repositories;
using AuctionHub.Domain.Interfaces.Services.Caching;
using AuctionHub.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace AuctionHub.Infrastructure.Repository
{
    internal sealed class AuctionRepository(AuctionHubContext context, ICachingService cachingService) : BaseRepository<Auction>(context), IAuctionRepository
    {
        public async Task<bool> CreateAsync(RequestCreateAuctionDTO content, CancellationToken cancellationToken = default)
        {
            var auction = Auction.Create(content);

            await context.Auctions.AddAsync(auction, cancellationToken);

            return await context.SaveChangesAsync(cancellationToken) > 0;
        }

        public async Task<bool> EndAsync(EndingAuctionResponseDTO content, CancellationToken cancellationToken = default)
        {
            var auction = await context.Auctions.FirstOrDefaultAsync(a => a.Id == content.Id, cancellationToken);

            if (auction is null) return false;

            auction.End(content);

            return await context.SaveChangesAsync(cancellationToken) > 0;
        }

        public Task<EndingAuctionResponseDTO[]> GetExpiredAuctionsAsync(DateTime endedAt, CancellationToken cancellationToken = default)
        {
            return GetAll(a => a.EndTime <= endedAt && a.Status == EAuctionStatus.OPEN)
                .Select(a => new EndingAuctionResponseDTO(a.Id, a.Bids.Max(b => (long?)b.BidderId)))
                .ToArrayAsync(cancellationToken);
        }

        public Task<OpenAuctionResponseDTO[]> GetScheduledAuctionsToStartAsync(DateTime currentDateTime, CancellationToken cancellationToken = default)
        {
            return GetAll(a => a.StartTime <= currentDateTime && a.Status == EAuctionStatus.SCHEDULED)
                .Select(a => new OpenAuctionResponseDTO(a.Id))
                .ToArrayAsync(cancellationToken);
        }

        public Task<AuctionBidInformationsDTO?> GetAuctionBidInformationsAsync(long id, CancellationToken cancellationToken = default)
        {
            return GetAll(a => a.Id == id )
                .Select(a => new AuctionBidInformationsDTO(a.Status, a.StartingPrice))
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<bool> OpenAsync(OpenAuctionResponseDTO content, CancellationToken cancellationToken = default)
        {
            var auction = await GetAll(a => a.Id == content.Id && a.Status == EAuctionStatus.SCHEDULED)
                .FirstOrDefaultAsync(cancellationToken);

            if (auction is null)
                return false;

            auction.Open();

            return context.SaveChangesAsync(cancellationToken).Result > 0;
        }

        public async Task<bool> UpdateCurrentPriceAsync(RequestUpdateAuctionCurrentPriceDTO content, CancellationToken cancellationToken = default)
        {
            var auction = await context.Auctions.FirstOrDefaultAsync(a => a.Id == content.AuctionId, cancellationToken);

            if (auction is null) return false;

            auction.UpdateCurrentPrice(content.NewPrice);

            return await context.SaveChangesAsync(cancellationToken) > 0;
        }

        public async Task<AuctionDetailsResponseDTO?> GetAsync(long id, CancellationToken cancellationToken = default)
        {
            var cacheKey = $"Auction_{id}";
            var cachedAuction = cachingService.Get<AuctionDetailsResponseDTO>(cacheKey);
            if (cachedAuction != null)
                return cachedAuction;

            var auction = await GetAll(a => a.Id == id)
                .Select(a => new AuctionDetailsResponseDTO(
                    a.Id,
                    a.Title,
                    a.Description,
                    a.StartingPrice,
                    a.CurrentPrice,
                    a.StartTime,
                    a.EndTime,
                    a.Status,
                    a.Seller.Name,
                    a.Winner != null ? a.Winner.Name : null
                ))
                .FirstOrDefaultAsync(cancellationToken);

            if (auction is null) return auction;

            cachingService.Set(cacheKey, auction, TimeSpan.FromMinutes(5));

            return auction;
        }
    }
}
