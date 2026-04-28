using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Sinks.Grafana.Loki;
using OpenTelemetry.Trace;
using Microsoft.Extensions.Hosting;

namespace AuctionHub.Infrastructure.Extensions
{
    public static class ConfigureObservabilityExtensions
    {
        public static IServiceCollection AddObservability(this IServiceCollection services, IHostBuilder host)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.GrafanaLoki("http://localhost:3100")
                .Enrich.FromLogContext()
                .CreateLogger();

            host.UseSerilog();

            services.AddOpenTelemetry()
                .WithTracing(tracing =>
                {
                    tracing
                        .AddAspNetCoreInstrumentation()
                        .AddHttpClientInstrumentation()
                        .AddOtlpExporter(o =>
                        {
                            o.Endpoint = new Uri("http://localhost:4317");
                        });
                });
            return services;
        }
    }
}
