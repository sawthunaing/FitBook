using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;
using Stn.FitBook.Domain.IServices;
using Stn.FitBook.Domain.Models.Dtos.Request;
using Stn.FitBook.Domain.Models.Dtos.Response;

namespace Stn.FitBook.Api.Controllers
{
    [ApiController]
    [Route("[controller]/v1/")]
    public class PackagesController : ControllerBase
    {
        private readonly IPackageService _packageService;
        private readonly ILogger<PackagesController> _logger;
       
        public PackagesController(IPackageService packageService,  ILogger<PackagesController> logger)
        {
            _packageService = packageService;
            _logger = logger;
           
        }
               
        [HttpGet("GetPackagesAsync")]

        public async Task<PackageResponse> GetPackagesAsync()
        {
            _logger.LogInformation("{name} {message}", "GetUserByIdRequest", null);
            var response = await _packageService.GetPackagesAsync();
            _logger.LogInformation("{name} {message}", "GetUserByIdResponse", response.ToJson());
            return await Task.FromResult(response);
        }

      
        [HttpGet("GetPackagesByUserIdAsync")]
        public async Task<PackegeByUserIdResponse> GetPackagesByUserIdAsync(int userId)
        {
            _logger.LogInformation("{name} {message}", "GetUserByIdRequest", null);
            var response = await _packageService.GetPackagesByUserIdAsync(userId);
            _logger.LogInformation("{name} {message}", "GetUserByIdResponse", response.ToJson());
            return await Task.FromResult(response);
        }

        
        [HttpPost("PurchasePackageAsync")]
        public async Task<PurchasePackageResponse> PurchasePackageAsync(PurchasePackage reqModel)
        {
            _logger.LogInformation("{name} {message}", "PurchasePackageAsyncRequest", reqModel.ToJson());
            var response = await _packageService.PurchasePackageAsync(reqModel);
            _logger.LogInformation("{name} {message}", "PurchasePackageAsyncResponse", response.ToJson());
            return await Task.FromResult(response);
        }

    }

   
}
