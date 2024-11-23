using Microsoft.AspNetCore.Mvc;
using RuleEngineAPI;
using RuleEngineAPI;
using System.Data;
using RuleEngineAPI;
using System.Data.SqlClient;
using static RuleEngineAPI.RuleService;
using RuleEngine.Entity;

namespace RuleEngineAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    
    public class TransactionController : ControllerBase
    {
        private readonly RuleProcessor _ruleProcessor;

        public TransactionController()
        {
            // Load sanctioned countries (can be from a database or JSON file)
            var sanctionedCountries = new List<string>
            {
                "Switzerland",
                "North Korea",
                "Iran",
                "Russia"
            };
            // Load country list
            var countryList = new List<string>
            {
                "United States",
                "United Kingdom",
                "Switzerland",
                "India",
                "Germany",
                "France",
                "China"
            };

            _ruleProcessor = new RuleProcessor(sanctionedCountries, countryList);
        }

        [HttpPost("flag-and-review")]
        public IActionResult FlagAndReviewTransactions([FromBody] List<Transaction> transactions)
        {
            if (transactions == null || transactions.Count == 0)
                return BadRequest("No transactions provided.");

            var (flagged, review) = _ruleProcessor.ProcessTransactions(transactions);

            return Ok(new
            {
                FlaggedTransactions = flagged,
                ReviewTransactions = review
            });
        }
    }

}


