using Microsoft.AspNetCore.Mvc;
using RuleEngine.Library;

namespace RuleEngineAPI
{
    [ApiController]
    [Route("api/sanctions")]
    public class SanctionController : ControllerBase
    {
        private readonly SftpHelper _sftpHelper;

        public SanctionController()
        {
            // Replace with actual SFTP credentials
            _sftpHelper = new SftpHelper("sftp.example.com", "username", "password");
        }

        [HttpPost("process-transactions")]
        public IActionResult ProcessTransactions()
        {
            try
            {
                var ruleFilePath = "rules.json"; // Local rules file path
                var transactionRemotePath = "/input/problem2/transactions.json";
                var blacklistRemotePath = "/input/problem2/blacklist.xml";
                var outputRemotePath = "/output/problem2/";

                var service = new SanctionProcessorService(_sftpHelper);
                service.Run(transactionRemotePath, blacklistRemotePath, ruleFilePath, outputRemotePath);

                return Ok(new { Message = "Processing completed. Results uploaded to SFTP." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred.", Details = ex.Message });
            }
        }
    }

}
