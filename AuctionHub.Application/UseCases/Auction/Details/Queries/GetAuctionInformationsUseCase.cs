using AuctionHub.Domain.DTOs.Auction.Details.Response;
using AuctionHub.Domain.Errors.Common.Auction;
using AuctionHub.Domain.Errors.Common.Base;
using AuctionHub.Domain.Interfaces.UoW;
using AuctionHub.Domain.Interfaces.UseCases.Auction.Details.Queries;
using OneOf;

namespace AuctionHub.Application.UseCases.Auction.Details.Queries
{
    internal sealed class GetAuctionInformationsUseCase(
        IUnitOfWork unitOfWork
    ) : IGetAuctionInformationsUseCase
    {
        public async Task<OneOf<AuctionDetailsResponseDTO, BaseError>> GetAsync(long auctionId, CancellationToken cancellationToken = default)
        {
            var auction = await unitOfWork.AuctionRepository.GetAsync(auctionId, cancellationToken);

            if (auction is null)
                return new AuctionNotFoundError();

            return auction;
        }
    }
}
