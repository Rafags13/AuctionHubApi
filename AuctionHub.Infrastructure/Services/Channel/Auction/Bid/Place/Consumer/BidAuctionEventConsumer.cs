using AuctionHub.Domain.DTOs.Auction.UpdatePrice;
using AuctionHub.Domain.DTOs.Common;
using AuctionHub.Domain.DTOs.Payment.Create.Request;
using AuctionHub.Domain.Interfaces.Repositories;
using AuctionHub.Domain.Interfaces.Services.Channel;
using AuctionHub.Infrastructure.Context;
using AuctionHub.Infrastructure.Observability;
using AuctionHub.Infrastructure.Services.Channel.Auction.Bid.Place.Producer;
using AuctionHub.Infrastructure.Services.Channel.Payment.Process.Producer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading.Channels;

namespace AuctionHub.Infrastructure.Services.Channel.Auction.Bid.Place.Consumer
{
    internal sealed class BidAuctionEventConsumer(
        ILogger<BidAuctionEventConsumer> logger,
        ChannelReader<ChannelDTO<BidAuctionEvent>> channel,
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
                        var message = @event.Message;

                        using var transaction = await context.Database.BeginTransactionAsync(stoppingToken);

                        var auctionNewPriceInfo = new RequestUpdateAuctionCurrentPriceDTO(message.AuctionId, message.Amount);
                        var paymentInfo = new CreatePaymentRequestDTO(message.AuctionId, message.BidderId, message.Amount);

                        var bidId = await bidRepository.CreateAsync(message, stoppingToken);
                        var paymentId = await paymentRepository.CreateAsync(paymentInfo, stoppingToken);

                        if (!bidId.HasValue || !paymentId.HasValue)
                            logger.LogError("Ocorreu um erro ao processar o lance no leilão {AuctionId}.", message.AuctionId);
                        else
                        {
                            await transaction.CommitAsync(stoppingToken);
                            await paymentChannel.DispatchAsync(new ProcessPaymentEvent(paymentId.Value, message.Amount, bidId.Value, message.AuctionId), stoppingToken);
                            logger.LogInformation("A new bid at auction {AuctionId} was created by {BidderId} at {Time}", message.AuctionId, message.BidderId, DateTime.UtcNow);

                            using var activity = Telemetry.ActivitySource.StartActivity(
                                "BidAuction",
                                ActivityKind.Producer,
                                @event.ParentContext
                            );

                            activity?.SetTag("auction.id", message.AuctionId);
                            activity?.SetTag("auction.amount", message.Amount);
                            activity?.SetTag("bidder.id", message.Amount);
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
