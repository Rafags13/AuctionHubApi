using AuctionHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AuctionHub.Infrastructure.Context
{
    internal sealed class AuctionHubContext(DbContextOptions<AuctionHubContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Auction> Auctions { get; set; }
        public DbSet<Bid> Bids { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AuctionHubContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries<BaseEntity>()
                .Where(q => q.State is EntityState.Added or EntityState.Modified))
            {
                if (entry.State == EntityState.Modified)
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                else if (entry.State == EntityState.Added)
                    entry.Entity.CreatedAt ??= DateTime.UtcNow;
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
