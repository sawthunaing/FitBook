using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Stn.FitBook.Domain.IServices;
using Stn.FitBook.Domain.IRepositories;
using Stn.FitBook.Domain.Models.Entities;
using Microsoft.Extensions.Logging;
using Stn.FitBook.Domain.Models.Dtos.Request;
using Stn.FitBook.Domain.Models.Dtos.Response;
using Ubiety.Dns.Core;

namespace Stn.FitBook.Service.Services
{
    public class UserServices : IUserService
    {
        private readonly ILogger<UserServices> _logger;
        private readonly IQueryRepository<User> _userQueryRepo;
        private readonly ICommandRepository<User> _userCommandRepo;
        private readonly IUnitOfWork _uow;
        private readonly IValidator _validator;


        public UserServices(ILogger<UserServices> logger
            , IQueryRepository<User> userQueryRepo
            , ICommandRepository<User> userCommandRepo
            , IUnitOfWork uow
            , IValidator validator

        )
        {
            _logger = logger;
            _userQueryRepo = userQueryRepo;
            _userCommandRepo = userCommandRepo;
            _uow = uow;
            _validator = validator;
        }
      
        public async Task<UserProfileResponse> GetUserByIdAsync(int Id)
        {
            var userResp = await _userQueryRepo.GetByIdAsync(x => x.UserId == Id);
            var resp = new UserProfileResponse();
            resp.userInfo = userResp;


            return resp;
        }
        public async Task<HttpStatusCode> InsertUserAsync(Register reqModel)
        {
            _validator.ValidInsertUserRequest(reqModel);

            var userInfo = new User();
            userInfo.FullName = reqModel.FullName;
            userInfo.Email = reqModel.Email;
            userInfo.CountryCode = reqModel.CountryCode;
            userInfo.Password = reqModel.Password;
            userInfo.Created = DateTime.UtcNow;
            userInfo.CreatedBy = reqModel.FullName;

            await _userCommandRepo.CreateAsync(userInfo);
            await _uow.CommitAsync();
            return HttpStatusCode.Created;
        }
        public async Task<HttpStatusCode> UpdateUserAsync(Update reqModel)
        {
            StringBuilder errorMessage = new StringBuilder();
            _validator.ValidUpdateUserRequest(reqModel);
            var userInfo = await _userQueryRepo.GetByIdAsync(x => x.UserId == reqModel.UserId);
            if (userInfo == null)
            {
                //Validate Password
                errorMessage.AppendLine("Invalid UserID.");
                throw new ArgumentNullException(errorMessage.ToString());
            }
            
            userInfo.FullName = reqModel.FullName;
            userInfo.Email = reqModel.Email;
            userInfo.CountryCode = reqModel.CountryCode;
            userInfo.UserId = reqModel.UserId;

            await _userCommandRepo.UpdateAsync(userInfo);
            await _uow.CommitAsync();
            return HttpStatusCode.OK;
        }

        public async Task<HttpStatusCode> ChangePasswordAsync(ChangePassword changePwdReq)
        {
            StringBuilder errorMessage = new StringBuilder();
            var userResp = await _userQueryRepo.GetByIdAsync(x => x.UserId == changePwdReq.UserId);
            if(userResp.Password != changePwdReq.OldPassword)
            {
                errorMessage.AppendLine("Password is invalid.");
                throw new ArgumentNullException(errorMessage.ToString());
            }
            userResp.Password = changePwdReq.NewPassword;
            await _userCommandRepo.UpdateAsync(userResp);
            await _uow.CommitAsync();
            return HttpStatusCode.OK;
        }

        public async Task<ResetPasswordResponse> ResetPasswordAsync(ResetPassword resetPwd)
        {
            var resp = new ResetPasswordResponse();
            //Generate new pwd
            var userInfo = await _userQueryRepo.GetByIdAsync(x => x.Email == resetPwd.Email);
            resp.NewPassword =  ExternalServices.GeneratePassword();
            resp.Email = resetPwd.Email;

            userInfo.Password = ExternalServices.ComputeSha256Hash(resp.NewPassword);

            await _userCommandRepo.UpdateAsync(userInfo);
            await _uow.CommitAsync();
            return resp;
        }

        public async Task<HttpStatusCode> VerifyEmailAsync(VerifyEmail reqModel)
        {
            var userInfo = await _userQueryRepo.GetByIdAsync(x => x.Email == reqModel.Email);
            userInfo.VerificationStatus = true;
            await _userCommandRepo.UpdateAsync(userInfo);
            await _uow.CommitAsync();
            return HttpStatusCode.OK;
        }



    }
}
