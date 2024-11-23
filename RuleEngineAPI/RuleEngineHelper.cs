using System;
using System.Collections.Generic;
using System.Linq;

namespace RuleEngineAPI
{
    public static class RuleHelper
    {
        // Extract country name from address
        public static string ExtractCountry(string address)
        {
            if (string.IsNullOrWhiteSpace(address))
                return null;

            // Split address by common delimiters and take the last part as the country
            var parts = address.Split(new[] { ',', '-', '/' }, StringSplitOptions.RemoveEmptyEntries)
                                .Select(p => p.Trim())
                                .ToArray();

            if (parts.Length == 0)
                return null;

            var lastPart = parts[^1];

            // Handle cases like "Saint Kitts and Nevis"
            if (lastPart.Contains("and") || lastPart.Contains(" "))
                return lastPart;

            return lastPart;
        }

    }
}
