using Stn.FitBook.Domain.IServices;
using Stn.FitBook.Domain.IRepositories;
using Stn.FitBook.Domain.Models.Entities;
using Microsoft.Extensions.Logging;
using Stn.FitBook.Domain.Models.Dtos.Request;
using Stn.FitBook.Domain.Constants;

namespace Stn.FitBook.Service.Services
{
    public class SchedulerForRefundServices : ISchedulerForRefundService
    {
        private readonly ILogger<SchedulerForRefundServices> _logger;
        private readonly IQueryRepository<ClassSchedule> _classScheduleQueryRepo;
        private readonly ICommandRepository<ClassSchedule> _classScheduleCommandRepo;
        private readonly ICustomRepository _customRepository;
        private readonly ICommandRepository<Waitlist> _waitlistCommandRepo;
        private readonly IQueryRepository<Waitlist> _waitlistQueryRepo;
        private readonly IQueryRepository<UserPackage> _userPackageQueryRepo;
        private readonly ICommandRepository<UserPackage> _userPackageCommandRepo;
        private readonly IUnitOfWork _uow;
        private readonly IValidator _validator;


        public SchedulerForRefundServices(ILogger<SchedulerForRefundServices> logger
            , IQueryRepository<ClassSchedule> classScheduleQueryRepo
            , ICommandRepository<ClassSchedule> classScheduleCommandRepo
            , IQueryRepository<Waitlist> waitlistQueryRepo
            , ICommandRepository<Waitlist> waitlistCommandRepo
            , IQueryRepository<UserPackage> userPackageQueryRepo
            , ICommandRepository<UserPackage> userPackageCommandRepo
            , ICustomRepository customRepository
            , IUnitOfWork uow
            , IValidator validator

        )
        {
            _logger = logger;
            _classScheduleQueryRepo = classScheduleQueryRepo;
            _classScheduleCommandRepo = classScheduleCommandRepo;
            _waitlistQueryRepo = waitlistQueryRepo;
            _waitlistCommandRepo = waitlistCommandRepo;
            _userPackageQueryRepo = userPackageQueryRepo;
            _userPackageCommandRepo = userPackageCommandRepo;
            _customRepository = customRepository;
            _uow = uow;
            _validator = validator;
        }
             
        public async Task CheckRefundAfterClassJob()
        {
            //check all class end and check refund status 
            var lstClassSchedule = await _classScheduleQueryRepo.GetAllAsync(x => x.ClassEndTime < DateTime.UtcNow && x.Status == (int)ClassScheudleStatus.Pending);

            foreach (var classSchedule in lstClassSchedule)
            {
                var refundWaitList = await _waitlistQueryRepo.GetAllAsync(x => x.ScheduleId == classSchedule.ScheduleId);
                classSchedule.Status = (int)ClassScheudleStatus.Success;
                await _classScheduleCommandRepo.UpdateAsync(classSchedule);
            }
           
            Console.WriteLine("Hello from a Scheduled job!");
         //   throw new NotImplementedException();
        }

        public async Task refund( List<Waitlist> waitListInfo )
        {
            foreach (var item in waitListInfo)
            {
                var userPackageInfo = await _userPackageQueryRepo.GetByIdAsync(x => x.UserPackageId == item.UserPackageId);
                userPackageInfo.AvailableCredits += item.RequiredCredit;
                await _userPackageCommandRepo.UpdateAsync(userPackageInfo);
            }
         
        }
    }
}
