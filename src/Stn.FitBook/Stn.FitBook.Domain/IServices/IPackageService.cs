using Stn.FitBook.Domain.Models.Dtos.Request;
using Stn.FitBook.Domain.Models.Dtos.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stn.FitBook.Domain.IServices
{
    public interface IPackageService
    {
        public Task<PackageResponse> GetPackagesAsync();
        public Task<PackegeByUserIdResponse> GetPackagesByUserIdAsync(int userId);
        public Task<PurchasePackageResponse> PurchasePackageAsync(PurchasePackage purchasePackageInfo);

    }
}
