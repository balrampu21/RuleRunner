using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuleEngine.Entity
{
    public class TransactionQueue
    {
        public int QueueId { get; set; }
        public int TransactionId { get; set; }
        public string Status { get; set; } // Pending, InProcess, Completed, Failed
    }

}
