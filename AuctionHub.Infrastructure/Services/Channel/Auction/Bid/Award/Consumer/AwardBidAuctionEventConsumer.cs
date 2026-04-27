using AuctionHub.Domain.DTOs.Auction.UpdatePrice;
using AuctionHub.Domain.DTOs.Notification.Create;
using AuctionHub.Domain.Interfaces.Repositories;
using AuctionHub.Domain.Interfaces.Services.Channel;
using AuctionHub.Infrastructure.Context;
using AuctionHub.Infrastructure.Services.Channel.Auction.Bid.Award.Producer;
using AuctionHub.Infrastructure.Services.Channel.Auction.Bid.Place.Consumer;
using AuctionHub.Infrastructure.Services.Channel.Notification.Create.Producer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading.Channels;

namespace AuctionHub.Infrastructure.Services.Channel.Auction.Bid.Award.Consumer
{
    internal sealed class AwardBidAuctionEventConsumer(
        ILogger<AwardBidAuctionEventConsumer> logger,
        ChannelReader<AwardBidAuctionEvent> channel,
        IServiceScopeFactory serviceScopeFactory
    ) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                var scope = serviceScopeFactory.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<AuctionHubContext>();
                var auctionRepository = scope.ServiceProvider.GetRequiredService<IAuctionRepository>();
                var bidRepository = scope.ServiceProvider.GetRequiredService<IBidRepository>();
                var notificationChannel = scope.ServiceProvider.GetRequiredService<IBaseEventProducer<CreateNotificationEvent>>();

                await foreach (var @event in channel.ReadAllAsync(stoppingToken))
                {
                    try
                    {
                        using var transaction = await context.Database.BeginTransactionAsync(stoppingToken);

                        var outBidId = await bidRepository.GetOutBidIdAsync(@event.AuctionId, stoppingToken);

                        if ((outBidId.HasValue &&
                             !await bidRepository.OutbidAsync(outBidId.Value, stoppingToken)) ||
                            !await auctionRepository.UpdateCurrentPriceAsync(
                               new RequestUpdateAuctionCurrentPriceDTO(@event.AuctionId, @event.Amount), stoppingToken
                        ))
                        {
                            logger.LogError("Ocorreu um erro ao processar o lance no leilão {AuctionId}.", @event.AuctionId);
                        }

                        await transaction.CommitAsync(stoppingToken);

                        if (outBidId.HasValue)
                        {
                            var auctionOutBidInformations = await auctionRepository.GetOutBidAsync(@event.AuctionId, stoppingToken);
                            if (auctionOutBidInformations != null)
                            {
                                var outbidNotification = new CreateOutBidNotificationRequestDTO(auctionOutBidInformations.Title, auctionOutBidInformations.UserId);
                                await notificationChannel.DispatchAsync(new CreateNotificationEvent(outbidNotification), stoppingToken);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "Erro ao processar evento no consumidor {ConsumerName}.", nameof(BidAuctionEventConsumer));
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Consumer {ConsumerName} encerrado devido a uma exceção.", nameof(BidAuctionEventConsumer));
            }
        }
    }
}
