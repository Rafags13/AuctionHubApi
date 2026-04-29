using AuctionHub.Domain.DTOs.Common;
using AuctionHub.Domain.DTOs.Notification.Create;
using AuctionHub.Domain.Entities;
using AuctionHub.Domain.Interfaces.Repositories;
using AuctionHub.Domain.Interfaces.Services.Channel;
using AuctionHub.Infrastructure.Context;
using AuctionHub.Infrastructure.Observability;
using AuctionHub.Infrastructure.Repository;
using AuctionHub.Infrastructure.Services.Channel.Auction.Open.Producer;
using AuctionHub.Infrastructure.Services.Channel.Notification.Create.Producer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading.Channels;

namespace AuctionHub.Infrastructure.Services.Channel.Auction.Open.Consumer
{
    internal sealed class OpenAuctionEventConsumer(
        ILogger<OpenAuctionEventConsumer> logger,
        ChannelReader<ChannelDTO<OpenAuctionEvent>> channel,
        IServiceScopeFactory serviceScopeFactory
    ) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                var scope = serviceScopeFactory.CreateScope();
                var repository = scope.ServiceProvider.GetRequiredService<IAuctionRepository>();
                var notificationChannel = scope.ServiceProvider.GetRequiredService<IBaseEventProducer<CreateNotificationEvent>>();

                await foreach (var @event in channel.ReadAllAsync(stoppingToken))
                {
                    try
                    {
                        var message = @event.Message;
                        if (!await repository.OpenAsync(message, stoppingToken))
                            logger.LogError("Ocorreu um erro ao abrir o leilão {AuctionId}.", message.Id);

                        var openAuction = await repository.GetOpenAsync(message.Id, stoppingToken);

                        if(openAuction != null)
                        {
                            var createAuctionNotification = new CreateStartAuctionNotificationRequestDTO(openAuction.Title, openAuction.UserId);
                            await notificationChannel.DispatchAsync(new CreateNotificationEvent(createAuctionNotification), stoppingToken);
                        }

                        logger.LogInformation("Auction {AuctionId} started at {Time}", message.Id, DateTime.UtcNow);

                        using var activity = Telemetry.ActivitySource.StartActivity(
                                "OpenAuction",
                                ActivityKind.Producer,
                                @event.ParentContext
                            );

                        activity?.SetTag("auction.id", message.Id);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "Erro ao processar evento no consumidor {ConsumerName}.", nameof(OpenAuctionEventConsumer));
                    }
                }
            }
            catch (Exception e)
            {
                logger.LogError(e, "Consumer {ConsumerName} encerrado devido a uma exceção.", nameof(OpenAuctionEventConsumer));
            }
        }
    }
}
