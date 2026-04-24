using AuctionHub.Domain.DTOs.Authentication.Register.Request;
using AuctionHub.Domain.DTOs.User.Common;
using AuctionHub.Domain.Enums.User;

namespace AuctionHub.Domain.Entities
{
    public class User : BaseEntity
    {
        public string Name { get; init; } = string.Empty;
        public string Email { get; init; } = string.Empty;
        public string PasswordHash { get; private set; } = string.Empty;
        public ERole Role { get; init; }
        public EUserStatus Status { get; init; }
        public string? RefreshToken { get; private set; }
        public DateTime? ExpirationRefreshToken { get; private set; }

        protected User() { }

        public User(long id, string name, string email, string passwordHash, ERole role, EUserStatus status, DateTime createdAt)
        {
            Id = id;
            Name = name;
            Email = email;
            PasswordHash = passwordHash;
            Role = role;
            Status = status;
            CreatedAt = createdAt;
        }

        public User(RequestRegisterUserDTO content)
        {
            Name = content.Name;
            Email = content.Email;
            PasswordHash = content.Password;
            Role = content.Role;
            Status = EUserStatus.ACTIVE;
        }

        #region [Factory]
        public static User Create(RequestRegisterUserDTO content, string hashedPassword)
        {
            var user = new User(content);
            user.SetPassword(hashedPassword);

            return user;
        }

        public void SetPassword(string password)
        {
            PasswordHash = password;
        }

        public void Refresh(RefreshTokenDTO content)
        {
            RefreshToken = content.RefreshToken;
            ExpirationRefreshToken = content.ExpiresAt;
        }
        #endregion
    }
}
