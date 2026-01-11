using Kiki.Courier.DAL.Contract;
using Kiki.Courier.Domain;

namespace Kiki.Courier.DAL
{
    public class CouponCodeDAL : ICouponDAL
    {
        private List<CouponCode> _couponCodes;
        public CouponCodeDAL()
        {
            _couponCodes = new List<CouponCode>
            {
                new CouponCode
                {
                    Code = "OFR001",
                    DiscountPercentage = 10,
                    MinimumWeight = 70,
                    MaximumWeight = 200,
                    MinimumDistance = 0,
                    MaximumDistance = 199
                },
                new CouponCode
                {
                    Code = "OFR002",
                    DiscountPercentage = 7,
                    MinimumWeight = 100,
                    MaximumWeight = 250,
                    MinimumDistance = 50,
                    MaximumDistance = 150
                },
                new CouponCode
                {
                    Code = "OFR003",
                    DiscountPercentage = 5,
                    MinimumWeight = 10,
                    MaximumWeight = 150,
                    MinimumDistance = 50,
                    MaximumDistance = 250
                }
            };
        }
        public async Task<IEnumerable<CouponCode>> GetAllCouponCodesAsync()
        {
            // Simulating async operation
            // In real scenario, this would be a database call
            return await Task.FromResult(_couponCodes);
        }

        public async Task<CouponCode> GetCouponCodeAsync(string code)
        {
            // Simulating async operation
            // In real scenario, this would be a database call
            var coupon = _couponCodes.FirstOrDefault(c => c.Code.Equals(code, StringComparison.OrdinalIgnoreCase));
            return await Task.FromResult(coupon);
        }
    }
}
