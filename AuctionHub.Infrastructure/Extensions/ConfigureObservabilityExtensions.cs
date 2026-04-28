using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Sinks.Grafana.Loki;
using OpenTelemetry.Trace;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;

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

            services.AddOpenTelemetry()
                .WithTracing(tracing =>
                {
                    tracing
                        .AddAspNetCoreInstrumentation()
                        .AddHttpClientInstrumentation()
                        .AddOtlpExporter(o =>
                        {
                            o.Endpoint = new Uri(openTelemetryUrl);
                        });
                });
            return services;
        }
    }
}
