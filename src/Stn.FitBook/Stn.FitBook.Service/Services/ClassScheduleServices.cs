using Stn.FitBook.Domain.IServices;
using Stn.FitBook.Domain.IRepositories;
using Stn.FitBook.Domain.Models.Entities;
using Microsoft.Extensions.Logging;
using Stn.FitBook.Domain.Models.Dtos.Request;
using Stn.FitBook.Domain.Models.Dtos.Response;
using System.Net;
using Stn.FitBook.Repo.Ef.Repositories;
using Stn.FitBook.Domain.Constants;
using System.Text;

namespace Stn.FitBook.Service.Services
{
    public class ClassScheduleServices : IScheduleClassService
    {
        private readonly ILogger<ClassScheduleServices> _logger;
        private readonly IQueryRepository<ClassSchedule> _classScheduleQueryRepo;
        private readonly ICommandRepository<ClassSchedule> _classScheduleCommandRepo;
        private readonly IQueryRepository<Booking> _bookingQueryRepo;
        private readonly ICommandRepository<Booking> _bookingCommandRepo;
        private readonly IQueryRepository<UserPackage> _userPackageQueryRepo;
        private readonly ICommandRepository<UserPackage> _userPackageCommandRepo;
        private readonly ICommandRepository<Waitlist> _waitlistCommandRepo;
        private readonly IQueryRepository<Waitlist> _waitlistQueryRepo;
        private readonly ICustomRepository _customRepository;
        private readonly IUnitOfWork _uow;
        private readonly IValidator _validator;
        private readonly ICachesService _cachesService;



        public ClassScheduleServices(ILogger<ClassScheduleServices> logger
            , IQueryRepository<ClassSchedule> classScheduleQueryRepo
            , ICommandRepository<ClassSchedule> classScheduleCommandRepo
            , IQueryRepository<Booking> bookinQueryRepo
            , ICommandRepository<Booking> bookingCommandRepo
            , ICustomRepository customRepository
            , IQueryRepository<UserPackage> userPackageQueryRepo
            , ICommandRepository<UserPackage> userPackageCommandRepo
              , IQueryRepository<Waitlist> waitlistQueryRepo
            , ICommandRepository<Waitlist> waitlistCommandRepo
            , IUnitOfWork uow
            , IValidator validator
            , ICachesService cachesService

        )
        {
            _logger = logger;
            _classScheduleQueryRepo = classScheduleQueryRepo;
            _classScheduleCommandRepo = classScheduleCommandRepo;
            _bookingQueryRepo = bookinQueryRepo;
            _bookingCommandRepo = bookingCommandRepo;
            _customRepository = customRepository;
            _userPackageQueryRepo = userPackageQueryRepo;
            _userPackageCommandRepo = userPackageCommandRepo;
            _waitlistQueryRepo = waitlistQueryRepo;
            _waitlistCommandRepo = waitlistCommandRepo;
            _uow = uow;
            _validator = validator;
            _cachesService = cachesService;
        }

        public async Task<GetClassScheduleByCountryResponse> GetClassScheduleByCountryAsync(GetClassScheduleByCountry reqModel)
        {
            var lstClassSchedule = await _classScheduleQueryRepo.GetAllAsync(x => x.Country == reqModel.Country &&  x.ClassStartTime > DateTime.Now);
            var resp = new GetClassScheduleByCountryResponse();
            resp.lstClassSchedule = lstClassSchedule;


            return resp;
        }

