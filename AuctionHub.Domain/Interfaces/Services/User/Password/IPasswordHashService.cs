namespace AuctionHub.Domain.Interfaces.Services.User.Password
{
    public interface IPasswordHashService
    {
        string GenerateHash(string password);
        bool VerifyHash(string password, string hash);
    }
}
