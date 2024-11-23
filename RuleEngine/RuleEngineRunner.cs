using RuleEngine.Entity;
using RuleEngine.Interfaces;

namespace RuleRunner
{
    public class RuleEngine
    {
        public string ApplyRules(Transaction transaction)
        {
            if (transaction.Amount > 10000)
                return "Transaction exceeds allowed amount limit.";

            if (transaction.Type != "Debit" && transaction.Type != "Credit")
                return "Invalid transaction type.";

            // Additional rules can be added here.

            return null;  // No rule violation
        }
    }


}
