using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Stn.FitBook.Domain.Models.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Stn.FitBook.Domain.Models.Dtos.Request;
using Stn.FitBook.Domain.Models.Dtos.Response;
using Stn.FitBook.Domain.IServices;
using Microsoft.Extensions.Logging;
using Stn.FitBook.Domain.IRepositories;
using Microsoft.AspNetCore.DataProtection;
using System.Collections.Concurrent;
using Ubiety.Dns.Core;
using System.Net;
using System.ComponentModel.DataAnnotations;
using RefreshToken = Stn.FitBook.Domain.Models.Dtos.Request.RefreshToken;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Stn.FitBook.Service.Services
{
    public class JwtServices  : IJwtService
    {
        private const string Secret = "this is my custom Secret key for authentication";

        static readonly ConcurrentDictionary<string, Guid> _refreshToken = new ConcurrentDictionary<string, Guid>();

        private readonly ILogger<JwtServices> _logger;
        private readonly IQueryRepository<User> _userQueryRepo;
        private readonly ICommandRepository<User> _userCommandRepo;
        private readonly IUnitOfWork _uow;
        private readonly IConfiguration _configuration;
        private readonly IValidator _validtor;


        public JwtServices(ILogger<JwtServices> logger
            , IQueryRepository<User> userQueryRepo
            , ICommandRepository<User> userCommandRepo
            , IUnitOfWork uow
            , IConfiguration configuration
            , IValidator validator

        )
        {
            _logger = logger;
            _userQueryRepo = userQueryRepo;
            _userCommandRepo = userCommandRepo;
            _uow = uow;
            _configuration = configuration;
            _validtor = validator;
        }
        public async Task<AuthenticationResult> GenerateAuthTokenAsync(Login loginModel)
        {
            var response = new AuthenticationResult();
            _validtor.ValidLogin(loginModel);
            var userInfo = await _userQueryRepo.GetByIdAsync(x => x.Email == loginModel.Email && x.Password == loginModel.Password );
            if (userInfo is null)
            {
                throw new ArgumentException($"User Credentials is not valid.");
            }
            if (userInfo.VerificationStatus is false)
            {
                throw new ArgumentException($"User Account is not verified.");
            }
           

            return CreateAuthResult(userInfo.Email);

        }

        public async Task<AuthenticationResult> RefreshTokenAsync(RefreshToken refresgReq)
        {
            var response = new AuthenticationResult();
            if(!IsValid(refresgReq,out string validUserName))
            {
                return (new AuthenticationResult
                {
                    ResponseCode = "419",
                    ResponseMessage = "User Not Found"
                });
            }

            return CreateAuthResult(validUserName);

        }

        public async Task<HttpStatusCode> RevokeRefreshTokenAsync(string userName)
        {
            if (_refreshToken.TryRemove(userName, out _))
            {
                return HttpStatusCode.OK;
            }
            return HttpStatusCode.NotFound;
        }
        private AuthenticationResult CreateAuthResult(string email)
        {
            var response = new AuthenticationResult();
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
            var signingCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var expirationTimeStamp = DateTime.Now.AddMinutes(double.Parse(_configuration["JWT:ExpireAddMinutes"]));

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, email),
                new Claim("role", email),
                new Claim("scope", "user")

            };

            var tokenOptions = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                claims: claims,
                expires: expirationTimeStamp,
                signingCredentials: signingCredentials
            );

            response.ResponseCode = "000";
            response.ResponseMessage = "Success";
            response.Email = email;
            response.Expiration = expirationTimeStamp;
            response.Token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
            response.RefereshToken = GenerateRefreshToken(email);

            return response;
        }

        private bool IsValid(RefreshToken authResult, out string validUserName)
        {
            validUserName = string.Empty;

            ClaimsPrincipal principal = GetPrincipalFromExpiredToken(authResult.Token);
            if (principal is null)
            {
                return false;
            }

            validUserName = principal.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(validUserName))
            {
                return false;
            }

            if (!Guid.TryParse(authResult.RefereshToken, out Guid givenRefreshToken))
            {
                return false;
            }

            if (!_refreshToken.TryGetValue(validUserName, out Guid currentRefreshToken))
            {
                return false;
            }

            if (currentRefreshToken != givenRefreshToken)
            {
                return false;
            }

            return true;
        }

        private ClaimsPrincipal GetPrincipalFromExpiredToken(string accessToken)
        {
            TokenValidationParameters tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"])),
                ValidateLifetime = false,
            };

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            ClaimsPrincipal principal = tokenHandler.ValidateToken(accessToken, tokenValidationParameters, out SecurityToken securityToken);
            if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token");
            }

            return principal;
        }

        private string GenerateRefreshToken(string userName)
        {
            Guid newRefreshToken = _refreshToken.AddOrUpdate(userName, (u) => Guid.NewGuid(), (k, old) => Guid.NewGuid());
            return newRefreshToken.ToString("D");
        }
    }
     
}
