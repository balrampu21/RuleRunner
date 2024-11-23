using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuleEngine.Entity
{
    public class Device
    {
        public string IpAddress { get; set; }
        public string DeviceId { get; set; }
        public GeoLocation Geolocation { get; set; }
    }
}
