namespace AuctionHub.Domain.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> Paginate<T>(this IQueryable<T> source, int page = 0, int pageSize = 10)
        {
            return source.Skip(page * pageSize).Take(pageSize);
        }
    }
}
