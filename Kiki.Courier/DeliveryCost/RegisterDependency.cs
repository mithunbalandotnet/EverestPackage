using Kiki.Courier.DAL;
using Kiki.Courier.DAL.Contract;
using Kiki.Courier.ServiceLayer;
using Kiki.Courier.ServiceLayer.IContract;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryCost
{
    internal class RegisterDependency
    {
        public static void Register(IServiceCollection services)
        {
            // Register your dependencies here
            services.AddTransient<IPackageService, PackageService>();
            services.AddTransient<ICouponDAL, CouponCodeDAL>();
            services.AddTransient<IPackageDAL, PackageDAL>();
        }
    }
}
