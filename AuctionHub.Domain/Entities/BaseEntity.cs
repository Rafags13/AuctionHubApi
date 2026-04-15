using System.ComponentModel.DataAnnotations;

namespace AuctionHub.Domain.Entities
{
    public class BaseEntity
    {
        [Key]
        public long Id { get; init; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
