using RuleEngine.Entity;

namespace RuleEngineAPI
{
    public class RuleProcessor
    {
        private readonly List<string> _sanctionedCountries;

        public RuleProcessor(List<string> sanctionedCountries)
        {
            _sanctionedCountries = sanctionedCountries;
        }

        public (List<FlaggedTransactions>, List<ReviewTransactions>) ProcessTransactions(List<Transaction> transactions)
        {
            var flaggedTransactions = new List<FlaggedTransactions>();
            var reviewTransactions = new List<ReviewTransactions>();

            foreach (var transaction in transactions)
            {
                var senderCountry = RuleHelper.ExtractCountry(transaction.sender_address);
                var receiverCountry = RuleHelper.ExtractCountry(transaction.receiver_address);

                // Add to flagged if any country matches sanctioned list
                if (RuleHelper.IsSanctionedCountry(senderCountry, _sanctionedCountries) ||
                    RuleHelper.IsSanctionedCountry(receiverCountry, _sanctionedCountries))
                {
                    flaggedTransactions.Add(new FlaggedTransactions { Transaction = transaction });
                }
                // Add to review if country extraction fails
                else if (string.IsNullOrEmpty(senderCountry) || string.IsNullOrEmpty(receiverCountry))
                {
                    reviewTransactions.Add(new ReviewTransactions { Transaction = transaction });
                }
            }

            return (flaggedTransactions, reviewTransactions);
        }
    }
}

