using Stn.FitBook.Domain.Models.Dtos.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stn.FitBook.Domain.IRepositories
{
   

    public interface ICustomRepository 
    {
        Task<PackegeByUserIdResponse> GetPackagesByUserIdAsync(int userId);
       
    }
}
