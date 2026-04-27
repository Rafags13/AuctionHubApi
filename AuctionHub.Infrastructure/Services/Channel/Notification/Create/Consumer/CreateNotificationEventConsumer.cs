using AuctionHub.Domain.Interfaces.Repositories;
using AuctionHub.Infrastructure.Services.Channel.Notification.Create.Producer;
using AuctionHub.Infrastructure.Services.Channel.Payment.Process.Producer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading.Channels;

namespace AuctionHub.Infrastructure.Services.Channel.Notification.Create.Consumer
{
    internal sealed class CreateNotificationEventConsumer(
        ILogger<CreateNotificationEventConsumer> logger,
        ChannelReader<CreateNotificationEvent> channel,
        IServiceScopeFactory serviceScopeFactory
    ) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                var scope = serviceScopeFactory.CreateScope();
                var notificationRepository = scope.ServiceProvider.GetRequiredService<INotificationRepository>();

                await foreach (var @event in channel.ReadAllAsync(stoppingToken))
                {
                    if(!await notificationRepository.CreateAsync(@event, stoppingToken))
                    {
                        logger.LogError("Falha ao criar a notificação para o evento {Event}.", @event);
                    }
                }
            } catch(Exception ex)
            {
                logger.LogError(ex, "Consumer {ConsumerName} encerrado devido a uma exceção.", nameof(ProcessPaymentEvent));
            }
        }
    }
}
