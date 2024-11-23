using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuleEngine.Entity
{
    public class Merchant
    {
        public string MerchantId { get; set; }
        public string MccCode { get; set; }
        public float RiskScore { get; set; }
        public string Country { get; set; }
        public decimal AverageTicketSize { get; set; }
    }
}
