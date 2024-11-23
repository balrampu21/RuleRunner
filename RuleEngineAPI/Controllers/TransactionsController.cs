using Microsoft.AspNetCore.Mvc;
using RuleEngine;
using RuleEngine.Entity;
using System.Data;
using System.Data.SqlClient;

namespace RuleEngineAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly TransactionQueueProcessor _queueProcessor;
        private readonly IDbConnection _dbConnection;

        public TransactionController(TransactionQueueProcessor queueProcessor, IDbConnection dbConnection)
        {
            _queueProcessor = queueProcessor;
            _dbConnection = dbConnection;
        }

        // API endpoint to trigger the queue processing
        [HttpPost("process-queue")]
        public IActionResult ProcessQueue()
        {
            _queueProcessor.ProcessQueue();
            return Ok(new { message = "Transaction queue processed successfully." });
        }

        // API endpoint to upload transactions from a file
        [HttpPost("upload-transactions")]
        public IActionResult UploadTransactions([FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            var transactions = ProcessFile(file); // Implement file parsing logic
            return Ok(new { message = "Transactions uploaded successfully.", transactions });
        }

        // Helper function to process the uploaded file (e.g., CSV, JSON)
        private List<Transaction> ProcessFile(IFormFile file)
        {
            var transactions = new List<Transaction>();

            // Example CSV parsing (for demonstration purposes, you would parse the file to get transactions)
            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');

                    var transaction = new Transaction
                    {
                        Amount = Convert.ToDecimal(values[0]),
                        MerchantId = Convert.ToInt32(values[1]),
                       // CardHolderId = Convert.ToInt32(values[2]),
                        Timestamp = DateTime.Parse(values[3]),
                        Type = values[4],
                        Status = "Pending"  // Default status to "Pending" for uploaded transactions
                    };

                    transactions.Add(transaction);
                }
            }

            // Insert the transactions into the database and queue
            foreach (var transaction in transactions)
            {
                _dbConnection.Execute("INSERT INTO Transactions (Amount, MerchantId, CardHolderId, Timestamp, Type, Status) VALUES (@Amount, @MerchantId, @CardHolderId, @Timestamp, @Type, @Status)", transaction);

                var queueItem = new TransactionQueue
                {
                    TransactionId = transaction.TransactionId,
                    Status = "Pending"
                };

                _dbConnection.Execute("INSERT INTO TransactionQueue (TransactionId, Status) VALUES (@TransactionId, @Status)", queueItem);
            }

            return transactions;
        }

        // API endpoint to directly test a transaction without uploading a file
        [HttpPost("test-transaction")]
        public IActionResult TestTransaction([FromBody] Transaction transaction)
        {
            if (transaction == null)
                return BadRequest("Transaction data is missing.");

            // Apply the rules and get violation message (if any)
            var ruleEngine = new RuleEngine();
            var violationMessage = ruleEngine.ApplyRules(transaction);

            if (violationMessage != null)
            {
                return BadRequest(new { message = violationMessage });
            }

            // Set status as "Pending" before processing the transaction
            transaction.Status = "Pending";

            // Insert the transaction into the database
            var insertTransactionQuery = "INSERT INTO Transactions (Amount, MerchantId, CardHolderId, Timestamp, Type, Status) VALUES (@Amount, @MerchantId, @CardHolderId, @Timestamp, @Type, @Status)";
            _dbConnection.Execute(insertTransactionQuery, transaction);

            // Insert the transaction into the queue for further processing
            var queueItem = new TransactionQueue
            {
                TransactionId = transaction.TransactionId,
                Status = "Pending"
            };

            var insertQueueQuery = "INSERT INTO TransactionQueue (TransactionId, Status) VALUES (@TransactionId, @Status)";
            _dbConnection.Execute(insertQueueQuery, queueItem);

            return Ok(new { message = "Transaction is valid and queued for processing.", transaction });
        }

        // API endpoint to update transaction status after processing
        [HttpPost("update-status")]
        public IActionResult UpdateTransactionStatus([FromBody] TransactionStatusUpdate statusUpdate)
        {
            if (statusUpdate == null)
                return BadRequest("Status update data is missing.");

            var updateQuery = "UPDATE Transactions SET Status = @Status WHERE TransactionId = @TransactionId";
            _dbConnection.Execute(updateQuery, statusUpdate);

            return Ok(new { message = "Transaction status updated successfully." });
        }
    }


}


