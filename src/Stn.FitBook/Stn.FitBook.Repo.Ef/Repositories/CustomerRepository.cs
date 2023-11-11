using Microsoft.EntityFrameworkCore;
using Stn.FitBook.Domain.IRepositories;
using Stn.FitBook.Domain.Models.Dtos.Response;
using Stn.FitBook.Domain.Models.Entities;

namespace Stn.FitBook.Repo.Ef.Repositories
{
    public class CustomerRepository : ICustomRepository
    {
        #region private
        private readonly DBContext _context;
        #endregion
        public CustomerRepository(DBContext context)
        {
            _context = context;
        }
              

        public async Task<PackegeByUserIdResponse> GetPackagesByUserIdAsync(int userId)
        {
            var response = new PackegeByUserIdResponse();
            response.lstPackages = await (from a in _context.UserPackages
                                         join c in _context.Packages on a.PackageId equals c.PackageId
                                         where a.UserId == userId
                                          select new PackageDto
                                          {
                                              PackageID = c.PackageId,
                                              PackageName = c.PackageName,
                                              Country = c.Country,
                                              AvailableCredits = a.AvailableCredits,
                                              Price = c.Price,
                                              ExpiryDate = c.ExpiryDate
                                          }).ToListAsync();

            return response;

            
        }
    }
}
