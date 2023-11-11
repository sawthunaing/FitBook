using System;
using System.Collections.Generic;

#nullable disable

namespace Stn.FitBook.Domain.Models.Entities
{
    public partial class ClassSchedule
    {
        public ClassSchedule()
        {
            Bookings = new HashSet<Booking>();
            Waitlists = new HashSet<Waitlist>();
        }

        public int ScheduleId { get; set; }
        public string ClassName { get; set; }
        public string Country { get; set; }
        public int PackageId { get; set; }
        public int? RequiredCredits { get; set; }
        public DateTime? ClassStartTime { get; set; }
        public DateTime? ClassEndTime { get; set; }
        public int? AvailableSlots { get; set; }
        public int Status { get; set; }
        public DateTime? Created { get; set; }
        public string CreatedBy { get; set; }

        public virtual ICollection<Booking> Bookings { get; set; }
        public virtual ICollection<Waitlist> Waitlists { get; set; }
    }
}
