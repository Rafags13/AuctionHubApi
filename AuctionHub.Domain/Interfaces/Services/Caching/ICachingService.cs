namespace AuctionHub.Domain.Interfaces.Services.Caching
{
    public interface ICachingService
    {
        T? Get<T>(string key);
        void Set<T>(string key, T value, TimeSpan? expiration = null);
        void Remove(string key);
    }
}
