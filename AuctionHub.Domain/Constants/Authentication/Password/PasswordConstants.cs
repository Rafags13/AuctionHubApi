namespace AuctionHub.Domain.Constants.Authentication.Password
{
    public static class PasswordConstants
    {
        public static readonly int MAX_BYTES = 32;
        public static readonly int LANES_NUMBER = 8;
        public static readonly int MEMORY_SIZE = 128 * 1024;
        public static readonly int ITERATIONS = 4;
        public static string HASH { get; set; } = string.Empty;
    }
}
