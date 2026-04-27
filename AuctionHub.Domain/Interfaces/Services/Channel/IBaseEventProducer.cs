namespace AuctionHub.Domain.Interfaces.Services.Channel
{
    public interface IBaseEventProducer<TRequest> where TRequest : class
    {
        Task DispatchAsync(TRequest @event, CancellationToken ct);
    }
}
