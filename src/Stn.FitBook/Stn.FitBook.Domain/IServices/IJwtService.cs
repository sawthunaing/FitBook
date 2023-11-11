using Stn.FitBook.Domain.Models.Dtos.Request;
using Stn.FitBook.Domain.Models.Dtos.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Stn.FitBook.Domain.IServices
{
    public interface IJwtService
    {
        public Task<AuthenticationResult> GenerateAuthTokenAsync(Login request);

        public Task<AuthenticationResult> RefreshTokenAsync(RefreshToken request);

        public Task<HttpStatusCode> RevokeRefreshTokenAsync(string userName);

    }
}
