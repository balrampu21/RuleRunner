using System;
using System.Data.SqlClient;
using Dapper;
using RuleEngine.Entity;


namespace RuleEngine
{
    public class QueueProcessor
    {
        private readonly string _connectionString;

        public QueueProcessor(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void ProcessQueue()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                // Get the next transaction in the queue that is not being processed
                var query = "SELECT TOP 1 * FROM TransactionQueue WHERE Status = 'Pending'";
                var transactionQueue = connection.QuerySingleOrDefault<TransactionQueue>(query);

                if (transactionQueue != null)
                {
                    // Set the queue status to 'InProcess'
                    connection.Execute("UPDATE TransactionQueue SET Status = 'InProcess' WHERE QueueId = @QueueId", new { QueueId = transactionQueue.QueueId });

                    // Process the transaction
                    ProcessTransaction(transactionQueue.TransactionId);

                    // Mark the transaction as 'Completed'
                    connection.Execute("UPDATE TransactionQueue SET Status = 'Completed' WHERE QueueId = @QueueId", new { QueueId = transactionQueue.QueueId });
                }
            }
        }

        private void ProcessTransaction(int transactionId)
        {
            // Retrieve transaction from the database
            var transaction = GetTransaction(transactionId);

            // Apply rules and update status (same as TransactionProcessor)
            var processor = new TransactionProcessor(_connectionString);
            processor.ProcessTransactions(new List<Transaction> { transaction });
        }

        private Transaction GetTransaction(int transactionId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var query = "SELECT * FROM Transactions WHERE TransactionId = @TransactionId";
                return connection.QuerySingleOrDefault<Transaction>(query, new { TransactionId = transactionId });
            }
        }
    }


}
