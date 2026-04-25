using AuctionHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuctionHub.Infrastructure.Context.Configurations
{
    internal sealed class AuctionConfigurations : IEntityTypeConfiguration<Auction>
    {
        public void Configure(EntityTypeBuilder<Auction> builder)
        {
            builder.Property(x => x.Title).HasMaxLength(200);

            builder.Property(x => x.Description).HasMaxLength(1000);
        }
    }
}
