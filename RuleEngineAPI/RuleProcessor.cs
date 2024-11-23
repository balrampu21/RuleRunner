using RuleEngine.Entity;

namespace RuleEngineAPI
{
    public class RuleProcessor
    {
        private readonly List<string> _sanctionedCountries;
        private readonly List<string> _countryList;


        public RuleProcessor(List<string> sanctionedCountries , List<string> countryList)
        {
            _sanctionedCountries = sanctionedCountries;
            _countryList = countryList;

        }

        public (List<FlaggedTransactions>, List<ReviewTransactions>) ProcessTransactions(List<Transaction> transactions)
        {
            var flaggedTransactions = new List<FlaggedTransactions>();
            var reviewTransactions = new List<ReviewTransactions>();

            foreach (var transaction in transactions)
            {
                var senderCountry = RuleHelper.ExtractCountry(transaction.sender_address);
                var receiverCountry = RuleHelper.ExtractCountry(transaction.receiver_address);

                // Process sender's address
                ProcessCountry(senderCountry, transaction, flaggedTransactions, reviewTransactions);

                // Process receiver's address
                ProcessCountry(receiverCountry, transaction, flaggedTransactions, reviewTransactions);
            }

            return (flaggedTransactions, reviewTransactions);
        }
        private void ProcessCountry(
            string country,
            Transaction transaction,
            List<FlaggedTransactions> flaggedTransactions,
            List<ReviewTransactions> reviewTransactions)
        {
            if (string.IsNullOrEmpty(country))
            {
                // If country is not extractable, add to review
                reviewTransactions.Add(new ReviewTransactions { Transaction = transaction });
                return;
            }

            // Exact match with sanctioned countries
            if (_sanctionedCountries.Contains(country, StringComparer.OrdinalIgnoreCase))
            {
                flaggedTransactions.Add(new FlaggedTransactions { Transaction = transaction });
                return;
            }

            // Partial match with sanctioned countries
            if (_sanctionedCountries.Any(s => s.Contains(country, StringComparison.OrdinalIgnoreCase)))
            {
                reviewTransactions.Add(new ReviewTransactions { Transaction = transaction });
                return;
            }

            // Check in the country list
            if (_countryList.Contains(country, StringComparer.OrdinalIgnoreCase))
            {
                // Exact match in country list - No action
                return;
            }

            // Partial match with country list - No action
            if (_countryList.Any(c => c.Contains(country, StringComparison.OrdinalIgnoreCase)))
            {
                return;
            }

            // If country not found in the list, add to review
            reviewTransactions.Add(new ReviewTransactions { Transaction = transaction });
        }
    }
}

