using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenTelemetry.Exporter;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog;
using Serilog.Sinks.Grafana.Loki;

namespace AuctionHub.Infrastructure.Extensions
{
    public static class ConfigureObservabilityExtensions
    {
        public static IServiceCollection AddObservability(this IServiceCollection services, IConfiguration configuration, IHostBuilder host)
        {
            var lokiServiceUrl = Environment.GetEnvironmentVariable("LOKI_SERVICE_URL") ??
                configuration["LOKI_SERVICE_URL"] ??
                throw new ArgumentNullException("Não foi possível encontrar a variável de ambiente LOKI_SERVICE_URL");

            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.GrafanaLoki(lokiServiceUrl)
                .Enrich.FromLogContext()
                .CreateLogger();

            host.UseSerilog();

            var openTelemetryUrl = Environment.GetEnvironmentVariable("OPEN_TELEMETRY_URL") ??
                configuration["OPEN_TELEMETRY_URL"] ??
                throw new ArgumentNullException("Não foi possível encontrar a variável de ambiente OPEN_TELEMETRY_URL");

            services
                .AddOpenTelemetry()               
                .WithTracing(tracing =>
                {
                    tracing
                        .SetResourceBuilder(
                            ResourceBuilder.CreateDefault()
                                .AddService("AuctionHub.WebApi"))
                        .AddAspNetCoreInstrumentation()
                        .AddHttpClientInstrumentation()
                        .AddSource("AuctionHub")
                        .AddOtlpExporter(o =>
                        {
                            o.Endpoint = new Uri(openTelemetryUrl);
                            o.Protocol = OtlpExportProtocol.Grpc;
                        })
                        .AddConsoleExporter();
                });
            return services;
        }
    }
}
