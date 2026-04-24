using AuctionHub.Domain.Constants.Authentication.Password;
using AuctionHub.Domain.Interfaces.Services.Authentication.Password;
using Konscious.Security.Cryptography;
using System.Text;

namespace AuctionHub.Infrastructure.Services.Authentication.Password
{
    internal sealed class PasswordHashService : IPasswordHashService
    {
        public string GenerateHash(string password)
        {
            using var argon2 = GetConfiguredArgon(password);

            byte[] hashBytes = argon2.GetBytes(PasswordConstants.MAX_BYTES);

            return Convert.ToBase64String(hashBytes);
        }

        public bool VerifyHash(string password, string hash)
        {
            using var argon2 = GetConfiguredArgon(password);

            byte[] hashBytes = argon2.GetBytes(PasswordConstants.MAX_BYTES);
            var storedHashBytes = Convert.FromBase64String(hash);

            return hashBytes.SequenceEqual(storedHashBytes);
        }

        private static Argon2id GetConfiguredArgon(string password)
        {
            var passwordBytes = Encoding.UTF8.GetBytes(password);

            var salt = Encoding.UTF8.GetBytes(PasswordConstants.HASH);

            using var argon2 = new Argon2id(passwordBytes);

            argon2.Salt = salt;
            argon2.DegreeOfParallelism = PasswordConstants.LANES_NUMBER;
            argon2.MemorySize = PasswordConstants.MEMORY_SIZE;
            argon2.Iterations = PasswordConstants.ITERATIONS;

            return argon2;
        }
    }
}
