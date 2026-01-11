using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kiki.Courier.DAL.Contract
{
    public interface IPackageDAL
    {
        Task<int> GetRatePerKgAsync();
        Task<int> GetRatePerKmAsync();
    }
}
