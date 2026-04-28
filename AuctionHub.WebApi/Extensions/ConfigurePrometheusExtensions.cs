using Prometheus;

namespace AuctionHub.WebApi.Extensions
{
    internal static class ConfigurePrometheusExtensions
    {
        internal static IApplicationBuilder AddPrometheus(this IApplicationBuilder app)
        {
            app.UseHttpMetrics();
            app.UseMetricServer();

            return app;
        }
    }
}
