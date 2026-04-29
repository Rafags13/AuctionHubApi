using AuctionHub.Domain.DTOs.Common;
using AuctionHub.Domain.Interfaces.Repositories;
using AuctionHub.Infrastructure.Context;
using AuctionHub.Infrastructure.Observability;
using AuctionHub.Infrastructure.Repository;
using AuctionHub.Infrastructure.Services.Channel.Auction.Create.Producer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading.Channels;

namespace AuctionHub.Infrastructure.Services.Channel.Auction.Create.Consumer
{
    internal sealed class CreateAuctionEventConsumer(
        ILogger<CreateAuctionEventConsumer> logger,
        ChannelReader<ChannelDTO<CreateAuctionEvent>> channel,
        IServiceScopeFactory serviceScopeFactory
    ) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                var scope = serviceScopeFactory.CreateScope();
                var repository = scope.ServiceProvider.GetRequiredService<IAuctionRepository>();

                await foreach (var @event in channel.ReadAllAsync(stoppingToken))
                {
                    try
                    {
                        var message = @event.Message;
                        var auctionId = await repository.CreateAsync(message, stoppingToken);

                        if (!auctionId.HasValue)
                        {
                            logger.LogError("Ocorreu um erro ao tentar criar um novo leilão em {CreateAuctionEventConsumer}", nameof(CreateAuctionEventConsumer));
                            continue;
                        }

                        using var activity = Telemetry.ActivitySource.StartActivity(
                            "CreateAuction",
                            ActivityKind.Consumer,
                            @event.ParentContext
                        );

                        activity?.SetTag("auction.title", message.Title);
                        activity?.SetTag("auction.description", message.Description);
                        activity?.SetTag("auction.startTime", message.StartTime);
                        activity?.SetTag("auction.endTime", message.EndTime);
                        activity?.SetTag("auction.SellerId", message.SellerId);

                        logger.LogInformation("Um novo leilão {Title} foi criado pelo vendedor {SellerId} às {Time}", message.Title, message.SellerId, DateTime.UtcNow);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "Erro ao processar evento no consumidor {ConsumerName}.", nameof(CreateAuctionEventConsumer));
                    }
                }
            }
            catch(Exception e)
            {
                logger.LogError(e, "Consumer {ConsumerName} encerrado devido a uma exceção.", nameof(CreateAuctionEventConsumer));
            }
        }
    }
}
