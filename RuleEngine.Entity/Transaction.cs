using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuleEngine.Entity
{
    public class Transaction
    {
        public int TransactionId { get; set; }
        public decimal Amount { get; set; }
        public int MerchantId { get; set; }
        public int CardHolderId { get; set; }
        public DateTime Timestamp { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }  // Pending, InProcess, Completed, Failed
    }

}
