using AuctionHub.Domain.DTOs.Common;
using AuctionHub.Domain.DTOs.Payment.Pay.Request;
using AuctionHub.Domain.Interfaces.Repositories;
using AuctionHub.Domain.Interfaces.Services.Channel;
using AuctionHub.Domain.Interfaces.Services.External.Payment;
using AuctionHub.Infrastructure.Observability;
using AuctionHub.Infrastructure.Services.Channel.Auction.Bid.Award.Producer;
using AuctionHub.Infrastructure.Services.Channel.Auction.Bid.Cancel.Producer;
using AuctionHub.Infrastructure.Services.Channel.Payment.Process.Producer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading.Channels;

namespace AuctionHub.Infrastructure.Services.Channel.Payment.Process.Consumer
{
    internal sealed class ProcessPaymentEventConsumer(
        ILogger<ProcessPaymentEventConsumer> logger,
        ChannelReader<ChannelDTO<ProcessPaymentEvent>> channel,
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
                    var message = @event.Message;

                    try
                    {
                        var successfulPayment = await externalPaymentService.ProcessAsync(message.Id, stoppingToken);

                        var paymentDate = DateTime.UtcNow;
                        if (successfulPayment)
                        {
                            await paymentRepository.PayAsync(new PayPaymentRequestDTO(message.Id, paymentDate), stoppingToken);
                            await awardBidChannel.DispatchAsync(new AwardBidAuctionEvent(message.AuctionId, message.Amount), stoppingToken);
                            logger.LogInformation("The payment {PaymentId} of Bid {BidId} got success at {Time}", message.Id, message.BidId, DateTime.UtcNow);
                        }
                        else
                        {
                            await paymentRepository.FailAsync(message.Id, stoppingToken);
                            await cancelBidChannel.DispatchAsync(new CancelBidAuctionEvent(message.BidId), stoppingToken);
                            logger.LogInformation("The payment {PaymentId} of Bid {BidId} got failed at {Time}", message.Id, message.BidId, DateTime.UtcNow);
                        }

                        using var activity = Telemetry.ActivitySource.StartActivity(
                                "ProcessPayment",
                                ActivityKind.Producer,
                                @event.ParentContext
                            );

                        activity?.SetTag("payment.id", message.Id);
                        activity?.SetTag("payment.amount", message.Amount);
                        activity?.SetTag("payment.successful", successfulPayment);
                        activity?.SetTag("auction.id", message.AuctionId);
                        activity?.SetTag("bid.id", message.BidId);
                    }
                    catch(Exception ex)
                    {
                        logger.LogError(ex, "Erro ao processar o evento de pagamento com ID {PaymentId}.", message.Id);
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
