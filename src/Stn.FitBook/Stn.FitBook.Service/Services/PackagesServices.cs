using Stn.FitBook.Domain.IServices;
using Stn.FitBook.Domain.IRepositories;
using Stn.FitBook.Domain.Models.Entities;
using Microsoft.Extensions.Logging;
using Stn.FitBook.Domain.Models.Dtos.Request;
using Stn.FitBook.Domain.Models.Dtos.Response;
using System.Text;

namespace Stn.FitBook.Service.Services
{
    public class PackagesServices : IPackageService
    {
        private readonly ILogger<PackagesServices> _logger;
        private readonly IQueryRepository<Package> _packageQueryRepo;
        private readonly IQueryRepository<UserPackage> _userPackageQueryRepo;
        private readonly ICommandRepository<UserPackage> _userPackageCommandRepo;
        private readonly ICustomRepository _customRepository;
        private readonly IUnitOfWork _uow;
        private readonly IValidator _validator;


        public PackagesServices(ILogger<PackagesServices> logger
            , IQueryRepository<Package> packageQueryRepo
             , IQueryRepository<UserPackage> userPackageQueryRepo
            , ICommandRepository<UserPackage> userpackageCommandRepo
            , ICustomRepository customRepository
            , IUnitOfWork uow
            , IValidator validator

        )
        {
            _logger = logger;
            _packageQueryRepo = packageQueryRepo;
            _userPackageQueryRepo = userPackageQueryRepo;
            _userPackageCommandRepo = userpackageCommandRepo;
            _customRepository = customRepository;
            _uow = uow;
            _validator = validator;
        }

        public async Task<PackageResponse> GetPackagesAsync()
        {
            var lstPackages = await _packageQueryRepo.GetAllAsync(x => x.ExpiryDate > DateTime.Now);
            var resp = new PackageResponse();
            resp.lstPackages = lstPackages;


            return resp;
        }

        public async Task<PackegeByUserIdResponse> GetPackagesByUserIdAsync(int userId)
        {

            var response = await _customRepository.GetPackagesByUserIdAsync(userId);
          
            return response;
        }
        public async Task<PurchasePackageResponse> PurchasePackageAsync(PurchasePackage reqModel)
        {
            var response = new PurchasePackageResponse();

            StringBuilder errorMessage = new StringBuilder();
            //valid purchasePackageInfo
            _validator.ValidPurchasePackagesRequest(reqModel);

            var packagesInfo =await _packageQueryRepo.GetByIdAsync(x => x.PackageId == reqModel.PackageId);

            if(packagesInfo == null)
            {
                errorMessage.AppendLine("PurchasePackage must not be empty.");
                throw new ArgumentNullException(errorMessage.ToString());
            }

            if(reqModel.CountryCode != packagesInfo.Country)
            {
                errorMessage.AppendLine("Can't purchase package from different country.");
                throw new ArgumentNullException(errorMessage.ToString());
            }

            //Call Payment Method
            if (!ExternalServices.PaymentCharge(packagesInfo, reqModel.UserId))
            {
                errorMessage.AppendLine("Paymant is not success .");
                throw new ArgumentNullException(errorMessage.ToString());
            }

            //insert or update userpackage 

            var existingUserPackage = await _userPackageQueryRepo.GetByIdAsync(x => x.UserId == reqModel.UserId && x.PackageId == reqModel.PackageId);

            if (existingUserPackage == null)
            {

                var userPackage = new UserPackage();
                userPackage.UserId = reqModel.UserId;
                userPackage.PackageId = reqModel.PackageId;
                userPackage.PurchaseDate = DateTime.UtcNow.Date;
                userPackage.AvailableCredits = packagesInfo.Credits;

                await _userPackageCommandRepo.CreateAsync(userPackage);
            }
            else
            {
                existingUserPackage.AvailableCredits += packagesInfo.Credits;
                await _userPackageCommandRepo.UpdateAsync(existingUserPackage);
            }
            await _uow.CommitAsync();

            response.PackageID = packagesInfo.PackageId;
            response.PackageName = packagesInfo.PackageName;

            return response;
        }




    }
}
