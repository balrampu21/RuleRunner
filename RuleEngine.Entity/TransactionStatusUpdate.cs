using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuleEngine.Entity
{
    public class TransactionStatusUpdate
    {
        public int TransactionId { get; set; }
        public string Status { get; set; }  // Status can be "Pending", "InProcess", "Completed", "Failed"
    }
}
