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
    public class ClassScheduleController : ControllerBase
    {
        private readonly IScheduleClassService _classScheduleService;
        private readonly ILogger<ClassScheduleController> _logger;
        
        public ClassScheduleController(IScheduleClassService scheduleClassService, ILogger<ClassScheduleController> logger)
        {
            _classScheduleService = scheduleClassService;
            _logger = logger;
        
        }

        
        [HttpPost("GetClassScheduleByCountryAsync")]

        public async Task<GetClassScheduleByCountryResponse> GetClassScheduleByCountryAsync(GetClassScheduleByCountry reqModel)
        {
            _logger.LogInformation("{name} {message}", "GetClassScheduleByCountryAsyncRequest", reqModel.ToJson);
            var response = await _classScheduleService.GetClassScheduleByCountryAsync(reqModel);
            _logger.LogInformation("{name} {message}", "GetClassScheduleByCountryAsyncResponse", response.ToJson());
            return await Task.FromResult(response);
        }

       
        [HttpPost("BookClasByScheduleIDAsync")]
        public async Task<BookClasByScheduleIDResponse> BookClasByScheduleIDAsync(BookClasByScheduleID reqModel)
        {
            _logger.LogInformation("{name} {message}", "BookClasByScheduleIDAsyncRequest", reqModel.ToJson);
            var response = await _classScheduleService.BookClasByScheduleIDAsync(reqModel);
            _logger.LogInformation("{name} {message}", "BookClasByScheduleIDAsyncResponse", response.ToJson());
            return await Task.FromResult(response);
        }

       
        [HttpDelete("CancelBookingAsync")]
        public async Task<HttpStatusCode> CancelBookingAsync(CancelBookingByBookingId reqModel)
        {
            _logger.LogInformation("{name} {message}", "CancelBookingAsyncRequest", reqModel.ToJson());
            var response = await _classScheduleService.CancelBookingAsync(reqModel);
            _logger.LogInformation("{name} {message}", "CancelBookingAsyncResponse", response.ToJson());
            return await Task.FromResult(response);
        }

        [HttpPost("AddToWaitlistAsync")]
        public async Task<AddToWaitListResponse> AddToWaitlistAsync(AddToWaitList reqModel)
        {
            _logger.LogInformation("{name} {message}", "AddToWaitlistAsyncRequest", reqModel.ToJson());
            var response = await _classScheduleService.AddToWaitlistAsync(reqModel);
            _logger.LogInformation("{name} {message}", "AddToWaitlistAsyncResponse", response.ToJson());
            return await Task.FromResult(response);
        }

        [HttpPost("CheckInAsync")]
        public async Task<HttpStatusCode> CheckInAsync(CheckIn reqModel)
        {
            _logger.LogInformation("{name} {message}", "CheckInAsyncRequest", reqModel.ToJson());
            var response = await _classScheduleService.CheckInAsync(reqModel);
            _logger.LogInformation("{name} {message}", "CheckInAsyncResponse", response.ToJson());
            return await Task.FromResult(response);
        }

    }
}
