using AuctionHub.Domain.DTOs.Auction.UpdatePrice;
using AuctionHub.Domain.Interfaces.Repositories;
using AuctionHub.Infrastructure.Context;
using AuctionHub.Infrastructure.Repository;
using AuctionHub.Infrastructure.Services.Channel.Auction.Bid.Producer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading.Channels;

namespace AuctionHub.Infrastructure.Services.Channel.Auction.Bid.Consumer
{
    internal sealed class BidAuctionEventConsumer(
        ILogger<BidAuctionEventConsumer> logger,
        ChannelReader<BidAuctionEvent> channel,
        IServiceScopeFactory serviceScopeFactory
    ) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                var scope = serviceScopeFactory.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<AuctionHubContext>();
                var bidRepository = scope.ServiceProvider.GetRequiredService<IBidRepository>();
                var auctionRepository = scope.ServiceProvider.GetRequiredService<IAuctionRepository>();

                await foreach (var @event in channel.ReadAllAsync(stoppingToken))
                {
                    try
                    {
                        using var transaction = await context.Database.BeginTransactionAsync(stoppingToken);

                        var auctionNewPriceInfo = new RequestUpdateAuctionCurrentPriceDTO(@event.AuctionId, @event.Amount);

                        if (!await bidRepository.CreateAndOutBidAsync(@event, stoppingToken) ||
                            !await auctionRepository.UpdateCurrentPriceAsync(auctionNewPriceInfo, stoppingToken))
                            logger.LogError("Ocorreu um erro ao processar o lance no leilão {AuctionId}.", @event.AuctionId);

                        await transaction.CommitAsync(stoppingToken);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "Erro ao processar evento no consumidor {ConsumerName}.", nameof(BidAuctionEventConsumer));
                    }
                }
            }
            catch (Exception e)
            {
                logger.LogError(e, "Consumer {ConsumerName} encerrado devido a uma exceção.", nameof(BidAuctionEventConsumer));
            }
        }
    }
}
