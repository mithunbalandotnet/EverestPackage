using Kiki.Courier.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kiki.Courier.ServiceLayer.IContract
{
    public interface IPackageService
    {
        Task CalculatePackageDeliveryCostAsync(int basicCost, IEnumerable<Package> packages);

        Task CalculatePackageDeliveryTimeAsync(IEnumerable<Package> packages, int vehicleCount, int maxSpeed, int maxWeight);
    }
}
