namespace AuctionHub.Domain.DTOs.Common
{
    public record PaginatedDTO<T>(IEnumerable<T> Items, int TotalItems, int Page, int PageSize);
}
