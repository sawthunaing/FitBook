using Microsoft.Extensions.Logging;
using Stn.FitBook.Domain.IRepositories;
using Stn.FitBook.Domain.IServices;
using Stn.FitBook.Domain.Models.Dtos.Request;
using Stn.FitBook.Domain.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stn.FitBook.Service.Services
{
    public class ValidatorServices : IValidator
    {
        private readonly ILogger<ValidatorServices> _logger;

        public ValidatorServices(ILogger<ValidatorServices> logger)
        {
            _logger = logger;
          
        }
        public void ValidLogin(Login request)
        {
            _logger.LogInformation("Login request : {request}", request);
            StringBuilder errorMessage = new StringBuilder();
            errorMessage.Append("Argument Missing Exception.");

            if (request is null)
            {
                errorMessage.AppendLine("Login Request must not be empty.");
                throw new ArgumentNullException(errorMessage.ToString());
            }
            if (string.IsNullOrEmpty(request.Email))
            {
                errorMessage.AppendLine("Username must not be empty.");
                throw new ArgumentNullException(nameof(request.Email), errorMessage.ToString());
            }
          
            if (string.IsNullOrEmpty(request.Password))
            {
                errorMessage.AppendLine("Password must not be empty.");
                throw new ArgumentNullException(nameof(request.Password), errorMessage.ToString());
            }
        }

        public void ValidInsertUserRequest(Register request)
        {
            _logger.LogInformation("User request : {request}", request);
            StringBuilder errorMessage = new StringBuilder();
            errorMessage.Append("Argument Missing Exception.");

            if (request is null)
            {
                errorMessage.AppendLine("User Request must not be empty.");
                throw new ArgumentNullException(errorMessage.ToString());
            }
            if (string.IsNullOrEmpty(request.FullName))
            {
                errorMessage.AppendLine("FullName must not be empty.");
                throw new ArgumentNullException(nameof(request.FullName), errorMessage.ToString());
            }
          
            if (string.IsNullOrEmpty(request.Email))
            {
                errorMessage.AppendLine("Username must not be empty.");
                throw new ArgumentNullException(nameof(request.Email), errorMessage.ToString());
            }
            if (string.IsNullOrEmpty(request.Password))
            {
                errorMessage.AppendLine("Password must not be empty.");
                throw new ArgumentNullException(nameof(request.Password), errorMessage.ToString());
            }
        }

        public void ValidUpdateUserRequest(Update request)
        {
            _logger.LogInformation("User request : {request}", request);
            StringBuilder errorMessage = new StringBuilder();
            errorMessage.Append("Argument Missing Exception.");

            if (request is null)
            {
                errorMessage.AppendLine("User Request must not be empty.");
                throw new ArgumentNullException(errorMessage.ToString());
            }
            if (request.UserId <= 0)
            {
                errorMessage.AppendLine("UserId must be greater zero.");
                throw new ArgumentNullException(nameof(request.UserId), errorMessage.ToString());
            }
            if (string.IsNullOrEmpty(request.FullName))
            {
                errorMessage.AppendLine("FullName must not be empty.");
                throw new ArgumentNullException(nameof(request.FullName), errorMessage.ToString());
            }

            if (string.IsNullOrEmpty(request.Email))
            {
                errorMessage.AppendLine("Username must not be empty.");
                throw new ArgumentNullException(nameof(request.Email), errorMessage.ToString());
            }
          
        }

        public void ValidPurchasePackagesRequest(PurchasePackage request)
        {
          
            StringBuilder errorMessage = new StringBuilder();
            errorMessage.Append("Argument Missing Exception.");

            if (request is null)
            {
                errorMessage.AppendLine("PurchasePackage Request must not be empty.");
                throw new ArgumentNullException(errorMessage.ToString());
            }
            if (request.UserId <= 0)
            {
                errorMessage.AppendLine("UserId must be greater zero.");
                throw new ArgumentNullException(nameof(request.UserId), errorMessage.ToString());
            }

            if (request.PackageId <= 0)
            {
                errorMessage.AppendLine("packageId must be greater zero.");
                throw new ArgumentNullException(nameof(request.PackageId), errorMessage.ToString());
            }
            if (request.Price <= 0)
            {
                errorMessage.AppendLine("price must be greater zero.");
                throw new ArgumentNullException(nameof(request.Price), errorMessage.ToString());
            }

            if (string.IsNullOrEmpty(request.CountryCode))
            {
                errorMessage.AppendLine("CountryCode must not be empty.");
                throw new ArgumentNullException(nameof(request.CountryCode), errorMessage.ToString());
            }
        }
    }
}
