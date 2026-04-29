using AuctionHub.Domain.DTOs.Common;
using AuctionHub.Domain.Interfaces.Repositories;
using AuctionHub.Infrastructure.Observability;
using AuctionHub.Infrastructure.Services.Channel.Auction.Bid.Cancel.Producer;
using AuctionHub.Infrastructure.Services.Channel.Auction.Bid.Place.Consumer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading.Channels;

namespace AuctionHub.Infrastructure.Services.Channel.Auction.Bid.Cancel.Consumer
{
    internal sealed class CancelBidAuctionEventConsumer(
        ILogger<CancelBidAuctionEventConsumer> logger,
        ChannelReader<ChannelDTO<CancelBidAuctionEvent>> channel,
        IServiceScopeFactory serviceScopeFactory
    ) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                var scope = serviceScopeFactory.CreateScope();
                var bidRepository = scope.ServiceProvider.GetRequiredService<IBidRepository>();

                await foreach(var @event in channel.ReadAllAsync(stoppingToken))
                {
                    var message = @event.Message;

                    if (!await bidRepository.CancelAsync(message.Id, stoppingToken))
                    {
                        logger.LogWarning("Falha ao cancelar o lance com ID {BidId}.", message.Id);
                        continue;
                    }

                    logger.LogInformation("The bid {BidId} was cancelled at {Time}", message.Id, DateTime.UtcNow);

                    using var activity = Telemetry.ActivitySource.StartActivity(
                                "CancelBidAuction",
                                ActivityKind.Producer,
                                @event.ParentContext
                            );

                    activity?.SetTag("auction.id", message.Id);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Consumer {ConsumerName} encerrado devido a uma exceção.", nameof(BidAuctionEventConsumer));
            }
        }
    }
}
