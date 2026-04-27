using AuctionHub.Domain.DTOs.Common;
using AuctionHub.Domain.DTOs.Notification.Create;
using AuctionHub.Domain.DTOs.Notification.Paginated.Request;
using AuctionHub.Domain.DTOs.Notification.Paginated.Response;
using AuctionHub.Domain.DTOs.Notification.Read.Request;
using AuctionHub.Domain.DTOs.Notification.Read.Response;
using AuctionHub.Domain.Entities;
using AuctionHub.Domain.Extensions;
using AuctionHub.Domain.Interfaces.Repositories;
using AuctionHub.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace AuctionHub.Infrastructure.Repository
{
    internal sealed class NotificationRepository(AuctionHubContext context) : BaseRepository<Notification>(context), INotificationRepository
    {
        public async Task<bool> CreateAsync(CreateNotificationRequestDTO content, CancellationToken cancellationToken = default)
        {
            var notification = Notification.Create(content);

            await context.AddAsync(notification, cancellationToken);

            return await context.SaveChangesAsync(cancellationToken) > 0;
        }

        public Task<ReadNotificationResponseDTO?> GetAsync(long id, CancellationToken cancellationToken = default)
        {
            return GetAll(n => n.Id == id)
                .Select(n => new ReadNotificationResponseDTO(n.Type, n.Message, n.ReadAt, n.CreatedAt))
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<PaginatedDTO<PaginatedNotificationResponseDTO>> GetPaginatedAsync(PaginatedNotificationRequestDTO content, CancellationToken cancellationToken = default)
        {
            var query = GetAll(n => n.UserId == content.UserId);

            var totalItems = await query.CountAsync(cancellationToken);

            var items = await query
                .Paginate(content.Page, content.PageSize)
                .OrderBy(n => n.Id)
                .Select(n => new PaginatedNotificationResponseDTO(n.Id, n.Type, n.ReadAt))
                .ToArrayAsync(cancellationToken);

            return new PaginatedDTO<PaginatedNotificationResponseDTO>(items, totalItems, content.Page, content.PageSize);
        }

        public Task<bool> IsAllowedToReadAsync(long id, long userId, CancellationToken cancellationToken = default)
        {
            return AnyAsync(n => n.Id == id && n.UserId == userId, cancellationToken);
        }

        public async Task<bool> ReadAsync(ReadNotificationRequestDTO content, CancellationToken cancellationToken = default)
        {
            var notification = await FirstOrDefaultAsync(n => n.Id == content.Id, cancellationToken);

            if (notification == null)
                return false;

            notification.Read(content.ReadAt);

            return await context.SaveChangesAsync(cancellationToken) > 0;
        }
    }
}
