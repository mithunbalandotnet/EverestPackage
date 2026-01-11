using Kiki.Courier.DAL;
using Kiki.Courier.DAL.Contract;
using Kiki.Courier.Domain;
using Kiki.Courier.ServiceLayer.IContract;

namespace Kiki.Courier.ServiceLayer
{
    public class PackageService : IPackageService
    {
        private readonly ICouponDAL _couponDAL;
        private readonly IPackageDAL _packageDAL;

        public PackageService(ICouponDAL couponDAL, IPackageDAL packageDAL)
        {
            _couponDAL = couponDAL;
            _packageDAL = packageDAL;
        }
        public async Task CalculatePackageDeliveryCostAsync(int basicCost, List<Package> packages)
        {
            int ratePerKg = await _packageDAL.GetRatePerKgAsync();
            int ratePerKm = await _packageDAL.GetRatePerKmAsync();

            foreach (var package in packages)
            {
                package.TotalCost = basicCost + (package.WeightInKg * ratePerKg) + (package.DistanceInKm * ratePerKm);
                // Placeholder for actual coupon code calculation logic
                // This is where you would implement the logic to calculate discounts
                // based on the coupon codes associated with each package.
                var coupon = await _couponDAL.GetCouponCodeAsync(package.CouponCode);
                if (coupon != null)
                {
                    // Apply discount logic here
                    if(package.WeightInKg >= coupon.MinimumWeight && 
                        package.DistanceInKm >= coupon.MinimumDistance &&
                        package.WeightInKg <= coupon.MaximumWeight &&
                        package.DistanceInKm <= coupon.MaximumDistance
                        )
                    {
                        package.Discount = (package.TotalCost * coupon.DiscountPercentage) / 100;
                        package.TotalCost -= package.Discount;
                    }
                }
            }
        }

        public async Task CalculatePackageDeliveryTimeAsync(List<Package> packages, int vehicleCount, 
            int maxSpeed, int maxWeight)
        {
            var packagesCopy = new List<Package>(packages); 
            List<Vehicle> vehicles = GetPackagesForVehicle(packagesCopy, vehicleCount, maxWeight, maxSpeed);
            packages.ForEach(p =>
            {
                var vehicle = vehicles.First(v => v.Trips.Any(t => t.Packages.Contains(p)));
                var trip = vehicle.Trips.First(t => t.Packages.Contains(p));
                p.EstimatedDeliveryTime = Math.Round(trip.StartTime + 1.0000 * p.DistanceInKm / (maxSpeed * 1.0000), 2);
            });
        }

        private List<Vehicle> GetPackagesForVehicle(List<Package> packages, int vehicleCount, int maxWeight, int maxSpeed)
        {
            int vehicleId = 0;
            List<Vehicle> vehicles = new List<Vehicle>();
            List<(List<Package> packages, int TotalWeight) > sumPackages = new List<(List<Package>, int)>();
            for (int i = 0; i < packages.Count; i++)
            {
                var shipment = (Packages : new List<Package>() { packages[i] }, TotalWeight : packages[i].WeightInKg);
                sumPackages.Add(shipment);
                shipment = (Packages: new List<Package>() { packages[i] }, TotalWeight: packages[i].WeightInKg);
                for (int j = i + 1; j < packages.Count; j++)
                {
                    if(i != j && (shipment.TotalWeight + packages[j].WeightInKg) <= maxWeight)
                    {
                        shipment.TotalWeight += packages[j].WeightInKg;
                        shipment.Packages.Add(packages[j]);
                        sumPackages.Add(shipment);
                        if(shipment.Packages.Count >= 3)
                        {
                            var newList = shipment.Packages.Skip(2).Append(packages[i]).ToList();
                            sumPackages.Add((Packages: newList, TotalWeight: newList.Select(p => p.WeightInKg).Sum()));
                        }
                    }
                }
            }
            // sumPackages contains all combinations of packages
            sumPackages.Sort((a, b) => b.TotalWeight.CompareTo(a.TotalWeight));

            do
            {
                var journey = sumPackages[0];
                if (vehicleId < vehicleCount)
                {
                    vehicleId++;
                    var vehicle = new Vehicle
                    {
                        Id = vehicleId,
                        Trips = new List<Trip>() { new Trip {
                        Packages = journey.packages,
                        Distance = journey.packages.Select(p => p.DistanceInKm).Max(),
                        StartTime = 0, EndTime = journey.packages.Select(p => p.DistanceInKm).Max() * 2.0000 / (maxSpeed * 1.0000)
                    } }
                    };
                    vehicles.Add(vehicle);
                }
                else
                {
                    var vehicle = vehicles.OrderBy(v => v.Trips.Last().EndTime).First();
                    var lastTripEndTime = vehicle.Trips.Last().EndTime;
                    vehicle.Trips.Add(new Trip
                    {
                        Packages = journey.packages,
                        Distance = journey.packages.Select(p => p.DistanceInKm).Max(),
                        StartTime = lastTripEndTime,
                        EndTime = lastTripEndTime + (journey.packages.Select(p => p.DistanceInKm).Max() * 2.0000 / (maxSpeed * 1.0000))
                    });
                    vehicles.Add(vehicle);
                }

                journey.packages.ForEach(p =>
                {
                    sumPackages.RemoveAll(sp => sp.packages.Contains(p));
                });
            } while (sumPackages.Count > 0);

            return vehicles;
        }
    }
}
