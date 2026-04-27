using AuctionHub.Domain.DTOs.Auction.UpdatePrice;
using AuctionHub.Domain.DTOs.Payment.Create.Request;
using AuctionHub.Domain.Interfaces.Repositories;
using AuctionHub.Domain.Interfaces.Services.Channel;
using AuctionHub.Infrastructure.Context;
using AuctionHub.Infrastructure.Services.Channel.Auction.Bid.Place.Producer;
using AuctionHub.Infrastructure.Services.Channel.Payment.Process.Producer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading.Channels;

namespace AuctionHub.Infrastructure.Services.Channel.Auction.Bid.Place.Consumer
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
                var paymentRepository = scope.ServiceProvider.GetRequiredService<IPaymentRepository>();
                var paymentChannel = scope.ServiceProvider.GetRequiredService<IBaseEventProducer<ProcessPaymentEvent>>();

                await foreach (var @event in channel.ReadAllAsync(stoppingToken))
                {
                    try
                    {
                        using var transaction = await context.Database.BeginTransactionAsync(stoppingToken);

                        var auctionNewPriceInfo = new RequestUpdateAuctionCurrentPriceDTO(@event.AuctionId, @event.Amount);
                        var paymentInfo = new CreatePaymentRequestDTO(@event.AuctionId, @event.BidderId, @event.Amount);

                        var bidId = await bidRepository.CreateAsync(@event, stoppingToken);
                        var paymentId = await paymentRepository.CreateAsync(paymentInfo, stoppingToken);

                        if (!bidId.HasValue || !paymentId.HasValue)
                            logger.LogError("Ocorreu um erro ao processar o lance no leilão {AuctionId}.", @event.AuctionId);
                        else
                        {
                            await transaction.CommitAsync(stoppingToken);
                            await paymentChannel.DispatchAsync(new ProcessPaymentEvent(paymentId.Value, @event.Amount, bidId.Value, @event.AuctionId), stoppingToken);
                        }
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
