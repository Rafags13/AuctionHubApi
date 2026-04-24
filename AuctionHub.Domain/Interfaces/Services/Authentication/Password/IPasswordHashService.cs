namespace AuctionHub.Domain.Interfaces.Services.Authentication.Password
{
    public interface IPasswordHashService
    {
        string GenerateHash(string password);
        bool VerifyHash(string password, string hash);
    }
}
