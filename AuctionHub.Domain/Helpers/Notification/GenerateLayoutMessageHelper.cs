namespace AuctionHub.Domain.Helpers.Notification
{
    public static class GenerateLayoutMessageHelper
    {
        public static string GenerateOutbidMessage(string auctionTitle)
        {
            return $"You have been outbid in the auction '{auctionTitle}'. Place a higher bid to stay in the game!";
        }

        public static string GenerateAuctionWonMessage(string auctionTitle)
        {
            return $"Congratulations! You have won the auction '{auctionTitle}'. Follow the instructions below to get your prize!";
        }

        public static string GenerateAuctionStartedMessage(string auctionTitle)
        {
            return $"Your auction '{auctionTitle}' has started!";
        }
    }
}
