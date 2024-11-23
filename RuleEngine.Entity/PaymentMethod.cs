using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuleEngine.Entity
{
    public class PaymentMethod
    {
        public string CardType { get; set; }
        public string EntryMode { get; set; }
        public string AuthenticationMethod { get; set; }
    }
}
