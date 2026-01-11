using Kiki.Courier.DAL.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kiki.Courier.DAL
{
    public class PackageDAL : IPackageDAL
    {
        public Task<int> GetRatePerKgAsync()
        {
            // For demonstration purposes, returning a fixed rate per kg.
            return Task.FromResult(10);
        }

        public Task<int> GetRatePerKmAsync()
        {
            // For demonstration purposes, returning a fixed rate per km.
            return Task.FromResult(5);
        }
    }
}
