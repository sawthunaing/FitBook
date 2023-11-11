using System;
using System.Collections.Generic;

#nullable disable

namespace Stn.FitBook.Domain.Models.Entities
{
    public partial class Booking
    {
        public int BookingId { get; set; }
        public int? UserId { get; set; }
        public int? ScheduleId { get; set; }
        public string BookingReference { get; set; }
        public DateTime? BookingTime { get; set; }
        public bool? CheckInStatus { get; set; }
        public int Status { get; set; }

        public virtual ClassSchedule Schedule { get; set; }
        public virtual User User { get; set; }
    }
}
