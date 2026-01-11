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
        public async Task CalculatePackageDeliveryCostAsync(int basicCost, IEnumerable<Package> packages)
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

        public async Task CalculatePackageDeliveryTimeAsync(IEnumerable<Package> packages, int vehicleCount, 
            int maxSpeed, int maxWeight)
        {
            
        }
    }
}
