using AuctionHub.Domain.Interfaces.Services.Channel;
using Microsoft.Extensions.Logging;
using System.Threading.Channels;

namespace AuctionHub.Infrastructure.Services.Channel.Producer
{
    internal sealed class BaseEventProducer<TRequest>(
        ILogger<BaseEventProducer<TRequest>> logger,
        ChannelWriter<TRequest> channel) : IBaseEventProducer<TRequest> where TRequest : class
    {
        public async Task DispatchAsync(TRequest @event, CancellationToken ct)
        {
            logger.LogInformation(
                "Starting BaseEventProducer dispatch. MessageType={MessageType}",
                nameof(TRequest));

            try
            {
                await channel.WriteAsync(@event, ct);

                logger.LogInformation(
                    "BaseEventProducer dispatched successfully. MessageType={MessageType}",
                    nameof(TRequest));
            }
            catch (OperationCanceledException ex)
            {
                logger.LogWarning(
                    ex,
                    "Dispatch of BaseEventProducer was cancelled. MessageType={MessageType}",
                    nameof(TRequest));
            }
            catch (Exception ex)
            {
                logger.LogCritical(
                    ex,
                    "Unexpected error while dispatching BaseEventProducer. MessageType={MessageType}",
                    nameof(TRequest));
            }
        }
    }
}
