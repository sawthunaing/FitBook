using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;
using Stn.FitBook.Domain.IServices;
using Stn.FitBook.Domain.Models.Dtos.Request;
using Stn.FitBook.Domain.Models.Dtos.Response;
using System.Net;

namespace Stn.FitBook.Api.Controllers
{
    [ApiController]
    [Route("[controller]/v1/")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UsersController> _logger;
        private readonly IJwtService _jwtService;
        public UsersController(IUserService userService,IJwtService jwtService, ILogger<UsersController> logger)
        {
            _userService = userService;
            _logger = logger;
            _jwtService = jwtService;
        }

        [HttpPost("LoginAsync")]
        public async Task<AuthenticationResult> LoginAsync(Login reqModel)
        {
            _logger.LogInformation("{name} {message}", "LoginAsyncReqmodel", reqModel.ToJson());
            var response = await _jwtService.GenerateAuthTokenAsync(reqModel);
            _logger.LogInformation("{name} {message}", "LoginAsyncResponse", response.ToJson());
            return await Task.FromResult(response);
        }

        [HttpPost("RegisterAsync")]
        public async Task<HttpStatusCode> RegisterAsync(Register reqModel)
        {
            _logger.LogInformation("{name} {message}", "RegisterAsyncRequest", reqModel.ToJson());
            var response = await _userService.InsertUserAsync(reqModel);
            _logger.LogInformation("{name} {message}", "RegisterAsyncResponse", response.ToJson());
            return await Task.FromResult(response);
        }

        [HttpPost("ResetPasswordAsync")]
        public async Task<ResetPasswordResponse> ResetPasswordAsync(ResetPassword reqModel)
        {
            _logger.LogInformation("{name} {message}", "ResetPasswordAsyncRequest", reqModel.ToJson());
            var response = await _userService.ResetPasswordAsync(reqModel);
            _logger.LogInformation("{name} {message}", "ResetPasswordAsyncResponse", response.ToJson());
            return await Task.FromResult(response);
        }

        [HttpPost("VerifyEmailAsync")]
        public async Task<HttpStatusCode> VerifyEmailAsync(VerifyEmail reqModel)
        {
            _logger.LogInformation("{name} {message}", "VerifyEmailAsyncRequest", reqModel.ToJson());
            var response = await _userService.VerifyEmailAsync(reqModel);
            _logger.LogInformation("{name} {message}", "VerifyEmailAsyncResponse", response.ToJson());
            return await Task.FromResult(response);
        }


       
        [HttpGet("GetUserByIdAsync")]
       
        public async Task<UserProfileResponse> GetUserByIdAsync(int Id)
        {
            _logger.LogInformation("{name} {message}", "GetUserByIdRequest", null);
            var response = await _userService.GetUserByIdAsync(Id);
            _logger.LogInformation("{name} {message}", "GetUserByIdResponse", response.ToJson());
            return await Task.FromResult(response);
        }

    
        [HttpPut("UpdateUserAsync")]
        public async Task<HttpStatusCode> UpdateUserAsync(Update reqModel)
        {
            _logger.LogInformation("{name} {message}", "UpdateUserAsyncRequest", reqModel.ToJson());
            var response = await _userService.UpdateUserAsync(reqModel);
            _logger.LogInformation("{name} {message}", "UpdateUserAsyncResponse", response.ToJson());
            return await Task.FromResult(response);
        }

       
        [HttpPut("ChangePasswordAsync")]
        public async Task<HttpStatusCode> ChangePasswordAsync(ChangePassword reqModel)
        {
            _logger.LogInformation("{name} {message}", "ChangePasswordAsyncRequest", reqModel.ToJson());
            var response = await _userService.ChangePasswordAsync(reqModel);
            _logger.LogInformation("{name} {message}", "ChangePasswordAsyncResponse", response.ToJson());
            return await Task.FromResult(response);
        }
      
    }
}
