using Stn.FitBook.Domain.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stn.FitBook.Domain.Models.Dtos.Response
{
   

    public class AuthenticationResult : BaseResponse
    {
        public string Email { get; set; }
        public string Token { get; set; }
        public string RefereshToken { get; set; }
        public DateTime Expiration { get; set; }
    }


    public class RegisterResponse : BaseResponse
    {
        public int UserId { get; set; }
      
    }

    public class UserProfileResponse : BaseResponse
    {
     
        public User userInfo { get; set; }

    }


    public class ResetPasswordResponse : BaseResponse
    {
        public string Email { get; set; }
        public string NewPassword { get; set; }
    

    }
       

    public class PackageResponse : BaseResponse
    {
       public List<Package> lstPackages { get; set; }
    }

    public class PackegeByUserIdResponse : BaseResponse
    {
        public List<PackageDto> lstPackages { get; set; }
    }

    public class PackageDto
    {
        public int PackageID { get; set; }
        public string PackageName { get; set; }
        public string Country { get; set; }
        public int? AvailableCredits { get; set; }
        public decimal? Price { get; set; }
        public DateTime? ExpiryDate { get; set; }
    }

    public class PurchasePackageResponse : BaseResponse
    {
        public int PackageID { get; set; }
        public string PackageName { get; set; }
        
    }

    public class GetClassScheduleByCountryResponse : BaseResponse
    {
       public List<ClassSchedule> lstClassSchedule { get; set; }
    }

    public class BookClasByScheduleIDResponse : BaseResponse
    {
        public string BookingReference { get; set; }
    }

    public class AddToWaitListResponse : BaseResponse
    {
        public int WaitingId { get; set; }
    }







}
