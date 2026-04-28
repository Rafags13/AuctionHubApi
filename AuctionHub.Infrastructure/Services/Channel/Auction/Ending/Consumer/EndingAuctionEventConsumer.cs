using AuctionHub.Domain.DTOs.Notification.Create;
using AuctionHub.Domain.Interfaces.Repositories;
using AuctionHub.Domain.Interfaces.Services.Channel;
using AuctionHub.Infrastructure.Services.Channel.Auction.Ending.Producer;
using AuctionHub.Infrastructure.Services.Channel.Notification.Create.Producer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading.Channels;

namespace AuctionHub.Infrastructure.Services.Channel.Auction.Ending.Consumer
{
    internal sealed class EndingAuctionEventConsumer(
        ILogger<EndingAuctionEventConsumer> logger,
        ChannelReader<EndAuctionEvent> channel,
        IServiceScopeFactory serviceScopeFactory
    ) : BackgroundService
    {
        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
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
                        if(!await repository.EndAsync(@event, stoppingToken))
                            logger.LogError("Ocorreu um erro ao finalizar o leilão {AuctionId}.", @event.Id);

                        var winnerInformations = await repository.GetWinnerAsync(@event.Id, stoppingToken);

                        if(winnerInformations != null)
                        {
                            var wonNotification = new CreateWonAuctionNotificationRequestDTO(winnerInformations.Title, winnerInformations.UserId);
                            await notificationChannel.DispatchAsync(new CreateNotificationEvent(wonNotification), stoppingToken);
                        }

                        logger.LogInformation(
                            "The auction {AuctionId} was finished with Winner {UserId} at {Time}",
                            @event.Id,
                            winnerInformations != null ? winnerInformations.UserId : "No Winners",
                            DateTime.UtcNow);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "Erro ao processar evento no consumidor {ConsumerName}.", nameof(EndingAuctionEventConsumer));
                    }
                }
            }
            catch (Exception e)
            {
                logger.LogError(e, "Consumer {ConsumerName} encerrado devido a uma exceção.", nameof(EndingAuctionEventConsumer));
            }
        }
    }
}
