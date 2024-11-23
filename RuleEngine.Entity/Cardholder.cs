using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuleEngine.Entity
{
    public class Cardholder
    {
        public string CardHash { get; set; }
        public string CountryOfIssue { get; set; }
        public int VelocityLast24h { get; set; }
        public int VelocityLast1h { get; set; }
    }
}
