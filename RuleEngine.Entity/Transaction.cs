using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuleEngine.Entity
{
    public class Transaction
    {
        public long transaction_id { get; set; }
        public string sender_account { get; set; } 
        public string sender_routing_number { get; set; } 

        public string sender_name { get; set; }
        public string sender_address { get; set; }
        public string receiver_account { get; set; }
        public string receiver_routing_number { get; set; }
        public string receiver_name { get; set; }
        public string receiver_address { get; set; }
        public decimal? transaction_amount { get; set; }


    }

}
