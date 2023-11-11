using Stn.FitBook.Domain.Models.Dtos.Request;
using Stn.FitBook.Domain.Models.Dtos.Response;
using Stn.FitBook.Domain.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stn.FitBook.Domain.IServices
{
    public interface IValidator
    {
        public void ValidLogin(Login request);
        public void ValidInsertUserRequest(Register request);
        public void ValidUpdateUserRequest(Update request);
        public void ValidPurchasePackagesRequest(PurchasePackage request);
    }
}
