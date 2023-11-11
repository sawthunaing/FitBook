using System;
using System.Collections.Generic;

#nullable disable

namespace Stn.FitBook.Domain.Models.Entities
{
    public partial class User
    {
        public User()
        {
            Bookings = new HashSet<Booking>();
            UserPackages = new HashSet<UserPackage>();
            Waitlists = new HashSet<Waitlist>();
        }

        public int UserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string CountryCode { get; set; }
        public string Password { get; set; }
        public bool? VerificationStatus { get; set; } = false;
        public DateTime? Created { get; set; }
        public string CreatedBy { get; set; }

        public virtual ICollection<Booking> Bookings { get; set; }
        public virtual ICollection<UserPackage> UserPackages { get; set; }
        public virtual ICollection<Waitlist> Waitlists { get; set; }
    }
}
