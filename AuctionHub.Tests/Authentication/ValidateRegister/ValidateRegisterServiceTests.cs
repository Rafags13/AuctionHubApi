using AuctionHub.Domain.Errors.Authentication.Register;
using FluentAssertions;

namespace AuctionHub.Tests.Authentication.ValidateRegister
{
    public class ValidateRegisterServiceTests
    {
        private readonly ValidateRegisterFixture _fixture = new();

        [Fact]
        public async Task Should_Return_Error_When_Name_Is_Empty()
        {
            var service = _fixture.Create();
            var request = _fixture.CreateValidRequest() with { Name = "" };

            var result = await service.ValidateAsync(request, default);

            result.Should().BeOfType<NameIsRequiredError>();
        }

        [Fact]
        public async Task Should_Return_Error_When_Email_Is_Empty()
        {
            var service = _fixture.Create();
            var request = _fixture.CreateValidRequest() with { Email = "" };

            var result = await service.ValidateAsync(request, default);

            result.Should().BeOfType<EmailIsRequiredError>();
        }

        [Fact]
        public async Task Should_Return_Error_When_Password_Is_Weak()
        {
            var service = _fixture.Create();
            var request = _fixture.CreateValidRequest() with { Password = "123" };

            var result = await service.ValidateAsync(request, default);

            result.Should().BeOfType<WeakPasswordError>();
        }

        [Fact]
        public async Task Should_Return_Error_When_Email_Is_Invalid()
        {
            var service = _fixture.Create();
            var request = _fixture.CreateValidRequest() with { Email = "invalid-email" };

            var result = await service.ValidateAsync(request, default);

            result.Should().BeOfType<InvalidEmailFormatError>();
        }

        [Fact]
        public async Task Should_Return_Error_When_User_Already_Exists()
        {
            var mocks = new ValidateRegisterMockBuilder()
                .WithExistingUser()
                .Build();

            var service = _fixture.Create(mocks);
            var request = _fixture.CreateValidRequest();

            var result = await service.ValidateAsync(request, default);

            result.Should().BeOfType<UserAlreadyExistsError>();
        }

        [Fact]
        public async Task Should_Return_Null_When_All_Is_Valid()
        {
            var mocks = new ValidateRegisterMockBuilder()
                .WithNoExistingUser()
                .Build();

            var service = _fixture.Create(mocks);
            var request = _fixture.CreateValidRequest();

            var result = await service.ValidateAsync(request, default);

            result.Should().BeNull();
        }
    }
}
