namespace AuctionHub.WebApi.Extensions
{
    internal static class ConfigurePrometheusExtensions
    {
        internal static IApplicationBuilder AddPrometheus(this WebApplication app)
        {
            app.MapPrometheusScrapingEndpoint();

            return app;
        }
    }
}
