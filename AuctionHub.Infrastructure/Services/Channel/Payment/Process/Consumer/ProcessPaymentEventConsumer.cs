using AuctionHub.Domain.DTOs.Payment.Pay.Request;
using AuctionHub.Domain.Interfaces.Repositories;
using AuctionHub.Domain.Interfaces.Services.Channel;
using AuctionHub.Domain.Interfaces.Services.External.Payment;
using AuctionHub.Infrastructure.Services.Channel.Auction.Bid.Award.Producer;
using AuctionHub.Infrastructure.Services.Channel.Auction.Bid.Cancel.Producer;
using AuctionHub.Infrastructure.Services.Channel.Payment.Process.Producer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading.Channels;

namespace AuctionHub.Infrastructure.Services.Channel.Payment.Process.Consumer
{
    internal sealed class ProcessPaymentEventConsumer(
        ILogger<ProcessPaymentEventConsumer> logger,
        ChannelReader<ProcessPaymentEvent> channel,
        IServiceScopeFactory serviceScopeFactory
    ) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                var scope = serviceScopeFactory.CreateScope();
                var externalPaymentService = scope.ServiceProvider.GetRequiredService<IExternalIntegrationPaymentService>();
                var paymentRepository = scope.ServiceProvider.GetRequiredService<IPaymentRepository>();
                var awardBidChannel = scope.ServiceProvider.GetRequiredService<IBaseEventProducer<AwardBidAuctionEvent>>();
                var cancelBidChannel = scope.ServiceProvider.GetRequiredService<IBaseEventProducer<CancelBidAuctionEvent>>();
                await foreach (var @event in channel.ReadAllAsync(stoppingToken))
                {
                    try
                    {
                        var successfulPayment = await externalPaymentService.ProcessAsync(@event.Id, stoppingToken);

                        var paymentDate = DateTime.UtcNow;
                        if (successfulPayment)
                        {
                            await paymentRepository.PayAsync(new PayPaymentRequestDTO(@event.Id, paymentDate), stoppingToken);
                            await awardBidChannel.DispatchAsync(new AwardBidAuctionEvent(@event.AuctionId, @event.Amount), stoppingToken);
                        }
                        else
                        {
                            await paymentRepository.FailAsync(@event.Id, stoppingToken);
                            await cancelBidChannel.DispatchAsync(new CancelBidAuctionEvent(@event.BidId), stoppingToken);
                        }
                    }
                    catch(Exception ex)
                    {
                        logger.LogError(ex, "Erro ao processar o evento de pagamento com ID {PaymentId}.", @event.Id);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Consumer {ConsumerName} encerrado devido a uma exceção.", nameof(ProcessPaymentEvent));
            }
        }
    }
}
