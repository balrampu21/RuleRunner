
using RuleEngine.Entity;

namespace RuleEngine.Interfaces
{
    public interface IRule
    {
        bool Evaluate(Transaction transaction);
    }
}
