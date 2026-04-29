using System.Diagnostics;

namespace AuctionHub.Domain.DTOs.Common
{
    public record ChannelDTO<TRequest>(TRequest Message, ActivityContext ParentContext);
}
