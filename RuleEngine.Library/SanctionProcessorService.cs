namespace RuleEngine.Library
{
    public class SanctionProcessorService
    {
        private readonly SftpHelper _sftpHelper;

        public SanctionProcessorService(SftpHelper sftpHelper)
        {
            _sftpHelper = sftpHelper;
        }

        public void Run(string transactionRemotePath, string blacklistRemotePath, string ruleFilePath, string outputRemotePath)
        {
            string localTransactionFile = Path.Combine(Path.GetTempPath(), "transactions.json");
            string localBlacklistFile = Path.Combine(Path.GetTempPath(), "blacklist.xml");
            string localOutputFolder = Path.Combine(Path.GetTempPath(), "output");

            Directory.CreateDirectory(localOutputFolder);

            // Download files from SFTP
            _sftpHelper.DownloadFile(transactionRemotePath, localTransactionFile);
            _sftpHelper.DownloadFile(blacklistRemotePath, localBlacklistFile);

            // Process transactions
            var processor = new SanctionProcessor(localOutputFolder);
            processor.ProcessTransactions(localTransactionFile, localBlacklistFile, ruleFilePath);

            // Upload output files back to SFTP
            _sftpHelper.UploadFile(Path.Combine(localOutputFolder, "flagged_transactions.csv"),
                Path.Combine(outputRemotePath, "flagged_transactions.csv"));

            _sftpHelper.UploadFile(Path.Combine(localOutputFolder, "flagged_transaction_for_review.csv"),
                Path.Combine(outputRemotePath, "flagged_transaction_for_review.csv"));
        }
    }

}
