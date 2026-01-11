using Kiki.Courier.DAL.Contract;
using Kiki.Courier.Domain;
using Kiki.Courier.ServiceLayer;
using Kiki.Courier.ServiceLayer.IContract;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kiki.Courier.Test.Service
{
    [TestClass]
    public class PackageServiceTest
    {
        private IPackageService _packageService;
        private Mock<IPackageDAL> _packageDALMock;
        private Mock<ICouponDAL> _couponDALMock;
        private List<CouponCode> _couponCodes;
        private List<Package> _packages;

        [TestInitialize]
        public void Setup()
        {
            _packageDALMock = new Mock<IPackageDAL>();
            _couponDALMock = new Mock<ICouponDAL>();
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

            _packageDALMock.Setup(p => p.GetRatePerKgAsync()).ReturnsAsync(10);
            _packageDALMock.Setup(p => p.GetRatePerKmAsync()).ReturnsAsync(5);
            _couponDALMock.Setup(c => c.GetCouponCodeAsync(It.IsAny<string>()))
                .ReturnsAsync((string code) => _couponCodes.FirstOrDefault(c => c.Code == code));
            _couponDALMock.Setup(c => c.GetAllCouponCodesAsync())
                .ReturnsAsync(_couponCodes);
            _packages = new List<Package>
            {
                new Package
                {
                    Id = "PKG1",
                    WeightInKg = 50,
                    DistanceInKm = 30,
                    CouponCode = "OFR001"
                },
                new Package
                {
                    Id = "PKG2",
                    WeightInKg = 75,
                    DistanceInKm = 125,
                    CouponCode = "OFR008"
                },
                new Package
                {
                    Id = "PKG3",
                    WeightInKg = 175,
                    DistanceInKm = 100,
                    CouponCode = "OFR003"
                },
                new Package
                {
                    Id = "PKG4",
                    WeightInKg = 110,
                    DistanceInKm = 60,
                    CouponCode = "OFR002"
                },
                new Package
                {
                    Id = "PKG5",
                    WeightInKg = 155,
                    DistanceInKm = 95,
                    CouponCode = "NA"
                }
            };
            _packageService = new PackageService(_couponDALMock.Object, _packageDALMock.Object);
        }

        [TestMethod]
        public async Task CalculatePackageDeliveryCostAsyncTest()
        {
            await _packageService.CalculatePackageDeliveryCostAsync(100, _packages);
            Assert.AreEqual(750, _packages[0].TotalCost);
            Assert.AreEqual(1475, _packages[1].TotalCost);
            Assert.AreEqual(2350, _packages[2].TotalCost);
            Assert.AreEqual(1395, _packages[3].TotalCost);
            Assert.AreEqual(2125, _packages[4].TotalCost);

            Assert.AreEqual(0, _packages[0].Discount);
            Assert.AreEqual(0, _packages[1].Discount);
            Assert.AreEqual(0, _packages[2].Discount);
            Assert.AreEqual(105, _packages[3].Discount);
            Assert.AreEqual(0, _packages[4].Discount);
        }

        [TestMethod]
        public async Task CalculatePackageDeliveryTimeAsync()
        {
            await _packageService.CalculatePackageDeliveryTimeAsync(_packages, 2, 70, 200);
            Assert.AreEqual(1.79, _packages[1].EstimatedDeliveryTime);
            Assert.AreEqual(0.86, _packages[3].EstimatedDeliveryTime);
        }
    }
}
