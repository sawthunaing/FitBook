using Hangfire;
using Microsoft.AspNetCore.Mvc;
using Stn.FitBook.Domain.IServices;

namespace Stn.FitBook.Api.Controllers
{
    [ApiController]
    [Route("[controller]/v1/")]
    public class SchedulerForRefundController : ControllerBase
    {
        private readonly ISchedulerForRefundService _schedulerForRefund;
        private readonly ILogger<SchedulerForRefundController> _logger;
        private readonly IBackgroundJobClient _backgroundJobClient;
        private readonly IRecurringJobManager _recurringJobManager;
        public SchedulerForRefundController(ISchedulerForRefundService schedulerForRefund, IBackgroundJobClient backgroundJobClient, IRecurringJobManager recurringJobManager, ILogger<SchedulerForRefundController> logger)
        {
            _schedulerForRefund = schedulerForRefund;
            _logger = logger;
            _backgroundJobClient = backgroundJobClient;
            _recurringJobManager = recurringJobManager;
        }

        [HttpGet("CheckRefundAfterClassJob")]
        public ActionResult CheckRefundAfterClassJob()
        {
            _recurringJobManager.AddOrUpdate("jobId", () => _schedulerForRefund.CheckRefundAfterClassJob(), Cron.Minutely);
            return Ok();
        }

    }

   
}
