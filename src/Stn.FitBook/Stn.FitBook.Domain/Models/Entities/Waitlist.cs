using System;
using System.Collections.Generic;

#nullable disable

namespace Stn.FitBook.Domain.Models.Entities
{
    public partial class Waitlist
    {
        public int WaitlistId { get; set; }
        public int? UserId { get; set; }
        public int? ScheduleId { get; set; }
        public DateTime? WaitlistTime { get; set; }
        public int RequiredCredit { get; set; }
        public int UserPackageId { get; set; }
        public virtual ClassSchedule Schedule { get; set; }
        public virtual User User { get; set; }
    }
}
