using AuctionHub.Domain.Constants.Authentication.Login;
using AuctionHub.Domain.Constants.Authentication.Password;
using AuctionHub.Domain.Interfaces.Repositories;
using AuctionHub.Domain.Interfaces.Services.Channel;
using AuctionHub.Domain.Interfaces.UoW;
using AuctionHub.Infrastructure.Context;
using AuctionHub.Infrastructure.Repository;
using AuctionHub.Infrastructure.Services.BackgroundServices.Auction.Ending;
using AuctionHub.Infrastructure.Services.BackgroundServices.Auction.Open;
using AuctionHub.Infrastructure.Services.Channel.Auction.Bid.Award.Consumer;
using AuctionHub.Infrastructure.Services.Channel.Auction.Bid.Award.Producer;
using AuctionHub.Infrastructure.Services.Channel.Auction.Bid.Cancel.Consumer;
using AuctionHub.Infrastructure.Services.Channel.Auction.Bid.Cancel.Producer;
using AuctionHub.Infrastructure.Services.Channel.Auction.Bid.Place.Consumer;
using AuctionHub.Infrastructure.Services.Channel.Auction.Bid.Place.Producer;
using AuctionHub.Infrastructure.Services.Channel.Auction.Create.Consumer;
using AuctionHub.Infrastructure.Services.Channel.Auction.Create.Producer;
using AuctionHub.Infrastructure.Services.Channel.Auction.Ending.Consumer;
using AuctionHub.Infrastructure.Services.Channel.Auction.Ending.Producer;
using AuctionHub.Infrastructure.Services.Channel.Auction.Open.Consumer;
using AuctionHub.Infrastructure.Services.Channel.Auction.Open.Producer;
using AuctionHub.Infrastructure.Services.Channel.Notification.Create.Consumer;
using AuctionHub.Infrastructure.Services.Channel.Notification.Create.Producer;
using AuctionHub.Infrastructure.Services.Channel.Payment.Process.Consumer;
using AuctionHub.Infrastructure.Services.Channel.Payment.Process.Producer;
using AuctionHub.Infrastructure.Services.Channel.Producer;
using AuctionHub.Infrastructure.UoW;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading.Channels;

namespace AuctionHub.Infrastructure.Extensions
{
    public static class ConfigureInfrastructureExtensions
    {
        public static IServiceCollection ConfigureInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            return services
                .ConfigureContextDatabase(configuration)
                .ConfigureRepository()
                .ConfigureBackgrounServices()
                .ConfigureConstants(configuration);
        }

        private static IServiceCollection ConfigureRepository(this IServiceCollection services)
        {
            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IAuctionRepository, AuctionRepository>();
            services.AddScoped<IBidRepository, BidRepository>();
            services.AddScoped<IPaymentRepository, PaymentRepository>();
            services.AddScoped<INotificationRepository, NotificationRepository>();

            services.AddTransient(typeof(IBaseEventProducer<>), typeof(BaseEventProducer<>));

            return services;
        }

        private static IServiceCollection ConfigureContextDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            string connectionString = Environment.GetEnvironmentVariable("CONTEXT_DATA_SOURCE")
                                       ?? configuration.GetConnectionString("CONTEXT_DATA_SOURCE")
                                       ?? throw new ArgumentNullException("Não foi possível encontrar a string de conexão para CONTEXT_DATA_SOURCE");

            services.AddDbContext<AuctionHubContext>(options =>
            {
                options.UseNpgsql(connectionString);
                options.LogTo(Console.WriteLine, LogLevel.Information);
                options.EnableSensitiveDataLogging();
                options.EnableDetailedErrors();
            });

            return services;
        }

        private static IServiceCollection ConfigureBackgrounServices(this IServiceCollection services)
        {
            services.AddChannelService<CreateAuctionEvent, CreateAuctionEventConsumer>();
            services.AddChannelService<EndAuctionEvent, EndingAuctionEventConsumer>();
            services.AddChannelService<OpenAuctionEvent, OpenAuctionEventConsumer>();
            services.AddChannelService<BidAuctionEvent, BidAuctionEventConsumer>();
            services.AddChannelService<ProcessPaymentEvent, ProcessPaymentEventConsumer>();
            services.AddChannelService<AwardBidAuctionEvent, AwardBidAuctionEventConsumer>();
            services.AddChannelService<CancelBidAuctionEvent, CancelBidAuctionEventConsumer>();
            services.AddChannelService<CreateNotificationEvent, CreateNotificationEventConsumer>();

            services.AddHostedService<EndAuctionBackgroundService>();
            services.AddHostedService<OpenAuctionBackgroundService>();

            return services;
        }

        public static IServiceCollection AddChannelService<TChannel, TConsumer>(this IServiceCollection services) 
            where TChannel : class
            where TConsumer : BackgroundService
        {
            services.AddChannel<TChannel>();
            services.AddHostedService<TConsumer>();

            return services;
        }

        private static IServiceCollection AddChannel<T>(this IServiceCollection services)
            where T : class
        {
            var channel = Channel.CreateUnbounded<T>(new UnboundedChannelOptions
            {
                SingleReader = true,
                SingleWriter = false
            });

            services.AddSingleton(channel);
            services.AddSingleton(channel.Writer);
            services.AddSingleton(channel.Reader);

            return services;
        }

        private static IServiceCollection ConfigureConstants(this IServiceCollection services, IConfiguration configuration)
        {
            PasswordConstants.HASH = Environment.GetEnvironmentVariable("PASSWORD_HASH") ??
                configuration["PASSWORD_HASH"] ??
                throw new ArgumentNullException("Não foi possível encontrar a variável de ambiente PASSWORD_HASH");

            AuthenticationJwtConstants.SECRET_KEY = Environment.GetEnvironmentVariable("JWT_SECRET_KEY") ??
                configuration["JWT_SECRET_KEY"] ??
                throw new ArgumentNullException("Não foi possível encontrar a variável de ambiente JWT_SECRET_KEY");

            return services;
        }
    }
}
