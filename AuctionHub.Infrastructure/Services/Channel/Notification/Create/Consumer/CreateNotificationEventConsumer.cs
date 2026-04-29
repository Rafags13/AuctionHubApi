using AuctionHub.Domain.DTOs.Common;
using AuctionHub.Domain.Interfaces.Repositories;
using AuctionHub.Infrastructure.Observability;
using AuctionHub.Infrastructure.Services.Channel.Notification.Create.Producer;
using AuctionHub.Infrastructure.Services.Channel.Payment.Process.Producer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading.Channels;

namespace AuctionHub.Infrastructure.Services.Channel.Notification.Create.Consumer
{
    internal sealed class CreateNotificationEventConsumer(
        ILogger<CreateNotificationEventConsumer> logger,
        ChannelReader<ChannelDTO<CreateNotificationEvent>> channel,
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
                    var message = @event.Message;

                    if (!await notificationRepository.CreateAsync(message, stoppingToken))
                    {
                        logger.LogError("Falha ao criar a notificação para o evento {Event}.", message);
                    } else
                    {
                        logger.LogInformation("A new notification of Type {Type} was created to {UserId} at {Time}", message.Type, message.UserId, DateTime.UtcNow);
                        using var activity = Telemetry.ActivitySource.StartActivity(
                                "CreateNotification",
                                ActivityKind.Producer,
                                @event.ParentContext
                            );

                        activity?.SetTag("notification.type", message.Type);
                        activity?.SetTag("notification.userId", message.UserId);
                    }
                }
            } catch(Exception ex)
            {
                logger.LogError(ex, "Consumer {ConsumerName} encerrado devido a uma exceção.", nameof(ProcessPaymentEvent));
            }
        }
    }
}
