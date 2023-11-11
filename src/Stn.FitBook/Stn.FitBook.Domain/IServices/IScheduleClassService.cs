using Stn.FitBook.Domain.Models.Dtos.Request;
using Stn.FitBook.Domain.Models.Dtos.Response;
using System.Net;

namespace Stn.FitBook.Domain.IServices
{
    public interface IScheduleClassService
    {
        public Task<GetClassScheduleByCountryResponse> GetClassScheduleByCountryAsync(GetClassScheduleByCountry getClassScheduleByCountryReq);
        public Task<BookClasByScheduleIDResponse> BookClasByScheduleIDAsync(BookClasByScheduleID BookClasByScheduleIDReq);
        public Task<HttpStatusCode> CancelBookingAsync(CancelBookingByBookingId cancelBookingReq);
        public Task<AddToWaitListResponse> AddToWaitlistAsync(AddToWaitList addToWaitListReq);
        public Task<HttpStatusCode> CheckInAsync(CheckIn checkInReq);

    }
}
