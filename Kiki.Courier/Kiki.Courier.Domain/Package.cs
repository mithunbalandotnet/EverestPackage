using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kiki.Courier.Domain
{
    public class Package
    {
        public string Id { get; set; }
        public int WeightInKg { get; set; }
        public int DistanceInKm { get; set; }
        public string CouponCode { get; set; }
        public int TotalCost { get; set; } = 0;
        public int Discount { get; set; } = 0;
        public decimal EstimatedDeliveryTime { get; set; } = 0;
    }
}
