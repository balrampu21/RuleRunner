using RuleEngine.Entity;
using RuleEngine.Interfaces;

namespace RuleEngine
{


    public class VelocityRule : IRule
    {
        private readonly int _maxTransactions;
        private readonly int _timeWindowInSeconds;

        public VelocityRule(int maxTransactions, int timeWindowInSeconds)
        {
            _maxTransactions = maxTransactions;
            _timeWindowInSeconds = timeWindowInSeconds;
        }

        public bool Evaluate(Transaction transaction)
        {
            // Implement logic for velocity evaluation
            // Example: Check transactions count within the time window
            return true; // Replace with actual implementation
        }

        
    }

}
