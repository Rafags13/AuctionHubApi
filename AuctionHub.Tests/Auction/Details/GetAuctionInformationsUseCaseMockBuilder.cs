using AuctionHub.Domain.DTOs.Auction.Details.Response;
using AuctionHub.Domain.Enums.Auction;
using AuctionHub.Domain.Interfaces.UoW;
using Moq;

namespace AuctionHub.Tests.Auction.Details
{
    public class GetAuctionInformationsUseCaseMockBuilder
    {
        private readonly Mock<IUnitOfWork> _unitOfWork = new();

        private AuctionDetailsResponseDTO? _auction;

        public GetAuctionInformationsUseCaseMockBuilder WithAuction()
        {
            _auction = new AuctionDetailsResponseDTO(
                Id: 1,
                Title: "Test Auction",
                Description: "Test Description",
                StartingPrice: 100,
                CurrentPrice: 150,
                StartTime: DateTime.UtcNow.AddHours(-1),
                EndTime: DateTime.UtcNow.AddHours(1),
                Status: EAuctionStatus.OPEN,
                SellerName: "Seller",
                WinnerName: null
            );

            return this;
        }

        public GetAuctionInformationsUseCaseMockBuilder WithoutAuction()
        {
            _auction = null;
            return this;
        }

        public GetAuctionInformationsUseCaseMocks Build()
        {
            _unitOfWork
                .Setup(x => x.AuctionRepository.GetAsync(
                    It.IsAny<long>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(_auction);

            return new GetAuctionInformationsUseCaseMocks(_unitOfWork);
        }
    }
}
