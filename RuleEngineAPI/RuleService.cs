using RuleEngine.Entity;

namespace RuleEngineAPI
{
    public class RuleService
    {
        public class RuleProcessor
        {
            private readonly List<string> _sanctionedCountries;

            public RuleProcessor(List<string> sanctionedCountries)
            {
                _sanctionedCountries = sanctionedCountries;
            }

            public List<FlaggedTransactions> ProcessTransactions(List<Transaction> transactions)
            {
                var flaggedTransactions = new List<FlaggedTransactions>();

                foreach (var transaction in transactions)
                {
                    var senderCountry = RuleHelper.ExtractCountry(transaction.sender_address);
                    var receiverCountry = RuleHelper.ExtractCountry(transaction.receiver_address);

                    if (RuleHelper.IsSanctionedCountry(senderCountry, _sanctionedCountries) ||
                        RuleHelper.IsSanctionedCountry(receiverCountry, _sanctionedCountries))
                    {
                        flaggedTransactions.Add(new FlaggedTransactions { Transaction = transaction });
                    }
                }

                return flaggedTransactions;
            }
        }
    }
}
