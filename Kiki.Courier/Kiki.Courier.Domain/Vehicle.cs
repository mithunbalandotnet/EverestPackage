using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kiki.Courier.Domain
{
    public class Vehicle
    {
        public int Id { get; set; }
        public List<Trip> Trips { get; set; }
    }
}
