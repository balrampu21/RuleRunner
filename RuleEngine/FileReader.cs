using RuleEngine.Entity;

namespace RuleEngine
{
    public class FileReader
    {
        public List<Transaction> ReadTransactionsFromFile(string filePath)
        {
            var transactions = new List<Transaction>();

            if (File.Exists(filePath))
            {
                var lines = File.ReadAllLines(filePath);
                foreach (var line in lines.Skip(1)) // Skip header
                {
                    var columns = line.Split(',');

                    if (columns.Length == 4)
                    {
                        var transaction = new Transaction
                        {
                            Amount = decimal.Parse(columns[0]),
                            Type = columns[1],
                            TransactionDate = DateTime.Parse(columns[2]),
                            Status = "Pending"
                        };
                        transactions.Add(transaction);
                    }
                }
            }

            return transactions;
        }
    }
}

