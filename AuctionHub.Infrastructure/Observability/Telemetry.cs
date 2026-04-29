using System.Diagnostics;

namespace AuctionHub.Infrastructure.Observability
{
    public static class Telemetry
    {
        public static readonly ActivitySource ActivitySource =
            new("AuctionHub");
    }
}
