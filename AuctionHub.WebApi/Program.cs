using AuctionHub.Application.Extensions.UseCases;
using AuctionHub.Infrastructure.Extensions;
using AuctionHub.WebApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddMemoryCache()
    .AddEndpointsApiExplorer()
    .AddObservability(builder.Host)
    .AddSwaggerSecureDefinition()
    .AddHttpContextAccessor()
    .AddJsonStringEnumConverter()
    .AddServices()
    .AddUseCases()
    .ConfigureInfrastructure(builder.Configuration)
    .ConfigureAuthorization();

var app = builder.Build();

app.ConfigureSwagger();

app.Services.ConfigureMigrations();

app.AddPrometheus()
    .UseAuthentication()
    .UseAuthorization();

app.AddEndpoints();

app.Run();