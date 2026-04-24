using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace AuctionHub.Domain.Helpers
{
    public static partial class PasswordHelper
    {
        public static bool IsValid(string password)
        {
            if (string.IsNullOrEmpty(password))
                return false;

            var regex = PasswordRequirementsRegex();
            return regex.IsMatch(password);
        }

        [GeneratedRegex(@"^(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z0-9]).{8,}$")]
        private static partial Regex PasswordRequirementsRegex();
    }
}
