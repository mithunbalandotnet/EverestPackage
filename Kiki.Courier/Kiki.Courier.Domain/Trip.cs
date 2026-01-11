using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kiki.Courier.Domain
{
    public class Trip
    {
        public decimal StartTime { get; set; }
        public decimal EndTime { get; set; }
        public int Distance { get; set; }
        public List<Package> Packages { get; set; } = new List<Package>();
    }
}
