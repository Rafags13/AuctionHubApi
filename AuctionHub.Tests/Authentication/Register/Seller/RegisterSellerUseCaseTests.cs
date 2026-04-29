using AuctionHub.Domain.Errors.Authentication.Register;
using AuctionHub.Domain.Errors.Common.Base;
using FluentAssertions;
using Moq;

namespace AuctionHub.Tests.Authentication.Register.Seller
{
    public class RegisterSellerUseCaseTests
    {
        private readonly RegisterSellerFixture _fixture = new();

        [Fact]
        public async Task Should_Register_Successfully_When_Data_Is_Valid()
        {
            var mocks = new RegisterSellerMockBuilder()
                .WithValidValidation()
                .WithSuccessfulCreation()
                .Build();

            var sut = _fixture.Create(mocks);
            var request = _fixture.CreateValidRequest();

            var result = await sut.RegisterAsync(request, default);

            result.IsT0.Should().BeTrue();
            result.AsT0.Should().BeTrue();
        }

        [Fact]
        public async Task Should_Return_Error_When_Validation_Fails()
        {
            var mocks = new RegisterSellerMockBuilder()
                .WithValidationError(new EmailIsRequiredError())
                .Build();

            var sut = _fixture.Create(mocks);
            var request = _fixture.CreateValidRequest();

            var result = await sut.RegisterAsync(request, default);

            result.IsT1.Should().BeTrue();
            result.AsT1.Should().BeOfType<EmailIsRequiredError>();
        }

        [Fact]
        public async Task Should_Return_DatabaseError_When_Create_Fails()
        {
            var mocks = new RegisterSellerMockBuilder()
                .WithValidValidation()
                .WithCreationFailure()
                .Build();

            var sut = _fixture.Create(mocks);
            var request = _fixture.CreateValidRequest();

            var result = await sut.RegisterAsync(request, default);

            result.IsT1.Should().BeTrue();
            result.AsT1.Should().BeOfType<DatabaseError>();
        }

        [Fact]
        public async Task Should_Generate_Hash_When_Valid()
        {
            var mocks = new RegisterSellerMockBuilder()
                .WithValidValidation()
                .WithSuccessfulCreation()
                .Build();

            var sut = _fixture.Create(mocks);
            var request = _fixture.CreateValidRequest();

            await sut.RegisterAsync(request, default);

            mocks.PasswordHashService.Verify(x => x.GenerateHash(request.Password), Times.Once);
        }
    }
}
