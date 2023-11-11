using Stn.FitBook.Domain.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stn.FitBook.Domain.Models.Dtos.Request
{
    public class Login
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class Register
    {
        public string Email { get; set; }
        public string FullName { get; set; }
        public string CountryCode { get; set; }
        public string Password { get; set; }
    }

    public class Update
    {
        public int UserId { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string CountryCode { get; set; }
        
    }

    public class ChangePassword
    {
        public int UserId { get; set; }
        public string Email { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }

    public class ResetPassword
    {
        public string Email { get; set; }
        public string OTP { get; set; }
        
    }

    public class VerifyEmail
    {
        public string Email { get; set; }
        public string Guid { get; set; }

    }


    public class PurchasePackage
    {
        public int UserId { get; set; }
        public int PackageId { get; set; }
        public decimal Price { get; set; }
        public string CountryCode { get; set; }

    }


    public class GetClassScheduleByCountry
    {
        public int UserId { get; set; }
        public string Country { get; set; }
    }

    public class BookClasByScheduleID
    {  

        public int UserId { get; set; }
        public int ScheduleId { get; set; }
        public string Country { get; set; }
        public int RequiredCredit { get; set; }
    }

    public class CancelBookingByBookingId
    {
        public int UserId { get; set; }
        public string BookingReference { get; set; }
    }

    public class AddToWaitList
    {
        public int userId { get; set; }
        public int ScheduleId { get; set; }
        public int UserPackageId { get; set; }
        public string Country { get; set; }
        public int RequiredCredit { get; set; }
    }


    public class CheckIn
    {
        public int UserId { get; set; }
        public int ScheduleId { get; set; }
        public string BookingReference { get; set; }
    }

}
