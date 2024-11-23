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

            var parts = address.Split(',').Select(p => p.Trim()).ToArray();
            return parts.Length > 0 ? parts[^1] : null; // Last part is assumed to be the country
        }

        // Check if a country is sanctioned
        public static bool IsSanctionedCountry(string country, List<string> sanctionedCountries)
        {
            return sanctionedCountries.Contains(country, StringComparer.OrdinalIgnoreCase);
        }
    }
}
