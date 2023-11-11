using Stn.FitBook.Domain.Models.Dtos;
using Stn.FitBook.Domain.Models.Dtos.Request;
using Stn.FitBook.Domain.Models.Dtos.Response;
using Stn.FitBook.Domain.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Stn.FitBook.Domain.IServices
{
    public interface IUserService
    {
       
        public Task<UserProfileResponse> GetUserByIdAsync(int Id);
        public Task<HttpStatusCode> VerifyEmailAsync(VerifyEmail registerReq);
        public Task<HttpStatusCode> InsertUserAsync(Register registerReq);
        public Task<HttpStatusCode> UpdateUserAsync(Update registerReq);
        public Task<HttpStatusCode> ChangePasswordAsync(ChangePassword changePwdDto);
        public Task<ResetPasswordResponse> ResetPasswordAsync(ResetPassword resetPwdDto);


    }

  
}
