using AuctionHub.Domain.Interfaces.Repositories;
using AuctionHub.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AuctionHub.Infrastructure.Repository
{
    internal class BaseRepository<T>(AuctionHubContext context) : IBaseRepository<T> where T : class
    {
        public Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default)
        {
            return context.Set<T>().AnyAsync(predicate, ct);
        }

        public IQueryable<T> GetAll(Expression<Func<T, bool>> predicate)
        {
            return context.Set<T>().Where(predicate);
        }

        public IQueryable<T> GetAll()
        {
            return context.Set<T>().AsQueryable();
        }
    }
}