        public async Task<BookClasByScheduleIDResponse> BookClasByScheduleIDAsync(BookClasByScheduleID reqModel)
        {
            var resp = new BookClasByScheduleIDResponse();
            var classScheduleInfo = new ClassSchedule();

            //Validate ReqModel
            var cachesKey = CacheKey.Key + reqModel.ScheduleId.ToString();
            var avialbeSlots = await _cachesService.GetClassScheduleSlots(cachesKey);
            if (!String.IsNullOrEmpty(avialbeSlots))
            {
                if (int.Parse(avialbeSlots) < 1)
                {
                    int slots = int.Parse(avialbeSlots) - 1;
                    await _cachesService.UpdateClassScheduleSlots(cachesKey, slots.ToString());
                    
                }
                else
                {
                    //Full book the class
                    resp.ResponseCode = "01";
                    resp.ResponseMessage = "This Class is full Booked.";
                }
                return resp;
            }
            else
            {


                //check credit and debit credit
                classScheduleInfo = await _classScheduleQueryRepo.GetByIdAsync(x => x.ScheduleId == reqModel.ScheduleId && x.Country == reqModel.Country && x.ClassStartTime > DateTime.UtcNow);
                if (classScheduleInfo == null)
                {
                    resp.ResponseCode = "01";
                    resp.ResponseMessage = "Invalid classScheduleInfo.";
                    return resp;
                }

                var vailableSlots = classScheduleInfo.AvailableSlots - 1;
                await _cachesService.SetClassScheduleSlots(CacheKey.Key + classScheduleInfo.ScheduleId.ToString(), vailableSlots.ToString());


            }

            var userPackage = await _userPackageQueryRepo.GetByIdAsync(x => x.UserId == reqModel.UserId && x.PackageId == classScheduleInfo.PackageId);
                if (userPackage == null || userPackage.AvailableCredits < 1)
                {
                    //invalid userpackage
                    resp.ResponseCode = "01";
                    resp.ResponseMessage = "Invalid UserPackage.";
                    return resp;
                }
        
                if(reqModel.RequiredCredit > userPackage.AvailableCredits)
                {
                    resp.ResponseCode = "08";
                    resp.ResponseMessage = "Insufficient Credit.";
                    return resp;
                }

                userPackage.AvailableCredits = userPackage.AvailableCredits - reqModel.RequiredCredit;

          

                //insert booking table
                var bookingInfo = new Booking();
                bookingInfo.BookingReference = Guid.NewGuid().ToString();
                bookingInfo.ScheduleId = reqModel.ScheduleId;
                bookingInfo.UserId = reqModel.UserId;
                bookingInfo.CheckInStatus = false;
                bookingInfo.Status = 1;
                bookingInfo.BookingTime = DateTime.UtcNow;

                classScheduleInfo.AvailableSlots -= 1;

                await _userPackageCommandRepo.UpdateAsync(userPackage);
                await _bookingCommandRepo.CreateAsync(bookingInfo);
                await _classScheduleCommandRepo.UpdateAsync(classScheduleInfo);
                await _uow.CommitAsync();

                resp.BookingReference = bookingInfo.BookingReference;

                return resp;
            
        }
        public async Task<HttpStatusCode> CancelBookingAsync(CancelBookingByBookingId reqModel)
        {
            //Validate ReqModel

            var bookingInfo =await _bookingQueryRepo.GetByIdAsync(x => x.BookingReference == reqModel.BookingReference);
            var classScheduleInfo = await _classScheduleQueryRepo.GetByIdAsync(x => x.ScheduleId == bookingInfo.ScheduleId);
            var userPackage = await _userPackageQueryRepo.GetByIdAsync(x => x.UserId == reqModel.UserId && x.PackageId == classScheduleInfo.PackageId);

            // remove booking table 

            bookingInfo.Status = (int)BookingStatus.Cancel;

            await _bookingCommandRepo.UpdateAsync(bookingInfo);
            await _uow.CommitAsync();
            // check within or not (class start time + 4 hr) if no refund  else refund
            var refundValidTime = DateTime.UtcNow.AddHours(4);
            if(classScheduleInfo.ClassStartTime > refundValidTime)
            {
                //refund
                userPackage.AvailableCredits += classScheduleInfo.RequiredCredits;
            }


            // select one from waiting list order by creadedby 
            var lstWaitlistInfo = await _waitlistQueryRepo.GetAllAsync(x => x.ScheduleId == classScheduleInfo.ScheduleId);
            if(!lstWaitlistInfo.Any())
            {
                return HttpStatusCode.OK;
            }
            var firstWaitListInfo = lstWaitlistInfo.OrderByDescending(x => x.WaitlistId ).FirstOrDefault();
            var newbookingInfo = new Booking();
            newbookingInfo.BookingReference = Guid.NewGuid().ToString();
            newbookingInfo.ScheduleId = firstWaitListInfo.ScheduleId;
            newbookingInfo.UserId = firstWaitListInfo.UserId;
            newbookingInfo.CheckInStatus = false;
            newbookingInfo.Status = (int)BookingStatus.Pending;
            newbookingInfo.BookingTime = DateTime.UtcNow;

            await _bookingCommandRepo.CreateAsync(newbookingInfo);
         
            await _uow.CommitAsync();
            // send email for to join attend class

            ExternalServices.SendEmail("", "Booking process is success");

            return HttpStatusCode.OK;
        }
        public async Task<AddToWaitListResponse> AddToWaitlistAsync(AddToWaitList reqModel)
        {
            var resp = new AddToWaitListResponse();

            //valid req

            //check credit and debit credit
            var classScheduleInfo = await _classScheduleQueryRepo.GetByIdAsync(x => x.ScheduleId == reqModel.ScheduleId && x.Country == reqModel.Country && x.ClassStartTime > DateTime.UtcNow);
            if (classScheduleInfo == null)
            {
                resp.ResponseCode = "01";
                resp.ResponseMessage = "Invalid UserPackage.";
                return resp;
            }

            var userPackage = await _userPackageQueryRepo.GetByIdAsync(x => x.UserId == reqModel.userId && x.PackageId == classScheduleInfo.PackageId);
            if (userPackage == null || userPackage.AvailableCredits < 1)
            {
                resp.ResponseCode = "01";
                resp.ResponseMessage = "Invalid UserPackage.";
                return resp;
            }

            if (reqModel.RequiredCredit > userPackage.AvailableCredits)
            {
                resp.ResponseCode = "08";
                resp.ResponseMessage = "Insufficient Credit.";
                return resp;
            }

            userPackage.AvailableCredits = userPackage.AvailableCredits - reqModel.RequiredCredit;

            //Insert WaitList
            var waitListInfo = new Waitlist();
            waitListInfo.WaitlistTime = DateTime.UtcNow;
            waitListInfo.ScheduleId = reqModel.ScheduleId;
            waitListInfo.UserId = reqModel.userId;
            waitListInfo.RequiredCredit = reqModel.RequiredCredit;
            waitListInfo.UserPackageId = reqModel.UserPackageId;

            await _userPackageCommandRepo.UpdateAsync(userPackage);
            await _waitlistCommandRepo.CreateAsync(waitListInfo);
            await _uow.CommitAsync();

            return resp;

        }
        public async Task<HttpStatusCode> CheckInAsync(CheckIn reqModel)
        {
            //valid req
            StringBuilder errorMessage = new StringBuilder();
            //check booking is avaiable or not (starttime !> datetimeNow)
            var classScheduleInfo = await _classScheduleQueryRepo.GetByIdAsync(x => x.ScheduleId == reqModel.ScheduleId && x.ClassStartTime > DateTime.UtcNow);
            if (classScheduleInfo == null)
            {
                errorMessage.AppendLine("Invalid Class.");
                throw new ArgumentNullException(errorMessage.ToString());
            }

            var bookingInfo = await _bookingQueryRepo.GetByIdAsync(x => x.ScheduleId == reqModel.ScheduleId && x.BookingReference == reqModel.BookingReference && x.Status == (int)BookingStatus.Pending);
            if (bookingInfo == null)
            {
                errorMessage.AppendLine("Invalid Class.");
                throw new ArgumentNullException(errorMessage.ToString());
            }
            bookingInfo.CheckInStatus = true;

            await _bookingCommandRepo.UpdateAsync(bookingInfo);
         
            await _uow.CommitAsync();
            return HttpStatusCode.OK;
        }




    }
}
