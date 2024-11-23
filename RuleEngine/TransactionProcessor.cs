using RuleEngine.Entity;
using System.Data.SqlClient;
using Dapper;
using System.Data;

namespace RuleEngine
{
    public class TransactionQueueProcessor
    {
        private readonly IDbConnection _dbConnection;

        public TransactionQueueProcessor(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public void ProcessQueue()
        {
            var pendingQueueItems = _dbConnection.Query<TransactionQueue>("SELECT * FROM TransactionQueue WHERE Status = 'Pending'").ToList();

            foreach (var queueItem in pendingQueueItems)
            {
                var transaction = _dbConnection.QuerySingleOrDefault<Transaction>("SELECT * FROM Transactions WHERE TransactionId = @TransactionId", new { queueItem.TransactionId });

                if (transaction == null) continue;

                // Apply rules
                var ruleEngine = new RuleEngine();
                var violationMessage = ruleEngine.ApplyRules(transaction);

                if (violationMessage != null)
                {
                    transaction.Status = "Failed";
                    _dbConnection.Execute("UPDATE Transactions SET Status = @Status WHERE TransactionId = @TransactionId", new { Status = "Failed", transaction.TransactionId });
                    _dbConnection.Execute("UPDATE TransactionQueue SET Status = @Status WHERE TransactionId = @TransactionId", new { Status = "Failed", transaction.TransactionId });
                }
                else
                {
                    // Process the transaction (e.g., mark it as completed)
                    transaction.Status = "Completed";
                    _dbConnection.Execute("UPDATE Transactions SET Status = @Status WHERE TransactionId = @TransactionId", new { Status = "Completed", transaction.TransactionId });
                    _dbConnection.Execute("UPDATE TransactionQueue SET Status = @Status WHERE TransactionId = @TransactionId", new { Status = "Completed", transaction.TransactionId });
                }
            }
        }
    }


}
